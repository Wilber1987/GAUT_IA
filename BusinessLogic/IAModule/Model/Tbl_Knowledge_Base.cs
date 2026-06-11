using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APPCORE;
using BusinessLogic.ApiChat.AutomaticIA.TrainingModule;

namespace BusinessLogic.ApiChat.AutomaticIAOllama.Model
{
    public class Tbl_Knowledge_Base : EntityClass
    {
        [PrimaryKey(Identity = true)]
        public int? Id_Knowledge { get; set; }

        // Corresponde a ServicesIdentification de tu UserMessage
        public string? ServiceTag { get; set; }

        public int? ServiceTagId { get; set; }

        // El bloque de texto (Pregunta + Respuesta) que servirá de contexto
        public string? Content_Text { get; set; }

        // Almacenamiento binario del vector (Embedding)
        public byte[]? Vector_Data { get; set; }

        // Propiedad calculada (no mapeada a columna si tu mapper lo permite)
        // Útil para depuración o si necesitas ver los números en caliente
        public float[]? VectorFloats
        {
            get => VectorHelper.ByteArrayToFloatArray(Vector_Data);
            set => Vector_Data = VectorHelper.FloatArrayToByteArray(value);
        }
        public string? Category { get; set; }
        public string? SourceKey { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? SearchText { get;  set; }
    }

}