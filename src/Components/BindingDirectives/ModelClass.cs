using System;

namespace BindingDirectives
{
	public class ModelClass
	{
		public string Name { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public decimal? BankBalance { get; set; } = 42.42m;
	}
}
