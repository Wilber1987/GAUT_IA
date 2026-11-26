using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.IAModule.Utils
{
    public static class TextChunker
    {
        /// <summary>
        /// Divide un texto largo en fragmentos con solapamiento,
        /// para no perder contexto entre chunks consecutivos.
        /// </summary>
        public static List<string> ChunkText(string text, int chunkSizeWords = 300, int overlapWords = 50)
        {
            var words = text.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            var chunks = new List<string>();

            if (words.Length == 0) return chunks;

            int step = chunkSizeWords - overlapWords;
            for (int i = 0; i < words.Length; i += step)
            {
                var chunkWords = words.Skip(i).Take(chunkSizeWords);
                chunks.Add(string.Join(" ", chunkWords));

                if (i + chunkSizeWords >= words.Length) break;
            }

            return chunks;
        }
    }
}