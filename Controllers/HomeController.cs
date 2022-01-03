using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using YemekTarifleri.Models;
using YemekTarifleri.ViewModels;

namespace YemekTarifleri.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var db = new YemekTarifleriEntities())
            {
                return View(db.Kategoriler.Include(x => x.Tarifler).ToList());
            }
        }

        //GET Home/Tarif/1
        public ActionResult Tarif(int id)
        {
            var model = new TarifViewModel();
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            using (var db = new YemekTarifleriEntities())
            {
                model.Tarif = db.Tarifler.Include(x => x.Resimler).Include(x => x.Malzemeler).Include(x => x.Yorumlar).Include(x => x.YapimAsamalari).FirstOrDefault(f => f.Id == id);
                if (model.Tarif == null) return RedirectToAction("Index", "Home");
                /*using (var app = new ApplicationDbContext())
                {
                    model.Kullanicilar = app.Users.ToList();
                }*/

                model.Kullanicilar = manager.Users.ToList();
                return View(model);
            }
        }

        //GET Home/RandomTarif
        public ActionResult RandomTarif()
        {
            using (var db = new YemekTarifleriEntities())
            {
                var tarifler = db.Tarifler.ToList();
                var rid = new Random().Next(1, tarifler.Count);

                return RedirectToAction("Tarif", new { id = rid });
            }
        }

        //GET Home/Kategori/1
        public ActionResult Kategori(int id)
        {
            using (var db = new YemekTarifleriEntities())
            {
                var kategori = db.Kategoriler.Include(x => x.Tarifler).FirstOrDefault(f => f.Id == id);
                if (kategori != null)
                {
                    return View(kategori);
                }

                return RedirectToAction("Index", "Home");
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult YorumCreate(TarifViewModel model)
        {
            var yorum = new Yorumlar();
            using (var db = new YemekTarifleriEntities())
            {
                yorum.Text = model.Yorum;
                yorum.UserId = HttpContext.User.Identity.GetUserId();
                yorum.TarifId = model.TarifId;
                yorum.OTarihi = DateTime.Now;
                db.Yorumlar.Add(yorum);
                db.SaveChanges();
                return RedirectToAction("Tarif", new { id = model.TarifId });
            }
        }

        //GET Navbar
        public ActionResult Navbar()
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = manager.FindById(User.Identity.GetUserId());
            // kategori for layout.
            using (var db = new YemekTarifleriEntities())
            {
                ViewBag.UserProfilPhotos = user != null ? user.ProfilResmi : "Default.jpg";

                var kategoriler = db.Kategoriler.ToList();
                return PartialView("_NavbarPartial", kategoriler);
            }
        }
    }
}