# Mottu API — Sprint 3 (.NET 8)

API REST para gestão de **Motos** e **Manutenções** com autenticação **JWT**.  
Projeto estruturado em camadas com foco em **boas práticas REST**: paginação, filtros, **HATEOAS**, status codes adequados e documentação Swagger/OpenAPI.

---

## Objetivos do Projeto

1. Entregar uma API RESTful funcional e bem documentada.
2. Atender integralmente aos critérios da disciplina:
   - 3 entidades principais: **Usuario**, **Moto**, **Manutencao**.
   - CRUDs com boas práticas (paginação, HATEOAS, status correctos).
   - Swagger/OpenAPI com descrições, exemplos e modelos.
3. Servir de backend para o app mobile (React Native) da sprint, com fluxo de **login** e endpoints integráveis.

---

## Arquitetura e Decisões

A solução segue uma arquitetura em camadas para separar responsabilidades e facilitar testes/manutenção:

- **Domain**: contém as entidades de negócio (`Usuario`, `Moto`, `Manutencao`) e enums. Sem dependência de infraestrutura.
- **Application**: camada de aplicação, com **DTOs**, **Validadores** (FluentValidation) e **mapeamentos** (AutoMapper). Isola regras e contratos de entrada/saída.
- **Infrastructure**: persistência e serviços de dados com **Entity Framework Core** e **SQLite**. Inclui `AppDbContext`, `Migrations` e `DbSeeder`.
- **Api**: camada de apresentação HTTP. Contém os Controllers, configuração do **JWT**, **Swagger/OpenAPI**, formatação JSON e injeção de dependências.

**Justificativa do domínio**  
O escopo atende a um cenário de gestão de frotas para locação/entrega de motos:
- `Moto`: ativo principal a ser gerido (modelo, placa, ano, status, localização).
- `Manutencao`: histórico e controle de serviços executados nas motos.
- `Usuario`: autenticação e autorização mínima para consumo da API pelo app mobile.

---

## Tecnologias e Pacotes

- **.NET 8 / ASP.NET Core Web API**
- **Entity Framework Core** + **SQLite**
- **Swagger / Swashbuckle** (+ Annotations, Filters)
- **JWT** (Microsoft.AspNetCore.Authentication.JwtBearer)
- **AutoMapper**
- **FluentValidation**
- **xUnit** + **FluentAssertions** + **EFCore.InMemory** (testes)

---

## Como Executar

Pré-requisitos:
- .NET 8 SDK instalado.

Passos:

```bash
dotnet tool install --global dotnet-ef
dotnet restore
dotnet build

# cria/aplica schema do banco (SQLite) a partir das migrations
dotnet ef database update -p AdvancedBusinessAPI.Infrastructure -s AdvancedBusinessAPI.Api

# executa a API
dotnet run --project AdvancedBusinessAPI.Api
```

Abra a URL informada no console e acrescente **/swagger** para acessar a documentação interativa.

---

## Banco de Dados e Migrações

- Banco padrão: **SQLite** (arquivo local).
- As **Migrations** ficam em `AdvancedBusinessAPI.Infrastructure/Migrations/` e são versionadas no Git.
- Para criar novas migrations:
  ```bash
  dotnet ef migrations add NomeDaMigration -p AdvancedBusinessAPI.Infrastructure -s AdvancedBusinessAPI.Api
  dotnet ef database update -p AdvancedBusinessAPI.Infrastructure -s AdvancedBusinessAPI.Api
  ```

O projeto inclui um **DbSeeder** para dados iniciais (usuário, motos e manutenções de exemplo).

---

## Autenticação (JWT)

Fluxo:
1. `POST /api/v1/auth/register` cria um novo usuário.
2. `POST /api/v1/auth/login` retorna um **token JWT**.
3. Use o token no botão **Authorize** do Swagger ou no header:
   ```
   Authorization: Bearer {token}
   ```
---

## Padrões REST adotados

