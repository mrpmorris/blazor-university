using System.ComponentModel.DataAnnotations;

namespace ValidatingUserInput.Models
{
	public class Person
	{
		[Required]
		public string Name { get; set; }
		[Range(18, 80, ErrorMessage = "Age must be between 18 and 80.")]
		public int Age { get; set; }
	}
}
