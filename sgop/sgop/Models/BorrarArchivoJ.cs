using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace sgop.Models
{
    public class BorrarArchivoJ

    {

        public bool confirmacion { get; set; }
        public void borrar(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                this.confirmacion = true;
            }
            else
            {
                this.confirmacion = false;

            }
        }


    }
}