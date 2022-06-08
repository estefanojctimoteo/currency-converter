using System;
using System.Runtime.Serialization;

namespace Currency_Converter.Services.Api.ViewModels
{
    public class AddUserViewModel
    {
        public string Email { get; set; }
    }
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
    }
    public class ConversionViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string CurrencyFrom { get; set; }
        public decimal AmountFrom { get; set; }
        public string CurrencyTo { get; set; }        
        public decimal? AmountTo { get; set; }
        public decimal? Fee { get; set; }
        public DateTimeOffset? DateTimeUtc { get; set; }
    }
    public class AddConversionViewModel
    {
        public Guid UserId { get; set; }
        public string CurrencyFrom { get; set; }
        public decimal AmountFrom { get; set; }
        public string CurrencyTo { get; set; }
    }
}