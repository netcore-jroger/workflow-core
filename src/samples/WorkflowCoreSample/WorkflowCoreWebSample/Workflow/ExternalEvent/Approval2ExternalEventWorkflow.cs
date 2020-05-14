using System;
using WorkflowCore.Interface;

namespace WorkflowCoreWebSample.Workflow.ExternalEvent
{
    public class Approval2ExternalEventWorkflow : IWorkflow<Approval2EventData>
    {
        public string Id => nameof(Approval2ExternalEventWorkflow);

        public int Version => 1;

        public static string EventName01 => "Approval2EventName01";

        public static string EventName02 => "Approval2EventName02";

        public void Build(IWorkflowBuilder<Approval2EventData> builder)
        {
            builder.StartWith<StartupStep>()

                // user approval 1
                .WaitFor(EventName01, (data, context) => ConstValues.Approval2EventKey, data => DateTime.Now)
                    .Output(data => data.JsonData, e => e.EventData)
                .Then<Approval2ResultStep>()
                    .Input(step => step.JsonData, data => data.JsonData)

                // user approval 2
                .WaitFor(EventName02, (data, context) => ConstValues.Approval2EventKey, data => DateTime.Now)
                    .Output(data => data.JsonData, e => e.EventData)
                .Then<Approval2ResultStep>()
                    .Input(step => step.JsonData, data => data.JsonData);
        }
    }

    public class Approval2EventData
    {
        public string JsonData { get; set; }
    }
}
