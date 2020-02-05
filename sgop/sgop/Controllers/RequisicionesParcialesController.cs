using Newtonsoft.Json.Linq;
using sgop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sgop.Controllers.Requisiciones
{
    public class RequisicionesParcialesController : Controller
    {
        // GET: RequisicionesParciales
        [HttpPost]
        public ActionResult Index(FormCollection fc)
        {
            using (var db = new sgopEntities())
            {
                try
                {
                    int idLicitacion = Convert.ToInt32(fc["idLicitacion"]);
                    //Del idLicitacion se obtiene el idRequisicion
                    var requisicion = db.licitaciones.Where(req => req.idLicitacion == idLicitacion).First();
                    int idRequisicion = (int)requisicion.idRequisicion;
                    double acumulado = 0;

                    LicitacionesRequisicionesViewModel model = new LicitacionesRequisicionesViewModel();
                    List<RequisicionesParcialesViewModel> lstReqParciales = new List<RequisicionesParcialesViewModel>();
                    List<RequisicionesParcialesViewModel> lstReqParcialesDistinct = new List<RequisicionesParcialesViewModel>();
                    List<LicitacionesRequisicionesViewModel> lstRequisiciones = new List<LicitacionesRequisicionesViewModel>();
                    MethodEncrypt me = new MethodEncrypt();

                    var licitacion = db.licitaciones.Where(lic => lic.idLicitacion == idLicitacion).First();
                    var municipio = db.catalogoMunicipios.Where(mun => mun.idMunicipio == licitacion.idMunicipio).First();
                    var empresa = db.catalogoEmpresas.Where(emp => emp.idEmpresa == licitacion.idEmpresa).First();
                    var proyecto = db.proyectos.Where(proy => proy.idLicitacion == idLicitacion).First();

                    model.idLicitacion = licitacion.idLicitacion;
                    model.idProyecto = proyecto.idProyecto;
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

                    //Obtiene las requisiciones parciales con el idRequisicion
                    lstReqParciales = (from reqp in db.requisicionesParciales
                                       join mat in db.catalogoMateriales on reqp.idMaterial equals mat.idMaterial
                                       join cc in db.catalogoConceptos on reqp.idConcepto equals cc.idConcepto
                                       where reqp.idRequisicion == idRequisicion && reqp.noRequisicion == 1
                                       select new RequisicionesParcialesViewModel
                                       {
                                           idRequisicionParcial = reqp.idRequisicionParcial,
                                           idRequisicionRango = reqp.idRequisicion,
                                           idRequisicionRangoEncrypt = "",
                                           noRequisicion = reqp.noRequisicion,
                                           idConcepto = reqp.idConcepto,
                                           idMaterial = reqp.idMaterial,
                                           cantidad = reqp.cantidad,
                                           aprobada = reqp.aprobada,
                                           descripcionMaterial = mat.descripcion,
                                           claveConcepto = cc.clave,
                                           descripcionConcepto = cc.descripcion,
                                           unidadConcepto = cc.unidad,
                                           precioUnitario = cc.precioUnitario,
                                           cantReqXcantMat = 0,
                                           cantidadDisponible = 0
                                       }).ToList();

                    //Recorre la lista de requisiciones parciales para guardar las cantidades totales
                    foreach (var item in lstReqParciales)
                    {
                        //Busca en la relacionConceptosMateriales el idConcepto y idMaterial, para obtener la cantidad necesaria de material para ese concepto por unidad
                        var relConcMat = db.relacionConceptosMateriales.Where(au => au.idConcepto == item.idConcepto && au.idMaterial == item.idMaterial).First();
                        //Busca en las requisiciones el idConcepto y el idRequisicionRango, para obtener la cantidad que se necesita de ese concepto
                        var req = db.requisiciones.Where(au => au.idConcepto == item.idConcepto && au.idRequisicionRango == idRequisicion).First();

                        item.idRequisicionRangoEncrypt = me.getEncrypt(item.idRequisicionRango.ToString());//Encripta el idRequisicion
                        item.cantReqXcantMat = relConcMat.cantidad * req.cantidad;//Guarda la cantidad total de ese concepto a requerir
                        item.cantidadDisponible = item.cantReqXcantMat;//La cantidad disponible es la misma de la multiplicacion (mas adelante se actualiza)
                    }

                    //Recorre la lista de las requisiciones parciales encontradas
                    foreach (var reqPar in lstReqParciales) 
                    {
                        //Recorre la lista de requisiciones parciales encontradas
                        foreach (var reqParcDi in lstReqParciales)
                        {
                            //Compara el idConcepto y idMaterial de ambas listas
                            if (reqPar.idConcepto == reqParcDi.idConcepto && reqPar.idMaterial == reqParcDi.idMaterial)
                            {
                                acumulado += (double)reqParcDi.cantidad;//Guarda la cantidad de la requisicion parcial
                            }
                        }
                        reqPar.cantidadDisponible -= acumulado;//A la cantidad disponible le resta la misma cantidad menos el acumulado
                        acumulado = 0;
                    }

                    //Guarda esta variable para hacer el recorrido en la vista y pintar los conceptos y materiales
                    ViewBag.lstReqParcialesDistinct = (from reqp in db.requisicionesParciales
                                                       orderby reqp.noRequisicion descending
                                                       where reqp.idRequisicion == idRequisicion
                                                       select new RequisicionesParcialesViewModel
                                                       {
                                                           noRequisicion = reqp.noRequisicion
                                                       }).Distinct().ToList();
                    //Guarda esta variable para hacer el recorrido en la vista y pintar los conceptos y materiales
                    ViewBag.lstRequisicionesDistinct = (from req in db.requisiciones
                                                        join rcm in db.relacionConceptosMateriales on req.idConcepto equals rcm.idConcepto
                                                        join cm in db.catalogoMateriales on rcm.idMaterial equals cm.idMaterial
                                                        join con in db.catalogoConceptos on req.idConcepto equals con.idConcepto
                                                        where req.idRequisicionRango == idRequisicion
                                                        select new LicitacionesRequisicionesViewModel
                                                        {
                                                           idConcepto = req.idConcepto
                                                       }).Distinct().ToList();

                    //Busca los materiales necesarios para cada concepto que hay en la requisicion con el idRequisicion que se le envía
                    lstRequisiciones = (from req in db.requisiciones
                                        join rcm in db.relacionConceptosMateriales on req.idConcepto equals rcm.idConcepto
                                        join cm in db.catalogoMateriales on rcm.idMaterial equals cm.idMaterial
                                        join con in db.catalogoConceptos on req.idConcepto equals con.idConcepto
                                        where req.idRequisicionRango == idRequisicion
                                        select new LicitacionesRequisicionesViewModel
                                        {
                                            idRequisicion = req.idRequisicion,
                                            idRequisicionRango = req.idRequisicionRango,
                                            idRequisicionEncrypt = "",
                                            idConcepto = req.idConcepto,
                                            idConceptoEncrypt = "",
                                            cantReq = req.cantidad,
                                            cantMat = rcm.cantidad,
                                            cantReqXcantMat = 0,
                                            total = req.total,
                                            clave = con.clave,
                                            descripcion = con.descripcion,
                                            descripcionMaterial = cm.descripcion,
                                            unidad = con.unidad,
                                            precioUnitario = con.precioUnitario,
                                            idMaterial = cm.idMaterial
                                        }).ToList();

                    foreach (var item in lstRequisiciones)
                    {
                        item.idConceptoEncrypt = me.getEncrypt(item.idConcepto.ToString());//Encripta el idConcepto
                        item.idRequisicionEncrypt = me.getEncrypt(item.idRequisicionRango.ToString());//Encripta el idRequisicion
                        item.cantReqXcantMat = item.cantReq * item.cantMat;//Multiplica la cantidad pedida en la requisicion por la cantidad necesaria de material
                    }

                    ViewBag.lstRequisiciones = lstRequisiciones;//Guarda la variable para recorrerla en la vista
                    ViewBag.lstReqParciales = lstReqParciales;//Guarda la variable para recorrerla en la vista
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

        public ActionResult GuardaRequisicion(FormCollection fc)
        {
            
            MethodEncrypt me = new MethodEncrypt();
            List<RequisicionesParcialesViewModel> lstReqParciales = new List<RequisicionesParcialesViewModel>();
            int idLicitacion = 0;
            int idRequisicion = 0;
            int hayRequisiciones = 0;
            int noReqInt = 1;
            JArray jsonRequisiciones = null;

            //VERIFICA QUE EXISTA LA LICITACION Y LA REQUISICION
            using (var db = new sgopEntities())
            {
                try
                {
                    idLicitacion = Convert.ToInt32(fc["idLicitacion"]);
                    db.licitaciones.Where(lic => lic.idLicitacion == idLicitacion).First();

                    idRequisicion = Convert.ToInt32(fc["idRequisicion"]);
                    db.licitaciones.Where(lic => lic.idLicitacion == idLicitacion && lic.idRequisicion == idRequisicion).First();
                    db.Dispose();
                }
                catch (Exception)
                {
                    db.Dispose();
                    return Content("noExisteLicitacionReq");
                }
            }

            //VERIFICA SI ES REQUISICION NUEVA O YA EXISTENTE
            if (fc["noRequisicion"] != null && fc["noRequisicion"].Equals("nueva"))
            {
                jsonRequisiciones = JArray.Parse(fc["requisicionesAgregadas"].ToString());

                if (jsonRequisiciones.Count == 0)
                {
                    return Content("nadaParaGuardar");
                }

                using (var db = new sgopEntities())
                {
                    //Verifica que haya al menos un material por agregar
                    for (int i = 0; i < jsonRequisiciones.Count; i++)
                    {
                        if (jsonRequisiciones[i]["agregado"].ToString().Equals("1"))
                        {
                            hayRequisiciones = 1;
                        }
                    }
                    if (hayRequisiciones == 0)
                    {
                        return Content("nadaParaGuardar");
                    }

                    lstReqParciales = (from reqp in db.requisicionesParciales
                                       orderby reqp.noRequisicion descending
                                       where reqp.idRequisicion == idRequisicion
                                       select new RequisicionesParcialesViewModel
                                       {
                                            noRequisicion = reqp.noRequisicion
                                       }).Distinct().ToList();
                    foreach (var item in lstReqParciales)
                    {
                        noReqInt += 1;
                    }

                    for (int i = 0; i < jsonRequisiciones.Count; i++)
                    {
                        //Si no se eliminó de la tabla, agrega la requisicion
                        if (jsonRequisiciones[i]["agregado"].ToString().Equals("1"))
                        {
                            int idConcepto = Convert.ToInt32(jsonRequisiciones[i]["idConcepto"]);
                            int idMaterial = Convert.ToInt32(jsonRequisiciones[i]["idMaterial"]);
                            double cantidad = Convert.ToDouble(jsonRequisiciones[i]["cantidad"]);

                            requisicionesParciales tReqParc = new requisicionesParciales();

                            tReqParc.idRequisicion = idRequisicion;
                            tReqParc.noRequisicion = noReqInt;
                            tReqParc.idConcepto = idConcepto;
                            tReqParc.idMaterial = idMaterial;
                            tReqParc.cantidad = cantidad;
                            tReqParc.aprobada = "0";

                            db.requisicionesParciales.Add(tReqParc);
                            db.SaveChanges();
                        }
                    }
                }
            }
            else
            {
                jsonRequisiciones = JArray.Parse(fc["requisicionesAgregadas"].ToString());

                if (jsonRequisiciones.Count == 0)
                {
                    return Content("nadaParaGuardar");
                }

                try
                {
                    noReqInt = Convert.ToInt32(fc["noRequisicion"]);
                }
                catch (Exception)
                {
                    return Content("noExisteLicitacionReq");
                }

                using (var db = new sgopEntities())
                {
                    for (int i = 0; i < jsonRequisiciones.Count; i++)
                    {
                        int idConcepto = Convert.ToInt32(jsonRequisiciones[i]["idConcepto"]);
                        int idMaterial = Convert.ToInt32(jsonRequisiciones[i]["idMaterial"]);
                        double cantidad = Convert.ToDouble(jsonRequisiciones[i]["cantidad"]);

                        //Si existe la requisicion parcial con el idRequisicion, noRequisicion, idConcepto y idMaterial enviados
                        //Actualiza la requisicion
                        try
                        {
                            //Si no se eliminó de la tabla, actualiza la cantidad de la requisicion
                            if (jsonRequisiciones[i]["agregado"].ToString().Equals("1"))
                            {
                                var reqParc = db.requisicionesParciales.Where(rp => rp.idRequisicion == idRequisicion && rp.noRequisicion == noReqInt && rp.idConcepto == idConcepto && rp.idMaterial == idMaterial).First();

                                if (reqParc.aprobada.Equals("0"))
                                {
                                    reqParc.cantidad = cantidad;
                                    db.Entry(reqParc).State = System.Data.Entity.EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                            //Si se eliminó de la tabla, pone la requisicion en 0
                            else
                            {
                                try
                                {
                                    var reqParc = db.requisicionesParciales.Where(rp => rp.idRequisicion == idRequisicion && rp.noRequisicion == noReqInt && rp.idConcepto == idConcepto && rp.idMaterial == idMaterial).First();
                                    if (reqParc.aprobada.Equals("0"))
                                    {
                                        reqParc.cantidad = 0;
                                        db.Entry(reqParc).State = System.Data.Entity.EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                }
                                catch (Exception)
                                {

                                }
                            }
                        }
                        //Si no existe, agrega una nueva requisicion
                        catch (Exception)
                        {
                            requisicionesParciales tReqParc = new requisicionesParciales();

                            tReqParc.idRequisicion = idRequisicion;
                            tReqParc.noRequisicion = noReqInt;
                            tReqParc.idConcepto = idConcepto;
                            tReqParc.idMaterial = idMaterial;
                            tReqParc.cantidad = cantidad;
                            tReqParc.aprobada = "0";

                            db.requisicionesParciales.Add(tReqParc);
                            db.SaveChanges();
                        }
                    }
                }
            }
            return Content("requisicionGuardada");
        }

        public ActionResult CambiaRequisicion(FormCollection fc)
        {
            int idMaterialAdd = 0;
            string contenido = "";
            double acumulado = 0;
            int noRequisicion = 0;//0 cuando sea nueva
            int idLicitacion  = Convert.ToInt32(fc["idLicitacion"]);
            int idRequisicion = Convert.ToInt32(fc["idRequisicion"]);
            MethodEncrypt me = new MethodEncrypt();

            try
            {
                noRequisicion = Convert.ToInt32(fc["noRequisicion"]);
            }
            catch (Exception){}

            List<LicitacionesRequisicionesViewModel> lstRequisicionesDistinct = new List<LicitacionesRequisicionesViewModel>();
            List<LicitacionesRequisicionesViewModel> lstRequisiciones = new List<LicitacionesRequisicionesViewModel>();
            List<RequisicionesParcialesViewModel> lstReqParciales = new List<RequisicionesParcialesViewModel>();
            List<RequisicionesParcialesViewModel> lstReqParciales2 = new List<RequisicionesParcialesViewModel>();
            List<RequisicionesParcialesJson> lista = new List<RequisicionesParcialesJson>();

            using (var db = new sgopEntities())
            {
                //Obtiene todas las requisiciones parciales con el noRequisicion enviado y el idRequisicion *Nueva
                lstReqParciales = (from reqp in db.requisicionesParciales
                                   join mat in db.catalogoMateriales on reqp.idMaterial equals mat.idMaterial
                                   join cc in db.catalogoConceptos on reqp.idConcepto equals cc.idConcepto
                                   where reqp.idRequisicion == idRequisicion && reqp.noRequisicion == noRequisicion
                                   select new RequisicionesParcialesViewModel
                                   {
                                       idRequisicionParcial = reqp.idRequisicionParcial,
                                       idRequisicionRango = reqp.idRequisicion,
                                       idRequisicionRangoEncrypt = "",
                                       noRequisicion = reqp.noRequisicion,
                                       idConcepto = reqp.idConcepto,
                                       idMaterial = reqp.idMaterial,
                                       cantidad = reqp.cantidad,
                                       aprobada = reqp.aprobada,
                                       descripcionMaterial = mat.descripcion,
                                       claveConcepto = cc.clave,
                                       descripcionConcepto = cc.descripcion,
                                       unidadConcepto = cc.unidad,
                                       precioUnitario = cc.precioUnitario,
                                       cantReqXcantMat = 0,
                                       cantidadDisponible = 0
                                   }).ToList();

                //Obtiene todas las requisiciones parciales con el idRequisicion (sirve para cuando se envía requisicion nueva)
                lstReqParciales2 = (from reqp in db.requisicionesParciales
                                   join mat in db.catalogoMateriales on reqp.idMaterial equals mat.idMaterial
                                   join cc in db.catalogoConceptos on reqp.idConcepto equals cc.idConcepto
                                   where reqp.idRequisicion == idRequisicion
                                   select new RequisicionesParcialesViewModel
                                   {
                                       idRequisicionParcial = reqp.idRequisicionParcial,
                                       idRequisicionRango = reqp.idRequisicion,
                                       idRequisicionRangoEncrypt = "",
                                       noRequisicion = reqp.noRequisicion,
                                       idConcepto = reqp.idConcepto,
                                       idMaterial = reqp.idMaterial,
                                       cantidad = reqp.cantidad,
                                       aprobada = reqp.aprobada,
                                       descripcionMaterial = mat.descripcion,
                                       claveConcepto = cc.clave,
                                       descripcionConcepto = cc.descripcion,
                                       unidadConcepto = cc.unidad,
                                       precioUnitario = cc.precioUnitario,
                                       cantReqXcantMat = 0,
                                       cantidadDisponible = 0
                                   }).ToList();

                //Recorre la lista de requisiciones parciales para guardar las cantidades totales *Nueva
                foreach (var item in lstReqParciales)
                {
                    //Busca en la relacionConceptosMateriales el idConcepto y idMaterial, para obtener la cantidad necesaria de material para ese concepto por unidad
                    var relConcMat = db.relacionConceptosMateriales.Where(au => au.idConcepto == item.idConcepto && au.idMaterial == item.idMaterial).First();
                    //Busca en las requisiciones el idConcepto y el idRequisicionRango, para obtener la cantidad que se necesita de ese concepto
                    var req = db.requisiciones.Where(au => au.idConcepto == item.idConcepto && au.idRequisicionRango == idRequisicion).First();

                    item.idRequisicionRangoEncrypt = me.getEncrypt(item.idRequisicionRango.ToString());//Encripta el idRequisicion
                    item.cantReqXcantMat = relConcMat.cantidad * req.cantidad;//Guarda la cantidad total de ese concepto a requerir
                    item.cantidadDisponible = item.cantReqXcantMat;//La cantidad disponible es la misma de la multiplicacion (mas adelante se actualiza)
                }

                //Recorre la lista de las requisiciones parciales encontradas para poner el acumulado *Nueva
                foreach (var reqPar in lstReqParciales)
                {
                    //Recorre la lista de requisiciones parciales encontradas para poner el acumulado
                    foreach (var reqParcDi in lstReqParciales)
                    {
                        //Compara el idConcepto y idMaterial de ambas listas
                        if (reqPar.idConcepto == reqParcDi.idConcepto && reqPar.idMaterial == reqParcDi.idMaterial)
                        {
                            acumulado += (double)reqParcDi.cantidad;//Guarda la cantidad de la requisicion parcial
                        }
                    }
                    reqPar.cantidadDisponible -= acumulado;//A la cantidad disponible le resta la misma cantidad menos el acumulado
                    acumulado = 0;
                }

                //Recorre la lista de requisiciones parciales para guardar las cantidades totales
                foreach (var item in lstReqParciales2)
                {
                    //Busca en la relacionConceptosMateriales el idConcepto y idMaterial, para obtener la cantidad necesaria de material para ese concepto por unidad
                    var relConcMat = db.relacionConceptosMateriales.Where(au => au.idConcepto == item.idConcepto && au.idMaterial == item.idMaterial).First();
                    //Busca en las requisiciones el idConcepto y el idRequisicionRango, para obtener la cantidad que se necesita de ese concepto
                    var req = db.requisiciones.Where(au => au.idConcepto == item.idConcepto && au.idRequisicionRango == idRequisicion).First();

                    item.idRequisicionRangoEncrypt = me.getEncrypt(item.idRequisicionRango.ToString());//Encripta el idRequisicion
                    item.cantReqXcantMat = relConcMat.cantidad * req.cantidad;//Guarda la cantidad total de ese concepto a requerir
                    item.cantidadDisponible = item.cantReqXcantMat;//La cantidad disponible es la misma de la multiplicacion (mas adelante se actualiza)
                }

                //Recorre la lista de las requisiciones parciales encontradas para poner el acumulado
                foreach (var reqPar in lstReqParciales2)
                {
                    //Recorre la lista de las requisiciones parciales encontradas para poner el acumulado
                    foreach (var reqParcDi in lstReqParciales2)
                    {
                        //Compara el idConcepto y idMaterial de ambas listas
                        if (reqPar.idConcepto == reqParcDi.idConcepto && reqPar.idMaterial == reqParcDi.idMaterial)
                        {
                            acumulado += (double)reqParcDi.cantidad;//Guarda la cantidad de la requisicion parcial
                        }
                    }
                    reqPar.cantidadDisponible -= acumulado;//A la cantidad disponible le resta la misma cantidad menos el acumulado
                    acumulado = 0;
                }

                //Recorre la lista de las requisiciones parciales encontradas para poner el acumulado *Nueva
                foreach (var reqPar in lstReqParciales)
                {
                    foreach (var reqPar2 in lstReqParciales2)
                    {
                        if (reqPar2.idConcepto == reqPar.idConcepto && reqPar2.idMaterial == reqPar.idMaterial && reqPar2.noRequisicion == reqPar.noRequisicion)
                        {
                            reqPar.cantidadDisponible = reqPar2.cantidadDisponible;
                            break;
                        }
                    }
                }

                lstRequisicionesDistinct = (from req in db.requisiciones
                                            join rcm in db.relacionConceptosMateriales on req.idConcepto equals rcm.idConcepto
                                            join cm in db.catalogoMateriales on rcm.idMaterial equals cm.idMaterial
                                            join con in db.catalogoConceptos on req.idConcepto equals con.idConcepto
                                            where req.idRequisicionRango == idRequisicion
                                            select new LicitacionesRequisicionesViewModel
                                            {
                                                idConcepto = req.idConcepto
                                            }).Distinct().ToList();

                lstRequisiciones = (from req in db.requisiciones
                                    join rcm in db.relacionConceptosMateriales on req.idConcepto equals rcm.idConcepto
                                    join cm in db.catalogoMateriales on rcm.idMaterial equals cm.idMaterial
                                    join con in db.catalogoConceptos on req.idConcepto equals con.idConcepto
                                    where req.idRequisicionRango == idRequisicion
                                    select new LicitacionesRequisicionesViewModel
                                    {
                                        idRequisicion = req.idRequisicion,
                                        idRequisicionRango = req.idRequisicionRango,
                                        idRequisicionEncrypt = "",
                                        idConcepto = req.idConcepto,
                                        idConceptoEncrypt = "",
                                        cantReq = req.cantidad,
                                        cantMat = rcm.cantidad,
                                        cantReqXcantMat = 0,
                                        total = req.total,
                                        clave = con.clave,
                                        descripcion = con.descripcion,
                                        descripcionMaterial = cm.descripcion,
                                        unidad = con.unidad,
                                        precioUnitario = con.precioUnitario,
                                        idMaterial = cm.idMaterial,
                                        cantDisponible = 0
                                    }).ToList();

                foreach (var item in lstRequisiciones)
                {
                    item.idConceptoEncrypt = me.getEncrypt(item.idConcepto.ToString());
                    item.idRequisicionEncrypt = me.getEncrypt(item.idRequisicionRango.ToString());
                    item.cantReqXcantMat = item.cantReq * item.cantMat;
                    item.cantDisponible = item.cantReqXcantMat;
                }

                //Recorre la lista de las requisiciones parciales encontradas para poner el acumulado
                foreach (var reqPar in lstReqParciales2)
                {
                    foreach (var reqPar2 in lstRequisiciones)
                    {
                        if (reqPar2.idConcepto == reqPar.idConcepto && reqPar2.idMaterial == reqPar.idMaterial)
                        {
                            reqPar2.cantReqXcantMat = reqPar.cantidadDisponible;
                            break;
                        }
                    }
                }
            }

            foreach (var item in lstRequisicionesDistinct)
            {
                //PINTA EL CONCEPTO
                foreach (var item2 in lstRequisiciones)
                {
                    if (item.idConcepto == item2.idConcepto)
                    {
                        contenido += "<tr class='bg-dark' style='color:white;'>";
                        contenido += "<td>" + item2.clave + "</td>";
                        contenido += "<td colspan='5'>" + item2.descripcion + "</td>";
                        contenido += "</tr>";
                        break;
                    }
                }

                //PINTA LOS MATERIALES
                foreach (var item2 in lstRequisiciones)
                {
                    if (item.idConcepto == item2.idConcepto)
                    {
                        if (noRequisicion != 0)
                        {
                            foreach (var reqParc in lstReqParciales)
                            {
                                if (item2.idConcepto == reqParc.idConcepto && item2.idMaterial == reqParc.idMaterial)
                                {
                                    idMaterialAdd = (int)item2.idMaterial;
                                    contenido += "<tr class='bg-light'>";
                                    contenido += "<td>";
                                    if (!reqParc.aprobada.Equals("1"))
                                    {
                                        contenido += "<div class='btn-group'>";
                                        if (reqParc.cantidad != 0)
                                        {
                                            contenido += "<button title='Aprobar la requisición' class='btn btn-primary btn-sm rounded' type='button' id='btnAprobar." + reqParc.idConcepto + "." + reqParc.idMaterial + "' onclick='aprobarReq(\"" + reqParc.idConcepto + "\",\"" + reqParc.idMaterial + "\", \"" + reqParc.descripcionMaterial + "\" , " + reqParc.cantidad + "); '><i class='fa fa-check-circle'></i> Aprobar</button>";
                                            contenido += "<button title='Agregar cantidad de material' class='btn btn-success btn-sm rounded' type='button' style='display:none;' id='btnAgregar." + reqParc.idConcepto + "." + reqParc.idMaterial + "' onclick='agregaRequisicionTabla(\"" + reqParc.idConcepto + "\",\"" + reqParc.idMaterial + "\", \"" + reqParc.descripcionMaterial + "\"," + reqParc.cantidadDisponible + ");'><i class='fa fa-plus-circle'></i> Agregar</button>";
                                            contenido += "<button title='Quitar la cantidad de material' class='btn btn-danger btn-sm rounded' type='button' id='btnQuitar." + reqParc.idConcepto + "." + reqParc.idMaterial + "' onclick='quitaRequisicionTabla(\"" + reqParc.idConcepto + "\",\"" + reqParc.idMaterial + "\");'><i class='fa fa-times-circle'></i> Quitar</button>";
                                        }
                                        else
                                        {
                                            contenido += "<button title='Agregar cantidad de material' class='btn btn-success btn-sm rounded' type='button' id='btnAgregar." + reqParc.idConcepto + "." + reqParc.idMaterial + "' onclick='agregaRequisicionTabla(\"" + reqParc.idConcepto + "\",\"" + reqParc.idMaterial + "\", \"" + reqParc.descripcionMaterial + "\", " + reqParc.cantidadDisponible + ");'><i class='fa fa-plus-circle'></i> Agregar</button>";
                                            contenido += "<button title='Quitar la cantidad de material' class='btn btn-danger btn-sm rounded' type='button' style='display:none;' id='btnQuitar." + reqParc.idConcepto + "." + reqParc.idMaterial + "' onclick='quitaRequisicionTabla(\"" + reqParc.idConcepto + "\",\"" + reqParc.idMaterial + "\");'><i class='fa fa-times-circle'></i> Quitar</button>";
                                        }
                                        contenido += "</div>";
                                    }
                                    else
                                    {
                                        contenido += "<strong><a href='#' style='color:black;' title='Ver N° Documento'>REQUISICIÓN APROBADA</a></strong>";
                                    }
                                    contenido += "</td>";
                                    contenido += "<td>" + reqParc.descripcionMaterial + "</td>";
                                    contenido += "<td class='text-center'>" + reqParc.cantReqXcantMat + "</td>";
                                    if (reqParc.cantidadDisponible > 0)
                                    {
                                        contenido += "<td class='text-center' id='campoDisponible." + reqParc.idConcepto + "." + reqParc.idMaterial + "'>" + reqParc.cantidadDisponible + "</td>";
                                    }
                                    else
                                    {
                                        contenido += "<td class='text-center text-danger' id='campoDisponible." + reqParc.idConcepto + "." + reqParc.idMaterial + "'>" + reqParc.cantidadDisponible + "</td>";
                                    }
                                    contenido += "<td class='text-center' id='campoRequisicion." + reqParc.idConcepto + "." + reqParc.idMaterial + "'>" + reqParc.cantidad + " </td>";
                                    contenido += "</tr>";
                                    break;
                                }
                            }
                            if (item2.idMaterial != idMaterialAdd)
                            {
                                contenido += "<tr class='bg-light'>";
                                contenido += "<td>";
                                contenido += "<div class='btn-group'>";
                                contenido += "<button class='btn btn-success btn-sm rounded' type='button' id='btnAgregar." + item2.idConcepto + "." + item2.idMaterial + "' onclick='agregaRequisicionTabla(\"" + item2.idConcepto + "\",\"" + item2.idMaterial + "\", \"" + item2.descripcionMaterial + "\", " + item2.cantReqXcantMat + ");'><i class='fa fa-plus-circle'></i> Agregar</button>";
                                contenido += "<button class='btn btn-danger btn-sm rounded' type='button' id='btnQuitar." + item2.idConcepto + "." + item2.idMaterial + "' style='display:none;' onclick='quitaRequisicionTabla(\"" + item2.idConcepto + "\",\"" + item2.idMaterial + "\");'><i class='fa fa-times-circle'></i> Quitar</button>";
                                contenido += "</div>";
                                contenido += "</td>";
                                contenido += "<td>" + item2.descripcionMaterial + "</td>";
                                contenido += "<td class='text-center'>" + item2.cantDisponible + "</td>";
                                if (item2.cantReqXcantMat > 0)
                                {
                                    contenido += "<td class='text-center' id='campoDisponible." + item2.idConcepto + "." + item2.idMaterial + "'>" + item2.cantReqXcantMat + "</td>";
                                }
                                else
                                {
                                    contenido += "<td class='text-center text-danger' id='campoDisponible." + item2.idConcepto + "." + item2.idMaterial + "'>" + item2.cantReqXcantMat + "</td>";
                                }
                                contenido += "<td class='text-center' id='campoRequisicion." + item2.idConcepto + "." + item2.idMaterial + "'>0</td>";
                                contenido += "</tr>";
                                idMaterialAdd = 0;
                            }
                        }
                        else
                        {
                            contenido += "<tr class='bg-light'>";
                            contenido += "<td>";
                            contenido += "<div class='btn-group'>";
                            contenido += "<button class='btn btn-success btn-sm rounded' type='button' id='btnAgregar." + item2.idConcepto + "." + item2.idMaterial + "' onclick='agregaRequisicionTabla(\"" + item2.idConcepto + "\",\"" + item2.idMaterial + "\", \"" + item2.descripcionMaterial + "\", " + item2.cantReqXcantMat + ");'><i class='fa fa-plus-circle'></i> Agregar</button>";
                            contenido += "<button class='btn btn-danger btn-sm rounded' type='button' id='btnQuitar." + item2.idConcepto + "." + item2.idMaterial + "' style='display:none;' onclick='quitaRequisicionTabla(\"" + item2.idConcepto + "\",\"" + item2.idMaterial + "\");'><i class='fa fa-times-circle'></i> Quitar</button>";
                            contenido += "</div>";
                            contenido += "</td>";
                            contenido += "<td>" + item2.descripcionMaterial + "</td>";
                            contenido += "<td class='text-center'>" + item2.cantDisponible + "</td>";
                            if (item2.cantReqXcantMat > 0)
                            {
                                contenido += "<td class='text-center' id='campoDisponible." + item2.idConcepto + "." + item2.idMaterial + "'>" + item2.cantReqXcantMat + "</td>";
                            }
                            else
                            {
                                contenido += "<td class='text-center text-danger' id='campoDisponible." + item2.idConcepto + "." + item2.idMaterial + "'>" + item2.cantReqXcantMat + "</td>";
                            }
                            contenido += "<td class='text-center' id='campoRequisicion." + item2.idConcepto + "." + item2.idMaterial + "'>0</td>";
                            contenido += "</tr>";
                        }
                    }
                }
            }

            foreach (var item in lstReqParciales)
            {
                RequisicionesParcialesJson obj = new RequisicionesParcialesJson();
                obj.idConcepto = (int)item.idConcepto;
                obj.idMaterial = (int)item.idMaterial;
                obj.cantidad = (double)item.cantidad;
                obj.disponible = (double)item.cantidadDisponible;
                obj.agregado = "1";
                lista.Add(obj);
            }

            if (noRequisicion != 0)
            {
            }

            return Json(new { contenido, lista });
        }

        public ActionResult AprobarRequisicion(FormCollection fc)
        {
            using (var db = new sgopEntities())
            {
                try
                {
                    //Guarda los valores enviados en las variables
                    int noRequisicion = Convert.ToInt32(fc["noRequisicion"]);
                    int idLicitacion = Convert.ToInt32(fc["idLicitacion"]);
                    int idRequisicion = Convert.ToInt32(fc["idRequisicion"]);
                    int idConcepto = Convert.ToInt32(fc["idConcepto"]);
                    int idMaterial = Convert.ToInt32(fc["idMaterial"]);
                    Rangos rango = new Rangos();

                    //Busca la requisición parcial para actualizarla a aprobada
                    var reqParcial = db.requisicionesParciales.Where(rp => rp.idRequisicion == idRequisicion &&
                                                                           rp.noRequisicion == noRequisicion &&
                                                                           rp.idConcepto == idConcepto && 
                                                                           rp.idMaterial == idMaterial &&
                                                                           rp.cantidad > 0 && rp.aprobada.Equals("0")).First();
                    int idRequisicionParcial = reqParcial.idRequisicionParcial;
                    reqParcial.aprobada = "1";
                    db.Entry(reqParcial).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    int noDcoumento = rango.getSiguienteID("FACTURACLIENTE");//Obtiene el noDocumento siguiente

                    //Agrega la requisicion a la tabla control pagos
                    controlPagos tCP = new controlPagos();
                    tCP.idRequisicionParcial = idRequisicionParcial;
                    tCP.noDocumento = noDcoumento;
                    tCP.idRequisicion = idRequisicion;
                    tCP.clDocumento = "F";
                    tCP.fechaDocumento = DateTime.Now;
                    tCP.usuarioCreacion = 1;//Cambiar cuando se agregue la sesión

                    db.controlPagos.Add(tCP);
                    db.SaveChanges();

                    db.Dispose();
                    return Content(noDcoumento.ToString());
                }
                catch (Exception e)
                {
                    db.Dispose();
                    return Content("error");
                }
            }
        }
    }
}