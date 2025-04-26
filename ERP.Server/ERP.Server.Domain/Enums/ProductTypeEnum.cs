using Ardalis.SmartEnum;

namespace ERP.Server.Domain.Enums;

public sealed class ProductTypeEnum : SmartEnum<ProductTypeEnum>
{
    public static readonly ProductTypeEnum Product = new("Mamül", 1);
    public static readonly ProductTypeEnum SemiProduct = new("Yarı Mamül", 2);

    public ProductTypeEnum(string name, int value) : base(name, value)
    {
    }
}
