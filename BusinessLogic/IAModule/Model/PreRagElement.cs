using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.IAModule.Model
{
    public class PreRagElement
    {
        public string? Key { get; set; }
        public string? Category { get; set; }
        public string? Query { get; set; }
        public string? BodyResponse { get; set; }
        public int? Id_Servicio { get; internal set; }
    }
}