using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers;
using APPCORE;
using BusinessLogic.Questionnaires.Mapping;
using BusinessLogic.Questionnaires.Transactional;
using Microsoft.AspNetCore.Mvc;

namespace UI.Questionnaires.ApiControllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ApiTestSentManagerController : ControllerBase
    {
        [HttpPost]
        [AuthController]
        public List<TestSent> GetTestSent(TestSent Inst)
        {
            return Inst.Get<TestSent>();
        }
        [HttpPost]
        [AuthController]
        public TestSent? FindTestSent(TestSent Inst)
        {
            return Inst.Find<TestSent>();
        }
        [HttpPost]
        [AuthController]
        public ResponseService SaveTestSent(TestSent inst)
        {
            return TestSentOperations.SaveTets(inst);
        }
        [HttpPost]
        [AuthController]
        public ResponseService UpdateTestSent(TestSent inst)
        {
            return inst.Update();
        }
    }
}