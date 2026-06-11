using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Operations.AnaliticOperations;
using Operations.AnaliticOperations.Model;

namespace UI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ApiDashboardController : ControllerBase
    {
        [HttpPost]
        [AuthController]
        public async Task<ActionResult> DocumentDataByTime(DataAnaliticRequest request)
        {
            return Ok(await ProcessDocumentsDataOperation.GetByPeriodo(request));
        }
    }
}