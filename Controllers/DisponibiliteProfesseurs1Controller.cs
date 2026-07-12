
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionSalleEmploiTemps.Models;
using GestionSalleEmploiTemps.Data;

public class DisponibiliteProfesseurs1Controller : Controller
{
    private readonly ApplicationDbContext _context;

    public DisponibiliteProfesseurs1Controller(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: DISPONIBILITEPROFESSEURS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.DisponibilitesProfesseurs.ToListAsync());
    }

    // GET: DISPONIBILITEPROFESSEURS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var disponibiliteprofesseur = await _context.DisponibilitesProfesseurs
            .FirstOrDefaultAsync(m => m.Id == id);
        if (disponibiliteprofesseur == null)
        {
            return NotFound();
        }

        return View(disponibiliteprofesseur);
    }

    // GET: DISPONIBILITEPROFESSEURS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: DISPONIBILITEPROFESSEURS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,ProfesseurId,Jour,HeureDebut,HeureFin,Statut,Professeur")] DisponibiliteProfesseur disponibiliteprofesseur)
    {
        if (ModelState.IsValid)
        {
            _context.Add(disponibiliteprofesseur);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(disponibiliteprofesseur);
    }

    // GET: DISPONIBILITEPROFESSEURS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var disponibiliteprofesseur = await _context.DisponibilitesProfesseurs.FindAsync(id);
        if (disponibiliteprofesseur == null)
        {
            return NotFound();
        }
        return View(disponibiliteprofesseur);
    }

    // POST: DISPONIBILITEPROFESSEURS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,ProfesseurId,Jour,HeureDebut,HeureFin,Statut,Professeur")] DisponibiliteProfesseur disponibiliteprofesseur)
    {
        if (id != disponibiliteprofesseur.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(disponibiliteprofesseur);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DisponibiliteProfesseurExists(disponibiliteprofesseur.Id))
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
        return View(disponibiliteprofesseur);
    }

    // GET: DISPONIBILITEPROFESSEURS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var disponibiliteprofesseur = await _context.DisponibilitesProfesseurs
            .FirstOrDefaultAsync(m => m.Id == id);
        if (disponibiliteprofesseur == null)
        {
            return NotFound();
        }

        return View(disponibiliteprofesseur);
    }

    // POST: DISPONIBILITEPROFESSEURS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var disponibiliteprofesseur = await _context.DisponibilitesProfesseurs.FindAsync(id);
        if (disponibiliteprofesseur != null)
        {
            _context.DisponibilitesProfesseurs.Remove(disponibiliteprofesseur);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool DisponibiliteProfesseurExists(int? id)
    {
        return _context.DisponibilitesProfesseurs.Any(e => e.Id == id);
    }
}
