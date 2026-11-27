using System.Text.Json;

namespace BusinessLogic.IA.Agent
{
    public class AgentValidationException : Exception
    {
        public AgentValidationException(string message) : base(message) { }
    }

    public class AgentSettings
    {
        public Dictionary<string, string> ImportRoots { get; set; } = new();
        public string[] AllowedExtensions { get; set; } =
            [".js", ".ts", ".cs", ".json", ".md", ".txt", ".html", ".css", ".sql", ".csv", ".xml", ".py", ".jpg", ".jpeg", ".png"];
        public long MaxFileBytes { get; set; } = 256 * 1024;
        public int MaxFilesPerWorkspace { get; set; } = 200;
        public int MaxImportDepth { get; set; } = 4;
        public string[] IgnoredFolders { get; set; } = ["bin", "obj", "node_modules", ".git"];
        public bool AllowCustomRoots { get; set; } = true;

        public static AgentSettings Default { get; } = Load();

        private static AgentSettings Load()
        {
            var path = Path.Combine(AppContext.BaseDirectory, "AgentConfig", "agent_settings.json");
            if (!File.Exists(path)) return new AgentSettings();
            return JsonSerializer.Deserialize<AgentSettings>(File.ReadAllText(path),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new AgentSettings();
        }
    }
}