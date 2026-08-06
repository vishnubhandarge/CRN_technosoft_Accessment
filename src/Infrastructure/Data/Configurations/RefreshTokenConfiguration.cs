using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshToken", "dbo");

        builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.Id)
            .ValueGeneratedOnAdd();

        builder.Property(rt => rt.Token)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(rt => rt.Expires)
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(rt => rt.Created)
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(rt => rt.CreatedByIp)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(rt => rt.Revoked)
            .HasColumnType("datetime")
            .IsRequired(false);

        builder.Property(rt => rt.RevokedByIp)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(rt => rt.ReplacedByToken)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
