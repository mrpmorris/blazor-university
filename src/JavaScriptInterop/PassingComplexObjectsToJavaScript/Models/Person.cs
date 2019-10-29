using System.Collections.Generic;

namespace PassingComplexObjectsToJavaScript.Models
{
	public class Person
	{
		public string Salutation { get; set; }
		public string GivenName { get; set; }
		public string FamilyName { get; set; }
		public List<KeyValuePair<string, string>> PhoneNumbers { get; set; }

		public Person()
		{
			PhoneNumbers = new List<KeyValuePair<string, string>>();
		}
	}
}
