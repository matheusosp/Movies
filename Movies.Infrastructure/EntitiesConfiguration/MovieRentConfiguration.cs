using Microsoft.EntityFrameworkCore;
using Movies.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Movies.Infrastructure.EntitiesConfiguration
{
    public class MovieRentConfiguration : IEntityTypeConfiguration<MovieRent>
    {
        public void Configure(EntityTypeBuilder<MovieRent> builder)
        {
            builder.Property(p => p.CPFClient).HasMaxLength(14).IsRequired();
            builder.Property(p => p.RentDate);

            // Considerei que um filme pode ser alugado varias vezes e uma locação pode ter varios filmes, o filme pode
            // ser alugado varias vezes não considerando a quantidade de filmes disponiveis, algo assim, apenas vai considerar
            // se a Flag Active do Filme é True, poderia considerar que o filme teria
            // que ser alugado apenas uma vez, mas pra fazer sentido teria de ter uma propriedade de EndDateRent para só poder alugar o filme
            // quando o mesmo não esta alugado para outra pessoa
            builder
                .HasMany(mr => mr.Movies)     
                .WithMany(m => m.MovieRents)
                .UsingEntity(j => j.ToTable("MoviesRents"));
        }
    }
}
