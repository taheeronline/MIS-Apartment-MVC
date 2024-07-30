using ManageApartment.Repositories.Interface;
using ManageApartment.UI.Models;
using ManageApartment.UI.ViewModels.Apartment;
using ManageApartment.UI.ViewModels.Flat;
using ManageApartment.UI.ViewModels.Home;
using ManageApartment.UI.ViewModels.Resident;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ManageApartment.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly iApartmentRepository _apartmentService;
        private readonly iFlatRepository _flatService;
        private readonly iResidentRepository _residentService;

        public HomeController(ILogger<HomeController> logger, iApartmentRepository apartmentService, iFlatRepository flatService, iResidentRepository residentService)
        {
            _apartmentService = apartmentService;
            _logger = logger;
            _flatService = flatService;
            _residentService = residentService;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var apartments = await _apartmentService.GetAllApartmentsAsync(); // Fetch apartments from your data source
                var flats = await _flatService.GetAllFlatsAsync(); // Fetch flats from your data source
                var residents = await _residentService.GetAllResidentsAsync(); // Fetch residents from your data source

                var vmApartments = new List<vmApartment>();
                foreach (var apartment in apartments)
                {
                    var numberOfFlats = await _apartmentService.GetNumberOfFlatsAsync(apartment.ID);
                    vmApartments.Add(new vmApartment
                    {
                        ID = apartment.ID,
                        Title = apartment.Title,
                        Address = apartment.Address,
                        GpsLocation = apartment.GpsLocation,
                        ResidentGroupEmail = apartment.ResidentGroupEmail,
                        AssociationGroupEmail = apartment.AssociationGroupEmail,
                        Phone1 = apartment.Phone1,
                        Phone2 = apartment.Phone2,
                        NumberOfFlats = numberOfFlats
                    }
                    );
                }

                var vmFlats = new List<vmFlat>();
                foreach (var flat in flats)
                {
                    vmFlats.Add(new vmFlat { ID = flat.ID, Title = flat.Title, Description = flat.Description, Area = flat.Area, ApartmentTitle = flat.Apartment.Title, IsVacant = flat.IsVacant });
                }

                var vmResidents = new List<vmResident>();
                foreach (var resident in residents)
                {
                    vmResidents.Add(new vmResident
                    {
                        ID = resident.ID,
                        FullName = resident.FullName,
                        CurrentAddress = resident.CurrentAddress,
                        Email = resident.Email,
                        Phone1 = resident.Phone1,
                        IsOwner = resident.IsOwner,
                        MaintenancePayee = resident.MaintenancePayee,
                        FlatTitle = resident.Flat.Title,
                        EntryDate = resident.EntryDate
                    });
                }

                var model = new vmHome
                {
                    vmApartments = vmApartments,
                    vmFlats = vmFlats,
                    vmResidents = vmResidents
                };

                return View(model);
            }
            return RedirectToAction("Login", "User");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
