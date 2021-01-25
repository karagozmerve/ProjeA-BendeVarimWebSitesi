using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bendevarimproje.Models;
using bendevarimproje.Controllers;

namespace bendevarimproje.Controllers
{
    public class BagistekController : Controller
    {
        projedenemeEntities db = new projedenemeEntities();

        // GET: Bagistek
        public ActionResult Istekler()
        {

                return View(db.istek.ToList());
        }
        [HttpPost]
        public ActionResult Istekler(string search)
        {
            foreach (var i in db.istek.ToList())
            {
                foreach(var e in i.etiketler.Split(','))
                {
                    if (e == search) {
                    return View(db.istek.Where(x => x.id == i.id).ToList());
                    }
                }

            }

            return View(db.istek.ToList());
        }

        public ActionResult Istekyap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Istekyap(FormCollection form)
        {
            istek m = new istek();

            m.mail = Session["kmail"].ToString();
            m.baslik = form["baslik"].Trim();
            m.aciklama = form["aciklama"].Trim();
            m.etiketler = form["tag"];
            db.istek.Add(m);
            db.SaveChanges();

            string[] tags = form["tag"].Split(',');

            foreach (var t in tags)
            {
                bridge b = new bridge();
                b.istekid = m.id;
                string eti = null;
                foreach (var g in db.etiket.ToList())
                {

                    string a = t.Trim();
                    g.tag = g.tag.Trim();
                    if (g.tag.ToLower() == a.ToLower())
                    {
                        eti = g.id.ToString();
                        break;
                    }
                }

                if (eti == null)
                {
                    etiket etik = new etiket();
                    etik.tag = t.Trim();
                    db.etiket.Add(etik);
                    db.SaveChanges();
                    eti = etik.id.ToString();
                }
                b.etiketid = Convert.ToInt32(eti);
                db.bridge.Add(b);
                db.SaveChanges();

            }
            db.SaveChanges();

            return View();
        }
        public ActionResult Bagislar()
        {

            return View(db.bagis.ToList());
        }
        [HttpPost]
        public ActionResult Bagislar(string search)
        {
            foreach (var i in db.bagis.ToList())
            {
                foreach (var e in i.etiketler.Split(','))
                {
                    if (e == search)
                    {
                        return View(db.bagis.Where(x => x.id == i.id).ToList());
                    }
                }

            }

            return View(db.bagis.ToList());
        }
        public ActionResult Bagisyap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Bagisyap(FormCollection form)
        {
            bagis ba = new bagis();

            ba.mail = Session["kmail"].ToString();
            ba.baslik = form["baslik"].Trim();
            ba.aciklama = form["aciklama"].Trim();
            ba.etiketler = form["tag"].Trim();
            db.bagis.Add(ba);
            db.SaveChanges();

            string[] tags = form["tag"].Split(',');
            foreach (var t in tags)
            {
                bridgeb br = new bridgeb();
                br.bagisid = ba.id;
                string eti = null;
                foreach (var g in db.etiket.ToList())
                {

                    string a = t.Trim();
                    g.tag = g.tag.Trim();
                    if (g.tag.ToLower() == a.ToLower())
                    {
                        eti = g.id.ToString();
                        break;
                    }
                }

                if (eti == null)
                {
                    etiket etik = new etiket();
                    etik.tag = t.Trim();
                    db.etiket.Add(etik);
                    db.SaveChanges();
                    eti = etik.id.ToString();
                }
                br.etiketid = Convert.ToInt32(eti);
                db.bridgeb.Add(br);
                db.SaveChanges();

            }
            db.SaveChanges();

            return View();
        }
        public ActionResult Eslesmeler() { return View(); }
        public ActionResult IstekEslesmesi()
        {
            List<bagis> ObjCustomer = new List<bagis>();
            List<bagis> listele = new List<bagis>();

            foreach (var g in db.istek.ToList())
            {
                if (g.mail.ToLower() == Session["kmail"].ToString())

                {
                    foreach (var t in db.bridge.ToList())
                    {
                        if (g.id == t.istekid)
                        {

                            foreach (var c in db.bridgeb.ToList())
                            {
                                if (t.etiketid == c.etiketid)
                                {
                                    //numbers.Add(c.bagisid.ToString());
                                    foreach (var n in db.bagis.ToList())
                                    {
                                        if (c.bagisid == n.id)
                                        {
                                            if (n.mail.ToLower() != Session["kmail"].ToString())
                                            {
                                               
                                                //ViewBag.deneme = new List<string>() { { "Mail Adresi:"},n.mail.ToString(), n.baslik.ToString(), n.aciklama.ToString() };
                                                bagis Obj = new bagis();
                                                Obj.id = n.id;
                                                Obj.mail = n.mail.ToString();
                                                Obj.baslik = n.baslik.ToString();
                                                Obj.aciklama = n.aciklama.ToString();
                                                if (ObjCustomer.Count() != 0) {
                                                  if(  ObjCustomer.Exists(x => x.id == n.id)!=true)
                                                    {
                                                        ObjCustomer.Add(Obj);
                                                    }
                                                       
                                                    
                                                }
                                                else { ObjCustomer.Add(Obj); }
                                                

                                            }

                                        }

                                    }

                                }
                            }
                        }
                    }

                }

            }

            return View(ObjCustomer.ToList());

        }
        public ActionResult BagisEslesmesi()
        {
            List<istek> ObjCustomer = new List<istek>();

            foreach (var g in db.bagis.ToList())
            {
                if (g.mail.ToLower() == Session["kmail"].ToString())

                {
                    foreach (var t in db.bridgeb.ToList())
                    {
                        if (g.id == t.bagisid)
                        {

                            foreach (var c in db.bridge.ToList())
                            {
                                if (t.etiketid == c.etiketid)
                                {
                                    //numbers.Add(c.bagisid.ToString());
                                    foreach (var n in db.istek.ToList())
                                    {
                                        if (c.istekid == n.id)
                                        {
                                            if (n.mail.ToLower() != Session["kmail"].ToString())
                                            {

                                                //ViewBag.deneme = new List<string>() { { "Mail Adresi:"},n.mail.ToString(), n.baslik.ToString(), n.aciklama.ToString() };

                                                istek Obj = new istek();
                                                Obj.id = n.id;
                                                Obj.mail = n.mail.ToString();
                                                Obj.baslik = n.baslik.ToString();
                                                Obj.aciklama = n.aciklama.ToString();
                                                if (ObjCustomer.Count() != 0)
                                                {

                                                    if (ObjCustomer.Exists(x => x.id == n.id) != true)
                                                    {
                                                        ObjCustomer.Add(Obj);
                                                    }


                                                }
                                                else { ObjCustomer.Add(Obj); }


                                            }

                                        }

                                    }

                                }
                            }
                        }
                    }

                }

            }


            return View(ObjCustomer.ToList());
        }
        public ActionResult ParaBagisi()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ParaBagisi(FormCollection form)
        {
            parabagisistek ba = new parabagisistek();

            ba.mail = Session["kmail"].ToString();
            ba.miktar= Convert.ToDouble(form["miktar"]);
            ba.banka = form["banka"].Trim();
            db.parabagisistek.Add(ba);
            db.SaveChanges();
            return View();
        }
        public ActionResult Bagisİslem()
        {
            double a=0;
            foreach (var b in db.onaylananparabagisi)
            {
                a = a + b.miktar;
                
            }
            ViewBag.miktar = a;
            return View();
        }
        public ActionResult Mesaj(int id)
        {
            List<bagis> ObjCustomer = new List<bagis>();
            foreach (var m in db.bagis.ToList())
            {
                if (m.id == id)
                {
                    bagis Obj = new bagis();
                    Obj.mail = m.mail.ToString();
                    Obj.baslik = m.baslik.ToString();
                    ObjCustomer.Add(Obj);
                }
            }
            return View(ObjCustomer.ToList());
        }
        public ActionResult Mesajistek(int id)
        {
            List<istek> ObjCustomer = new List<istek>();
            foreach (var m in db.istek.ToList())
            {
                if (m.id == id)
                {
                    istek Obj = new istek();
                    Obj.mail = m.mail.ToString();
                    Obj.baslik = m.baslik.ToString();
                    ObjCustomer.Add(Obj);
                }
            }
            return View(ObjCustomer.ToList());
        }
        [HttpPost]
        public ActionResult Gonder(FormCollection form)
        {
            mesaj m = new mesaj();
            m.gonderenadi = form["gonderenadi"].Trim();
            m.gonderenmail= Session["kmail"].ToString();
            m.alicimail = form["alicimail"].Trim();
            m.konu = form["konu"].Trim();
            m.mesaj1 = form["mesaj1"].Trim();
            m.durum = "okunmadi";
            db.mesaj.Add(m);
            db.SaveChanges();
            return RedirectToAction("Mesajlarım");
        }
        public ActionResult Mesajlarım()
        {
            if (Session["kmail"] == null)
            {
                return View();
            }
            List<mesaj> ObjCustomer = new List<mesaj>();
            foreach (var m in db.mesaj.ToList())
            {
                if (m.alicimail == Session["kmail"].ToString())
                {
                    if (m.durum == "okunmadi") 
                    { 
                    mesaj Obj = new mesaj();
                    Obj.id = m.id;
                    Obj.gonderenadi = m.gonderenadi.ToString();
                    Obj.gonderenmail = m.gonderenmail.ToString();
                    Obj.alicimail = m.alicimail.ToString();
                    Obj.konu = m.konu.ToString();
                    Obj.mesaj1 = m.mesaj1.ToString();
                    ObjCustomer.Add(Obj);
                        Session.Add("mesaj", Obj.mesaj1);
                    }
                }

            }
            return View(ObjCustomer.ToList());
        }
        public ActionResult MesajYanit(int id)
        {
            List<mesaj> ObjCustomer = new List<mesaj>();
            foreach (var m in db.mesaj.ToList())
            {
                if (m.id == id)
                {
                    mesaj Obj = new mesaj();
                    Obj.gonderenadi = m.gonderenadi.ToString();
                    Obj.gonderenmail = m.gonderenmail.ToString();
                    Obj.alicimail = m.alicimail.ToString();
                    Obj.konu = m.konu.ToString();
                    ObjCustomer.Add(Obj);
                }
            }
            return View(ObjCustomer.ToList());
        }
        public ActionResult Okundu(int id)
        {
            mesaj updated = (from c in db.mesaj
                              where c.id == id
                              select c).FirstOrDefault();

            updated.durum = "okundu";
            db.SaveChanges();
            return RedirectToAction("Mesajlarım");
        }
        public ActionResult OkunmusMesajlarım()
        {
            List<mesaj> ObjCustomer = new List<mesaj>();
            foreach (var m in db.mesaj.ToList())
            {
                if (m.alicimail == Session["kmail"].ToString())
                {
                    if (m.durum == "okundu")
                    {
                        mesaj Obj = new mesaj();
                        Obj.id = m.id;
                        Obj.gonderenadi = m.gonderenadi.ToString();
                        Obj.gonderenmail = m.gonderenmail.ToString();
                        Obj.alicimail = m.alicimail.ToString();
                        Obj.konu = m.konu.ToString();
                        Obj.mesaj1 = m.mesaj1.ToString();
                        ObjCustomer.Add(Obj);
                    }
                }
            }
            return View(ObjCustomer.ToList());
        }
        public ActionResult GonderdigimMesajlar()
        {
            if (Session["kmail"] == null)
            {
                return View();
            }
            List<mesaj> ObjCustomer = new List<mesaj>();
            foreach (var m in db.mesaj.ToList())
            {
                if (m.gonderenmail == Session["kmail"].ToString())
                {
                        mesaj Obj = new mesaj();
                        Obj.id = m.id;
                        Obj.gonderenadi = m.gonderenadi.ToString();
                        Obj.gonderenmail = m.gonderenmail.ToString();
                        Obj.alicimail = m.alicimail.ToString();
                        Obj.konu = m.konu.ToString();
                        Obj.mesaj1 = m.mesaj1.ToString();
                        ObjCustomer.Add(Obj);                    
                }

            }
            return View(ObjCustomer.ToList());
        }
        public ActionResult OzelBagis()
        {
            return View(db.bagisekle.ToList());
        }
        public ActionResult Bagis(int id)
        {
            List<bagisekle> ObjCustomer = new List<bagisekle>();
            foreach (var m in db.bagisekle.ToList())
            {
                if (m.id == id)
                {
                    bagisekle Obj = new bagisekle();
                    Obj.bagisadi = m.bagisadi.Trim();
                    Obj.miktar = m.miktar;
                    ObjCustomer.Add(Obj);
                }
            }
            return View(ObjCustomer.ToList());
        }
        [HttpPost]
        public ActionResult Bagis(FormCollection form)
        {
            bagisyap ba = new bagisyap();

            ba.mail = Session["kmail"].ToString();
            ba.bagisadi = form["bagisadi"];
            ba.miktar = Convert.ToString(form["miktar"]);
            ba.banka = form["banka"].Trim();
            ba.onaydurumu = "onaylanmadi";
            db.bagisyap.Add(ba);
            db.SaveChanges();
            return RedirectToAction("OzelBagis");
        }
    }
}