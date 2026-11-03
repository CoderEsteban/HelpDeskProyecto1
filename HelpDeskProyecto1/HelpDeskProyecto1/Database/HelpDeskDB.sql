USE master;
GO

CREATE DATABASE HelpDeskDB;
GO

USE HelpDeskDB;
GO


/* =========================================================
   TABLA: Usuarios
   ========================================================= */

CREATE TABLE Usuarios
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(256) NOT NULL,
    Rol NVARCHAR(20) NOT NULL,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    Estado BIT NOT NULL DEFAULT 1,

    CONSTRAINT CK_Usuarios_Rol
        CHECK (Rol IN ('Usuario', 'Administrador'))
);
GO


/* =========================================================
   TABLA: EstadosTicket
   ========================================================= */

CREATE TABLE EstadosTicket
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL UNIQUE
);
GO


/* =========================================================
   TABLA: Tickets
   ========================================================= */

CREATE TABLE Tickets
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Titulo NVARCHAR(150) NOT NULL,
    Descripcion NVARCHAR(1000) NOT NULL,
    IdUsuarioSolicitante INT NOT NULL,
    IdUsuarioAsignado INT NULL,
    IdEstado INT NOT NULL,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    FechaActualizacion DATETIME NOT NULL DEFAULT GETDATE(),
    Categoria NVARCHAR(100) NOT NULL,

    CONSTRAINT FK_Tickets_UsuarioSolicitante
        FOREIGN KEY (IdUsuarioSolicitante)
        REFERENCES Usuarios(Id),

    CONSTRAINT FK_Tickets_UsuarioAsignado
        FOREIGN KEY (IdUsuarioAsignado)
        REFERENCES Usuarios(Id),

    CONSTRAINT FK_Tickets_Estado
        FOREIGN KEY (IdEstado)
        REFERENCES EstadosTicket(Id)
);
GO


/* =========================================================
   TABLA: Comentarios
   ========================================================= */

CREATE TABLE Comentarios
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    IdTicket INT NOT NULL,
    IdUsuario INT NOT NULL,
    Texto NVARCHAR(1000) NOT NULL,
    Fecha DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Comentarios_Ticket
        FOREIGN KEY (IdTicket)
        REFERENCES Tickets(Id),

    CONSTRAINT FK_Comentarios_Usuario
        FOREIGN KEY (IdUsuario)
        REFERENCES Usuarios(Id)
);
GO


/* =========================================================
   ESTADOS INICIALES
   ========================================================= */

INSERT INTO EstadosTicket (Nombre)
VALUES
    ('Abierto'),
    ('En Proceso'),
    ('Resuelto'),
    ('Cerrado');
GO


/* =========================================================
   USUARIOS DE PRUEBA
   La contraseña se guarda como SHA-256 hexadecimal.
   ========================================================= */

INSERT INTO Usuarios
(
    Nombre,
    Email,
    PasswordHash,
    Rol
)
VALUES
(
    'Usuario de prueba',
    'usuario@helpdesk.local',
    CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'Usuario123*'), 2),
    'Usuario'
),
(
    'Administrador',
    'admin@helpdesk.local',
    CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'Admin123*'), 2),
    'Administrador'
);
GO


/* =========================================================
   VERIFICACIÓN
   ========================================================= */

SELECT * FROM Usuarios;
SELECT * FROM EstadosTicket;
SELECT * FROM Tickets;
SELECT * FROM Comentarios;
GO