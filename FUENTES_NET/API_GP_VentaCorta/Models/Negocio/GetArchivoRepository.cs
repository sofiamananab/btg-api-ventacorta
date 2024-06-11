using API_GP_VentaCorta.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_GP_VentaCorta.Models.Negocio
{
    public static class GetArchivoRepository
    {

        private static DTOAPIResponse ValidaParametros(ParametersDTO searchParameters)
        {
            string CodeError = "0";
            string DesError = "";

            if (searchParameters == null) { return new DTOAPIResponse { code = "1", message = "No existen parámetros" }; }

            //if (string.IsNullOrEmpty(cargaArchivoParametros.RutaArchivo)) { CodeError = "1";  DesError += "No existe ruta de archivo."; }
            //if (string.IsNullOrEmpty(cargaArchivoParametros.idUsuario)) { CodeError = "1"; DesError += "No existe id usuario de archivo."; }

            //if (!string.IsNullOrEmpty(cargaArchivoParametros.RutaArchivo)) 
            //{
            //    if (!File.Exists(cargaArchivoParametros.RutaArchivo))
            //    {
            //        CodeError = "1"; DesError += "No existe archivo.";
            //    }
            //}

            return new DTOAPIResponse { code = CodeError, message = DesError };
        }




    }
}