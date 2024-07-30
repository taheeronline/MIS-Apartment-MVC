using ManageApartment.Entities;
using ManageApartment.Repositories.Interface;
using ManageApartment.UI.ViewModels.Flat;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManageFlat.UI.Controllers
{
    public class FlatController : Controller
    {
        private readonly iFlatRepository _flatService;
        private readonly iApartmentRepository _apartmentService;
        private readonly ILogger<FlatController> _logger;

        public FlatController(iFlatRepository flatService, iApartmentRepository apartmentService, ILogger<FlatController> logger)
        {
            _flatService = flatService;
            _apartmentService = apartmentService;
            _logger = logger;

            _logger.LogDebug(1, "NLog injected into ApartmentController");
        }

        // GET: Flat/Index
        public async Task<IActionResult> Index()
        {
            try
            {
                if (HttpContext.Session.GetInt32("UserId") != null)
                {

                    _logger.LogDebug(1, "Index Loading...");
                    var apartments = await _apartmentService.GetAllApartmentsAsync();
                    ViewBag.ApartmentList = new SelectList(apartments, "ID", "Title");

                    var flats = await _flatService.GetAllFlatsAsync();
                    var vm = new List<vmFlat>();
                    foreach (var flat in flats)
                    {
                        vm.Add(new vmFlat { ID = flat.ID, Title = flat.Title, Description = flat.Description, Area = flat.Area, ApartmentTitle = flat.Apartment.Title, IsVacant = flat.IsVacant });
                    }
                    _logger.LogDebug(1, "Index Loaded...");
                    return View(vm);
                }
                return RedirectToAction("Index", "Home");

            }
            catch (Exception ex)
            {
                // Log the error or handle it as needed
                return View("Error",ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> FilterFlats(int apartmentId)
        {
            try
            {
                if (HttpContext.Session.GetInt32("UserId") != null)
                {
                    _logger.LogDebug("Filtering flats...");

                    // Fetch all apartments and set them in ViewBag for the SelectList
                    var apartments = await _apartmentService.GetAllApartmentsAsync();
                    ViewBag.ApartmentList = new SelectList(apartments, "ID", "Title");

                    // Fetch flats based on the selected ApartmentId
                    var flats = await _flatService.GetFlatsByApartmentIdAsync(apartmentId);

                    // Fetch the selected apartment's name
                    var selectedApartment = apartments.FirstOrDefault(a => a.ID == apartmentId);
                    ViewBag.SelectedApartmentName = selectedApartment?.Title;

                    // Map the flats to the view model
                    var vm = flats.Select(flat => new vmFlat
                    {
                        ID = flat.ID,
                        Title = flat.Title,
                        Description = flat.Description,
                        Area = flat.Area,
                        ApartmentTitle = flat.Apartment.Title,
                        IsVacant = flat.IsVacant
                    }).ToList();

                    _logger.LogDebug("Flats filtered and loaded...");
                    return View("Index", vm);
                }

                _logger.LogWarning("User not logged in. Redirecting to Home Index.");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while filtering flats.");
                return View("Error");
            }
        }

        // GET: Flat/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var apartments = await _apartmentService.GetAllApartmentsAsync();
            ViewBag.ApartmentList = new SelectList(apartments, "ID", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(vmAddFlat vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogDebug(1, "Create Initiated..");
                    var flat = new Flat
                    {
                        Title = vm.Title,
                        Description = vm.Description,
                        Area = vm.Area,
                        ApartmentId = vm.ApartmentId
                    };
                    await _flatService.AddFlatAsync(flat);
                    _logger.LogDebug(1, "Create Completed..");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError(string.Empty, "Unable to save changes. Please try again.");
            }
            return View(vm);
        }

        // GET: Flat/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var flat = await _flatService.GetFlatByIdAsync(id);
                var vm = new vmUpdateFlat
                {
                    Title = flat.Title,
                    Description = flat.Description,
                    Area = flat.Area,
                    ApartmentId = flat.ApartmentId,
                    IsVacant = flat.IsVacant
                };

                var apartments = await _apartmentService.GetAllApartmentsAsync();
                ViewBag.ApartmentList = new SelectList(apartments, "ID", "Title");
                return View(vm);
            }
            catch (Exception ex)
            {
                // Log the error or handle it as needed
                return View("Error", ex);
            }
        }

        // PUT: Flat/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, vmUpdateFlat model)
        {
            if (id != model.ID)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogDebug(1, "Edit Initiated..");
                    var flat = new Flat
                    {
                        ID = model.ID,
                        Title = model.Title,
                        Description = model.Description,
                        Area = model.Area,
                        IsVacant = model.IsVacant,
                        ApartmentId = model.ApartmentId
                    };
                    await _flatService.UpdateFlatAsync(flat);
                    _logger.LogDebug(1, "Edit Completed..");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                // Log the error or handle it as needed
                ModelState.AddModelError(string.Empty, "Unable to update changes. Please try again.");
            }
            return View(model);
        }

        // POST: Flat/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _flatService.DeleteFlatAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                // Log the error or handle it as needed
                return View("Error");
            }
        }
    }
}
