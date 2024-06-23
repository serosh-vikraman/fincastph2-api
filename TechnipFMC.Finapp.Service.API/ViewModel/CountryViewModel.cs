namespace TechnipFMC.Finapp.Service.API.ViewModel
{
    public class CountryViewModel
    {
        public int CountryID { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public string Status { get; set; }
    }
}