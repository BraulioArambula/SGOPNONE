using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Estimacion.Models
{
    public class SubirDocViewModel
    {
        public bool confirmacion { get; set; }

        public void Subir(string path, HttpPostedFileBase doc)
        {
            try
            {
                doc.SaveAs(path);
                this.confirmacion = true;
            }
            catch (Exception)
            {
                this.confirmacion = false;
                throw;
            }
        }
    }
}