using Newtonsoft.Json.Linq;
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
    public class LicitacionesController : Controller
    {
        // GET: Licitaciones
        public ActionResult Index()
        {
            ViewBag.Message = "Your application description page.";
            //creamos una lista de nuestra clase Licitaciones y Municipios
            List<ListarLicitaciones> list = null;
            List<ListarMunicipio> listMunicipios = null;
            List<ListarEmpresa> listarEmpresas = null;
            List<ListarEstatus> listarEstatus = null;
            using (sgopEntities bd = new sgopEntities())
            {

                //hacemos un select a nuestra tabla con los campos que queremos mostrar
                list = (from b in bd.licitaciones
                        select new ListarLicitaciones
                        {
                            idLicitacion = b.idLicitacion,
                            noLicitacion = b.noLicitacion,
                            nombreObra = b.nombreObra,
                            localidad = b.localidad,
                            fechaVisita = b.fechaVisita,
                            fechaAclaraciones = b.fechaAclaraciones,
                            fechaFallo = b.fechaFallo,
                            idMunicipio = b.idMunicipio,
                            idEstatus = b.idEstatus,
                            idEmpresa = b.idEmpresa,
                            fechaCreacion = b.fechaCreacion,
                            //DEscripcion
                            descripcion_empresa = b.catalogoEmpresas.razonSocial,
                            descripcion_estatus = b.catalogoEstatus.descripcion,
                            descripcion_muni = b.catalogoMunicipios.descripcion,


                        }).ToList();
                //********************************************************************
                listMunicipios = (from b in bd.catalogoMunicipios
                                  select new ListarMunicipio
                                  {
                                      idMunicipio = b.idMunicipio,
                                      descripcion = b.descripcion,

                                  }).ToList();
                //*************************************************************************
                listarEmpresas = (from b in bd.catalogoEmpresas
                                  select new ListarEmpresa
                                  {
                                      idEmpresa = b.idEmpresa,
                                      razonSocial = b.razonSocial,

                                  }).ToList();
                //************************************************************************
                listarEstatus = (from b in bd.catalogoEstatus
                                 where b.perteneceA == 0


                                 select new ListarEstatus
                                 {
                                     idEstatus = b.idEstatus,
                                     descripcion = b.descripcion,

                                 }).ToList();
                //************************************************************************

                ViewBag.listarEmpresas = listarEmpresas;
                ViewBag.listMunicipios = listMunicipios;
                ViewBag.listarEstatus = listarEstatus;
                return View(list);
            }
        }// Para listar las Licitaciones

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult crearLicitacion(FormCollection fc)
        {
            Rangos rangos = new Rangos();
            int idLicitacion = rangos.getSiguienteID("LICITACIONES");
            string noLicitacion = fc["noLicitacion"];
            int idMunicipio = Convert.ToInt32(fc["idMunicipio"]);
            string localidad = fc["localidad"];
            string nombreObra = fc["nombreObra"];
            string fechaVisita = fc["fechaVisita"].ToString();
            string fechaAclaraciones = fc["fechaAclaraciones"];
            string fechaPropuesta = fc["fechaPropuesta"];
            string fechaFallo = fc["fechaFallo"];
            int idEmpresa = Convert.ToInt32(fc["idEmpresa"]);

            //Conectamos  BD para Llenar Table "Licitaciones"
            using (sgopEntities bd = new sgopEntities())
            {  //Formulario

                licitaciones tlicitaciones = new licitaciones();
                tlicitaciones.idLicitacion = idLicitacion;
                tlicitaciones.noLicitacion = noLicitacion;
                tlicitaciones.idMunicipio = idMunicipio;
                tlicitaciones.localidad = localidad;
                tlicitaciones.nombreObra = nombreObra;
                tlicitaciones.idEmpresa = idEmpresa;

                //Convertir Stringo TO date
                string iDate = fechaVisita;
                DateTime oDate = Convert.ToDateTime(iDate);
                string iDate1 = fechaAclaraciones;
                DateTime oDate1 = Convert.ToDateTime(iDate1);
                string iDate2 = fechaPropuesta;
                DateTime oDate2 = Convert.ToDateTime(iDate2);
                string iDate3 = fechaFallo;
                DateTime oDate3 = Convert.ToDateTime(iDate3);
                // Sustituimos la conversion 
                tlicitaciones.fechaVisita = oDate;
                tlicitaciones.fechaAclaraciones = oDate1;
                tlicitaciones.fechaPropuesta = oDate2;
                tlicitaciones.fechaFallo = oDate3;
                //Las opciones que no estan en formulario 
                tlicitaciones.fechaModificacion = DateTime.Now;

                tlicitaciones.idEstatus = 1;
                tlicitaciones.actaVisita = "";
                tlicitaciones.actaAclaraciones = "";
                tlicitaciones.actaPropuesta = "";
                tlicitaciones.actaFallo = "";
                tlicitaciones.idRequisicion = 0;
                tlicitaciones.fechaCreacion = DateTime.Now;
                tlicitaciones.usuarioCreacion = 1;
                tlicitaciones.usuarioModificacion = 1;

                bd.licitaciones.Add(tlicitaciones);
                bd.SaveChanges();
                bd.Dispose();
                return Json(new { a = true, b = "Guardado Con Exito" });
            }
        } //Mandar ah llenar tabla licitaciones

        [HttpPost]
        public ActionResult Editar_Licitacion(FormCollection fc)
        {
            int Id_Licitacion = Convert.ToInt32(fc["id_Licitacion"]);
            double? totalPropuesta = 0;

            EditarLicitaciones model = new EditarLicitaciones();
            DateTime date = DateTime.Now;
            List<ListarMunicipio> listMunicipios = null;
            List<ListarEmpresa> listarEmpresas = null;
            List<ListarEstatus> listarEstatus = null;
            List<EditarLicitaciones> listarRequi = null;

            using (var db = new sgopEntities())
            {

                var olicitacion = db.licitaciones.Find(Id_Licitacion);

                model.idLicitacion = olicitacion.idLicitacion;
                model.noLicitacion = olicitacion.noLicitacion;
                model.nombreObra = olicitacion.nombreObra;
                model.idMunicipio = olicitacion.idMunicipio;


                var municipio = db.catalogoMunicipios.Find(olicitacion.idMunicipio);
                model.descripcion_muni = municipio.descripcion;
                var estatus = db.catalogoEstatus.Find(olicitacion.idEstatus);
                model.descripcion_estatus = estatus.descripcion;
                var empresa = db.catalogoEmpresas.Find(olicitacion.idEmpresa);
                model.descripcion_empresa = empresa.razonSocial;
                model.idEmpresa = empresa.idEmpresa;

                //*************************CONSULTA DE REQUICICION********************************
                if (olicitacion.idRequisicion != 0)
                {
                    listarRequi = (from req in db.requisiciones
                                   where req.idRequisicionRango == olicitacion.idRequisicion
                                   select new EditarLicitaciones
                                   {
                                       idRequisicion = req.idRequisicion,
                                       total_requisicion = req.total
                                   }).ToList();

                    foreach (var item in listarRequi)
                    {
                        totalPropuesta += item.total_requisicion;
                    }
                }

                //*********************************************************

                model.localidad = olicitacion.localidad;
                model.fechaVisita = olicitacion.fechaVisita;
                model.fechaAclaraciones = olicitacion.fechaAclaraciones;
                model.fechaPropuesta = olicitacion.fechaPropuesta;
                model.fechaFallo = olicitacion.fechaFallo;
                model.idEstatus = olicitacion.idEstatus;
                model.propuesta_licitacion = totalPropuesta.ToString();
                model.idRequisicion = olicitacion.idRequisicion;
                date = (DateTime)olicitacion.fechaVisita;
                model.fechaVisitaFomato = date.ToString("yyyy-MM-dd");

                date = (DateTime)olicitacion.fechaAclaraciones;
                model.fechaAclaracionesFormato = date.ToString("yyyy-MM-dd");

                date = (DateTime)olicitacion.fechaPropuesta;
                model.fechaPropuestaFormato = date.ToString("yyyy-MM-dd");

                date = (DateTime)olicitacion.fechaFallo;
                model.fechaFalloFormato = date.ToString("yyyy-MM-dd");

                model.actaVisita = olicitacion.actaVisita;
                model.actaAclaraciones = olicitacion.actaAclaraciones;
                model.actaPropuesta = olicitacion.actaPropuesta;
                model.actaFallo = olicitacion.actaFallo;




                //********************************************************************
                listMunicipios = (from b in db.catalogoMunicipios
                                  select new ListarMunicipio
                                  {
                                      idMunicipio = b.idMunicipio,
                                      descripcion = b.descripcion,

                                  }).ToList();
                //*************************************************************************
                listarEmpresas = (from b in db.catalogoEmpresas
                                  select new ListarEmpresa
                                  {
                                      idEmpresa = b.idEmpresa,
                                      razonSocial = b.razonSocial,

                                  }).ToList();
                //*************************************************************************
                //************************************************************************
                if (!model.actaFallo.Equals("") && !model.actaPropuesta.Equals("") && !model.actaVisita.Equals("") && !model.actaAclaraciones.Equals(""))
                {
                    listarEstatus = (from b in db.catalogoEstatus
                                     where b.perteneceA == 0
                                     select new ListarEstatus
                                     {
                                         idEstatus = b.idEstatus,
                                         descripcion = b.descripcion,

                                     }).ToList();
                }
                else
                {
                    listarEstatus = (from b in db.catalogoEstatus
                                     where b.perteneceA == 0 && b.idEstatus != 2
                                     select new ListarEstatus
                                     {

                                         idEstatus = b.idEstatus,
                                         descripcion = b.descripcion,

                                     }).ToList();
                }

                //************************************************************************
                //************************************************************************
            }

            ViewBag.listMunicipios = listMunicipios;
            ViewBag.listarEmpresas = listarEmpresas;
            ViewBag.listarEstatus = listarEstatus;
            return View(model);
        } // EDITAR LICITACION

        [HttpPost]
        public ActionResult Guarda_Licitacion(FormCollection fc)
        {
            int idLicitacion = Convert.ToInt32(fc["idLicitacion"]);

            string noLicitacion = fc["noLicitacion"];
            int idMunicipio = Convert.ToInt32(fc["idMunicipio"]);
            string localidad = fc["localidad"];
            //string nombreObra        = fc["nombreObra"];
            string fechaVisita = fc["fechaVisita"];
            string fechaAclaraciones = fc["fechaAclaraciones"];
            int idEmpresa = Convert.ToInt32(fc["idEmpresa"]);
            int idEstatus = Convert.ToInt32(fc["idEstatus"]);
            string fechaPropuesta = fc["fechaPropuesta"];
            string fechaFallo = fc["fechaFallo"];

            //Conectamos  BD para Llenar Table "Licitaciones"
            using (sgopEntities bd = new sgopEntities())
            {  //Formulario
                var tlicitaciones = bd.licitaciones.Find(idLicitacion);
                tlicitaciones.noLicitacion = noLicitacion;
                tlicitaciones.idMunicipio = idMunicipio;
                tlicitaciones.localidad = localidad;
                //tlicitaciones.nombreObra   = nombreObra;
                tlicitaciones.idEmpresa = idEmpresa;
                tlicitaciones.idEstatus = idEstatus;

                //Convertir Stringo TO date
                string iDate = fechaVisita;
                DateTime oDate = Convert.ToDateTime(iDate);
                string iDate1 = fechaAclaraciones;
                DateTime oDate1 = Convert.ToDateTime(iDate1);
                string iDate2 = fechaPropuesta;
                DateTime oDate2 = Convert.ToDateTime(iDate2);
                string iDate3 = fechaFallo;
                DateTime oDate3 = Convert.ToDateTime(iDate3);

                // Sustituimos la conversion de fechas de formulario
                tlicitaciones.fechaVisita = oDate;
                tlicitaciones.fechaAclaraciones = oDate1;
                tlicitaciones.fechaPropuesta = oDate2;
                tlicitaciones.fechaFallo = oDate3;

                //Las opciones que no estan en formulario 
                tlicitaciones.fechaModificacion = DateTime.Now;
                //* tlicitaciones.fechaPropuesta = DateTime.Now;
                //tlicitaciones.fechaFallo = DateTime.Now;

                ///bd.licitaciones.Add(tlicitaciones);
                bd.Entry(tlicitaciones).State = System.Data.Entity.EntityState.Modified;
                bd.SaveChanges();
                bd.Dispose();

                return Json(new { a = true, b = "Guardado Con Exito" });

            }
            //-------------------------------------------------------
        }//recibe el FORMULARIO y confirma la modificacion

        public ActionResult Carga_Doc(HttpPostedFileBase doc1, HttpPostedFileBase doc2, HttpPostedFileBase doc3, HttpPostedFileBase doc4, FormCollection fc)
        {
            int idLicitacion = Convert.ToInt32(fc["idLicitacion"]);
            var link1 = ""; var link3 = "";
            var link2 = ""; var link4 = "";

            SubirDoc subir = new SubirDoc();
            //Path para crear la direccion con el numero de la licitacion//
            string path1 = this.Server.MapPath("~/Res/docLicitacion/") + idLicitacion;
            DirectoryInfo dir = new DirectoryInfo(path1);


            if (dir.Exists)
            {
                // var a = 1 + 1;
                string path3 = this.Server.MapPath("~/Res/docLicitacion/") + idLicitacion + ("/ActaVisita");
                DirectoryInfo dir3 = new DirectoryInfo(path3);
                string path4 = this.Server.MapPath("~/Res/docLicitacion/") + idLicitacion + ("/ActaAclaraciones");
                DirectoryInfo dir4 = new DirectoryInfo(path4);
                string path5 = this.Server.MapPath("~/Res/docLicitacion/") + idLicitacion + ("/ActaPropuesta");
                DirectoryInfo dir5 = new DirectoryInfo(path5);
                string path6 = this.Server.MapPath("~/Res/docLicitacion/") + idLicitacion + ("/ActaFallo");
                DirectoryInfo dir6 = new DirectoryInfo(path6);

                if (dir3.Exists)
                {
                    if (doc1 != null)
                    {

                        //METE DOCUMENTO EN LA LICITACION//
                        string path = Server.MapPath("~/Res/docLicitacion/") + idLicitacion + ("/ActaVisita/");
                        path += doc1.FileName;
                        subir.Subir(path, doc1);
                        link1 = path;
                        bool bande = subir.confirmacion;
                        //******************************//
                    }

                }
                else
                {
                    dir3.Create();
                    if (doc1 != null)
                    {
                        //METE DOCUMENTO EN LA LICITACION//
                        string path = Server.MapPath("~/Res/docLicitacion/") + idLicitacion + ("/ActaVisita/");
                        path += doc1.FileName;
                        subir.Subir(path, doc1);
                        link1 = path;
                        bool bande = subir.confirmacion;
                        //******************************//
                    }
                }

                if (dir4.Exists)
                {
                    if (doc2 != null)
                    {
                        //METE DOCUMENTO EN LA LICITACION//
                        string path = Server.MapPath("~/Res/docLicitacion/") + idLicitacion + ("/ActaAclaraciones/");
                        path += doc2.FileName;
                        subir.Subir(path, doc2);
                        link2 = path;
                        bool bande = subir.confirmacion;
                        //******************************//
                    }
                }
                else
                {
                    dir4.Create();
                    if (doc2 != null)
                    {
                        //METE DOCUMENTO EN LA LICITACION//
                        string path = Server.MapPath("~/Res/docLicitacion/") + idLicitacion + ("/ActaAclaraciones/");
                        path += doc2.FileName;
                        subir.Subir(path, doc2);
                        link2 = path;
                        bool bande = subir.confirmacion;
                        //******************************//
                    }
                }

                if (dir5.Exists)
                {
                    if (doc3 != null)
                    {

                        //METE DOCUMENTO EN LA LICITACION//
                        string path = Server.MapPath("~/Res/docLicitacion/") + idLicitacion + ("/ActaPropuesta/");
                        path += doc3.FileName;
                        subir.Subir(path, doc3);
                        link3 = path;
                        bool bande = subir.confirmacion;
                        //******************************//
                    }
                }
                else
                {
                    dir5.Create();
                    if (doc3 != null)
                    {
                        //METE DOCUMENTO EN LA LICITACION//
                        string path = Server.MapPath("~/Res/docLicitacion/") + idLicitacion + ("/ActaPropuesta/");
                        path += doc3.FileName;
                        subir.Subir(path, doc3);
                        link3 = path;
                        bool bande = subir.confirmacion;
                        //******************************//
                    }
                }
                if (dir6.Exists)
                {
                    if (doc4 != null)
                    {
                        //METE DOCUMENTO EN LA LICITACION//
                        string path = Server.MapPath("~/Res/docLicitacion/") + idLicitacion + ("/ActaFallo/");
                        path += doc4.FileName;
                        subir.Subir(path, doc4);
                        link4 = path;
                        bool bande = subir.confirmacion;
                        //******************************//
                    }

                }
                else
                {
                    dir6.Create();
                    if (doc4 != null)
                    {
                        //METE DOCUMENTO EN LA LICITACION//
                        string path = Server.MapPath("~/Res/docLicitacion/") + idLicitacion + ("/ActaFallo/");
                        path += doc4.FileName;
                        subir.Subir(path, doc4);
                        link4 = path;
                        bool bande = subir.confirmacion;
                        //******************************//
                    }

                }
            }
            else
            {
                // var n = 2 + 2;
                dir.Create();
                string path3 = this.Server.MapPath("~/Res/docLicitacion/") + idLicitacion + ("/ActaVisita");
                DirectoryInfo dir3 = new DirectoryInfo(path3);
                string path4 = this.Server.MapPath("~/Res/docLicitacion/") + idLicitacion + ("/ActaAclaraciones");
                DirectoryInfo dir4 = new DirectoryInfo(path4);
                string path5 = this.Server.MapPath("~/Res/docLicitacion/") + idLicitacion + ("/ActaPropuesta");
                DirectoryInfo dir5 = new DirectoryInfo(path5);
                string path6 = this.Server.MapPath("~/Res/docLicitacion/") + idLicitacion + ("/ActaFallo");
                DirectoryInfo dir6 = new DirectoryInfo(path6);
                if (dir3.Exists)
                {
                    if (doc1 != null)
                    {

                        //METE DOCUMENTO EN LA LICITACION//
                        string path = Server.MapPath("~/Res/docLicitacion/") + idLicitacion + ("/ActaVisita/");
                        path += doc1.FileName;
                        subir.Subir(path, doc1);
                        link1 = path;
                        bool bande = subir.confirmacion;
                        //******************************//
                    }

                }
                else
                {
                    dir3.Create();
                    if (doc1 != null)
                    {
                        //METE DOCUMENTO EN LA LICITACION//
                        string path = Server.MapPath("~/Res/docLicitacion/") + idLicitacion + ("/ActaVisita/");
                        path += doc1.FileName;
                        subir.Subir(path, doc1);
                        link1 = path;
                        bool bande = subir.confirmacion;
                        //******************************//
                    }
                }

                if (dir4.Exists)
                {
                    if (doc2 != null)
                    {
                        //METE DOCUMENTO EN LA LICITACION//
                        string path = Server.MapPath("~/Res/docLicitacion/") + idLicitacion + ("/ActaAclaraciones/");
                        path += doc2.FileName;
                        subir.Subir(path, doc2);
                        link2 = path;
                        bool bande = subir.confirmacion;
                        //******************************//
                    }
                }
                else
                {
                    dir4.Create();
                    if (doc2 != null)
                    {
                        //METE DOCUMENTO EN LA LICITACION//
                        string path = Server.MapPath("~/Res/docLicitacion/") + idLicitacion + ("/ActaAclaraciones/");
                        path += doc2.FileName;
                        subir.Subir(path, doc2);
                        link2 = path;
                        bool bande = subir.confirmacion;
                        //******************************//
                    }
                }

                if (dir5.Exists)
                {
                    if (doc3 != null)
                    {

                        //METE DOCUMENTO EN LA LICITACION//
                        string path = Server.MapPath("~/Res/docLicitacion/") + idLicitacion + ("/ActaPropuesta/");
                        path += doc3.FileName;
                        subir.Subir(path, doc3);
                        link3 = path;
                        bool bande = subir.confirmacion;
                        //******************************//
                    }
                }
                else
                {
                    dir5.Create();
                    if (doc3 != null)
                    {
                        //METE DOCUMENTO EN LA LICITACION//
                        string path = Server.MapPath("~/Res/docLicitacion/") + idLicitacion + ("/ActaPropuesta/");
                        path += doc3.FileName;
                        subir.Subir(path, doc3);
                        link3 = path;
                        bool bande = subir.confirmacion;
                        //******************************//
                    }
                }
                if (dir6.Exists)
                {
                    if (doc4 != null)
                    {
                        //METE DOCUMENTO EN LA LICITACION//
                        string path = Server.MapPath("~/Res/docLicitacion/") + idLicitacion + ("/ActaFallo/");
                        path += doc4.FileName;
                        subir.Subir(path, doc4);
                        link4 = path;
                        bool bande = subir.confirmacion;

                        //******************************//
                    }

                }
                else
                {
                    dir6.Create();
                    if (doc4 != null)
                    {
                        //METE DOCUMENTO EN LA LICITACION//
                        string path = Server.MapPath("~/Res/docLicitacion/") + idLicitacion + ("/ActaFallo/");
                        path += doc4.FileName;
                        subir.Subir(path, doc4);
                        link4 = path;
                        bool bande = subir.confirmacion;
                        //******************************//
                    }
                }
            }

            //**********************************************************************//
            using (var db = new sgopEntities())
            {
                var olicitacion = db.licitaciones.Find(idLicitacion);
                if (doc1 != null) { olicitacion.actaVisita = link1; }
                if (doc2 != null) { olicitacion.actaAclaraciones = link2; }
                if (doc3 != null) { olicitacion.actaPropuesta = link3; }
                if (doc3 != null) { olicitacion.actaFallo = link4; }

                db.Entry(olicitacion).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
            }



            //****************************BASE**************************************//
            return RedirectToAction("Index");
        }

        public ActionResult Borrar_Doc(FormCollection fc)
        {
            JArray json = JArray.Parse(fc[0].ToString());
            //int idLicitacion = Convert.ToInt32(fc["idLicitacion"]);
            //string id_licitacion = json[0]["id_licitacion"].ToString();
            int id_licitacion = Convert.ToInt32(json[0]["id_licitacion"].ToString());
            string visita = json[0]["visita"].ToString();
            string aclaracion = json[0]["aclaracion"].ToString();
            string propuesta = json[0]["propuesta"].ToString();
            string fallo = json[0]["fallo"].ToString();
            //****MSJ
            int bande = 0;
            //***
            string visitaBorra = "";
            string aclaracionBorra = "";
            string propuestaBorra = "";
            string falloBorra = "";
            //  SubirDoc subir = new SubirDoc();
            // subir.Subir(path, doc1);
            BorrarArchivoJ borrar = new BorrarArchivoJ();
            using (var db = new sgopEntities())
            {
                var olicitacion = db.licitaciones.Find(id_licitacion);

                if (visita.Equals("1"))
                {
                    visitaBorra = olicitacion.actaVisita;
                    borrar.borrar(visitaBorra);
                    olicitacion.actaVisita = "";
                    bande = 1;
                }
                if (aclaracion.Equals("1"))
                {
                    aclaracionBorra = olicitacion.actaAclaraciones;
                    borrar.borrar(aclaracionBorra);
                    olicitacion.actaAclaraciones = "";
                    bande = 1;
                }
                if (propuesta.Equals("1"))
                {
                    propuestaBorra = olicitacion.actaPropuesta;
                    borrar.borrar(propuestaBorra);
                    olicitacion.actaPropuesta = "";
                    bande = 1;
                }
                if (fallo.Equals("1"))
                {
                    falloBorra = olicitacion.actaFallo;
                    borrar.borrar(falloBorra);
                    olicitacion.actaFallo = "";
                    bande = 1;
                }

                db.Entry(olicitacion).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
            }

            if (bande == 0)
            {
                return Json(new { a = false, b = "Gu" });
            }
            else
            {
                return Json(new { a = true, b = "Gu" });
            }

            //return RedirectToAction("Index");
            // return Content("");
        }

        public ActionResult MenuLikeProyectos(string[][] array)
        {

            string noLicitacion = array[0][1];
            int Estatus = Int32.Parse(array[2][1].Substring(0, 1));
            int idMunicipio = -1;
            if (array[1][1] != "")
            {
                if (array[1][1].Substring(0, 1) == "0")
                {
                    idMunicipio = Int32.Parse(array[1][1].Substring(0, 1));
                }
                else
                {
                    idMunicipio = Int32.Parse(array[1][1].Substring(0));
                }
            }
            if (Estatus == 0) { Estatus = -1; }
            if (idMunicipio == 0) { idMunicipio = -1; }

            try
            {
                List<licitacionesViewModel> lst = null;
                using (sgopEntities db = new sgopEntities())
                {
                    try
                    {

                        lst = (from l in db.licitaciones
                               join id in db.catalogoEmpresas on l.idEmpresa equals id.idEmpresa
                               join mun in db.catalogoMunicipios on l.idMunicipio equals mun.idMunicipio
                               join e in db.catalogoEstatus on l.idEstatus equals e.idEstatus
                               where (l.noLicitacion.Contains(noLicitacion) || l.nombreObra.Contains(noLicitacion)) && (Estatus != -1 ? l.idEstatus == Estatus : true)
                               && (idMunicipio != -1 ? l.idMunicipio == idMunicipio : true)

                               select new licitacionesViewModel
                               {

                                   idLicitacion = (int)l.idLicitacion,
                                   empresa = id.razonSocial,

                                   nombreObra = l.nombreObra,
                                   noLicitacion = l.noLicitacion,
                                   municipio = mun.descripcion,
                                   estatus = e.descripcion,
                                   localidad = l.localidad,
                                   fecha = (DateTime)l.fechaCreacion,


                               }).ToList();
                        foreach (var i in lst)
                        {

                            i.fechaCreacion = i.fecha.ToString("yyyy-MM-dd");
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

        [HttpPost]
        public ActionResult Visualizar_Licitacion(FormCollection fc)
        {
            int Id_Licitacion = Convert.ToInt32(fc["id_Licitacion"]);
            double? totalPropuesta = 0;

            EditarLicitaciones model = new EditarLicitaciones();
            DateTime date = DateTime.Now;
            List<ListarMunicipio> listMunicipios = null;
            List<ListarEmpresa> listarEmpresas = null;
            List<ListarEstatus> listarEstatus = null;
            List<EditarLicitaciones> listarRequi = null;

            using (var db = new sgopEntities())
            {

                var olicitacion = db.licitaciones.Find(Id_Licitacion);

                model.idLicitacion = olicitacion.idLicitacion;
                model.noLicitacion = olicitacion.noLicitacion;
                model.nombreObra = olicitacion.nombreObra;
                model.idMunicipio = olicitacion.idMunicipio;


                var municipio = db.catalogoMunicipios.Find(olicitacion.idMunicipio);
                model.descripcion_muni = municipio.descripcion;
                var estatus = db.catalogoEstatus.Find(olicitacion.idEstatus);
                model.descripcion_estatus = estatus.descripcion;
                var empresa = db.catalogoEmpresas.Find(olicitacion.idEmpresa);
                model.descripcion_empresa = empresa.razonSocial;
                model.idEmpresa = empresa.idEmpresa;

                //*************************CONSULTA DE REQUICICION********************************
                if (olicitacion.idRequisicion != 0)
                {
                    listarRequi = (from req in db.requisiciones
                                   where req.idRequisicionRango == olicitacion.idRequisicion
                                   select new EditarLicitaciones
                                   {
                                       idRequisicion = req.idRequisicion,
                                       total_requisicion = req.total
                                   }).ToList();

                    foreach (var item in listarRequi)
                    {
                        totalPropuesta += item.total_requisicion;
                    }
                }

                //*********************************************************

                model.localidad = olicitacion.localidad;
                model.fechaVisita = olicitacion.fechaVisita;
                model.fechaAclaraciones = olicitacion.fechaAclaraciones;
                model.fechaPropuesta = olicitacion.fechaPropuesta;
                model.fechaFallo = olicitacion.fechaFallo;
                model.idEstatus = olicitacion.idEstatus;
                model.propuesta_licitacion = totalPropuesta.ToString();
                model.idRequisicion = olicitacion.idRequisicion;
                date = (DateTime)olicitacion.fechaVisita;
                model.fechaVisitaFomato = date.ToString("yyyy-MM-dd");

                date = (DateTime)olicitacion.fechaAclaraciones;
                model.fechaAclaracionesFormato = date.ToString("yyyy-MM-dd");

                date = (DateTime)olicitacion.fechaPropuesta;
                model.fechaPropuestaFormato = date.ToString("yyyy-MM-dd");

                date = (DateTime)olicitacion.fechaFallo;
                model.fechaFalloFormato = date.ToString("yyyy-MM-dd");

                model.actaVisita = olicitacion.actaVisita;
                model.actaAclaraciones = olicitacion.actaAclaraciones;
                model.actaPropuesta = olicitacion.actaPropuesta;
                model.actaFallo = olicitacion.actaFallo;




                //********************************************************************
                listMunicipios = (from b in db.catalogoMunicipios
                                  select new ListarMunicipio
                                  {
                                      idMunicipio = b.idMunicipio,
                                      descripcion = b.descripcion,

                                  }).ToList();
                //*************************************************************************
                listarEmpresas = (from b in db.catalogoEmpresas
                                  select new ListarEmpresa
                                  {
                                      idEmpresa = b.idEmpresa,
                                      razonSocial = b.razonSocial,

                                  }).ToList();
                //************************************************************************
                listarEstatus = (from b in db.catalogoEstatus
                                 where b.perteneceA == 0
                                 select new ListarEstatus
                                 {
                                     idEstatus = b.idEstatus,
                                     descripcion = b.descripcion,

                                 }).ToList();
                //************************************************************************
            }

            ViewBag.listMunicipios = listMunicipios;
            ViewBag.listarEmpresas = listarEmpresas;
            ViewBag.listarEstatus = listarEstatus;
            return View(model);
        } // Visualizar LICITACION



    }
}