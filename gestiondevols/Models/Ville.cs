

namespace gestiondevols.Models
{
    public class Ville
    {
        public string? IdVille { get; set; }
        public string? Nom { get; set; }

        

        // Liste des vols de départ de cette ville
        public List<Vol> VolsDepart { get; set; }

        // Liste des vols d'arrivée à cette ville
        public List<Vol> VolsArrivee { get; set; }
    }
}
