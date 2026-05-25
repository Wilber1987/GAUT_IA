-- DROP SCHEMA administrative_access;

CREATE SCHEMA administrative_access;
-- administrative_access.Transactional_Configuraciones definition

-- Drop table

-- DROP TABLE administrative_access.Transactional_Configuraciones;

CREATE TABLE administrative_access.Transactional_Configuraciones (
	Id_Configuracion int IDENTITY(1,1) NOT NULL,
	Nombre nvarchar(50) COLLATE Modern_Spanish_CI_AS NULL,
	Descripcion nvarchar(250) COLLATE Modern_Spanish_CI_AS NULL,
	Valor nvarchar(MAX) COLLATE Modern_Spanish_CI_AS NULL,
	Tipo_Configuracion varchar(100) COLLATE Modern_Spanish_CI_AS NULL,
	CONSTRAINT PK_Transactional_Configuraciones PRIMARY KEY (Id_Configuracion),
	CONSTRAINT Transactional_Configuraciones_UN UNIQUE (Nombre)
);

-- DROP SCHEMA [security];

CREATE SCHEMA [security];
-- [security].Security_Permissions definition

-- Drop table

-- DROP TABLE [security].Security_Permissions;

CREATE TABLE [security].Security_Permissions (
	Id_Permission int IDENTITY(1,1) NOT NULL,
	Descripcion nvarchar(100) COLLATE Modern_Spanish_CI_AS NULL,
	Estado nvarchar(50) COLLATE Modern_Spanish_CI_AS NULL,
	Detalles nvarchar(500) COLLATE Modern_Spanish_CI_AS NULL,
	CONSTRAINT PK_Security_Permissions PRIMARY KEY (Id_Permission)
);


-- [security].Security_Roles definition

-- Drop table

-- DROP TABLE [security].Security_Roles;

CREATE TABLE [security].Security_Roles (
	Id_Role int IDENTITY(1,1) NOT NULL,
	Descripcion nvarchar(150) COLLATE Modern_Spanish_CI_AS NULL,
	Estado nvarchar(50) COLLATE Modern_Spanish_CI_AS NULL,
	CONSTRAINT PK_Security_Roles PRIMARY KEY (Id_Role)
);


-- [security].Security_Users definition

-- Drop table

-- DROP TABLE [security].Security_Users;

CREATE TABLE [security].Security_Users (
	Id_User int IDENTITY(1,1) NOT NULL,
	Nombres nvarchar(150) COLLATE Modern_Spanish_CI_AS NULL,
	Estado nvarchar(50) COLLATE Modern_Spanish_CI_AS NULL,
	Descripcion nvarchar(500) COLLATE Modern_Spanish_CI_AS NULL,
	Password nvarchar(500) COLLATE Modern_Spanish_CI_AS NULL,
	Mail nvarchar(150) COLLATE Modern_Spanish_CI_AS NULL,
	Token nvarchar(500) COLLATE Modern_Spanish_CI_AS NULL,
	Token_Date datetime NULL,
	Token_Expiration_Date datetime NULL,
	Nombres_Completo nvarchar(500) COLLATE Modern_Spanish_CI_AS NULL,
	CONSTRAINT PK_Security_Users PRIMARY KEY (Id_User)
);


-- [security].Security_Permissions_Roles definition

-- Drop table

-- DROP TABLE [security].Security_Permissions_Roles;

CREATE TABLE [security].Security_Permissions_Roles (
	Id_Role int NOT NULL,
	Id_Permission int NOT NULL,
	Estado nvarchar(50) COLLATE Modern_Spanish_CI_AS NULL,
	CONSTRAINT PK_Security_Permissions_Roles PRIMARY KEY (Id_Role,Id_Permission),
	CONSTRAINT FK_Security_Permissions_Roles_Security_Permissions FOREIGN KEY (Id_Permission) REFERENCES [security].Security_Permissions(Id_Permission),
	CONSTRAINT FK_Security_Permissions_Roles_Security_Roles FOREIGN KEY (Id_Role) REFERENCES [security].Security_Roles(Id_Role)
);


-- [security].Security_Users_Roles definition

-- Drop table

-- DROP TABLE [security].Security_Users_Roles;

