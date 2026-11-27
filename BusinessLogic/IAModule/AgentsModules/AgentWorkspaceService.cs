using System.Text;
using APPCORE;
using CAPA_NEGOCIO.MAPEO;
using CAPA_NEGOCIO.SystemConfig;

namespace BusinessLogic.IA.Agent
{
    public class AgentWorkspaceService
    {
        public Tbl_Agent_Workspace Workspace { get; set; } = null!;
        public string Root { get; set; } = null!;

        public static AgentWorkspaceService ForToken(string? token, string? userId)
        {
            if (string.IsNullOrWhiteSpace(token)) throw new AgentValidationException("Token requerido.");
            if (string.IsNullOrWhiteSpace(userId)) throw new AgentValidationException("UserId requerido.");

            var ws = new Tbl_Agent_Workspace().Find<Tbl_Agent_Workspace>(FilterData.Equal("Token", token));
            if (ws == null)
            {
                var root = Path.Combine(SystemConfigImpl.GetMediaAttachPath(), "agent_workspaces", token);
                ws = new Tbl_Agent_Workspace
                {
                    Token = token,
                    UserId = userId,
                    RootPath = root,
                    CreatedAt = DateTime.Now
                }.Save() as Tbl_Agent_Workspace;
            }
            else if (ws.UserId != userId)
                throw new AgentValidationException("El workspace no pertenece al usuario.");

            Directory.CreateDirectory(ws!.RootPath!); // si la carpeta física desapareció, se recrea
            return new AgentWorkspaceService { Workspace = ws, Root = ws.RootPath! };
        }

        public void BindFolder(string? path)
        {
            if (!AgentSettings.Default.AllowCustomRoots)
                throw new AgentValidationException("Este servidor no permite rutas personalizadas.");
            if (string.IsNullOrWhiteSpace(path))
                throw new AgentValidationException("Ruta requerida.");

            string full;
            try { full = Path.GetFullPath(path.Trim().Trim('"')); }
            catch { throw new AgentValidationException("Ruta inválida."); }

            if (!Directory.Exists(full))
                throw new AgentValidationException("La carpeta no existe en el servidor.");
            if (string.Equals(full, Path.GetPathRoot(full), StringComparison.OrdinalIgnoreCase))
                throw new AgentValidationException("No se permite la raíz de una unidad (ej. C:\\).");

            foreach (var f in GetFiles(includeExcluded: true)) f.Delete(); // ajusta al método Delete de tu ORM si difiere

            Workspace.RootPath = full;
            Workspace.Update();
            Root = full;
            ScanAndRegister();
        }

        public int ScanAndRegister()
        {
            int count = 0;
            Walk(Root, 0);
            return count;

            void Walk(string dir, int depth)
            {
                if (depth > AgentSettings.Default.MaxImportDepth || count >= AgentSettings.Default.MaxFilesPerWorkspace) return;
                foreach (var file in Directory.GetFiles(dir))
                {
                    if (count >= AgentSettings.Default.MaxFilesPerWorkspace) return;
                    var ext = Path.GetExtension(file).ToLowerInvariant();
                    if (!AgentSettings.Default.AllowedExtensions.Contains(ext)) continue;
                    var info = new FileInfo(file);
                    if (info.Length > AgentSettings.Default.MaxFileBytes || info.Name.EndsWith(".tmp")) continue;

                    var rel = Path.GetRelativePath(Root, file).Replace('\\', '/');
                    RegisterFile(rel, "USER", info.Length);
                    count++;
                }
                foreach (var sub in Directory.GetDirectories(dir))
                {
                    var info = new DirectoryInfo(sub);
                    if (AgentSettings.Default.IgnoredFolders.Contains(info.Name, StringComparer.OrdinalIgnoreCase)) continue;
                    if (info.Attributes.HasFlag(FileAttributes.ReparsePoint)) continue; // no seguir symlinks
                    Walk(sub, depth + 1);
                }
            }
        }
        public static Tbl_Agent_Workspace GetOwnedById(int? id, string? userId)
        {
            var ws = new Tbl_Agent_Workspace().Find<Tbl_Agent_Workspace>(FilterData.Equal("Id_Workspace", id));
            if (ws == null || ws.UserId != userId)
                throw new AgentValidationException("Workspace no encontrado o no autorizado.");
            return ws;
        }

