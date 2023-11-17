using Microsoft.EntityFrameworkCore;
using Movies.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Movies.Infrastructure.EntitiesConfiguration
{
    public class GenderConfiguration : IEntityTypeConfiguration<Gender>
    {
        public void Configure(EntityTypeBuilder<Gender> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
            builder.Property(p => p.RegistrationDate);
            builder.Property(p => p.Active);
        }
    }
}
