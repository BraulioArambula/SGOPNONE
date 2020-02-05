using sgop.Models;
using sgop.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sgop.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost]
        public ActionResult Index(int idUser)
        {
            try
            {
                using (sgopEntities db = new sgopEntities())
                {
                    int[] array = new int[3]; int cont = 0;
                    try
                    {
                        relacionSistemasRoles oRol = new relacionSistemasRoles();
                        var temp = db.relacionSistemasRoles.Where(d => d.idUsuario == idUser).ToArray();
                        foreach (var i in temp)
                        {
                            if (i.idSistema == 1 || i.idSistema == 2 || i.idSistema == 3)
                            {
                                array[cont] = (int)i.idSistema;
                                cont++;
                            }

                        }
                        ViewBag.aux = array;
                        return View();

                    }
                    catch (Exception)
                    {
                        return Content("No tienes ningun permiso, contacta a un Administrador.");

                    }
                }
            }
            catch (Exception)
            {
                return Content("Ocurrio un error, intentalos mas tarde.");
            }



        }

        [HttpPost]
        public ActionResult BuscarId(string user)
        {
            try
            {
                using (sgopEntities db = new sgopEntities())
                {
                    try
                    {
                        var con = db.usuarios.Where(aux => aux.usuario == user).First();
                        var x = 1;
                        return Content("true");
                    }
                    catch (Exception)
                    {
                        return Content("Usuario incorrecto");
                    }
                }
            }
            catch (Exception ex)
            {
                return Content("Ocurrio un error " + ex.Message);
            }
        }

        public ActionResult PrincipalConstructora()
        {
            try
            {
                List<ProyectoViewModel> lst = null;
                List<LicitacionViewModel> lst2 = null;
                using (sgopEntities db = new sgopEntities())
                {
                    try
                    {
                        lst = (from d in db.proyectos
                               join l in db.licitaciones on d.idLicitacion equals l.idLicitacion
                               join id in db.catalogoEmpresas on l.idEmpresa equals id.idEmpresa
                               join mun in db.catalogoMunicipios on l.idMunicipio equals mun.idMunicipio
                               join e in db.catalogoEstatus on d.idEstatus equals e.idEstatus
                               orderby d.fechaCreacion
                               select new ProyectoViewModel
                               {
                                   idProyecto = d.idProyecto,
                                   idLicitacion = (int)d.idLicitacion,
                                   Empresa = id.razonSocial,
                                   nombreObra = l.nombreObra,
                                   noLicitacion = l.noLicitacion,
                                   Municipio = mun.descripcion,
                                   Estatus = e.descripcion,
                                   localidad = l.localidad,

                               }).ToList();

                        lst2 = (from l in db.licitaciones
                                join id in db.catalogoEmpresas on l.idEmpresa equals id.idEmpresa
                                join mun in db.catalogoMunicipios on l.idMunicipio equals mun.idMunicipio
                                join e in db.catalogoEstatus on l.idEstatus equals e.idEstatus
                                orderby l.fechaCreacion
                                select new LicitacionViewModel
                                {

                                    idLicitacion = l.idLicitacion,
                                    Empresa = id.razonSocial,
                                    nombreObra = l.nombreObra,
                                    noLicitacion = l.noLicitacion,
                                    Municipio = mun.descripcion,
                                    Estatus = e.descripcion,
                                    localidad = l.localidad,

                                }).ToList();
                        ViewBag.licitacion = lst2;
                        ViewBag.proyecto = lst;
                        return View();

                    }
                    catch (Exception)
                    {
                        return Content("Ocurrio un error, intentalos mas tarde.");
                    }
                }
            }
            catch (Exception)
            {
                return Content("Ocurrio un error, intentalos mas tarde.");
            }
        }

        public JsonResult BuscarMunicipios()
        {
            try
            {
                using (sgopEntities db = new sgopEntities())
                {
                    try
                    {
                        var con = db.catalogoMunicipios.ToArray();
                        int aux = con.Length; int cont = 0;
                        string[] array = new string[aux];
                        foreach (var i in con)
                        {
                            if (i != null)
                            {
                                array[cont] = i.descripcion;
                                cont++;
                            }
                        }

                        return Json(new { a = true, b = array });

                    }
                    catch (Exception)
                    {

                        return Json(new { a = false, b = "Ocurrio un error, al cargar los Municipios." });
                    }
                }
            }
            catch (Exception)
            {

                return Json(new { a = false, b = "Ocurrio un error,al cargar los Municipios." });
            }
        }

        public ActionResult MenuProyectos()
        {
            try
            {
                List<ProyectoViewModel> lst = null;
                using (sgopEntities db = new sgopEntities())
                {
                    try
                    {
                        lst = (from d in db.proyectos
                               join l in db.licitaciones on d.idLicitacion equals l.idLicitacion
                               join id in db.catalogoEmpresas on l.idEmpresa equals id.idEmpresa
                               join mun in db.catalogoMunicipios on l.idMunicipio equals mun.idMunicipio
                               join e in db.catalogoEstatus on d.idEstatus equals e.idEstatus
                               select new ProyectoViewModel
                               {
                                   idProyecto = d.idProyecto,
                                   idLicitacion = (int)d.idLicitacion,
                                   Empresa = id.razonSocial,
                                   fechaContrato = (DateTime)d.fechaContrato,
                                   polizaAnticipo = d.polizaAnticipo,
                                   nombreObra = l.nombreObra,
                                   noLicitacion = l.noLicitacion,
                                   Municipio = mun.descripcion,
                                   Estatus = e.descripcion,
                                   localidad = l.localidad,
                                   polizaVicios = d.polizaVicios,
                                   polizaCumplimiento = d.policaCumplimiento,
                                   //plazoDias = (int) d.plazoDias,                                  
                                   fechaInicioContrato = (DateTime)d.fechaInicioContrato,
                                   fechaFinalContrato = (DateTime)d.fechaFinalContrato,
                                   fechaInicioReal = (DateTime)d.fechaInicioReal,
                                   fechaFinalReal = (DateTime)d.fechaFinalReal,
                                   actaEntrega = d.actaEntrega,
                                   //jefeObra = (int) d.jefeObra
                               }).ToList();
                        return View(lst);

                    }
                    catch (Exception ex)
                    {
                        return Content(ex.Message);
                    }
                }
            }
            catch (Exception)
            {
                return Content("Ocurrio un error, intentalos mas tarde.");
            }
        }

        [HttpPost]
        public ActionResult MenuLikeProyectos(string[][] array)
        {
            string noLicitacion = array[0][1];
            string nombreObra = array[4][1]; string localidad = array[5][1]; //int idMunicipio = Int32.Parse(array[1][1].Substring(1, 1));
            int Estatus = Int32.Parse(array[6][1].Substring(0, 1)); Estatus = Estatus + 1;

            DateTime fechaFinal = Convert.ToDateTime("2000-01-01"); string temp = array[3][1];
            if (array[3][1] != "") { fechaFinal = Convert.ToDateTime(array[3][1]); }

            DateTime fechaInicial = Convert.ToDateTime("2000-01-01"); string temp2 = array[2][1];
            if (array[2][1] != "") { fechaInicial = Convert.ToDateTime(array[2][1]); }

            int idMunicipio = -1;
            if (array[1][1] != "") { idMunicipio = Int32.Parse(array[1][1].Substring(0)); }


            try
            {
                List<ProyectoViewModel> lst = null;
                using (sgopEntities db = new sgopEntities())
                {
                    try
                    {

                        lst = (from d in db.proyectos
                               join l in db.licitaciones on d.idLicitacion equals l.idLicitacion
                               join id in db.catalogoEmpresas on l.idEmpresa equals id.idEmpresa
                               join mun in db.catalogoMunicipios on l.idMunicipio equals mun.idMunicipio
                               join e in db.catalogoEstatus on d.idEstatus equals e.idEstatus
                               where l.noLicitacion.Contains(noLicitacion) && l.nombreObra.Contains(nombreObra) && d.idEstatus == Estatus
                               && (temp != "" ? d.fechaFinalContrato == fechaFinal : true) && (temp2 != "" ? d.fechaInicioContrato == fechaInicial : true)
                               && (idMunicipio != -1 ? l.idMunicipio == idMunicipio : true) && l.localidad.Contains(localidad)
                               //&& l.idMunicipio == idMunicipio && d.fechaInicioContrato == fechaInicial &&  d.fechaFinalContrato == fechaFinal  
                               select new ProyectoViewModel
                               {
                                   idProyecto = d.idProyecto,
                                   idLicitacion = (int)d.idLicitacion,
                                   Empresa = id.razonSocial,
                                   fechaContrato = (DateTime)d.fechaCreacion,
                                   nombreObra = l.nombreObra,
                                   noLicitacion = l.noLicitacion,
                                   Municipio = mun.descripcion,
                                   Estatus = e.descripcion,
                                   localidad = l.localidad,
                                   FCreacion = d.fechaCreacion.ToString()
                               }).ToList();
                        foreach (var i in lst)
                        {

                            i.FCreacion = i.fechaContrato.ToString("yyyy-MM-dd");
                        }

                        return Json(new { a = true, b = lst.ToArray() });

                    }
                    catch (Exception)
                    {
                        return Json(new { a = false, b = "Ocurrio un error, intentalo mas tarde." });
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { a = false, b = "Ocurrio un error, intentalo mas tarde." });
            }
        }


        public ActionResult BuscarLikeLicitacion(string nombre)
        {
            try
            {
                using (sgopEntities db = new sgopEntities())
                {
                    try
                    {
                        var con = db.licitaciones.Where(d => d.idEstatus == 2 && d.noLicitacion.Contains(nombre)).ToArray(); //Despues Cambiar la consulta para buscar solo aprobadas
                        int aux = con.Length; int cont = 0;
                        string[] array = new string[aux];

                        foreach (var i in con)
                        {
                            if (i != null)
                            {
                                try
                                {
                                    var con2 = db.proyectos.Where(d => d.idLicitacion == i.idLicitacion).First();
                                }
                                catch (Exception)
                                {
                                    array[cont] = i.noLicitacion;
                                    cont++;
                                }

                            }
                        }
                        string[] array2 = new string[cont]; cont = 0;
                        foreach (var y in array)
                        {
                            if (y != null)
                            {
                                array2[cont] = y;
                                cont++;
                            }
                        }
                        return Json(new { a = true, b = array2 });

                    }
                    catch (Exception)
                    {

                        return Json(new { a = false, b = "Ocurrio un error, intentalos mas tarde." });
                    }
                }
            }
            catch (Exception)
            {

                return Json(new { a = false, b = "Ocurrio un error, intentalos mas tarde." });
            }
        }

        public ActionResult BuscarLicitacion(string nombre)
        {
            try
            {
                using (sgopEntities db = new sgopEntities())
                {
                    try
                    {
                        var con = db.licitaciones.Where(d => d.noLicitacion == nombre).First();

                        try
                        {
                            var con2 = db.proyectos.Where(d => d.idLicitacion == con.idLicitacion).First();
                            return Json(new { a = false, b = "Ya existe un Proyecto para esa Licitacion." });
                        }
                        catch (Exception)
                        {
                            return Json(new { a = true, b = con.idLicitacion });
                        }



                    }
                    catch (Exception)
                    {

                        return Json(new { a = false, b = "No se encontro la Licitacion." });
                    }
                }
            }
            catch (Exception)
            {

                return Json(new { a = false, b = "Ocurrio un error, intentalos mas tarde." });
            }
        }

        [HttpPost]
        public ActionResult Crear_Proyecto(int id_lic)
        {
            try
            {
                ProyectoViewModel model = new ProyectoViewModel();
                List<LicitacionViewModel> lst = null;
                using (sgopEntities db = new sgopEntities())
                {
                    lst = (from d in db.licitaciones
                           join em in db.catalogoEmpresas on d.idEmpresa equals em.idEmpresa
                           join mun in db.catalogoMunicipios on d.idMunicipio equals mun.idMunicipio
                           where d.idLicitacion == id_lic
                           select new LicitacionViewModel
                           {

                               idLicitacion = (int)d.idLicitacion,
                               Municipio = mun.descripcion,
                               localidad = d.localidad,
                               noLicitacion = d.noLicitacion,
                               nombreObra = d.nombreObra,
                               //Faltan propuesta, requisisicones y Avance
                               Empresa = em.razonSocial,

                           }).ToList();
                    foreach (var i in lst)
                    {
                        model.idLicitacion = i.idLicitacion;
                        model.Municipio = i.Municipio;
                        model.localidad = i.localidad;
                        model.noLicitacion = i.noLicitacion;
                        model.nombreObra = i.nombreObra;
                        model.Empresa = i.Empresa;

                    }

                    return View(model);
                }
            }
            catch (Exception)
            {
                return View("MenuProyectos");
            }
        }


        [HttpPost]
        public ActionResult Subir_Proyecto(int Estatus, ProyectoViewModel model, HttpPostedFileBase PolizaAnticipo, HttpPostedFileBase PolizaVicios, HttpPostedFileBase PolizaCumplimiento)
        {
            model.fechaContrato.ToString("dd-MMM-yyyy");

            SubirDocs oSubir = new SubirDocs();

            if (!ModelState.IsValid)
            {
                return View("Crear_Proyecto", model);
            }
            else
            {
                string polizaAnticipo = null;
                if (PolizaAnticipo != null)
                {
                    string path = Server.MapPath("~/Res/Documentos/");
                    path += model.idLicitacion + "-01-" + PolizaAnticipo.FileName;
                    oSubir.Subir(path, PolizaAnticipo);
                    polizaAnticipo = PolizaAnticipo.FileName;


                }
                string polizaVicios = null;
                if (PolizaVicios != null)
                {
                    string path = Server.MapPath("~/Res/Documentos/");
                    path += model.idLicitacion + "-02-" + PolizaVicios.FileName;
                    oSubir.Subir(path, PolizaVicios);
                    polizaVicios = PolizaVicios.FileName;

                }
                string polizaCumplimiento = null;
                if (PolizaCumplimiento != null)
                {
                    string path = Server.MapPath("~/Res/Documentos/");
                    path += model.idLicitacion + "-03-" + PolizaCumplimiento.FileName;
                    oSubir.Subir(path, PolizaCumplimiento);
                    polizaCumplimiento = PolizaCumplimiento.FileName;
                }


                using (var db = new sgopEntities())
                {
                    proyectos oProyecto = new proyectos();

                    oProyecto.idLicitacion = model.idLicitacion;
                    oProyecto.fechaContrato = Convert.ToDateTime(model.fechaContrato.ToString("dd-MMM-yyyy"));
                    oProyecto.polizaAnticipo = polizaAnticipo;
                    oProyecto.polizaVicios = polizaVicios;
                    oProyecto.policaCumplimiento = polizaCumplimiento;
                    oProyecto.idEstatus = Estatus;
                    //Falta plazo dias.
                    oProyecto.fechaInicioContrato = Convert.ToDateTime(model.fechaInicioContrato.ToString("dd-MMM-yyyy"));
                    oProyecto.fechaFinalContrato = Convert.ToDateTime(model.fechaFinalContrato.ToString("dd-MMM-yyyy"));
                    oProyecto.fechaInicioReal = Convert.ToDateTime(model.fechaInicioReal.ToString("dd-MMM-yyyy"));
                    oProyecto.fechaFinalReal = Convert.ToDateTime(model.fechaFinalReal.ToString("dd-MMM-yyyy"));
                    //Falta Acta entrega y Jefe de Obra.
                    oProyecto.fechaCreacion = DateTime.Today;
                    oProyecto.usuarioCreacion = 1;   //Session["Id"].ToString();

                    db.proyectos.Add(oProyecto);
                    db.SaveChanges();


                }
                if (oSubir.confirmacion != true)
                {
                    ViewBag.message = "Ocurrio un error al cargar algun documento.";
                    ViewBag.status = "Error!";
                }
                else
                {
                    ViewBag.message = "Se agrego el usuario exitosamente!";
                    ViewBag.status = "Exito!";
                }

                return View("Crear_Proyecto");

            }

        }

        [HttpPost]
        public ActionResult CargarEstatus()
        {
            try
            {
                using (sgopEntities db = new sgopEntities())
                {
                    try
                    {
                        var con = db.catalogoEstatus.ToArray();
                        int aux = con.Length; int cont = 0;
                        string[] array = new string[aux];
                        foreach (var i in con)
                        {
                            if (i != null)
                            {
                                array[cont] = i.descripcion;
                                cont++;
                            }
                        }

                        return Json(new { a = true, b = array });

                    }
                    catch (Exception)
                    {

                        return Json(new { a = false, b = "Ocurrio un error, al cargar los Estatus." });
                    }
                }
            }
            catch (Exception)
            {

                return Json(new { a = false, b = "Ocurrio un error,al cargar los Estatus." });
            }
        }


        [HttpGet]
        public ActionResult ModificarProyecto(int idProyecto = -1, int idLicitacion = -1)
        {
            if (idProyecto == -1 || idLicitacion == -1) { return RedirectToAction("MenuProyectos"); }

            try
            {
                EditarProyectoViewModel model = new EditarProyectoViewModel();
                List<LicitacionViewModel> lst = null;
                using (sgopEntities db = new sgopEntities())
                {
                    lst = (from d in db.licitaciones
                           join em in db.catalogoEmpresas on d.idEmpresa equals em.idEmpresa
                           join mun in db.catalogoMunicipios on d.idMunicipio equals mun.idMunicipio
                           where d.idLicitacion == idLicitacion
                           select new LicitacionViewModel
                           {

                               idLicitacion = (int)d.idLicitacion,
                               Municipio = mun.descripcion,
                               localidad = d.localidad,
                               noLicitacion = d.noLicitacion,
                               nombreObra = d.nombreObra,
                               //Faltan propuesta, requisisicones y Avance
                               Empresa = em.razonSocial,

                           }).ToList();
                    var oProyecto = db.proyectos.Where(d => d.idProyecto == idProyecto).First();
                    var oEstatus = db.catalogoEstatus.ToArray();
                    foreach (var i in lst)
                    {
                        model.idLicitacion = i.idLicitacion;
                        model.Municipio = i.Municipio;
                        model.localidad = i.localidad;
                        model.noLicitacion = i.noLicitacion;
                        model.nombreObra = i.nombreObra;
                        model.Empresa = i.Empresa;


                    }
                    if (oProyecto.polizaAnticipo != null) { model.polizaAnticipo = true; } else { model.polizaAnticipo = false; }
                    if (oProyecto.polizaVicios != null) { model.polizaVicios = true; } else { model.polizaVicios = false; }
                    if (oProyecto.policaCumplimiento != null) { model.polizaCumplimiento = true; } else { model.polizaCumplimiento = false; }
                    model.NpolizaAnticipo = oProyecto.polizaAnticipo;
                    model.NpolizaCumplimiento = oProyecto.policaCumplimiento;
                    model.NpolizaVicios = oProyecto.polizaVicios;


                    var aux = (DateTime)oProyecto.fechaContrato;
                    model.fechaContrato = Convert.ToDateTime(aux.ToString("dd/MM/yyyy"));

                    aux = (DateTime)oProyecto.fechaInicioContrato;
                    model.fechaInicioContrato = Convert.ToDateTime(aux.ToString("dd/MM/yyyy"));

                    aux = (DateTime)oProyecto.fechaFinalContrato;
                    model.fechaFinalContrato = Convert.ToDateTime(aux.ToString("dd/MM/yyyy"));

                    aux = (DateTime)oProyecto.fechaInicioReal;
                    model.fechaInicioReal = Convert.ToDateTime(aux.ToString("dd/MM/yyyy"));

                    aux = (DateTime)oProyecto.fechaFinalReal;
                    model.fechaFinalReal = Convert.ToDateTime(aux.ToString("dd/MM/yyyy"));

                    List<SelectListItem> opciones = new List<SelectListItem>();

                    foreach (var i in oEstatus)
                    {
                        SelectListItem selListItem = new SelectListItem() { Text = i.descripcion, Value = i.idEstatus.ToString() };
                        opciones.Add(selListItem);
                    }

                    foreach (var opcion in opciones)
                    {
                        if (opcion.Value.Contains(oProyecto.idEstatus.ToString())) { opcion.Selected = true; }
                    }


                    ViewBag.opciones = opciones;
                    return View(model);
                }
            }
            catch (Exception)
            {
                return RedirectToAction("MenuProyectos");
            }
        }

        [HttpPost]
        public ActionResult ModificarProyecto(EditarProyectoViewModel model, HttpPostedFileBase PolizaAnticipo, HttpPostedFileBase PolizaVicios, HttpPostedFileBase PolizaCumplimiento)
        {
            try
            {
                bool flag = false;
                SubirDocs oSubir = new SubirDocs();
                using (var db = new sgopEntities())
                {

                    var oProyecto = db.proyectos.Find(model.idProyecto);

                    if (oProyecto.idEstatus == Int32.Parse(model.Status) && oProyecto.fechaContrato == model.fechaContrato && oProyecto.fechaInicioContrato == model.fechaInicioContrato &&
                        oProyecto.fechaFinalContrato == model.fechaFinalContrato && oProyecto.fechaInicioReal == model.fechaInicioReal && oProyecto.fechaFinalReal == model.fechaFinalReal
                        && PolizaVicios == null && PolizaCumplimiento == null && PolizaAnticipo == null)
                    {
                        ViewBag.message = "No hay nada que actualizar!";
                        ViewBag.status = "Info!";
                        flag = true;
                    }


                    if (PolizaAnticipo != null)
                    {

                        int i = PolizaAnticipo.FileName.IndexOf(".");
                        string temp = PolizaAnticipo.FileName.Substring(i);
                        string path = Server.MapPath("~/Res/Documentos/");
                        path += model.idLicitacion + "-01" + temp;
                        oSubir.Subir(path, PolizaAnticipo);
                        oProyecto.polizaAnticipo = PolizaAnticipo.FileName;
                    }

                    if (PolizaVicios != null)
                    {
                        int i = PolizaVicios.FileName.IndexOf(".");
                        string temp = PolizaVicios.FileName.Substring(i);
                        string path = Server.MapPath("~/Res/Documentos/");
                        path += model.idLicitacion + "-02" + temp;
                        oSubir.Subir(path, PolizaVicios);
                        oProyecto.polizaVicios = PolizaVicios.FileName;
                    }

                    if (PolizaCumplimiento != null)
                    {
                        int i = PolizaCumplimiento.FileName.IndexOf(".");
                        string temp = PolizaCumplimiento.FileName.Substring(i);
                        string path = Server.MapPath("~/Res/Documentos/");
                        path += model.idLicitacion + "-03" + temp;
                        oSubir.Subir(path, PolizaCumplimiento);
                        oProyecto.policaCumplimiento = PolizaCumplimiento.FileName;
                    }

                    BorrarArchivo oBorrar = new BorrarArchivo();

                    oProyecto.idEstatus = Int32.Parse(model.Status);
                    oProyecto.fechaContrato = model.fechaContrato;
                    oProyecto.fechaInicioContrato = model.fechaInicioContrato;
                    oProyecto.fechaFinalContrato = model.fechaFinalContrato;
                    oProyecto.fechaInicioReal = model.fechaInicioReal;
                    oProyecto.fechaFinalReal = model.fechaFinalReal;



                    oProyecto.usuarioModificacion = 1; //Session["User"].ToString();
                    oProyecto.fechaModificacion = DateTime.Today;


                    if (flag == false)
                    {
                        db.Entry(oProyecto).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }



                    if (oProyecto.polizaAnticipo != null) { model.polizaAnticipo = true; } else { model.polizaAnticipo = false; }
                    if (oProyecto.polizaVicios != null) { model.polizaVicios = true; } else { model.polizaVicios = false; }
                    if (oProyecto.policaCumplimiento != null) { model.polizaCumplimiento = true; } else { model.polizaCumplimiento = false; }

                    if (oProyecto.polizaAnticipo != null) { model.NpolizaAnticipo = oProyecto.polizaAnticipo; } else { model.NpolizaAnticipo = null; }
                    if (oProyecto.polizaVicios != null) { model.NpolizaVicios = oProyecto.polizaVicios; } else { model.NpolizaVicios = null; }
                    if (oProyecto.policaCumplimiento != null) { model.NpolizaCumplimiento = oProyecto.policaCumplimiento; } else { model.NpolizaCumplimiento = null; }

                    var oEstatus = db.catalogoEstatus.ToArray();

                    List<SelectListItem> opciones = new List<SelectListItem>();

                    foreach (var i in oEstatus)
                    {
                        SelectListItem selListItem = new SelectListItem() { Text = i.descripcion, Value = i.idEstatus.ToString() };
                        opciones.Add(selListItem);
                    }

                    foreach (var opcion in opciones)
                    {
                        if (opcion.Value.Contains(oProyecto.idEstatus.ToString())) { opcion.Selected = true; }
                    }
                    ViewBag.opciones = opciones;

                }

                if (flag == true)
                {
                    return View(model);
                }

                ViewBag.message = "Se actualizaron los datos exitosamente!";
                ViewBag.status = "Exito!";


                return View(model);
            }
            catch (Exception)
            {
                ViewBag.message = "Ocurrio un error al actualizar los datos!.";
                ViewBag.status = "Error!";
                return View(model);
            }

        }


        [HttpPost]
        public ActionResult VisualizarProyecto(int idProyecto = -1)
        {
            if (idProyecto == -1) { return RedirectToAction("MenuProyectos"); }

            try
            {
                EditarProyectoViewModel model = new EditarProyectoViewModel();
                List<EditarProyectoViewModel> lst = null;
                using (sgopEntities db = new sgopEntities())
                {
                    var oProyecto = db.proyectos.Where(d => d.idProyecto == idProyecto).First();
                    lst = (from d in db.licitaciones
                           join em in db.catalogoEmpresas on d.idEmpresa equals em.idEmpresa
                           join mun in db.catalogoMunicipios on d.idMunicipio equals mun.idMunicipio
                           where d.idLicitacion == oProyecto.idLicitacion
                           select new EditarProyectoViewModel
                           {

                               idLicitacion = (int)d.idLicitacion,
                               Municipio = mun.descripcion,
                               localidad = d.localidad,
                               noLicitacion = d.noLicitacion,
                               nombreObra = d.nombreObra,
                               //Faltan propuesta, requisisicones y Avance
                               Empresa = em.razonSocial,

                           }).ToList();

                    var oEstatus = db.catalogoEstatus.Where(d => d.idEstatus == oProyecto.idEstatus).First();
                    model.Status = oEstatus.descripcion;

                    foreach (var i in lst)
                    {
                        model.idLicitacion = i.idLicitacion;
                        model.Municipio = i.Municipio;
                        model.localidad = i.localidad;
                        model.noLicitacion = i.noLicitacion;
                        model.nombreObra = i.nombreObra;
                        model.Empresa = i.Empresa;


                    }
                    if (oProyecto.polizaAnticipo != null) { model.polizaAnticipo = true; } else { model.polizaAnticipo = false; }
                    if (oProyecto.polizaVicios != null) { model.polizaVicios = true; } else { model.polizaVicios = false; }
                    if (oProyecto.policaCumplimiento != null) { model.polizaCumplimiento = true; } else { model.polizaCumplimiento = false; }
                    model.NpolizaAnticipo = oProyecto.polizaAnticipo;
                    model.NpolizaCumplimiento = oProyecto.policaCumplimiento;
                    model.NpolizaVicios = oProyecto.polizaVicios;


                    var aux = (DateTime)oProyecto.fechaContrato;
                    model.fechaContrato = Convert.ToDateTime(aux.ToString("dd/MM/yyyy"));

                    aux = (DateTime)oProyecto.fechaInicioContrato;
                    model.fechaInicioContrato = Convert.ToDateTime(aux.ToString("dd/MM/yyyy"));

                    aux = (DateTime)oProyecto.fechaFinalContrato;
                    model.fechaFinalContrato = Convert.ToDateTime(aux.ToString("dd/MM/yyyy"));

                    aux = (DateTime)oProyecto.fechaInicioReal;
                    model.fechaInicioReal = Convert.ToDateTime(aux.ToString("dd/MM/yyyy"));

                    aux = (DateTime)oProyecto.fechaFinalReal;
                    model.fechaFinalReal = Convert.ToDateTime(aux.ToString("dd/MM/yyyy"));


                    return View(model);
                }
            }
            catch (Exception)
            {
                return RedirectToAction("MenuProyectos");
            }
        }

        public ActionResult BorrarDocumentos(int idProyecto, string[] array)
        {
            try
            {
                using (var db = new sgopEntities())
                {
                    BorrarArchivo oBorrar = new BorrarArchivo();
                    var oProyecto = db.proyectos.Find(idProyecto);
                    foreach (var i in array)
                    {
                        if (i == oProyecto.polizaVicios)
                        {
                            int x = i.IndexOf(".");
                            string temp = i.Substring(x);
                            string path = @"~\Res\Documentos\"; //Cambiar Ruta
                            path += oProyecto.idLicitacion + "-02" + temp;
                            oBorrar.borrar(path);
                            oProyecto.polizaVicios = null;
                        }
                        if (i == oProyecto.polizaAnticipo)
                        {
                            int x = i.IndexOf(".");
                            string temp = i.Substring(x);
                            string path = @"~\Res\Documentos\";
                            path += oProyecto.idLicitacion + "-01" + temp;
                            oBorrar.borrar(path);
                            oProyecto.polizaAnticipo = null;
                        }
                        if (i == oProyecto.policaCumplimiento)
                        {
                            int x = i.IndexOf(".");
                            string temp = i.Substring(x);
                            string path = @"~\Res\Documentos\";
                            path += oProyecto.idLicitacion + "-03" + temp;
                            oBorrar.borrar(path);
                            oProyecto.policaCumplimiento = null;
                        }

                    }
                    db.Entry(oProyecto).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                return Json(new { a = true, b = "Se eliminaron los documentos exitosamente!." });
            }
            catch (Exception)
            {
                return Json(new { a = false, b = "Ocurrio un error, al borrar." });
            }

        }
    }
}