using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using APPCORE;
using BusinessLogic.IA;
using BusinessLogic.Questionnaires.Mapping;
using CAPA_NEGOCIO;
using CAPA_NEGOCIO.Templates.Model;
using DataBaseModel;

namespace BusinessLogic.FormsIA
{
    public class FormsIAOperation : TransactionalClass
    {
        public async Task<bool> ProcessForm()
        {

            List<TestSent> testSentsList = new TestSent().Where<TestSent>(
                FilterData.Equal("State", TestSentEnum.FINISH)
            );
            foreach (var testSent in testSentsList)
            {
                try
                {
                    //TODO TOKEN DEBE SER PROCESADO
                    List<Resultados_Tests> resultados_Tests = new Resultados_Tests().Where<Resultados_Tests>(
                        FilterData.Equal("IdToken", testSent.Token)
                    );
                    List<Section> sections = [];
                    foreach (var resultado in resultados_Tests)
                    {
                        Section section = await this.BuildIASection(resultado, sections);
                        sections.Add(section);
                    }

                    BeginGlobalTransaction();
                    var responseTemplate = new TemplateData
                    {
                        Token = testSent.Token, 
                        Descripcion = TemplatesDataType.PROCESO_DE_SOLICITUD,
                        Sections = sections,
                    }.Save();
                    testSent.State = TestSentEnum.PROCESSED;
                    testSent.Update();
                    CommitGlobalTransaction();
                }
                catch (System.Exception)
                {
                    RollBackGlobalTransaction();
                }
            }
            return true;
        }

        private async Task<Section> BuildIASection(Resultados_Tests resultado, List<Section> sectionsContext)
        {
            //APARTIR DEL RESULTADO CREAMOS EL PROMPT

            string prompt = this.BuildPromptFromResultado(resultado);
            //CON EL PRONT PROCESAMOS EL REQUEST A LA IA
            var instanceIA = new ApiChatProcessorServices();
			// Ejecutar asincronía sincrónicamente usando GetAwaiter().GetResult()

            var message = new UserMessage
            {
                Text = prompt
            };
			var IAResponse = await new IAService().Chat(message);
            //RETORNAMOS UNA SECCION CON ESA INFO
            return new Section
            {
                Body = IAResponse.MessageIA
            };
        }
        private string BuildPromptFromResultado(Resultados_Tests resultado)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Descripcion del proceso:" + resultado.Tests?.Descripcion);
            sb.AppendLine("A continuación se presentan las respuestas de los requerimientos de **" +
                          $"\"{resultado.Seccion}\".** Analiza la información y genera una recomendación técnica detallada.");
            sb.AppendLine();
            sb.AppendLine($"**SECCIÓN: {resultado.Seccion}**");
            //sb.AppendLine($"FECHA DE EVALUACIÓN: {resultado.Fecha?.ToString("yyyy-MM-dd HH:mm")}");
            sb.AppendLine();
            //sb.AppendLine("PREGUNTAS Y RESPUESTAS:");
            //sb.AppendLine();

            var respuestas = resultado.Resultados_Pregunta_Tests ?? new List<Resultados_Pregunta_Tests>();
            foreach (var item in respuestas)
            {
                var pregunta = item.Pregunta_Tests;

                if (pregunta == null) continue;

                 var respuestaTexto = !string.IsNullOrWhiteSpace(item.Respuesta)
                    ? LimpiarHtml(item.Respuesta)
                    : item.Cat_Valor_Preguntas?.Descripcion ?? $"[Valor ID: {item.Id_valor_pregunta}]";
                sb.AppendLine($"  - {pregunta.Descripcion_pregunta}: {respuestaTexto}");
            }

            sb.AppendLine("Con base en esta información, proporciona:");
            sb.AppendLine("  1. Un análisis de la situación actual.");
            sb.AppendLine("  2. Recomendaciones técnicas específicas para esta sección.");
            sb.AppendLine("  3. Posibles riesgos o consideraciones importantes a tener en cuenta.");

            return sb.ToString();
        }

        private string LimpiarHtml(string html)
        {
            if (string.IsNullOrWhiteSpace(html)) return string.Empty;

            var texto = Regex.Replace(html, @"<li>", "\n      • ");
            texto = Regex.Replace(texto, @"<[^>]+>", string.Empty);
            texto = texto.Replace("&nbsp;", " ").Trim();

            return texto;
        }
    }
}