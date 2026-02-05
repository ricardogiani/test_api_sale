using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Guid, Application.Products.GetProduct.GetProductCommand>()
                .ConstructUsing(id => new Application.Products.GetProduct.GetProductCommand(id));
            CreateMap<GetProductResult, ProductResponse>();

            CreateMap<CreateProductRequest, CreateProductCommand>();
            CreateMap<CreateProductResult, CreateProductResponse>();

            CreateMap<UpdateProductRequest, UpdateProductCommand>();
            CreateMap<UpdateProductResult, CreateProductResponse>();

            CreateMap<GetPaginatedProductsResult, PaginatedResponse<ProductResponse>>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Products));

            
        }
    }
}