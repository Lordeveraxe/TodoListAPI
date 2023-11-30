
# Todo List API Project

## Overview

This project is a simple Todo List API built with ASP.NET Core, Entity Framework (EF) Core, and MySQL. It allows users to create, read, update, and delete todo items. Each item has a title and a description.

## Setup and Installation

### Prerequisites

- .NET Core SDK
- MySQL Server
- EF Core CLI tools

### Database Setup

First, create a MySQL database and a table for the todo items. Use the following SQL script:

```sql
CREATE DATABASE TodoListDB;

USE TodoListDB;

CREATE TABLE TodoItems (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Title VARCHAR(255) NOT NULL,
    Description TEXT
);
```

### Application Setup

1. **Clone the Repository**
   
   Clone this repository to your local machine.

2. **Install EF Core MySQL Provider**

   Run the following command in the project directory to install the EF Core MySQL provider:

   ```bash
   dotnet add package Pomelo.EntityFrameworkCore.MySql
   ```

3. **Scaffold the Model from the Database**

   Use EF Core to scaffold the model from your MySQL database. Run the following command, replacing the connection string with your database details:

   ```bash
   dotnet ef dbcontext scaffold "Server=localhost;Database=TodoListDB;User=root;Password=root;" Pomelo.EntityFrameworkCore.MySql -output-dir Models
   ```

   This command will create entity classes and a `DbContext` in the `Models` directory based on the existing database schema.

### Generate Controller

Generate the `TodoItemsController` using the ASP.NET Core scaffolding tool:

```bash
dotnet aspnet-codegenerator controller -name TodoItemsController -async -api -m TodoItem -dc TodoContext -outDir Controllers
```

This command creates an API controller with CRUD actions, using `TodoItem` as the model and `TodoContext` as the database context.

### Run the Application

To run the application, use the following command in the project directory:

```bash
dotnet run
```

The API will be hosted locally, and you can interact with it using tools like Postman or Swagger.

## Features

- Create, Read, Update, and Delete Todo items.
- Each Todo item consists of a title and a description.
- Persistence of Todo items in a MySQL database using EF Core.
