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

    public class Approval2ResultStep : StepBody
    {
        private readonly ILogger<Approval2ResultStep> _logger;

        public Approval2ResultStep(ILogger<Approval2ResultStep> logger)
        {
            this._logger = logger;
        }

        public string JsonData { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            this._logger.LogWarning($"------ ApprovalResult: {(this.JsonData == null ? "empty_event_data" : this.JsonData)} ------");

            return ExecutionResult.Next();
        }
    }
}
