namespace ManageApartment.UI.ViewModels.Resident
{
    public class vmResident
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string CurrentAddress { get; set; }
        public string Email { get; set; }
        public string Phone1 { get; set; }

        public DateTime EntryDate { get; set; }

        public bool IsOwner { get; set; }
        public bool MaintenancePayee { get; set; }

        public string FlatTitle { get; set; }
    }
}
