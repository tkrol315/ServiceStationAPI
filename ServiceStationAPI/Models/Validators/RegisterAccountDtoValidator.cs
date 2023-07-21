using FluentValidation;
using ServiceStationAPI.Entities;
using System;

namespace ServiceStationAPI.Models.Validators
{
    public class RegisterAccountDtoValidator: AbstractValidator<RegisterAccountDto>
    {
      
        public RegisterAccountDtoValidator(ServiceStationDbContext dbContext)
        {
            RuleFor(r=>r.Name).NotEmpty();
            RuleFor(r => r.Surname).NotEmpty();
            RuleFor(r => r.Email).NotEmpty().EmailAddress();
            RuleFor(r => r.Email).Custom((value, context) =>
            {
                var emailTaken = dbContext.Users.Any(u => u.Email == value);
                if (emailTaken)
                    context.AddFailure("Email","That email is taken");
            });
            RuleFor(r => r.Password).MinimumLength(8);
            RuleFor(r => r.Password).Equal(e=>e.ConfirmPassword);
        }
    }
}
