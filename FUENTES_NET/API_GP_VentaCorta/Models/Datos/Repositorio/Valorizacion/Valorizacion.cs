using API_GP_VentaCorta.Models.Datos.DTO;
using API_GP_VentaCorta.Models.DTO;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_GP_VentaCorta.Models.Datos.Repositorio.Valorizacion
{
    public class Valorizacion
    {
        #region Const
        private const string AM_GETVALORIZACION_PR = "AM_PACTO_PG.AM_GETVALORIZACION_PR";
        private const string AM_GETVALORVC_PR = "AM_PACTO_PG.AM_GETVALORVC_PR";
        private const string AM_GETNEMOAUTOCOMPLETEVC_PR = "AM_PACTO_PG.AM_GETNEMOAUTOCOMPLETEVC_PR";


        #endregion

        public static List<ArchivoValorizacion> getDatosArchivoValorizacion(ParametersDTO searchParameters)
        {
            List<ArchivoValorizacion> Value = new List<ArchivoValorizacion>();

            // Crea el contexto genérico de base de datos
            GenericRepositoyDB contextDBRepository = new GenericRepositoyDB();

            try
            {
                // Carga los parámetros de entrada
                List<Parameter> inputParameters = new List<Parameter>
                {
                    new Parameter { name = "EN_PAGINA", type = OracleDbType.Double, value = searchParameters.enPagina },
                    new Parameter { name = "EN_NUMEROFILAS", type = OracleDbType.Double, value = searchParameters.numFilas },
                    new Parameter { name = "EV_FILTER", type = OracleDbType.Varchar2, value = string.IsNullOrEmpty(searchParameters.Filter) ? "" : searchParameters.Filter },
                    new Parameter { name = "EV_ORDERBY", type = OracleDbType.Varchar2, value = null },
                    new Parameter { name = "EN_IDENTPAG", type = OracleDbType.Double, value = searchParameters.EnConf }
                };

                // Carga los parámetros de salida
                List<Parameter> outParameters = new List<Parameter>
                {
                    new Parameter { name = "SN_TOTALFILAS", type = OracleDbType.Double },
                    new Parameter { name = "RC_DATOS", isCursor = true }
                };

                // Apertura de conexión
                contextDBRepository.openConnection();

                // Ejecuta procedimiento de Selección
                contextDBRepository.execProcedure(inputParameters, AM_GETVALORIZACION_PR, ref outParameters);

                // Cargar Datos
                Value = listGet(((OracleDataReader)outParameters[1].value));
            }
            catch //(Exception e)
            {
                throw;
            }
            finally
            {
                // Cierre de comexión
                contextDBRepository.closeConnection();
            }

            return Value;
        }

        #region Listget
        private static List<ArchivoValorizacion> listGet(OracleDataReader dr)
        {
            List<ArchivoValorizacion> listValues = new List<ArchivoValorizacion>();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    ArchivoValorizacion values = new ArchivoValorizacion
                    {
                        TOTAL_FILAS = dr.GetDecimal(1),
                        ID_VALORIZACION = dr.GetDecimal(2),
                        ID_USUARIO = dr.GetString(3),
                        FEC_CARGA = dr.GetDateTime(4),
                        FEC_PROCESO = dr.GetDateTime(5),
                        NOM_MERCADO = dr.GetString(6),
                        MTO_BASE = dr.GetDecimal(7),
                        MTO_VENTACORTA = dr.GetDecimal(8),
                        MTO_GTIASIMULTANEA = dr.GetDecimal(9),
                        MTO_FONDOGTIA = dr.GetDecimal(10),
                        MTO_GTIACORRIENTE = dr.GetDecimal(11),
                        NOM_NEMOTECNICO = dr.GetString(12),
                        IND_ESTADO = dr.GetString(13)
                    };

                    listValues.Add(values);
                }
            }
            return listValues;
        }
        #endregion

        public static List<ArchivoValorizacion> getNemoValor(string sNemo)
        {
            List<ArchivoValorizacion> Value = new List<ArchivoValorizacion>();

            // Crea el contexto genérico de base de datos
            GenericRepositoyDB contextDBRepository = new GenericRepositoyDB();

            try
            {
                // Carga los parámetros de entrada
                List<Parameter> inputParameters = new List<Parameter>
                {
                    new Parameter { name = "EV_NEMO", type = OracleDbType.Varchar2, value = sNemo }
                };

                // Carga los parámetros de salida
                List<Parameter> outParameters = new List<Parameter>
                {
                    new Parameter { name = "RC_DATOS", isCursor = true }
                };

                // Apertura de conexión
                contextDBRepository.openConnection();

                // Ejecuta procedimiento de Selección
                contextDBRepository.execProcedure(inputParameters, AM_GETVALORVC_PR, ref outParameters);

                // Cargar Datos
                Value = listGetValor(((OracleDataReader)outParameters[0].value));
            }
            catch //(Exception e)
            {
                throw;
            }
            finally
            {
                // Cierre de comexión
                contextDBRepository.closeConnection();
            }

            return Value;
        }

        #region Listget
        private static List<ArchivoValorizacion> listGetValor(OracleDataReader dr)
        {
            List<ArchivoValorizacion> listValues = new List<ArchivoValorizacion>();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    ArchivoValorizacion values = new ArchivoValorizacion
                    {
                        FEC_CARGA = dr.GetDateTime(0),
                        MTO_VENTACORTA = dr.GetDecimal(1),
                        MTO_GTIASIMULTANEA = dr.GetDecimal(2),
                        ID_VALORIZACION = 0,
                        ID_USUARIO = "",
                        FEC_PROCESO = null,
                        NOM_MERCADO="",
                        MTO_BASE=0,
                        MTO_FONDOGTIA = 0,
                        MTO_GTIACORRIENTE = 0,
                        NOM_NEMOTECNICO="",
                        IND_ESTADO="",
                        TOTAL_FILAS=1
                    };

                    listValues.Add(values);
                }
            }
            return listValues;
        }
        #endregion
        public static List<ArchivoValorizacion> getNemoAutocomplete(string sNemo)
        {
            List<ArchivoValorizacion> Value = new List<ArchivoValorizacion>();

            // Crea el contexto genérico de base de datos
            GenericRepositoyDB contextDBRepository = new GenericRepositoyDB();

            try
            {
                // Carga los parámetros de entrada
                List<Parameter> inputParameters = new List<Parameter>
                {
                    new Parameter { name = "EV_NEMO", type = OracleDbType.Varchar2, value = sNemo }
                };

                // Carga los parámetros de salida
                List<Parameter> outParameters = new List<Parameter>
                {
                    new Parameter { name = "RC_DATOS", isCursor = true }
                };

                // Apertura de conexión
                contextDBRepository.openConnection();

                // Ejecuta procedimiento de Selección
                contextDBRepository.execProcedure(inputParameters, AM_GETNEMOAUTOCOMPLETEVC_PR, ref outParameters);

                // Cargar Datos
                Value = listGetValorNemosAutocomplete(((OracleDataReader)outParameters[0].value));
            }
            catch //(Exception e)
            {
                throw;
            }
            finally
            {
                // Cierre de comexión
                contextDBRepository.closeConnection();
            }

            return Value;
        }

        #region listGetValorNemosAutocomplete
        private static List<ArchivoValorizacion> listGetValorNemosAutocomplete(OracleDataReader dr)
        {
            List<ArchivoValorizacion> listValues = new List<ArchivoValorizacion>();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    ArchivoValorizacion values = new ArchivoValorizacion
                    {
                        FEC_CARGA = null,
                        MTO_VENTACORTA = null,
                        MTO_GTIASIMULTANEA = null,
                        ID_VALORIZACION = 0,
                        ID_USUARIO = "",
                        FEC_PROCESO = null,
                        NOM_MERCADO = "",
                        MTO_BASE = 0,
                        MTO_FONDOGTIA = 0,
                        MTO_GTIACORRIENTE = 0,
                        NOM_NEMOTECNICO = dr.GetString(0),
                        IND_ESTADO = "",
                        TOTAL_FILAS = 1
                    };

                    listValues.Add(values);
                }
            }
            return listValues;
        }
        #endregion 
    }
}