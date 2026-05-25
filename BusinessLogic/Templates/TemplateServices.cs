using System.Reflection;
using System.Text;
using APPCORE;
using APPCORE.Security;
using DataBaseModel;

namespace CAPA_NEGOCIO.Templates
{
    public class TemplateServices
    {
       
        public static string RenderTemplate(string templateContent, object model)
        {
            PropertyInfo[] properties = model.GetType().GetProperties();
            string renderedTemplate = templateContent;
            foreach (PropertyInfo property in properties)
            {
                string propertyName = property.Name;
                object propertyValue = property.GetValue(model, null);

                string placeholder = $"{{{{ {propertyName} }}}}";

                if (propertyValue != null)
                {
                    renderedTemplate = renderedTemplate.Replace(placeholder, propertyValue.ToString());
                }
                else
                {
                    renderedTemplate = renderedTemplate.Replace(placeholder, "");
                }
            }
            LoggerServices.AddMessageInfo("FIN DE RENDER PROPS");
            return renderedTemplate;
        }

        public static string RenderTemplateInvitacion(string templateContent, Security_Users model, string? nombre_completo)
        {
            if (string.IsNullOrWhiteSpace(templateContent))
                throw new ArgumentException("El contenido de la plantilla no puede estar vacío.", nameof(templateContent));

            if (model == null)
                throw new ArgumentNullException(nameof(model), "El modelo no puede ser nulo.");

            var theme = new PageConfig();

            try
            {
                var decryptedPassword = EncrypterServices.Decrypt(model.Password);
                var sanitizedNombreCompleto = nombre_completo ?? string.Empty;

                var sb = new StringBuilder(templateContent);
                sb.Replace("{{ logo }}", theme.MEDIA_IMG_PATH + theme.LOGO_PRINCIPAL)
                  .Replace("{{ link }}", theme.URL_BASE)
                  .Replace("{{ usuario }}", model.Mail ?? string.Empty)
                  //.Replace("{{ contrasena }}", decryptedPassword)
                  .Replace("{{ nombre }}", sanitizedNombreCompleto);

                return sb.ToString();
            }
            catch (Exception ex)
            {
                return "";
                //throw new InvalidOperationException("Error al procesar la plantilla de invitación.", ex);
            }
        }

        public static string RenderTemplateCredenciales(string templateContent, Security_Users model, string? nombre_completo)
        {
            if (string.IsNullOrWhiteSpace(templateContent))
                throw new ArgumentException("El contenido de la plantilla no puede estar vacío.", nameof(templateContent));

            if (model == null)
                throw new ArgumentNullException(nameof(model), "El modelo no puede ser nulo.");

            var theme = new PageConfig();

            try
            {
                var decryptedPassword = EncrypterServices.Decrypt(model.Password);
                var sanitizedNombreCompleto = nombre_completo ?? string.Empty;

                var sb = new StringBuilder(templateContent);
                sb.Replace("{{ logo }}", theme.MEDIA_IMG_PATH + theme.LOGO_PRINCIPAL)
                  .Replace("{{ link }}", theme.URL_BASE)
                  .Replace("{{ usuario }}", model.Mail ?? string.Empty)
                  .Replace("{{ contrasena }}", decryptedPassword)
                  .Replace("{{ nombre }}", sanitizedNombreCompleto);

                return sb.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
                //throw new InvalidOperationException("Error al procesar la plantilla de invitación.", ex);
            }
        }

    }
}