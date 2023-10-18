using Microsoft.AspNetCore.Authorization;

namespace ServiceStationAPI.Authorization
{
    public enum ResourceOperation
    {
        Read,
        ReadById,
        CreateVehicle,
        CreateOrderNote,
        Update,
        Delete,
    }
    public class ResourceOperationRequirement : IAuthorizationRequirement
    {
        public ResourceOperation Operation { get;}

        public ResourceOperationRequirement(ResourceOperation operation)
        {
            Operation = operation;
        }
    }
}
