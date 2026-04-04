using EngConnect.Application.UseCases.CourseModules.Common;
using EngConnect.Application.UseCases.Courses.Common;
using EngConnect.Application.UseCases.Lessons.Common;
using EngConnect.Application.UseCases.Users.Common;
using EngConnect.Domain.Persistence.Models;
using Mapster;

namespace EngConnect.Application.Mapping
{
    public static class MappingConfig
    {
        public static void RegisterMappings()
        {
            //Add mapping config
            
            //User Role Mapping
            TypeAdapterConfig<UserRole, GetRoleForUserResponse>.NewConfig()
                .Map(dest => dest.RoleCode, src => src.Role!.Code)
                .Map(dest => dest.RoleDescription, src => src.Role!.Description);
            
            //Permission Role Mapping
            TypeAdapterConfig<PermissionRole, GetPermissionResponseForUser>.NewConfig()
                .Map(dest => dest.PermissionCode, src => src.Permission!.Code)
                .Map(dest => dest.PermissionDescription, src => src.Permission!.Description);
            
            //Course Module Mapping
            TypeAdapterConfig<CourseCourseModule, GetCourseModuleResponseInCourse>.NewConfig()
                .Map(dest => dest.ModuleTitle, src => src.CourseModule!.Title)
                .Map(dest => dest.ModuleDescription, src => src.CourseModule!.Description)
                .Map(dest => dest.ModuleOutcomes, src => src.CourseModule!.Outcomes)
                .Map(dest => dest.ParentModuleId, src => src.CourseModule!.ParentModuleId)
                .Map(dest => dest.ModuleNumber, src => src.ModuleNumber);
            
            //Course Session Mapping
            TypeAdapterConfig<CourseModuleCourseSession, GetSessonResponseInCourseModule>.NewConfig()
                .Map(dest => dest.SessionTitle, src => src.CourseSession!.Title)
                .Map(dest => dest.SessionDescription, src => src.CourseSession!.Description)
                .Map(dest => dest.SessionOutcomes, src => src.CourseSession!.Outcomes)
                .Map(dest => dest.ParentSessionId, src => src.CourseSession!.ParentSessionId)
                .Map(dest => dest.SessionNumber, src => src.SessionNumber);    
           
            //Course Category Mapping
            TypeAdapterConfig<CourseCategory, GetCourseCategoryResponseInCourse>.NewConfig()
                .Map(dest => dest.CategoryName, src => src.Category!.Name)
                .Map(dest => dest.CategoryDescription, src => src.Category!.Description)
                .Map(dest => dest.CategoryType, src => src.Category!.Type);
            
            //Course Mapping with nested Course Modules and Course Sessions
            TypeAdapterConfig<Course, GetCourseResponseDetail>
                .NewConfig()
                .Map(dest => dest.CourseCourseModules,
                    src => src.CourseCourseModules.Select(ccm => new GetCourseModuleResponseInCourse
                    {
                        Id = ccm.Id,
                        CourseModuleId = ccm.CourseModuleId,
                        ParentModuleId = ccm.CourseModule.ParentModuleId,
                        ModuleTitle = ccm.CourseModule.Title,
                        ModuleDescription = ccm.CourseModule.Description,
                        ModuleOutcomes = ccm.CourseModule.Outcomes,
                        ModuleNumber = ccm.ModuleNumber,

                        CourseModuleCourseSessions = ccm.CourseModule.CourseModuleCourseSessions
                            .Select(cs => new GetSessonResponseInCourseModule
                            {
                                Id = cs.Id,
                                CourseSessionId = cs.CourseSessionId,
                                ParentSessionId = cs.CourseSession.ParentSessionId,
                                SessionTitle = cs.CourseSession.Title,
                                SessionDescription = cs.CourseSession.Description,
                                SessionOutcomes = cs.CourseSession.Outcomes,
                                SessionNumber = cs.SessionNumber
                            }).ToList()
                    }));
            //Course Module Mapping with nested Course Sessions
            TypeAdapterConfig<CourseModule, GetCourseModuleDetailResponse>
                .NewConfig()
                .Map(dest => dest.CourseSessions,
                    src => src.CourseModuleCourseSessions.Select(cs => new GetSessonResponseInCourseModule
                    {
                        Id = cs.Id,
                        CourseSessionId = cs.CourseSessionId,
                        ParentSessionId = cs.CourseSession.ParentSessionId,
                        SessionTitle = cs.CourseSession.Title,
                        SessionDescription = cs.CourseSession.Description,
                        SessionOutcomes = cs.CourseSession.Outcomes,
                        SessionNumber = cs.SessionNumber
                    }).ToList());
            
            TypeAdapterConfig<Lesson, GetLessonResponse>
                .NewConfig()
                .Map(dest => dest.CourseId, src => src.Enrollment.CourseId);
        }
    }
}
