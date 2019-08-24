using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Threading.Tasks;

public class QuartzJobRunner : IJob
{
    private readonly IServiceProvider serviceProvider;

    public QuartzJobRunner( IServiceProvider serviceProvider )
    {
        this.serviceProvider = serviceProvider;
    }

    public async Task Execute( IJobExecutionContext context )
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var jobType = context.JobDetail.JobType;
            var job = scope.ServiceProvider.GetRequiredService( jobType ) as IJob;

            await job.Execute( context );
        }
    }
}
