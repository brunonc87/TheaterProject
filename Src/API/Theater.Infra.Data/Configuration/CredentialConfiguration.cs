using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theater.Domain.Credentials;

namespace Theater.Infra.Data.Configuration
{
    public class CredentialConfiguration : IEntityTypeConfiguration<Credential>
    {
        public void Configure(EntityTypeBuilder<Credential> builder)
        {
            builder.ToTable("Credentials");
            builder.HasKey(c => c.CredentialID).HasName("CredentialID");
            builder.Property(c => c.Login).IsRequired().HasMaxLength(250).HasColumnName("Login");
            builder.Property(c => c.Password).IsRequired().HasMaxLength(250).HasColumnName("Password");
        }
    }
}
