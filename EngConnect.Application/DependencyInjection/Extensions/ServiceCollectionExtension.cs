using EngConnect.Application.Mapping;
using EngConnect.Application.UseCases.Authentication.LoginByUser;
using EngConnect.Application.UseCases.Authentication.RefreshToken;
using EngConnect.Application.UseCases.Authentication.RegisterStudent;
using EngConnect.Application.UseCases.Authentication.RegisterTutor;
using EngConnect.Application.UseCases.Authentication.RegisterUser;
using EngConnect.Application.UseCases.Authentication.VerifyEmail;
using EngConnect.Application.UseCases.CourseCourseModules.AddCourseModuleToCourse;
using EngConnect.Application.UseCases.Users.ChangePassword;
using EngConnect.Application.UseCases.Users.CreateUser;
using EngConnect.Application.UseCases.Users.ForgotPassword;
using EngConnect.Application.UseCases.Users.ResetPassword;
using EngConnect.Application.UseCases.Users.UpdateUser;
using EngConnect.Application.UseCases.CourseEnrollments.CreateCourseEnrollment;
using EngConnect.Application.UseCases.CourseEnrollments.GetListCourseEnrollments;
using EngConnect.Application.UseCases.CourseEnrollments.UpdateCourseEnrollment;
using EngConnect.Application.UseCases.CourseEnrollments.UpdateCourseEnrollmentStatus;
using EngConnect.Application.UseCases.AiSummerize.GetAiSummary;
using EngConnect.Application.UseCases.CourseModuleCourseSessions.AddCourseSessionToCourseModule;
using EngConnect.Application.UseCases.Courses.CreateCourse;
using EngConnect.Application.UseCases.Courses.UpdateCourse;
using EngConnect.Application.UseCases.Courses.UpdateThumbnailCourse;
using EngConnect.Application.UseCases.Courses.UpdateDemoVideoCourse;
using EngConnect.Application.UseCases.CourseResources.CreateCourseResource;
using EngConnect.Application.UseCases.CourseResources.UpdateCourseResource;
using EngConnect.Application.UseCases.CourseSessionCourseResources.AddCourseResourceToCourseSession;
using EngConnect.Application.UseCases.FileStorage.DeleteFileFromDrive;
using EngConnect.Application.UseCases.FileStorage.GetFileFromDrive;
using EngConnect.Application.UseCases.FileStorage.UploadFileToDrive;
using EngConnect.Application.UseCases.LessonRecords.CreateLessonRecord;
using EngConnect.Application.UseCases.LessonRecords.UpdateLessonRecord;
using EngConnect.Application.UseCases.Lessons.CreateLesson;
using EngConnect.Application.UseCases.Lessons.GetListLessons;
using EngConnect.Application.UseCases.Lessons.UpdateLesson;
using EngConnect.Application.UseCases.Lessons.UpdateLessonStatus;
using EngConnect.Application.UseCases.LessonScripts.CreateLessonScript;
using EngConnect.Application.UseCases.LessonScripts.UpdateLessonScript;
using EngConnect.Application.UseCases.Meetings.UploadRecordingChunk;
using EngConnect.Application.UseCases.Students.CreateStudent;
using EngConnect.Application.UseCases.Students.GetListStudents;
using EngConnect.Application.UseCases.Students.UpdateAvatarStudent;
using EngConnect.Application.UseCases.Students.UpdateStudent;
using EngConnect.Application.UseCases.SupportTicketMessages.CreateSupportTicketMessage;
using EngConnect.Application.UseCases.SupportTicketMessages.UpdateSupportTicketMessage;
using EngConnect.Application.UseCases.SupportTickets.CreateSupportTicket;
using EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicket;
using EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicketStatus;
using EngConnect.Application.UseCases.Tutors.CreateTutor;
using EngConnect.Application.UseCases.Tutors.UpdateCvUrlTutor;
using EngConnect.Application.UseCases.Tutors.UpdateIntroVideoUrlTutor;
using EngConnect.Application.UseCases.Tutors.UpdateTutor;
using EngConnect.BuildingBlock.DependencyInjection.Extensions;
using FluentValidation;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using EngConnect.Application.UseCases.TutorVerification.CreateTutorVerificationRequest;
using EngConnect.Application.UseCases.TutorVerification.ReviewTutorVerificationRequest;
using EngConnect.Application.UseCases.TutorDocuments.RemoveTutorDocument;
using EngConnect.Application.UseCases.TutorDocuments.UploadTutorDocument;

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
        // Tutor Section
        services.AddScoped<IValidator<CreateTutorCommand>, CreateTutorCommandValidator>();
        services.AddScoped<IValidator<UpdateTutorCommand>, UpdateTutorCommandValidator>();
        services.AddScoped<IValidator<RegisterTutorCommand>, RegisterTutorCommandValidator>();
        services.AddScoped<IValidator<UpdateCvUrlTutorCommand>, UpdateCvUrlTutorValidator>();
        services.AddScoped<IValidator<UpdateIntroVideoUrlTutorCommand>, UpdateIntroVideoUrlTutorValidator>();
        services.AddScoped<IValidator<UploadTutorDocumentCommand>, UploadTutorDocumentCommandValidator>();
        services.AddScoped<IValidator<RemoveTutorDocumentCommand>, RemoveTutorDocumentCommandValidator>();
        services.AddScoped<IValidator<CreateTutorVerificationRequestCommand>, CreateTutorVerificationRequestCommandValidator>(); 
        services.AddScoped<IValidator<ReviewTutorVerificationRequestCommand>, ReviewTutorVerificationRequestCommandValidator>();

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
        services.AddScoped<IValidator<RegisterStudentCommand>, RegisterStudentCommandValidator>();
        services.AddScoped<IValidator<GetListStudentQuery>, GetListStudentQueryValidator>();
        services.AddScoped<IValidator<UpdateAvatarStudentCommand>, UpdateAvatarValidator>();
        
        // Lesson Section
        services.AddScoped<IValidator<CreateLessonCommand>, CreateLessonCommandValidator>();
        services.AddScoped<IValidator<UpdateLessonCommand>, UpdateLessonCommandValidator>();
        services.AddScoped<IValidator<GetListLessonQuery>, GetListLessonValidator>();
        services.AddScoped<IValidator<UpdateLessonStatusCommand>, UpdateLessonStatusCommandValidator>();
        
        // Course Enrollment Section
        services.AddScoped<IValidator<CreateCourseEnrollmentCommand>, CreateCourseEnrollmentCommandValidator>();
        services.AddScoped<IValidator<UpdateCourseEnrollmentCommand>, UpdateCourseEnrollmentCommandValidator>();
        services.AddScoped<IValidator<GetListCourseEnrollmentQuery>, GetListCourseEnrollmentValidator>();
        services.AddScoped<IValidator<UpdateCourseEnrollmentStatusCommand>, UpdateCourseEnrollmentStatusValidator>();
        
        // Lesson Record Section
        services.AddScoped<IValidator<CreateLessonRecordCommand>, CreateLessonRecordCommandValidator>();
        services.AddScoped<IValidator<UpdateLessonRecordCommand>,  UpdateLessonRecordCommandValidator>();
        services.AddScoped<IValidator<CreateLessonScriptCommand>, CreateLessonScriptCommandValidator>();
        services.AddScoped<IValidator<UpdateLessonScriptCommand>, UpdateLessonScriptCommandValidator>();
        
        // File Storage Section (test)
        services.AddScoped<IValidator<UploadFileToDriveCommand>, UploadFileToDriveCommandValidator>();
        services.AddScoped<IValidator<GetFileFromDriveCommand>, GetFileFromDriveCommandValidator>();
        services.AddScoped<IValidator<DeleteFileFromDriveCommand>, DeleteFileFromDriveCommandValidator>();
        
        // Course Relationship Section
        services.AddScoped<IValidator<AddCourseModuleToCourseCommand>, AddCourseModuleToCourseCommandValidator>();
        services.AddScoped<IValidator<AddCourseSessionToCourseModuleCommand>, AddCourseSessionToCourseModuleCommandValidator>();
        services.AddScoped<IValidator<AddCourseResourceToCourseSessionCommand>, AddCourseResourceToCourseSessionCommandValidator>();
        
        // Course Section
        services.AddScoped<IValidator<UpdateCourseCommand>, UpdateCourseCommandValidator>();
        services.AddScoped<IValidator<CreateCourseCommand>, CreateCourseCommandValidator>();
        services.AddScoped<IValidator<CreateCourseResourceCommand>, CreateCourseResourceCommandValidator>();
        services.AddScoped<IValidator<UpdateCourseResourceCommand>, UpdateCourseResourceCommandValidator>();
        services.AddScoped<IValidator<UpdateThumbnailCourseCommand>, UpdateThumbnailCourseValidator>();
        services.AddScoped<IValidator<UpdateDemoVideoCourseCommand>, UpdateDemoVideoCourseValidator>();
        
        services.AddScoped<IValidator<CreateSupportTicketCommand>, CreateSupportTicketCommandValidator>();
        services.AddScoped<IValidator<UpdateSupportTicketCommand>, UpdateSupportTicketCommandValidator>();
        services.AddScoped<IValidator<UpdateSupportTicketStatusCommand>, UpdateSupportTicketStatusCommandValidator>();
        services.AddScoped<IValidator<CreateSupportTicketMessageCommand>, CreateSupportTicketMessageCommandValidator>();
        services.AddScoped<IValidator<UpdateSupportTicketMessageCommand>, UpdateSupportTicketMessageCommandValidator>();
        services.AddScoped<IValidator<UploadRecordingChunkCommand>, UploadRecordingChunkCommandValidator>();

        // Ai Summarize Section
        services.AddScoped<IValidator<GetAiSummaryCommand>, GetAiSummaryCommandValidator>();
        services.AddScoped<IValidator<UploadRecordingChunkCommand>, UploadRecordingChunkCommandValidator>();
    }
}
