using System;
using WorkflowCore.Interface;

namespace WorkflowCoreWebSample.Workflow.ExternalEvent
{
    public class ApprovalExternalEventWorkflow : IWorkflow<ApprovalEventData>
    {
        public string Id => nameof(ApprovalExternalEventWorkflow);

        public int Version => 1;

        public static string EventName01 => "ApprovalEventName01";

        public static string EventName02 => "ApprovalEventName02";

        public void Build(IWorkflowBuilder<ApprovalEventData> builder)
        {
            builder.StartWith<StartupStep>()

                // user approval 1
                .WaitFor(EventName01, (data, context) => ConstValues.ApprovalEventKey, data => DateTime.Now)
                    // .Output(data => data.ApprovalResult, e => e.EventData)
                    .Output((step, data) => {
                        var eventData = step.EventData as ApprovalEventData;
                        if (eventData != null)
                        {
                            data.UserName = eventData.UserName;
                            data.ApprovalResult = eventData.ApprovalResult;
                        }
                    })
                .Then<ApprovalResultStep>()
                    .Input(step => step.EventData, data => data)

                // user approval 2
                .WaitFor(EventName02, (data, context) => ConstValues.ApprovalEventKey, data => DateTime.Now)
                    .Output((step, data) => {
                        var eventData = step.EventData as ApprovalEventData;
                        if (eventData != null)
                        {
                            data.UserName = eventData.UserName;
                            data.ApprovalResult = eventData.ApprovalResult;
                        }
                    })
                .Then<ApprovalResultStep>()
                    // .Input(step => step.ApprovalResult, data => data.ApprovalResult);
                    .Input(step => step.EventData, data => data);
        }
    }

    public class ApprovalEventData
    {
        public string UserName { get; set; }

        public int ApprovalResult { get; set; }
    }
}
