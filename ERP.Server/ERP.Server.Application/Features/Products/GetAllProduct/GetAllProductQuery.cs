using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Products.GetAllProduct;

public sealed record GetAllProductQuery() : IRequest<Result<List<GetAllProductQueryResponse>>>;
