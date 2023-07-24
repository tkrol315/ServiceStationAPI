using FluentValidation;

namespace ServiceStationAPI.Models.Validators
{
    public class CreateVehicleDtoValidator:AbstractValidator<CreateVehicleDto>
    {
        public CreateVehicleDtoValidator()
        {
            RuleFor(c=>c.Brand).NotEmpty();
            RuleFor(c => c.Model).NotEmpty();
            RuleFor(c => c.Vin).NotEmpty();
            RuleFor(c => c.RegistrationNumber).NotEmpty();

        }
    }
}
