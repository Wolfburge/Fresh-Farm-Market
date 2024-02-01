namespace Fresh_Farm_Market.ViewModels
{
	public class Audit
	{
		public int Id { get; set; }
		public string UserID { get; set; }
		public string Action {  get; set; }
		public DateTime DateTime { get; set; }
	}
}
