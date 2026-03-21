using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Settings;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.Constants;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Abstraction;
using EngConnect.Domain.Constants;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using EngConnect.Domain.Settings;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EngConnect.Application.UseCases.Authentication.RegisterTutor
{
    public class RegisterTutorCommandHandler : ICommandHandler<RegisterTutorCommand>
    {
        private readonly ILogger<RegisterTutorCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IRedisService _redisService;
        private readonly RedisCacheSettings _redisCacheSettings;

        public RegisterTutorCommandHandler(ILogger<RegisterTutorCommandHandler> logger, IUnitOfWork unitOfWork, IMapper mapper, 
            IOptions<AppSettings> appSettings, IJwtTokenService jwtTokenService, IRedisService redisService, 
            IOptions<RedisCacheSettings> redisCacheSettings)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtTokenService = jwtTokenService;
            _redisService = redisService;
            _redisCacheSettings = redisCacheSettings.Value;
            _appSettings = appSettings.Value;
        }

        public async Task<Result> HandleAsync(
            RegisterTutorCommand command,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Start RegisterTutorCommandHandler: {@Command}", command);

            try
            {
                // Check user
                var user = await _unitOfWork.GetRepository<User, Guid>().
                    FindAll(x => x.Id == command.UserId,
                    cancellationToken: cancellationToken)
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);
                if (ValidationUtil.IsNullOrEmpty(user))
                {
                    _logger.LogWarning("User with ID '{UserId}' not found", command.UserId);
                    return Result.Failure(HttpStatusCode.BadRequest,UserErrors.UserNotFound());
                }
                var repo = _unitOfWork.GetRepository<Tutor, Guid>();
                
                // Check duplicate tutor profile for the same user
                var existingTutor = await repo.FindFirstAsync(
                    x => x.UserId == command.UserId,
                    cancellationToken: cancellationToken);
                if (existingTutor != null)                {
                    _logger.LogWarning("Tutor profile already exists for user ID '{UserId}'", command.UserId);
                    return Result.Failure(HttpStatusCode.BadRequest, TutorErrors.TutorProfileAlreadyExists());
                }   

                var entity = _mapper.Map<Tutor>(command);
                entity.UserId = user.Id;
                entity.Avatar = _appSettings.DefaultAvatarPath;
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

                var refreshedUser = await _unitOfWork.GetRepository<User, Guid>().
                    FindAll(x => x.Id == command.UserId,
                        cancellationToken: cancellationToken)
                    .Include(x => x .UserRoles)
                    .ThenInclude(x => x.Role)
                    .Include(x => x.Student)
                    .Include(x => x.Tutor)
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);
                
                //Generate new token for user
                var accessToken = _jwtTokenService.GenerateAccessToken(refreshedUser);
                var refreshToken = _jwtTokenService.GenerateRefreshToken();
                
                // Remove old refresh token if exist
                var deleteRefreshTokenKeyPattern =  RedisKeyGenerator.GenerateRefreshTokenKeyDeletePattern(user.Id);
                await _redisService.DeleteCacheWithPatternAsync(deleteRefreshTokenKeyPattern);
                // Store the refresh token in the database
                var newRefreshTokenKey = RedisKeyGenerator.GenerateRefreshTokenKey(user.Id, refreshToken);
                await _redisService.SetCacheAsync(newRefreshTokenKey, refreshToken,
                    TimeSpan.FromMinutes(_redisCacheSettings.RefreshTokenExpirationMinutes));
                    
                _logger.LogInformation("End RegisterTutorCommandHandler");
                var response = new 
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
                return Result.Success(response);
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
