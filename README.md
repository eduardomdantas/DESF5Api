# DESF5 - Atividade do Desafio Final

API para gestão de clientes, produtos e pedidos desenvolvida com .NET 8 e PostgreSQL

[UML](docs/images/uml.png)

[Diagrama de Arquitetura](docs/diagrams/c4.md)

## 🚀 Funcionalidades Principais

- **Gestão Completa de:**
  - 👥 Clientes (CRUD com validação de CPF)
  - 📦 Produtos (CRUD com gestão de preços)
  - 🛒 Pedidos (Com itens e cálculo automático de total)
  
- 🔐 Autenticação JWT com roles de administrador
- ✅ Validações robustas com FluentValidation
- 📊 Padrão Observer para logging extensível
- 🐳 Execução em containers Docker
- 📄 Documentação Swagger integrada

## 🛠 Stack Tecnológica

| Componente       | Tecnologias                                                                 |
|------------------|-----------------------------------------------------------------------------|
| **Backend**      | .NET 8, C# 12, ASP.NET Core                                                 |
| **Banco de Dados**| PostgreSQL 15, Dapper                                                      |
| **Autenticação** | JWT, BCrypt.NET                                                             |
| **Infraestrutura**| Docker, Docker Compose                                                     |
| **Validação**    | FluentValidation                                                            |
| **Logging**      | Padrão Observer (Console + extensível)                                      |

## 📦 Estrutura do Projeto

Desf5/
├── Controllers/ # Endpoints da API
├── Models/ # Entidades de domínio
├── Repository/ # Camada de acesso a dados
├── Services/ # Lógica de negócio
├── Validators/ # Regras de validação
├── WebAPI/ # Configuração inicial
└── Dockerfile # Configuração do container

## 🔌 Endpoints Principais

### Autenticação
| Método | Endpoint       | Descrição                |
|--------|----------------|--------------------------|
| POST   | /api/auth/login| Autentica usuário (JWT)  |

### Clientes
| Método | Endpoint            | Descrição                |
|--------|---------------------|--------------------------|
| GET    | /api/clientes       | Lista todos clientes     |
| POST   | /api/clientes       | Cria novo cliente        |
| PUT    | /api/clientes/{id}  | Atualiza cliente         |

### Produtos
| Método | Endpoint            | Descrição                |
|--------|---------------------|--------------------------|
| GET    | /api/produtos       | Lista todos produtos     |
| POST   | /api/produtos       | Cria novo cliente        |
| PUT    | /api/produtos/{id}  | Atualiza produto         |

### Pedidos
| Método | Endpoint                    | Descrição                |
|--------|-----------------------------|--------------------------|
| POST   | /api/pedidos                | Cria um pedido           |
| PUT    | /api/pedidos/{id}           | Atualiza pedido          |
| POST   | /api/pedidos/{id}/itens     | Adiciona item ao pedido  |
| DELETE | /api/pedidos/itens/{itemId} | Remove item do pedido    |

## 🐋 Execução com Docker

1. Clone o repositório:
```bash
git clone https://github.com/seu-usuario/desf5.git
```

2. Inicie os containers:
```bash
docker-compose up -d --build
```

3. Acesse a documentação:
http://localhost:8080/swagger

## ⚙ Configuração de Ambiente
Crie um arquivo .env na raiz:
POSTGRES_USER=admin
POSTGRES_PASSWORD=******
JWT_SECRET=<chave-secreta-forte>

## Licença
Este projeto está licenciado sob a MIT License.

Desenvolvido por Eduardo Medeiros Dantas - 2025