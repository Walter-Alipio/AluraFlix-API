# AluraFlix

# Challenge Back-End 5ª edição

## API Rest

![Badge em desenvolvimento](https://img.shields.io/badge/Status-Em%20Desenvolvimento-green)

## 🔨 Objetivo do projeto

O objetivo do Alura Challenge é aplicar os conhecimentos obtidos através dos cursos disponíveis na plataforma. Recebemos os casds de desafio ao início de cada uma das 4 semans de challenge. Não há restrição quanto a qual técnologia deva ser utilizada pelos alunos, ficando a cargo de cada um decidir como irá construir o projeto.

### História

Após alguns testes com protótipos feitos pelo time de UX de uma empresa, foi requisitada a primeira versão de uma plataforma para compartilhamento de vídeos. A plataforma deve permitir ao usuário montar playlists com links para seus vídeos preferidos, separados por categorias.<br>
Os times de frontend e UI já estão trabalhando no layout e nas telas. Para o backend, as principais funcionalidades a serem implementadas são:

<ul>

   <li> API com rotas implementadas segundo o padrão REST;</li>
   <li> Validações feitas conforme as regras de negócio;</li>
   <li> Implementação de base de dados para persistência das informações;</li>
   <li> Serviço de autenticação para acesso às rotas GET, POST, PUT e DELETE.</li>

</ul>

## :ok: Semana 1

- [x] Armazenar no banco de dados as informações sobre os vídeos
- [x] Todos os campos de vídeos devem ser obrigatórios e validados.
- [x] Implementar para /videos POST/GET/GET_ID/PUT/DELETE.
- [x] PUT atualiza um ou mais campos de um vídeo. Retornar um Json com informações do video atualizado.

## :construction: Semana 2

- [x] Armazenar no banco de dados as informações sobre as categorias.
- [x] Uma nova categoria não pode ser criada caso tenha algum campo vazio.Caso em branco, informar: `O campo é obrigatório`.
- [x] Implementar para /categorias POST/GET/GET_ID/PUT/DELETE.
- [x] Implemente uma relação entre vídeos e categorias, atribuindo para cada vídeo uma categoria.
- [x] Criar uma rota `GET` relacionando `categorias` e `videos`, exemplo: `GET categorias/:id/videos/`.
- [x] Criar uma rota que busque vídeos por nome via `query parameters`, exemplo: `GET /videos/?search=jogos`.
- [x] A categoria com `ID = 1`, deve chamar `LIVRE` e caso ela não seja especificada na criação do vídeo, atribuir o `ID = 1`.
- [ ] Criar testes de unidade para os modelos e controller.
- [ ] Crie testes de integração.

## ✔️ Tecnologias utilizadas

- [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) , [EntityFrameworkCore 6.0.10](https://learn.microsoft.com/en-us/ef/) , [AutoMapper 12.0.0](https://automapper.org/) , [FluentResults 3.14.0](https://github.com/altmann/FluentResults) , [MySQL 8](https://dev.mysql.com/doc/relnotes/mysql/8.0/en/) , [Pomelo.EntityFrameworkCore](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql) , [Swagger](https://swagger.io/)

<br>

## 🛠️ Abrir e rodar o projeto

Altere o arquivo appsettings.json incluindo o código:

` "ConnectionStrings": { "DbConnection": //string de conexão }`

Criar a base de dados:

`dotnet ef database update`
<br><br>

# **Rotas do projeto**

# Categorias

### 1.1 Cadastra um nova categoria

| Método | Autorização | Rota        | Descrição               | Body Params | Query Params |
| ------ | ----------- | ----------- | ----------------------- | ----------- | ------------ |
| POST   | -           | /categorias | Cadastra nova categoria | JSON        | -            |

#### Body Params exemplo:

```
{
  "title": "Educação",
  "cor": "#ff0000 "
}
```

### 1.2 Retorna categoria por id

| Método | Autorização | Rota               | Descrição                             | Body Params | Query Params |
| ------ | ----------- | ------------------ | ------------------------------------- | ----------- | ------------ |
| GET    | -           | /categorias/`{id}` | Retorna uma categoria por id numérico | -           | -            |

### 1.3 Retorna lista de categoria

| Método | Autorização | Rota        | Descrição                   | Body Params | Query Params |
| ------ | ----------- | ----------- | --------------------------- | ----------- | ------------ |
| GET    | -           | /categorias | Retorna todas as categorias | -           | -            |

### 1.4 Atualiza categoria

| Método | Autorização | Rota               | Descrição                        | Body Params | Query Params |
| ------ | ----------- | ------------------ | -------------------------------- | ----------- | ------------ |
| PUT    | -           | /categorias/`{id}` | Permite atualizar uma categoria. | JSON        | -            |
|        |             |                    | Retorna dados atualizados.       |             |              |

#### Body Params exemplo:

```
{
  "title": "Saúde",
  "cor": "#0fff00 "
}
```

#### Resposta exemplo:

```
{
   "id": 2,
   "title": "Saúde",
   "cor": "#0fff00 "
}
```

### 1.5 Exluir categoria

| Método | Autorização | Rota               | Descrição                            | Body Params | Query Params |
| ------ | ----------- | ------------------ | ------------------------------------ | ----------- | ------------ |
| DELETE | -           | /categorias/`{id}` | Exclui a categoria indicada pelo id. | -           | -            |

<br><br>

# Videos

### 2.1 Cadastra video

| Método | Autorização | Rota    | Descrição           | Body Params | Query Params |
| ------ | ----------- | ------- | ------------------- | ----------- | ------------ |
| POST   | -           | /videos | Cadastra novo video | JSON        | -            |

#### Body Params exemplo:

```
{
  "title": "Como desenvolver boas práticas de programação?",
  "description": "Paulo Silveira e Fábio Akita discutem sobre boas práticas de programação",
  "url": "https://www.youtube.com/watch?v=GUanHEGlje4",
  "categoriaId": 4
}
```

### 2.2 Retorna video por id

| Método | Autorização | Rota           | Descrição                        | Body Params | Query Params |
| ------ | ----------- | -------------- | -------------------------------- | ----------- | ------------ |
| GET    | -           | /videos/`{id}` | Retorna um video por id numérico | -           | -            |

### 2.3 Retorna lista de videos

| Método | Autorização | Rota    | Descrição                           | Body Params | Query Params              |
| ------ | ----------- | ------- | ----------------------------------- | ----------- | ------------------------- |
| GET    | -           | /videos | Retorna todos os videos             | -           | -                         |
| GET    | -           | /videos | Retorna videos com título informado | -           | ?search=`Titulo do video` |

### 2.4 Atualiza video

| Método | Autorização | Rota           | Descrição                                    | Body Params | Query Params |
| ------ | ----------- | -------------- | -------------------------------------------- | ----------- | ------------ |
| PUT    | -           | /videos/`{id}` | Permite atualizar um ou mais dados do video. | JSON        | -            |
|        |             |                | Retorna dados atualizados.                   |             |              |

#### Body Params exemplo:

```
{
  "title": "[Video Alura] Como desenvolver boas práticas de programação?"
}
```

#### Resposta exemplo:

```
  "id": 5,
  "title": "[Video Alura] Como desenvolver boas práticas de programação?",
  "description": "Paulo Silveira e Fábio Akita discutem sobre boas práticas de programação",
  "url": "https://www.youtube.com/watch?v=GUanHEGlje4",
  "categoria": {
    "id": 4,
    "title": "Educação",
    "cor": "#ff0000 "
  }
}
```

### 2.5 Exluir video

| Método | Autorização | Rota           | Descrição                         | Body Params | Query Params |
| ------ | ----------- | -------------- | --------------------------------- | ----------- | ------------ |
| DELETE | -           | /videos/`{id}` | Exclui um video indicado pelo id. | -           | -            |
