using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace sgop.Models
{
    public class SubirDocs
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

            }
        }

    }
}