using curso.api.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace curso.api.Infraestructure.Data.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("TB_USER");
            builder.HasKey(p => p.Code);
            builder.Property(p => p.Code).ValueGeneratedOnAdd();
            builder.Property(p => p.Login);
            builder.Property(p => p.Email);
            builder.Property(p => p.Password);
        }
    }
}
