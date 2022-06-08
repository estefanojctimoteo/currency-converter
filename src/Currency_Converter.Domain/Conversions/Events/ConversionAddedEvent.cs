using System;
using Currency_Converter.Domain.Core.Events;

namespace Currency_Converter.Domain.Conversions.Events
{
    public class ConversionAddedEvent : Event
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public string CurrencyFrom { get; private set; }
        public decimal AmountFrom { get; private set; }
        public string CurrencyTo { get; private set; }
        public decimal? AmountTo { get; private set; }
        public decimal? Fee { get; private set; }
        public DateTimeOffset? DateTimeUtc { get; private set; }

        public ConversionAddedEvent(Guid id, Guid userId, string currencyFrom, decimal amountFrom, string currencyTo, decimal? amountTo, decimal? fee, DateTimeOffset? dateTimeUtc)
        {
            Id = id;
            UserId = userId;
            CurrencyFrom = currencyFrom;
            AmountFrom = amountFrom;
            CurrencyTo = currencyTo;
            AmountTo = amountTo;
            Fee = fee;
            DateTimeUtc = dateTimeUtc;
        }
    }
}