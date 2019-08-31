using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace HandlingFormSubmission.Components
{
	public class PeteValidator : ComponentBase
	{
		[CascadingParameter]
		public EditContext EditContext { get; set; }

		private EditContext PreviousEditContext;

		public override async Task SetParametersAsync(ParameterView parameters)
		{
			await base.SetParametersAsync(parameters);
			//if (EditContext == PreviousEditContext)
				//return;

			var validationMessageStore = new ValidationMessageStore(EditContext);
			PreviousEditContext = EditContext;
			EditContext.OnFieldChanged += (sender, args) =>
			{
				System.Diagnostics.Debug.WriteLine("PeteValidator: " + args.FieldIdentifier.FieldName);
			};

			EditContext.OnValidationRequested += (sender, args) =>
			{
				System.Diagnostics.Debug.WriteLine("PeteValidator object");
			};
		}
	}
}
