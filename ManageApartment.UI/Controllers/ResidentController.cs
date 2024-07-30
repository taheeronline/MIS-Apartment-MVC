using ManageApartment.Entities;
using ManageApartment.Repositories.Interface;
using ManageApartment.UI.ViewModels.Resident;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManageApartment.UI.Controllers
{
    public class ResidentController : Controller
    {
        private readonly iResidentRepository _residentService;
        private readonly iFlatRepository _flatService;
        private readonly ILogger<ResidentController> _logger;

        public ResidentController(iFlatRepository flatService, iResidentRepository residentService, ILogger<ResidentController> logger)
        {
            _flatService = flatService;
            _residentService = residentService;
            _logger = logger;
        }

        // GET: Flat/Index
        public async Task<IActionResult> Index()
        {
            try
            {
                if (HttpContext.Session.GetInt32("UserId") != null)
                {
                    _logger.LogDebug(1, "Index Loading...");
                    var residents = await _residentService.GetAllResidentsAsync();
                    var vm = new List<vmResident>();
                    foreach (var resident in residents)
                    {
                        vm.Add(new vmResident
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
                    _logger.LogDebug(1, "Index Loaded...");
                    return View(vm);
                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                // Log the error or handle it as needed
                return View("Error");
            }
        }

        // GET: Flat/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var flats = await _flatService.GetAllFlatsAsync();
            ViewBag.FlatList = new SelectList(flats, "ID", "Title");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(vmAddResident vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogDebug(1, "Create Initiated..");
                    var resident = new Resident
                    {
                        FullName = vm.FullName,
                        CurrentAddress = vm.CurrentAddress,
                        PermanentAddress = vm.PermanentAddress,
                        Email = vm.Email,
                        Phone1 = vm.Phone1,
                        Phone2 = vm.Phone2,
                        IsOwner = vm.IsOwner,
                        MaintenancePayee = vm.MaintenancePayee,
                        EntryDate = vm.EntryDate,
                        FlatId = vm.FlatId
                    };
                    await _residentService.AddResidentAsync(resident);
                    _logger.LogDebug(1, "Create Completed..");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                // Log the error or handle it as needed
                ModelState.AddModelError(string.Empty, "Unable to save changes. Please try again.");
            }
            return View(vm);
        }

        // GET: Flat/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var resident = await _residentService.GetResidentByIdAsync(id);
                var vm = new vmUpdateResident
                {
                    ID = resident.ID,
                    FullName = resident.FullName,
                    CurrentAddress = resident.CurrentAddress,
                    PermanentAddress = resident.PermanentAddress,
                    Email = resident.Email,
                    Phone1 = resident.Phone1,
                    Phone2 = resident.Phone2,
                    IsOwner = resident.IsOwner,
                    MaintenancePayee = resident.MaintenancePayee,
                    FlatId = resident.FlatId,
                    EntryDate = resident.EntryDate,
                    ExitDate = resident.EntryDate
                };

                var flats = await _flatService.GetAllFlatsAsync();
                ViewBag.FlatList = new SelectList(flats, "ID", "Title");
                return View(vm);
            }
            catch (Exception ex)
            {
                // Log the error or handle it as needed
                return View("Error");
            }
        }

        // PUT: Flat/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, vmUpdateResident model)
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
                    var resident = new Resident
                    {
                        ID = model.ID,
                        FullName = model.FullName,
                        CurrentAddress = model.CurrentAddress,
                        PermanentAddress = model.PermanentAddress,
                        Email = model.Email,
                        Phone1 = model.Phone1,
                        Phone2 = model.Phone2,
                        IsOwner = model.IsOwner,
                        MaintenancePayee = model.MaintenancePayee,
                        FlatId = model.FlatId,
                        EntryDate = model.EntryDate,
                        ExitDate = model.ExitDate
                    };
                    await _residentService.UpdateResidentAsync(resident);
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
                await _residentService.DeleteResidentAsync(id);
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

