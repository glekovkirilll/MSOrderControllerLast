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
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index(Int32? historyId)
        {
            if (historyId == null)
            {
                return this.NotFound();
            }

            var history = await _context.Histories
                .SingleOrDefaultAsync(x => x.Id == historyId);

            if (history == null)
            {
                return this.NotFound();
            }

            this.ViewBag.History = history;

            var applicationDbContext = await _context.Orders.
                Include(o => o.History)
                .Include(o => o.Unit)
                .Include(w => w.History.Customer)
                .Where(x => x.HistoryId == historyId)
                .OrderBy(s => s.Index)
                .ToListAsync();

            return View(applicationDbContext);
        }

        // GET: Orders/Create
        public async Task<IActionResult> Create(Int32? historyId)
        {
            if (historyId == null)
            {
                return this.NotFound();
            }

            var history = await _context.Histories
                .SingleOrDefaultAsync(x => x.Id == historyId);

            if (history == null)
            {
                return this.NotFound();
            }

            this.ViewBag.History = history;

            ViewData["HistoryId"] = new SelectList(_context.Histories, "Id", "Id");
            ViewData["UnitId"] = new SelectList(_context.Units, "Id", "Name");
            return View(new OrdersCreateModel());
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? historyId, OrdersCreateModel model)
        {
            if (historyId == null)
            {
                return this.NotFound();
            }

            var history = await _context.Histories
                .SingleOrDefaultAsync(x => x.Id == historyId);
            var orders = _context.Orders
              .Where(m => m.HistoryId == history.Id);

            if (history == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                var numbers = orders.Any() ? orders.Max(x => x.Number) + 1 : 1;
                var indexId = _context.Orders.Any() ? _context.Orders.Max(x => x.Index) + 1 : 1;
                var order = new Order
                {
                    HistoryId = history.Id,
                    UnitId = model.UnitId,
                    Number = numbers++,
                    Index = indexId++,
                    SerialNumber = model.SerialNumber,
                    Status = model.Status,
                    Measure = model.Measure
                };

                _context.Add(order);
                await _context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { historyId = history.Id });
            }
            ViewData["UnitId"] = new SelectList(_context.Units, "Id", "Name", model.UnitId);
            return View(model);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(Int32? historyId, Int32? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            var history = await _context.Histories
                .SingleOrDefaultAsync(x => x.Id == historyId);


            this.ViewBag.History = history;

            var model = new OrdersCreateModel
            {
                UnitId = order.UnitId,
                Index = order.Index,
                SerialNumber = order.SerialNumber,
                Amount = order.Amount,
                Status = order.Status, 
                Measure = order.Measure
            };

            ViewData["HistoryId"] = new SelectList(_context.Histories, "Id", "Id", order.HistoryId);
            ViewData["UnitId"] = new SelectList(_context.Units, "Id", "Name", order.UnitId);
            return View(model);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? historyId, Int32? id, OrdersCreateModel model)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var order = await _context.Orders.SingleOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return this.NotFound();
            }
            var history = await _context.Histories
                .SingleOrDefaultAsync(x => x.Id == historyId);


            if (ModelState.IsValid)
            {
                order.UnitId = model.UnitId;
                order.Index = model.Index;
                order.SerialNumber = model.SerialNumber;
                order.Amount = model.Amount;
                order.Status = model.Status; 
                order.Measure = model.Measure;

                await _context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { historyId = order.HistoryId });
            }
            ViewData["HistoryId"] = new SelectList(_context.Histories, "Id", "Id", order.HistoryId);
            ViewData["UnitId"] = new SelectList(_context.Units, "Id", "Name", order.UnitId);
            return View(model);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.History)
                .Include(o => o.Unit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            //var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return this.RedirectToAction("Index", new { historyId = order.HistoryId });
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
