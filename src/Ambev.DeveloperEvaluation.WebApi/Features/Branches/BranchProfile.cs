using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Branches;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches
{
    public class BranchProfile : Profile
    {
        public BranchProfile()
        {
            CreateMap<BranchResult, BranchResponse>();
        }
    }
}