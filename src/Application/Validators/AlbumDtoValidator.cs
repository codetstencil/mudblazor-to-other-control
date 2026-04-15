namespace Application.Validators
{
    public class AlbumDtoValidator : AbstractValidator<AlbumDto>
    {
        public AlbumDtoValidator()
        {

        }
    }
}

/* Example:

        public AlbumDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(160);

            RuleFor(x => x.ArtistId)
                .GreaterThan(0);
        }
*/
