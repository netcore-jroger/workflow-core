using Microsoft.Extensions.Logging;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowCoreWebSample.Workflow
{
    public class StartupStep : StepBody
    {
        private readonly ILogger<StartupStep> _logger;

        public StartupStep(ILogger<StartupStep> logger)
        {
            this._logger = logger;
        }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            this._logger.LogWarning($"------ {context.Workflow.WorkflowDefinitionId} start ------");

            return ExecutionResult.Next();
        }
    }
}
