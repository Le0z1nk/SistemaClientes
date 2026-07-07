
# 👥 SistemaClientes

Sistema de cadastro de clientes desenvolvido com **COBOL** e **ASP.NET**, demonstrando a integração entre uma aplicação moderna e um sistema legado de mainframe.

O projeto consiste em uma API REST desenvolvida em ASP.NET que realiza operações de cadastro, consulta e atualização de clientes, consumindo uma aplicação COBOL responsável pelo processamento das informações.

---

# 🚀 Tecnologias Utilizadas

- COBOL
- ASP.NET
- C#
- REST API
- Visual Studio
- Swagger
- JSON
- HTTP

---

# 📂 Funcionalidades

✅ Cadastro de clientes

✅ Consulta de cliente por código

✅ Atualização do contato de um cliente

✅ Comunicação entre aplicação ASP.NET e programa COBOL

---

# 🏗️ Arquitetura

O projeto foi dividido em duas partes:

### Backend ASP.NET

Responsável por:

- Disponibilizar os endpoints REST;
- Receber as requisições HTTP;
- Validar os dados enviados pelo cliente;
- Realizar a comunicação com o programa COBOL;
- Retornar as respostas em formato JSON.

### Aplicação COBOL

Responsável por:

- Processar as operações de cadastro;
- Consultar clientes pelo código;
- Atualizar informações de contato;
- Manipular os dados conforme as regras de negócio.

---

# ⚙️ Como Executar

## 1. Clonar o repositório

```bash
git clone https://github.com/Le0z1nk/SistemaClientes.git
```

---

## 2. Abrir o projeto

Abra a solução no **Visual Studio**.

---

## 3. Restaurar as dependências

```bash
dotnet restore
```

---

## 4. Executar a aplicação

```bash
dotnet run
```

ou execute diretamente pelo Visual Studio.

---


# 🧪 Testes

Os endpoints podem ser testados utilizando:

- Swagger
- Insomnia
- 
---

# 🎯 Objetivo do Projeto


O projeto permitiu aplicar conceitos de:

- Desenvolvimento de APIs REST;
- Comunicação entre aplicações;
- Integração entre tecnologias legadas e modernas;
- Arquitetura cliente-servidor;
- Manipulação de dados via HTTP.
