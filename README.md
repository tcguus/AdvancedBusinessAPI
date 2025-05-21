# MottuAPI

Projeto desenvolvido para a disciplina **Advanced Business Development with .NET**, com o objetivo de criar uma API RESTful utilizando ASP.NET Core, Entity Framework Core e banco de dados Oracle, seguindo os padrões modernos de desenvolvimento e documentação com OpenAPI (Swagger).

---

## Tecnologias utilizadas

- ASP.NET Core 8 (Web API)
- Entity Framework Core
- Banco de Dados Oracle XE
- Oracle.ManagedDataAccess
- Swagger / OpenAPI
- Rider IDE

---

## Funcionalidades da API

A API permite a **gestão de motos no pátio da Mottu**, oferecendo as seguintes operações:

| Método | Rota                | Descrição                            |
|--------|---------------------|----------------------------------------|
| GET    | `/api/moto`         | Retorna todas as motos                |
| GET    | `/api/moto/{id}`    | Retorna uma moto pelo ID              |
| POST   | `/api/moto`         | Cadastra uma nova moto                |
| PUT    | `/api/moto/{id}`    | Atualiza os dados de uma moto         |
| DELETE | `/api/moto/{id}`    | Remove uma moto                       |

---

## Estrutura da entidade

A entidade `Moto` possui os seguintes campos:

```csharp
public class Moto
{
    public int Id { get; set; }
    public string Placa { get; set; }
    public string Modelo { get; set; }
    public string Status { get; set; }
}
```
---

## Exemplos de Requisição (JSON)
### POST /api/moto
```csharp
{
  "placa": "ABC1234",
  "modelo": "Sport",
  "status": "disponível"
}
```

### PUT /api/moto/1
```csharp
{
  "id": 1,
  "placa": "ABC1234",
  "modelo": "Sport",
  "status": "em manutenção"
}
```

---

## Instalação e Execução
A seguir estão os passos necessários para executar o projeto localmente em sua máquina.

### Pré-requisitos:
- [.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Oracle Database XE instalado e ativo
- Rider, Visual Studio ou outro editor compatível
- SQL Developer (opcional, para inspeção direta no banco)

> Certifique-se de que o Oracle Listener e o OracleServiceXE estão em execução no Windows (via `services.msc`), antes de iniciar o projeto.

---
## Como instalar o Oracle XE (caso não tenha)
**1. Acesse: https://www.oracle.com/database/technologies/xe-downloads.html**

**2. Baixe a versão para Windows x64**

**3. Siga a instalação padrão (Next > Next > Finish)**

**4. Durante a instalação:**
- Defina uma senha segura para o usuário SYSTEM
- O Service Name será XEPDB1 (guarde isso)

**5. Após instalar, abra o services.msc e verifique se estes serviços estão "Em execução":**
- OracleOraDB21Home1TNSListener
- OracleServiceXE

> Após esses passos, o banco Oracle estará pronto para ser acessado pela API.

---
 
### Passos para rodar o projeto:
**1. Clone este repositório:**
```bash
git clone https://github.com/tcguus/AdvancedBusinessAPI.git
cd AdvancedBusinessAPI
```
**2. Configure a string de conexão no arquivo appsettings.json:**
```bash
"ConnectionStrings": {
  "OracleConnection": "User Id=system;Password=SUA_SENHA;Data Source=localhost:1521/XEPDB1"
}
```
**3. Execute as migrations para criar as tabelas no banco:**
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```
**4. Rode a aplicação:**
```bash
dotnet run
```
**5. Acesse o Swagger no navegador:**
```bash
https://localhost:5298/swagger
```
> A porta pode variar conforme seu ambiente. Verifique o terminal após iniciar a aplicação.

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
