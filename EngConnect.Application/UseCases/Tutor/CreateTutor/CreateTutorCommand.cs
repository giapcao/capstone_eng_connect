using EngConnect.BuildingBlock.Application.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngConnect.Application.UseCases.Tutor.CreateTutor
{
    public record CreateTutorCommand(CreateTutorRequest Request) : ICommand;
}
