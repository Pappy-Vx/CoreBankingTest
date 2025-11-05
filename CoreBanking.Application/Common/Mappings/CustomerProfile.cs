using AutoMapper;
using CoreBanking.Application.Customers.Commands.CreateCustomer;
using CoreBanking.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBanking.Application.Common.Mappings
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            // Map from API request DTO to command
            CreateMap<CreateCustomerCommand, Customer>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));

            // Optional: Reverse map if needed (command to request for responses)
            // CreateMap<CreateCustomerCommand, CreateCustomerRequest>();
        }
    }
}
