using System.Text.Json.Serialization;
using EngConnect.BuildingBlock.Domain.Constants;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EngConnect.BuildingBlock.Application.Base;

public record AuditInfo
{
    [BindNever][JsonIgnore] public Guid? CreatedById { get; set; }
    [BindNever][JsonIgnore] public string? CreatedBy { get; set; }
    [BindNever][JsonIgnore] public UserRoleEnum? Role { get; set; }
}