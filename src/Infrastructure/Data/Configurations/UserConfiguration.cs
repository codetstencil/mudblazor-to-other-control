namespace Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.PasswordHash)
                .IsRequired();

            builder.Property(u => u.PasswordSalt)
                .IsRequired();

            builder.HasIndex(u => u.UserName)
                .IsUnique();
        }
    }
}
