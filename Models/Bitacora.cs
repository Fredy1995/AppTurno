//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Tuturno.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Bitacora
    {
        public int idUser { get; set; }
        public string usuario { get; set; }
        public string direccionIP { get; set; }
        public string hostname { get; set; }
        public Nullable<System.DateTime> fechaIngreso { get; set; }
        public Nullable<int> VisitasAlDia { get; set; }
    }
}
