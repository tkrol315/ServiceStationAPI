using FluentValidation;

namespace ServiceStationAPI.Models.Validators
{
    public class CreateVehicleDtoValidator:AbstractValidator<CreateVehicleDto>
    {
        public CreateVehicleDtoValidator()
        {
            RuleFor(c=>c.Brand).NotEmpty();
            RuleFor(c => c.Model).NotEmpty();
            RuleFor(c => c.OwnerName).NotEmpty();
            RuleFor(c => c.OwnerSurname).NotEmpty();
            RuleFor(c => c.Email).EmailAddress();
            RuleFor(c => c.Vin).NotEmpty();
            RuleFor(c => c.RegistrationNumber).NotEmpty();

        }
    }
}
