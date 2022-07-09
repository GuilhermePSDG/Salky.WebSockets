

using System.Net.WebSockets;
using System.Text;
using System.Text.Json;



var TempTasks = new List<Task>();


var getInput = (string text) =>
{
    Console.Write(text);
    return Console.ReadLine();
};

ClientCli.instance.AddCommand("add-con", new("To add connections", async () =>
{
    if(!int.TryParse(getInput("The number of connections : "), out var n)) n = 0;
    for (int i = 0; i < n; i++)
        ClientCli.instance.clients.Add(await ConnectionFactory());
}));
ClientCli.instance.AddCommand("send", new("Send message", async () =>
{
    var res = getInput("Write your message : ") ?? "Default message";
    foreach (var con in ClientCli.instance.clients)
        TempTasks.Add(con.SendAsync(Encoding.UTF8.GetBytes(res), WebSocketMessageType.Text, true, CancellationToken.None));
    await Task.WhenAll(TempTasks);
    TempTasks.Clear();
}));
ClientCli.instance.AddCommand("send-bin", new Command("Send binary message", async () =>
{
    var res = getInput("Write your message : ") ?? "Default message";
    foreach (var x in ClientCli.instance.clients)
        TempTasks.Add(x.SendAsync(Encoding.UTF8.GetBytes(res), WebSocketMessageType.Binary, true, CancellationToken.None));
    await Task.WhenAll(TempTasks);
    TempTasks.Clear();
}));
ClientCli.instance.AddCommand("teste-route", new Command("Send MessageServer Protocol", async () =>
{
    var cli = new ClientCli();
    cli.AddCommand("ping", new Command("", async () =>
    {
        foreach(var x in ClientCli.instance.clients)
            await x.SendMessageServer("teste/ping", "post", "");
    }));
    cli.AddCommand("send-data", new Command("", async () =>
    {
        foreach (var x in ClientCli.instance.clients)
            await x.SendMessageServer("teste/data", "post", new { name = "Teste data" , date = DateTime.Now});
    }));
    cli.AddCommand("send-to-all", new Command("", async () =>
    {
        var r = getInput("Pool Id : ") ?? "defaultpool";
        var msg = getInput("Content : ") ?? "No Content";
        foreach (var x in ClientCli.instance.clients)
            await x.SendMessageServer("teste/message", "redirect", new { poolKey = r, content = msg });
    }));
    cli.AddCommand("entry-pool", new Command("", async () =>
    {
        var r = getInput("Pool Id : ") ?? "defaultpool";
        foreach (var x in ClientCli.instance.clients)
            await x.SendMessageServer("teste/entry", "listener",  r);
    }));
    cli.AddCommand("leave-pool", new Command("", async () =>
    {
        var r = getInput("Pool Id : ") ?? "defaultpool";
        foreach (var x in ClientCli.instance.clients)
            await x.SendMessageServer("teste/leave", "listener",r);
    }));
    cli.AddCommand("cls", new Command("", () =>
    {
        Console.Clear();
        return Task.CompletedTask;
    }));
    await cli.Run();

}));
ClientCli.instance.AddCommand("close", new("To close all connections", async () =>
{
    foreach (var x in ClientCli.instance.clients)
        await x.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
    ClientCli.instance.clients.Clear();
}));
ClientCli.instance.AddCommand("abort", new("To abort all", () =>
{
    ClientCli.instance.clients.ForEach(x => x.Abort());
    ClientCli.instance.clients.Clear();
    return Task.CompletedTask;
}));
ClientCli.instance.AddCommand("cls", new("To clear the console", () =>
{
    Console.Clear();
    return Task.CompletedTask;
}));

await ClientCli.instance.Run();



async void Receive(ClientWebSocket ws)
{
    while (ws.State == WebSocketState.Open)
    {
        try
        {
            var buffer = new byte[500_000];
            var r = await ws.ReceiveAsync(buffer, CancellationToken.None);
            var msg = Encoding.UTF8.GetString(buffer, 0, r.Count);
            var msgB = new StringBuilder();
            msgB.AppendLine($"Message received : {msg}");
            msgB.AppendLine($"Message Type : {r.MessageType.ToString()}");
            if (r.MessageType == WebSocketMessageType.Close)
            {
                msgB.AppendLine($"Close status : {r.CloseStatus}");
                msgB.AppendLine($"Close description : {r.CloseStatusDescription}");
            }
            Console.WriteLine(msgB.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while receiving message, ex:{ex.Message}\n\n{ex.ToString()}\n");
        }
    }
}


async Task<ClientWebSocket> ConnectionFactory(string Ip = "wss://localhost:7075", bool EnableReceiveMessage = true)
{

    var ws = new ClientWebSocket();
    var rngIp = $"{Ip}/{Guid.NewGuid()}";
    await ws.ConnectAsync(new Uri(rngIp), CancellationToken.None);
    if (EnableReceiveMessage)
    {
        Receive(ws);
    }
    return ws;
};
public record Command(string description, Func<Task> exec);


public class ClientCli
{
    public static ClientCli instance { get; private set; }

    private readonly Dictionary<string, Command> commands = new ();
    public readonly List<ClientWebSocket> clients = new ();


    private bool toQuit = false;
    static ClientCli()
    {
        instance = new ClientCli();
    }
    public ClientCli()
    {
        AddCommand("quit", new("", () =>
        {
            this.toQuit = true;
            return Task.CompletedTask;
        }));
    }
    public void AddCommand(string cmd,Command command)
    {
        commands.Add(cmd,command);

    }
    public async Task Run()
    {
        while (true)
        {
            if (this.toQuit) break;
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Total connections : {clients.Count}");
                Console.ForegroundColor = ConsoleColor.White;
                foreach (var cmd in commands)
                {
                    Console.WriteLine($"key: {cmd.Key} for: {cmd.Value.description}");
                }
                var choice = Console.ReadLine()?.ToLower().Trim() ?? "";
                if (commands.TryGetValue(choice, out var f))
                {
                    try
                    {
                        await f.exec();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Command Excpetion : {ex.Message}\n\n{ex.ToString()}");
                    }
                }
                else
                {
                    Console.WriteLine("Unregonized command");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}


public static class ex
{ 
    public static async Task SendMessageServer(this ClientWebSocket ws,string path,string method,object? data)
    {
        await ws.SendAsync(JsonSerializer.SerializeToUtf8Bytes(new {path=path,method=method,data=data }),WebSocketMessageType.Text,true,CancellationToken.None);
    }
}
