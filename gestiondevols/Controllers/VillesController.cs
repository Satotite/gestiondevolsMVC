using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using gestiondevols.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Numerics;

namespace gestiondevols.Controllers
{
    public class VillesController : Controller
    {
        //Interface de manipulation de la BDD FireBase
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "OMbL3yVfIHQIgVkmxbYao29cnIu1pzUBIz6J7Vdw",
            BasePath = "https://gestiondesvols-430cf-default-rtdb.europe-west1.firebasedatabase.app"



        };
        IFirebaseClient? client;





        // GET: Villes
        public ActionResult Index()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Villes");
            dynamic? data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Ville>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    Ville ville = JsonConvert.DeserializeObject<Ville>(((JProperty)item).Value.ToString());
                    list.Add(ville);
                }
            }
            return View(list); 
        }

        // GET: Villes/Details/5
        // GET: Vols/Details/5
        public ActionResult Details(string id)
        {
            client = new FireSharp.FirebaseClient(config);

            // Récupérer les détails de la ville
            FirebaseResponse response = client.Get("Villes/" + id);
            Ville ville = JsonConvert.DeserializeObject<Ville>(response.Body);

            // Récupérer les vols liés à la ville (départs et arrivées)
            FirebaseResponse resVols = client.Get("Vols");
            dynamic dataVols = JsonConvert.DeserializeObject<dynamic>(resVols.Body);
            var listVolsDepart = new List<Vol>();
            var listVolsArrivee = new List<Vol>();

            if (dataVols != null)
            {
                foreach (var item in dataVols)
                {
                    var vol = JsonConvert.DeserializeObject<Vol>(((JProperty)item).Value.ToString());
                    if (vol.VilleDepart == ville.Nom)
                    {
                        listVolsDepart.Add(vol);
                    }
                    if (vol.VilleArrivee == ville.Nom)
                    {
                        listVolsArrivee.Add(vol);
                    }
                }
            }

            // Ajouter les listes de vols de départ et d'arrivée au modèle de vue
            ville.VolsDepart = listVolsDepart;
            ville.VolsArrivee = listVolsArrivee;

            return View(ville);
        }



        // GET: Villes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Villes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Ville vi)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
                PushResponse response = client.Push("Villes/", vi);
                vi.IdVille = response.Result.name;
                SetResponse setResponse = client.Set("Villes/" + vi.IdVille, vi);


                if (setResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    ModelState.AddModelError(string.Empty, "OK");
                else
                    ModelState.AddModelError(string.Empty, "KO!!");

            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return RedirectToAction("Index");
        }

        // GET: Villes/Edit/5
        public ActionResult Edit(string id)
        {
            client = new FireSharp.FirebaseClient(config);
           
            FirebaseResponse response = client.Get($"Villes/{id}");
            Ville vi = JsonConvert.DeserializeObject<Ville>(response.Body);
            return View(vi);
        }

        // POST: Villes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Ville vi)
        {
            client = new FireSharp.FirebaseClient(config);
            SetResponse response = client.Set("Villes/" + vi.IdVille, vi);
            return RedirectToAction("Index");
        }

        // GET: Villes/Delete/5
        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Villes/" + id);
            if (response.Body == "null")
            {
                return NotFound();
            }
            Ville ville = JsonConvert.DeserializeObject<Ville>(response.Body);
            return View(ville);
        }

        // POST: Villes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, Ville vi)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Delete("Villes/" + id);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

   
    
    }
}
