using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class QuartzHostedService : IHostedService
{
    private readonly ISchedulerFactory schedulerFactory;
    private readonly IJobFactory jobFactory;
    private readonly IEnumerable<JobSchedule> jobSchedules;

    public QuartzHostedService(
        ISchedulerFactory schedulerFactory,
        IJobFactory jobFactory,
        IEnumerable<JobSchedule> jobSchedules )
    {
        this.schedulerFactory = schedulerFactory;
        this.jobFactory = jobFactory;
        this.jobSchedules = jobSchedules;
    }

    public IScheduler Scheduler { get; set; }

    public async Task StartAsync( CancellationToken cancellationToken )
    {
        Scheduler = await schedulerFactory.GetScheduler( cancellationToken );
        Scheduler.JobFactory = jobFactory;

        foreach( var jobSchedule in jobSchedules )
        {
            var job = CreateJob( jobSchedule );
            var trigger = CreateTrigger( jobSchedule );

            await Scheduler.ScheduleJob( job, trigger, cancellationToken );
        }

        await Scheduler.Start( cancellationToken );
    }

    public async Task StopAsync( CancellationToken cancellationToken )
    {
        await Scheduler?.Shutdown( cancellationToken );
    }

    private ITrigger CreateTrigger( JobSchedule jobSchedule )
    {
        return TriggerBuilder
            .Create()
            .WithIdentity( $"{jobSchedule.JobType.FullName}.trigger" )
            .WithCronSchedule( jobSchedule.CronExpression )
            .WithDescription( jobSchedule.CronExpression )
            .Build();
    }

    private IJobDetail CreateJob( JobSchedule jobSchedule )
    {
        var jobType = jobSchedule.JobType;
        return JobBuilder
            .Create( jobType )
            .WithIdentity( jobType.FullName )
            .WithDescription( jobType.Name )
            .Build();
    }
}
