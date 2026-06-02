using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EMIT_EDT.Data;
using EMIT_EDT.Models;
using Microsoft.AspNetCore.Authorization;

namespace EMIT_EDT.Controllers
{
    /// <summary>
    /// Contrôleur de gestion des professeurs (enseignants).
    /// Chaque professeur est lié à un compte utilisateur Identity.
    /// </summary>
    [Authorize]
    public class EnseignantController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Utilisateur> _userManager;

        public EnseignantController(ApplicationDbContext context, UserManager<Utilisateur> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ========== LISTE ==========
        public async Task<IActionResult> Index()
        {
            var enseignants = await _context.Enseignants
                .Include(e => e.Utilisateur)
                .Include(e => e.Matieres)
                .OrderBy(e => e.Utilisateur!.Nom)
                .ToListAsync();
            return View(enseignants);
        }

        // ========== AJOUT (GET) ==========
        [Authorize(Roles = "SuperAdmin,RespEDT")]
        public IActionResult Create()
        {
            ViewBag.Matieres = new SelectList(_context.MatiereUEs, "MatiereUEId", "Nom");
            return View();
        }

        // ========== AJOUT (POST) ==========
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,RespEDT")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Enseignant enseignant, string email, string password, string nom, string prenom)
        {
            // 1. Créer le compte utilisateur Identity
            var user = new Utilisateur
            {
                UserName = email,
                Email = email,
                Nom = nom,
                Prenom = prenom,
                Role = "Enseignant",
                Actif = true,
                DateCreation = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                ViewBag.Matieres = new SelectList(_context.MatiereUEs, "MatiereUEId", "Nom");
                return View(enseignant);
            }

            await _userManager.AddToRoleAsync(user, "Enseignant");

            // 2. Associer l'enseignant à l'utilisateur créé
            enseignant.UtilisateurId = user.Id;

            // 3. Supprimer la validation du champ UtilisateurId (il est maintenant attribué)
            ModelState.Remove("UtilisateurId");

            // 4. Vérifier la validité du modèle (maintenant que UtilisateurId est satisfait)
            if (ModelState.IsValid)
            {
                _context.Enseignants.Add(enseignant);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Professeur créé avec succès.";
                return RedirectToAction(nameof(Index));
            }

            // En cas d'échec de validation, supprimer le compte utilisateur créé pour ne pas le laisser orphelin
            await _userManager.DeleteAsync(user);
            ViewBag.Matieres = new SelectList(_context.MatiereUEs, "MatiereUEId", "Nom");
            return View(enseignant);
        }

        // ========== MODIFICATION (GET) ==========
        [Authorize(Roles = "SuperAdmin,RespEDT")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var enseignant = await _context.Enseignants
                .Include(e => e.Utilisateur)
                .FirstOrDefaultAsync(e => e.EnseignantId == id);
            if (enseignant == null) return NotFound();

            ViewBag.Matieres = new SelectList(_context.MatiereUEs, "MatiereUEId", "Nom");
            return View(enseignant);
        }

        // ========== MODIFICATION (POST) ==========
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,RespEDT")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Enseignant enseignant, string? email, string? nom, string? prenom)
        {
            if (id != enseignant.EnseignantId) return NotFound();

            var existing = await _context.Enseignants
                .Include(e => e.Utilisateur)
                .FirstOrDefaultAsync(e => e.EnseignantId == id);
            if (existing == null) return NotFound();

            // Mise à jour des champs de l'enseignant
            existing.Matricule = enseignant.Matricule;
            existing.Titre = enseignant.Titre;
            existing.Contact = enseignant.Contact;
            existing.DureTravaille = enseignant.DureTravaille;
            existing.Statut = enseignant.Statut;

            // Mise à jour des informations de l'utilisateur associé (si présentes)
            if (existing.Utilisateur != null)
            {
                if (!string.IsNullOrEmpty(email)) existing.Utilisateur.Email = email;
                if (!string.IsNullOrEmpty(nom)) existing.Utilisateur.Nom = nom;
                if (!string.IsNullOrEmpty(prenom)) existing.Utilisateur.Prenom = prenom;
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Professeur modifié avec succès.";
            return RedirectToAction(nameof(Index));
        }

        // ========== SUPPRESSION ==========
        [Authorize(Roles = "SuperAdmin,RespEDT")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var enseignant = await _context.Enseignants
                .Include(e => e.Utilisateur)
                .FirstOrDefaultAsync(e => e.EnseignantId == id);
            if (enseignant == null) return NotFound();

            // Supprimer d'abord le compte utilisateur associé
            if (enseignant.Utilisateur != null)
                await _userManager.DeleteAsync(enseignant.Utilisateur);

            // Ensuite supprimer l'enseignant
            _context.Enseignants.Remove(enseignant);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Professeur supprimé.";
            return RedirectToAction(nameof(Index));
        }
    }
}