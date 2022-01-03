using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;
using WebGrease.Css.Extensions;
using YemekTarifleri.Models;
using YemekTarifleri.ViewModels;

namespace YemekTarifleri.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        //*****************************List************************//
        // GET: Admin List For Tarifler
        public ActionResult Index(string searchText, int page = 1)
        {
            using (var db = new YemekTarifleriEntities())
            {
                var tarifler = db.Tarifler.ToList();
                if (page <= 0) page = 1;
                if (!String.IsNullOrWhiteSpace(searchText))
                {
                    tarifler = db.Tarifler.Where(s => s.Name.Contains(searchText)).ToList();
                }

                return View("Tarifler", tarifler.ToPagedList(page, 10));
            }
        }

        // GET: Admin List For Kategoriler
        public ActionResult Kategoriler(string searchText, int page = 1)
        {
            using (var db = new YemekTarifleriEntities())
            {
                var kategori = db.Kategoriler.ToList();
                if (page <= 0) page = 1;
                if (!String.IsNullOrEmpty(searchText))
                {
                    kategori = db.Kategoriler.Where(s => s.Name.Contains(searchText)).ToList();
                }

                return View(kategori.ToPagedList(page, 10));
            }
        }

        // GET: Admin List For Yorumlar
        public ActionResult Yorumlar(string searchText, int page = 1)
        {
            using (var db = new YemekTarifleriEntities())
            {
                var yorumlar = db.Yorumlar.ToList();
                if (page <= 0) page = 1;
                if (!String.IsNullOrEmpty(searchText))
                {
                    yorumlar = db.Yorumlar.Where(s => s.Text.Contains(searchText)).ToList();
                }

                return View(yorumlar.ToPagedList(page, 10));
            }
        }

        // GET: Admin List For Kullanıcilar
        public ActionResult Kullanicilar(string searchText, int page = 1)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            using (var db = new ApplicationDbContext())
            {
                //var kullanıcılar = db.Users.ToList();
                var kullanıcılar = manager.Users.ToList();
                if (page <= 0) page = 1;
                if (!String.IsNullOrEmpty(searchText))
                {
                    //kullanıcılar = db.Users.Where(s => s.UserName.Contains(searchText) || s.Email.Contains(searchText)).ToList();
                    kullanıcılar = manager.Users.Where(s => s.UserName.Contains(searchText) || s.Email.Contains(searchText)).ToList();
                }

                return View(kullanıcılar.ToPagedList(page, 10));
            }
        }
        //*****************************List************************//

        //*****************************Create************************//
        // GET Create Tarifler
        public ActionResult TarifCreate()
        {
            using (var db = new YemekTarifleriEntities())
            {
                ViewBag.Kategoriler = db.Kategoriler.ToList();
                return View();
            }
        }

        // POST Create Tarifler
        [HttpPost]
        public ActionResult TarifCreate(TarifCreateViewModel model)
        {
            using (var db = new YemekTarifleriEntities())
            {
                ViewBag.Kategoriler = db.Kategoriler.ToList();
            }

            var Tarif = new Tarifler();

            if (model?.KategoriId == null || !ModelState.IsValid) return View(model);
            using (var db = new YemekTarifleriEntities())
            {
                Tarif.Name = model.Name;
                Tarif.YSuresi = model.YSuresi;
                Tarif.Porsiyon = model.Porsiyon;
                Tarif.Durum = model.Durum;
                Tarif.OTarihi = DateTime.Now;
                Tarif.UserId = HttpContext.User.Identity.GetUserId();
                Tarif.KategoriId = model.KategoriId;
                Tarif.Malzemeler = model.Malzemeler.Split(',').Select(malzeme => new Malzemeler() { Name = malzeme }).ToList();
                Tarif.YapimAsamalari = model.YapimAsamalari.Split(',').Select(asama => new YapimAsamalari() { Name = asama }).ToList();
                Directory.CreateDirectory(Server.MapPath("~/Content/UploadedFiles/") + model.Name.Replace(" ", "_"));
                var dbfiles = new List<string>();
                foreach (var resim in model.files.Select((value, i) => new { i, value }))
                {
                    //Checking file is available to save.
                    if (resim.value == null || resim.value.ContentLength <= 0) continue;
                    var inputFileName = Path.GetFileName(resim.value.FileName);
                    var serverSavePath = Path.Combine(Server.MapPath("~/Content/UploadedFiles/"), model.Name.Replace(" ", "_"), inputFileName);
                    dbfiles.Add(inputFileName);
                    resim.value.SaveAs(serverSavePath);
                }

                Tarif.Resimler = dbfiles.Select(resim => new Resimler() { Name = resim }).ToList();
                db.Tarifler.Add(Tarif);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // GET Create Kategori
        public ActionResult KategoriCreate()
        {
            return View();
        }

        // POST Create Kategori
        [HttpPost]
        public ActionResult KategoriCreate(Kategoriler kategori)
        {
            if (!ModelState.IsValid) return View(kategori);
            using (var db = new YemekTarifleriEntities())
            {
                db.Kategoriler.Add(kategori);
                db.SaveChanges();
                return RedirectToAction("Kategoriler");
            }
        }
        //*****************************Create************************//

        //*****************************Edit************************//
        // GET Edit Tarif
        public ActionResult TarifEdit(int id)
        {
            using (var db = new YemekTarifleriEntities())
            {
                var tarif = db.Tarifler.Include(x => x.Resimler).Include(x => x.Malzemeler).Include(x => x.Yorumlar).Include(x => x.YapimAsamalari).FirstOrDefault(f => f.Id == id);

                if (tarif != null)
                {
                    ViewBag.Kategoriler = db.Kategoriler.ToList();
                    var model = new TarifEditViewModel();
                    model.Id = id;
                    model.Name = tarif.Name;
                    model.YSuresi = tarif.YSuresi;
                    model.Porsiyon = tarif.Porsiyon;
                    model.Durum = tarif.Durum;
                    model.KategoriId = tarif.KategoriId;
                    tarif.Malzemeler.ForEach(t => { model.Malzemeler += t.Name + ","; });
                    tarif.YapimAsamalari.ForEach(t => { model.YapimAsamalari += t.Name + ","; });
                    model.Resimler = tarif.Resimler.ToList();

                    return View(model);
                }

                return RedirectToAction("Index", "Home");
            }
        }

        // POST Edit Tarif
        [HttpPost]
        public ActionResult TarifEdit(TarifEditViewModel model)
        {
            using (var db = new YemekTarifleriEntities())
            {
                ViewBag.Kategoriler = db.Kategoriler.ToList();
            }

            var Tarif = new Tarifler();

            if (model?.KategoriId == null || !ModelState.IsValid) return View(model);
            using (var db = new YemekTarifleriEntities())
            {
                var realTarif = db.Tarifler.FirstOrDefault(s => s.Id == model.Id);
                if (realTarif.Name != model.Name)
                {
                    var oldPath = Path.Combine(Server.MapPath("~/Content/UploadedFiles/"), realTarif.Name.Replace(" ", "_"));
                    var newPath = Path.Combine(Server.MapPath("~/Content/UploadedFiles/"), model.Name.Replace(" ", "_"));
                    Directory.Move(oldPath, newPath);
                }

                Tarif.Id = model.Id;
                Tarif.Name = model.Name;
                Tarif.YSuresi = model.YSuresi;
                Tarif.Porsiyon = model.Porsiyon;
                Tarif.Durum = model.Durum;
                Tarif.KategoriId = model.KategoriId;
                Tarif.OTarihi = realTarif.OTarihi;
                Tarif.UserId = realTarif.UserId;
                var Malzemeler = model.Malzemeler.Split(',').Select(malzeme => new Malzemeler() { Name = malzeme, TarifId = model.Id }).ToList();
                var YapimAsamalari = model.YapimAsamalari.Split(',').Select(asama => new YapimAsamalari() { Name = asama, TarifId = model.Id }).ToList();

                if (model.files != null)
                {
                    var dbfiles = new List<string>();
                    foreach (var resim in model.files.Select((value, i) => new { i, value }))
                    {
                        //Checking file is available to save.
                        if (resim.value == null || resim.value.ContentLength <= 0) continue;
                        var inputFileName = Path.GetFileName(resim.value.FileName);
                        var serverSavePath = Path.Combine(Server.MapPath("~/Content/UploadedFiles/"), model.Name.Replace(" ", "_"), inputFileName);
                        dbfiles.Add(inputFileName);
                        resim.value.SaveAs(serverSavePath);
                    }

                    var resimler = dbfiles.Select(resim => new Resimler() { Name = resim, TarifId = model.Id }).ToList();
                    db.Resimler.AddRange(resimler);
                }

                db.Malzemeler.RemoveRange(db.Malzemeler.Where(s => s.TarifId == model.Id).ToList());
                db.YapimAsamalari.RemoveRange(db.YapimAsamalari.Where(s => s.TarifId == model.Id).ToList());

                db.Malzemeler.AddRange(Malzemeler);
                db.YapimAsamalari.AddRange(YapimAsamalari);

                db.Tarifler.AddOrUpdate(Tarif);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // GET Edit Kategori
        public ActionResult KategoriEdit(int id)
        {
            using (var db = new YemekTarifleriEntities())
            {
                var kategori = db.Kategoriler.FirstOrDefault(k => k.Id == id);
                if (kategori != null)
                {
                    return View(kategori);
                }

                return RedirectToAction("Index", "Home");
            }
        }

        // POST Edit Kategori
        [HttpPost]
        public ActionResult KategoriEdit(Kategoriler kategori)
        {
            if (!ModelState.IsValid) return View(kategori);
            using (var db = new YemekTarifleriEntities())
            {
                db.Entry(kategori).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Kategoriler");
            }
        }

        // GET Edit Yorum
        public ActionResult YorumEdit(int id)
        {
            using (var db = new YemekTarifleriEntities())
            {
                var yorum = db.Yorumlar.FirstOrDefault(k => k.Id == id);
                if (yorum != null)
                {
                    return View(yorum);
                }

                return RedirectToAction("Index", "Home");
            }
        }

        // POST Edit Yorum
        [HttpPost]
        public ActionResult YorumEdit(Yorumlar model)
        {
            if (!ModelState.IsValid) return View(model);
            using (var db = new YemekTarifleriEntities())
            {
                var Yorum = db.Yorumlar.FirstOrDefault(d => d.Id == model.Id);
                Yorum.Text = model.Text;

                db.Yorumlar.AddOrUpdate(Yorum);
                db.SaveChanges();
                return RedirectToAction("Yorumlar");
            }
        }
        //*****************************Edit************************//

        //*****************************Delete************************//
        // GET Delete Tarif
        public ActionResult TarifDelete(int id)
        {
            using (var db = new YemekTarifleriEntities())
            {
                var tarif = db.Tarifler.Find(id);
                if (tarif != null)
                {
                    db.Malzemeler.RemoveRange(tarif.Malzemeler);
                    db.Resimler.RemoveRange(tarif.Resimler);
                    db.YapimAsamalari.RemoveRange(tarif.YapimAsamalari);
                    db.Yorumlar.RemoveRange(tarif.Yorumlar);
                    db.Tarifler.Remove(tarif);

                    if (Directory.Exists(Server.MapPath("~/Content/UploadedFiles/") + tarif.Name.Replace(" ", "_")))
                    {
                        Directory.Delete(Server.MapPath("~/Content/UploadedFiles/") + tarif.Name.Replace(" ", "_"), true);
                    }

                    db.SaveChanges();
                    return RedirectToAction("Index", "Admin");
                }

                return RedirectToAction("Index", "Home");
            }
        }

        // GET Delete Kategori
        public ActionResult KategoriDelete(int id)
        {
            using (var db = new YemekTarifleriEntities())
            {
                var kategori = db.Kategoriler.Find(id);

                if (kategori != null)
                {
                    db.Kategoriler.Remove(kategori);
                    db.SaveChanges();
                    return RedirectToAction("Kategoriler", "Admin");
                }

                return RedirectToAction("Index", "Home");
            }
        }

        // GET Delete Resim
        public ActionResult ResimDelete(int id)
        {
            using (var db = new YemekTarifleriEntities())
            {
                var resim = db.Resimler.Find(id);

                if (resim != null)
                {
                    var resimPath = Path.Combine(Server.MapPath("~/Content/UploadedFiles/"), resim.Tarifler.Name.Replace(" ", "_"), resim.Name);
                    if (System.IO.File.Exists(resimPath))
                    {
                        System.IO.File.Delete(resimPath);
                    }

                    db.Resimler.Remove(resim);
                    db.SaveChanges();
                    return RedirectToAction("TarifEdit", "Admin", new { id = resim.TarifId });
                }

                return RedirectToAction("Index", "Home");
            }
        }

        // GET Delete Yorum
        public ActionResult YorumDelete(int id)
        {
            using (var db = new YemekTarifleriEntities())
            {
                var yorum = db.Yorumlar.Find(id);

                if (yorum != null)
                {
                    db.Yorumlar.Remove(yorum);
                    db.SaveChanges();
                    return RedirectToAction("Yorumlar", "Admin");
                }

                return RedirectToAction("Index", "Home");
            }
        }
        //*****************************Delete************************//
    }
}