using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_GP_VentaCorta.Models.Datos.DTO
{
    public class EventosDTO
    {
        public decimal ID_OPERACION { get; set; }
        public string TIP_OPERACION { get; set; }
        public decimal SEC_EVENTO { get; set; }
        public string ID_USUARIO { get; set; }
        public string NOMBRE_USUARIO { get; set; }
        public DateTime FEC_CREACION { get; set; }
        public string NOM_CAMPO { get; set; }
        public string VAL_CAMPO { get; set; }
        public string DES_EVENTO { get; set; }

    }
}