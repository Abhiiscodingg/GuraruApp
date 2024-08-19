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
    public class MachineOperatorController : Controller
    {
        private readonly GuraruContext _context;

        public MachineOperatorController(GuraruContext context)
        {
            _context = context;
        }

        // GET: MachineOperator
        public async Task<IActionResult> Index()
        {
            return View(await _context.MachineOperators.ToListAsync());
        }

        // GET: MachineOperator/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var machineOperator = await _context.MachineOperators
                .FirstOrDefaultAsync(m => m.OperatorId == id);
            if (machineOperator == null)
            {
                return NotFound();
            }

            return View(machineOperator);
        }

        // GET: MachineOperator/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MachineOperator/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OperatorId,OperatorName,IsDeleted,AdhaarNo,LocationDetails")] MachineOperator machineOperator)
        {
            if (ModelState.IsValid)
            {
                _context.Add(machineOperator);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(machineOperator);
        }

        // GET: MachineOperator/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var machineOperator = await _context.MachineOperators.FindAsync(id);
            if (machineOperator == null)
            {
                return NotFound();
            }
            return View(machineOperator);
        }

        // POST: MachineOperator/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OperatorId,OperatorName,IsDeleted,AdhaarNo,LocationDetails")] MachineOperator machineOperator)
        {
            if (id != machineOperator.OperatorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(machineOperator);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MachineOperatorExists(machineOperator.OperatorId))
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
            return View(machineOperator);
        }

        // GET: MachineOperator/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var machineOperator = await _context.MachineOperators
                .FirstOrDefaultAsync(m => m.OperatorId == id);
            if (machineOperator == null)
            {
                return NotFound();
            }

            return View(machineOperator);
        }

        // POST: MachineOperator/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var machineOperator = await _context.MachineOperators.FindAsync(id);
            if (machineOperator != null)
            {
                _context.MachineOperators.Remove(machineOperator);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MachineOperatorExists(int id)
        {
            return _context.MachineOperators.Any(e => e.OperatorId == id);
        }
    }
}
