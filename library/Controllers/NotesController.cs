using library.Data;
using library.Models;
using Microsoft.AspNetCore.Mvc;

namespace library.Controllers;

public class NotesController : Controller
{
    private readonly NoteRepository _notes;

    // ASP.NET Core hands in the repository, because Program.cs registered it.
    public NotesController(NoteRepository notes)
    {
        _notes = notes;
    }

    // GET /Notes : the list of notes.
    public async Task<IActionResult> Index()
    {
        var notes = await _notes.GetAllAsync();
        return View(notes); // Views/Notes/Index.cshtml
    }
    // GET /Notes/Create : an empty form.
    public IActionResult Create()
    {
        return View(new Note());
    }

    // POST /Notes/Create : the form was submitted.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Note note)
    {
        if (!ModelState.IsValid) // the title was empty or too long
        {
            return View(note);
        }
        await _notes.AddAsync(note);
        return RedirectToAction(nameof(Index));
    }
}