CREATE TABLE [security].Security_Users_Roles (
	Id_Role int NOT NULL,
	Id_User int NOT NULL,
	Estado nvarchar(50) COLLATE Modern_Spanish_CI_AS NULL,
	CONSTRAINT PK_Security_Users_Roles PRIMARY KEY (Id_Role,Id_User),
	CONSTRAINT FK_Security_Users_Roles_Security_Roles FOREIGN KEY (Id_Role) REFERENCES [security].Security_Roles(Id_Role),
	CONSTRAINT FK_Security_Users_Roles_Security_Users FOREIGN KEY (Id_User) REFERENCES [security].Security_Users(Id_User)
);


-- [security].Tbl_Profile definition

-- Drop table

-- DROP TABLE [security].Tbl_Profile;

CREATE TABLE [security].Tbl_Profile (
	Id_Perfil int IDENTITY(1,1) NOT NULL,
	Nombres nvarchar(300) COLLATE Modern_Spanish_CI_AS NULL,
	Apellidos nvarchar(300) COLLATE Modern_Spanish_CI_AS NULL,
	FechaNac date NULL,
	IdUser int NULL,
	Sexo nvarchar(50) COLLATE Modern_Spanish_CI_AS NULL,
	Foto nvarchar(MAX) COLLATE Modern_Spanish_CI_AS NULL,
	DNI nvarchar(50) COLLATE Modern_Spanish_CI_AS NULL,
	Correo_institucional nvarchar(300) COLLATE Modern_Spanish_CI_AS NULL,
	Id_Pais_Origen int NULL,
	Estado nvarchar(50) COLLATE Modern_Spanish_CI_AS NULL,
	ORCID nvarchar(500) COLLATE Modern_Spanish_CI_AS NULL,
	CONSTRAINT PK_Tbl_InvestigatorProfile PRIMARY KEY (Id_Perfil)
);


-- [security].Tbl_Profile foreign keys

ALTER TABLE [security].Tbl_Profile ADD CONSTRAINT FK_Tbl_InvestigatorProfile_Security_Users FOREIGN KEY (IdUser) REFERENCES [security].Security_Users(Id_User);

CREATE TABLE administrative_access.Log (
	Id_Log int IDENTITY(1,1) NOT NULL,
	LogType nvarchar(MAX) COLLATE Latin1_General_CI_AI NULL,
	Fecha datetime NULL,
	message varchar(MAX) COLLATE Modern_Spanish_CI_AS NULL,
	body nvarchar(MAX) COLLATE Modern_Spanish_CI_AS NULL,
	Id_User int NULL,
	CONSTRAINT PK_CaseTable_Agendas PRIMARY KEY (Id_Log)
);

--DATOS INICIALES

-- dbo.Log definition

-- Drop table

-- DROP TABLE dbo.Log;

-- helpdesk.Cat_Cargos_Dependencias definition

-- Drop table

-- DROP TABLE helpdesk.Cat_Cargos_Dependencias;

CREATE TABLE security.Cat_Cargos_Dependencias (
	Id_Cargo int IDENTITY(1,1) NOT NULL,
	Descripcion nvarchar(50) COLLATE Modern_Spanish_CI_AS NULL,
	CONSTRAINT PK_Cat_Cargos_Dependencias PRIMARY KEY (Id_Cargo)
);


-- helpdesk.Cat_Paises definition

-- Drop table

-- DROP TABLE helpdesk.Cat_Paises;

CREATE TABLE security.Cat_Paises (
	Id_Pais int IDENTITY(1,1) NOT NULL,
	Estado nvarchar(50) COLLATE Modern_Spanish_CI_AS NULL,
	Descripcion nvarchar(50) COLLATE Modern_Spanish_CI_AS NULL,
	CONSTRAINT PK_Cat_Nacionalidad PRIMARY KEY (Id_Pais)
);


-- notificaciones_mensajeria.conversacion definition

-- Drop table

-- DROP TABLE notificaciones_mensajeria.conversacion;
CREATE schema notificaciones_mensajeria

CREATE TABLE notificaciones_mensajeria.conversacion (
	id_conversacion int IDENTITY(1,1) NOT NULL,
	descripcion nvarchar(255) COLLATE Modern_Spanish_CI_AS NULL,
	CONSTRAINT PK__conversa__DA68168748946A7A PRIMARY KEY (id_conversacion)
);


-- notificaciones_mensajeria.notificaciones definition

-- Drop table

