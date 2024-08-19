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
    public class ManufacturingStage2Controller : Controller
    {
        private readonly GuraruContext _context;

        public ManufacturingStage2Controller(GuraruContext context)
        {
            _context = context;
        }

        // GET: ManufacturingStage2
        public async Task<IActionResult> Index()
        {
            var guraruContext = _context.ManufacturingStage2s.Include(m => m.QualityNavigation);
            return View(await guraruContext.ToListAsync());
        }

        // GET: ManufacturingStage2/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manufacturingStage2 = await _context.ManufacturingStage2s
                .Include(m => m.QualityNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (manufacturingStage2 == null)
            {
                return NotFound();
            }

            return View(manufacturingStage2);
        }

        // GET: ManufacturingStage2/Create
        public IActionResult Create()
        {
            ViewData["Quality"] = new SelectList(_context.RawQualities, "QualityId", "QualityId");
            return View();
        }

        // POST: ManufacturingStage2/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SubmittedDate,SubmittedWeight,Quality,CompletedWeight,CompletedDate,SubmittedBy,CreatedBy,CreatedDate")] ManufacturingStage2 manufacturingStage2)
        {
            if (ModelState.IsValid)
            {
                manufacturingStage2.CreatedDate = DateTime.Now;
                manufacturingStage2.CreatedBy = manufacturingStage2.SubmittedBy;
                _context.Add(manufacturingStage2);
                await _context.SaveChangesAsync();
                UpdateStage1UsedWeight(manufacturingStage2.SubmittedWeight, manufacturingStage2.Quality);
                return RedirectToAction(nameof(Index));
            }
            ViewData["Quality"] = new SelectList(_context.RawQualities, "QualityId", "QualityId", manufacturingStage2.Quality);
            return View(manufacturingStage2);
        }

        private void UpdateStage1UsedWeight(int? stage2SubmittedWeight, int? quality)
        {
            var stage1List = _context.ManufacturingStage1s.Where(m => m.Quality == quality && m.CompletedWeight!=null && m.CompletedWeight!=0 && m.CompletedWeight != m.UsedWeight).ToList();
            int? tempSubmittedWt = stage2SubmittedWeight;
            foreach (var stage1 in stage1List)
            {
                if (tempSubmittedWt != 0)
                {
                    int? wtToUpdate = stage1.CompletedWeight - (stage1.UsedWeight ?? 0);
                    if (tempSubmittedWt >= wtToUpdate)
                    {
                        stage1.UsedWeight = (stage1.UsedWeight ?? 0) + wtToUpdate;
                        tempSubmittedWt = tempSubmittedWt - wtToUpdate;
                    }
                    else
                    {
                        stage1.UsedWeight = (stage1.UsedWeight ?? 0) + tempSubmittedWt;
                        tempSubmittedWt = 0;
                    }
                    _context.ManufacturingStage1s.Where(a => a.Id == stage1.Id).ExecuteUpdate(m => m.SetProperty(u => u.UsedWeight, stage1.UsedWeight));
                }
            }
        }

        // GET: ManufacturingStage2/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manufacturingStage2 = await _context.ManufacturingStage2s.FindAsync(id);
            if (manufacturingStage2 == null)
            {
                return NotFound();
            }
            ViewData["Quality"] = new SelectList(_context.RawQualities, "QualityId", "QualityId", manufacturingStage2.Quality);
            return View(manufacturingStage2);
        }

        // POST: ManufacturingStage2/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SubmittedDate,SubmittedWeight,Quality,CompletedWeight,CompletedDate,SubmittedBy,CreatedBy,CreatedDate")] ManufacturingStage2 manufacturingStage2)
        {
            if (id != manufacturingStage2.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(manufacturingStage2);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManufacturingStage2Exists(manufacturingStage2.Id))
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
            ViewData["Quality"] = new SelectList(_context.RawQualities, "QualityId", "QualityId", manufacturingStage2.Quality);
            return View(manufacturingStage2);
        }

        // GET: ManufacturingStage2/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manufacturingStage2 = await _context.ManufacturingStage2s
                .Include(m => m.QualityNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (manufacturingStage2 == null)
            {
                return NotFound();
            }

            return View(manufacturingStage2);
        }

        // POST: ManufacturingStage2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var manufacturingStage2 = await _context.ManufacturingStage2s.FindAsync(id);
            if (manufacturingStage2 != null)
            {
                _context.ManufacturingStage2s.Remove(manufacturingStage2);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ManufacturingStage2Exists(int id)
        {
            return _context.ManufacturingStage2s.Any(e => e.Id == id);
        }
    }
}
