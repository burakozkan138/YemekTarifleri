using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using YemekTarifleri.Models;
using YemekTarifleri.ViewModels;

namespace YemekTarifleri.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        // GET: Settings
        public ActionResult Index()
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = manager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return View(user);
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: Settings ProfilEdit
        public ActionResult ProfilEdit()
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = manager.FindById(User.Identity.GetUserId());

            if (user != null)
            {
                var model = new ProfilEditViewModel()
                {
                    UserName = user.UserName,
                    Sehir = user.Sehir,
                    Twitter = user.Twitter,
                    Facebook = user.Facebook,
                    Instagram = user.Instagram,
                    Youtube = user.Youtube,
                    Ad = user.FullName.Split()[0],
                    Soyad = user.FullName.Split()[1],
                    Resim = user.ProfilResmi
                };

                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        // Post: Settings ProfilEdit
        public ActionResult ProfilEdit(ProfilEditViewModel model)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = manager.FindById(User.Identity.GetUserId());
            if (!ModelState.IsValid) return View(model);

            if (user != null)
            {
                user.UserName = model.UserName;
                user.Sehir = model.Sehir;
                user.Twitter = model.Twitter;
                user.Facebook = model.Facebook;
                user.Instagram = model.Instagram;
                user.Youtube = model.Youtube;
                user.FullName = model.Ad + " " + model.Soyad;

                manager.Update(user);

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult ProfilResmi()
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = manager.FindById(User.Identity.GetUserId());

            if (user != null)
            {
                var model = new ProfilResimViewModel();
                model.Resim = user.ProfilResmi;

                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult ProfilResmi(ProfilResimViewModel model)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = manager.FindById(User.Identity.GetUserId());

            if (user != null || model.files != null)
            {
                var inputFileName = Guid.NewGuid() + Path.GetFileName(model.files.FileName);
                var serverSavePath = Path.Combine(Server.MapPath("~/Content/ProfilPhotos/"), inputFileName);
                model.files.SaveAs(serverSavePath);

                user.ProfilResmi = inputFileName;

                manager.Update(user);
                return RedirectToAction("Index", "Settings");
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult ProfilResmiDelete()
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = manager.FindById(User.Identity.GetUserId());

            if (user != null && user.ProfilResmi != "Default.jpg")
            {
                var resimPath = Path.Combine(Server.MapPath("~/Content/ProfilPhotos/"), user.ProfilResmi);
                if (System.IO.File.Exists(resimPath))
                {
                    System.IO.File.Delete(resimPath);
                }

                user.ProfilResmi = "Default.jpg";
                manager.Update(user);
                return RedirectToAction("ProfilResmi", "Settings");
            }

            return RedirectToAction("Index", "Settings");
        }
    }
}