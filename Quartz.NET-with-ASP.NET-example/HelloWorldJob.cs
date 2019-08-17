using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading.Tasks;

[DisallowConcurrentExecution]
public class HelloWorldJob : IJob
{
    private readonly ILogger<HelloWorldJob> logger;

    public HelloWorldJob( ILogger<HelloWorldJob> logger )
    {
        this.logger = logger;
    }

    public Task Execute( IJobExecutionContext context )
    {
        logger.LogInformation( $"{DateTime.Now}: Hello World!" );
        return Task.CompletedTask;
    }
}
