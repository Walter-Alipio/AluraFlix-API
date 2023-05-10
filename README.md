# AluraFlix - API

<img src="./img/AluraFlix-Logo.png">

A plataforma que permiti aos usuários montar playlists com links para seus vídeos preferidos, separados por categorias.

| :placard: Vitrine.Dev |                                                      |
| --------------------- | ---------------------------------------------------- |
| :sparkles: Nome       | **Challenge Back-End 5ª edição - AluraFlix**         |
| :label: Tecnologias   | c#, .Net 6, SQL Server, Entity, Identity, JWT, XUnit |
| :fire: Desafio        | https://www.alura.com.br/challenges/back-end         |

#

![Badge em desenvolvimento](https://img.shields.io/badge/Status-Em%20Desenvolvimento-green)

## 🔨 Objetivo do projeto

O objetivo do Alura Challenge é aplicar os conhecimentos obtidos através dos cursos disponíveis na plataforma. As tarefas são disponibilizadas ao início de cada uma das 4 semans de challenge. Não há restrição quanto a qual técnologia deva ser utilizada pelos alunos, ficando a cargo de cada um decidir como irá construir o projeto.

## Extras

- As rotas agora requerem autenticação, menos get Videos e Categorias.
- Apenas o adminstrador pode cadastrar, alterar ou deletar uma categoria.
- Há um novo endpoint para acessar apenas os videos do próprio usuário.
- Apenas o proprietário do video pode alterar ou deletar um video.

### História

Após alguns testes com protótipos feitos pelo time de UX de uma empresa, foi requisitada a primeira versão de uma plataforma para compartilhamento de vídeos. A plataforma deve permitir ao usuário montar playlists com links para seus vídeos preferidos, separados por categorias.<br>
Os times de frontend e UI já estão trabalhando no layout e nas telas. Para o backend, as principais funcionalidades a serem implementadas são:

<ul>

   <li> API com rotas implementadas segundo o padrão REST;</li>
   <li> Validações feitas conforme as regras de negócio;</li>
   <li> Implementação de base de dados para persistência das informações;</li>
   <li> Serviço de autenticação para acesso às rotas POST, PUT e DELETE.</li>

</ul>

## :ok: Semana 1

- [x] Armazenar no banco de dados as informações sobre os vídeos
- [x] Todos os campos de vídeos devem ser obrigatórios e validados.
- [x] Implementar para /videos POST/GET/GET_ID/PUT/DELETE.
- [x] PUT atualiza um ou mais campos de um vídeo.

## :ok: Semana 2

- [x] Armazenar no banco de dados as informações sobre as categorias.
- [x] Uma nova categoria não pode ser criada caso tenha algum campo vazio.Caso em branco, informar: `O campo é obrigatório`.
- [x] Implementar para /categorias POST/GET/GET_ID/PUT/DELETE.
- [x] Implemente uma relação entre vídeos e categorias, atribuindo para cada vídeo uma categoria.
- [x] Criar uma rota `GET` relacionando `categorias` e `videos`, exemplo: `GET categorias/:id/videos/`.
- [x] Criar uma rota que busque vídeos por nome via `query parameters`, exemplo: `GET /videos/?search=jogos`.
- [x] A categoria com `ID = 1`, deve chamar `LIVRE` e caso ela não seja especificada na criação do vídeo, atribuir o `ID = 1`.
- [x] Criar testes de unidade para os modelos e controller.
- [ ] Crie testes de integração.

## :ok: Semana 3 e 4

- [x] Sistema de autenticação.
- [x] Alteração no banco de dados para tabela de usuário.
- [ ] Deploy.

## ✔️ Tecnologias utilizadas

- [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [EntityFrameworkCore 6.0](https://learn.microsoft.com/en-us/ef/)
- [AutoMapper](https://automapper.org/)
- [IdentityFramework 6.0](https://learn.microsoft.com/pt-br/aspnet/core/security/authentication/identity?view=aspnetcore-7.0&tabs=visual-studio)
- [JWT Bearer](https://jwt.io/introduction)
- [FluentResults](https://github.com/altmann/FluentResults)
- [DotEnv](https://github.com/bolorundurowb/dotenv.net)
- [SQL Sever 2022](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads)
- [Swagger](https://swagger.io/)
- [Docker](https://www.docker.com/)

<br>

## 🛠️ Abrir e rodar o projeto

Clone o projeto para seu repositório.
<br>
Instale as dependências através do comando:

`dotnet restore`

Configure sua connection string pelo user-secrets com o comando:

`dotnet user-secrets "DbConnection" "SUA_STRING_AQUI"`

Criar a base de dados:

`dotnet ef database update`

O adminstrador deve ser criado direto no banco de dados e deve ser atribuído o papel amdmin para o mesmo.

Crie um arquivo `.env` na raiz do projeto PlayListAPI seguindo o modelo do arquivo `.env.example`
<br><br>

# **Rotas do projeto**

<img src="./img/API.png#vitrinedev">

# Categorias

| Método | Autorização | Rota                       | Descrição                                        | Body Params | Query Params |
| ------ | ----------- | -------------------------- | ------------------------------------------------ | ----------- | ------------ |
| POST   | Admin       | /categorias                | Cadastra nova categoria                          | JSON        | -            |
| GET    | -           | /categorias/`{id}`         | Retorna uma categoria por id numérico            | -           | -            |
| GET    | -           | /categorias/`{id}/`/videos | Retorna lista de videos pertencentes a categoria | -           | -            |
| GET    | -           | /categorias                | Retorna todas as categorias                      | -           | -            |
| PUT    | Admin       | /categorias/`{id}`         | Permite atualizar uma categoria.                 | JSON        | -            |
|        |             |                            | Retorna dados atualizados.                       |             |              |
| DELETE | Admin       | /categorias/`{id}`         | Exclui a categoria indicada pelo id.             | -           | -            |

<br><br>

# Videos

| Método | Autorização | Rota           | Descrição                                    | Body Params | Query Params              |
| ------ | ----------- | -------------- | -------------------------------------------- | ----------- | ------------------------- |
| POST   | User        | /videos        | Cadastra novo video                          | JSON        | -                         |
| GET    | -           | /videos/`{id}` | Retorna um video por id numérico             | -           | -                         |
| GET    | -           | /videos        | Retorna todos os videos                      | -           | -                         |
| GET    | -           | /videos        | Retorna videos com título informado          | -           | ?search=`Titulo do video` |
| GET    | -           | /meus_videos   | Retorna lista de videos do usuário logado    | -           |                           |
| GET    | -           | /videos/bypage | Retorna lista de videos paginado             | -           | ?page=`1`&pageSize=`5`    |
| PUT    | User        | /videos/`{id}` | Permite atualizar um ou mais dados do video. | JSON        | -                         |
| DELETE | User        | /videos/`{id}` | Exclui um video indicado pelo id.            | -           | -                         |

<br><br>

# Usuário

| Método | Autorização | Rota             | Descrição                                  | Body Params | Query Params |
| ------ | ----------- | ---------------- | ------------------------------------------ | ----------- | ------------ |
| POST   | -           | /User/CriarLogin | Cadastra novo Usuario                      | JSON        | -            |
| POST   | -           | /User/Login      | Verifica usuário e retorna token de acesso | JSON        | -            |
| POST   | -           | /User/Logout     | Desloga usuário                            | JSON        | -            |
