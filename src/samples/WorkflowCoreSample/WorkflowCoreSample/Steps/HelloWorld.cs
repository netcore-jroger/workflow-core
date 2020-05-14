using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowCoreSample.Steps
{
    public class HelloWorld : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("Hello World!");
            return ExecutionResult.Next();
        }
    }

    public class GoodbyeWorld : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("Goodbye World!");

            return ExecutionResult.Next();
        }
    }

    public class CustomMessage : StepBody
    {
        public string Message { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine($"------> {this.Message}");

            return ExecutionResult.Next();
        }
    }
}
