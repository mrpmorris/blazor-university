using AccessingEditContext.Pages;
using System.ComponentModel.DataAnnotations;

namespace AccessingEditContext
{
	public class Contact
	{
		[Required]
		public string EmailAddress { get; set; }
	}
}
