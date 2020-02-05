using sgop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sgop.Controllers
{

    public class EstimacionController : Controller
    {

        //SE OBTIENE LA INFORMACION DE LA LICITACION Y SE COLOCA EN LA LISTA
        public ActionResult Estimacion(FormCollection fc)
        {
            int idLicitacion = Convert.ToInt32(fc["idLicitacion"]);
            int idRequisicion = 0;
            int RequisicionRango = 0;
            List<EstimacionInnerLicitacionViewModel> lst;
            List<ConceptoViewModel> lstConceptos;
            List<EstimacionViewModel> lstEstimacion;
            List<EstimacionViewModel> lstEstimacion2 = new List<Models.EstimacionViewModel>();
            using (sgopEntities db = new sgopEntities())
            {

                lst = (from d in db.licitaciones
                       join e in db.catalogoMunicipios on d.idMunicipio equals e.idMunicipio
                       join f in db.requisiciones on d.idRequisicion equals f.idRequisicionRango
                       join p in db.proyectos on d.idLicitacion equals p.idLicitacion
                       where d.idLicitacion == idLicitacion
                       select new Models.EstimacionInnerLicitacionViewModel
                       {
                           NoLicitacion = d.noLicitacion,
                           Localidad = d.localidad,
                           NombreObra = d.nombreObra,
                           IdRequisicion = f.idRequisicion.ToString(),
                           RequisicionRango = f.idRequisicionRango.ToString(),
                           IdMunicipio = e.descripcion.ToString(),
                           Cantidad = f.cantidad.ToString(),
                           Total = f.total.ToString(),
                           IdLicitacion = d.idLicitacion.ToString(),
                           IdProyecto = p.idProyecto.ToString()
                       }).ToList();

                RequisicionRango = Convert.ToInt32(lst.First().RequisicionRango);
                idRequisicion = Convert.ToInt32(lst.First().IdRequisicion);
                lstConceptos = (from a in db.catalogoConceptos
                                join r in db.requisiciones on a.idConcepto equals r.idConcepto
                                where r.idRequisicion == idRequisicion
                                select new Models.ConceptoViewModel
                                {
                                    IdConcepto = a.idConcepto,
                                    Clave = a.clave,
                                    Descripcion = a.descripcion,
                                    Unidad = a.unidad,
                                    PrecioUnitario = a.precioUnitario.ToString()
                                }).ToList();
                int indice = 0;
                lstEstimacion = (from b in db.estimaciones.Where(x => x.idRequisicion == RequisicionRango)
                                 select new Models.EstimacionViewModel
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
                        lstEstimacion2.Add(new Models.EstimacionViewModel
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
                ViewBag.conceptos = lstConceptos;
                return View(lst);

            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Conceptos(FormCollection fc)
        {
            int cant = Convert.ToInt32(fc["totalfi"]);
            int idReq = Convert.ToInt32(fc["idReq"]);

            List<Models.ConceptoViewModel> lstC;
            using (sgopEntities db = new sgopEntities())
            {
                lstC = (from d in db.catalogoConceptos
                        join r in db.requisiciones on d.idConcepto equals r.idConcepto
                        where r.idRequisicionRango == idReq
                        select new Models.ConceptoViewModel
                        {
                            IdConcepto = d.idConcepto,
                            Clave = d.clave,
                            Descripcion = d.descripcion,
                            Unidad = d.unidad
                        }).ToList();
            }
            string opc = "opt";
            string cadena = "<option>Selecciona concepto</option>";
            if (cant != lstC.Count())
            {
                foreach (var Elemento in lstC)
                {
                    if (cant == 0)
                    {
                        cadena = cadena + "<option value='" + Elemento.IdConcepto + "'>" + Elemento.Clave + "</option>";
                    }
                    else
                    {
                        for (int i = 0; i < cant; i++)
                        {
                            opc = opc + i.ToString();
                            if (Elemento.IdConcepto == Convert.ToInt32(fc[opc]))
                            {
                            }
                            else
                            {
                                cadena = cadena + "<option value='" + Elemento.IdConcepto + "'>" + Elemento.Clave + "</option>";
                            }

                            opc = "opt";
                        }
                    }
                }
            }

            return Content(cadena);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar(FormCollection fc)
        {
            int filas = Convert.ToInt32(fc["totalfi"]);
            string clave = "opt";
            string estimacion = "est";
            try
            {

                using (sgopEntities db = new sgopEntities())
                {
                    for (int i = 0; i < filas; i++)
                    {
                        clave = clave + i.ToString();
                        estimacion = estimacion + i.ToString();
                        var oEstimacion = new estimaciones();
                        oEstimacion.idConcepto = Convert.ToInt32(fc[clave]);
                        oEstimacion.idRequisicion = Convert.ToInt32(fc["idReq"]);
                        oEstimacion.noEstimacion = Convert.ToInt32(fc["estMax"]) + 1;
                        oEstimacion.cantidad = Convert.ToInt32(fc[estimacion]);

                        db.estimaciones.Add(oEstimacion);
                        db.SaveChanges();
                        clave = "opt";
                        estimacion = "est";
                    }

                }

                return Content("1");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult getDescripcion(FormCollection fc)
        {
            int idProducto = Convert.ToInt32(fc["conc"]);
            string descripcion = "";
            List<Models.ConceptoViewModel> lstCon;
            using (var db = new sgopEntities())
            {
                try
                {
                    lstCon = (from a in db.catalogoConceptos.Where(id => id.idConcepto == idProducto)
                              select new Models.ConceptoViewModel
                              {
                                  IdConcepto = a.idConcepto,
                                  Clave = a.clave,
                                  Descripcion = a.descripcion,
                                  Unidad = a.unidad,
                                  PrecioUnitario = a.precioUnitario.ToString()
                              }).ToList();
                    foreach (var des in lstCon)
                    {
                        descripcion = des.Descripcion;
                    }
                    return Content(descripcion);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult getEstimacion(FormCollection fc)
        {
            int idEstimacion = Convert.ToInt32(fc["est"]);
            int idReq = Convert.ToInt32(fc["idReq"]);
            int idRequisicion = Convert.ToInt32(fc["idRequisicion"]);
            string descripcion = "";
            List<Models.EstimacionConceptoViewModel> lstEst;
            List<Models.EstimacionConceptoViewModel> lstEst2;
            using (var db = new sgopEntities())
            {
                try
                {
                    lstEst = (from e in db.estimaciones
                              join c in db.catalogoConceptos on e.idConcepto equals c.idConcepto
                              join r2 in db.requisiciones on e.idRequisicion equals r2.idRequisicionRango
                              where e.noEstimacion == idEstimacion && e.idRequisicion == idReq && r2.idRequisicion == idRequisicion

                              select new Models.EstimacionConceptoViewModel
                              {
                                  IdEstimacion = e.idEstimacion.ToString(),
                                  IdConcepto = c.clave.ToString(),
                                  Cantidad = r2.cantidad.ToString(),
                                  Descripcion = c.descripcion.ToString(),
                                  Estimacion = e.cantidad.ToString(),
                                  Total = r2.total.ToString(),
                                  PrecioUnitario = c.precioUnitario.ToString()
                              }).ToList();
                    lstEst2 = (from e in db.estimaciones
                               join c in db.catalogoConceptos on e.idConcepto equals c.idConcepto
                               join r in db.requisiciones on e.idRequisicion equals r.idRequisicionRango
                               where e.noEstimacion <= idEstimacion && e.idRequisicion == idReq

                               select new Models.EstimacionConceptoViewModel
                               {
                                   IdEstimacion = e.idEstimacion.ToString(),
                                   IdConcepto = c.clave.ToString(),
                                   Cantidad = e.cantidad.ToString(),
                                   Descripcion = c.descripcion.ToString(),
                                   Estimacion = e.cantidad.ToString(),
                                   Total = r.total.ToString(),
                                   PrecioUnitario = c.precioUnitario.ToString()
                               }).ToList();
                    int contador = 0;
                    int acumulado = 0;
                    int z = lstEst2.Count() - 2;
                    for (int i = 0; i < idEstimacion - 1; i++)
                    {
                        acumulado = acumulado + Convert.ToInt32(lstEst2[i].PrecioUnitario) * Convert.ToInt32(lstEst2[i].Cantidad);
                    }

                    foreach (var des in lstEst)
                    {
                        descripcion = descripcion
                            + "<input name='EdidEstimacion" + contador + "' id='EdidEstimacion" + contador + "' value='" + des.IdEstimacion + "' hidden/>"
                            + "<div class='col-2 text-muted font-weight-bolder text-center'>"
                            + des.IdConcepto
                            + "</div>"
                            + "<div class='col-3 text-muted font-weight-bolder'>"
                            + des.Descripcion
                            + "</div>"
                            + "<div class='col-1 text-muted font-weight-bolder text-center'>"
                            + des.Cantidad
                            + "</div>"
                            + "<div class='col-1 text-muted font-weight-bolder text-center'>"
                            + des.Total
                            + "</div>"
                            + "<div class='col-2 text-muted font-weight-bolder text-center'>"
                            + "<input type='text' class='form-control form-control-sm w-50 mx-auto text-center' id='est" + contador + "' name='est" + contador + "' onkeyup='mostrar(" + contador + ")' value='" + des.Estimacion + "'/>"
                            + "</div>"
                            + "<div class='col-1 text-muted font-weight-bolder text-center'>"
                            + "<label name='precio" + contador + "' id='precio" + contador + "'>" + Convert.ToInt32(des.Estimacion) * Convert.ToInt32(des.PrecioUnitario) + "</label>"
                            + "</div>"
                            + "<input type='text' id='unitario" + contador + "' name='unitario" + contador + "' value='" + des.PrecioUnitario + "' hidden/>"
                            + "<div class='col-2 text-muted font-weight-bolder text-center'>"
                            + acumulado
                            + "</div>";
                        contador++;
                    }
                    descripcion = "<input type='text' value='" + contador + "' hidden/>" + descripcion;
                    return Content(descripcion);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(FormCollection fc)
        {
            int filas = Convert.ToInt32(fc["Edtotalfi"]);
            string idEst = "EdidEstimacion";
            string cantidad = "est";
            int id = 0;
            try
            {
                using (sgopEntities db = new sgopEntities())
                {
                    for (int i = 0; i < filas; i++)
                    {
                        idEst = idEst + i.ToString();
                        cantidad = cantidad + i.ToString();
                        id = Convert.ToInt32(fc[idEst]);
                        var oTabla = db.estimaciones.Find(Convert.ToInt32(fc[idEst]));
                        oTabla.cantidad = Convert.ToDouble(fc[cantidad]);

                        db.Entry(oTabla).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        idEst = "EdidEstimacion";
                        cantidad = "est";
                    }
                }
                return Content("1");
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

    }
}