using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_GP_VentaCorta.Models.DTO
{
    public class ParametersDTO
    {
        public decimal IdOperacion { get; set; }

        public decimal NumPagina { get; set; }
        public decimal NumFilas { get; set; }
        public string Filtro { get; set; }
        public decimal IdentPag { get; set; }
        public string Orderby { get; set; }


        public string StartDate { get; set; }

        public string EndDate { get; set; }
        public int idClasificacion { get; set; }
        public int idTipMovto { get; set; }
        public string configuracion1 { get; set; }
        public string configuracion2 { get; set; }
        public string configuracion3 { get; set; }
        public string configuracion4 { get; set; }
        public string configuracion5 { get; set; }


        public string contenidoBulk { get; set; }
        public string idUsuario { get; set; }

        public int enPagina { get; set; }
        public int numFilas { get; set; }


        public long ClientId { get; set; }
        public long AccountId { get; set; }
        public long AccountId2 { get; set; }
        public string NamePeriod { get; set; }
        public string Filter { get; set; }

        public int EnConf { get; set; }
        public int conInstrumentos { get; set; }

        public bool IsExport { get; set; }

    }
}