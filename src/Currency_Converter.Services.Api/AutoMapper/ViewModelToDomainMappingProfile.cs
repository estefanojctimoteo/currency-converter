using AutoMapper;
using Currency_Converter.Services.Api.ViewModels;
using Currency_Converter.Domain.Users.Commands;
using Currency_Converter.Domain.Conversions.Commands;

namespace Currency_Converter.Services.Api.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            #region Users

            CreateMap<AddUserViewModel, AddUserCommand>()
                .ConstructUsing(u => new AddUserCommand(u.Email));

            #endregion

            #region Conversions

            CreateMap<AddConversionViewModel, AddConversionCommand>()
                .ConstructUsing(c => new AddConversionCommand(c.UserId, c.CurrencyFrom, c.AmountFrom, c.CurrencyTo));

            #endregion
        }
    }
}