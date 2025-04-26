using AutoMapper;
using ERP.Server.Application.Features.Customers.CreateCustomer;
using ERP.Server.Application.Features.Customers.UpdateCustomer;
using ERP.Server.Application.Features.Depots.CreateDepot;
using ERP.Server.Application.Features.Depots.UpdateDepot;
using ERP.Server.Application.Features.Invoices.CreateInvoice;
using ERP.Server.Application.Features.Invoices.UpdateInvoice;
using ERP.Server.Application.Features.Orders.CreateOrder;
using ERP.Server.Application.Features.Orders.UpdateOrder;
using ERP.Server.Application.Features.Productions.CreateProduction;
using ERP.Server.Application.Features.Products.CreateProduct;
using ERP.Server.Application.Features.Products.UpdateProduct;
using ERP.Server.Application.Features.RecipeDetails.CreateRecipeDetail;
using ERP.Server.Application.Features.RecipeDetails.UpdateRecipeDetail;
using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Enums;

namespace ERP.Server.Application.Mapping;
public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateCustomerCommand, Customer>();
        CreateMap<UpdateCustomerCommand, Customer>();

        CreateMap<CreateDepotCommand, Depot>();
        CreateMap<UpdateDepotCommand, Depot>();

        CreateMap<CreateProductCommand, Product>()
         .ForMember(member => member.Type,
             options =>
             options.MapFrom(p => ProductTypeEnum.FromValue(p.TypeValue)));

        CreateMap<UpdateProductCommand, Product>()
           .ForMember(member => member.Type,
               options =>
               options.MapFrom(p => ProductTypeEnum.FromValue(p.TypeValue)));

        CreateMap<CreateRecipeDetailCommand, RecipeDetail>();
        CreateMap<UpdateRecipeDetailCommand, RecipeDetail>();

        CreateMap<CreateOrderCommand, Order>()
            .ForMember(member => member.Details,
            options =>
            options.MapFrom(p => p.Details.Select(s => new OrderDetail
            {
                Price = s.Price,
                ProductId = s.ProductId,
                Quantity = s.Quantity
            }).ToList()));

        CreateMap<UpdateOrderCommand, Order>()
           .ForMember(member => member.Details, options =>
                 options.Ignore());

        CreateMap<CreateInvoiceCommand, Invoice>()
           .ForMember(member => member.Type, options =>
           options.MapFrom(p => InvoiceTypeEnum.FromValue(p.TypeValue)))
          .ForMember(member => member.Details,
          options =>
          options.MapFrom(p => p.Details.Select(s => new InvoiceDetail
          {
              Price = s.Price,
              ProductId = s.ProductId,
              DepotId = s.DepotId,
              Quantity = s.Quantity
          }).ToList()));

        CreateMap<UpdateInvoiceCommand, Invoice>()
         .ForMember(member => member.Details, options =>
               options.Ignore());

        CreateMap<CreateProductionCommand, Production>();
    }
}
