
-- Script to create TaskSystem DB

-- Tabla de Usuarios
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL UNIQUE,
    FullName NVARCHAR(200) NOT NULL,
    PasswordHash NVARCHAR(500) NOT NULL,
    IsOnline BIT NOT NULL DEFAULT 0,
    LastActive DATETIME NULL
);

-- Tabla de Prioridades
CREATE TABLE Priorities (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL
);

-- Tabla de Tareas
CREATE TABLE Tasks (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    IsCompleted BIT NOT NULL DEFAULT 0,
    DueDate DATE NULL,
    PriorityId INT NULL,
    UserId INT NOT NULL,
    IsLocked BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (PriorityId) REFERENCES Priorities(Id),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);
