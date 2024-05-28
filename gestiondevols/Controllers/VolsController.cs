using gestiondevols.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace gestiondevols.Controllers
{
    public class VolsController : Controller
    {
        //Interface de manipulation de la BDD FireBase
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "OMbL3yVfIHQIgVkmxbYao29cnIu1pzUBIz6J7Vdw",
            BasePath = "https://gestiondesvols-430cf-default-rtdb.europe-west1.firebasedatabase.app"



        };
        IFirebaseClient? client;


        // GET: Vols
        public ActionResult Index()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Vols");
            dynamic? data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Vol>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<Vol>(((JProperty)item).Value.ToString()));
                }
            }
            return View(list);
        }

        // GET: Vols/Details/5

        // GET: Vols/Details/5
        public ActionResult Details(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Vols/" + id);
            Vol vol = JsonConvert.DeserializeObject<Vol>(response.Body);
            return View(vol);
        }


        public ActionResult Create()
        {
            return View();
        }

        // POST: Villes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Vol vol)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
                PushResponse response = client.Push("Vol/", vol);
                vol.IdVol = response.Result.name;
                SetResponse setResponse = client.Set("Vols/" + vol.IdVol, vol);


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



        // GET: Vols/Edit/5
        public ActionResult Edit(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Vols/" + id);
            Vol vol = JsonConvert.DeserializeObject<Vol>(response.Body);
            return View(vol);
        }

        // POST: Vols/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Vol vol)
        {
            client = new FireSharp.FirebaseClient(config);
            SetResponse response = client.Set("Vols/" + vol.IdVol, vol);
            return RedirectToAction("Index");
        }
        //affichage de la vue suppression
        public ActionResult Delete(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Vols/" + id);
            Vol vol = JsonConvert.DeserializeObject<Vol>(response.Body);
            return View(vol);
        }


    }

  }
