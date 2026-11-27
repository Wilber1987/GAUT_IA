using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.IAModule.AgentsModules
{

    public class PlanFormatter
    {
        public string GetSystemPrompt()
        {
            return """
                Eres un planificador de operaciones de archivos.

                Tu tarea es decidir qué archivos deben leerse, crearse,
                modificarse o sobrescribirse.

                REGLAS OBLIGATORIAS:

                1. Devuelve exclusivamente un objeto JSON válido.
                2. No utilices bloques Markdown.
                3. No escribas ```json ni ``` .
                4. No incluyas razonamientos ni explicaciones.
                5. No inventes propiedades.
                6. No generes contenido de archivos.
                7. Utiliza exclusivamente esta estructura:

                {
                "action": "read_file | create_files | final",
                "readPath": null,
                "files": [
                    {
                    "path": "ruta/archivo.ext",
                    "action": "create_file | overwrite_file | edit_file"
                    }
                ],
                "answer": null
                }

                Para leer un archivo:
                {
                "action": "read_file",
                "readPath": "wiki.html",
                "files": [],
                "answer": null
                }

                Para crear o modificar archivos:
                {
                "action": "create_files",
                "readPath": null,
                "files": [
                    {
                    "path": "wiki.html",
                    "action": "edit_file"
                    }
                ],
                "answer": null
                }

                Para responder sin operaciones:
                {
                "action": "final",
                "readPath": null,
                "files": [],
                "answer": "Respuesta al usuario"
                }
                """;
        }

        public string BuildUserPrompt(string userRequest)
        {
            return $"""
                PETICIÓN DEL USUARIO:

                {userRequest}

                Analiza la petición y devuelve únicamente el plan JSON.
                Recuerda que estás en la fase de planificación.
                No generes contenido de archivos.
                """;
        }
    }
}