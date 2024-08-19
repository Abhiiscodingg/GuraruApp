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
    public class RawQualitiesController : Controller
    {
        private readonly GuraruContext _context;

        public RawQualitiesController(GuraruContext context)
        {
            _context = context;
        }

        // GET: RawQualities
        public async Task<IActionResult> Index()
        {
            return View(await _context.RawQualities.ToListAsync());
        }

        // GET: RawQualities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rawQuality = await _context.RawQualities
                .FirstOrDefaultAsync(m => m.QualityId == id);
            if (rawQuality == null)
            {
                return NotFound();
            }

            return View(rawQuality);
        }

        // GET: RawQualities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RawQualities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QualityId,QualityName,QualityCode,IsDeleted")] RawQuality rawQuality)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rawQuality);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rawQuality);
        }

        // GET: RawQualities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rawQuality = await _context.RawQualities.FindAsync(id);
            if (rawQuality == null)
            {
                return NotFound();
            }
            return View(rawQuality);
        }

        // POST: RawQualities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QualityId,QualityName,QualityCode,IsDeleted")] RawQuality rawQuality)
        {
            if (id != rawQuality.QualityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rawQuality);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RawQualityExists(rawQuality.QualityId))
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
            return View(rawQuality);
        }

        // GET: RawQualities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rawQuality = await _context.RawQualities
                .FirstOrDefaultAsync(m => m.QualityId == id);
            if (rawQuality == null)
            {
                return NotFound();
            }

            return View(rawQuality);
        }

        // POST: RawQualities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rawQuality = await _context.RawQualities.FindAsync(id);
            if (rawQuality != null)
            {
                _context.RawQualities.Remove(rawQuality);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RawQualityExists(int id)
        {
            return _context.RawQualities.Any(e => e.QualityId == id);
        }
    }
}
