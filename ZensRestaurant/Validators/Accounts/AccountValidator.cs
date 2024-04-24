using FluentValidation;
using Service.DTOs.Accounts;
namespace ZensRestaurant.Validators.Accounts
{
    public class AccountValidator : AbstractValidator<AccountRequest>
    {
        public AccountValidator()
        {
            #region Email
            RuleFor(b => b.Email)
                     .Cascade(CascadeMode.StopOnFirstFailure)
                     .NotNull().WithMessage("{PropertyName} is null.")
                     .NotEmpty().WithMessage("{PropertyName} is empty.")
                     .MaximumLength(100).WithMessage("{PropertyName} is required less than or equal to 100 characters.");

            #endregion

            #region Password
            RuleFor(b => b.Password)
                     .Cascade(CascadeMode.StopOnFirstFailure)
                     .NotNull().WithMessage("{PropertyName} is null.")
                     .NotEmpty().WithMessage("{PropertyName} is empty.")
                     .MaximumLength(50).WithMessage("{PropertyName} is required less than or equal to 50 characters.");

            #endregion
        }
    }
}
