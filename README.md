# Salky.WebSockets
> Salky abstrai o uso de WebSocket para uma aplicação similar a implementação de http do dotnet

### Como usar

- 1 - Criar WebHost
- 2 - Executar o codigo abaixo
<img src="readme/example1.png">
    - `SetAuthGuard` é usado para permitir ou não a entrada de um usuario
    - `UseDefaultConnectionMannager` irá armazenar e disponibilizar as conexões abertas quando preciso
        - `UseBasicConnectionRemotion` irá remover e adicionar as novas conexões automaticamente
    - `UseRouter` ira mapear as rotas de WebSocket
<br>
- Criar a sua rota, que será uma clase que herda de `WebSocketRouteBase` e é anotada por `WebSocketRoute`, por convenção o caminho da rota no caso será `'teste'`, sendo possível passar por parâmetro o nome da rota.
    - Cada metodo que está anotado será uma rota aninhada dentro da rota da classe

    - Cada rota final, possui um metodo e um caminho

    - O anotador do metodo representa o `metodo` e o `caminho`

    - Cada rota recebe um unico parametro, de qualquer tipo, 
    recomendado encapsular os parametros em uma classe ou record quando preciso
    
    - `WebSocketRouteBase` fornece metodos para manipular os clientes, recuperar claims/id do client via `User` como demontrado abaixo

<img src="readme/example2.png">


### Como usar no lado do cliente `(JS)`


<h4 align="center"> 🚧 Projeto em construção 🚧 </h4>