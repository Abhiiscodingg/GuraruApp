using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GuraruRepository.Models;

namespace Guraru.Controllers
{
    public class GuraruExitsController : Controller
    {
        private readonly GuraruContext _context;

        public GuraruExitsController(GuraruContext context)
        {
            _context = context;
        }

        // GET: GuraruExits
        public async Task<IActionResult> Index()
        {
            var guraruContext = _context.GuraruExits.Include(g => g.QualityNavigation);
            return View(await guraruContext.ToListAsync());
        }

        // GET: GuraruExits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guraruExit = await _context.GuraruExits
                .Include(g => g.QualityNavigation)
                .FirstOrDefaultAsync(m => m.ExitId == id);
            if (guraruExit == null)
            {
                return NotFound();
            }

            return View(guraruExit);
        }

        // GET: GuraruExits/Create
        public IActionResult Create()
        {
            ViewData["Quality"] = new SelectList(_context.RawQualities, "QualityId", "QualityName");
            return View();
        }

        // POST: GuraruExits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExitId,ExitWeight,Quality,BillNumber,BillAmount,DriverName,VehicleNumber,DestinationCity,ChallanPhotoPath,Quality,CreatedDate,CreatedBy,SubmittedDate,SubmittedBy")] GuraruExit guraruExit)
        {
            if (ModelState.IsValid)
            {
                guraruExit.CreatedBy = guraruExit.SubmittedBy;
                guraruExit.CreatedDate = DateTime.Now;
                _context.Add(guraruExit);
                await _context.SaveChangesAsync();
                UpdateStage2UsedWeight(guraruExit.ExitWeight, guraruExit.Quality);
                return RedirectToAction(nameof(Index));
            }
            ViewData["Quality"] = new SelectList(_context.RawQualities, "QualityId", "QualityId", guraruExit.Quality);
            return View(guraruExit);
        }

        private void UpdateStage2UsedWeight(int? exitWeight, int? quality)
        {
            var stage2List = _context.ManufacturingStage2s.Where(m => m.Quality == quality && m.CompletedWeight != null && m.CompletedWeight!=0 && m.CompletedWeight != m.UsedWeight).ToList();
            int? tempExitWeight = exitWeight;
            foreach (var eachStage2 in stage2List)
            {
                if (tempExitWeight != 0)
                {
                    int? wtToUpdate = eachStage2.CompletedWeight - (eachStage2.UsedWeight ?? 0);
                    if (tempExitWeight >= wtToUpdate)
                    {
                        eachStage2.UsedWeight = (eachStage2.UsedWeight ?? 0) + wtToUpdate;
                        tempExitWeight = tempExitWeight - wtToUpdate;
                    }
                    else
                    {
                        eachStage2.UsedWeight = (eachStage2.UsedWeight ?? 0) + tempExitWeight;
                        tempExitWeight = 0;
                    }
                    _context.ManufacturingStage2s.Where(a => a.Id == eachStage2.Id).ExecuteUpdate(m => m.SetProperty(u => u.UsedWeight, eachStage2.UsedWeight));
                }
            }
        }

        // GET: GuraruExits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guraruExit = await _context.GuraruExits.FindAsync(id);
            if (guraruExit == null)
            {
                return NotFound();
            }
            ViewData["Quality"] = new SelectList(_context.RawQualities, "QualityId", "QualityId", guraruExit.Quality);
            return View(guraruExit);
        }

        // POST: GuraruExits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExitId,ExitWeight,BillNumber,BillAmount,DriverName,VehicleNumber,DestinationCity,ChallanPhotoPath,Quality,CreatedDate,CreatedBy,SubmittedDate,SubmittedBy")] GuraruExit guraruExit)
        {
            if (id != guraruExit.ExitId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(guraruExit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GuraruExitExists(guraruExit.ExitId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Quality"] = new SelectList(_context.RawQualities, "QualityId", "QualityId", guraruExit.Quality);
            return View(guraruExit);
        }

        // GET: GuraruExits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guraruExit = await _context.GuraruExits
                .Include(g => g.QualityNavigation)
                .FirstOrDefaultAsync(m => m.ExitId == id);
            if (guraruExit == null)
            {
                return NotFound();
            }

            return View(guraruExit);
        }

        // POST: GuraruExits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var guraruExit = await _context.GuraruExits.FindAsync(id);
            if (guraruExit != null)
            {
                _context.GuraruExits.Remove(guraruExit);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GuraruExitExists(int id)
        {
            return _context.GuraruExits.Any(e => e.ExitId == id);
        }
    }
}
