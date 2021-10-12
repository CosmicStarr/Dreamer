using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Orders;

namespace Data
{
    public class OrderConfiguration : IEntityTypeConfiguration<ActualOrder>
    {
        public void Configure(EntityTypeBuilder<ActualOrder> builder)
        {
            builder.OwnsOne(o => o.ShippingAddress, a =>
            {
                a.WithOwner();
            });
            builder.Property(s =>s.Status).HasConversion(
                c => c.ToString(),c => (OrderStatus) Enum.Parse(typeof(OrderStatus),c)
            );
            builder.HasMany(o=>o.OrderedItems).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}