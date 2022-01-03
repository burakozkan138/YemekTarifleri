using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using YemekTarifleri.Models;

namespace YemekTarifleri.ViewModels
{
    public class TarifViewModel
    {
        public Tarifler Tarif { get; set; }

        public List<ApplicationUser> Kullanicilar { get; set; }

        /*Yeni Yorum*/
        [Required(ErrorMessage = "Yorum Alanı Boş Bırakılamaz.")]
        [Display(Name = "Yorum")]
        [MaxLength(250)]
        public string Yorum { get; set; }

        [Required] public int TarifId { get; set; }
    }

    public class ProfilEditViewModel
    {
        [Required(ErrorMessage = "Kullanıcı Adı Boş Bırakılamaz.")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Adınız Boş Bırakılamaz.")]
        [Display(Name = "Ad")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Soyad Boş Bırakılamaz.")]
        [Display(Name = "Soyad")]
        public string Soyad { get; set; }

        [Display(Name = "Şehriniz")] public string Sehir { get; set; }
        [Display(Name = "Youtube")] public string Youtube { get; set; }
        [Display(Name = "Facebook")] public string Facebook { get; set; }
        [Display(Name = "Twitter")] public string Twitter { get; set; }
        [Display(Name = "Instagram")] public string Instagram { get; set; }
        public string Resim { get; set; }
    }

    public class ProfilResimViewModel
    {
        [Display(Name = "Resimler")] public string Resim { get; set; }
        [Display(Name = "Resim Seç")] public HttpPostedFileBase files { get; set; }
    }
}