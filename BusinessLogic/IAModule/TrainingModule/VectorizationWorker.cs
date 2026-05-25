using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APPCORE;
using BusinessLogic.ApiChat.AutomaticIAOllama.Model;
using BusinessLogic.IA;
using CAPA_NEGOCIO.MAPEO;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.Extensions.Hosting;

namespace BusinessLogic.ApiChat.AutomaticIA.TrainingModule
{
    public class VectorizationWorker
    {


        public async Task ProcessPendingComments()
        {
            // 1. Obtener comentarios nuevos de agentes o IA que tengan contenido útil
            // Filtramos mensajes muy cortos (como "ok" o "hola") que no sirven para RAG


           /* var respuestasAgente = new Tbl_Comments().Where<Tbl_Comments>(
                            FilterData.Limit(400),
                            FilterData.ISNull("IsVectorized"),
                            FilterData.NotNull("Id_User")
            );
            */
            /*foreach (var respuesta in respuestasAgente)
            {
                // 2. Buscamos el mensaje del usuario que detonó esta respuesta
                // Es el mensaje anterior más cercano en el mismo caso
                Tbl_Case? Tbl_CaseWithComments = new Tbl_Case() { Id_Case = respuesta.Id_Case }.Find<Tbl_Case>();



                var preguntaUsuario = new Tbl_Comments().Find<Tbl_Comments>(
                    FilterData.Distinc("Id_User", respuesta.Id_User),
                    FilterData.Equal("Id_Case", respuesta.Id_Case),
                    FilterData.Less("Fecha", respuesta.Fecha)
                );

                if (!string.IsNullOrEmpty(preguntaUsuario?.Body))
                {
                    // 3. CREAMOS EL PAR (Este es el secreto de la fluidez)
                    string contenidoParaVector = $"Pregunta Usuario: {preguntaUsuario} | Respuesta Agente: {respuesta.Body}";

                    // 4. Generamos el vector de este bloque completo
                    float[] vector = await new OllamaService().GetEmbedding(contenidoParaVector);
                    byte[] vectorBytes = VectorHelper.FloatArrayToByteArray(vector);

                    // 5. Guardamos en la base de conocimiento
                    var newVector = new Tbl_Knowledge_Base
                    {
                        ServiceTagId = Tbl_CaseWithComments?.Tbl_Servicios?.Id_Servicio,
                        ServiceTag = Tbl_CaseWithComments?.Tbl_Servicios?.Nombre_Servicio,
                        Content_Text = contenidoParaVector,
                        Vector_Data = vectorBytes
                    }.Save() as Tbl_Knowledge_Base;
                }
                respuesta.IsVectorized = true;
                respuesta.Update();
            }*/
        }
    }
}