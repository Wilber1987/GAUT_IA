using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APPCORE;
using BusinessLogic.ApiChat.AutomaticIAOllama.Model;
using BusinessLogic.IA;
using BusinessLogic.IAModule.Model;
using CAPA_NEGOCIO.MAPEO;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.Extensions.Hosting;

namespace BusinessLogic.ApiChat.AutomaticIA.TrainingModule
{
    public class VectorizationWorker
    {


        public static async Task ProcessTraining(List<PreRagElement> preRagElements)
        {
            IAService iaService = new IAService();
            foreach (var item in preRagElements)
            {
                string? searchText = string.IsNullOrWhiteSpace(item.Query) ? item.BodyResponse : $"{item.Query} {item.BodyResponse}";

                string contentText = $@"Pregunta: {item.Query} \n Respuesta: {item.BodyResponse}";
                float[] vector = await iaService.GetEmbedding(searchText);
                byte[] vectorBytes = VectorHelper.FloatArrayToByteArray(vector);
                var knowledge = new Tbl_Knowledge_Base
                {
                    ServiceTagId = item.Id_Servicio,
                    ServiceTag = item.Category,
                    SearchText = searchText,
                    Content_Text = contentText,
                    Vector_Data = vectorBytes,
                    CreatedAt = DateTime.UtcNow
                }.Save() as Tbl_Knowledge_Base;
                Console.WriteLine($"Nuevo conocimiento: ${knowledge?.Id_Knowledge} - ${knowledge?.Category}");
            }
        }

        public  async Task ProcessTraining(PreRagElement preRagElements)
        {
            IAService iaService = new IAService();
            var chunkEmbeddings = await iaService.GenerateEmbeddingsFromPdfAsync($@"Pregunta: {preRagElements.Query} \n Respuesta: {preRagElements.BodyResponse}");

            foreach (var (chunkText, vector) in chunkEmbeddings)
            {
                new Tbl_Knowledge_Base
                {
                    Content_Text = chunkText,
                    Vector_Data = VectorHelper.FloatArrayToByteArray(vector),
                    ServiceTag = preRagElements.Category,
                    ServiceTagId = preRagElements.Id_Servicio,
                    CreatedAt = DateTime.UtcNow
                }.Save();
            }

        }
    }
}