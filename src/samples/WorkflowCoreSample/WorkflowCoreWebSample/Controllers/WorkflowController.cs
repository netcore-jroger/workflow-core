using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WorkflowCore.Interface;
using WorkflowCore.Services.DefinitionStorage;
using WorkflowCoreWebSample.Dto;
using WorkflowCoreWebSample.Workflow;
using WorkflowCoreWebSample.Workflow.ExternalEvent;

namespace WorkflowCoreWebSample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkflowController : Controller
    {
        private readonly IWorkflowHost _host;
        private readonly IDefinitionLoader _definitionLoader;
        private readonly ApprovalExternalEventWorkflow _approvalExternalEventWorkflow;

        public WorkflowController(IWorkflowHost host, IDefinitionLoader definitionLoader)
        {
            this._host = host;
            this._definitionLoader = definitionLoader;
            this._approvalExternalEventWorkflow = new ApprovalExternalEventWorkflow();
        }

        [Route("/")]
        [HttpGet]
        public async Task<IActionResult> StartWorkflow()
        {
            var data = new ApprovalEventData();
            var workflowId = await this._host.StartWorkflow<ApprovalEventData>(this._approvalExternalEventWorkflow.Id, this._approvalExternalEventWorkflow.Version, data);

            return Json(new { message = $"{this._approvalExternalEventWorkflow.Id} - {workflowId} start." });
        }

        [HttpPost("definition")]
        public async Task<IActionResult> RegisterWorkflow()
        {
            var definitionSource = await new StreamReader(this.Request.Body).ReadToEndAsync();
            if (string.IsNullOrWhiteSpace(definitionSource)) return Json(new { message = "definitionSource can not empty." });

            var definition = this._definitionLoader.LoadDefinition(definitionSource, Deserializers.Yaml);

            _ = await this._host.StartWorkflow(definition.Id, definition.Version);

            return Json(new { message = "custom workflow register successed." });
        }

        [HttpPost("fire")]
        public async Task<IActionResult> Fire([FromBody] ApprovalDto dto)
        {
            var eventData = new ApprovalEventData { UserName = dto.UserName, ApprovalResult = dto.ApprovalResult };

            await this._host.PublishEvent(dto.EventName, ConstValues.ApprovalEventKey, eventData);

            return Json(new { message = "approval event published." });
        }

        [HttpPost("fire2")]
        public async Task<IActionResult> Fire2([FromBody] ApprovalDto dto)
        {
            var eventData = new ApprovalEventData { UserName = dto.UserName, ApprovalResult = dto.ApprovalResult };

            await this._host.PublishEvent(dto.EventName, ConstValues.Approval2EventKey, JsonConvert.SerializeObject(eventData));

            return Json(new { message = "approval event published." });
        }
    }
}
