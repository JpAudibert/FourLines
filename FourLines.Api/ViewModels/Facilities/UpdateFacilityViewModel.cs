namespace FourLines.Api.ViewModels.Facilities
{
    public class UpdateFacilityViewModel
    {
        [Required(ErrorMessage = "Facility name is required.")]
        public string Name { get; init; } = default!;

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; init; } = default!;

        [Required(ErrorMessage = "City is required.")]
        public string City { get; init; } = default!;

        [Required(ErrorMessage = "State is required.")]
        public string State { get; init; } = default!;

        [Required(ErrorMessage = "Zip code is required.")]
        [MaxLength(9)]
        public string ZipCode { get; init; } = default!;

        [Required(ErrorMessage = "Registration number is required.")]
        [MaxLength(18)]
        public string RegistrationNumber { get; init; } = default!;
    }
}
