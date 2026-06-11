CREATE schema ia_module CREATE TABLE ia_module.Tbl_Knowledge_Base (
    Id_Knowledge INT IDENTITY(1, 1) PRIMARY KEY,
    ServiceTag NVARCHAR(100),
    -- Tu ServicesIdentification
    ServiceTagId int,
    Content_Text NVARCHAR(MAX),
    Vector_Data VARBINARY(MAX),
    -- Almacenará el float[] convertido a bytes
    CreatedAt DATETIME DEFAULT GETDATE(),
    Category NVARCHAR(MAX),
    SourceKey NVARCHAR(MAX),
    SearchText NVARCHAR(MAX),
);

-- Indice para filtrar por departamento antes de comparar vectores
CREATE INDEX IX_Knowledge_Service ON ia_module.Tbl_Knowledge_Base(ServiceTag);