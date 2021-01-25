using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bendevarimproje.Models;
namespace bendevarimproje.Controllers
{
    public class AdminController : Controller
    {
        projedenemeEntities db = new projedenemeEntities();

        // GET: Admin
        public ActionResult Giris()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Giris(FormCollection form)
        {
            if (form["kadi"]=="admin" &&form["sifre"]== "123admin")
            {
                Session.Add("kadi", form["kadi"]);
                return RedirectToAction("Kullanicilar", "Admin");
            }
            else
            {
                ViewBag.Message = "Kullanıcı Adı veya Şifre Hatalı.Lütfen Tekrar deneyin";
                return View();
            }
           
        }
        public ActionResult AdminSayfasi()
        {

            return View();
        }
        public ActionResult Kullanicilar()
        {

            return View(db.kullanicilar.ToList());
        }
        public ActionResult KullaniciEdit(int id)
        {
            var std = db.kullanicilar.Where(s => s.id == id).FirstOrDefault();

            return View(std);
         
        }
        [HttpPost]
        public ActionResult KullaniciEdit(kullanicilar k)
        {
          foreach(var bul in db.kullanicilar.ToList())
            {
                if (bul.id == k.id)
                {
                    foreach(var i in db.istek.ToList()) {
                        if (i.mail == bul.mail) {
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
        public ActionResult Sil(int id)
        {
            foreach (var bul in db.kullanicilar.ToList())
            {
                if (bul.id == id)
                {
                    foreach (var i in db.istek.ToList())
                    {
                        if (i.mail == bul.mail)
                        {
                            foreach(var x in db.bridge.ToList()) {
                                if (i.id == x.istekid)
                                {
                                    istek varmis = (from a in db.istek where a.mail.Trim() == bul.mail.Trim() select a).FirstOrDefault();
                                    bridge silc=(from v in db.bridge where v.istekid==x.istekid select v).FirstOrDefault();
                                    if (silc != null)
                                    {
                                        db.bridge.Remove(silc);
                                        db.SaveChanges();
                                    }
                                    if (varmis != null)
                                    {
                                        db.istek.Remove(varmis);
                                        db.SaveChanges();
                                    }
                           
                            }
                            }
                        }

                    }

                }
            }
            foreach (var bul in db.kullanicilar.ToList())
            {
                if (bul.id == id)
                {
                    foreach (var i in db.bagis.ToList())
                    {
                        if (i.mail == bul.mail)
                        {
                            foreach (var x in db.bridgeb.ToList())
                            {
                                if (i.id == x.bagisid)
                                {
                                    bagis varmis = (from a in db.bagis where a.mail.Trim() == bul.mail.Trim() select a).FirstOrDefault();
                                    bridgeb silc = (from v in db.bridgeb where v.bagisid == x.bagisid select v).FirstOrDefault();
                                    if (silc != null)
                                    {
                                        db.bridgeb.Remove(silc);
                                        db.SaveChanges();
                                    }
                                    if (varmis != null)
                                    {
                                        db.bagis.Remove(varmis);
                                        db.SaveChanges();
                                    }

                                }
                            }
                        }

                    }

                }
            }
            kullanicilar sil = (from c in db.kullanicilar
                                 where c.id == id
                                 select c).FirstOrDefault();
            db.kullanicilar.Remove(sil);
            db.SaveChanges();

            return RedirectToAction("Kullanicilar");

        }
        public ActionResult Bagislar()
        {
            return View(db.bagis.ToList());
        }
        public ActionResult BagisEdit(int id)
        {
            var std = db.bagis.Where(s => s.id == id).FirstOrDefault();

            return View(std);

        }
        [HttpPost]
        public ActionResult BagisEdit(bagis k)
        {

            bagis updated = (from c in db.bagis
                                    where c.id == k.id
                                    select c).FirstOrDefault();
      
            updated.baslik = k.baslik;
            updated.aciklama = k.aciklama;
            db.SaveChanges();
            return View();
        }
        public ActionResult BagisSil(int id)
        {
            foreach(var g in db.bagis.ToList())
            {
                if (g.id == id)
                {
                    foreach(var b in db.bridgeb.ToList())
                    {
                        if (b.bagisid == g.id)
                        {
                            bagis varmis = (from a in db.bagis where a.id == g.id select a).FirstOrDefault();
                            bridgeb silc = (from v in db.bridgeb where v.bagisid == b.bagisid select v).FirstOrDefault();
                            if (silc != null)
                            {
                                db.bridgeb.Remove(silc);
                                db.SaveChanges();
                            }
                            if (varmis != null)
                            {
                                db.bagis.Remove(varmis);
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }
            return RedirectToAction("Bagislar");
        }
        public ActionResult Istekler()
        {
            return View(db.istek.ToList());
        }
        public ActionResult IstekEdit(int id)
        {
            var std = db.istek.Where(s => s.id == id).FirstOrDefault();

            return View(std);

        }
        [HttpPost]
        public ActionResult IstekEdit(istek k)
        {

            istek updated = (from c in db.istek
                             where c.id == k.id
                             select c).FirstOrDefault();

            updated.baslik = k.baslik;
            updated.aciklama = k.aciklama;
            db.SaveChanges();
            return View();
        }
        public ActionResult IstekSil(int id)
        {
            foreach (var g in db.istek.ToList())
            {
                if (g.id == id)
                {
                    foreach (var b in db.bridge.ToList())
                    {
                        if (b.istekid == g.id)
                        {
                            istek varmis = (from a in db.istek where a.id == g.id select a).FirstOrDefault();
                            bridge silc = (from v in db.bridge where v.istekid == b.istekid select v).FirstOrDefault();
                            if (silc != null)
                            {
                                db.bridge.Remove(silc);
                                db.SaveChanges();
                            }
                            if (varmis != null)
                            {
                                db.istek.Remove(varmis);
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }
            return RedirectToAction("Istekler");
        }
        public ActionResult Etiketler()
        {
            return View(db.etiket.ToList());
        }
        public ActionResult EtiketEdit(int id)
        {
            var std = db.etiket.Where(s => s.id == id).FirstOrDefault();

            return View(std);

        }
        [HttpPost]
        public ActionResult EtiketEdit(etiket k)
        {

            etiket updated = (from c in db.etiket
                             where c.id == k.id
                             select c).FirstOrDefault();

            updated.tag = k.tag;
            db.SaveChanges();
            return View();
        }
        public ActionResult EtiketSil(int id)
        {
            foreach (var g in db.etiket.ToList())
            {
                if (g.id == id)
                {
                    foreach (var b in db.bridge.ToList())
                    {
                        if (b.etiketid == g.id)
                        {
                            bridge silc = (from v in db.bridge where v.etiketid == b.etiketid select v).FirstOrDefault();
                            if (silc != null)
                            {
                                db.bridge.Remove(silc);
                                db.SaveChanges();
                            }
                        
                        }
                    }
                }
            }
            foreach (var g in db.etiket.ToList())
            {
                if (g.id == id)
                {
                    foreach (var b in db.bridgeb.ToList())
                    {
                        if (b.etiketid == g.id)
                        {
                            bridgeb silc = (from v in db.bridgeb where v.etiketid == b.etiketid select v).FirstOrDefault();
                            if (silc != null)
                            {
                                db.bridgeb.Remove(silc);
                                db.SaveChanges();
                            }

                        }
                    }
                }
            }
            etiket sil = (from c in db.etiket
                                where c.id == id
                                select c).FirstOrDefault();
            db.etiket.Remove(sil);
            db.SaveChanges();
            return RedirectToAction("Etiketler");
        }
        public ActionResult ParaBagisiKontrol()
        {
            return View(db.parabagisistek.ToList());
        }
        public ActionResult ParaBagisiSil(int id)
        {
            parabagisistek sil = (from c in db.parabagisistek where c.id == id select c).FirstOrDefault();
            db.parabagisistek.Remove(sil);
            db.SaveChanges();
            return RedirectToAction("ParaBagisiKontrol");
        }
        public ActionResult ParaBagisiOnayla(int id)
        {
            parabagisistek ekle = (from c in db.parabagisistek
                             where c.id == id
                             select c).FirstOrDefault();
            onaylananparabagisi onay = new onaylananparabagisi();
            onay.mail = ekle.mail;
            onay.miktar = ekle.miktar;
            onay.banka = ekle.banka;
            db.onaylananparabagisi.Add(onay);
            db.SaveChanges();
            db.parabagisistek.Remove(ekle);
            db.SaveChanges();

            return RedirectToAction("ParaBagisiKontrol");
        }
        public ActionResult OnaylananParaBagisi()
        {
            return View(db.onaylananparabagisi.ToList());
        }
        public ActionResult BagisEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult BagisEkle(bagisekle ba, FormCollection form)
        {
            if (Request.Files.Count > 0)
            {
                //Guid nesnesini benzersiz dosya adı oluşturmak için tanımladık ve Replace ile aradaki “-” işaretini atıp yan yana yazma işlemi yaptık.
                string DosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
                //Kaydetceğimiz resmin uzantısını aldık.
                string uzanti = System.IO.Path.GetExtension(Request.Files[0].FileName);
                string TamYolYeri = "~/Content/images/" + DosyaAdi + uzanti;
                //Eklediğimiz Resni Server.MapPath methodu ile Dosya Adıyla birlikte kaydettik.
                Request.Files[0].SaveAs(Server.MapPath(TamYolYeri));
                //Ve veritabanına kayıt için dosya adımızı değişkene aktarıyoruz.
                ba.bagisfotograf = DosyaAdi + uzanti;
            }
            ba.bagisadi = form["bagisadi"];
            ba.aciklama = form["aciklama"];
            ba.miktar =Convert.ToInt32(form["miktar"]);
            db.bagisekle.Add(ba);
            db.SaveChanges();
            return View();
        }
        public ActionResult EklenenBagislar()
        {
            return View(db.bagisekle.ToList());
        }
        public ActionResult EBagisSil(int id)
        {
            bagisekle sil = (from c in db.bagisekle where c.id == id select c).FirstOrDefault();
            db.bagisekle.Remove(sil);
            db.SaveChanges();
            return RedirectToAction("EklenenBagislar");
        }
        public ActionResult EBagisDuzenle(int id)
        {
            var std = db.bagisekle.Where(s => s.id == id).FirstOrDefault();

            return View(std);
        }
        [HttpPost]
        public ActionResult EBagisDuzenle(bagisekle k)
        {
            bagisekle updated = (from c in db.bagisekle
                              where c.id == k.id
                              select c).FirstOrDefault();

            updated.bagisadi = k.bagisadi;
            updated.aciklama = k.aciklama;
            updated.miktar = k.miktar;
            db.SaveChanges();
            return View();
        }
        public ActionResult BagisOnayla()
        {
            List<bagisyap> ba = new List<bagisyap>();
            foreach (var i in db.bagisyap.ToList())
            {
                if (i.onaydurumu.ToString().Trim() == "onaylanmadi")
                {
                    bagisyap o = new bagisyap();
                    o.id = i.id;
                    o.mail = i.mail;
                    o.bagisadi = i.bagisadi;
                    o.miktar = i.miktar;
                    o.banka = i.banka;
                    ba.Add(o);
                }
            }
            return View(ba.ToList());
        }
        public ActionResult YapilanBagisOnayla(int id)
        {
            bagisyap updated = (from c in db.bagisyap
                                 where c.id == id
                                 select c).FirstOrDefault();

            updated.onaydurumu = "onaylandi";
            db.SaveChanges();
            return RedirectToAction("BagisOnayla");
        }
        public ActionResult YapilanBagisSil(int id)
        {
            bagisyap sil = (from c in db.bagisyap where c.id == id select c).FirstOrDefault();
            db.bagisyap.Remove(sil);
            db.SaveChanges();
            return RedirectToAction("BagisOnayla");
        }
        public ActionResult BagisListele()
        {
            List<bagisyap> ba = new List<bagisyap>();
            foreach (var i in db.bagisyap.ToList())
            {
                if (i.onaydurumu.ToString().Trim() == "onaylandi")
                {
                    bagisyap o = new bagisyap();
                    o.id = i.id;
                    o.mail = i.mail;
                    o.bagisadi = i.bagisadi;
                    o.miktar = i.miktar;
                    o.banka = i.banka;
                    ba.Add(o);
                }
            }
            return View(ba.ToList());
        }
    }
}