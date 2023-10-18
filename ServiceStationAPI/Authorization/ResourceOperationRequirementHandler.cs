using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Client.Region;
using ServiceStationAPI.Entities;
using ServiceStationAPI.Exceptions;
using System.Security.Claims;

namespace ServiceStationAPI.Authorization
{
    public class ResourceOperationRequirementHandler : AuthorizationHandler< ResourceOperationRequirement, Vehicle>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement resourceOperationRequirement, Vehicle vehicle)
        {
            var userId = Guid.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var role = context.User.FindFirst(c => c.Type == ClaimTypes.Role).Value;
            if(role == "Manager")
            {
                context.Succeed(resourceOperationRequirement);
            }
            else if (resourceOperationRequirement.Operation == ResourceOperation.CreateVehicle)
            {
                context.Succeed(resourceOperationRequirement);
            }
            else if(resourceOperationRequirement.Operation == ResourceOperation.CreateOrderNote && role == "Mechanic")
            {
                context.Succeed(resourceOperationRequirement);
            }
            else if ((resourceOperationRequirement.Operation == ResourceOperation.ReadById || resourceOperationRequirement.Operation == ResourceOperation.Read)
                && (userId == vehicle.OwnerId || role == "Mechanic"))
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
