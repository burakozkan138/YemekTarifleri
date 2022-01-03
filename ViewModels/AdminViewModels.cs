using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using YemekTarifleri.Models;

namespace YemekTarifleri.ViewModels
{
    public class TarifCreateViewModel
    {
        [Required(ErrorMessage = "Tarif Adı Boş Bırakılamaz.")]
        [Display(Name = "Tarif Adı")]
        [MaxLength(250)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Yapım Süresi Boş Bırakılamaz.")]
        [Display(Name = "Yapım Süresi")]
        [MaxLength(250)]
        public string YSuresi { get; set; }

        [Required(ErrorMessage = "Porsiyon Boş Bırakılamaz.")]
        [Display(Name = "Porsiyon")]
        [MaxLength(250)]
        public string Porsiyon { get; set; }

        [Display(Name = "Yayınlansın Mı?")] public bool Durum { get; set; }

        [Display(Name = "Kategori")]
        [Required(ErrorMessage = "Lütfen Kategori Seçiniz!")]
        public int KategoriId { get; set; }

        [Required(ErrorMessage = "Lütfen Malzeme Ekleyiniz!")]
        [Display(Name = "Kullanılacak Malzemeler")]
        public string Malzemeler { get; set; }

        [Required(ErrorMessage = "Lütfen Aşama Ekleyiniz!")]
        [Display(Name = "Yapım Aşamaları")]
        public string YapimAsamalari { get; set; }

        [Required(ErrorMessage = "Lütfen Resim Ekleyiniz!")]
        [Display(Name = "Resimler")]
        public HttpPostedFileBase[] files { get; set; }
    }

    public class TarifEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tarif Adı Boş Bırakılamaz.")]
        [Display(Name = "Tarif Adı")]
        [MaxLength(250)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Yapım Süresi Boş Bırakılamaz.")]
        [Display(Name = "Yapım Süresi")]
        [MaxLength(250)]
        public string YSuresi { get; set; }

        [Required(ErrorMessage = "Porsiyon Boş Bırakılamaz.")]
        [Display(Name = "Porsiyon")]
        [MaxLength(250)]
        public string Porsiyon { get; set; }

        [Display(Name = "Yayınlansın Mı?")] public bool Durum { get; set; }

        [Display(Name = "Kategori")]
        [Required(ErrorMessage = "Lütfen Kategori Seçiniz!")]
        public int KategoriId { get; set; }

        [Required(ErrorMessage = "Lütfen Malzeme Ekleyiniz!")]
        [Display(Name = "Kullanılacak Malzemeler")]
        public string Malzemeler { get; set; }

        [Required(ErrorMessage = "Lütfen Aşama Ekleyiniz!")]
        [Display(Name = "Yapım Aşamaları")]
        public string YapimAsamalari { get; set; }

        [Display(Name = "Resimler")] public List<Resimler> Resimler { get; set; }

        [Display(Name = "Resim Ekle")] public HttpPostedFileBase[] files { get; set; }
    }
}