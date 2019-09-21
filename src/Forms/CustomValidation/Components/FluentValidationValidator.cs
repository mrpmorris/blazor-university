﻿using Microsoft.AspNetCore.Components;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;
using System;
using System.Linq;
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

		// Keep a reference to our previous edit context,
		// so when know if SetParametersAsync changes it
		private EditContext PreviousEditContext;

		// Holds a list of IValidator instances to perform our actual validation
		private IEnumerable<IValidator> Validators;

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
		public async override Task SetParametersAsync(ParameterView parameters)
		{
			await base.SetParametersAsync(parameters);

			// If the EditForm.Model changes then we get a new EditContext
			// and need to hook it up
			if (EditContext != PreviousEditContext)
				EditContextChanged();
		}

		/// <summary>
		/// We trigger this when SetParametersAsync is executed and results in us having a
		/// new EditContext.
		/// </summary>
		void EditContextChanged()
		{
			PreviousEditContext = EditContext;

			// We need this to store our validation errors
			// Whenever we get a new EditContext (because EditForm.Model has changed)
			// we also need to discard our old message store and create a new one
			ValidationMessageStore = new ValidationMessageStore(EditContext);

			// Get all Fluent IValidator instances for this EditForm.Model object type
			CreateValidators();

			// If there are any validators, then observe any changes to the EditForm.Model
			// object
			if (Validators.Any())
				HookUpEditContextEvents();
		}

		/// <summary>
		/// Gets a list of validator types for the EditForm.Model object type and
		/// uses dependency injection to create instances of them. This implementation is
		/// specific to our FluenValidation implementation, it's not Blazor specific.
		/// </summary>
		private void CreateValidators()
		{
			// Get a list of discovered validator types for the EditForm.Model type
			IEnumerable<Type> validatorTypes = ValidatorRepository.GetValidators(EditContext.Model.GetType());

			// Create instances of those validators
			Validators = validatorTypes
				.Select(x => (IValidator)ServiceProvider.GetService(x));
		}

		private void HookUpEditContextEvents()
		{
			// We need to know when to validate the whole object, this
			// is triggered when the EditForm is submitted
			EditContext.OnValidationRequested += ValidationRequested;

			// We need to know when to validate an individual property, this
			// is triggered when the user edits something
			EditContext.OnFieldChanged += FieldChanged;
		}

		async void ValidationRequested(object sender, ValidationRequestedEventArgs args)
		{
			// Clear all errors from a previous validation
			ValidationMessageStore.Clear();

			// Loop through all registered Fluent validators - this is usually zero or one
			foreach (IValidator validator in Validators)
			{
				// Tell FluentValidation to validate the object
				ValidationResult result = await validator.ValidateAsync(EditContext.Model);

				// Now add the results to the ValidationMessageStore we created
				AddValidationResult(EditContext.Model, result);
			}
		}

		async void FieldChanged(object sender, FieldChangedEventArgs args)
		{
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

			// Loop through all registered Fluent validators - this is usually zero or one
			foreach (IValidator validator in Validators)
			{
				// Tell FluentValidation to validate the specified property on the object that was edited
				ValidationResult result = await validator.ValidateAsync(fluentValidationContext);

				// Now add the results to the ValidationMessageStore we created
				AddValidationResult(fieldIdentifier.Model, result);
			}
		}

		/// <summary>
		/// Adds all of the errors from the Fluent Validator to the ValidationMessageStore
		/// we created when the EditContext changed
		/// </summary>
		void AddValidationResult(object model, ValidationResult validationResult)
		{
			foreach (ValidationFailure error in validationResult.Errors)
			{
				object instance = error.CustomState ?? model;
				var fieldIdentifier = new FieldIdentifier(instance, error.PropertyName);
				ValidationMessageStore.Add(fieldIdentifier, error.ErrorMessage);
			}
		}

	}
}
