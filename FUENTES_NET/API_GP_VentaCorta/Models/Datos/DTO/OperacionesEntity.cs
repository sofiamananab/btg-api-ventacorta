using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_GP_VentaCorta.Models.Datos.DTO.Operaciones
{
    public class OperacionesEntity
    {
        public decimal ID_OPERACION { get; set; }
        public string ID_INSTRUMENTO { get; set; }
        public DateTime FEC_INICIO { get; set; }
        public DateTime FEC_VENCIMIENTO { get; set; }
        public decimal? MTO_CANTIDAD { get; set; }
        public decimal? MTO_PRECIOMEDIO { get; set; }
        public decimal? ID_CONTRAPARTE { get; set; }
        public string NOM_CONTRAPARTE { get; set; }
        public decimal? ID_FONDO { get; set; }
        public string NOM_FONDO { get; set; }
        public decimal? MTO_TASA { get; set; }
        public decimal? CAN_DIAS { get; set; }
        public decimal? BP { get; set; }
        public string ID_CUENTADCV { get; set; }
        public decimal? MTO_PREMIO { get; set; }
        public string IND_ESTADO { get; set; }
        public string ID_USUARIO { get; set; }
        public DateTime FEC_CREACION { get; set; }
        public string ID_ORIGEN { get; set; }
        public string NOM_USUARIO { get; set; }
        public string TIP_OPERACION { get; set; }
        public string NOM_TIPOPERACION { get; set; }
        public decimal? TOTAL_FILAS { get; set; }

        public List<PrepagoDTO> prepagoDTO { get; set; }
        public List<GarantiaDTO> garantiaDTO { get; set; }
    }
}