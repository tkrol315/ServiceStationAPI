using FluentValidation;
using ServiceStationAPI.Entities;

namespace ServiceStationAPI.Models.Validators
{
    public class LoginAccountDtoValidator:AbstractValidator<LoginAccountDto>
    {
        public LoginAccountDtoValidator(ServiceStationDbContext dbContext)
        {
            RuleFor(l => l.Email).EmailAddress();
            RuleFor(l =>l.Password).MinimumLength(8);
        }
    }
}
