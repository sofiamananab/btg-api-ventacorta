using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_GP_VentaCorta.Models.Datos.DTO
{
    public class GarantiaDTO
    {
        public decimal ID_OPERACION { get; set; }
        public decimal SEC_VALORES { get; set; }
        public string ID_NEMO { get; set; }
        public DateTime FEC_VALOR { get; set; }
        public decimal? MTO_VALOR { get; set; }
        public decimal? MTO_CANTIDAD { get; set; }
        public decimal? MTO_GARANTIA { get; set; }
        public string ID_USUARIO { get; set; }
        public DateTime? FEC_CREACION { get; set; }
        public string IND_ESTADO { get; set; }
    }
}