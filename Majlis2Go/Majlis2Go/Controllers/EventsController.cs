using Microsoft.AspNetCore.Mvc;
using Majlis2Go.Models.Domain;

public class EventsController : Controller
{
    private readonly IRepository<Event> _repo;

    public EventsController(IRepository<Event> repo)
    {
        _repo = repo;
    }

    public IActionResult Index()
    {
        var events = _repo.GetAll();
        return View(events);
    }

    public IActionResult Details(Guid id)
    {
        var ev = _repo.Get(id);
        if (ev == null) return NotFound();
        return View(ev);
    }

    public IActionResult Create() => View();

    [HttpPost]
    public IActionResult Create(Event model)
    {
        if (!ModelState.IsValid) return View(model);
        _repo.Add(model);
        return RedirectToAction(nameof(Index));
    }
}
