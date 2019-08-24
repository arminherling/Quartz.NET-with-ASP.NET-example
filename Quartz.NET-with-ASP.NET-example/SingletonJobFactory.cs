using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;

public class SingletonJobFactory : IJobFactory
{
    private readonly IServiceProvider serviceProvider;

    public SingletonJobFactory( IServiceProvider serviceProvider )
    {
        this.serviceProvider = serviceProvider;
    }

    public IJob NewJob( TriggerFiredBundle bundle, IScheduler scheduler )
    {
        return serviceProvider.GetRequiredService<QuartzJobRunner>();
    }

    public void ReturnJob( IJob job )
    {
    }
}
