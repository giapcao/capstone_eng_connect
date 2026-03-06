using EngConnect.BuildingBlock.Application.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngConnect.Application.UseCases.Tutors.CreateTutor;

namespace EngConnect.Application.UseCases.Tutors.CreateTutor
{
    public record CreateTutorCommand(CreateTutorRequest Request) : ICommand;
}
