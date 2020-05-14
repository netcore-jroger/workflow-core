using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Sample04.Steps;

namespace WorkflowCore.Sample04
{
    // 外部事件工作流（场景：HR审批）

    public class EventSampleWorkflow : IWorkflow<MyDataClass>
    {
        public string Id => "EventSampleWorkflow";
            
        public int Version => 1;
            
        public void Build(IWorkflowBuilder<MyDataClass> builder)
        {
            builder
                .StartWith(context => ExecutionResult.Next())
                // 表示一个可等待的外部事件，可通过 IWorkflowHost.PublishEvent() 方法触发这个事件
                // 一个 workflow 的 事件只能被触发一次
                .WaitFor("MyEvent", (data, context) => context.Workflow.Id, data => DateTime.Now)
                    
                    // 下面 Output 方法的第一个参数是要为哪个属性设置值，第二个参数是将事件传过来的参数值为参数 1 赋值
                    .Output(data => data.Value1, step => step.EventData)
                .Then<CustomMessage>()
                    
                    // 下面 Input 方法的第一个参数是要为哪个属性设置值(这里 Message 是 string 类型)
                    // 第二个参数是一个 Func<MyDataClass, string> 委托，它的返回值会赋值给前面的 Message
                    .Input(step => step.Message, data => "The data from the event is " + data.Value1)
                .Then(context => Console.WriteLine("workflow complete"));
        }
    }
}
