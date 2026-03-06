using Microsoft.AspNetCore.Mvc;
using NHibernate.Linq;
using Majlis2Go.Models;

namespace Majlis2Go.Controllers
{
    public class CustomersController : Controller
    {
        private readonly NHibernate.ISession _session;

        public CustomersController(NHibernate.ISession session)
        {
            _session = session; // Injected by DI
        }

        // GET: /Customers
        public async Task<IActionResult> Index()
        {
            var customers = await _session.Query<Customer>().ToListAsync();
            return View(customers);
        }

        // GET: /Customers/Create
        public IActionResult Create()
        {
            return View(new Customer());
        }

        // POST: /Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            using var tx = _session.BeginTransaction();
            try
            {
                customer.Id = Guid.NewGuid();
                await _session.SaveAsync(customer);
                await tx.CommitAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        // GET: /Customers/Edit/{id}
        public async Task<IActionResult> Edit(Guid id)
        {
            var customer = await _session.GetAsync<Customer>(id);
            if (customer == null) return NotFound();
            return View(customer);
        }

        // POST: /Customers/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Customer customer)
        {
            if (!ModelState.IsValid) return View(customer);

            using var tx = _session.BeginTransaction();
            try
            {
                await _session.UpdateAsync(customer);
                await tx.CommitAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        // GET: /Customers/Delete/{id}
        public async Task<IActionResult> Delete(Guid id)
        {
            var customer = await _session.GetAsync<Customer>(id);
            if (customer == null) return NotFound();
            return View(customer);
        }

        // POST: /Customers/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            using var tx = _session.BeginTransaction();
            try
            {
                var customer = await _session.GetAsync<Customer>(id);
                if (customer != null)
                {
                    await _session.DeleteAsync(customer);
                }
                await tx.CommitAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }
    }
}
