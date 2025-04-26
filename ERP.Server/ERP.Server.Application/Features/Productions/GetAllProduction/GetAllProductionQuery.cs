using ERP.Server.Domain.Entities;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Productions.GetAllProduction;

public sealed record GetAllProductionQuery() : IRequest<Result<List<Production>>>;
