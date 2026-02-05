using EngConnect.BuildingBlock.Application.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngConnect.Application.UseCases.TutorVerification.CreateTutorVerificationRequest
{
    public record CreateTutorVerificationRequestCommand(CreateTutorVerificationRequest Request) : ICommand;
}
