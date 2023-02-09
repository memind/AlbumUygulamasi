using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Security.Cryptography.X509Certificates;

namespace DataAccessLayer.Mapping
{
    internal class AdminMapping : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.ToTable("Admins");

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Name)
                   .IsUnique();

            builder.Property(x => x.Id)
                   .IsRequired()
                   .HasColumnType("int")
                   .HasColumnName("AdminId")
                   .ValueGeneratedOnAdd()
                   .HasColumnOrder(1);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasColumnType("nvarchar(50)")
                   .HasColumnName("UserName")
                   .HasColumnOrder(2);

            builder.Property(x => x.Password)
                   .IsRequired()
                   .HasColumnType("nvarchar(64)")
                   .HasColumnName("Password")
                   .HasColumnOrder(3);
        }
    }
}
