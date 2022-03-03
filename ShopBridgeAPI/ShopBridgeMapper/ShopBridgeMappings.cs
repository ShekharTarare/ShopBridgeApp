using AutoMapper;
using ShopBridgeAPI.Models;
using ShopBridgeAPI.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridgeAPI.ShopBridgeMapper
{
    public class ShopBridgeMappings: Profile
    {
        public ShopBridgeMappings()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
