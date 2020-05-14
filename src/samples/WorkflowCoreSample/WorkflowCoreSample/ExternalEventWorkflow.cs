// ==================================================
// 
//  Author: JRoger
//  Create Date: ${YEAR}-${MONTH}-${DAY}
// 
// ==================================================

using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCoreSample.Steps;

namespace WorkflowCoreSample
{
    public class ExternalEventWorkflow : IWorkflow<MyData>
    {
        public static string EventName01 => "user_approval_01";

        public static string EventName02 => "user_approval_02";

        public string Id => "ExternalEventWorkflow";

        public int Version => 1;

        public void Build(IWorkflowBuilder<MyData> builder)
        {
            builder.StartWith(context => ExecutionResult.Next())

                // 第一个等待事件
                .WaitFor(EventName01, (data, context) => data.Request, data => DateTime.Now)
                    .Output(data => data.ApprovedBy, step => step.EventData)
                .Then<CustomMessage>()
                    .Input(step => step.Message, data => $"The data from the event [{EventName01}] is [{data.ApprovedBy}]")
                
                // 第二个等待事件
                .WaitFor(EventName02, (data, context) => data.Request, data => DateTime.Now)
                    .Output(data => data.ApprovedBy, step => step.EventData)
                .Then<CustomMessage>()
                    .Input(step => step.Message, data => $"The data from the event [{EventName02}] is [{data.ApprovedBy}]")
                
                .Then(context => Console.WriteLine("------> Workflow complete."));
        }
    }
}
