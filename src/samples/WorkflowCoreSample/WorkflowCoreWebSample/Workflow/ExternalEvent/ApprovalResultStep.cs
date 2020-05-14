using Microsoft.Extensions.Logging;
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

        public int ApprovalResult { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            this._logger.LogWarning($"------ ApprovalResult: {this.ApprovalResult} ------");

            return ExecutionResult.Next();
        }
    }
}
