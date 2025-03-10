# 🍔 Lanchonete TaBala Burgures

Sistema web para gestao de pedidos para uma lanchonete.

## 🧰 Tecnologias Utilizadas

- ASP.NET Core MVC  
- Entity Framework Core  
- SQL Server (via Migrations)  
- Identity para autenticação  
- Bootstrap 5  

## 👥 Perfis de Usuário

### Admin
- Pode gerenciar lanches, ingredientes e visualizar todos os pedidos.
- Pode concluir pedidos.

### Cliente
- Pode navegar pelo cardápio, montar e finalizar pedidos.

## 🎯 Funcionalidades

- ✅ Cadastro de ingredientes com preço  
- ✅ Cadastro de lanches com múltiplos ingredientes  
- ✅ Calculo automático do preço total do lanche  
- ✅ Cardápio com sistema de pedidos  
- ✅ Regras de desconto por quantidade:
  - 2 lanches: 3% de desconto
  - 3 lanches: 5% de desconto
  - 5 ou mais lanches: 10% de desconto
- ✅ Autenticação e associação de pedido ao usuário  
- ✅ Página “Meus Pedidos” com visualização de pedidos por usuário  
- ✅ “Concluir pedido” para administradores

## 📦 Instalação e Execução

### Pré-requisitos:
- .NET 7 SDK  
- Visual Studio 2022 ou VS Code  
- SQL Server LocalDB ou outro SQL Server disponível

### 🧠 Escolhas Técnicas
- ASP.NET Core MVC para estrutura clara em camadas.
- Entity Framework Core com Migrations para controle de dados.
- Identity para controle de login e segurança.

### Passos:
```bash
git clone https://github.com/seuusuario/lanchonete.git
cd lanchonete

Configurar o "DefaultConnection" que esta em "Appsettings.json" colocando a string de conexao do banco

update-database (para rodar a migration)

Criar usuario e fazer relação das regras de ADMIN para o mesmo.

pegando o ID de ADMIN na tabela "AspNetRoles" e colando na coluna "RoleID" na tabela "AspNetUserRoles"

