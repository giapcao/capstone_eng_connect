using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.Constants;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Constants;
using EngConnect.Domain.Persistence.Models;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Authentication.RegisterTutor
{
    public class RegisterTutorCommandHandler : ICommandHandler<RegisterTutorCommand>
    {
        private readonly ILogger<RegisterTutorCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RegisterTutorCommandHandler(ILogger<RegisterTutorCommandHandler> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> HandleAsync(
            RegisterTutorCommand command,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Start RegisterTutorCommandHandler: {@Command}", command);

            try
            {
                var repo = _unitOfWork.GetRepository<Domain.Persistence.Models.Tutor, Guid>();

                var entity = _mapper.Map<Domain.Persistence.Models.Tutor>(command);
                entity.Status = nameof(CommonStatus.Active);
                entity.VerifiedStatus = nameof(TutorVerifiedStatus.Unverified); // Set verified status to false (Unverified)
                // Tags are not included - left as null

                repo.Add(entity);

                //Assign Tutor role
                var roleTutorCode = nameof(UserRoleEnum.Tutor);
                var roleRepo = _unitOfWork.GetRepository<Role, Guid>();
                var tutorRole = await roleRepo.FindFirstAsync(
                    x => x.Code == roleTutorCode,
                    cancellationToken: cancellationToken);
                if (tutorRole != null)
                {
                    var userRole = new UserRole { UserId = command.UserId, RoleId = tutorRole.Id };
                    _unitOfWork.GetRepository<UserRole, Guid>().Add(userRole);
                }
                else
                {
                    _logger.LogWarning("Role with code '{Code}' not found — skipping role assignment", roleTutorCode);
                }

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("End RegisterTutorCommandHandler");
                return Result.Success(entity.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in RegisterTutorCommandHandler {@Message}", ex.Message);
                return Result.Failure<Guid>(HttpStatusCode.InternalServerError,
                    CommonErrors.InternalServerError());
            }
        }
    }
}
