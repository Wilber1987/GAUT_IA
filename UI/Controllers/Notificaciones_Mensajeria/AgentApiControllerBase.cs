using API.Controllers;
using APPCORE;
using BusinessLogic.IA.Agent;
using Microsoft.AspNetCore.Mvc;

namespace UI.SSMP_IA.ApiControllers.Agent
{
    [ApiController]
    public abstract class AgentApiControllerBase : ControllerBase
    {
        protected IActionResult Handle(Func<IActionResult> action)
        {
            try { return action(); }
            catch (AgentValidationException ex) { return BadRequest(new { error = ex.Message }); }
            catch (Exception ex)
            {
                LoggerServices.AddMessageError("ERROR: AgentApi", ex);
                return StatusCode(500, new { error = "Internal Server Error" });
            }
        }
    }
    [Route("api/[controller]")]
    [AuthController]
    public class AgentSkillsController : AgentApiControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Handle(() => Ok(SkillRegistry.Default.Catalog()));
    }

    [Route("api/[controller]")]
    [AuthController]
    public class AgentWorkspaceController : AgentApiControllerBase
    {
        [HttpGet("list")]
        public IActionResult List(string userId)
            => Handle(() => Ok(AgentWorkspaceService.ListForUser(userId)));

        [HttpGet("roots")]
        public IActionResult Roots()
            => Handle(() => Ok(AgentSettings.Default.ImportRoots.Keys.ToList()));

        [HttpPost("select")]
        public IActionResult Select(SelectWorkspaceRequest req)
            => Handle(() => Ok(AgentWorkspaceSelector.Select(req)));

        [HttpPost("setFolder")]
        public IActionResult SetFolder(SetFolderRequest req) => Handle(() =>
        {
            var ws = AgentWorkspaceService.ForToken(req.Token, req.UserId);
            ws.BindFolder(req.Path);
            return Ok(ws.ToDto(includeFiles: true));
        });
    }
    public class SetFolderRequest
    {
        public string? Token { get; set; }
        public string? UserId { get; set; }
        public string? Path { get; set; }
    }

    [Route("api/[controller]")]
    [AuthController]
    public class AgentFilesController : AgentApiControllerBase
    {
        [HttpGet("list")]
        public IActionResult List(string token, string userId)
            => Handle(() => Ok(AgentFileServices.List(token, userId)));

        [HttpPost("upload")]
        public IActionResult Upload(UploadFileRequest req)
            => Handle(() => Ok(AgentFileServices.Upload(req)));

        [HttpPost("exclude")]
        public IActionResult Exclude(ExcludeFileRequest req)
            => Handle(() => Ok(AgentFileServices.SetExcluded(req)));
    }

    [Route("api/[controller]")]
    [AuthController]
    public class AgentRunController : AgentApiControllerBase
    {
        [HttpGet("{id:int}")]
        public IActionResult Get(int id, string userId)
            => Handle(() => Ok(AgentRunServices.GetTrace(id, userId)));

        [HttpGet("byToken")]
        public IActionResult ByToken(string token, string userId)
            => Handle(() => Ok(AgentRunServices.ListByToken(token, userId)));
    }
}