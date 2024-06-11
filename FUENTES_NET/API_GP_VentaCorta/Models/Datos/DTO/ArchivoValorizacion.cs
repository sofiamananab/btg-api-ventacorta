using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_GP_VentaCorta.Models.Datos.DTO
{
    public class ArchivoValorizacion
    {
        public decimal? ID_VALORIZACION { get; set; }
        public string ID_USUARIO { get; set; }
        public DateTime? FEC_CARGA { get; set; }
        public DateTime? FEC_PROCESO { get; set; }
        public string NOM_MERCADO { get; set; }
        public decimal? MTO_BASE { get; set; }
        public decimal? MTO_VENTACORTA { get; set; }
        public decimal? MTO_GTIASIMULTANEA { get; set; }
        public decimal? MTO_FONDOGTIA { get; set; }
        public decimal? MTO_GTIACORRIENTE { get; set; }
        public string NOM_NEMOTECNICO { get; set; }
        public string IND_ESTADO { get; set; }
        public decimal? TOTAL_FILAS { get; set; }

    }
}