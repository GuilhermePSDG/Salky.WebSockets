# Salky.WebSockets
> Salky abstrai o uso de WebSocket para uma aplicação similar a implementação de http do dotnet
### Como usar

- Criar uma aplicação web
- Executar o código abaixo / injetar no contêiner de dependências
<img src="readme/example1.png">

- `SetAuthGuard` é usado para permitir ou não a entrada de um usuário
- `UseDefaultConnectionMannager` irá armazenar e disponibilizar as conexões abertas quando preciso
    - `UseBasicConnectionRemotion` irá remover e adicionar as novas conexões automaticamente
- `UseRouter` irá mapear as rotas de WebSocket


- Para criar a sua rota, basta criar uma classe que herda de `WebSocketRouteBase` e é anotada por `WebSocketRoute`.<br>Por convenção o caminho da rota no caso abaixo será `'teste'`, sendo possível passar por parâmetro o nome da rota.
   
    - Cada método anotado será uma rota aninhada dentro da rota da classe

    - Cada rota final, possui um método e um caminho

    - O anotador do método representa o `método` da rota e o `caminho`

    - Cada rota recebe um único parâmetro, de qualquer tipo. <br> Para múltiplos parâmetros é recomendado encapsular em uma classe ou record
    
    - `WebSocketRouteBase` fornece métodos para manipular os clientes, recuperar claims/id do client via `User` como demonstrado abaixo
    
    - Quando enviado para a pool `root` será enviada para todos os clientes disponíveis

<img src="readme/example2.png">

<br>

> ### [Como usar no lado do cliente `(JS)`](https://github.com/GuilhermePSDG/Salky.WebSockets/blob/main/Example/JavaScriptUseCase/index.js)
> ### [Exemplo no lado do server](https://github.com/GuilhermePSDG/Salky.WebSockets/tree/main/Example)
<br>


<h4 align="center"> 🚧 Projeto em construção 🚧 </h4>
