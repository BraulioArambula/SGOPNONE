using sgop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sgop.Controllers
{
    public class AccountingController : Controller
    {
        // GET: Accounting
        public ActionResult Index()
        {
            Session["NombreUsuario"] = "Angel Piñon";
            return View();
        }

        private List<controlPagosViewModel> consultaDocumentosPagos(
            sgopEntities conexion, int idRequisicion, int idRequisicionParcial)
        {
            List<controlPagosViewModel> lstControlPagos = new List<controlPagosViewModel>();
            using (conexion)
            {
                try
                {
                    if (idRequisicionParcial == 0)
                    {
                        lstControlPagos = (from ca in conexion.controlPagos
                                           join cb in conexion.requisicionesParciales
                                           on ca.idRequisicionParcial equals cb.idRequisicionParcial
                                           join cc in conexion.catalogoMateriales
                                           on cb.idMaterial equals cc.idMaterial
                                           join cd in conexion.catalogoConceptos
                                           on cb.idConcepto equals cd.idConcepto
                                           where ca.idRequisicion == idRequisicion
                                           orderby ca.idRequisicionParcial
                                           select new controlPagosViewModel
                                           {
                                               noDocumento = ca.noDocumento,
                                               descripcion = cc.descripcion,
                                               cantidad = cb.cantidad,
                                               idRequisicion = ca.idRequisicion,
                                               idRequisicionParcial = ca.idRequisicionParcial,
                                               clDocumento = ca.clDocumento,
                                               importe = ca.importe,
                                               fechaDocumento = ca.fechaDocumento,
                                               docCompensacion = ca.docCompensacion,
                                               fechaCompensacion = ca.fechaCompensacion,
                                               factura = ca.factura,
                                               usuarioCreacion = ca.usuarioCreacion,
                                               usuarioCompensacion = ca.usuarioCompensacion,
                                               idConcepto = cd.idConcepto,
                                               clave = cd.clave,
                                               descripcionConcepto = cd.descripcion
                                           }).ToList();
                    }
                    else
                    {
                        lstControlPagos = (from ca in conexion.controlPagos
                                           join cb in conexion.requisicionesParciales
                                           on ca.idRequisicionParcial equals cb.idRequisicionParcial
                                           join cc in conexion.catalogoMateriales
                                           on cb.idMaterial equals cc.idMaterial
                                           join cd in conexion.catalogoConceptos
                                           on cb.idConcepto equals cd.idConcepto
                                           where ca.idRequisicion == idRequisicion
                                           && cb.noRequisicion == idRequisicionParcial
                                           orderby ca.idRequisicionParcial
                                           select new controlPagosViewModel
                                           {
                                               noDocumento = ca.noDocumento,
                                               descripcion = cc.descripcion,
                                               cantidad = cb.cantidad,
                                               idRequisicion = ca.idRequisicion,
                                               idRequisicionParcial = ca.idRequisicionParcial,
                                               clDocumento = ca.clDocumento,
                                               importe = ca.importe,
                                               fechaDocumento = ca.fechaDocumento,
                                               docCompensacion = ca.docCompensacion,
                                               fechaCompensacion = ca.fechaCompensacion,
                                               factura = ca.factura,
                                               usuarioCreacion = ca.usuarioCreacion,
                                               usuarioCompensacion = ca.usuarioCompensacion,
                                               idConcepto = cd.idConcepto,
                                               clave = cd.clave,
                                               descripcionConcepto = cd.descripcion
                                           }).ToList();
                    }
                }
                catch (Exception ex)
                {
                    return lstControlPagos;
                }
            }
            return lstControlPagos;
        }

        [HttpPost]
        public ActionResult controlPagos(FormCollection formularioBusqueda)
        {

            licitacionesViewModel licitaciondb = new licitacionesViewModel();
            int idLicitacion = Convert.ToInt32(formularioBusqueda["idLicitacion"].ToString());
            int idRequisicionParcial;
            List<controlPagosViewModel> lstControlPagos = new List<controlPagosViewModel>();

            if (formularioBusqueda["idRequisicionParcial"] != null)
            {
                idRequisicionParcial = Convert.ToInt32(formularioBusqueda["idRequisicionParcial"].ToString());
            }
            else
            {
                idRequisicionParcial = 0;

            }

            using (var conexion = new sgopEntities())
            {
                try
                {
                    licitaciones consultaLicitacionesDB = conexion.licitaciones.Where(auxLicitaciones => auxLicitaciones.idLicitacion == idLicitacion).First();
                    licitaciondb.idLicitacion = consultaLicitacionesDB.idLicitacion;
                    licitaciondb.idRequisicion = consultaLicitacionesDB.idRequisicion;
                    licitaciondb.nombreObra = consultaLicitacionesDB.nombreObra;
                    licitaciondb.localidad = consultaLicitacionesDB.localidad;
                    licitaciondb.noLicitacion = consultaLicitacionesDB.noLicitacion;

                    catalogoMunicipios consultaMunicipiosDB = conexion.catalogoMunicipios.Where(
                        auxMunicipios => auxMunicipios.idMunicipio == consultaLicitacionesDB.idMunicipio).First();
                    licitaciondb.municipio = consultaMunicipiosDB.descripcion;

                    licitaciondb.lstRequisicionesParciales = (from ca in conexion.requisicionesParciales
                                                              join cb in conexion.controlPagos
                                                              on ca.idRequisicionParcial equals cb.idRequisicionParcial
                                                              where ca.idRequisicion == licitaciondb.idRequisicion
                                                              && cb.idRequisicion == licitaciondb.idRequisicion
                                                              select ca.noRequisicion
                                                              ).Distinct().ToList();

                    lstControlPagos = consultaDocumentosPagos(
                        conexion, Convert.ToInt32(licitaciondb.idRequisicion), idRequisicionParcial);

                    ViewBag.lstControlPagos = lstControlPagos;
                    conexion.Dispose();
                    return View(licitaciondb);
                }
                catch (Exception ex)
                {
                    return Content("Error al consultar la base de datos");
                }
            }
        }

        [HttpPost]
        public ActionResult controlPagosString(FormCollection formularioBusqueda)
        {

            licitacionesViewModel licitaciondb = new licitacionesViewModel();
            int idLicitacion = Convert.ToInt32(formularioBusqueda["idLicitacion"].ToString());
            int idRequisicionParcial;
            List<controlPagosViewModel> lstControlPagos = new List<controlPagosViewModel>();

            if (formularioBusqueda["idRequisicionParcial"] != null)
            {
                idRequisicionParcial = Convert.ToInt32(formularioBusqueda["idRequisicionParcial"].ToString());
            }
            else
            {
                idRequisicionParcial = 0;

            }

            using (var conexion = new sgopEntities())
            {
                try
                {
                    licitaciones consultaLicitacionesDB = conexion.licitaciones.Where(auxLicitaciones => auxLicitaciones.idLicitacion == idLicitacion).First();
                    licitaciondb.idLicitacion = consultaLicitacionesDB.idLicitacion;
                    licitaciondb.idRequisicion = consultaLicitacionesDB.idRequisicion;
                    licitaciondb.nombreObra = consultaLicitacionesDB.nombreObra;
                    licitaciondb.localidad = consultaLicitacionesDB.localidad;
                    licitaciondb.noLicitacion = consultaLicitacionesDB.noLicitacion;

                    catalogoMunicipios consultaMunicipiosDB = conexion.catalogoMunicipios.Where(
                        auxMunicipios => auxMunicipios.idMunicipio == consultaLicitacionesDB.idMunicipio).First();
                    licitaciondb.municipio = consultaMunicipiosDB.descripcion;

                    licitaciondb.lstRequisicionesParciales = (from ca in conexion.requisicionesParciales
                                                              join cb in conexion.controlPagos
                                                              on ca.idRequisicionParcial equals cb.idRequisicionParcial
                                                              where ca.idRequisicion == licitaciondb.idRequisicion
                                                              && cb.idRequisicion == licitaciondb.idRequisicion
                                                              select ca.noRequisicion
                                                              ).Distinct().ToList();

                    lstControlPagos = consultaDocumentosPagos(
                        conexion, Convert.ToInt32(licitaciondb.idRequisicion), idRequisicionParcial);

                    int contador = 0;
                    int auxIdConcepto = 0;
                    DateTime auxiliarFechas;
                    string cadenaRegreso = "";
                    foreach (var registro in lstControlPagos)
                    {
                        if (registro.idConcepto != auxIdConcepto)
                        {
                            cadenaRegreso = cadenaRegreso +
                                "<tr class='text-primary border border-right-0 border-primary font-weight-bold'>";
                            cadenaRegreso = cadenaRegreso +
                                "<td class='text-center align-middle' colspan='1'>Concepto</td>";
                            cadenaRegreso = cadenaRegreso +
                                "<td class='text-center align-middle' colspan='1'>" + registro.clave + "</td>";
                            cadenaRegreso = cadenaRegreso +
                                "<td class='text-left align-middle' colspan='8'>" + registro.descripcionConcepto + "</td></tr>";
                            auxIdConcepto = registro.idConcepto;
                        }
                        contador = contador + 1;

                        cadenaRegreso = cadenaRegreso +
                            "<tr>";
                        if (registro.docCompensacion != null)
                        {
                            cadenaRegreso = cadenaRegreso +
                                "<td class='text-muted text-center small align-middle' onclick='abrirModalCompensado();'>" + registro.noDocumento + "</td>";
                        }
                        else
                        {
                            cadenaRegreso = cadenaRegreso +
                                "<td class='text-muted text-center small align-middle' onclick='abrirModalCompensar(" + registro.noDocumento + ");'>" + registro.noDocumento + "</td>";
                        }
                        cadenaRegreso = cadenaRegreso +
                            "<td class='text-muted text-left small align-middle'>" + registro.descripcion + "</td>";
                        cadenaRegreso = cadenaRegreso +
                            "<td class='text-muted text-center small align-middle'>" + registro.cantidad + "</td>";
                        cadenaRegreso = cadenaRegreso +
                            "<td class='text-muted text-center small align-middle'>" + registro.importe + "</td>";
                        cadenaRegreso = cadenaRegreso +
                            "<td class='text-muted text-center small align-middle'>" + registro.clDocumento + "</td>";
                        if (registro.fechaDocumento != null)
                        {
                            auxiliarFechas = (DateTime)registro.fechaDocumento;
                            cadenaRegreso = cadenaRegreso +
                                "<td class='text-muted text-center small align-middle'>" + auxiliarFechas.ToString("MM/dd/yyyy") + "</td>";
                        }
                        else
                        {
                            cadenaRegreso = cadenaRegreso +
                                "<td class='text-muted text-center small align-middle'>" + registro.fechaDocumento + "</td>";
                        }

                        cadenaRegreso = cadenaRegreso +
                            "<td class='text-muted text-center small align-middle'>" + registro.docCompensacion + "</td>";

                        if (registro.fechaCompensacion != null)
                        {
                            auxiliarFechas = (DateTime)registro.fechaCompensacion;
                            cadenaRegreso = cadenaRegreso +
                                "<td class='text-muted text-center small align-middle'>" + auxiliarFechas.ToString("MM/dd/yyyy") + "</td>";
                        }
                        else
                        {
                            cadenaRegreso = cadenaRegreso +
                                "<td class='text-muted text-center small align-middle'>" + registro.fechaCompensacion + "</td>";
                        }
                        cadenaRegreso = cadenaRegreso +
                            "<td class='text-muted text-center small align-middle'><div class='custom-control custom-checkbox'>";
                        if (registro.factura != null)
                        {
                            cadenaRegreso = cadenaRegreso +
                            "<input type = 'checkbox' class='custom-control-input' id='" + contador + "' name='" + contador + "' checked disabled/>";
                        }
                        else
                        {
                            cadenaRegreso = cadenaRegreso +
                            "<input type = 'checkbox' class='custom-control-input' id='" + contador + "' name='" + contador + "' disabled/>";
                        }

                        cadenaRegreso = cadenaRegreso + "<label class='custom-control-label' for='" + contador + "'></label></div></td>";
                        cadenaRegreso = cadenaRegreso + "<td class='text-muted text-center small'><a class='btn btn-sm text-sm-center text-primary' data-toggle='modal' data-target='#modalCargarFactura'>";
                        cadenaRegreso = cadenaRegreso + "<i class='far fa-file'></i></a></td></tr>";

                    }
                    conexion.Dispose();
                    return Content(cadenaRegreso);
                }
                catch (Exception ex)
                {
                    return Content("Error al consultar la base de datos");
                }
            }
        }

        [HttpPost]
        public ActionResult compensarPagos(FormCollection formularioBusqueda)
        {
            int noDocumento = Convert.ToInt32(formularioBusqueda["noDocumento"].ToString());
            controlPagos documentoCompensar = new controlPagos();
            using (var conexion = new sgopEntities())
            {
                try
                {
                    documentoCompensar = conexion.controlPagos.Where(
                        auxControlPagos => auxControlPagos.noDocumento == noDocumento).First();
                    Rangos rango = new Rangos();
                    controlPagos documentoCompensador = new controlPagos();
                    documentoCompensador.idRequisicionParcial = documentoCompensar.idRequisicionParcial;
                    documentoCompensador.noDocumento = rango.getSiguienteID("EGRESOS");
                    documentoCompensador.idRequisicion = documentoCompensar.idRequisicion;
                    documentoCompensador.clDocumento = "E";
                    documentoCompensador.importe = Convert.ToDouble(formularioBusqueda["importe"].ToString());
                    documentoCompensador.fechaDocumento = DateTime.Now;
                    documentoCompensador.docCompensacion = documentoCompensador.noDocumento;
                    documentoCompensador.fechaCompensacion = documentoCompensador.fechaDocumento;
                    documentoCompensador.usuarioCreacion = 1;
                    documentoCompensador.usuarioCompensacion = 1;

                    documentoCompensar.importe = documentoCompensador.importe * -1;
                    documentoCompensar.docCompensacion = documentoCompensador.noDocumento;
                    documentoCompensar.fechaCompensacion = documentoCompensador.fechaDocumento;
                    documentoCompensar.usuarioCompensacion = 1;

                    conexion.controlPagos.Add(documentoCompensador);
                    conexion.Entry(documentoCompensar).State = System.Data.Entity.EntityState.Modified;
                    conexion.SaveChanges();

                    return RedirectToAction("index");

                }
                catch (Exception ex)
                {
                    return Content("Error al consultar la base de datos 123");
                }
            }
        }

        private List<ControlCobrosViewModel> consultaDocumentosCobros(
            sgopEntities conexion, int idRequisicion, int noEstimacion)
        {
            List<ControlCobrosViewModel> lstControlCobros = new List<ControlCobrosViewModel>();
            using (conexion)
            {
                try
                {
                    if (noEstimacion == 0)
                    {
                        lstControlCobros = (from ca in conexion.controlCobros
                                            where ca.idRequisicion == idRequisicion
                                            select new ControlCobrosViewModel
                                            {
                                                noDocumento = ca.noDocumento,
                                                idRequisicion = ca.idRequisicion,
                                                noEstimacion = ca.noEstimacion,
                                                clDocumento = ca.clDocumento,
                                                importe = ca.importe,
                                                fechaDocumento = ca.fechaDocumento,
                                                docCompensacion = ca.docCompensacion,
                                                fechaCompensacion = ca.fechaCompensacion,
                                                factura = ca.factura,
                                                usuarioCreacion = ca.usuarioCreacion,
                                                usuarioCompensacion = ca.usuarioCompensacion
                                            }).ToList();
                    }
                    else
                    {
                        lstControlCobros = (from ca in conexion.controlCobros
                                            where ca.idRequisicion == idRequisicion
                                            && ca.noEstimacion == noEstimacion
                                            select new ControlCobrosViewModel
                                            {
                                                noDocumento = ca.noDocumento,
                                                idRequisicion = ca.idRequisicion,
                                                noEstimacion = ca.noEstimacion,
                                                clDocumento = ca.clDocumento,
                                                importe = ca.importe,
                                                fechaDocumento = ca.fechaDocumento,
                                                docCompensacion = ca.docCompensacion,
                                                fechaCompensacion = ca.fechaCompensacion,
                                                factura = ca.factura,
                                                usuarioCreacion = ca.usuarioCreacion,
                                                usuarioCompensacion = ca.usuarioCompensacion
                                            }).ToList();
                    }
                }
                catch (Exception ex)
                {
                    return lstControlCobros;
                }
            }
            return lstControlCobros;
        }

        [HttpPost]
        public ActionResult controlCobros(FormCollection formularioBusqueda)
        {

            licitacionesViewModel licitaciondb = new licitacionesViewModel();
            int idLicitacion = Convert.ToInt32(formularioBusqueda["idLicitacion"].ToString());
            int noEstimacion;
            List<ControlCobrosViewModel> lstControlCobros = new List<ControlCobrosViewModel>();

            if (formularioBusqueda["noEstimacion"] != null)
            {
                noEstimacion = Convert.ToInt32(formularioBusqueda["idRequisicionParcial"].ToString());
            }
            else
            {
                noEstimacion = 0;

            }

            using (var conexion = new sgopEntities())
            {
                try
                {
                    licitaciones consultaLicitacionesDB = conexion.licitaciones.Where(auxLicitaciones => auxLicitaciones.idLicitacion == idLicitacion).First();
                    licitaciondb.idLicitacion = consultaLicitacionesDB.idLicitacion;
                    licitaciondb.idRequisicion = consultaLicitacionesDB.idRequisicion;
                    licitaciondb.nombreObra = consultaLicitacionesDB.nombreObra;
                    licitaciondb.localidad = consultaLicitacionesDB.localidad;
                    licitaciondb.noLicitacion = consultaLicitacionesDB.noLicitacion;

                    catalogoMunicipios consultaMunicipiosDB = conexion.catalogoMunicipios.Where(
                        auxMunicipios => auxMunicipios.idMunicipio == consultaLicitacionesDB.idMunicipio).First();
                    licitaciondb.municipio = consultaMunicipiosDB.descripcion;

                    licitaciondb.lstEstimaciones = (from ca in conexion.controlCobros
                                                    where ca.idRequisicion == licitaciondb.idRequisicion
                                                    select ca.noEstimacion
                                                    ).Distinct().ToList();

                    lstControlCobros = consultaDocumentosCobros(
                        conexion, Convert.ToInt32(licitaciondb.idRequisicion), noEstimacion);

                    ViewBag.lstControlCobros = lstControlCobros;
                    conexion.Dispose();
                    return View(licitaciondb);
                }
                catch (Exception ex)
                {
                    return Content("Error al consultar la base de datos");
                }
            }
        }


        [HttpPost]
        public ActionResult controlCobrosString(FormCollection formularioBusqueda)
        {

            licitacionesViewModel licitaciondb = new licitacionesViewModel();
            int idLicitacion = Convert.ToInt32(formularioBusqueda["idLicitacion"].ToString());
            int noEstimacion;
            List<ControlCobrosViewModel> lstControlCobros = new List<ControlCobrosViewModel>();

            if (formularioBusqueda["noEstimacion"] != null)
            {
                noEstimacion = Convert.ToInt32(formularioBusqueda["noEstimacion"].ToString());
            }
            else
            {
                noEstimacion = 0;

            }

            using (var conexion = new sgopEntities())
            {
                try
                {
                    licitaciones consultaLicitacionesDB = conexion.licitaciones.Where(auxLicitaciones => auxLicitaciones.idLicitacion == idLicitacion).First();
                    licitaciondb.idLicitacion = consultaLicitacionesDB.idLicitacion;
                    licitaciondb.idRequisicion = consultaLicitacionesDB.idRequisicion;
                    licitaciondb.nombreObra = consultaLicitacionesDB.nombreObra;
                    licitaciondb.localidad = consultaLicitacionesDB.localidad;
                    licitaciondb.noLicitacion = consultaLicitacionesDB.noLicitacion;

                    catalogoMunicipios consultaMunicipiosDB = conexion.catalogoMunicipios.Where(
                        auxMunicipios => auxMunicipios.idMunicipio == consultaLicitacionesDB.idMunicipio).First();
                    licitaciondb.municipio = consultaMunicipiosDB.descripcion;

                    licitaciondb.lstEstimaciones = (from ca in conexion.controlCobros
                                                    where ca.idRequisicion == licitaciondb.idRequisicion
                                                    select ca.noEstimacion
                                                    ).Distinct().ToList();

                    lstControlCobros = consultaDocumentosCobros(
                        conexion, Convert.ToInt32(licitaciondb.idRequisicion), noEstimacion);

                    int contador = 0;
                    DateTime auxiliarFechas;
                    string cadenaRegreso = "";
                    foreach (var registro in lstControlCobros)
                    {
                        contador = contador + 1;
                        cadenaRegreso = cadenaRegreso +
                            "<tr>";
                        if (registro.docCompensacion != null)
                        {
                            cadenaRegreso = cadenaRegreso +
                                "<td class='text-muted text-center small align-middle' onclick='abrirModalCompensado();'>" + registro.noDocumento + "</td>";
                        }
                        else
                        {
                            cadenaRegreso = cadenaRegreso +
                                "<td class='text-muted text-center small align-middle' onclick='abrirModalCompensar(" + registro.noDocumento + ");'>" + registro.noDocumento + "</td>";
                        }
                        cadenaRegreso = cadenaRegreso +
                            "<td class='text-muted text-left small align-middle'> Estimacion No. " + registro.noEstimacion + "</td>";
                        cadenaRegreso = cadenaRegreso +
                            "<td class='text-muted text-center small align-middle'>" + registro.importe + "</td>";
                        cadenaRegreso = cadenaRegreso +
                            "<td class='text-muted text-center small align-middle'>" + registro.clDocumento + "</td>";
                        if (registro.fechaDocumento != null)
                        {
                            auxiliarFechas = (DateTime)registro.fechaDocumento;
                            cadenaRegreso = cadenaRegreso +
                                "<td class='text-muted text-center small align-middle'>" + auxiliarFechas.ToString("MM/dd/yyyy") + "</td>";
                        }
                        else
                        {
                            cadenaRegreso = cadenaRegreso +
                                "<td class='text-muted text-center small align-middle'>" + registro.fechaDocumento + "</td>";
                        }

                        cadenaRegreso = cadenaRegreso +
                            "<td class='text-muted text-center small align-middle'>" + registro.docCompensacion + "</td>";

                        if (registro.fechaCompensacion != null)
                        {
                            auxiliarFechas = (DateTime)registro.fechaCompensacion;
                            cadenaRegreso = cadenaRegreso +
                                "<td class='text-muted text-center small align-middle'>" + auxiliarFechas.ToString("MM/dd/yyyy") + "</td>";
                        }
                        else
                        {
                            cadenaRegreso = cadenaRegreso +
                                "<td class='text-muted text-center small align-middle'>" + registro.fechaCompensacion + "</td>";
                        }
                        cadenaRegreso = cadenaRegreso +
                            "<td class='text-muted text-center small align-middle'><div class='custom-control custom-checkbox'>";
                        if (registro.factura != null)
                        {
                            cadenaRegreso = cadenaRegreso +
                            "<input type = 'checkbox' class='custom-control-input' id='" + contador + "' name='" + contador + "' checked disabled/>";
                        }
                        else
                        {
                            cadenaRegreso = cadenaRegreso +
                            "<input type = 'checkbox' class='custom-control-input' id='" + contador + "' name='" + contador + "' disabled/>";
                        }

                        cadenaRegreso = cadenaRegreso + "<label class='custom-control-label' for='" + contador + "'></div></label></td>";
                        cadenaRegreso = cadenaRegreso + "<td class='text-muted text-center small'><a class='btn btn-sm text-sm-center text-primary' data-toggle='modal' data-target='#modalCargarFactura'>";
                        cadenaRegreso = cadenaRegreso + "<i class='far fa-file'></i></a></td></tr>";

                    }
                    conexion.Dispose();
                    return Content(cadenaRegreso);
                }
                catch (Exception ex)
                {
                    return Content("Error al consultar la base de datos");
                }
            }
        }

    }
}