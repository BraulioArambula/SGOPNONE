using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sgop.Models
{
    public class SubirImagen
    {
        public bool confirmacion { get; set; }
        public void Subir(string path, HttpPostedFileBase imagen)
        {
            try
            {
                imagen.SaveAs(path);
                this.confirmacion = true;
            }
            catch (Exception)
            {
                this.confirmacion = false;
            
            }
        }

    }


}