        public static List<object> ListForUser(string? userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) throw new AgentValidationException("UserId requerido.");
            var workspaces = new Tbl_Agent_Workspace().Where<Tbl_Agent_Workspace>(FilterData.Equal("UserId", userId));
            return workspaces.OrderByDescending(w => w.CreatedAt).Select(w =>
            {
                var files = new Tbl_Agent_File().Where<Tbl_Agent_File>(FilterData.Equal("Id_Workspace", w.Id_Workspace));
                return (object)new
                {
                    w.Id_Workspace,
                    w.Token,
                    w.CreatedAt,
                    UserFiles = files.Count(f => f.Origin == "USER"),
                    AgentFiles = files.Count(f => f.Origin == "AGENT")
                };
            }).ToList();
        }

        public string ResolveSafePath(string? relative)
        {
            if (string.IsNullOrWhiteSpace(relative)) throw new AgentValidationException("Ruta vacía.");

            // 1. Normalizar separadores y quitar leading slashes/backslashes.
            //    "/user/x.md" → "user/x.md" ; "\\user\\x.md" → "user/x.md"
            var normalized = relative.Replace('\\', '/').TrimStart('/');

            // 2. Rechazar '..' ANTES de cualquier Path.* (Path.Combine no escapa, pero GetFullPath sí)
            if (normalized.Split('/').Any(seg => seg == ".."))
                throw new AgentValidationException("La ruta no puede contener '..'.");

            // 3. Rechazar unidad Windows explícita ("C:...", "\\\\server\\share")
            if (normalized.Length >= 2 && char.IsLetter(normalized[0]) && normalized[1] == ':')
                throw new AgentValidationException("No se permiten rutas con unidad de disco.");
            if (normalized.StartsWith("//", StringComparison.Ordinal))
                throw new AgentValidationException("No se permiten rutas UNC.");

            var ext = Path.GetExtension(normalized).ToLowerInvariant();
            if (!AgentSettings.Default.AllowedExtensions.Contains(ext))
                throw new AgentValidationException($"Extensión '{ext}' no permitida. Usa una de: {string.Join(", ", AgentSettings.Default.AllowedExtensions)}");

            // 4. Defensa final: el path absoluto resuelto debe estar dentro de Root
            var rootFull = Path.GetFullPath(Root) + Path.DirectorySeparatorChar;
            var full = Path.GetFullPath(Path.Combine(Root, normalized));
            if (!full.StartsWith(rootFull, StringComparison.OrdinalIgnoreCase))
                throw new AgentValidationException("Ruta fuera del workspace.");
            return full;
        }
   
        public List<Tbl_Agent_File> GetFiles(bool includeExcluded = false)
            => new Tbl_Agent_File().Where<Tbl_Agent_File>(FilterData.Equal("Id_Workspace", Workspace.Id_Workspace))
                .Where(f => includeExcluded || !f.Excluded).ToList();

        public Tbl_Agent_File? FindFile(string relative)
            => GetFiles().FirstOrDefault(f =>
                string.Equals(f.RelativePath, relative.Replace('\\', '/'), StringComparison.OrdinalIgnoreCase));

