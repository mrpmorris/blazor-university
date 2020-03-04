using System.ComponentModel.DataAnnotations;

namespace AccessingFormState
{
	public class Contact
	{
		[Required]
		public string EmailAddress { get; set; }
	}
}
