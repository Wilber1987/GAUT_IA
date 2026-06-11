using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APPCORE;
using CAPA_NEGOCIO.Templates.Model;
using Operations.AnaliticOperations.Model;
using Operations.EstadisticModule;

namespace Operations.AnaliticOperations
{
    public class ProcessDocumentsDataOperation
    {
        static readonly Dictionary<string, ModelProperty> ModelObject = new Dictionary<string, ModelProperty>
        {
            ["Descripcion"] = new ModelProperty { Type = "TEXT" },
            ["Fecha"] = new ModelProperty { Type = "DATE" },
            ["Year"] = new ModelProperty { Type = "NUMBER" },
            ["Month"] = new ModelProperty { Type = "TEXT" }
        };      
        public static async Task<object?> GetByPeriodo(DataAnaliticRequest request)
        {
            // Consulta a la vista/entidad
            var bdData = new TemplateData
            {
                orderData = [
                OrdeData.Asc("Fecha")
            ]
            }.Where<TemplateData>(
                FilterData.GreaterEqual("Fecha", request.Desde),
                FilterData.LessEqual("Fecha", request.Hasta)
            ); // 👈 Importante: materializar la consulta           
            //var resultado = await EjecutarH1_AntiguedadBienestarAsync(datosPreAgrupados);

            // Ejecución del helper genérico
            var result = DataGroupingHelper.GroupData(
                data: bdData.Cast<object>(),
                groupParams: request.GroupParams ?? [],
                evalParams: request.EvalParams,
                modelObject: ModelObject,
                title: "Test",
                isFinalGroupedData: true
            );
            return result;
        }

        // Helper mínimo de reflexión (compatible con tu código existente)
        private static object? GetPropertyValue(object obj, string propName)
        {
            if (obj == null || string.IsNullOrEmpty(propName)) return null;
            return obj.GetType().GetProperty(propName)?.GetValue(obj);
        }        
    }

}