using AutoMapper;
using System.Numerics;
using Trains_FSD.Areas.Data;
using Trains_FSD.Models.Entities;
using Trains_FSD.ViewModels;

namespace Trains_FSD.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<City, CityVM>();

            CreateMap<Train, TrainVM>();

            CreateMap<User, UserSafeVM>();


            CreateMap<Traject, TrajectVM>();

            CreateMap<Order, OrderVM>().ForMember(dest => dest.Tickets, opt => opt.MapFrom(src => src.Tickets));
            CreateMap<OrderVM, Order>();


            CreateMap<TicketAddVM, Ticket>().ForMember(dest => dest.TicketDetails, opt => opt.MapFrom(src => src.TicketDetails)); 
            CreateMap<Ticket, TicketAddVM>();

            CreateMap<TicketDetailVM, TicketDetail>();
            CreateMap<TicketDetail, TicketDetailVM>();

            CreateMap<UserRegistrationVM, User>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.UserName));

            CreateMap<TrajectLine, TrajectLineVM>()
                .ForMember(a => a.Line, opt => opt.MapFrom(src => src.Line));

            CreateMap<Line, LineVM>()
                .ForMember(dest => dest.DepartureCity, opts => opts.MapFrom(src => src.DepartureCity))
                .ForMember(dest => dest.ArrivalCity, opts => opts.MapFrom(src => src.ArrivalCity))
                .ForMember(dest => dest.DepartureTime, opts => opts.MapFrom(src => DateTime.Today.Add(src.DepartureTime)))
                .ForMember(dest => dest.ArrivalTime, opts => opts.MapFrom(src => DateTime.Today.Add(src.ArrivalTime))); 

        }
    }
}
