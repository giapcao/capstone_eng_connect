using EngConnect.Application.UseCases.CourseModules.Common;
using EngConnect.Application.UseCases.Courses.Common;
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
            TypeAdapterConfig<CourseModule, GetCourseModuleResponseInCourse>.NewConfig()
                .Map(dest => dest.ModuleTitle, src => src.Title)
                .Map(dest => dest.ModuleDescription, src => src.Description)
                .Map(dest => dest.ModuleOutcomes, src => src.Outcomes);
            
            //Course Session Mapping
            TypeAdapterConfig<CourseSession, GetSessonResponseInCourseModule>.NewConfig()
                .Map(dest => dest.SessionTitle, src => src.Title)
                .Map(dest => dest.SessionDescription, src => src.Description)
                .Map(dest => dest.SessionOutcomes, src => src.Outcomes);    
           
            //Course Category Mapping
            TypeAdapterConfig<CourseCategory, GetCourseCategoryResponseInCourse>.NewConfig()
                .Map(dest => dest.CategoryName, src => src.Category!.Name)
                .Map(dest => dest.CategoryDescription, src => src.Category!.Description)
                .Map(dest => dest.CategoryType, src => src.Category!.Type);
        }
    }
}