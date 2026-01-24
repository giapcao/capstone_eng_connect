using EngConnect.Application.UseCases.Students.CreateStudent;
using EngConnect.Application.UseCases.Students.UpdateStudent;
using EngConnect.Application.UseCases.User.Common;
using EngConnect.Domain.Persistence.Models;
using Mapster;

namespace EngConnect.Application.Mapping
{
    public static class MappingConfig
    {
        public static void RegisterMappings()
        {
            //Add mapping config
            // TypeAdapterConfig<ProductVariant, GetProductVariantResponse>.NewConfig()
            //     .Map(dest => dest.VariantDetail, src => src.ProductVariantDetails);

            TypeAdapterConfig<CreateStudentCommand, Student>.NewConfig();
            TypeAdapterConfig<UpdateStudentCommand, Student>
                .NewConfig();
        }
    }
}