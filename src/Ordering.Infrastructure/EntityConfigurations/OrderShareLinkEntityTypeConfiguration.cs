using eShop.Ordering.Infrastructure.Sharing;

namespace eShop.Ordering.Infrastructure.EntityConfigurations;

class OrderShareLinkEntityTypeConfiguration
    : IEntityTypeConfiguration<OrderShareLink>
{
    public void Configure(EntityTypeBuilder<OrderShareLink> shareLinkConfiguration)
    {
        shareLinkConfiguration.ToTable("ordersharelinks");

        shareLinkConfiguration.Property(l => l.Code)
            .HasMaxLength(16)
            .IsRequired();

        shareLinkConfiguration.HasIndex(l => l.Code)
            .IsUnique();

        shareLinkConfiguration.HasIndex(l => l.OrderId);

        shareLinkConfiguration.HasOne<Order>()
            .WithMany()
            .HasForeignKey(l => l.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
