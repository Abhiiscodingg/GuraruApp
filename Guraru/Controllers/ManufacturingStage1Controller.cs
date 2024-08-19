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
    public class ManufacturingStage1Controller : Controller
    {
        private readonly GuraruContext _context;

        public ManufacturingStage1Controller(GuraruContext context)
        {
            _context = context;
        }

        // GET: ManufacturingStage1
        public async Task<IActionResult> Index()
        {
            var guraruContext = _context.ManufacturingStage1s.Include(m => m.MachineNavigation).Include(m => m.MachineOperatorNavigation).Include(m => m.QualityNavigation);
            return View(await guraruContext.ToListAsync());
        }

        // GET: ManufacturingStage1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manufacturingStage1 = await _context.ManufacturingStage1s
                .Include(m => m.MachineNavigation)
                .Include(m => m.MachineOperatorNavigation)
                .Include(m => m.QualityNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (manufacturingStage1 == null)
            {
                return NotFound();
            }

            return View(manufacturingStage1);
        }

        // GET: ManufacturingStage1/Create
        public IActionResult Create()
        {
            ViewData["Machine"] = new SelectList(_context.Machines, "MachineId", "MachineName");
            ViewData["MachineOperator"] = new SelectList(_context.MachineOperators, "OperatorId", "OperatorName");
            ViewData["Quality"] = new SelectList(_context.RawQualities, "QualityId", "QualityName");
            //ViewData["RawThStock"] = TempData["RawThStock"];
            //ViewData["SelectedQuality"] = TempData["RawThStock"];
            return View();
        }

        // POST: ManufacturingStage1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Machine,MachineOperator,Quality,SubmittedWeght,CompletedWeight,SubmittedDate,SubmittedBy,CreatedBy,CreatedDate,CompletedDate")] ManufacturingStage1 manufacturingStage1)
        {
            if (ModelState.IsValid)
            {
                manufacturingStage1.CreatedDate = DateTime.Now;
                manufacturingStage1.CreatedBy = manufacturingStage1.SubmittedBy;
                //Updating Used Weight in ThreadRaw Table - Start
                UpdateThreadUsedWeight(manufacturingStage1.SubmittedWeght,manufacturingStage1.Quality);
                //End - Updating Used Weight in ThreadRaw Table
                _context.Add(manufacturingStage1);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Machine"] = new SelectList(_context.Machines, "MachineId", "MachineId", manufacturingStage1.Machine);
            ViewData["MachineOperator"] = new SelectList(_context.MachineOperators, "OperatorId", "OperatorId", manufacturingStage1.MachineOperator);
            ViewData["Quality"] = new SelectList(_context.RawQualities, "QualityId", "QualityId", manufacturingStage1.Quality);
            return View(manufacturingStage1);
        }

        public void GetThreadStocksForQuality(RawQuality rawQuality)
        {
            int? threadStock = _context.RawThreads.Where(m => m.ThWeight != m.UsedWeight && m.Quality.Equals(rawQuality.QualityId)).Sum(m => (m.ThWeight) - (m.UsedWeight ?? 0));
            TempData["RawThStock"] = threadStock;
            TempData["RawThStock"] = rawQuality.QualityId;
            RedirectToAction(nameof(Create));
        }
        private void UpdateThreadUsedWeight(int? stage1SubmittedWeight,int? quality)
        {
            var threadList = _context.RawThreads.Where(m => m.Quality==quality && m.ThWeight != m.UsedWeight).ToList();
            int? tempSubmittedWt = stage1SubmittedWeight;
            foreach (var eachThread in threadList)
            {
                if (tempSubmittedWt != 0)
                {
                    int? wtToUpdate = eachThread.ThWeight - (eachThread.UsedWeight??0);
                    if (tempSubmittedWt >= wtToUpdate)
                    {
                        eachThread.UsedWeight = (eachThread.UsedWeight??0) + wtToUpdate;
                        tempSubmittedWt = tempSubmittedWt - wtToUpdate;
                    }
                    else
                    {
                        eachThread.UsedWeight = (eachThread.UsedWeight??0) + tempSubmittedWt;
                        tempSubmittedWt = 0;
                    }
                    _context.RawThreads.Where(a => a.ThreadId == eachThread.ThreadId).ExecuteUpdate(m => m.SetProperty(u => u.UsedWeight, eachThread.UsedWeight));
                }
            }
        }


        // GET: ManufacturingStage1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manufacturingStage1 = await _context.ManufacturingStage1s.FindAsync(id);
            if (manufacturingStage1 == null)
            {
                return NotFound();
            }
            ViewData["Machine"] = new SelectList(_context.Machines, "MachineId", "MachineId", manufacturingStage1.Machine);
            ViewData["MachineOperator"] = new SelectList(_context.MachineOperators, "OperatorId", "OperatorId", manufacturingStage1.MachineOperator);
            ViewData["Quality"] = new SelectList(_context.RawQualities, "QualityId", "QualityId", manufacturingStage1.Quality);
            return View(manufacturingStage1);
        }

        // POST: ManufacturingStage1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Machine,MachineOperator,Quality,SubmittedWeght,CompletedWeight,SubmittedDate,SubmittedBy,CreatedBy,CreatedDate,CompletedDate")] ManufacturingStage1 manufacturingStage1)
        {
            if (id != manufacturingStage1.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(manufacturingStage1);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManufacturingStage1Exists(manufacturingStage1.Id))
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
            ViewData["Machine"] = new SelectList(_context.Machines, "MachineId", "MachineId", manufacturingStage1.Machine);
            ViewData["MachineOperator"] = new SelectList(_context.MachineOperators, "OperatorId", "OperatorId", manufacturingStage1.MachineOperator);
            ViewData["Quality"] = new SelectList(_context.RawQualities, "QualityId", "QualityId", manufacturingStage1.Quality);
            return View(manufacturingStage1);
        }

        // GET: ManufacturingStage1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manufacturingStage1 = await _context.ManufacturingStage1s
                .Include(m => m.MachineNavigation)
                .Include(m => m.MachineOperatorNavigation)
                .Include(m => m.QualityNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (manufacturingStage1 == null)
            {
                return NotFound();
            }

            return View(manufacturingStage1);
        }

        // POST: ManufacturingStage1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var manufacturingStage1 = await _context.ManufacturingStage1s.FindAsync(id);
            if (manufacturingStage1 != null)
            {
                _context.ManufacturingStage1s.Remove(manufacturingStage1);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ManufacturingStage1Exists(int id)
        {
            return _context.ManufacturingStage1s.Any(e => e.Id == id);
        }
    }
}
