using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tuturno.Models
{
    public class modelPrincipal
    {
        public List<Analistas> analist { get; set; }
        public List<AnalistasM> analistm { get; set;}
        public List<AnalistasC> analistasc { get;  set; }
        public List<Productos> productos { get; set; }
        public string nombre { get; set; }
        public DateTime fecha { get; set; }
    }
}