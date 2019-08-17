using System;
using System.Collections.Generic;

namespace CascadingValuesByType.Models
{
	public class Vacancy
	{
		public string Title { get; set; }
		public DateTime ClosingDate { get; set; }
		public Address Address { get; set; }
		public List<Application> Applications { get; set; } = new List<Application>();
	}
}
