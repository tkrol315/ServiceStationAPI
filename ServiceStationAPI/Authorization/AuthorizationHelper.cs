using Azure;
using Microsoft.AspNetCore.Authorization;
using ServiceStationAPI.Entities;
using ServiceStationAPI.Exceptions;
using ServiceStationAPI.Services;
using System.Security.Claims;

namespace ServiceStationAPI.Authorization
{
    public interface IAuthorizationHelper
    {
        Task ValidateAuthorization(Vehicle vehicle, ResourceOperation operation);      
    }
    public class AuthorizationHelper : IAuthorizationHelper
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;
        public AuthorizationHelper(IAuthorizationService authorizatonService, IUserContextService userContextService)
        {
            _authorizationService = authorizatonService;
            _userContextService = userContextService;
        }
        public async Task ValidateAuthorization(Vehicle vehicle, ResourceOperation operation)
        {
            var user = _userContextService.User;
            var AuthorizationResoult = await _authorizationService.AuthorizeAsync(user, vehicle, new ResourceOperationRequirement(operation));
            if (!AuthorizationResoult.Succeeded)
            {
                throw new ForbiddenException("Access denied");
            }
        }
    }
}
