using FluentValidation;
using Currency_Converter.Domain.Core.Models;
using Currency_Converter.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Currency_Converter.Domain.Conversions
{
    public class Conversion : Entity<Conversion>
    {
        public Guid UserId { get; private set; }
        public string CurrencyFrom { get; private set; }
        public decimal AmountFrom { get; private set; }
        public string CurrencyTo { get; private set; }
        public decimal? AmountTo { get; private set; }
        public decimal? Fee { get; private set; }
        public DateTimeOffset? DateTimeUtc { get; private set; }

        public Conversion(Guid id, Guid userId, string currencyFrom, decimal amountFrom, string currencyTo, decimal? amountTo, decimal? fee, DateTimeOffset? dateTimeUtc)
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
        
        protected Conversion() { }

        public virtual User User { get; set; }

        public override bool IsValid()
        {
            Validate();
            return ValidationResult.IsValid;
        }
        private void Validate()
        {
            ValidateUser();
            ValidateCurrencies();
            ValidateAmountFrom();
            ValidateFee();
            ValidateDateTimeUtc();

            ValidationResult = Validate(this);
        }
        private void ValidateCurrencies()
        {
            RuleFor(c => c.CurrencyFrom)
                .NotEqual(c => c.CurrencyTo)
                .WithMessage("[CurrencyFrom] and [CurrencyTo] must be different.");
        }
        private void ValidateUser()
        {
            RuleFor(c => c.UserId)
                .Must(BeAValidGuid)
                .WithMessage("It is mandatory to input the [UserId] as a valid Guid.");
        }
        private bool BeAValidGuid(Guid guid)
        {
            return guid.GetType() == typeof(Guid);
        }
        private void ValidateAmountFrom()
        {
            RuleFor(c => c.AmountFrom)
                .GreaterThan(0)
                .WithMessage("It is mandatory to input the [AmountFrom].");
        }
        private void ValidateFee()
        {
            RuleFor(c => c.Fee)
                .GreaterThan(0)
                .WithMessage("It is mandatory to input the [Fee].");
        }
        private void ValidateDateTimeUtc()
        {
            RuleFor(c => c.DateTimeUtc)
                .LessThan(DateTime.Now)
                .GreaterThan(new DateTime(0))
                .GreaterThan(DateTime.MinValue)
                .WithMessage("It is mandatory to input the [DateTimeUtc].");
        }
    }
}