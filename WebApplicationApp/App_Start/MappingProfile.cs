using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplicationApp.Dtos;
using WebApplicationApp.Models;

namespace WebApplicationApp.App_Start
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            //Domino a Dto
            Mapper.CreateMap<Customers, CustomerDto>();
            Mapper.CreateMap<MembershipType, MembershipTypeDto>();
            Mapper.CreateMap<Movie, MovieDto>();
            Mapper.CreateMap<Genre, GenreDto>();

            //Dto a Dominio

            Mapper.CreateMap<CustomerDto, Customers>()
                .ForMember(c => c.Id, opt => opt.Ignore());

            Mapper.CreateMap<MovieDto, Movie>()
    .ForMember(m => m.Id, opt => opt.Ignore());
        }
    }
}