using FluentValidation;

namespace ServiceStationAPI.Models.Validators
{
    public class UpdateVehicleDtoValidator:AbstractValidator<UpdateVehicleDto>
    {
        public UpdateVehicleDtoValidator()
        {
            RuleFor(u=>u.Brand).NotEmpty();
            RuleFor(u => u.Model).NotEmpty();
            RuleFor(u => u.RegistrationNumber).NotEmpty();
        }
    }
}
