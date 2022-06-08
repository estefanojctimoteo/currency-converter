using AutoMapper;
using Currency_Converter.Services.Api.ViewModels;
using Currency_Converter.Domain.Conversions;
using Currency_Converter.Domain.Users;

namespace Currency_Converter.Services.Api.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Conversion, ConversionViewModel>();
            CreateMap<User, UserViewModel>();            
        }
    }
}