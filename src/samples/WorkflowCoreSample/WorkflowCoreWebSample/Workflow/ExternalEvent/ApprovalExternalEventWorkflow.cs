using System;
using WorkflowCore.Interface;

namespace WorkflowCoreWebSample.Workflow.ExternalEvent
{
    public class ApprovalExternalEventWorkflow : IWorkflow<ApprovalEventData>
    {
        public string Id => nameof(ApprovalExternalEventWorkflow);

        public int Version => 1;

        public static string EventName01 => "ApprovalEventName01";

        public void Build(IWorkflowBuilder<ApprovalEventData> builder)
        {
            builder.StartWith<StartupStep>()
                .WaitFor(EventName01, (data, context) => ConstValues.ApprovalEventKey, data => DateTime.Now)
                    .Output(data => data.ApprovalResult, e => e.EventData)
                .Then<ApprovalResultStep>()
                    .Input(step => step.ApprovalResult, data => data.ApprovalResult);
        }
    }

    public class ApprovalEventData
    {
        public string UserName { get; set; }

        public int ApprovalResult { get; set; }
    }
}
