using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.ApiChat.AutomaticIAOllama.Model
{
    public class OllamaGenerateResponse
    {
        // El nombre del modelo que respondió
    public string? model { get; set; }
    
    // La fecha de creación de la respuesta
    public DateTime? created_at { get; set; }
    
    // El texto de la respuesta generado por la IA
    public string? response { get; set; }
    
    // Indica si la respuesta terminó (útil si no usas streaming)
    public bool? done { get; set; }
    
    // Estadísticas opcionales (tokens, duración, etc.)
    public long? total_duration { get; set; }
    public int? prompt_eval_count { get; set; }
    public int? eval_count { get; set; }
    }
}