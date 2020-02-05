using sgop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Estimacion.Controllers
{
    public class RolloController : Controller
    {
        // GET: Rollo
        public ActionResult RolloFotografico(FormCollection fc)
        {
            int idLicitacion = Convert.ToInt32(fc["licitacion1"]);
            int idRequisicion = 0;
            List<EstimacionInnerLicitacionViewModel> lst;
            List<EstimacionViewModel> lstEstimacion;
            List<EstimacionViewModel> lstEstimacion2 = new List<EstimacionViewModel>();
            using (sgopEntities db = new sgopEntities())
            {

                lst = (from d in db.licitaciones
                       join e in db.catalogoMunicipios on d.idMunicipio equals e.idMunicipio
                       join f in db.requisiciones on d.idRequisicion equals f.idRequisicionRango
                       where d.idLicitacion == idLicitacion
                       select new EstimacionInnerLicitacionViewModel
                       {
                           NoLicitacion = d.noLicitacion,
                           Localidad = d.localidad,
                           NombreObra = d.nombreObra,
                           IdRequisicion = f.idRequisicionRango.ToString(),
                           IdMunicipio = e.descripcion.ToString(),
                           Cantidad = f.cantidad.ToString(),
                           Total = f.total.ToString(),
                           IdLicitacion = d.idLicitacion.ToString(),
                       }).ToList();
                foreach (var Elemento in lst)
                {
                    idRequisicion = Convert.ToInt32(Elemento.IdRequisicion);
                }
                int indice = 0;
                lstEstimacion = (from b in db.estimaciones.Where(x => x.idRequisicion == idRequisicion)
                                 select new EstimacionViewModel
                                 {
                                     IdEstimacion = b.idEstimacion.ToString(),
                                     IdConcepto = b.idConcepto.ToString(),
                                     IdRequisicion = b.idRequisicion.ToString(),
                                     Cantidad = b.cantidad.ToString(),
                                     NoEstimacion = b.noEstimacion.ToString()
                                 }).ToList();
                foreach (var item in lstEstimacion)
                {
                    if (indice != Convert.ToInt32(item.NoEstimacion))
                    {
                        lstEstimacion2.Add(new EstimacionViewModel
                        {
                            IdEstimacion = item.IdEstimacion,
                            IdConcepto = item.IdConcepto,
                            IdRequisicion = item.IdRequisicion,
                            Cantidad = item.Cantidad,
                            NoEstimacion = item.NoEstimacion
                        });
                        indice = Convert.ToInt32(item.NoEstimacion);
                    }
                }
                ViewBag.estimaciones = lstEstimacion2;
                return View(lst);

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult getEstimacion(FormCollection fc)
        {
            List<int> prueba1 = new List<int>();
            prueba1.Add(1);
            prueba1.Add(2);
            int[] prueba = new int[2];
            prueba[0] = 23;
            prueba[1] = 24;
            int idEstimacion = Convert.ToInt32(fc["est"]);
            int idRequisicion = Convert.ToInt32(fc["idReq"]);
            string descripcion = "";
            int indice = 0;
            int estima = 0;
            List<ArchivoFotoConceptoViewModel> lstEst = new List<ArchivoFotoConceptoViewModel>();
            List<ArchivoFotoConceptoViewModel> lstEst2 = new List<ArchivoFotoConceptoViewModel>();
            using (var db = new sgopEntities())
            {
                try
                {
                    lstEst = (from e in db.archivosFotograficos
                              join c in db.catalogoConceptos on e.idConcepto equals c.idConcepto
                              where e.noEstimacion == idEstimacion && e.idRequisicion == idRequisicion
                              orderby e.idConcepto
                              select new ArchivoFotoConceptoViewModel
                              {
                                  IdArchFoto = e.idArchFoto.ToString(),
                                  NoEstimacion = e.noEstimacion.ToString(),
                                  IdRequisicion = e.idRequisicion.ToString(),
                                  IdConcepto = e.idConcepto.ToString(),
                                  Foto = e.foto.ToString(),
                                  Descripcion = c.descripcion
                              }).ToList();
                    int ind1 = 0;
                    int conce = 0;
                    int boton = lstEst.Count();
                    if (boton != 0)
                    {


                        foreach (var elemento in lstEst)
                        {
                            if (indice == Convert.ToInt32(elemento.NoEstimacion))
                            {
                                if (conce != Convert.ToInt32(elemento.IdConcepto))
                                {
                                    descripcion = descripcion + "<div name='fila" + estima + "' id='" + estima + "'></div>"
                                                              + "<input type='file' class='filein' name='archivo" + estima + "' id='archivo" + estima + "' multiple  />"
                                                              + "</div>"
                                                              + "</div>"
                                                              + "<div class='row small'>"
                                                              + "<div class='col-12'>"
                                                              + "<h5 class='text-muted'>" + elemento.Descripcion + "</h5>"
                                                              + "</div>"
                                                              + "</div>"
                                                              + "<div class='row' id='cuerpoTabla'>"
                                                              + "<div class='col-12'>"
                                                              + "<img src='/Res/" + elemento.Foto + "' alt='" + elemento.Foto + "' height='50'>";
                                    estima++;
                                }
                                else
                                {
                                    descripcion = descripcion + "<img src='/Res/" + elemento.Foto + "' alt='" + elemento.Foto + "' height='50'>";
                                }
                                indice = Convert.ToInt32(elemento.NoEstimacion);
                                conce = Convert.ToInt32(elemento.IdConcepto);
                            }
                            else
                            {
                                indice = Convert.ToInt32(elemento.NoEstimacion);
                                conce = Convert.ToInt32(elemento.IdConcepto);
                                if (ind1 == 0)
                                {
                                    descripcion = descripcion + "<div class='row small'>"
                                                              + "<div class='col-12'>"
                                                              + "<h5 class='text-muted'>" + elemento.Descripcion + "</h5>"
                                                              + "</div>"
                                                              + "</div>"
                                                              + "<div class='row' id='cuerpoTabla'>"
                                                              + "<div class='col-12'>"
                                                              + "<img src='/Res/" + elemento.Foto + "' alt='" + elemento.Foto + "' height='50'>";
                                    ind1 = 1;
                                }
                                else
                                {
                                    descripcion = descripcion + "<div name='fila" + estima + "' id='" + estima + "'></div>"
                                                              + "<input type='file' class='filein' name='archivo" + estima + "' id='archivo" + estima + "' multiple />"
                                                              + "</div>"
                                                              + "</div>"
                                                              + "<div class='row small'>"
                                                              + "<div class='col-12'>"
                                                              + "<h5 class='text-muted'>" + elemento.Descripcion + "</h5>"
                                                              + "</div>"
                                                              + "</div>"
                                                              + "<div class='row' id='cuerpoTabla'>"
                                                              + "<div class='col-12'>"
                                                              + "<img src='/Res/" + elemento.Foto + "' alt='" + elemento.Foto + "' height='50'>";
                                    estima++;
                                }
                            }
                        }

                        //descripcion = descripcion + "<div name='fila" + estima + "' id='" + estima + "'></div>"
                        //                          + "<button type = 'button' onclick = 'abrirArch(" + estima + ")' class='btn btn-outline-info btn-sm'><span class='fas fa-plus mx-1'></span></button>"
                        //                                      + "</div>"
                        //                                      + "</div>";
                        descripcion = descripcion + "<input id='files' name='files' class='filein' type='file' multiple/>";
                    }
                    else
                    {
                        lstEst2 = (from e in db.estimaciones
                                   join c in db.catalogoConceptos on e.idConcepto equals c.idConcepto
                                   where e.noEstimacion == idEstimacion && e.idRequisicion == idRequisicion
                                   orderby e.idConcepto
                                   select new ArchivoFotoConceptoViewModel
                                   {
                                       IdArchFoto = e.idEstimacion.ToString(),
                                       Descripcion = c.descripcion.ToString(),
                                       IdRequisicion = e.idRequisicion.ToString(),
                                       IdConcepto = c.idConcepto.ToString(),
                                       NoEstimacion = e.noEstimacion.ToString(),
                                       Foto = ""
                                   }).ToList();
                        estima = 0;
                        foreach (var item in lstEst2)
                        {
                            descripcion = descripcion + "<div class='row small'>"
                                                        + "<div class='col-12'>"
                                                        + "<h5 class='text-muted'>" + item.Descripcion + "</h5>"
                                                        + "</div>"
                                                        + "</div>"
                                                        + "<div class='row' id='cuerpoTabla'>"
                                                        + "<div name='fila" + estima + "' id='" + estima + "'></div>"
                                                        + "<input type='file' class='filein' name='archivo" + estima + "' id='archivo" + estima + "' multiple />"
                                                        + "</div>";
                            estima++;
                        }
                    }
                    return Json(new { lstEst, lstEst2 });
                }
                catch (Exception e)
                {

                    throw new Exception(e.Message);
                }
            }
        }

        [HttpPost]
        public ActionResult CargaDocs(IEnumerable<HttpPostedFileBase> files, int noEstimacion, int idRequisicion, int idConcepto)
        {
            Models.SubirDocViewModel subir = new Models.SubirDocViewModel();
            foreach (var item in files)
            {
                if (item.ContentLength > 0)
                {
                    string path = Server.MapPath("~/Res/");
                    path += item.FileName;
                    subir.Subir(path, item);
                    bool bande = subir.confirmacion;
                    using (sgopEntities db = new sgopEntities())
                    {
                        var oFoto = new archivosFotograficos();
                        oFoto.noEstimacion = noEstimacion;
                        oFoto.idRequisicion = idRequisicion;
                        oFoto.idConcepto = idConcepto;
                        oFoto.foto = item.FileName;

                        db.archivosFotograficos.Add(oFoto);
                        db.SaveChanges();

                    }
                }
            }
            return Content("");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarArch(FormCollection fc)
        {
            var nombre = Request.MapPath("~/Res/" + fc["name"]);
            try
            {
                using (sgopEntities db = new sgopEntities())
                {
                    archivosFotograficos arch = db.archivosFotograficos.Find(Convert.ToInt32(fc["num"]));
                    db.archivosFotograficos.Remove(arch);
                    db.SaveChanges();
                }
                System.IO.File.Delete(nombre);
                return Content("1");
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult getFoto(FormCollection fc)
        {
            List<ArchivoFotoViewModel> lstFoto = new List<ArchivoFotoViewModel>();
            int requisicion = Convert.ToInt32(fc["idreq"]);
            int estimacion = Convert.ToInt32(fc["est"]);
            using (var db = new sgopEntities())
            {
                try
                {
                    lstFoto = (from f in db.archivosFotograficos
                               where f.idRequisicion == requisicion && f.noEstimacion == estimacion
                               select new ArchivoFotoViewModel
                               {
                                   IdRequisicion = f.idRequisicion.ToString(),
                                   IdArchivo = f.idArchFoto.ToString(),
                                   IdConcepto = f.idConcepto.ToString(),
                                   NoEstimacion = f.noEstimacion.ToString(),
                                   Foto = f.foto.ToString()
                               }).ToList();

                    return Json(lstFoto);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }
    }
}