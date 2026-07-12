using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionSalleEmploiTemps.Models;
using GestionSalleEmploiTemps.Data;

public class ProfesseursController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public ProfesseursController(ApplicationDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public async Task<IActionResult> Index(
        string? recherche,
        string? specialite,
        string? departement,
        string? statut)
    {
        var professeurs = _context.Professeurs.AsQueryable();

        if (!string.IsNullOrWhiteSpace(recherche))
        {
            professeurs = professeurs.Where(p =>
                (p.Nom != null && p.Nom.Contains(recherche)) ||
                (p.Prenom != null && p.Prenom.Contains(recherche)) ||
                (p.Email != null && p.Email.Contains(recherche)) ||
                (p.MatriculeProfesseur != null && p.MatriculeProfesseur.Contains(recherche)) ||
                (p.Telephone != null && p.Telephone.Contains(recherche)));
        }

        if (!string.IsNullOrWhiteSpace(specialite))
        {
            professeurs = professeurs.Where(p =>
                p.Specialite != null && p.Specialite.Contains(specialite));
        }

        if (!string.IsNullOrWhiteSpace(departement))
        {
            professeurs = professeurs.Where(p =>
                p.Departement != null && p.Departement.Contains(departement));
        }

        if (!string.IsNullOrWhiteSpace(statut))
        {
            professeurs = professeurs.Where(p => p.Statut == statut);
        }

        ViewBag.Recherche = recherche;
        ViewBag.Specialite = specialite;
        ViewBag.Departement = departement;
        ViewBag.Statut = statut;

        return View(await professeurs
            .OrderBy(p => p.Nom)
            .ThenBy(p => p.Prenom)
            .ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var professeur = await _context.Professeurs
            .FirstOrDefaultAsync(m => m.Id == id);

        if (professeur == null) return NotFound();

        return View(professeur);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,Nom,Prenom,Email,Telephone,Specialite,Grade,Diplome,Departement,MatriculeProfesseur,DateEmbauche,Statut,PhotoUrl")]
        Professeur professeur,
        IFormFile? photo)
    {
        NormaliserProfesseur(professeur);

        await VerifierDoublonsAsync(professeur);

        if (photo != null && photo.Length > 0)
        {
            var resultatPhoto = await EnregistrerPhotoAsync(photo);

            if (!resultatPhoto.Succes)
            {
                ModelState.AddModelError("PhotoUrl", resultatPhoto.Message);
            }
            else
            {
                professeur.PhotoUrl = resultatPhoto.Chemin;
            }
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Add(professeur);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Professeur ajouté avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                SupprimerPhoto(professeur.PhotoUrl);
                ModelState.AddModelError("", "Impossible d'enregistrer : email, téléphone ou matricule déjà utilisé.");
            }
        }

        return View(professeur);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var professeur = await _context.Professeurs.FindAsync(id);

        if (professeur == null) return NotFound();

        return View(professeur);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int? id,
        [Bind("Id,Nom,Prenom,Email,Telephone,Specialite,Grade,Diplome,Departement,MatriculeProfesseur,DateEmbauche,Statut,PhotoUrl")]
        Professeur professeur,
        IFormFile? photoFile) // Paramètre synchronisé sur "photoFile" avec la vue
    {
        if (id != professeur.Id) return NotFound();

        NormaliserProfesseur(professeur);

        await VerifierDoublonsAsync(professeur, professeur.Id);

        // Extraction sans suivi d'état EF pour isoler l'ancienne valeur
        var ancienProfesseur = await _context.Professeurs
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == professeur.Id);

        if (ancienProfesseur == null) return NotFound();

        // On assigne par défaut l'ancienne photo, elle sera écrasée seulement si un nouveau fichier valide est fourni
        professeur.PhotoUrl = ancienProfesseur.PhotoUrl;

        if (photoFile != null && photoFile.Length > 0)
        {
            var resultatPhoto = await EnregistrerPhotoAsync(photoFile);

            if (!resultatPhoto.Succes)
            {
                ModelState.AddModelError("PhotoUrl", resultatPhoto.Message);
            }
            else
            {
                // Nettoyage de l'ancienne image uniquement en cas de succès d'écriture du nouveau fichier
                SupprimerPhoto(ancienProfesseur.PhotoUrl);
                professeur.PhotoUrl = resultatPhoto.Chemin;
            }
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(professeur);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Professeur modifié avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfesseurExists(professeur.Id))
                    return NotFound();

                throw;
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Impossible de modifier : email, téléphone ou matricule déjà utilisé.");
            }
        }

        return View(professeur);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var professeur = await _context.Professeurs
            .FirstOrDefaultAsync(m => m.Id == id);

        if (professeur == null) return NotFound();

        return View(professeur);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var professeur = await _context.Professeurs.FindAsync(id);

        if (professeur != null)
        {
            SupprimerPhoto(professeur.PhotoUrl);

            _context.Professeurs.Remove(professeur);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Professeur supprimé avec succès.";
        }

        return RedirectToAction(nameof(Index));
    }

    private bool ProfesseurExists(int? id)
    {
        return _context.Professeurs.Any(e => e.Id == id);
    }

    private void NormaliserProfesseur(Professeur professeur)
    {
        professeur.Nom = professeur.Nom?.Trim();
        professeur.Prenom = professeur.Prenom?.Trim();
        professeur.Email = professeur.Email?.Trim().ToLower();
        professeur.Telephone = professeur.Telephone?.Trim();
        professeur.Specialite = professeur.Specialite?.Trim();
        professeur.Grade = professeur.Grade?.Trim();
        professeur.Diplome = professeur.Diplome?.Trim();
        professeur.Departement = professeur.Departement?.Trim();
        professeur.MatriculeProfesseur = professeur.MatriculeProfesseur?.Trim().ToUpper();
        professeur.Statut = professeur.Statut?.Trim();
    }

    private async Task VerifierDoublonsAsync(Professeur professeur, int? idActuel = null)
    {
        if (!string.IsNullOrWhiteSpace(professeur.Email))
        {
            bool emailExiste = await _context.Professeurs.AnyAsync(p =>
                p.Id != idActuel &&
                p.Email != null &&
                p.Email.ToLower() == professeur.Email.ToLower());

            if (emailExiste)
            {
                ModelState.AddModelError("Email", "Cet email est déjà utilisé par un autre professeur.");
            }
        }

        if (!string.IsNullOrWhiteSpace(professeur.MatriculeProfesseur))
        {
            bool matriculeExiste = await _context.Professeurs.AnyAsync(p =>
                p.Id != idActuel &&
                p.MatriculeProfesseur != null &&
                p.MatriculeProfesseur.ToUpper() == professeur.MatriculeProfesseur.ToUpper());

            if (matriculeExiste)
            {
                ModelState.AddModelError("MatriculeProfesseur", "Ce matricule existe déjà.");
            }
        }

        if (!string.IsNullOrWhiteSpace(professeur.Telephone))
        {
            bool telephoneExiste = await _context.Professeurs.AnyAsync(p =>
                p.Id != idActuel &&
                p.Telephone != null &&
                p.Telephone == professeur.Telephone);

            if (telephoneExiste)
            {
                ModelState.AddModelError("Telephone", "Ce numéro de téléphone est déjà utilisé.");
            }
        }
    }

    private async Task<(bool Succes, string? Chemin, string Message)> EnregistrerPhotoAsync(IFormFile photo)
    {
        var extensionsAutorisees = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var extension = Path.GetExtension(photo.FileName).ToLower();

        if (!extensionsAutorisees.Contains(extension))
        {
            return (false, null, "Format image invalide. Utilisez JPG, PNG ou WEBP.");
        }

        if (photo.Length > 2 * 1024 * 1024)
        {
            return (false, null, "La photo ne doit pas dépasser 2 Mo.");
        }

        var dossier = Path.Combine(_environment.WebRootPath, "uploads", "professeurs");

        if (!Directory.Exists(dossier))
        {
            Directory.CreateDirectory(dossier);
        }

        var nomFichier = $"prof_{Guid.NewGuid()}{extension}";
        var cheminComplet = Path.Combine(dossier, nomFichier);

        using (var stream = new FileStream(cheminComplet, FileMode.Create))
        {
            await photo.CopyToAsync(stream);
        }

        var cheminWeb = $"/uploads/professeurs/{nomFichier}";

        return (true, cheminWeb, "Photo enregistrée.");
    }

    private void SupprimerPhoto(string? photoUrl)
    {
        if (string.IsNullOrWhiteSpace(photoUrl)) return;

        var cheminRelatif = photoUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString());
        var cheminComplet = Path.Combine(_environment.WebRootPath, cheminRelatif);

        if (System.IO.File.Exists(cheminComplet))
        {
            System.IO.File.Delete(cheminComplet);
        }
    }
}