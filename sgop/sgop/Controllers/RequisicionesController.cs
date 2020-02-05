using Newtonsoft.Json.Linq;
using sgop.Controllers.Paginacion;
using sgop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sgop.Controllers.Requisiciones
{
    public class RequisicionesController : Controller
    {
        public ActionResult getPaggedDataConceptos(int pageNumber = 1, int pageSize = 10, string busqueda = "", string buscarPor = "", string conceptosAgregados = "")
        {
            List<CatalogoConceptosViewModel> listConceptos = new List<CatalogoConceptosViewModel>();
            List<CatalogoConceptosViewModel> listConceptosAux = new List<CatalogoConceptosViewModel>();
            List<CatalogoConceptosViewModel> listConceptosAux2 = new List<CatalogoConceptosViewModel>();
            List<CatalogoConceptosViewModel> listConceptosAux3 = new List<CatalogoConceptosViewModel>();

            MethodEncrypt me = new MethodEncrypt();

            using (var db = new sgopEntities())
            {
                if (busqueda.Equals(""))
                {
                    listConceptos = (from cc in db.catalogoConceptos
                                     select new CatalogoConceptosViewModel
                                     {
                                         idConcepto = cc.idConcepto,
                                         idConceptoEncrypt = "",
                                         clave = cc.clave,
                                         descripcion = cc.descripcion,
                                         unidad = cc.unidad,
                                         precioUnitario = cc.precioUnitario
                                     }).ToList();
                    foreach (var item in listConceptos)
                    {
                        item.idConceptoEncrypt = me.getEncrypt(item.idConcepto.ToString());
                    }
                    listConceptosAux = listConceptos;
                }
                else
                {
                    listConceptos = (from cc in db.catalogoConceptos
                                    select new CatalogoConceptosViewModel
                                    {
                                        idConcepto = cc.idConcepto,
                                        idConceptoEncrypt = "",
                                        clave = cc.clave,
                                        descripcion = cc.descripcion,
                                        unidad = cc.unidad,
                                        precioUnitario = cc.precioUnitario
                                    }).ToList();

                    foreach (var item in listConceptos)
                    {
                        item.idConceptoEncrypt = me.getEncrypt(item.idConcepto.ToString());
                        if (item.clave.ToLower().Contains(busqueda.ToLower()) || item.descripcion.ToLower().Contains(busqueda.ToLower()))
                        {
                            listConceptosAux.Add(item);
                        }
                    }
                }

                if (buscarPor.Equals("todas"))
                {
                    if (conceptosAgregados == "")
                    {
                        var pagedData = Pagination.PagedResult(listConceptosAux, pageNumber, pageSize);
                        db.Dispose();
                        return Json(pagedData, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        JArray jsonConceptos = JArray.Parse(conceptosAgregados);
                        listConceptosAux2 = listConceptosAux;

                        for (int i = 0; i < jsonConceptos.Count; i++)
                        {
                            foreach (var item in listConceptosAux)
                            {
                                if (item.idConcepto == Convert.ToInt32(jsonConceptos[i]["idConcepto"]))
                                {
                                    listConceptosAux2.Remove(item);
                                    break;
                                }
                            }
                        }
                        var pagedData = Pagination.PagedResult(listConceptosAux2, pageNumber, pageSize);
                        db.Dispose();
                        return Json(pagedData, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    foreach (var item in listConceptosAux)
                    {
                        if (item.unidad.ToLower().Equals(buscarPor.ToLower()))
                        {
                            listConceptosAux2.Add(item);
                        }
                    }
                    if (conceptosAgregados == "")
                    {
                        var pagedData = Pagination.PagedResult(listConceptosAux2, pageNumber, pageSize);
                        db.Dispose();
                        return Json(pagedData, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        JArray jsonConceptos = JArray.Parse(conceptosAgregados);
                        listConceptosAux3 = listConceptosAux2;

                        for (int i = 0; i < jsonConceptos.Count; i++)
                        {
                            foreach (var item in listConceptosAux2)
                            {
                                if (item.idConcepto == Convert.ToInt32(jsonConceptos[i]["idConcepto"]))
                                {
                                    listConceptosAux3.Remove(item);
                                    break;
                                }
                            }
                        }
                        var pagedData = Pagination.PagedResult(listConceptosAux3, pageNumber, pageSize);
                        db.Dispose();
                        return Json(pagedData, JsonRequestBehavior.AllowGet);
                    }
                }
            }
        }

        // GET: Requisiciones
        [HttpPost]
        public ActionResult Index(FormCollection fc)
        {
            int idLicitacion;
            MethodEncrypt me = new MethodEncrypt();
            LicitacionesRequisicionesViewModel model = new LicitacionesRequisicionesViewModel();
            List<CatalogoConceptosViewModel> lstConceptos = new List<CatalogoConceptosViewModel>();
            List<CatalogoConceptosViewModel> lstUnidades = new List<CatalogoConceptosViewModel>();
            List<LicitacionesRequisicionesViewModel> lstRequisiciones = new List<LicitacionesRequisicionesViewModel>();

            using (var db = new sgopEntities())
            {
                try
                {
                    idLicitacion = Convert.ToInt32(fc["idLicitacion"].ToString());
                    var licitacion = db.licitaciones.Where(lic => lic.idLicitacion == idLicitacion).First();
                    var municipio = db.catalogoMunicipios.Where(mun => mun.idMunicipio == licitacion.idMunicipio).First();
                    var empresa = db.catalogoEmpresas.Where(emp => emp.idEmpresa == licitacion.idEmpresa).First();

                    model.idLicitacion = licitacion.idLicitacion;
                    model.idLicitacionEncrypt = me.getEncrypt(fc["idLicitacion"].ToString());
                    model.idEmpresa = licitacion.idEmpresa;
                    model.razonSocial = empresa.razonSocial;
                    model.noLicitacion =  licitacion.noLicitacion;
                    model.nombreObra =  licitacion.nombreObra;
                    model.idMunicipio =  licitacion.idMunicipio;
                    model.nombreMunicipio = municipio.descripcion;
                    model.localidad =  licitacion.localidad;
                    model.fechaVisita =  licitacion.fechaVisita;
                    model.fechaAclaraciones =  licitacion.fechaAclaraciones;
                    model.fechaPropuesta =  licitacion.fechaPropuesta;
                    model.fechaFallo =  licitacion.fechaFallo;
                    model.actaVisita =  licitacion.actaVisita;
                    model.actaAclaraciones =  licitacion.actaAclaraciones;
                    model.actaPropuesta =  licitacion.actaPropuesta;
                    model.actaFallo =  licitacion.actaFallo;
                    model.idEstatus =  licitacion.idEstatus;
                    model.idRequisicion =  licitacion.idRequisicion;
                    model.fechaCreacion =  licitacion.fechaCreacion;
                    model.usuarioCreacion =  licitacion.usuarioCreacion;
                    model.fechaModificacion =  licitacion.fechaModificacion;
                    model.usuarioModificacion =  licitacion.usuarioModificacion;

                    lstConceptos = (from cc in db.catalogoConceptos
                                     select new CatalogoConceptosViewModel
                                     {
                                         idConcepto = cc.idConcepto,
                                         idConceptoEncrypt = "",
                                         clave = cc.clave,
                                         descripcion = cc.descripcion,
                                         unidad = cc.unidad,
                                         precioUnitario = cc.precioUnitario
                                     }).ToList();

                    lstUnidades = (from c in db.catalogoConceptos
                                   orderby c.unidad ascending
                                  select new CatalogoConceptosViewModel
                                  {
                                      unidad = c.unidad
                                  }).Distinct().ToList();

                    lstRequisiciones = (from req in db.requisiciones
                                        join con in db.catalogoConceptos on req.idConcepto equals con.idConcepto
                                        where req.idRequisicionRango == licitacion.idRequisicion
                                        select new LicitacionesRequisicionesViewModel
                                        {
                                            idRequisicion = req.idRequisicion,
                                            idRequisicionRango = req.idRequisicionRango,
                                            idConcepto = req.idConcepto,
                                            idConceptoEncrypt = "",
                                            cantidad = req.cantidad,
                                            total = req.total,
                                            clave = con.clave,
                                            descripcion = con.descripcion,
                                            unidad = con.unidad,
                                            precioUnitario = con.precioUnitario
                                        }).ToList();

                    foreach(var item in lstRequisiciones)
                    {
                        item.idConceptoEncrypt = me.getEncrypt(item.idConcepto.ToString());
                    }

                    ViewBag.lstConceptos = lstConceptos;
                    ViewBag.lstRequisiciones = lstRequisiciones;
                    ViewBag.lstUnidades = lstUnidades;
                    db.Dispose();
                }
                catch (Exception)
                {
                    db.Dispose();
                    return Redirect(Url.Content("~/Home"));
                }
            }
            
            return View(model);
        }

        [HttpPost]
        public ActionResult GuardaRequisicion(FormCollection fc)
        {
            using (var db = new sgopEntities())
            {
                try
                {
                    MethodEncrypt me = new MethodEncrypt();
                    Rangos rango = new Rangos();
                    int idLicitacion = Convert.ToInt32(me.getDecrypt(fc["idLicitacionEncrypt"]));
                    JArray jsonConceptosAgregados = JArray.Parse(fc["conceptosAgregar"].ToString());
                    var licitacion = db.licitaciones.Where(lic => lic.idLicitacion == idLicitacion).First();
                    List<RequisicionesMaterialesViewModel> lstMateriales = new List<RequisicionesMaterialesViewModel>();

                    int idRequisicionRango = (int)licitacion.idRequisicion;

                    //Si la licitacion no tiene agregada la requisicion obtiene una nueva
                    if (idRequisicionRango == 0)
                    {
                        idRequisicionRango = rango.getSiguienteID("REQUISICIONES");
                        //Actualiza la requisicion de la licitacion
                        licitacion.idRequisicion = idRequisicionRango;
                        db.Entry(licitacion).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        var requisicionesList = (db.requisiciones.Where(req => req.idRequisicionRango == idRequisicionRango)).ToList();
                        var reqMatList = db.requisicionesMateriales.Where(reqMat => reqMat.idRequisicion == idRequisicionRango).ToList();

                        //Si la cantidad de la lista enviada es diferente a la cantidad de las requisiciones que hay en la base de datos
                        //Entonces elimina todas las requisiciones totales y materiales
                        if (requisicionesList.Count != jsonConceptosAgregados.Count)
                        {
                            //Requisiciones totales
                            foreach (var item in requisicionesList)
                            {
                                int? idConcepto = item.idConcepto;
                                requisiciones requisicion = db.requisiciones.Where(req => req.idConcepto == idConcepto && req.idRequisicionRango == idRequisicionRango).First();
                                db.requisiciones.Remove(requisicion);
                                db.SaveChanges();
                            }

                            //Requisiciones Materiales
                            foreach (var item in reqMatList)
                            {
                                requisicionesMateriales requisicionMat = db.requisicionesMateriales.Where(reqMat => reqMat.idRequisicion == idRequisicionRango).First();
                                db.requisicionesMateriales.Remove(requisicionMat);
                                db.SaveChanges();
                            }
                        }
                    }

                    for (int i = 0; i < jsonConceptosAgregados.Count; i++)
                    {
                        int idConcepto = Convert.ToInt32(jsonConceptosAgregados[i]["idConcepto"]);
                        double cantidad = Convert.ToDouble(jsonConceptosAgregados[i]["cantidad"]);

                        //Trae la requisicion donde haya ese idConcepto y ese idRequisicion
                        try
                        {
                            //Si la encuentra, actualiza la cantidad de la requisicion con ese idConcepto y ese idRequisicion
                            var requisicion = db.requisiciones.Where(r => r.idConcepto == idConcepto && r.idRequisicionRango == idRequisicionRango).First();
                            requisicion.cantidad = cantidad;
                            db.Entry(requisicion).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();

                            //Trae la lista de las requisiciones materiales para ese idConcepto y idRequisicion 
                            var requisicionMatLista = db.requisicionesMateriales.Where(rm => rm.idConcepto == idConcepto && rm.idRequisicion == idRequisicionRango).ToList();

                            //Recorre la lista
                            foreach (var reqm in requisicionMatLista)
                            {
                                //Busca en la tabla relacionConceptosMateriales el idConcepto y idMaterial para obtener la cantidad necesaria
                                var relConcMat = db.relacionConceptosMateriales.Where(rcm => rcm.idConcepto == idConcepto && rcm.idMaterial == reqm.idMaterial).First();
                                //Busca la requisicionMaterial que se va a actualizar
                                var requisicionMat = db.requisicionesMateriales.Find(reqm.idRequisicionMaterial);

                                requisicionMat.total = cantidad * relConcMat.cantidad;//Actualiza el total
                                db.Entry(requisicionMat).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                        catch (Exception)
                        {
                            //Si no la encuentra, guarda una nueva requisicion con ese idConcepto y ese idRequisicion
                            var concepto = db.catalogoConceptos.Find(idConcepto);
                            requisiciones tRequisiciones = new requisiciones();

                            tRequisiciones.idRequisicionRango = idRequisicionRango;
                            tRequisiciones.idConcepto = idConcepto;
                            tRequisiciones.cantidad = cantidad;
                            tRequisiciones.total = concepto.precioUnitario * cantidad;

                            db.requisiciones.Add(tRequisiciones);
                            db.SaveChanges();

                            //Obtiene todos los materiales que se necesitan para ese idConcepto

                            lstMateriales = (from mat in db.relacionConceptosMateriales
                                             where mat.idConcepto == idConcepto
                                             select new RequisicionesMaterialesViewModel
                                             {
                                                 idRelacion = mat.idRelacion,
                                                 idConcepto = mat.idConcepto,
                                                 idMaterial = mat.idMaterial,
                                                 cantidadMaterial = mat.cantidad
                                             }).ToList();

                            foreach (var relacion in lstMateriales)
                            {
                                requisicionesMateriales tReqMat = new requisicionesMateriales();

                                tReqMat.idRequisicion = idRequisicionRango;
                                tReqMat.idConcepto = idConcepto;
                                tReqMat.idMaterial = relacion.idMaterial;
                                tReqMat.total = cantidad * relacion.cantidadMaterial;

                                db.requisicionesMateriales.Add(tReqMat);
                                db.SaveChanges();
                            }
                        }
                    }
                    db.Dispose();
                    return Content("requisicionGuardada");
                }
                catch (Exception)
                {
                    db.Dispose();
                    return Content("noExisteLicitacion");
                }
            }
        }

        [HttpPost]
        public ActionResult AddConceptoTablaTemp(FormCollection fc)
        {
            //Solo SI el campo (fechaPropuesta) en la tabla licitaciones no ha pasado, se puede editar. 
            int IDConcepto = 0;
            double cantidad = 0;
            double? total = 0, subtotal;
            MethodEncrypt me = new MethodEncrypt();
            string []contenidoTabla = new string[4];//Guarda el body y el footer de la tabla
            catalogoConceptos concepto;
            DateTime fechaPropuesta = DateTime.Parse(fc["fechaPropuesta"]);

            JArray jsonConceptos = JArray.Parse(fc["conceptosAgregados"].ToString());

            using (var db = new sgopEntities())
            {
                //Verifica que el IDConcepto sea correcto
                try
                {
                    for (int i = 0; i < jsonConceptos.Count; i++)
                    {
                        concepto = db.catalogoConceptos.Find(Convert.ToInt32(jsonConceptos[i]["idConcepto"]));
                        total += Convert.ToDouble(jsonConceptos[i]["cantidad"]) * concepto.precioUnitario;
                    }

                    IDConcepto = Convert.ToInt32(me.getDecrypt(fc["idConceptoEncrypt"].ToString()));
                    concepto = db.catalogoConceptos.Where(cc => cc.idConcepto == IDConcepto).First();
                    cantidad = Convert.ToDouble(fc["cantidad"].ToString());
                    subtotal = cantidad * concepto.precioUnitario;
                    total += subtotal;

                    //Body de la tabla
                    contenidoTabla[0] = "<tr id='fila." + fc["idConceptoEncrypt"].ToString() + "'>";
                    contenidoTabla[0] += "<td>" + concepto.clave + "</td>";
                    contenidoTabla[0] += "<td>" + concepto.descripcion + "</td>";
                    contenidoTabla[0] += "<td>" + concepto.unidad + "</td>";
                    contenidoTabla[0] += "<td id='cantConcepto." + fc["idConceptoEncrypt"].ToString() + "'>" + cantidad + "</td>";
                    contenidoTabla[0] += "<td>" + concepto.precioUnitario + "</td>";
                    contenidoTabla[0] += "<td id='subtotal." + fc["idConceptoEncrypt"].ToString() + "'>" + subtotal + "</td>";
                    contenidoTabla[0] += "<td class='text-center'>";
                    contenidoTabla[0] += "<button onclick='editarCantidad(\"" + fc["idConceptoEncrypt"].ToString() + "\",\"" + cantidad + "\",\"" + concepto.descripcion + "\");' class='btn btn-success'>Editar <i class='fa fa-edit'></i></button>";
                    contenidoTabla[0] += "<button onclick='quitarConcepto(\"" + fc["idConceptoEncrypt"].ToString() + "\",\"" + concepto.descripcion + "\");' class='btn btn-danger'>Quitar <i class='fa fa-trash'></i></button>";
                    contenidoTabla[0] += "</td>";
                    contenidoTabla[0] += "</tr>";

                    //Footer de la tabla
                    contenidoTabla[1] = "<tr>";
                    contenidoTabla[1] += "<td colspan='5' class='text-muted font-weight-bolder'>TOTAL</td>";
                    contenidoTabla[1] += "<td class='text-muted font-weight-bolder' id='total'>" + total + "</td>";
                    if(fechaPropuesta >= DateTime.Now)
                    {
                        contenidoTabla[1] += "<td class='text-muted font-weight-bolder'></td>";
                    }
                    contenidoTabla[1] += "</tr>";

                    //IDConcepto AGREGADO
                    contenidoTabla[2] = IDConcepto.ToString();

                    //cantidad AGREGADA
                    contenidoTabla[3] = cantidad.ToString();

                    db.Dispose();
                    return Json(contenidoTabla);
                }
                catch (Exception)
                {
                    contenidoTabla[0] = "error";
                    contenidoTabla[1] = "error";
                    db.Dispose();
                    return Json(contenidoTabla);
                }
            }
        }

        [HttpPost]
        public ActionResult getTotalRequisicion(FormCollection fc)
        {
            //Solo SI el campo (fechaPropuesta) en la tabla licitaciones no ha pasado, se puede editar. 
            JArray jsonConceptos = JArray.Parse(fc["conceptosAgregados"].ToString());

            using (var db = new sgopEntities())
            {
                double? total = 0;
                //Verifica que el IDConcepto sea correcto
                try
                {
                    for (int i = 0; i < jsonConceptos.Count; i++)
                    {
                        var concepto = db.catalogoConceptos.Find(Convert.ToInt32(jsonConceptos[i]["idConcepto"]));
                        total += Convert.ToDouble(jsonConceptos[i]["cantidad"]) * concepto.precioUnitario;
                    }

                    db.Dispose();
                    return Content(total.ToString());
                }
                catch (Exception)
                {
                    db.Dispose();
                    return Content("error");
                }
            }
        }

        [HttpPost]
        public ActionResult DesencriptaIDConcepto(FormCollection fc)
        {
            using (var db = new sgopEntities())
            {
                try
                {
                    MethodEncrypt me = new MethodEncrypt();
                    int idConcepto = Convert.ToInt32(me.getDecrypt(fc["idConceptoEncrypt"]));
                    string[] data = new string[2];

                    var concepto = db.catalogoConceptos.Find(idConcepto);

                    data[0] = idConcepto.ToString();
                    data[1] = (concepto.precioUnitario * Convert.ToDouble(fc["cantidad"])).ToString();
                    db.Dispose();
                    return Json(data);
                }
                catch (Exception)
                {
                    return Content("");
                }
            }
        }
    }
}