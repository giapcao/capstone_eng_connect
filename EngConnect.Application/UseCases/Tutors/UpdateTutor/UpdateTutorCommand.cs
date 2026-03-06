using EngConnect.BuildingBlock.Application.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngConnect.Application.UseCases.Tutors.UpdateTutor;

namespace EngConnect.Application.UseCases.Tutors.UpdateTutor
{
    public record UpdateTutorCommand(Guid Id, UpdateTutorRequest Request) : ICommand;
}
