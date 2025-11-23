# CareerMap.Recommendations.Api - API de Recomendações (.NET 8)

Esta é a implementação da API de Recomendações para o projeto **CareerMap**, desenvolvida em .NET 8, conforme os requisitos da disciplina **Advanced Business Development with .NET** da Global Solution 2025.

## 🚀 Requisitos Atendidos

| Requisito | Status | Detalhes da Implementação |
| :--- | :--- | :--- |
| **1. Boas Práticas REST** | ✅ Completo | Uso de verbos HTTP corretos (GET, POST, PUT, DELETE), Status Codes adequados (200, 201, 204, 404) e implementação de **Paginação** e **HATEOAS** no endpoint de `Carreiras`. |
| **2. Monitoramento e Observabilidade** | ✅ Completo | Implementação de **Health Checks** (`/health/ready` e `/health/live`) e **Logging** estruturado via Serilog. |
| **3. Versionamento da API** | ✅ Completo | Estrutura de rotas com versionamento explícito (`api/v1/[controller]`). |
| **4. Integração e Persistência** | ✅ Completo | Uso de **Entity Framework Core** com banco de dados SQLite (`CareerMapRecommendations.db`). O banco é criado e populado automaticamente com dados iniciais (Seed Data) ao iniciar a aplicação em ambiente de desenvolvimento. |
| **5. Testes Integrados** | ✅ Completo | Implementação de testes de integração com **xUnit** e `WebApplicationFactory`, utilizando um banco de dados **InMemory** para isolamento e agilidade. |

## 🛠️ Como Rodar o Projeto Localmente

### Pré-requisitos

*   .NET 8 SDK instalado.

### Passos

1.  **Navegue até a pasta da API:**
    ```bash
    cd CareerMap.Recommendations/CareerMap.Recommendations.Api
    ```
2.  **Rode a aplicação:**
    ```bash
    dotnet restore
    dotnet build
    dotnet run
    ```
3.  **Acesse o Swagger:**
    A API estará disponível em `http://localhost:5097` (ou outra porta configurada). O Swagger UI (documentação interativa) estará em:
    ```
    http://localhost:5097/swagger
    ```

## 🔗 Endpoints Principais (v1)

| Método | Rota | Descrição | Boas Práticas |
| :--- | :--- | :--- | :--- |
| `GET` | `/api/v1/Carreiras` | Lista paginada de carreiras. | Paginação, HATEOAS |
| `GET` | `/api/v1/Carreiras/{id}` | Detalhe de uma carreira. | Status Codes |
| `POST` | `/api/v1/Carreiras` | Cria uma nova carreira. | Status Code 201 (Created), HATEOAS |
| `PUT` | `/api/v1/Carreiras/{id}` | Atualiza uma carreira. | Status Code 204 (No Content) |
| `DELETE` | `/api/v1/Carreiras/{id}` | Exclui uma carreira. | Status Code 204 (No Content) |
| `GET` | `/health/ready` | Health Check de prontidão (inclui status do DB). | Observabilidade |
| `GET` | `/health/live` | Health Check de atividade. | Observabilidade |

## 🧪 Executando os Testes

1.  **Navegue até a pasta raiz da solução:**
    ```bash
    cd CareerMap.Recommendations
    ```
2.  **Execute os testes:**
    ```bash
    dotnet test
    ```

## ⚙️ Estrutura do Projeto

O projeto segue a arquitetura de Camadas (Domain, Infrastructure, API):

*   **`CareerMap.Recommendations.Domain`**: Contém as entidades de negócio (`Carreira`, `Competencia`).
*   **`CareerMap.Recommendations.Infrastructure`**: Contém a lógica de persistência (Entity Framework Core, `RecommendationsDbContext`, Migrations).
*   **`CareerMap.Recommendations.Api`**: O projeto principal que expõe os *endpoints* REST, contém os *Controllers*, DTOs e a configuração de *middleware* (Swagger, Serilog, Health Checks).
*   **`CareerMap.Recommendations.Tests`**: Contém os testes de integração.

---

