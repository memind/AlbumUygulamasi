using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Mapping
{
    internal class AlbumMapping : IEntityTypeConfiguration<Album>
    {
        public void Configure(EntityTypeBuilder<Album> builder)
        {
            builder.ToTable("Albums");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd()
                   .HasColumnType("int")
                   .HasColumnName("AlbumId")
                   .HasColumnOrder(1);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasColumnName("AlbumName")
                   .HasColumnType("nvarchar(max)")
                   .HasColumnOrder(2);

            builder.Property(x => x.Artist)
                   .IsRequired()
                   .HasColumnName("Artist")
                   .HasColumnType("nvarchar(max)")
                   .HasColumnOrder(3);

            builder.Property(x => x.ReleaseDate)
                   .IsRequired()
                   .HasColumnName("ReleaseDate")
                   .HasColumnType("datetime2")
                   .HasColumnOrder(4);

            builder.Property(x => x.Price)
                   .IsRequired()
                   .HasColumnName("Price")
                   .HasColumnType("money")
                   .HasColumnOrder(5);

            builder.Property(x => x.Discount)
                   .IsRequired()
                   .HasColumnName("Discount")
                   .HasColumnType("float")
                   .HasColumnOrder(6);

            builder.Property(x => x.IsContinued)
                   .IsRequired()
                   .HasColumnName("IsContinued")
                   .HasColumnType("bit")
                   .HasColumnOrder(7);
        }
    }
}
