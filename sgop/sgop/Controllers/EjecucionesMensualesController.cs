using sgop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sgop.Controllers.Requisiciones
{
    public class EjecucionesMensualesController : Controller
    {
        // GET: EjecucionesMensuales
        public ActionResult Index()
        {
            using (var db = new sgopEntities())
            {
                try
                {
                    int idLicitacion = Convert.ToInt32(100002);
                    //Del idLicitacion se obtiene el idRequisicion
                    var requisicion = db.licitaciones.Where(req => req.idLicitacion == idLicitacion).First();
                    int idRequisicion = (int)requisicion.idRequisicion;
                    //Del idLicitacion se obtiene el mes del contrato y el año del contrato
                    var proyecto = db.proyectos.Where(proy => proy.idLicitacion == idLicitacion).First();
                    ViewBag.mesContrato = proyecto.fechaInicioContrato.Value.Month;
                    ViewBag.anioContrato = proyecto.fechaInicioContrato.Value.Year;

                    List<LicitacionesRequisicionesViewModel> lstConceptosEnRequisicion = new List<LicitacionesRequisicionesViewModel>();
                    List<EjecucionesMensualesViewModel> lstEjecuciones = new List<EjecucionesMensualesViewModel>();
                    LicitacionesRequisicionesViewModel model = new LicitacionesRequisicionesViewModel();
                    MethodEncrypt me = new MethodEncrypt();

                    //Obtiene la lista de conceptos que hay en esa requisicion
                    ViewBag.lstConceptosEnRequisicion = (from req in db.requisiciones
                                                         join conc in db.catalogoConceptos on req.idConcepto equals conc.idConcepto
                                                         where req.idRequisicionRango == idRequisicion
                                                         select new LicitacionesRequisicionesViewModel
                                                         {
                                                             idRequisicionRango = req.idRequisicionRango,
                                                             idConcepto = req.idConcepto,
                                                             cantidad = req.cantidad,
                                                             total = req.total,
                                                             descripcion = conc.descripcion,
                                                             clave = conc.clave,
                                                             unidad = conc.unidad,
                                                             precioUnitario = conc.precioUnitario
                                                         }).ToList();

                    //sirve para saber cuantas columnas se van a agregar a la tabla
                    var lstEjecuciones2 = (from em in db.ejecucionesMensuales
                                           group em by em.idConcepto into g
                                           select new EjecucionesMensualesViewModel
                                           {
                                                idConcepto = g.Key,
                                                cantidad = 0
                                           }).ToList();

                    //Obtiene la lista de ejecuciones mensuales que hay con esa requisicion
                    lstEjecuciones = (from eM in db.ejecucionesMensuales
                                      join cc in db.catalogoConceptos on eM.idConcepto equals cc.idConcepto
                                      where eM.idRequisicion == idRequisicion
                                      select new EjecucionesMensualesViewModel
                                      {
                                          idEjecucion = eM.idEjecucion,
                                          periodo = eM.periodo,
                                          idRequisicion = eM.idRequisicion,
                                          idConcepto = eM.idConcepto,
                                          cantidad = eM.cantidad,
                                          clave = cc.clave,
                                          periodoMes = 0,
                                          periodoAnio = 0
                                      }).ToList();

                    //sirve para saber cuantas columnas se van a agregar a la tabla
                    foreach (var item in lstEjecuciones2)
                    {
                        foreach (var item2 in lstEjecuciones)
                        {
                            if (item.idConcepto == item2.idConcepto)
                            {
                                item.cantidad += 1;
                            }
                        }
                    }

                    //Descompone el periodo en mes y año
                    foreach (var item in lstEjecuciones)
                    {
                        item.periodoMes = Convert.ToInt32(item.periodo.Substring(0, 2));
                        item.periodoAnio = Convert.ToInt32(item.periodo.Substring(3, 2));
                    }

                    ViewBag.lstEjecuciones = lstEjecuciones;
                    ViewBag.cantColumnas = lstEjecuciones2.Max(e => e.cantidad);//Cantidad de columnas a agregar

                    var licitacion = db.licitaciones.Where(lic => lic.idLicitacion == idLicitacion).First();
                    var municipio = db.catalogoMunicipios.Where(mun => mun.idMunicipio == licitacion.idMunicipio).First();
                    var empresa = db.catalogoEmpresas.Where(emp => emp.idEmpresa == licitacion.idEmpresa).First();

                    model.idLicitacion = licitacion.idLicitacion;
                    model.idLicitacionEncrypt = me.getEncrypt(licitacion.idLicitacion.ToString());
                    model.idRequisicionRango = licitacion.idRequisicion;
                    model.idEmpresa = licitacion.idEmpresa;
                    model.razonSocial = empresa.razonSocial;
                    model.noLicitacion = licitacion.noLicitacion;
                    model.nombreObra = licitacion.nombreObra;
                    model.idMunicipio = licitacion.idMunicipio;
                    model.nombreMunicipio = municipio.descripcion;
                    model.localidad = licitacion.localidad;
                    model.fechaVisita = licitacion.fechaVisita;
                    model.fechaAclaraciones = licitacion.fechaAclaraciones;
                    model.fechaPropuesta = licitacion.fechaPropuesta;
                    model.fechaFallo = licitacion.fechaFallo;
                    model.actaVisita = licitacion.actaVisita;
                    model.actaAclaraciones = licitacion.actaAclaraciones;
                    model.actaPropuesta = licitacion.actaPropuesta;
                    model.actaFallo = licitacion.actaFallo;
                    model.idEstatus = licitacion.idEstatus;
                    model.idRequisicion = licitacion.idRequisicion;
                    model.fechaCreacion = licitacion.fechaCreacion;
                    model.usuarioCreacion = licitacion.usuarioCreacion;
                    model.fechaModificacion = licitacion.fechaModificacion;
                    model.usuarioModificacion = licitacion.usuarioModificacion;

                    db.Dispose();
                    return View(model);
                }
                catch (Exception)
                {
                    db.Dispose();
                    return Content("noExisteLicitacion");
                }
            }
        }

        public ActionResult getNameMonth(FormCollection fc)
        {
            int mes = Convert.ToInt32(fc["mes"]);
            if (mes == 1)
                return Content("Enero");
            else if (mes == 2)
                return Content("Febrero");
            else if (mes == 3)
                return Content("Marzo");
            else if (mes == 4)
                return Content("Abril");
            else if (mes == 5)
                return Content("Mayo");
            else if (mes == 6)
                return Content("Junio");
            else if (mes == 7)
                return Content("Julio");
            else if (mes == 8)
                return Content("Agosto");
            else if (mes == 9)
                return Content("Septiembre");
            else if (mes == 10)
                return Content("Octubre");
            else if (mes == 11)
                return Content("Noviembre");
            else
                return Content("Diciembre");
        }
    }
}