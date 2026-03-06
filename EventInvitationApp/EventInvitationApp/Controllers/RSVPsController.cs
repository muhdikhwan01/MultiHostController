using EventInvitationApp.Data;
using EventInvitationApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EventInvitationApp.Controllers
{
    public class RSVPsController : Controller
    {
        private readonly AppDbContext _context;

        public RSVPsController(AppDbContext context)
        {
            _context = context;
        }

        // POST: RSVPs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GuestName,IsAttending,EventId")] RSVP rsvp)
        {
            // 🔎 Log what was bound from the form
            Console.WriteLine($"[DEBUG] GuestName={rsvp.GuestName}, IsAttending={rsvp.IsAttending}, EventId={rsvp.EventId}");

            if (!ModelState.IsValid)
            {
                // 🔎 Log all model state errors
                foreach (var entry in ModelState)
                {
                    foreach (var error in entry.Value.Errors)
                    {
                        Console.WriteLine($"[ERROR] {entry.Key}: {error.ErrorMessage}");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(rsvp);
                await _context.SaveChangesAsync();

                Console.WriteLine("[DEBUG] RSVP successfully saved to DB!");

                return RedirectToAction("Details", "Events", new { id = rsvp.EventId });
            }

            // ❌ If invalid, reload event and return view
            var @event = await _context.Events
                .Include(e => e.RSVPs)
                .FirstOrDefaultAsync(e => e.EventId == rsvp.EventId);

            return View("~/Views/Events/Details.cshtml", @event);
        }
    }
}
