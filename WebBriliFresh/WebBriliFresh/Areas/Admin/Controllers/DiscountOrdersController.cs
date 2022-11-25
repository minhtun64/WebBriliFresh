﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBriliFresh.Models;

namespace WebBriliFresh.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DiscountOrdersController : Controller
    {
        private readonly BriliFreshDbContext _context;

        public DiscountOrdersController(BriliFreshDbContext context)
        {
            _context = context;
        }

        // GET: Admin/DiscountOrders
        public async Task<IActionResult> Index()
        {
            return View(await _context.DiscountOrders.ToListAsync());
        }

        // GET: Admin/DiscountOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DiscountOrders == null)
            {
                return NotFound();
            }

            var discountOrder = await _context.DiscountOrders
                .FirstOrDefaultAsync(m => m.DisId == id);
            if (discountOrder == null)
            {
                return NotFound();
            }

            return View(discountOrder);
        }

        // GET: Admin/DiscountOrders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/DiscountOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DisId,DisCode,DisRate,MaxDis,StartDate,EndDate,CusType")] DiscountOrder discountOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(discountOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(discountOrder);
        }

        // GET: Admin/DiscountOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DiscountOrders == null)
            {
                return NotFound();
            }

            var discountOrder = await _context.DiscountOrders.FindAsync(id);
            if (discountOrder == null)
            {
                return NotFound();
            }
            return View(discountOrder);
        }

        // POST: Admin/DiscountOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DisId,DisCode,DisRate,MaxDis,StartDate,EndDate,CusType")] DiscountOrder discountOrder)
        {
            if (id != discountOrder.DisId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(discountOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscountOrderExists(discountOrder.DisId))
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
            return View(discountOrder);
        }

        // GET: Admin/DiscountOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DiscountOrders == null)
            {
                return NotFound();
            }

            var discountOrder = await _context.DiscountOrders
                .FirstOrDefaultAsync(m => m.DisId == id);
            if (discountOrder == null)
            {
                return NotFound();
            }

            return View(discountOrder);
        }

        // POST: Admin/DiscountOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DiscountOrders == null)
            {
                return Problem("Entity set 'BriliFreshDbContext.DiscountOrders'  is null.");
            }
            var discountOrder = await _context.DiscountOrders.FindAsync(id);
            if (discountOrder != null)
            {
                _context.DiscountOrders.Remove(discountOrder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiscountOrderExists(int id)
        {
            return _context.DiscountOrders.Any(e => e.DisId == id);
        }
    }
}
