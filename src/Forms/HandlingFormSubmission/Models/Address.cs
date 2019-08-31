using System.ComponentModel.DataAnnotations;

namespace HandlingFormSubmission.Models
{
	public class Address
	{
		[Required]
		public string Line1 { get; set; }
		public string Line2 { get; set; }
		public string City { get; set; }
		[Required]
		public string PostalCode { get; set; }
	}
}
