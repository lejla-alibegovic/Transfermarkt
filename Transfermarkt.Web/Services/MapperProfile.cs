using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transfermarkt.Web.Models;
using Transfermarkt.Web.ViewModels;

namespace Transfermarkt.Web.Services
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Player, Player>();
            CreateMap<Player, PlayerInputVM>();
            CreateMap<Player, ContractInputVM>();
            CreateMap<ContractInputVM, Contract>();
        }
    }
}
