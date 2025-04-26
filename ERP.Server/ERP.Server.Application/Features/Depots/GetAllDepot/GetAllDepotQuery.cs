using ERP.Server.Domain.Entities;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Depots.GetAllDepot;

public sealed record GetAllDepotQuery : IRequest<Result<List<Depot>>>;
