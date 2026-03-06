using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Courses.SubmitCourse;

public class SubmitCourseCommand: ICommand
{
    public Guid CourseId { get; set; } 
}