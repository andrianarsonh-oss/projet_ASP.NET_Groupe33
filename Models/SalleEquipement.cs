namespace EMIT_EDT.Models
{
    public class SalleEquipement
    {
        public int SalleId { get; set; }
        public Salle? Salle { get; set; }

        public int EquipementId { get; set; }
        public Equipement? Equipement { get; set; }

        public int Quantite { get; set; } = 1;

        public string Etat { get; set; } = "Fonctionnel";
    }
}