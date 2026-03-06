using EngConnect.BuildingBlock.Application.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngConnect.Application.UseCases.CourseVerification.ReviewCourseVerificationRequest
{
    public record ReviewCourseVerificationRequestCommand(ReviewCourseVerificationRequest Request) : ICommand;
}
