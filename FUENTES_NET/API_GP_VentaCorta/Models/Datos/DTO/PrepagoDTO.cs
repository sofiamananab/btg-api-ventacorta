using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_GP_VentaCorta.Models.Datos.DTO
{
    public class PrepagoDTO
    {
        public decimal ID_OPERACION { get; set; }
        public decimal SEC_PREPAGO { get; set; }
        public DateTime? FEC_CREACION { get; set; }
        public string NOM_USUARIO { get; set; }
        public decimal? MTO_CANTIDAD { get; set; }
        public decimal? MTO_PREPAGO { get; set; }
        public string IND_ESTADO { get; set; }
        public string ID_USUARIO { get; set; }
    }
}