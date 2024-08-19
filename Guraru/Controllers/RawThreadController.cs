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
    public class RawThreadController : Controller
    {
        private readonly GuraruContext _context;

        public RawThreadController(GuraruContext context)
        {
            _context = context;
        }

        // GET: RawThread
        public async Task<IActionResult> Index()
        {
            var guraruContext = _context.RawThreads.Include(r => r.QualityNavigation);
            return View(await guraruContext.ToListAsync());
        }

        // GET: RawThread/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rawThread = await _context.RawThreads
                .Include(r => r.QualityNavigation)
                .FirstOrDefaultAsync(m => m.ThreadId == id);
            if (rawThread == null)
            {
                return NotFound();
            }

            return View(rawThread);
        }

        // GET: RawThread/Create
        public IActionResult Create()
        {
            ViewData["Quality"] = new SelectList(_context.RawQualities, "QualityId", "QualityName");
            return View();
        }

        // POST: RawThread/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ThreadId,ThWeight,BillAmount,BillNo,BillDate,CreatedBy,CreatedDate,CompanyName,Quality,IsDeleted")] RawThread rawThread)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rawThread);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Quality"] = new SelectList(_context.RawQualities, "QualityId", "QualityId", rawThread.Quality);
            return View(rawThread);
        }

        // GET: RawThread/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rawThread = await _context.RawThreads.FindAsync(id);
            if (rawThread == null)
            {
                return NotFound();
            }
            ViewData["Quality"] = new SelectList(_context.RawQualities, "QualityId", "QualityId", rawThread.Quality);
            return View(rawThread);
        }

        // POST: RawThread/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ThreadId,ThWeight,BillAmount,BillNo,BillDate,CreatedBy,CreatedDate,CompanyName,Quality,IsDeleted")] RawThread rawThread)
        {
            if (id != rawThread.ThreadId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rawThread);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RawThreadExists(rawThread.ThreadId))
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
            ViewData["Quality"] = new SelectList(_context.RawQualities, "QualityId", "QualityId", rawThread.Quality);
            return View(rawThread);
        }

        // GET: RawThread/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rawThread = await _context.RawThreads
                .Include(r => r.QualityNavigation)
                .FirstOrDefaultAsync(m => m.ThreadId == id);
            if (rawThread == null)
            {
                return NotFound();
            }

            return View(rawThread);
        }

        // POST: RawThread/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rawThread = await _context.RawThreads.FindAsync(id);
            if (rawThread != null)
            {
                _context.RawThreads.Remove(rawThread);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RawThreadExists(int id)
        {
            return _context.RawThreads.Any(e => e.ThreadId == id);
        }
    }
}
