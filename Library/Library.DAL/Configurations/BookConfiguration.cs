using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.DAL.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            const int maxLength = 50;
            
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                   .UseIdentityColumn();
                
            builder.Property(m => m.Title)
                   .IsRequired()
                   .HasMaxLength(maxLength);

            builder.HasOne(m => m.Author)
                   .WithMany(a => a.Books)
                   .HasForeignKey(m => m.AuthorId);

            builder.ToTable("Books");
        }
    }
}