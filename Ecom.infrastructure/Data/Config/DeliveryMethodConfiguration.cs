using Ecom.core.Entity.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecom.infrastructure.Data.Config
{
    public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(m => m.Price).HasColumnType("decimal(18, 2)");

            builder.HasData(
                new DeliveryMethod
                {
                    Id = 1,
                    DeliveryTime = "Only a week",
                    Description = "The fastest delivery in the world",
                    Name = "DHL",
                    Price = 15
                },
                new DeliveryMethod
                {
                    Id = 2,
                    DeliveryTime = "Only take two weeks",
                    Description = "Name your product safe",
                    Name = "xxx",
                    Price = 12
                }
            );
        }
    }
}
