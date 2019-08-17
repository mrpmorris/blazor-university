namespace CascadingValuesByType.Models
{
	public class UserPreferences
	{
		public string DateFormat { get; private set; }
		public bool ViewAnonymizedData { get; private set; }

		public UserPreferences(string dateFormat, bool viewAnonymizedData)
		{
			DateFormat = dateFormat;
			ViewAnonymizedData = viewAnonymizedData;
		}
	}
}
