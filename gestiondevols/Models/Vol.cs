namespace gestiondevols.Models
{
    public class Vol
    {
        public string IdVol { get; set; }
        public string Reference { get; set; }
        
        // Ville de départ
        public string? VilleDepart { get; set; }
        // Ville d'arrivée
        public string? VilleArrivee { get; set; }

        public DateTime TempsDepart { get; set; }
        public DateTime TempsArrivee { get; set; }

        // Clé étrangère de la ville de départ
        public string? IdVille { get; set; }

    }
}
