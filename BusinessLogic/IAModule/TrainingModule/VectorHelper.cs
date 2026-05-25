using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.ApiChat.AutomaticIA.TrainingModule
{
    public static class VectorHelper
    {
        /// <summary>
        /// Convierte el array de floats que devuelve Ollama a un array de bytes para SQL Server.
        /// </summary>
        public static byte[] FloatArrayToByteArray(float[] floats)
        {
            if (floats == null) return null;

            byte[] bytes = new byte[floats.Length * sizeof(float)];
            Buffer.BlockCopy(floats, 0, bytes, 0, bytes.Length);
            return bytes;
        }

        /// <summary>
        /// Convierte los bytes de la base de datos de vuelta a floats para cálculos matemáticos.
        /// </summary>
        public static float[] ByteArrayToFloatArray(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return null;

            float[] floats = new float[bytes.Length / sizeof(float)];
            Buffer.BlockCopy(bytes, 0, floats, 0, bytes.Length);
            return floats;
        }

        /// <summary>
        /// Calcula qué tan similares son dos vectores (1.0 es identidad, 0.0 es nada que ver).
        /// </summary>
        public static float CosineSimilarity(float[] v1, float[] v2)
        {
            if (v1 == null || v2 == null || v1.Length != v2.Length) return 0;

            float dot = 0, mag1 = 0, mag2 = 0;
            for (int i = 0; i < v1.Length; i++)
            {
                dot += v1[i] * v2[i];
                mag1 += v1[i] * v1[i];
                mag2 += v2[i] * v2[i];
            }

            float denominator = (float)(Math.Sqrt(mag1) * Math.Sqrt(mag2));
            return denominator == 0 ? 0 : dot / denominator;
        }
    }
}