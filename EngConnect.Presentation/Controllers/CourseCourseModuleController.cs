// using EngConnect.Application.UseCases.CourseCourseModules.AddCourseModuleToCourse;
// using EngConnect.Application.UseCases.CourseCourseModules.Common;
// using EngConnect.Application.UseCases.CourseCourseModules.GetListCourseCourseModule;
// using EngConnect.Application.UseCases.CourseCourseModules.RemoveCourseModuleFromCourse;
// using EngConnect.BuildingBlock.Application.Base;
// using EngConnect.BuildingBlock.Application.Utils;
// using EngConnect.BuildingBlock.Contracts.Shared;
// using EngConnect.BuildingBlock.Presentation.Controllers;
// using Microsoft.AspNetCore.Mvc;
//
// namespace EngConnect.Presentation.Controllers;
//
// /// <summary>
// /// Api quản lý liên kết giữa Course và CourseModule
// /// </summary>
// [ApiController]
// [Route("api/courses-modules")]
// public class CourseCourseModuleController : BaseApiController
// {
//     private readonly ICommandDispatcher _commandDispatcher;
//     private readonly IQueryDispatcher _queryDispatcher;
//
//     public CourseCourseModuleController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
//     {
//         _commandDispatcher = commandDispatcher;
//         _queryDispatcher = queryDispatcher;
//     }
//
//     /// <summary>
//     /// Lấy danh sách các module trong một khóa học
//     /// </summary>
//     [HttpGet]
//     [Produces("application/json")]
//     [ProducesResponseType(typeof(Result<PaginationResult<GetCourseCourseModuleResponse>>), StatusCodes.Status200OK)]
//     public async Task<IActionResult> GetListAsync([FromQuery] GetListCourseCourseModuleQuery query, CancellationToken cancellationToken = default)
//     {
//         var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
//         return FromResult(result);
//     }
//     
//     /// <summary>
//     /// Thêm module vào khóa học
//     /// </summary>
//     [HttpPost]
//     [Produces("application/json")]
//     [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
//     public async Task<IActionResult> AddModuleToCourseAsync([FromBody] AddCourseModuleToCourseCommand command)
//     {
//         var result = await _commandDispatcher.DispatchAsync(command);
//         return FromResult(result);
//     }
//
//     /// <summary>
//     /// Xóa module khỏi khóa học
//     /// </summary>
//     [HttpDelete("{id}")]
//     [Produces("application/json")]
//     [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
//     public async Task<IActionResult> RemoveModuleFromCourseAsync([FromRoute] Guid id)
//     {
//         var command = new RemoveCourseModuleFromCourseCommand(id);
//         var result = await _commandDispatcher.DispatchAsync(command);
//         return FromResult(result);
//     }
// }
