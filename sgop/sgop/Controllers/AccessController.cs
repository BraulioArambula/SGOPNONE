using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Net.Mail;
using sgop.Models.ViewModels;
using System.Web.Helpers;
using System.Text;
using System.IO;
using sgop.Models;
using System.Data.Entity;

namespace sgop.Controllers
{
    public class AccessController : Controller
    {

        public ActionResult Index() //Ya quedo
        {
            return View();
        }

        public ActionResult Enter(string user, string pass)
        {
            try
            {
                using (sgopEntities db = new sgopEntities())
                {
                    try
                    {
                        var con = db.usuarios.Where(aux => aux.usuario == user).First();

                        if (con.password.Equals(pass))
                        {
                            if (con.bloqueado == "1")
                            {
                                return Content("Usuario " + user.ToUpper() + " bloqueado.");
                            }
                            Session["Id"] = con.idUsuario;
                            Session["User"] = con.usuario;

                            return Json(new { a = true, b = con.idUsuario });
                        }
                        else
                        {
                            return Content("Usuario/Contraseña Incorrectos");
                        }
                    }
                    catch (Exception)
                    {
                        return Content("Ocurrio un error, intentalo mas tarde.");
                    }
                }
            }
            catch (Exception)
            {
                return Content("Ocurrio un error, intentalo mas tarde.");
            }
        } //Ya quedo

        public ActionResult Out()

        {

            Session.Clear();
            return RedirectToAction("Index");
        }//Ya quedo

        [HttpPost]
        public ActionResult Recuperar(string usuario, string email) //Ya quedo
        {
            using (sgopEntities db = new sgopEntities())
            {
                try
                {
                    var con = db.usuarios.Where(aux => aux.usuario == usuario).First();
                    if (con.correo.Equals(email))
                    {
                        MailMessage correo = new MailMessage();
                        correo.From = new MailAddress("oscar.ayala.soto.95@gmail.com");// Se va a cambiar el Correo
                        correo.To.Add(email); // Se agrega el email a donde se enviara el correo
                        correo.Subject = "Restablecer Contraseña"; //Asunto del correo
                        correo.Body = "Estimado/a <b>" + con.nombre + " " + con.apellidoPaterno + "</b>:<br/>" +  //Mensaje del Correo
                                       "Hace poco solicitaste restablecer la contraseña de tu usuario <b>" + usuario + "</b>." +
                                       "<br/>Tu actual contraseña es: <b>" + con.password + "</b>" +
                                       "<br/>Si no has solicitado este cambio o crees que alguien ha accedido a tu cuenta sin autorización, comunicate al siguiente numero 01 800 123 123 123." +
                                       "<br/><br/><b>Soporte técnico 2020</b>";

                        correo.IsBodyHtml = true;
                        correo.Priority = MailPriority.Normal;

                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "smtp.gmail.com";  //Configuraciones para  poder enviar el correo
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        smtp.UseDefaultCredentials = true;
                        string sCuenta = "oscar.ayala.soto.95@gmail.com"; // Se va a cambiar el Correo
                        string sPass = "paradise66";  // Contraseña del correo a utilizar
                        smtp.Credentials = new System.Net.NetworkCredential(sCuenta, sPass);
                        smtp.Send(correo); // Se envia el correo

                        ViewBag.message = "Estaremos comunicandonos a su correo para restaurar su contraseña.";
                        ViewBag.status = "Gracias!";
                    }
                }
                catch (Exception)
                {
                    ViewBag.message = "Ocurrio un problema, Intentelo mas tarde.";
                    ViewBag.status = "Error!";
                }
            }
            return View("Index");
        }


















    }
}