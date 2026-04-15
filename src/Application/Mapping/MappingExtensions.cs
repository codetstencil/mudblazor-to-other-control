using Microsoft.AspNetCore.Identity;

namespace Application.Mapping
{
            public static class MappingExtensions
        {

            // Album <-> AlbumDto
            public static AlbumDto ToDto(this Album album) => new()
            {
                AlbumId = album.AlbumId,
                Title = album.Title,
                ArtistId = album.ArtistId
            };

            public static Album ToEntity(this AlbumDto dto) => new()
            {
                AlbumId = dto.AlbumId,
                Title = dto.Title,
                ArtistId = dto.ArtistId
            };

            public static void MapTo(this AlbumDto dto, Album entity)
            {
                entity.AlbumId = dto.AlbumId;
                entity.Title = dto.Title;
                entity.ArtistId = dto.ArtistId;
            }

            // Artist <-> ArtistDto
            public static ArtistDto ToDto(this Artist artist) => new()
            {
                ArtistId = artist.ArtistId,
                Name = artist.Name
            };

            public static Artist ToEntity(this ArtistDto dto) => new()
            {
                ArtistId = dto.ArtistId,
                Name = dto.Name
            };

            public static void MapTo(this ArtistDto dto, Artist entity)
            {
                entity.ArtistId = dto.ArtistId;
                entity.Name = dto.Name;
            }

            // AlbumView <-> AlbumViewDto
            public static AlbumViewDto ToDto(this AlbumView albumview) => new()
            {
                Title = albumview.Title,
                Name = albumview.Name
            };


            // IdentityUser <-> UserDto
            public static UserDto ToDto(this IdentityUser user) => new()
            {
                Id = user.Id,
                Username = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                TwoFactorEnabled = user.TwoFactorEnabled
            };


            // Collection helpers
            public static IEnumerable<AlbumDto> ToDtos(this IEnumerable<Album> albums) =>
                albums.Select(a => a.ToDto());

            public static IEnumerable<ArtistDto> ToDtos(this IEnumerable<Artist> artists) =>
                artists.Select(a => a.ToDto());

            public static IEnumerable<AlbumViewDto> ToDtos(this IEnumerable<AlbumView> albumviews) =>
                albumviews.Select(a => a.ToDto());

            public static IEnumerable<UserDto> ToDtos(this IEnumerable<IdentityUser> users) =>
                users.Select(u => u.ToDto());


            // AuthResult -> AuthResponseDto
            public static AuthResponseDto ToDto(this AuthResult authResult) => new()
            {
                IsAuthenticated = authResult.Success,
                Token = authResult.Token,
                Message = authResult.Message
            };

        }

}

