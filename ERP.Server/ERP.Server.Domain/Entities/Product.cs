using ERP.Server.Domain.Abstractions;
using ERP.Server.Domain.Enums;

namespace ERP.Server.Domain.Entities;

public sealed class Product : Entity
{
    public string Name { get; set; } = string.Empty;
    public ProductTypeEnum Type { get; set; } = ProductTypeEnum.Product;

}
