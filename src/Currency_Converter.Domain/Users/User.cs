using System.Collections.Generic;
using Currency_Converter.Domain.Conversions;
using Currency_Converter.Domain.Core.Models;
using FluentValidation;
using System;
using Currency_Converter.Domain.Interfaces;

namespace Currency_Converter.Domain.Users
{
    public class User : Entity<User>, IUser
    {
        public string Email { get; private set; }        

        public User(Guid id, string email)
        {
            Id = id;
            Email = email;
        }

        protected User()
        {
        }

        public virtual ICollection<Conversion> Conversion { get; set; }

        public override bool IsValid()
        {
            Validate();
            return ValidationResult.IsValid;
        }
        private void Validate()
        {
            ValidateEmail();

            ValidationResult = Validate(this);
        }
        private void ValidateEmail()
        {
            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email address is required")
                .EmailAddress().WithMessage("A valid email is required");
        }

        public Guid GetUserId()
        {
            return Id;
        }
    }
}