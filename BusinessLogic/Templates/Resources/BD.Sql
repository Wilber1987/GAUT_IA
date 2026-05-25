-- OLIMPO.administrative_access.TemplateData definition

-- Drop table

-- DROP TABLE OLIMPO.administrative_access.TemplateData;

CREATE TABLE administrative_access.TemplateData (
	Id_Template int IDENTITY(0,1) NOT NULL,
	Descripcion nvarchar(MAX) COLLATE Modern_Spanish_CI_AS NULL,
	Sections nvarchar(MAX) COLLATE Modern_Spanish_CI_AS NULL,
	CONSTRAINT TemplateData_PK PRIMARY KEY (Id_Template)
);