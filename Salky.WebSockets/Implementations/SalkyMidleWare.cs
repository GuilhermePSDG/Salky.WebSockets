﻿// configure
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Salky.WebSockets.Contracts;
using Salky.WebSockets.Enums;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Authentication;
public class SalkyMidleWare
{
    public SalkyMidleWare(
        RequestDelegate next,
        ILogger<SalkyMidleWare> logger
        )
    {
        this._next = next;
        this.logger = logger;
        logger.LogInformation("SalkyMidleWare started");
    }

    private readonly RequestDelegate _next;
    private readonly ILogger<SalkyMidleWare> logger;

    public async Task InvokeAsync(HttpContext http,
        ILogger<SalkyMidleWare> logger,
        IMessageHandler MessageHandler,
        IEnumerable<IConnectionEventHandler> connectionEventHandler,
        IConnectionAuthGuard connectionAuth,
        ISalkyWebSocketFactory webSocketFactory
        )
    {
        if (!http.WebSockets.IsWebSocketRequest)
        {
            await this._next(http);
            return;
        }
        ISalkyWebSocket? ws = null;
        try
        {
            logger.LogInformation("Connection received");
            var usr = await connectionAuth.AuthenticateConnection(http);
            if (usr == null)
            {
                logger.LogInformation("Connection not autorized");
                return;
            }
            ws = await webSocketFactory.CreateNewAsync(http.WebSockets, usr);
            logger.LogInformation("Connection autorized");
            connectionEventHandler.ToList().ForEach(async x => await x.HandleOpen(ws));
            await LoopWaitingForMessage(ws, MessageHandler);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Connection Error");
        }
        finally
        {
            if (ws != null)
            {
                logger.LogInformation("Close handler called");
                try
                {
                    connectionEventHandler.ToList().ForEach(async x => await x.HandleClose(ws));
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Error on handle close message");
                }
            }
        }
    }
    private async Task LoopWaitingForMessage(ISalkyWebSocket ws
        , IMessageHandler MessageHandler)
    {
        while (ws.State == WebSocketState.Open)
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[512]);
            WebSocketReceiveResult? result = null;
            using var ms = new MemoryStream();
            bool canContinue = true;
            do
            {
                try
                {
                    result = await ws.ReceiveAsync(buffer, CancellationToken.None);
                }
                catch
                {
                    if (ws.State == WebSocketState.Aborted)
                    {
                        canContinue = false;
                        break;
                    }
                    else
                    {
                        throw;
                    }
                }

                ms.Write(buffer.Array ?? throw new NullReferenceException(), buffer.Offset, result.Count);
            } while (!result.EndOfMessage);
            if (!canContinue) break;
            ms.Seek(0, SeekOrigin.Begin);
            try
            {
                await MessageRouter(ws, result?.MessageType ?? throw new NullReferenceException(nameof(result)), ms, MessageHandler);

            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "");
            }
        }
    }
    private async Task MessageRouter(ISalkyWebSocket ws, WebSocketMessageType msgType, MemoryStream stream, IMessageHandler MessageHandler)
    {

        switch (msgType)
        {
            case WebSocketMessageType.Text:
                this.logger.LogInformation("Message text received");
                await MessageHandler.HandleText(ws, stream);
                break;
            case WebSocketMessageType.Binary:
                this.logger.LogInformation("Message Binary received");
                await MessageHandler.HandleBinary(ws, stream);
                break;
            case WebSocketMessageType.Close:
                this.logger.LogInformation($"{ws.CloseStatusDescription},{ws.CloseStatus}");
                Enum.TryParse(typeof(CloseDescription), ws.CloseStatusDescription, true, out var result);
                CloseDescription closeDescription = result is CloseDescription description ? description : CloseDescription.Unknow;
                await ws.CloseOutputAsync(ws.CloseStatus ?? WebSocketCloseStatus.NormalClosure, closeDescription);
                this.logger.LogInformation("Message close executed");
                break;
        }
    }



}
