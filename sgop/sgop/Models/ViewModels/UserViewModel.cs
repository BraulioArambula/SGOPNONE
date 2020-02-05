using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace sgop.Models.ViewModels
{
    public class UserViewModel
    {
        [Display(Name = "Usuario")]
        [Required]
        public string usuario { get; set; }

        [Display(Name = "Contraseña")]
        [Required]
        public string password { get; set; }


        [Display(Name = "Confirmar Contraseña")]
        [Compare("password", ErrorMessage = "La contraseña no coincide.")]
        public string ConfirmPass { get; set; }
        [Display(Name = "Nombre")]
        [Required]
        public string nombre { get; set; }
        [Display(Name = "Apellido Paterno")]
        [Required]
        public string apellidoPaterno { get; set; }

        [Display(Name = "Apellido Materno")]
        [Required]
        public string apellidoMaterno { get; set; }

        [Display(Name = "Correo")]
        [EmailAddress]
        [Required]
        public string correo { get; set; }

        [Display(Name = "Imagen")]
        public string imagenPerfil { get; set; }

        public int idUser { get; set; }
        public string idRol { get; set; }

        public string idSistema { get; set; }

        public string bloqueado { get; set; }
        public string primerAccesso { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string usuarioCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public string usuarioModificacion { get; set; }

        [Required]
        public int Comercializadora { get; set; }
        [Required]
        public int Inmobiliaria { get; set; }
        [Required]
        public int Constructora { get; set; }


    }

    public class EditUserViewModel
    {

        [Display(Name = "Usuario")]
        [Required]
        public string usuario { get; set; }

        [Display(Name = "Contraseña")]
        public string password { get; set; }


        [Display(Name = "Confirmar Contraseña")]
        [Compare("password", ErrorMessage = "La contraseña no coincide.")]
        public string ConfirmPass { get; set; }
        [Display(Name = "Nombre")]
        [Required]
        public string nombre { get; set; }
        [Display(Name = "Apellido Paterno")]
        [Required]
        public string apellidoPaterno { get; set; }

        [Display(Name = "Apellido Materno")]
        [Required]
        public string apellidoMaterno { get; set; }

        [Display(Name = "Correo")]
        [EmailAddress]
        [Required]
        public string correo { get; set; }

        [Display(Name = "Imagen")]
        public string imagenPerfil { get; set; }

        public int idUser { get; set; }
        public string idRol { get; set; }

        public string idSistema { get; set; }

        public string bloqueado { get; set; }
        public string primerAccesso { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string usuarioCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public string usuarioModificacion { get; set; }

        [Required]
        public int Comercializadora { get; set; }
        [Required]
        public int Inmobiliaria { get; set; }
        [Required]
        public int Constructora { get; set; }


    }

}