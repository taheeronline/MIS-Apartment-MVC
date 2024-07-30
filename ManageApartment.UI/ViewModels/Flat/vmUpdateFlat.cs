namespace ManageApartment.UI.ViewModels.Flat
{
    public class vmUpdateFlat
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Area { get; set; }
        public bool IsVacant { get; set; }

        public int ApartmentId { get; set; }
    }
}
