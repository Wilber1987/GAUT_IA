using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessLogic.IAModule.AgentsModules.Models
{
    public class CompletionResult
    {
        public string Content { get; set; } = "";

        public string? FinishReason { get; set; }
        public string? ReasoningContent { get; set; }

        public int? PromptTokens { get; set; }

        public int? CompletionTokens { get; set; }

        public bool IsTruncated =>
            string.Equals(
                FinishReason,
                "length",
                StringComparison.OrdinalIgnoreCase
            );

        public bool IsSuccessful =>
            !string.IsNullOrWhiteSpace(Content) && !IsTruncated;
    }

    public class FilePlan
    {
        public string Action { get; set; } = "final";

        public List<FileInstruction> Files { get; set; } = new();

        public string? ReadPath { get; set; }

        public string? Answer { get; set; }

        public static readonly string PlanSchema = """
            {
            "type": "object",
            "properties": {
                "action": {
                "type": "string",
                "enum": ["create_files", "read_file", "final"]
                },
                "files": {
                "type": "array",
                "items": {
                    "type": "object",
                    "properties": {
                    "path": { "type": "string" },
                    "action": {
                        "type": "string",
                        "enum": [
                        "create_file",
                        "overwrite_file",
                        "edit_file"
                        ]
                    }
                    },
                    "required": ["path", "action"],
                    "additionalProperties": false
                }
                },
                "readPath": {
                "type": ["string", "null"]
                },
                "answer": {
                "type": ["string", "null"]
                }
            },
            "required": [
                "action",
                "files",
                "readPath",
                "answer"
            ],
            "additionalProperties": false
            }
            """;

        public static FilePlan? Parse(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return null;

            try
            {
                return System.Text.Json.JsonSerializer.Deserialize<FilePlan>(
                    raw,
                    new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
            catch
            {
                return null;
            }
        }
    }

    public class FileInstruction
    {
        public string Path { get; set; } = "";

        public string Action { get; set; } = "create_file";
    }


}