        public void RegisterFile(string relative, string origin, long bytes, bool excluded = false)
        {
            relative = relative.Replace('\\', '/');
            var existing = GetFiles(includeExcluded: true).FirstOrDefault(f =>
                string.Equals(f.RelativePath, relative, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                existing.SizeBytes = bytes;
                existing.UpdatedAt = DateTime.Now;
                existing.Update();
                return;
            }
            if (GetFiles(includeExcluded: true).Count >= AgentSettings.Default.MaxFilesPerWorkspace)
                throw new AgentValidationException("Límite de archivos del workspace alcanzado.");

            new Tbl_Agent_File
            {
                Id_Workspace = Workspace.Id_Workspace,
                RelativePath = relative,
                Origin = origin,
                Excluded = excluded,
                SizeBytes = bytes,
                UpdatedAt = DateTime.Now
            }.Save();
        }

        public string BuildSnapshot()
        {
            var files = GetFiles();
            var sb = new StringBuilder();
            sb.AppendLine("Archivos de usuario (solo lectura, no los modifiques):");
            sb.AppendLine(string.Join("\n",
                files.Where(f => f.Origin == "USER")
                     .Select(f => $"- \"{f.RelativePath}\" ({f.SizeBytes} B)")
                     .DefaultIfEmpty("(ninguno)")));
            sb.AppendLine();
            sb.AppendLine("Archivos que YA EXISTEN en el workspace (creados por ti en turnos previos).");
            sb.AppendLine("NO los reescribas si no te lo piden. Solo crea los NUEVOS archivos que falten.");
            sb.AppendLine(string.Join("\n",
                files.Where(f => f.Origin == "AGENT")
                     .Select(f => $"- \"{f.RelativePath}\" ({f.SizeBytes} B)")
                     .DefaultIfEmpty("(ninguno)")));
            return sb.ToString();
        }

        public object ToDto(bool includeFiles)
        {
            var files = includeFiles ? GetFiles(includeExcluded: true) : new List<Tbl_Agent_File>();
            return new
            {
                Workspace.Id_Workspace,
                Workspace.Token,
                Workspace.RootPath,
                Workspace.CreatedAt,
                UserFiles = files.Where(f => f.Origin == "USER").Select(FileDto).ToList(),
                AgentFiles = files.Where(f => f.Origin == "AGENT").Select(FileDto).ToList()
            };
        }

        public static object FileDto(Tbl_Agent_File f)
            => new { f.Id_File, f.RelativePath, f.Origin, f.Excluded, f.SizeBytes, f.UpdatedAt };

        public int ImportFromRoot(string rootKey, string? subPath)
        {
            if (!AgentSettings.Default.ImportRoots.TryGetValue(rootKey, out var configuredRoot))
                throw new AgentValidationException("Raíz de importación no permitida.");

            var baseFull = Path.GetFullPath(configuredRoot);
            var start = Path.GetFullPath(Path.Combine(baseFull, subPath ?? ""));
            if (start != baseFull && !start.StartsWith(baseFull + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                throw new AgentValidationException("Subruta fuera de la raíz permitida.");
            if (!Directory.Exists(start))
                throw new AgentValidationException("La carpeta no existe en el servidor.");

            int imported = 0;
            Walk(start, 0);
            return imported;

            void Walk(string dir, int depth)
            {
                if (depth > AgentSettings.Default.MaxImportDepth) return;
                foreach (var file in Directory.GetFiles(dir))
                {
                    var ext = Path.GetExtension(file).ToLowerInvariant();
                    if (!AgentSettings.Default.AllowedExtensions.Contains(ext)) continue;
                    var info = new FileInfo(file);
                    if (info.Length > AgentSettings.Default.MaxFileBytes) continue;

                    var rel = Path.GetRelativePath(start, file).Replace('\\', '/');
                    var target = ResolveSafePath("user/" + rel);
                    Directory.CreateDirectory(Path.GetDirectoryName(target)!);
                    File.Copy(file, target, overwrite: true);
                    RegisterFile("user/" + rel, "USER", info.Length);
                    imported++;
                }
                foreach (var sub in Directory.GetDirectories(dir))
                {
                    var name = Path.GetFileName(sub);
                    if (AgentSettings.Default.IgnoredFolders.Contains(name, StringComparer.OrdinalIgnoreCase)) continue;
                    Walk(sub, depth + 1);
                }
            }
        }

        public void CopyFrom(Tbl_Agent_Workspace source)
        {
            if (source.RootPath == null || !Directory.Exists(source.RootPath))
                throw new AgentValidationException("El workspace origen no tiene archivos físicos.");

            var sourceFiles = new Tbl_Agent_File().Where<Tbl_Agent_File>(FilterData.Equal("Id_Workspace", source.Id_Workspace));
            foreach (var f in sourceFiles)
            {
                var from = Path.Combine(source.RootPath, f.RelativePath ?? "");
                if (!File.Exists(from)) continue;
                var to = ResolveSafePath(f.RelativePath);
                Directory.CreateDirectory(Path.GetDirectoryName(to)!);
                File.Copy(from, to, overwrite: true);
                RegisterFile(f.RelativePath!, f.Origin ?? "USER", new FileInfo(from).Length, f.Excluded);
            }
        }
    }

    public class SelectWorkspaceRequest
    {
        public string? Token { get; set; }
        public string? UserId { get; set; }
        public int? Id_Workspace { get; set; }
        public string? ImportRoot { get; set; }
        public string? ImportSubPath { get; set; }
    }

    public static class AgentWorkspaceSelector
    {
        public static object Select(SelectWorkspaceRequest req)
        {
            var ws = AgentWorkspaceService.ForToken(req.Token, req.UserId);
            if (req.Id_Workspace.HasValue)
                ws.CopyFrom(AgentWorkspaceService.GetOwnedById(req.Id_Workspace, req.UserId));
            if (!string.IsNullOrWhiteSpace(req.ImportRoot))
                ws.ImportFromRoot(req.ImportRoot, req.ImportSubPath);
            return ws.ToDto(includeFiles: true);
        }
    }
}