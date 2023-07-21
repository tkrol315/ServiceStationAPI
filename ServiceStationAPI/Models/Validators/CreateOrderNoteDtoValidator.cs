using FluentValidation;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ServiceStationAPI.Models.Validators
{
    public class CreateOrderNoteDtoValidator:AbstractValidator<CreateOrderNoteDto>
    {
        public CreateOrderNoteDtoValidator()
        {
            RuleFor(c=>c.Title).NotEmpty();
            RuleFor(c => c.Description).NotEmpty();
            RuleFor(c => c.VehicleId).NotEmpty();
            RuleFor(c => c.CreatorName).NotEmpty();
            RuleFor(c => c.CreatorSurname).NotEmpty();
            RuleFor(c => c.CreatorEmail).NotEmpty().EmailAddress();
        }
    }
}
