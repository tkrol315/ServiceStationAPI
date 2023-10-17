using Microsoft.AspNetCore.Authorization;
using ServiceStationAPI.Entities;
using ServiceStationAPI.Exceptions;
using System.Security.Claims;

namespace ServiceStationAPI.Authorization
{
    public class VehicleOperationRequirementHandler : AuthorizationHandler< ResourceOperationRequirement, Vehicle>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement resourceOperationRequirement, Vehicle vehicle)
        {
            var userId = Guid.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var role = context.User.FindFirst(c => c.Type == ClaimTypes.Role).Value;
            if(role == "Manager")
            {
                context.Succeed(resourceOperationRequirement);
            }
            else if (resourceOperationRequirement.Operation == ResourceOperation.Create)
            {
                context.Succeed(resourceOperationRequirement);
            }
            else if (resourceOperationRequirement.Operation == ResourceOperation.ReadById && (userId == vehicle.OwnerId || role == "Mechanic"))
            {
                context.Succeed(resourceOperationRequirement);
            }
            else if(resourceOperationRequirement.Operation == ResourceOperation.Delete && userId == vehicle.OwnerId)
            {
                context.Succeed(resourceOperationRequirement);
            }
            return Task.CompletedTask;
        }
    }
}
