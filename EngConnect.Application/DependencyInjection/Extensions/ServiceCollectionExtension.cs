using System.Reflection.Metadata;
using EngConnect.Application.Mapping;
using EngConnect.Application.UseCases.Authentication.LoginByUser;
using EngConnect.Application.UseCases.Authentication.RefreshToken;
using EngConnect.Application.UseCases.Authentication.RegisterUser;
using EngConnect.Application.UseCases.Authentication.VerifyEmail;
using EngConnect.Application.UseCases.Users.ChangePassword;
using EngConnect.Application.UseCases.Users.CreateUser;
using EngConnect.Application.UseCases.Users.ForgotPassword;
using EngConnect.Application.UseCases.Users.ResetPassword;
using EngConnect.Application.UseCases.Users.UpdateUser;
using EngConnect.Application.UseCases.CourseEnrollments.CreateCourseEnrollment;
using EngConnect.Application.UseCases.CourseEnrollments.GetListCourseEnrollments;
using EngConnect.Application.UseCases.CourseEnrollments.UpdateCourseEnrollment;
using EngConnect.Application.UseCases.CourseEnrollments.UpdateCourseEnrollmentStatus;
using EngConnect.Application.UseCases.LessonRecords.CreateLessonRecord;
using EngConnect.Application.UseCases.LessonRecords.UpdateLessonRecord;
using EngConnect.Application.UseCases.Lessons.CreateLesson;
using EngConnect.Application.UseCases.Lessons.GetListLessons;
using EngConnect.Application.UseCases.Lessons.UpdateLesson;
using EngConnect.Application.UseCases.Lessons.UpdateLessonStatus;
using EngConnect.Application.UseCases.LessonScripts.CreateLessonScript;
using EngConnect.Application.UseCases.LessonScripts.UpdateLessonScript;
using EngConnect.Application.UseCases.Students.CreateStudent;
using EngConnect.Application.UseCases.Students.GetListStudents;
using EngConnect.Application.UseCases.Students.UpdateStatusStudent;
using EngConnect.Application.UseCases.Students.UpdateStudent;
using EngConnect.BuildingBlock.DependencyInjection.Extensions;
using FluentValidation;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace EngConnect.Application.DependencyInjection.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddValidators();
        services.AddCqrs(AssemblyReference.Assembly);
        services.AddMappings();
    }

    private static void AddMappings(this IServiceCollection services)
    {
        services.AddScoped<IMapper, Mapper>();
        MappingConfig.RegisterMappings();
    }

    private static void AddValidators(this IServiceCollection services)
    {
        // Explicitly register specific validators if necessary
        // services.AddScoped<IValidator<COMMAND_CLASS>, COMMAND_VALIDATOR_CLASS>();

        // Authentication Section
        services.AddScoped<IValidator<LoginByUserCommand>, LoginByUserCommandValidator>();
        services.AddScoped<IValidator<RegisterUserCommand>, RegisterUserCommandValidator>();
        services.AddScoped<IValidator<VerifyEmailCommand>, VerifyEmailCommandValidator>();
        services.AddScoped<IValidator<RefreshTokenCommand>, RefreshTokenCommandValidator>();
        
        // User Section
        services.AddScoped<IValidator<UpdateUserCommand>, UpdateUserCommandValidator>();
        services.AddScoped<IValidator<CreateUserCommand>, CreateUserCommandValidator>();
        services.AddScoped<IValidator<ForgotPasswordCommand>, ForgotPasswordCommandValidator>();
        services.AddScoped<IValidator<ResetPasswordCommand>, ResetPasswordCommandValidator>();
        services.AddScoped<IValidator<ChangePasswordCommand>, ChangePasswordCommandValidator>();
        
        //Student Section
        services.AddScoped<IValidator<CreateStudentCommand>, CreateStudentCommandValidator>();
        services.AddScoped<IValidator<UpdateStudentCommand>, UpdateStudentCommandValidator>();
        services.AddScoped<IValidator<GetListStudentQuery>, GetListStudentQueryValidator>();
        services.AddScoped<IValidator<CreateLessonCommand>, CreateLessonCommandValidator>();
        services.AddScoped<IValidator<UpdateLessonCommand>, UpdateLessonCommandValidator>();
        services.AddScoped<IValidator< GetListLessonQuery>, GetListLessonValidator>();
        services.AddScoped<IValidator<UpdateLessonStatusCommand>, UpdateLessonStatusCommandValidator>();
        services.AddScoped<IValidator<CreateCourseEnrollmentCommand>, CreateCourseEnrollmentCommandValidator>();
        services.AddScoped<IValidator<UpdateCourseEnrollmentCommand>, UpdateCourseEnrollmentCommandValidator>();
        services.AddScoped<IValidator<GetListCourseEnrollmentQuery>, GetListCourseEnrollmentValidator>();
        services.AddScoped<IValidator<UpdateCourseEnrollmentStatusCommand>, UpdateCourseEnrollmentStatusValidator>();
        services.AddScoped<IValidator<CreateLessonRecordCommand>, CreateLessonRecordCommandValidator>();
        services.AddScoped<IValidator<UpdateLessonRecordCommand>,  UpdateLessonRecordCommandValidator>();
        services.AddScoped<IValidator<CreateLessonScriptCommand>, CreateLessonScriptCommandValidator>();
        services.AddScoped<IValidator<UpdateLessonScriptCommand>, UpdateLessonScriptCommandValidator>();
    }
}