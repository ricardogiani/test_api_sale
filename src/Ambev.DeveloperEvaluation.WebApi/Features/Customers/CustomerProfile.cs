using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;
using Ambev.DeveloperEvaluation.WebApi.Common;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
             
            CreateMap<GetCustomerResult, CustomerResponse>();

            //CreateMap<CreateCustomerRequest, CreateCustomerCommand>();
            //CreateMap<CreateCustomerResult, CreateCustomerResponse>();

            CreateMap<GetPaginatedCustomersResult, PaginatedResponse<CustomerResponse>>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Customers));
        }
    }
}