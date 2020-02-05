using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sgop.Models.ViewModels;
using sgop.Models;

namespace sgop.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AltaUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AltaUser(UserViewModel model, HttpPostedFileBase imagen)
        {

            SubirImagen oSubir = new SubirImagen();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                var nameImg = "";
                if (imagen != null)
                {
                    int i = imagen.FileName.IndexOf(".");
                    string temp = imagen.FileName.Substring(i);
                    string path = Server.MapPath("~/Res/img/fotos/");
                    path += Session["User"].ToString() + temp;
                    oSubir.Subir(path, imagen);
                    nameImg = imagen.FileName;
                }
                else
                {
                    nameImg = "default.png";
                }

                using (var db = new sgopEntities())
                {
                    usuarios oUser = new usuarios();
                    oUser.usuario = model.usuario;
                    oUser.password = model.password;
                    oUser.nombre = model.nombre;
                    oUser.apellidoMaterno = model.apellidoMaterno;
                    oUser.apellidoPaterno = model.apellidoPaterno;
                    oUser.correo = model.correo;
                    oUser.imagenPerfil = nameImg;
                    oUser.bloqueado = "0";
                    oUser.fechaCreacion = DateTime.Today;
                    oUser.usuarioCreacion = Session["User"].ToString();
                    db.usuarios.Add(oUser);
                    db.SaveChanges();

                    var con = db.usuarios.Where(aux => aux.usuario == model.usuario).First();
                    relacionSistemasRoles oRol = new relacionSistemasRoles();
                    if (model.Constructora != 0)
                    {
                        oRol.idUsuario = con.idUsuario;
                        oRol.idSistema = 1;
                        oRol.idRol = model.Constructora;
                        db.relacionSistemasRoles.Add(oRol);
                        db.SaveChanges();
                    }
                    if (model.Comercializadora != 0)
                    {
                        oRol.idUsuario = con.idUsuario;
                        oRol.idSistema = 2;
                        oRol.idRol = model.Comercializadora;
                        db.relacionSistemasRoles.Add(oRol);
                        db.SaveChanges();
                    }
                    if (model.Inmobiliaria != 0)
                    {
                        oRol.idUsuario = con.idUsuario;
                        oRol.idSistema = 3;
                        oRol.idRol = model.Inmobiliaria;
                        db.relacionSistemasRoles.Add(oRol);
                        db.SaveChanges();
                    }
                }
                if (oSubir.confirmacion != true)
                {
                    ViewBag.message = "Ocurrio un error al cargar la imagen.";
                    ViewBag.status = "Error!";
                }
                else
                {
                    ViewBag.message = "Se agrego el usuario exitosamente!";
                    ViewBag.status = "Exito!";
                }

                return View();

            }

        }

        public ActionResult MostrarImg(string user) //Ya quedo  - Es para mostrar la imagen del usuario.
        {
            try
            {
                using (sgopEntities db = new sgopEntities())
                {
                    var con = db.usuarios.Where(aux => aux.usuario == user).First();
                    return Redirect("~/Res/img/fotos/" + con.imagenPerfil);
                }
            }
            catch (Exception)
            {
                return Redirect("~/Res/img/fotos/default.png");
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
                        return Json(new { a = true });
                    }
                    catch (Exception)
                    {
                        return Json(new { a = false, b = "Usuario incorrecto" });

                    }
                }
            }
            catch (Exception)
            {
                return Json(new { a = false, b = "Ocurrio un error, intentalo mas tarde." });
            }
        }

        [HttpPost]
        public ActionResult BuscarLikeId(string user) //Ya quedo
        {
            string[] aux = new string[4];
            try
            {
                using (sgopEntities db = new sgopEntities())
                {
                    try
                    {
                        var con = db.usuarios.Where(d => d.usuario.Contains(user)).First();
                        return Json(new { a = con.usuario, b = con.nombre + " " + con.apellidoPaterno + " " + con.apellidoMaterno });

                    }
                    catch (Exception)
                    {

                        return Content("");
                    }
                }
            }
            catch (Exception)
            {

                return Content("");
            }
        }

        [HttpPost]
        public ActionResult Cambiar_Status(string user)
        {
            try
            {
                using (sgopEntities db = new sgopEntities())
                {
                    try
                    {
                        var oUser = db.usuarios.Where(d => d.usuario == user).First();
                        if (oUser.bloqueado == "1")
                        {
                            oUser.bloqueado = "0";
                            db.Entry(oUser).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();

                            return Json(new { a = true, b = "El Usuario ha sido desbloqueado." });
                        }
                        else
                        {
                            oUser.bloqueado = "1";
                            db.Entry(oUser).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            return Json(new { a = false, b = "Se ha bloqueado al usuario " + user.ToUpper() + "." });
                        }
                    }
                    catch (Exception)
                    {
                        return Json(new { a = false, b = "Hubo un error, intentelo mas tarde." });
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { a = false, b = "Hubo un error, intentelo mas tarde." });
            }

        } //Ya quedo

        [HttpPost]
        public ActionResult Cambiar_Pass(string user, string pass, string cpass)
        {
            try
            {
                using (sgopEntities db = new sgopEntities())
                {
                    try
                    {
                        if (pass == cpass)
                        {
                            var oUser = db.usuarios.Where(d => d.usuario == user).First();
                            oUser.password = pass;

                            db.Entry(oUser).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            ViewBag.info = 1;
                            return Json(new { a = true, b = "La Contraseña se actualizo correctamente." });
                        }


                        return Json(new { a = false, b = "La Contraseña NO coincide." });
                    }
                    catch (Exception)
                    {


                        return Json(new { a = false, b = "Hubo un error, intentelo mas tarde." });
                    }
                }
            }
            catch (Exception)
            {


                return Json(new { a = false, b = "Hubo un error, intentelo mas tarde." });
            }

        } //Ya quedo




        [HttpPost]
        public ActionResult Modificar_User(string userX)
        {
            try
            {
                EditUserViewModel model = new EditUserViewModel();
                using (var db = new sgopEntities())
                {

                    var oUser = db.usuarios.Where(d => d.usuario == userX).First();
                    model.idUser = oUser.idUsuario;
                    model.nombre = oUser.nombre;
                    model.usuario = oUser.usuario;
                    model.apellidoPaterno = oUser.apellidoPaterno;
                    model.apellidoMaterno = oUser.apellidoMaterno;
                    model.imagenPerfil = oUser.imagenPerfil;
                    model.correo = oUser.correo;
                    try
                    {
                        var oRol = db.relacionSistemasRoles.Where(d => d.idUsuario == oUser.idUsuario && d.idSistema == 1).First();
                        model.Constructora = (int)oRol.idRol;

                        oRol = db.relacionSistemasRoles.Where(d => d.idUsuario == oUser.idUsuario && d.idSistema == 2).First();
                        model.Comercializadora = (int)oRol.idRol;

                        oRol = db.relacionSistemasRoles.Where(d => d.idUsuario == oUser.idUsuario && d.idSistema == 3).First();
                        model.Inmobiliaria = (int)oRol.idRol;
                    }
                    catch (Exception)
                    {
                        model.Constructora = 0;
                        model.Constructora = 0;
                        model.Inmobiliaria = 0;
                    }
                }
                return View(model);

            }
            catch (Exception)
            {
                return View("Index");
            }

        }

        [HttpPost]
        public ActionResult ModificarUser(EditUserViewModel model, HttpPostedFileBase imagen)
        {

            if (!ModelState.IsValid)
            {
                return View("Modificar_User", model);
            }

            using (var db = new sgopEntities())
            {
                var oUser = db.usuarios.Find(model.idUser);
                oUser.usuario = model.usuario;
                oUser.nombre = model.nombre;
                oUser.apellidoPaterno = model.apellidoPaterno;
                oUser.apellidoMaterno = model.apellidoMaterno;
                oUser.correo = model.correo;
                oUser.usuarioModificacion = Session["User"].ToString();
                oUser.fechaModificacion = DateTime.Today;

                if (model.password != null && model.password.Trim() != "")
                {
                    oUser.password = model.password;
                }

                if (imagen != null)
                {
                    int i; string temp;
                    if (oUser.imagenPerfil != "default.png")
                    {
                        BorrarArchivo oBorrar = new BorrarArchivo();
                        i = oUser.imagenPerfil.IndexOf(".");
                        temp = oUser.imagenPerfil.Substring(i);
                        string path0 = @"~\Res\img\fotos";
                        path0 += Session["User"].ToString() + temp;
                        oBorrar.borrar(path0);
                    }

                    i = imagen.FileName.IndexOf(".");
                    temp = imagen.FileName.Substring(i);
                    SubirImagen oSubir = new SubirImagen();
                    string path = Server.MapPath("~/Res/img/fotos/");
                    path += Session["User"].ToString() + temp;
                    oSubir.Subir(path, imagen);
                    oUser.imagenPerfil = imagen.FileName;
                }
                else
                {
                    oUser.imagenPerfil = model.imagenPerfil;
                }

                db.Entry(oUser).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                relacionSistemasRoles oRol = new relacionSistemasRoles();
                if (model.Constructora != 0)
                {

                    var temp = db.relacionSistemasRoles.AsNoTracking().Where(d => d.idUsuario == model.idUser && d.idSistema == 1).FirstOrDefault();
                    oRol.idRelacion = temp.idRelacion;
                    oRol.idUsuario = model.idUser;
                    oRol.idSistema = 1;
                    oRol.idRol = model.Constructora;
                    db.Entry(oRol).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                }
                relacionSistemasRoles oRol1 = new relacionSistemasRoles();
                if (model.Comercializadora != 0)
                {
                    var temp = db.relacionSistemasRoles.AsNoTracking().Where(d => d.idUsuario == model.idUser && d.idSistema == 2).FirstOrDefault();
                    oRol1.idRelacion = temp.idRelacion;
                    oRol1.idUsuario = model.idUser;
                    oRol1.idSistema = 2;
                    oRol1.idRol = model.Comercializadora;
                    db.Entry(oRol1).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                relacionSistemasRoles oRol2 = new relacionSistemasRoles();
                if (model.Inmobiliaria != 0)
                {
                    var temp = db.relacionSistemasRoles.AsNoTracking().Where(d => d.idUsuario == model.idUser && d.idSistema == 3).FirstOrDefault();
                    oRol2.idRelacion = temp.idRelacion;
                    oRol2.idUsuario = model.idUser;
                    oRol2.idSistema = 3;
                    oRol2.idRol = model.Inmobiliaria;
                    db.Entry(oRol2).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                }

            }
            ViewBag.success = "Se actualizaron los datos exitosamente.";
            return View("Modificar_User", model);
        }


        [HttpPost]
        public ActionResult VisualizarUser(string userN)
        {

            UserViewModel model = new UserViewModel();
            using (var db = new sgopEntities())
            {
                var oUser = db.usuarios.Where(d => d.usuario == userN).First();
                model.idUser = oUser.idUsuario;
                model.nombre = oUser.nombre;
                model.usuario = oUser.usuario;
                model.apellidoPaterno = oUser.apellidoPaterno;
                model.apellidoMaterno = oUser.apellidoMaterno;
                model.imagenPerfil = oUser.imagenPerfil;
                model.correo = oUser.correo;

                try
                {
                    var oRol = db.relacionSistemasRoles.Where(d => d.idUsuario == oUser.idUsuario && d.idSistema == 1).First();
                    model.Constructora = (int)oRol.idRol;

                    oRol = db.relacionSistemasRoles.Where(d => d.idUsuario == oUser.idUsuario && d.idSistema == 2).First();
                    model.Comercializadora = (int)oRol.idRol;

                    oRol = db.relacionSistemasRoles.Where(d => d.idUsuario == oUser.idUsuario && d.idSistema == 3).First();
                    model.Inmobiliaria = (int)oRol.idRol;
                }
                catch (Exception)
                {
                    model.Constructora = 0;
                    model.Constructora = 0;
                    model.Inmobiliaria = 0;
                }
            }
            return View(model);


        } //Ya quedo

    }
}