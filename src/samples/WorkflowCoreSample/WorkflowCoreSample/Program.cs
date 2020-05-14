using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WorkflowCore.Interface;

namespace WorkflowCoreSample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = ConfigureServices();

            var host = serviceProvider.GetService<IWorkflowHost>();

            //host.RegisterWorkflow<ActivityWorkflow, MyData>();
            host.RegisterWorkflow<ExternalEventWorkflow, MyData>();
            
            await host.StartAsync(CancellationToken.None);

            Console.WriteLine("Starting workflow...");

            // await TestActivityAsync(host);
            await TestExternalEventAsync(host);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);

            await host.StopAsync(CancellationToken.None);
        }

        private static async Task TestExternalEventAsync(IWorkflowHost host)
        {
            var data = new MyData { Request = "abc" };
            var workflowId = await host.StartWorkflow("ExternalEventWorkflow", 1, data);

            //#pragma warning disable 4014
            //            Task.Run(async () => {
            //#pragma warning restore 4014
            //                await Task.Delay(10 * 1000);
            //                await host.PublishEvent(ExternalEventWorkflow.EventName, "abc", new MyData{ Request = "abc" });
            //            }).ConfigureAwait(false);

            Console.Write("------> Please input event data: ");
            var eventData = Console.ReadLine();
            await host.PublishEvent(ExternalEventWorkflow.EventName01, data.Request, eventData);

            await Task.Delay(500);
            await host.PublishEvent(ExternalEventWorkflow.EventName02, data.Request, eventData + " -- other");
        }

        private static async Task TestActivityAsync(IWorkflowHost host)
        {
            var workflowId = await host.StartWorkflow("activity-sample", new MyData { Request = "Speed $1,000,000" });
            var approval = await host.GetPendingActivity("get-approval", "work1", TimeSpan.FromMinutes(1D));

            if (approval != null)
            {
                Console.WriteLine($"Approval required for {approval.Parameters}");
                await host.SubmitActivitySuccess(approval.Token, "John Smith");
            }
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddLogging(builder => {
                builder.AddConsole();
                builder.AddDebug();
            });
            //services.AddWorkflow();
            services.AddWorkflow(wf => wf.UseMongoDB(@"mongodb://localhost:27017", "workflowcore"));

            return services.BuildServiceProvider();
        }
    }
}
