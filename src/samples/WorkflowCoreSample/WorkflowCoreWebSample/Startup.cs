using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCoreWebSample.Workflow;
using WorkflowCoreWebSample.Workflow.ExternalEvent;

namespace WorkflowCoreWebSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            services.AddWorkflow(options => options.UseMongoDB("mongodb://localhost:27017", "workflowcore"));
            services.AddWorkflowDSL();

            services.AddTransient<StartupStep>();
            services.AddTransient<ApprovalResultStep>();
            services.AddTransient<Approval2ResultStep>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            RegisterWorkflow(app);
        }

        private static void RegisterWorkflow(IApplicationBuilder app)
        {
            var host = app.ApplicationServices.GetService<IWorkflowHost>();

            host.RegisterWorkflow<ApprovalExternalEventWorkflow, ApprovalEventData>();

            host.Start();
        }
    }
}