- **Rotas estáveis** prefixadas com `/api/v1`.
- **Paginação** em coleções: `page`, `pageSize` e cabeçalho `X-Pagination` com metadados; `pageSize=0` retorna **todos** os itens.
- **Filtros** nos recursos de listagem (`modelo`, `placa`, `motoId`).
- **HATEOAS**: links de navegação em coleções e itens de `Motos`.
- **Status codes**:
  - 200 OK, 201 Created (+ `Location`), 204 No Content
  - 400 Bad Request, 401 Unauthorized, 404 Not Found
  - 409 Conflict (ex.: placa ou e-mail já existentes)
  - 422 Unprocessable Entity (ex.: `motoId` inexistente em manutenção)
- **Enums como texto** no JSON (JsonStringEnumConverter).

---

## Endpoints

Base: `/api/v1`

### Autenticação
- `POST /auth/register` – cria usuário.  
  Retornos: 201, 409, 400.
- `POST /auth/login` – retorna `{ token }`.  
  Retornos: 200, 401.
- `GET /auth/me` (opcional) – valida o token.

### Motos
- `GET /motos` – lista paginada, com filtros `modelo` e `placa`.  
  Suporta `page`, `pageSize`; `pageSize=0` retorna todas.  
  Retorna `X-Pagination` e links HATEOAS.  
  Retorno: 200.
- `GET /motos/{id}` – retorna uma moto por ID, com HATEOAS.  
  Retornos: 200, 404.
- `POST /motos` – cria moto (placa única).  
  Retornos: 201, 409, 400.
- `PUT /motos/{id}` – atualiza moto.  
  Retornos: 204, 404, 400.
- `DELETE /motos/{id}` – exclui moto.  
  Retornos: 204, 404.

### Manutenções
- `GET /manutencoes` – lista geral; filtro `?motoId={guid}`.  
  Retorno: 200.
- `GET /manutencoes/{id}` – detalhe por ID.  
  Retornos: 200, 404.
- `POST /manutencoes` – cria manutenção vinculada a uma moto existente.  
  Retornos: 201, 422, 400.
- `PUT /manutencoes/{id}` – atualiza manutenção.  
  Retornos: 204, 404, 400.
- `DELETE /manutencoes/{id}` – exclui manutenção.  
  Retornos: 204, 404.
- `GET /motos/{motoId}/manutencoes` – manutenções de uma moto.  
  Retorno: 200.

---

## Testes Automatizados

Projeto: `AdvancedBusinessAPI.Tests` (xUnit).

- **MotoValidatorTests**: valida regras de criação de moto (campos obrigatórios, tamanhos, faixas).
- **DbSeederTests**: garante que o seeder cria registros de usuário e motos ao iniciar.

Executar:

```bash
dotnet test
```

---

## Swagger/OpenAPI

- Ativo em ambiente de desenvolvimento em `/swagger`.
- Configurações principais:
  - `SwaggerDoc` com título, versão, descrição, contato e licença.
  - **JWT Bearer** no botão **Authorize**.
  - **Annotations** habilitadas (`[SwaggerOperation]`, `[SwaggerResponse]`).
  - **XML comments** incluídos (summary/remarks dos controllers).
  - **Servers** listados (HTTP/HTTPS).
  - **Swagger UI** customizado com `wwwroot/swagger.css`.
  - OperationFilter para respostas 401/403 automáticas em rotas protegidas.

---


## Guia de Troubleshooting

- Erro `no such table`: rode `dotnet ef database update` ou confirme que as migrations estão presentes.
- Erro de HTTPS: execute `dotnet dev-certs https --trust`.
- Swagger não abre: confirme URL/porta no `launchSettings.json` ou no console do run.
- Token inválido: gere novamente com `POST /auth/login` e use o botão **Authorize**.
- Conflito de pacote NuGet: alinhe versões (ex.: AutoMapper e sua extensão conforme a solution).

---

## Nossos integrantes
- **Gustavo Camargo de Andrade**
- RM555562
- 2TDSPF
-------------------------------------------
- **Rodrigo Souza Mantovanello**
- RM555451
- 2TDSPF
-------------------------------------------
- **Leonardo Cesar Rodrigues Nascimento**
- RM558373
- 2TDSPF
