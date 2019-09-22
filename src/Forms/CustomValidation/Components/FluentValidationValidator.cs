using Microsoft.AspNetCore.Components;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Threading.Tasks;

namespace CustomValidation.Components
{
	public class FluentValidationValidator : ComponentBase
	{
		/// <summary>
		/// The EditContext cascaded to us from the EditForm component.
		/// This changes whenever EditForm.Model changes
		/// </summary>
		[CascadingParameter]
		public EditContext EditContext { get; set; }

		[Parameter]
		public Type ValidatorType { get; set; }

		// Holds an instance to perform our actual validation
		private IValidator Validator;

		// This is where we register our validation errors for Blazor to pick up
		// in the UI. Like EditContext, this instance should be discarded when
		// EditForm.Model changes (for us, that's when EditContext changes).
		private ValidationMessageStore ValidationMessageStore;

		// Inject the service provider so we can create our IValidator instances
		[Inject]
		private IServiceProvider ServiceProvider { get; set; }

		/// <summary>
		/// Executed when a parameter or cascading parameter changes. We need
		/// to know if the EditContext has changed as a consequence of
		/// EditForm.Model changing.
		/// See http://blazor-university.com/components/component-lifecycles/ (component lifecycles).
		/// </summary>
		public override async Task SetParametersAsync(ParameterView parameters)
		{
			// Keep a reference to the original values so we can check if they have changed
			EditContext previousEditContext = EditContext;
			Type previousValidatorType = ValidatorType;

			await base.SetParametersAsync(parameters);

			if (EditContext == null)
				throw new NullReferenceException($"{nameof(FluentValidationValidator)} must be placed within an {nameof(EditForm)}");

			if (ValidatorType == null)
				throw new NullReferenceException($"{nameof(ValidatorType)} must be specified.");

			if (!typeof(IValidator).IsAssignableFrom(ValidatorType))
				throw new ArgumentException($"{ValidatorType.Name} must implement {typeof(IValidator).FullName}");

			if (ValidatorType != previousValidatorType)
				ValidatorTypeChanged();

			// If the EditForm.Model changes then we get a new EditContext
			// and need to hook it up
			if (EditContext != previousEditContext)
				EditContextChanged();
		}

		/// <summary>
		/// We create a new instance of the validator whenever ValidatorType changes.
		/// </summary>
		private void ValidatorTypeChanged()
		{
			Validator = (IValidator)ServiceProvider.GetService(ValidatorType);
		}

		/// <summary>
		/// We trigger this when SetParametersAsync is executed and results in us having a
		/// new EditContext.
		/// </summary>
		void EditContextChanged()
		{
			System.Diagnostics.Debug.WriteLine("EditContext has changed");

			// We need this to store our validation errors
			// Whenever we get a new EditContext (because EditForm.Model has changed)
			// we also need to discard our old message store and create a new one
			ValidationMessageStore = new ValidationMessageStore(EditContext);
			System.Diagnostics.Debug.WriteLine("New ValidationMessageStore created");

			// Observe any changes to the EditForm.Model object
			HookUpEditContextEvents();
		}

		private void HookUpEditContextEvents()
		{
			// We need to know when to validate the whole object, this
			// is triggered when the EditForm is submitted
			EditContext.OnValidationRequested += ValidationRequested;

			// We need to know when to validate an individual property, this
			// is triggered when the user edits something
			EditContext.OnFieldChanged += FieldChanged;

			System.Diagnostics.Debug.WriteLine("Hooked up EditContext events (OnValidationRequested and OnFieldChanged)");
		}

		async void ValidationRequested(object sender, ValidationRequestedEventArgs args)
		{
			System.Diagnostics.Debug.WriteLine("OnValidationRequested triggered: Validating whole object");

			// Clear all errors from a previous validation
			ValidationMessageStore.Clear();

			// Tell FluentValidation to validate the object
			ValidationResult result = await Validator.ValidateAsync(EditContext.Model);

			// Now add the results to the ValidationMessageStore we created
			AddValidationResult(EditContext.Model, result);
		}

		async void FieldChanged(object sender, FieldChangedEventArgs args)
		{
			System.Diagnostics.Debug.WriteLine($"OnFieldChanged triggered: Validating a single property named {args.FieldIdentifier.FieldName}" +
				$" on class {args.FieldIdentifier.Model.GetType().Name}");

			// Create a FieldIdentifier to identify which property
			// of an an object has been modified
			FieldIdentifier fieldIdentifier = args.FieldIdentifier;

			// Make sure we clear out errors from a previous validation
			// only for this Object+Property
			ValidationMessageStore.Clear(fieldIdentifier);

			// FluentValidation specific, we need to tell it to only validate
			// a specific property
			var propertiesToValidate = new string[] { fieldIdentifier.FieldName };
			var fluentValidationContext =
				new ValidationContext(
					instanceToValidate: fieldIdentifier.Model,
					propertyChain: new FluentValidation.Internal.PropertyChain(),
					validatorSelector: new FluentValidation.Internal.MemberNameValidatorSelector(propertiesToValidate)
				);

			// Tell FluentValidation to validate the specified property on the object that was edited
			ValidationResult result = await Validator.ValidateAsync(fluentValidationContext);

			// Now add the results to the ValidationMessageStore we created
			AddValidationResult(fieldIdentifier.Model, result);
		}

		/// <summary>
		/// Adds all of the errors from the Fluent Validator to the ValidationMessageStore
		/// we created when the EditContext changed
		/// </summary>
		void AddValidationResult(object model, ValidationResult validationResult)
		{
			foreach (ValidationFailure error in validationResult.Errors)
			{
				var fieldIdentifier = new FieldIdentifier(model, error.PropertyName);
				ValidationMessageStore.Add(fieldIdentifier, error.ErrorMessage);
			}
			EditContext.NotifyValidationStateChanged();
		}
	}
}
