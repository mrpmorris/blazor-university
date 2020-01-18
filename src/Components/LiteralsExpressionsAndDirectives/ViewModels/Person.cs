namespace LiteralsExpressionsAndDirectives.ViewModels
{
	public class Person
	{
		public string Salutation { get; set; }
		public string GivenName { get; set; }
		public string FamilyName { get; set; }

		public override string ToString() => $"{Salutation} {GivenName} {FamilyName}";
	}
}
