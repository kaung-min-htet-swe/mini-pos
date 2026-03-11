using System;
using System.Collections.Generic;

namespace ms_sql;

public partial class Order
{
    public Guid Id { get; set; }

    public Guid? CustomerId { get; set; }

    public Guid? ProcessedById { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Admin? ProcessedBy { get; set; }
}
