using CustomValidation.Models;
using FluentValidation;

namespace CustomValidation.Validators
{
	public class PersonValidator : AbstractValidator<Person>
	{
		public PersonValidator()
		{
			RuleFor(x => x.Name).NotEmpty();
			RuleFor(x => x.Age).InclusiveBetween(18, 80);
		}
	}
}
