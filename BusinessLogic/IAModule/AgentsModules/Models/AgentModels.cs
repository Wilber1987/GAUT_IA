using APPCORE;

namespace CAPA_NEGOCIO.MAPEO
{
    public class Tbl_Agent_Workspace : EntityClass
    {
        [PrimaryKey(Identity = true)]
        public int? Id_Workspace { get; set; }
        public string? Token { get; set; }
        public string? UserId { get; set; }
        public string? RootPath { get; set; }
        public string? Resumen { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

    public class Tbl_Agent_File : EntityClass
    {
        [PrimaryKey(Identity = true)]
        public int? Id_File { get; set; }
        public int? Id_Workspace { get; set; }
        public string? RelativePath { get; set; }
        public string? Origin { get; set; }        // USER | AGENT
        public bool Excluded { get; set; } = false;
        public long? SizeBytes { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class Tbl_Agent_Run : EntityClass
    {
        [PrimaryKey(Identity = true)]
        public int? Id_Run { get; set; }
        public string? Token { get; set; }
        public string? Skill { get; set; }
        public string? Status { get; set; }
        public int Steps { get; set; }
        public string? Trace { get; set; }
        public string? Error { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
    }
    public class GeneratedFile
    {
        public string Path { get; set; } = "";

        public string Action { get; set; } = "create_file";

        public string Content { get; set; } = "";
    }
}