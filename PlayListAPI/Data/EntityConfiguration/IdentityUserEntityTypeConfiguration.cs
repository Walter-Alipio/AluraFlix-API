using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class IdentityAdminConfig : IEntityTypeConfiguration<IdentityUser>
{
    private readonly IConfiguration _configuration;

    public IdentityAdminConfig(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(EntityTypeBuilder<IdentityUser> builder)
    {
        // Configurações da entidade IdentityUser aqui

        // Criação do superadmin aqui
        IdentityUser superAdmin = new IdentityUser
        {
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@admin.com",
            NormalizedEmail = "ADMIN@ADMIN.COM",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString(),
            Id = "99999"
        };

        PasswordHasher<IdentityUser> hasher = new PasswordHasher<IdentityUser>();

        superAdmin.PasswordHash = hasher.HashPassword(superAdmin,
        _configuration.GetValue<string>("AdminInfo:Password"));

        builder.HasData(superAdmin);
    }
}
