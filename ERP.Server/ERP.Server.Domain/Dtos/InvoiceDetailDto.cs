﻿namespace ERP.Server.Domain.Dtos;

public sealed record InvoiceDetailDto(
    Guid ProductId,
    Guid DepotId,
    decimal Quantity,
    decimal Price);