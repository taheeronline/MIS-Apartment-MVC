namespace ManageApartment.UI.ViewModels.Resident
{
    public class vmUpdateResident
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string PermanentAddress { get; set; }
        public string CurrentAddress { get; set; }
        public string Email { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime ExitDate { get; set; }
        public bool IsOwner { get; set; }
        public bool MaintenancePayee { get; set; }
        public int  FlatId { get; set; }
    }
}
