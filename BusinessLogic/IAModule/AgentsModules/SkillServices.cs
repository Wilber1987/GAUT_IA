using System.Text.Json;

namespace BusinessLogic.IA.Agent
{
    public class SkillDefinition
    {
        public string Name { get; set; } = "general";
        public string Description { get; set; } = "";
        public string Instructions { get; set; } = "";
        public bool UseRag { get; set; } = true;            // ← reactivado por defecto
        public int MaxTokens { get; set; } = 1000000;
        public int MaxFilesPerPlan { get; set; } = 20;
        public long MaxTotalContentBytes { get; set; } = 524_288;
        public bool IncludeAgentFilesInContext { get; set; } = true;
    }
    public class SkillRegistry
    {
        public static SkillRegistry Default { get; } = Load(Path.Combine(AppContext.BaseDirectory, "Skills"));
        private readonly Dictionary<string, SkillDefinition> _skills;
        private SkillRegistry(Dictionary<string, SkillDefinition> skills) => _skills = skills;

        public static SkillRegistry Load(string folder)
        {
            var map = new Dictionary<string, SkillDefinition>(StringComparer.OrdinalIgnoreCase);
            foreach (var dir in Directory.GetDirectories(folder))
            {
                var manifest = Path.Combine(dir, "skill.json");
                if (!File.Exists(manifest)) continue;
                var skill = JsonSerializer.Deserialize<SkillDefinition>(File.ReadAllText(manifest),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
                var md = Path.Combine(dir, "skill.md");
                skill.Instructions = File.Exists(md) ? File.ReadAllText(md) : "";
                map[skill.Name] = skill;
            }
            if (!map.ContainsKey("general"))
                map["general"] = new SkillDefinition { UseRag = true };
            return new SkillRegistry(map);
        }

        public SkillDefinition Resolve(string? name)
            => name != null && _skills.TryGetValue(name, out var s) ? s : _skills["general"];

        // Para el dropdown de la UI: solo nombre + descripción, jamás las instrucciones.
        public List<object> Catalog() => _skills.Values
            .Select(s => (object)new { s.Name, s.Description }).ToList();
    }
}