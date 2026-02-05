using EngConnect.BuildingBlock.Application.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngConnect.Application.UseCases.Tutor.UpdateTutor
{
    public record UpdateTutorCommand(Guid Id, UpdateTutorRequest Request) : ICommand;
}