-- DROP TABLE notificaciones_mensajeria.notificaciones;

CREATE TABLE notificaciones_mensajeria.notificaciones (
	Id int IDENTITY(1,1) NOT NULL,
	Id_User int NULL,
	Titulo nvarchar(MAX) COLLATE Modern_Spanish_CI_AS NULL,
	Mensaje nvarchar(MAX) COLLATE Modern_Spanish_CI_AS NULL,
	Fecha datetime NULL,
	Media nvarchar(MAX) COLLATE Modern_Spanish_CI_AS NULL,
	Tipo nvarchar(50) COLLATE Modern_Spanish_CI_AS NULL,
	Estado nvarchar(50) COLLATE Modern_Spanish_CI_AS NULL,
	Enviado bit NULL,
	Leido bit NULL,
	Telefono nvarchar(20) COLLATE Modern_Spanish_CI_AS NULL,
	Email nvarchar(50) COLLATE Modern_Spanish_CI_AS NULL,
	NotificationsServices nvarchar(MAX) COLLATE Modern_Spanish_CI_AS NULL,
	NotificationData nvarchar(MAX) COLLATE Modern_Spanish_CI_AS NULL,
	fecha_envio datetime NULL,
	CONSTRAINT PK_notificaciones PRIMARY KEY (Id)
);


-- helpdesk.Cat_Dependencias definition

-- Drop table

-- DROP TABLE helpdesk.Cat_Dependencias;

CREATE TABLE security.Cat_Dependencias (
	Id_Dependencia int IDENTITY(1,1) NOT NULL,
	Descripcion nvarchar(50) COLLATE Modern_Spanish_CI_AS NULL,
	Id_Dependencia_Padre int NULL,
	Username varchar(100) COLLATE Modern_Spanish_CI_AS NULL,
	Password varchar(100) COLLATE Modern_Spanish_CI_AS NULL,
	Host varchar(100) COLLATE Modern_Spanish_CI_AS NULL,
	AutenticationType varchar(MAX) COLLATE Modern_Spanish_CI_AS NULL,
	TENAT varchar(MAX) COLLATE Modern_Spanish_CI_AS NULL,
	CLIENT varchar(MAX) COLLATE Modern_Spanish_CI_AS NULL,
	OBJECTID varchar(MAX) COLLATE Modern_Spanish_CI_AS NULL,
	CLIENT_SECRET varchar(MAX) COLLATE Modern_Spanish_CI_AS NULL,
	HostService varchar(MAX) COLLATE Modern_Spanish_CI_AS NULL,
	SMTPHOST varchar(500) COLLATE Modern_Spanish_CI_AS NULL,
	DefaultDependency bit NULL,
	CONSTRAINT PK_Cat_Dependencias PRIMARY KEY (Id_Dependencia),
	CONSTRAINT FK_Cat_Dependencias_Cat_Dependencias FOREIGN KEY (Id_Dependencia_Padre) REFERENCES security.Cat_Dependencias(Id_Dependencia)
);


-- notificaciones_mensajeria.conversacion_usuarios definition

-- Drop table

-- DROP TABLE notificaciones_mensajeria.conversacion_usuarios;

CREATE TABLE notificaciones_mensajeria.conversacion_usuarios (
	id_conversacion int NOT NULL,
	id_usuario int NOT NULL,
	avatar nvarchar(250) COLLATE Modern_Spanish_CI_AS NULL,
	name nvarchar(250) COLLATE Modern_Spanish_CI_AS NULL,
	CONSTRAINT conversacion_usuarios_PK PRIMARY KEY (id_conversacion,id_usuario),
	CONSTRAINT conversacion_usuarios_FK FOREIGN KEY (id_conversacion) REFERENCES notificaciones_mensajeria.conversacion(id_conversacion)
);


-- notificaciones_mensajeria.mensajes definition

-- Drop table

-- DROP TABLE notificaciones_mensajeria.mensajes;

