using API_GP_VentaCorta.Models.Datos.DTO;
using API_GP_VentaCorta.Models.Datos.Repositorio.Valorizacion;
using API_GP_VentaCorta.Models.DTO;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace API_GP_VentaCorta.Models.Negocio
{
    public static class ValorizacionRepository
    {
        #region To Do
        /*
         * Faltan validaciones a datos de entrada
         * Faltan log 
         * retorno segun corresponda
         */
        #endregion 
        private const string AM_DESACTIVACARGA_PR = "AM_PACTO_PG.AM_DESACTIVACARGA_PR";
        private const string AM_ACTUALIZAGARANTIAS_PR = "AM_PACTO_PG.AM_ACTUALIZAGARANTIAS_PR";


        public static List<ErrorEntity> DesactivaCargaArchivoValorizacion(string Estado)
        {
            List<ErrorEntity> Value = new List<ErrorEntity>();

            // Crea el contexto genérico de base de datos
            GenericRepositoyDB contextDBRepository = new GenericRepositoyDB();

            try
            {
                // Carga los parámetros de entrada
                List<Parameter> inputParameters = new List<Parameter>
                {
                    new Parameter { name = "EV_ESTADO", type = OracleDbType.Varchar2, value = Estado }
                };

                // Carga los parámetros de salida
                List<Parameter> outParameters = new List<Parameter>
                {
                    new Parameter { name = "RC_DATOS", isCursor = true }
                };

                // Apertura de conexión
                contextDBRepository.openConnection();

                // Ejecuta procedimiento de Selección
                contextDBRepository.execProcedure(inputParameters, AM_DESACTIVACARGA_PR, ref outParameters);

                // Cargar Datos
                Value = listGetDesactiva(((OracleDataReader)outParameters[0].value));
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

        #region ListgetDesactiva
        private static List<ErrorEntity> listGetDesactiva(OracleDataReader dr)
        {
            List<ErrorEntity> listValues = new List<ErrorEntity>();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    ErrorEntity values = new ErrorEntity
                    {
                        ID_ERROR = dr.GetDecimal(0),
                        DES_ERROR = dr.GetString(1)
                    };

                    listValues.Add(values);
                }
            }
            return listValues;
        }
        #endregion

        #region Carga de archivo valorizacion
        public static DTOAPIResponse CargaArchivoValorizacion(ArchivoDTO cargaArchivoParametros)
        {
            DTOAPIResponse dtoAPIResponse = CargaValorizacion(cargaArchivoParametros);

            if (dtoAPIResponse.code == "0")
            {
                //Se debe actualiza el monto con el cálculo de la variación (Garantias) solo para las operaciones vigentes, con fecha antes del vencimiento

                List<ErrorEntity> listError = ActualizaGarantias();

            }

            return dtoAPIResponse;
        }
            public static DTOAPIResponse CargaValorizacion(ArchivoDTO cargaArchivoParametros)
        {
            Func<string, DateTime> toFecha = (string cadena) =>
            {
                string[] format = { "yyyyMMdd" };

                cadena = cadena.Substring(6, 4) + cadena.Substring(3, 2) + cadena.Substring(0, 2); ;
                DateTime.TryParseExact(cadena, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime date);

                return date;
            };

            string parametros = Newtonsoft.Json.JsonConvert.SerializeObject(cargaArchivoParametros);

            //string keyConexion = ConfigurationManager.AppSettings["ORADBCONN"];
            string keyConexion = ConfigurationManager.ConnectionStrings["ORADBCONN"].ConnectionString;
            DateTime fecha = DateTime.Now;
            DTOAPIResponse dTOAPIResponse;//= new DTOAPIResponse();
            try
            {
                dTOAPIResponse = ValidaParametros(cargaArchivoParametros);
                if (dTOAPIResponse.code != "0")
                {
                    dTOAPIResponse.param = parametros;
                    dTOAPIResponse.timestamp = fecha.ToString("o", CultureInfo.InvariantCulture);
                    return dTOAPIResponse;
                }

                DataSet datosPlanilla = getDataExcel(cargaArchivoParametros.RutaArchivo, "Hoja1");
                if (datosPlanilla is null)
                {
                    return new DTOAPIResponse { code = "1", message = "Archivo no contiene datos", timestamp = fecha.ToString("o", CultureInfo.InvariantCulture), param = parametros };
                }

                DataTable dt = datosPlanilla.Tables[0];

                dt = dt.DefaultView.ToTable();


                using (var connection = new OracleConnection(keyConexion))
                {
                    int Totalreg = dt.Rows.Count;
                    connection.Open();
                    //connection.KeepAliveTime = 5 * 60;

                    DateTime[] Fecprocesos = new DateTime[Totalreg];
                    String[] Idusuarios = new String[Totalreg];
                    String[] Nommercados = new String[Totalreg];
                    Decimal[] Mtobases = new Decimal[Totalreg];
                    Decimal[] Mtoventacortas = new Decimal[Totalreg];
                    Decimal[] Mtogtiasimultaneas = new Decimal[Totalreg];
                    Decimal[] Mtofondogtias = new Decimal[Totalreg];
                    Decimal[] Mtogtiacorrientes = new Decimal[Totalreg];
                    String[] Nomnemotecnicos = new String[Totalreg];
                    String[] Indestados = new String[Totalreg];

                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        Fecprocesos[j] = toFecha(Convert.ToString(dt.Rows[j]["Fecha proceso"]));
                        Idusuarios[j] = cargaArchivoParametros.idUsuario;
                        Nommercados[j] = Convert.ToString(dt.Rows[j]["Mercado"]);
                        Mtobases[j] = Convert.ToDecimal(dt.Rows[j]["Base"]);
                        Mtoventacortas[j] = Convert.ToDecimal(dt.Rows[j]["Venta corta"]);
                        Mtogtiasimultaneas[j] = Convert.ToDecimal(dt.Rows[j]["Garantia Simultanea"]);
                        Mtofondogtias[j] = Convert.ToDecimal(dt.Rows[j]["Fondo Garantias"]);
                        Mtogtiacorrientes[j] = Convert.ToDecimal(dt.Rows[j]["Garantias Corrientes"]);
                        Nomnemotecnicos[j] = Convert.ToString(dt.Rows[j]["Nemotecnico"]);
                        Indestados[j] = "1"; // Convert.ToString(dt.Rows[j][""]);
                    }

                    OracleParameter Idusuario = new OracleParameter { OracleDbType = OracleDbType.Varchar2, Value = Idusuarios };
                    OracleParameter Fecproceso = new OracleParameter { OracleDbType = OracleDbType.Date, Value = Fecprocesos };
                    OracleParameter Nommercado = new OracleParameter { OracleDbType = OracleDbType.Varchar2, Value = Nommercados };
                    OracleParameter Mtobase = new OracleParameter { OracleDbType = OracleDbType.Decimal, Value = Mtobases };
                    OracleParameter Mtoventacorta = new OracleParameter { OracleDbType = OracleDbType.Decimal, Value = Mtoventacortas };
                    OracleParameter Mtogtiasimultanea = new OracleParameter { OracleDbType = OracleDbType.Decimal, Value = Mtogtiasimultaneas };
                    OracleParameter Mtofondogtia = new OracleParameter { OracleDbType = OracleDbType.Decimal, Value = Mtofondogtias };
                    OracleParameter Mtogtiacorriente = new OracleParameter { OracleDbType = OracleDbType.Decimal, Value = Mtogtiacorrientes };
                    OracleParameter Nomnemotecnico = new OracleParameter { OracleDbType = OracleDbType.Varchar2, Value = Nomnemotecnicos };
                    OracleParameter Indestado = new OracleParameter { OracleDbType = OracleDbType.Varchar2, Value = Indestados };

                    // create command and set properties
                    OracleCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "INSERT INTO AM_OPERACIONES.ORC_VCVALORIZACION_TD (ID_VALORIZACION, FEC_CARGA, ID_USUARIO, FEC_PROCESO, NOM_MERCADO, MTO_BASE, MTO_VENTACORTA, MTO_GTIASIMULTANEA, MTO_FONDOGTIA, MTO_GTIACORRIENTE, NOM_NEMOTECNICO, IND_ESTADO) VALUES(ORC_VALORIZACION_SQ.NEXTVAL, SYSDATE, :1, :2, :3, :4, :5, :6, :7, :8, :9, :10)";

                    cmd.ArrayBindCount = Idusuarios.Length;
                    cmd.Parameters.Add(Idusuario);
                    cmd.Parameters.Add(Fecproceso);
                    cmd.Parameters.Add(Nommercado);
                    cmd.Parameters.Add(Mtobase);
                    cmd.Parameters.Add(Mtoventacorta);
                    cmd.Parameters.Add(Mtogtiasimultanea);
                    cmd.Parameters.Add(Mtofondogtia);
                    cmd.Parameters.Add(Mtogtiacorriente);
                    cmd.Parameters.Add(Nomnemotecnico);
                    cmd.Parameters.Add(Indestado);

                    cmd.ExecuteNonQuery();

                }
                dTOAPIResponse = new DTOAPIResponse { code = "0", message = "Archivo cargado con éxito. Cantidad de registros (" + dt.Rows.Count.ToString() + ")", timestamp = fecha.ToString("o", CultureInfo.InvariantCulture), param = parametros };

            }
            catch (Exception ex)
            {
                //throw ex;
                dTOAPIResponse = new DTOAPIResponse { code = "1", message = "Formato invalido de archivo. " + ex.Message.ToString(), timestamp = fecha.ToString("o", CultureInfo.InvariantCulture), param = parametros };
            }

            return dTOAPIResponse;
        }

        public static List<ErrorEntity> ActualizaGarantias()
        {
            List<ErrorEntity> Value = new List<ErrorEntity>();

            // Crea el contexto genérico de base de datos
            GenericRepositoyDB contextDBRepository = new GenericRepositoyDB();

            try
            {
                // Carga los parámetros de salida
                List<Parameter> outParameters = new List<Parameter>
                {
                    new Parameter { name = "RC_DATOS", isCursor = true }
                };

                // Apertura de conexión
                contextDBRepository.openConnection();

                // Ejecuta procedimiento de Selección
                contextDBRepository.execProcedure(null, AM_ACTUALIZAGARANTIAS_PR, ref outParameters);

                // Cargar Datos
                Value = listGetDesactiva(((OracleDataReader)outParameters[0].value));
            }
            catch (Exception e)
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

        private static DTOAPIResponse ValidaParametros(ArchivoDTO cargaArchivoParametros)
        {
            string CodeError = "0";
            string DesError = "";

            if (cargaArchivoParametros == null) { return new DTOAPIResponse { code = "1", message = "No existen parámetros" }; }

            if (string.IsNullOrEmpty(cargaArchivoParametros.RutaArchivo)) { CodeError = "1"; DesError += "No existe ruta de archivo."; }
            if (string.IsNullOrEmpty(cargaArchivoParametros.idUsuario)) { CodeError = "1"; DesError += "No existe id usuario de archivo."; }

            if (!string.IsNullOrEmpty(cargaArchivoParametros.RutaArchivo))
            {
                if (!File.Exists(cargaArchivoParametros.RutaArchivo))
                {
                    CodeError = "1"; DesError += "No existe archivo.";
                }
            }

            return new DTOAPIResponse { code = CodeError, message = DesError };
        }


        private static DataSet getDataExcel(string sArchivo, string sHoja)
        {
            if (string.IsNullOrEmpty(sHoja)) { sHoja = "Hoja1"; }

            return ExecSqlExcel(sArchivo, string.Format("select * from [{0}$]", sHoja));
        }

        private static DataSet ExecSqlExcel(string filename, string selectCommandText)
        {
            //' IMPORTANTE:
            //' Para que escanee toda la columna antes de determinar el tipo de dato, hay que modificar el siguiente valor en el servidor WEB
            //' HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Jet\4.0\Engines\Excel\ 
            //' TypeGuessRows = 0  ---> (Default TypeGuessRows = 8)

            //' IMEX=0 Export Mode
            //' IMEX=1 Import Mode ---> ImportMixedTypes = "Text"
            //' IMEX=2 Linked Mode
            //Boolean firstRowHasNames = true;
            System.Data.DataSet ds = new DataSet();

            int IMEX = 1;
            //' MAXSCANROWS Puede ser de 1 a 16 o 0 para escanear toda la columna
            int MAXSCANROWS = 0;

            //' Indica si la primera fila contiene los nombres de las columnas
            string HDR = "yes"; //IIf(firstRowHasNames, "yes", "no")
            System.Data.OleDb.OleDbConnection cn;
            System.Data.OleDb.OleDbDataAdapter cmd;

            cn = new System.Data.OleDb.OleDbConnection("provider=Microsoft.ACE.OLEDB.12.0;data source=" + filename + ";Extended Properties='Excel 8.0;HDR=" + HDR + ";IMEX=" + IMEX + ";MAXSCANROWS=" + MAXSCANROWS + ";'");

            //' Select the data from Sheet of the workbook.
            cmd = new System.Data.OleDb.OleDbDataAdapter(selectCommandText, cn);
            cn.Open();

            try
            {
                //'El acceso a los datos (la llamada al método Fill) debe tener lugar dentro 
                //'de un bloque Try y el bloque Finally asociado debe cerrar la conexión de datos (con el método Close). 
                //'Esta estructura cierra inmediatamente la conexión a la base de datos cuando se produce una excepción.
                cmd.Fill(ds);
            }
            finally
            {
                if (cn.State == System.Data.ConnectionState.Open) { cn.Close(); };
            }

            return ds;
        }

        #endregion
        #region obtener datos de archivo valorizacion
        public static List<ArchivoValorizacion> getDatosArchivoValorizacion(ParametersDTO searchParameters)
        {
            DTOAPIResponse dTOAPIResponse = ValidaParametrosArchivoValorizacion(searchParameters);
            if (dTOAPIResponse.code != "0")
            {
                throw new ArgumentException(dTOAPIResponse.message);
            }


            return Valorizacion.getDatosArchivoValorizacion(searchParameters);
        }
        private static DTOAPIResponse ValidaParametrosArchivoValorizacion(ParametersDTO searchParameters)
        {
            string CodeError = "0";
            string DesError = "";

            if (searchParameters == null) { return new DTOAPIResponse { code = "1", message = "No existen parámetros" }; }


            return new DTOAPIResponse { code = CodeError, message = DesError };
        }

        public static List<ArchivoValorizacion> getNemoValor(string sNemo)
        {
            if (string.IsNullOrEmpty(sNemo))  {  throw new ArgumentException("No existen parámetros");  }


            return Valorizacion.getNemoValor(sNemo);
        }
        public static List<ArchivoValorizacion> getNemoAutocomplete(string sNemo)
        {
            if (string.IsNullOrEmpty(sNemo)) { throw new ArgumentException("No existen parámetros"); }


            return Valorizacion.getNemoAutocomplete(sNemo);
        }

        


        //public static DTOAPIResponse getArchivosCargaGarantias(ParametersEntity searchParameters)
        //{
        //    using (var Contexto = new EntitiesPacto())
        //    {
        //        OracleParameter param1 = new OracleParameter("EN_PAGINA", OracleDbType.Double, searchParameters.enPagina, ParameterDirection.Input);
        //        OracleParameter param2 = new OracleParameter("EN_NUMEROFILAS", OracleDbType.Double, searchParameters.numFilas, ParameterDirection.Input);
        //        OracleParameter param3 = new OracleParameter("EV_FILTER", OracleDbType.Varchar2, searchParameters.Filter != null && searchParameters.Filter != "" ? searchParameters.Filter : "", ParameterDirection.Input);
        //        OracleParameter param4 = new OracleParameter("EV_ORDERBY", OracleDbType.Varchar2, "", ParameterDirection.Input);
        //        OracleParameter param5 = new OracleParameter("EN_IDENTPAG", OracleDbType.Double, searchParameters.EnConf, ParameterDirection.Input);
        //        OracleParameter param6 = new OracleParameter("SN_TOTALFILAS", OracleDbType.Double, ParameterDirection.Output);
        //        OracleParameter param7 = new OracleParameter("RC_DATOS", OracleDbType.RefCursor, ParameterDirection.Output);

        //        var cmd = "BEGIN AM_PACTO_PG.AM_GETVALORIZACION_PR (:EN_PAGINA, :EN_NUMEROFILAS, :EV_FILTER, :EV_ORDERBY, :EN_IDENTPAG, :SN_TOTALFILAS, :RC_DATOS ); end;";

        //        return Contexto.Database.SqlQuery<ValorizacionEntity>(cmd, param1, param2, param3, param4, param5, param6, param7).ToList();
        //    }
        //}

        #endregion


    }
}