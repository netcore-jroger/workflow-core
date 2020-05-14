using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowCoreWebSample.Workflow.ExternalEvent
{
    public class ApprovalResultStep : StepBody
    {
        private readonly ILogger<ApprovalResultStep> _logger;

        public ApprovalResultStep(ILogger<ApprovalResultStep> logger)
        {
            this._logger = logger;
        }

        public ApprovalEventData EventData { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            this._logger.LogWarning($"------ ApprovalResult: {(this.EventData == null ? "empty_event_data" : JsonConvert.SerializeObject(this.EventData))} ------");

            return ExecutionResult.Next();
        }
    }
}
