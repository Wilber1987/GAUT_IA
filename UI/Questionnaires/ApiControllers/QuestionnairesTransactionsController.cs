using DataBaseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QuestionnairesTransactionsController : ControllerBase
    {
        [HttpPost]
        public object? SaveResultado(Tests inst)
        {
            return inst?.SaveResultado(true, HttpContext.Session.GetString("sessionKey"));
        }

        [HttpPost]
        [AuthController]
        public object? SaveResultadoPrivate(Tests inst)
        {
            return inst?.SaveResultado();
        }
    }
}
