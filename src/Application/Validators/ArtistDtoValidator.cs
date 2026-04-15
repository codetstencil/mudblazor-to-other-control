namespace Application.Validators
{
    public class ArtistDtoValidator : AbstractValidator<ArtistDto>
    {
        public ArtistDtoValidator()
        {

        }
    }
}

/* Example:

        public ArtistDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(160);

            RuleFor(x => x.ArtistId)
                .GreaterThan(0);
        }
*/
