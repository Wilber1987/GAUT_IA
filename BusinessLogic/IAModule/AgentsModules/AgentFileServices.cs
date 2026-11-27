using CAPA_NEGOCIO;
using CAPA_NEGOCIO.MAPEO;
using APPCORE;
using APPCORE.Services;

namespace BusinessLogic.IA.Agent
{
    public class UploadFileRequest
    {
        public string? Token { get; set; }
        public string? UserId { get; set; }
        public ModelFiles? Attach { get; set; }
    }

    public class ExcludeFileRequest
    {
        public string? Token { get; set; }
        public string? UserId { get; set; }
        public int? Id_File { get; set; }
        public bool Excluded { get; set; }
    }

    public static class AgentFileServices
    {
        public static object List(string? token, string? userId)
            => AgentWorkspaceService.ForToken(token, userId).ToDto(includeFiles: true);

        public static object Upload(UploadFileRequest req)
        {
            var ws = AgentWorkspaceService.ForToken(req.Token, req.UserId);
            var attach = req.Attach ?? throw new AgentValidationException("Adjunto requerido.");

            var name = (attach.Name ?? "archivo.txt").Replace('\\', '/').TrimStart('/');
            if (Path.IsPathRooted(name) || name.Split('/').Contains("..") || name.Contains("//"))
                throw new AgentValidationException("Nombre/ruta de archivo inválido.");
            if (name.Split('/').Any(s => s.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 || s.Length == 0))
                throw new AgentValidationException("Nombre de archivo inválido.");

            var relative = "user/" + name;   // los uploads viven bajo user/ (prefijo reservado, el agente no escribe ahí)

            var base64 = attach.Value ?? throw new AgentValidationException("Contenido vacío.");
            var marker = base64.IndexOf("base64,", StringComparison.OrdinalIgnoreCase);
            if (marker >= 0) base64 = base64[(marker + 7)..];

            byte[] bytes;
            try { bytes = Convert.FromBase64String(base64); }
            catch { throw new AgentValidationException("El contenido no es base64 válido."); }

            if (bytes.LongLength > AgentSettings.Default.MaxFileBytes)
                throw new AgentValidationException($"Archivo excede {AgentSettings.Default.MaxFileBytes / 1024} KB.");

            var existing = ws.GetFiles(includeExcluded: true).FirstOrDefault(f =>
                string.Equals(f.RelativePath, relative, StringComparison.OrdinalIgnoreCase));
            if (existing?.Origin == "AGENT")
                relative = $"user/{Path.GetFileNameWithoutExtension(name)}_u{Path.GetExtension(name)}";

            var full = ws.ResolveSafePath(relative);
            Directory.CreateDirectory(Path.GetDirectoryName(full)!);
            var tmp = full + ".tmp";
            File.WriteAllBytes(tmp, bytes);
            File.Move(tmp, full, overwrite: true);

            ws.RegisterFile(relative, "USER", bytes.LongLength);
            return ws.ToDto(includeFiles: true);
        }

        public static object SetExcluded(ExcludeFileRequest req)
        {
            var ws = AgentWorkspaceService.ForToken(req.Token, req.UserId);
            var file = new Tbl_Agent_File().Find<Tbl_Agent_File>(FilterData.Equal("Id_File", req.Id_File));
            if (file == null || file.Id_Workspace != ws.Workspace.Id_Workspace)
                throw new AgentValidationException("Archivo no encontrado en este workspace.");

            file.Excluded = req.Excluded;
            file.Update();
            return ws.ToDto(includeFiles: true);
        }
    }
}