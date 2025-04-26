using ERP.Server.Domain.Abstractions;

namespace ERP.Server.Domain.Entities;

public class Production : Entity
{
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public Guid DepotId { get; set; }
    public Depot? Depot { get; set; }
    public decimal Quantity { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

}
