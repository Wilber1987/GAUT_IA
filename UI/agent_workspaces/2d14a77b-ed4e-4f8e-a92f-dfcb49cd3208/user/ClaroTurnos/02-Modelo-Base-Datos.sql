/* =========================================================================
   CLARO TURNOS - Modelo de Base de Datos
   SQL Server 2022
   ========================================================================= */

CREATE DATABASE ClaroTurnosDB;
GO
USE ClaroTurnosDB;
GO

/* ---------------------------- Catálogos base ---------------------------- */

CREATE TABLE Roles (
    RolId           INT IDENTITY(1,1) PRIMARY KEY,
    Nombre          NVARCHAR(50)  NOT NULL UNIQUE,     -- Administrador, Supervisor, Agente
    Descripcion     NVARCHAR(200) NULL,
    Activo          BIT NOT NULL DEFAULT 1
);
GO

CREATE TABLE Sucursales (
    SucursalId      INT IDENTITY(1,1) PRIMARY KEY,
    Codigo          NVARCHAR(20)  NOT NULL UNIQUE,
    Nombre          NVARCHAR(150) NOT NULL,
    Direccion       NVARCHAR(300) NULL,
    Ciudad          NVARCHAR(100) NULL,
    HorarioApertura TIME NULL,
    HorarioCierre   TIME NULL,
    Activo          BIT NOT NULL DEFAULT 1,
    FechaCreacion   DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

CREATE TABLE Servicios (
    ServicioId      INT IDENTITY(1,1) PRIMARY KEY,
    SucursalId      INT NOT NULL REFERENCES Sucursales(SucursalId),
    Nombre          NVARCHAR(100) NOT NULL,            -- Ventas, Servicio al Cliente, Soporte Técnico...
    Prefijo         CHAR(1) NOT NULL,                   -- A, B, C, D, E
    Orden           INT NOT NULL DEFAULT 0,
    SlaMinutos      INT NOT NULL DEFAULT 10,            -- meta de tiempo de atención
    ReinicioDiario  BIT NOT NULL DEFAULT 1,
    Activo          BIT NOT NULL DEFAULT 1,
    CONSTRAINT UQ_Servicio_Prefijo_Sucursal UNIQUE (SucursalId, Prefijo)
);
GO

CREATE TABLE EstadosTicket (
    EstadoTicketId  INT IDENTITY(1,1) PRIMARY KEY,
    Codigo          NVARCHAR(20) NOT NULL UNIQUE,       -- ESPERANDO, EN_ATENCION, ATENDIDO, CANCELADO, AUSENTE, TRANSFERIDO
    Nombre          NVARCHAR(50) NOT NULL
);
GO

CREATE TABLE Ventanillas (
    VentanillaId    INT IDENTITY(1,1) PRIMARY KEY,
    SucursalId      INT NOT NULL REFERENCES Sucursales(SucursalId),
    Numero          INT NOT NULL,
    Nombre          NVARCHAR(50) NULL,                  -- "Ventanilla 3"
    Activo          BIT NOT NULL DEFAULT 1,
    CONSTRAINT UQ_Ventanilla_Sucursal UNIQUE (SucursalId, Numero)
);
GO

/* ------------------------------- Usuarios -------------------------------- */

CREATE TABLE Usuarios (
    UsuarioId       INT IDENTITY(1,1) PRIMARY KEY,
    SucursalId      INT NULL REFERENCES Sucursales(SucursalId),
    NombreCompleto  NVARCHAR(150) NOT NULL,
    NombreUsuario   NVARCHAR(50)  NOT NULL UNIQUE,
    CorreoElectronico NVARCHAR(150) NULL,
    PasswordHash    NVARCHAR(256) NOT NULL,
    PasswordSalt    NVARCHAR(256) NOT NULL,
    Activo          BIT NOT NULL DEFAULT 1,
    FechaCreacion   DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    UltimoLogin     DATETIME2 NULL
);
GO

CREATE TABLE UsuariosRoles (
    UsuarioId       INT NOT NULL REFERENCES Usuarios(UsuarioId),
    RolId           INT NOT NULL REFERENCES Roles(RolId),
    PRIMARY KEY (UsuarioId, RolId)
);
GO

-- Servicios que puede atender cada agente (para "llamar siguiente")
CREATE TABLE UsuariosServicios (
    UsuarioId       INT NOT NULL REFERENCES Usuarios(UsuarioId),
    ServicioId      INT NOT NULL REFERENCES Servicios(ServicioId),
    PRIMARY KEY (UsuarioId, ServicioId)
);
GO

/* -------------------------------- Tickets -------------------------------- */

CREATE TABLE Tickets (
    TicketId        BIGINT IDENTITY(1,1) PRIMARY KEY,
    SucursalId      INT NOT NULL REFERENCES Sucursales(SucursalId),
    ServicioId      INT NOT NULL REFERENCES Servicios(ServicioId),
    EstadoTicketId  INT NOT NULL REFERENCES EstadosTicket(EstadoTicketId),
    Codigo          NVARCHAR(10) NOT NULL,               -- A-001
    Correlativo     INT NOT NULL,                        -- 1, 2, 3... (numérico, reinicia por día/servicio)
    FechaGeneracion DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    FechaLlamado    DATETIME2 NULL,
    FechaFin        DATETIME2 NULL,
    VentanillaId    INT NULL REFERENCES Ventanillas(VentanillaId),
    UsuarioId       INT NULL REFERENCES Usuarios(UsuarioId),   -- agente que lo atiende actualmente
    VecesLlamado    INT NOT NULL DEFAULT 0,
    TicketOrigenId  BIGINT NULL REFERENCES Tickets(TicketId),  -- si viene de una transferencia
    CONSTRAINT UQ_Ticket_Codigo_Sucursal_Fecha UNIQUE (SucursalId, Codigo, FechaGeneracion)
);
GO
CREATE INDEX IX_Tickets_Estado ON Tickets(EstadoTicketId, ServicioId, FechaGeneracion);
GO

-- Historial de cada evento del ticket (llamado, transferencia, finalización...)
CREATE TABLE Atenciones (
    AtencionId      BIGINT IDENTITY(1,1) PRIMARY KEY,
    TicketId        BIGINT NOT NULL REFERENCES Tickets(TicketId),
    UsuarioId       INT NULL REFERENCES Usuarios(UsuarioId),
    VentanillaId    INT NULL REFERENCES Ventanillas(VentanillaId),
    EstadoTicketId  INT NOT NULL REFERENCES EstadosTicket(EstadoTicketId),
    FechaEvento     DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    DuracionSegundos INT NULL,                            -- calculado al finalizar
    Observacion     NVARCHAR(300) NULL
);
GO
CREATE INDEX IX_Atenciones_Ticket ON Atenciones(TicketId);
GO

/* ---------------------------- Configuración ------------------------------ */

CREATE TABLE Configuraciones (
    ConfiguracionId INT IDENTITY(1,1) PRIMARY KEY,
    SucursalId      INT NULL REFERENCES Sucursales(SucursalId),  -- NULL = global
    Clave           NVARCHAR(100) NOT NULL,
    Valor           NVARCHAR(500) NOT NULL,
    Descripcion     NVARCHAR(300) NULL,
    CONSTRAINT UQ_Config_Clave_Sucursal UNIQUE (SucursalId, Clave)
);
GO

CREATE TABLE ImpresorasTermicas (
    ImpresoraId     INT IDENTITY(1,1) PRIMARY KEY,
    SucursalId      INT NOT NULL REFERENCES Sucursales(SucursalId),
    KioscoCodigo    NVARCHAR(30) NOT NULL,
    Nombre          NVARCHAR(100) NOT NULL,
    TipoConexion    NVARCHAR(20) NOT NULL,   -- IP, USB, BLUETOOTH
    DireccionIP     NVARCHAR(50) NULL,
    Puerto          INT NULL,
    AnchoPapelMm    INT NOT NULL DEFAULT 58, -- 58mm u 80mm
    PlantillaTicket NVARCHAR(MAX) NULL,      -- plantilla ESC/POS o texto con placeholders
    Activo          BIT NOT NULL DEFAULT 1
);
GO

/* ------------------------------ Auditoría --------------------------------- */

CREATE TABLE Auditoria (
    AuditoriaId     BIGINT IDENTITY(1,1) PRIMARY KEY,
    UsuarioId       INT NULL REFERENCES Usuarios(UsuarioId),
    Accion          NVARCHAR(100) NOT NULL,     -- LOGIN, LLAMAR_TURNO, FINALIZAR, CONFIG_CAMBIO...
    Entidad         NVARCHAR(100) NULL,
    EntidadId       NVARCHAR(50) NULL,
    DireccionIp     NVARCHAR(50) NULL,
    DatosAntes      NVARCHAR(MAX) NULL,
    DatosDespues    NVARCHAR(MAX) NULL,
    FechaEvento     DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

CREATE TABLE LogsEventos (
    LogId           BIGINT IDENTITY(1,1) PRIMARY KEY,
    Nivel           NVARCHAR(20) NOT NULL,      -- INFO, WARNING, ERROR
    Origen          NVARCHAR(100) NOT NULL,     -- Impresora, Hub, Api
    Mensaje         NVARCHAR(1000) NOT NULL,
    Detalle         NVARCHAR(MAX) NULL,
    FechaEvento     DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

/* ------------------------------ Datos base --------------------------------- */

INSERT INTO Roles (Nombre, Descripcion) VALUES
 ('Administrador', 'Acceso total al sistema'),
 ('Supervisor', 'Monitoreo de colas y reportes'),
 ('Agente', 'Atención de turnos en ventanilla');

INSERT INTO EstadosTicket (Codigo, Nombre) VALUES
 ('ESPERANDO', 'Esperando'),
 ('EN_ATENCION', 'En atención'),
 ('ATENDIDO', 'Atendido'),
 ('CANCELADO', 'Cancelado'),
 ('AUSENTE', 'Ausente'),
 ('TRANSFERIDO', 'Transferido');

INSERT INTO Sucursales (Codigo, Nombre, Ciudad) VALUES ('SUC-001', 'Sucursal Principal', 'Ciudad');

INSERT INTO Servicios (SucursalId, Nombre, Prefijo, Orden, SlaMinutos) VALUES
 (1, 'Ventas', 'A', 1, 10),
 (1, 'Servicio al Cliente', 'B', 2, 8),
 (1, 'Soporte Técnico', 'C', 3, 15),
 (1, 'Facturación', 'D', 4, 8),
 (1, 'Reclamos', 'E', 5, 12);

INSERT INTO Ventanillas (SucursalId, Numero, Nombre) VALUES
 (1,1,'Ventanilla 1'), (1,2,'Ventanilla 2'), (1,3,'Ventanilla 3'), (1,4,'Ventanilla 4');
GO
