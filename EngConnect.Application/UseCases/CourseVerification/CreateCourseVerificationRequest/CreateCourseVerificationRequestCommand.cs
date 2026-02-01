using EngConnect.BuildingBlock.Application.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngConnect.Application.UseCases.CourseVerification.CreateCourseVerificationRequest
{
    public record CreateCourseVerificationRequestCommand(CreateCourseVerificationRequest Request) : ICommand;
}
