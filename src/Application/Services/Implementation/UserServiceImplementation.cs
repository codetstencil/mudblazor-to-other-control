namespace Application.Services.Implementation
{
    public class UserServiceImplementation(
        IUserRepository userRepository,
        IValidator<UserDto> validator,
        UserManager<IdentityUser> userManager)
        : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        private readonly IValidator<UserDto> _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        private readonly UserManager<IdentityUser> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.ToDtos();
        }

        public async Task<UserDto?> GetByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Id cannot be empty", nameof(id));

            var user = await _userRepository.GetByIdAsync(id);
            return user?.ToDto();
        }

        public async Task<UserDto?> GetByUsernameAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("Username cannot be empty", nameof(username));

            var user = await _userRepository.GetByUsernameAsync(username);
            return user?.ToDto();
        }

        public async Task<PaginatedResult<UserDto>> GetPagedAsync(QueryParameters parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            var result = await _userRepository.GetPagedAsync(
                parameters.PageNumber,
                parameters.PageSize,
                parameters.SearchTerm,
                parameters.SortColumn,
                parameters.IsDescending);

            return new PaginatedResult<UserDto>
            {
                Items = result.Items.ToDtos(),
                TotalCount = result.TotalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }

        public async Task<UserDto> CreateAsync(UserDto userDto)
        {
            if (userDto == null)
                throw new ArgumentNullException(nameof(userDto));

            var validationResult = await _validator.ValidateAsync(userDto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var user = new IdentityUser
            {
                UserName = userDto.Username,
                Email = userDto.Email,
                PhoneNumber = userDto.PhoneNumber
            };

            var createdUser = await _userRepository.AddAsync(user, userDto.Password);
            return createdUser.ToDto();
        }

        public async Task<UserDto> UpdateAsync(UserDto userDto)
        {
            if (userDto == null)
                throw new ArgumentNullException(nameof(userDto));

            if (string.IsNullOrEmpty(userDto.Id))
                throw new ArgumentException("User Id cannot be empty", nameof(userDto.Id));

            var validationResult = await _validator.ValidateAsync(userDto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existingUser = await _userRepository.GetByIdAsync(userDto.Id);
            if (existingUser == null)
                throw new KeyNotFoundException($"User with ID {userDto.Id} not found");

            // Update user properties
            existingUser.UserName = userDto.Username;
            existingUser.Email = userDto.Email;
            existingUser.PhoneNumber = userDto.PhoneNumber;

            // If password is provided, update it
            if (!string.IsNullOrEmpty(userDto.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
                var passwordResult = await _userManager.ResetPasswordAsync(existingUser, token, userDto.Password);

                if (!passwordResult.Succeeded)
                {
                    var errors = string.Join(", ", passwordResult.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Failed to update password: {errors}");
                }
            }

            var updatedUser = await _userRepository.UpdateAsync(existingUser);
            return updatedUser.ToDto();
        }

        public async Task DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Id cannot be empty", nameof(id));

            await _userRepository.DeleteAsync(id);
        }
    }
}
