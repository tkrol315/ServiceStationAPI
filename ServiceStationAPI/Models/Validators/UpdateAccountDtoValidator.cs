using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ServiceStationAPI.Entities;

namespace ServiceStationAPI.Models.Validators
{
    public class UpdateAccountDtoValidator:AbstractValidator<UpdateAccountDto>
    {
        public UpdateAccountDtoValidator(ServiceStationDbContext dbContext)
        {
            RuleFor(u => u.Name).NotEmpty();
            RuleFor(u => u.Surname).NotEmpty();
            RuleFor(u => u.RoleId).InclusiveBetween(1, 3)
                .WithMessage("RoleId must be between 1 and 3.");
        }
    }
}
