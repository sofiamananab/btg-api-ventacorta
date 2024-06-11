using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_GP_VentaCorta.Models.Datos.DTO
{
    public class ErrorDTO
    {
        public Decimal ID_ERROR { get; set; }
        public string DES_ERROR { get; set; }
        public decimal ID_OPERACION { get; set; }
    }
}