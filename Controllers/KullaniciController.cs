using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bendevarimproje.Models;
using bendevarimproje.Controllers;
using System.Data.Entity.Core.Metadata.Edm;
using System.Net.Mail;
using System.Net;

namespace bendevarimproje.Controllers
{
    public class KullaniciController : Controller
    {
        projedenemeEntities db = new projedenemeEntities();
        // GET: Kullanici
        public ActionResult Ekle()
        {
            kullanicilar k = new kullanicilar();
            return View();
        }
        [HttpPost]
        public ActionResult Ekle(kullanicilar k)
        {
            var kontrol = db.kullanicilar.FirstOrDefault(m => m.mail == k.mail );
            if (kontrol != null)
            {
                ViewBag.Mesaj = "Bu Mail Adresiyle Daha Önceden Kayıt Yapılmış. Lütfen Yeni Bir Mail Adresi Giriniz.";
                return View();
            }
            else
            {
                db.kullanicilar.Add(k);
                db.SaveChanges();
                return View();
            }
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(kullanicilar user)
        {
            var kontrol = db.kullanicilar.FirstOrDefault(m => m.mail == user.mail && m.sifre == user.sifre);
            if (kontrol != null)
            {
                Session.Add("kmail",user.mail);
              
                return RedirectToAction("Index", "Home");


            }
            else
            {
                ViewBag.Message = "Mail Adresi veya Şifre Hatalı.Lütfen Tekrar deneyin. Denemeniz Başarısız Olmaya Devam Ederse bendevarim@gmail.com Adresine Mail Atabilirsiniz.";
                return View();
            }
    
        }
        public ActionResult ResetSifre()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ResetSifre(kullanicilar k)
        {
            var model = db.kullanicilar.Where(x => x.mail == k.mail).FirstOrDefault();
            if(model!=null)
            {
                Guid rastgele = Guid.NewGuid();
                model.sifre = rastgele.ToString().Substring(0, 8);
                db.SaveChanges();
                SmtpClient client = new SmtpClient("smtp.yandex.com",587);
                client.EnableSsl = true;
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("bendevarimuygulama@yandex.com", "Şifre Sıfırlama");
                mail.To.Add(k.mail);
                mail.IsBodyHtml = true;
                mail.Subject = "Şifre Sıfırlama İsteği";
                mail.Body += "Merhaba " + model.ad + "<br /> Yeni Şifreniz: " + model.sifre;
                client.Credentials = new NetworkCredential("bendevarimuygulama@yandex.com", "123bendevarim");
               
    
                client.Send(mail);
                return RedirectToAction("Login");
            }
            else 
            {
                ViewBag.hata = "Sistemde böyle bir mail adresi mevcut değildir.";
            return View();
            }
        }
        public ActionResult Hesabim()
        {
            return View();
        }
        public ActionResult Bilgilerim()
        {
            foreach(var e in db.kullanicilar.ToList())
            {
                if(e.mail.Trim()== Session["kmail"].ToString())
                {
                    var b = db.kullanicilar.Where(s => s.id == e.id).FirstOrDefault();
                    return View(b);
                }
            }
           
            return View();
        }
        [HttpPost]
        public ActionResult Bilgilerim(kullanicilar k)
        {
            foreach (var bul in db.kullanicilar.ToList())
            {
                if (bul.id == k.id)
                {
                    foreach (var i in db.istek.ToList())
                    {
                        if (i.mail == bul.mail)
                        {
                            istek varmis = (from a in db.istek where a.mail.Trim() == bul.mail.Trim() select a).FirstOrDefault();
                            if (varmis != null)
                            { varmis.mail = k.mail; db.SaveChanges(); }
                        }

                    }

                }
            }
            foreach (var bul in db.kullanicilar.ToList())
            {
                if (bul.id == k.id)
                {
                    foreach (var i in db.bagis.ToList())
                    {
                        if (i.mail == bul.mail)
                        {
                            bagis varmib = (from b in db.bagis where b.mail.Trim() == bul.mail.Trim() select b).FirstOrDefault();

                            if (varmib != null)
                            { varmib.mail = k.mail; db.SaveChanges(); }
                        }

                    }

                }
            }
            kullanicilar updated = (from c in db.kullanicilar
                                    where c.id == k.id
                                    select c).FirstOrDefault();


            updated.ad = k.ad;
            updated.soyad = k.soyad;
            updated.mail = k.mail;
            updated.tckimlik = k.tckimlik;
            updated.cinsiyet = k.cinsiyet;
            updated.sifre = k.sifre;
            db.SaveChanges();
            return View();
        }
        public ActionResult Isteklerim()
        {
            List<istek> ObjCustomer = new List<istek>();
            foreach (var i in db.istek.ToList())
            {
                if (i.mail == Session["kmail"].ToString())
                {
                    istek Obj = new istek();
                    Obj.id = i.id;
                    Obj.mail = i.mail.ToString();
                    Obj.baslik = i.baslik.ToString();
                    Obj.aciklama = i.aciklama.ToString();
                    Obj.etiketler = i.etiketler.ToString();
                    ObjCustomer.Add(Obj);

                }
            }
            if (ObjCustomer.ToList().Count() != 0)
            {
                return View(ObjCustomer.ToList());
            }
            else
            {
                ViewBag.Message = "Kullanıcı Hesabınıza Ait İstek Bulunamadı.";
                return View();
            }
        }
        public ActionResult IstekDuzenle(int id)
        {
            var std = db.istek.Where(s => s.id == id).FirstOrDefault();

            return View(std);      
            
        }
        [HttpPost]
        public ActionResult IstekDuzenle(istek k)
        {
            istek updated = (from c in db.istek
                              where c.id == k.id
                              select c).FirstOrDefault();

            updated.baslik = k.baslik;
            updated.aciklama = k.aciklama;
            if (updated.baslik  == null)
            {
                ViewBag.message = "Lütfen Gerekli Alanları Doldurunuz.";
            }
            else if (updated.aciklama == null)
            {
                ViewBag.message = "Lütfen Gerekli Alanları Doldurunuz.";

            }
            else { 
            db.SaveChanges();
            }
            return View();

        }
        public ActionResult IstekSil(int id)
        {
            foreach(var s in db.bridge.ToList())
            {
                bridge silc = (from v in db.bridge where v.istekid == id select v).FirstOrDefault();
                if (silc != null)
                {
                    db.bridge.Remove(silc);
                    db.SaveChanges();
                }
            }
            istek sil = (from v in db.istek where v.id == id select v).FirstOrDefault();
                db.istek.Remove(sil);
                db.SaveChanges();

            return RedirectToAction("Isteklerim");

        }
        public ActionResult Bagislarim()
        {
            List<bagis> ObjCustomer = new List<bagis>();
            foreach (var i in db.bagis.ToList())
            {
                if (i.mail == Session["kmail"].ToString())
                {
                    bagis Obj = new bagis();
                    Obj.id = i.id;
                    Obj.mail = i.mail.ToString();
                    Obj.baslik = i.baslik.ToString();
                    Obj.aciklama = i.aciklama.ToString();
                    Obj.etiketler = i.etiketler.ToString();

                    ObjCustomer.Add(Obj);

                }
            }
            if (ObjCustomer.ToList().Count() != 0)
            {
                return View(ObjCustomer.ToList());
            }
            else
            {
                ViewBag.Message = "Kullanıcı Hesabınıza Ait Bağış Bulunamadı.";
                return View();
            }
        }
        public ActionResult BagisDuzenle(int id)
        {
            var std = db.bagis.Where(s => s.id == id).FirstOrDefault();

            return View(std);

        }
        [HttpPost]
        public ActionResult BagisDuzenle(bagis k)
        {
            bagis updated = (from c in db.bagis
                             where c.id == k.id
                             select c).FirstOrDefault();

            updated.baslik = k.baslik;
            updated.aciklama = k.aciklama;
            if (updated.baslik == null)
            {
                ViewBag.message = "Lütfen Gerekli Alanları Doldurunuz.";
            }
            else if (updated.aciklama == null)
            {
                ViewBag.message = "Lütfen Gerekli Alanları Doldurunuz.";

            }
            else
            {
                db.SaveChanges();
            }
            return View();

        }
        public ActionResult BagisSil(int id)
        {
            foreach(var s in db.bridgeb.ToList())
            {
                bridgeb silc = (from v in db.bridgeb where v.bagisid == id select v).FirstOrDefault();
                if (silc != null)
                {
                    db.bridgeb.Remove(silc);
                    db.SaveChanges();
                }
            }
            bagis sil = (from v in db.bagis where v.id == id select v).FirstOrDefault();
            db.bagis.Remove(sil);
            db.SaveChanges();

            return RedirectToAction("Bagislarim");
        }
    }
}