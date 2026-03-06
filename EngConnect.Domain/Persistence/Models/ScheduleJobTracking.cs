using EngConnect.BuildingBlock.Contracts.Models.Quartz;

namespace EngConnect.Domain.Persistence.Models;

public class ScheduleJobTracking: ScheduleJobTrackingBase
{
    public ScheduleJobTracking(string jobName, DateTimeOffset? executeAt, 
        DateTimeOffset? nextFireAt, JobType jobType) : 
        base(jobName, executeAt, nextFireAt, jobType)
    {
    }

    public ScheduleJobTracking()
    {
    }
    
    //Mapping constructor
    public void MapFrom (ScheduleJobTrackingBase? source)
    {
        //Check source
        if (source == null)
        {
            return;
        }
        
        JobName = source.JobName;
        ExecuteAt = source.ExecuteAt;
        NextFireAt = source.NextFireAt;
        JobType = source.JobType;
    }
}