using FluentValidation;
using Service.DTOs.Accounts;
using Service.DTOs.Users;

namespace ZensRestaurant.Validators.Users
{
    public class UserValidator : AbstractValidator<UserRegisterRequest>
    {
        public UserValidator()
        {
            #region UserName
            RuleFor(b => b.UserName)
                     .Cascade(CascadeMode.StopOnFirstFailure)
                     .NotNull().WithMessage("{PropertyName} is null.")
                     .NotEmpty().WithMessage("{PropertyName} is empty.")
                     .MaximumLength(100).WithMessage("{PropertyName} is required less than or equal to 100 characters.");

            #endregion

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
