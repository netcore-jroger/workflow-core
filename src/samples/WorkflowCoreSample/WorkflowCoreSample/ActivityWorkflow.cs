using WorkflowCore.Interface;
using WorkflowCoreSample.Steps;

namespace WorkflowCoreSample
{
    public class ActivityWorkflow : IWorkflow<MyData>
    {
        public string Id => "activity-sample";

        public int Version => 1;

        public void Build(IWorkflowBuilder<MyData> builder)
        {
            builder.StartWith<HelloWorld>()
                .Activity("get-approval", data => data.Request)
                    .Output(data => data.ApprovedBy, step => step.Result)
                .Then<CustomMessage>()
                    .Input(step => step.Message, data => $"approved by {data.ApprovedBy}")
                .Then<GoodbyeWorld>();
        }
    }

    public class MyData
    {
        public string Request { get; set; }

        public string ApprovedBy { get; set; }
    }
}
