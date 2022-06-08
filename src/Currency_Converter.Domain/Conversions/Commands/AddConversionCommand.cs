using System;
using Currency_Converter.Domain.Core.Commands;

namespace Currency_Converter.Domain.Conversions.Commands
{
    public class AddConversionCommand : Command
    {
        public Guid Id { get; set; }
        public Guid UserId { get; private set; }
        public string CurrencyFrom { get; private set; }
        public decimal AmountFrom { get; private set; }
        public string CurrencyTo { get; private set; }
        public decimal AmountTo { get; set; }
        public decimal Fee { get; set; }
        public DateTimeOffset DateTimeUtc { get; set; }

        public AddConversionCommand(Guid userId, string currencyFrom, decimal amountFrom, string currencyTo)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            CurrencyFrom = currencyFrom;
            AmountFrom = amountFrom;
            CurrencyTo = currencyTo;
        }
    }
}