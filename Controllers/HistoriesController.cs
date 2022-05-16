using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MSOC.Data;
using MSOC.Models;
using MSOC.Models.ViewModels;

namespace MSOC.Controllers
{
    public class HistoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HistoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Histories
        public async Task<IActionResult> Index(String? searchStringCustomer, String? searchStringCatalogNumber)
        {
            

            if (!String.IsNullOrEmpty(searchStringCustomer))
            {
                ViewData["Cust"] = searchStringCustomer;
                if (!String.IsNullOrEmpty(searchStringCatalogNumber))
                {
                    ViewData["Unit"] = searchStringCatalogNumber;
                    var orders = _context.Histories.Include(c => c.Customer).Where(s => s.Customer.Name == searchStringCustomer);
                    var units = orders.Include(c => c.Customer).Include(s => s.Orders).ThenInclude(u => u.Unit).Where(x => x.Orders.Any(u => u.Unit.CatalogNumber == searchStringCatalogNumber));
                    return View(await units.ToListAsync());
                }
                else
                {
                    var orders = _context.Histories.Include(c => c.Customer).Where(s => s.Customer.Name == searchStringCustomer);
                    return View(await orders.ToListAsync());
                }
            }
            else if (!String.IsNullOrEmpty(searchStringCatalogNumber))
            {
                ViewData["Unit"] = searchStringCatalogNumber;
                var units = _context.Histories.Include(c => c.Customer).Include(s => s.Orders).ThenInclude(u => u.Unit).Where(x => x.Orders.Any(u => u.Unit.CatalogNumber == searchStringCatalogNumber));
                return View(await units.ToListAsync());
            }
            else
            {
                ViewData["Customer"] = new SelectList(_context.Customers, "Name");
                var applicationDbContext = _context.Histories.Include(h => h.Customer);
                return View(await applicationDbContext.ToListAsync());
            }

            
        }

        // GET: Histories/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name");
            return View(new HistoriesCreateModel());
        }

        // POST: Histories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HistoriesCreateModel model)
        {
            
            if (ModelState.IsValid)
            {
                var first = _context.Histories.Any() ? _context.Histories.Max(x => x.FirstNumber) + 1 : 1;
                var second = _context.Histories.Any() ? _context.Histories.Max(x => x.SecondNumber) + 1 : 1;

                var history = new History
                {
                    Name = model.Name,
                    CustomerId = model.CustomerId,
                    FirstNumber = first++,
                    SecondNumber = second++
                };


                _context.Add(history);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", model.CustomerId);
            return View(model);
        }

        // GET: Histories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var history = await _context.Histories.FindAsync(id);
            if (history == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", history.CustomerId);
            return View(history);
        }

        // POST: Histories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,FirstNumber,SecondNumber,CustomerId,Date")] History history)
        {
            if (id != history.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(history);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HistoryExists(history.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", history.CustomerId);
            return View(history);
        }

        // GET: Histories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var history = await _context.Histories
                .Include(h => h.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (history == null)
            {
                return NotFound();
            }

            return View(history);
        }

        // POST: Histories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var history = await _context.Histories.FindAsync(id);
            _context.Histories.Remove(history);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HistoryExists(int id)
        {
            return _context.Histories.Any(e => e.Id == id);
        }
    }
}
