using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movies.Domain.Entities;

namespace Movies.Infrastructure.EntitiesConfiguration
{
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
            builder.Property(p => p.RegistrationDate); // Não precisa do IsRequired pq é DateTime que sempre vem com data
            builder.Property(p => p.Active); // Valor padrão false

            builder.HasOne(p => p.Gender)
                .WithMany()
                .HasForeignKey(p => p.GenderId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade); // Se deletar um gênero, deleta todos os filmes desse gênero, e um filme precisa ter genero obrigatoriamente
        }
    }
}