CREATE TABLE notificaciones_mensajeria.mensajes (
	id_mensaje int IDENTITY(1,1) NOT NULL,
	remitente nvarchar(255) COLLATE Modern_Spanish_CI_AS NULL,
	usuario_id int NULL,
	destinatarios nvarchar(MAX) COLLATE Modern_Spanish_CI_AS NULL,
	asunto nvarchar(255) COLLATE Modern_Spanish_CI_AS NULL,
	body nvarchar(MAX) COLLATE Modern_Spanish_CI_AS NULL,
	id_conversacion int NULL,
	created_at datetime NOT NULL,
	updated_at datetime NOT NULL,
	enviado bit NULL,
	leido bit NULL,
	Foto nvarchar(MAX) COLLATE Modern_Spanish_CI_AS NULL,
	CONSTRAINT PK__mensajes__5B37C7F643702D9B PRIMARY KEY (id_mensaje),
	CONSTRAINT FK_mensajes_conversacion FOREIGN KEY (id_conversacion) REFERENCES notificaciones_mensajeria.conversacion(id_conversacion)
);


-- notificaciones_mensajeria.mensaje_adjuntos definition

-- Drop table

-- DROP TABLE notificaciones_mensajeria.mensaje_adjuntos;

CREATE TABLE notificaciones_mensajeria.mensaje_adjuntos (
	id int NOT NULL,
	mensaje_id int NULL,
	archivo nvarchar(255) COLLATE Modern_Spanish_CI_AS NULL,
	created_at datetime NOT NULL,
	updated_at datetime NOT NULL,
	CONSTRAINT PK__mensaje___3213E83FA9F043E9 PRIMARY KEY (id),
	CONSTRAINT FK_mensaje_adjuntos_mensajes FOREIGN KEY (mensaje_id) REFERENCES notificaciones_mensajeria.mensajes(id_mensaje)
);
CREATE NONCLUSTERED INDEX index_mensaje_adjuntos_on_mensaje_id ON notificaciones_mensajeria.mensaje_adjuntos (mensaje_id);


-- helpdesk.Tbl_Servicios definition

-- Drop table

-- DROP TABLE helpdesk.Tbl_Servicios;

CREATE TABLE security.Tbl_Servicios (
	Id_Servicio int IDENTITY(1,1) NOT NULL,
	Nombre_Servicio nvarchar(250) COLLATE Modern_Spanish_CI_AS NULL,
	Descripcion_Servicio nvarchar(2000) COLLATE Modern_Spanish_CI_AS NULL,
	Id_Tipo_Servicio int NULL,
	Visibilidad nvarchar(50) COLLATE Modern_Spanish_CI_AS NULL,
	Estado nvarchar(50) COLLATE Modern_Spanish_CI_AS NULL,
	Fecha_Inicio date NULL,
	Fecha_Finalizacion date NULL,
	Id_Dependencia int NULL,
	CONSTRAINT PK_Tbl_Servicios PRIMARY KEY (Id_Servicio)
);


-- security.Tbl_Dependencias_Usuarios definition

-- Drop table

-- DROP TABLE security.Tbl_Dependencias_Usuarios;

CREATE TABLE security.Tbl_Dependencias_Usuarios (
	Id_Perfil int NOT NULL,
	Id_Dependencia int NOT NULL,
	Id_Cargo int NULL,
	CONSTRAINT PK_Tbl_Dependencias_Usuarios PRIMARY KEY (Id_Perfil,Id_Dependencia)
);


-- security.Tbl_Dependencias_Usuarios foreign keys

ALTER TABLE security.Tbl_Dependencias_Usuarios ADD CONSTRAINT FK_Tbl_Dependencias_Usuarios_Cat_Cargos_Dependencias FOREIGN KEY (Id_Cargo) REFERENCES security.Cat_Cargos_Dependencias(Id_Cargo);
ALTER TABLE security.Tbl_Dependencias_Usuarios ADD CONSTRAINT FK_Tbl_Dependencias_Usuarios_Cat_Dependencias FOREIGN KEY (Id_Dependencia) REFERENCES security.Cat_Dependencias(Id_Dependencia);
ALTER TABLE security.Tbl_Dependencias_Usuarios ADD CONSTRAINT FK_Tbl_Dependencias_Usuarios_Tbl_InvestigatorProfile FOREIGN KEY (Id_Perfil) REFERENCES [security].Tbl_Profile(Id_Perfil);

-- helpdesk.Tbl_Servicios foreign keys

ALTER TABLE security.Tbl_Servicios ADD CONSTRAINT FK_Tbl_Servicios_Cat_Dependencias FOREIGN KEY (Id_Dependencia) REFERENCES security.Cat_Dependencias(Id_Dependencia);