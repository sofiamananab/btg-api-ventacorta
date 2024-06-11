using API_GP_VentaCorta.Models.Datos.DTO;
using API_GP_VentaCorta.Models.Datos.DTO.Operaciones;
using API_GP_VentaCorta.Models.DTO;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_GP_VentaCorta.Models.Datos.Repositorio.OperacionesVC
{
    public  class Operaciones
    {
        private const string AM_GETOPERACIONESVC_PR = "AM_PACTO_PG.AM_GETOPERACIONESVC_PR";
        private const string AM_GETPREPAGOSVC_PR = "AM_PACTO_PG.AM_GETPREPAGOSVC_PR";
        private const string AM_GETGARANTIASVC_PR = "AM_PACTO_PG.AM_GETGARANTIASVC_PR";
        private const string AM_GETFONDOSDCV_PR = "AM_PACTO_PG.AM_GETFONDOSDCV_PR";
        private const string AM_GETMAILVCTO_PR = "AM_PACTO_PG.AM_GETMAILVCTO_PR";

        private const string AM_SAVEOPERACIONVC_PR = "AM_PACTO_PG.AM_SAVEOPERACIONVC_PR";
        private const string AM_RENOVAROPERACIONVC_PR = "AM_PACTO_PG.AM_RENOVAROPERACIONVC_PR";
        private const string AM_VENCEROPERACIONVC_PR = "AM_PACTO_PG.AM_VENCEROPERACIONVC_PR";
        
        private const string AM_INSERTAPREPAGOVC_PR = "AM_PACTO_PG.AM_INSERTAPREPAGOVC_PR";
        private const string AM_INSERTAGARANTIAVC_PR = "AM_PACTO_PG.AM_INSERTAGARANTIAVC_PR";
        
        private const string AM_ELIMINAPREPAGOVC_PR = "AM_PACTO_PG.AM_ELIMINAPREPAGOVC_PR";
        private const string AM_ELIMINAGARANTIAVC_PR = "AM_PACTO_PG.AM_ELIMINAGARANTIAVC_PR";
        private const string AM_GETEVENTOS_PR = "AM_PACTO_PG.AM_GETEVENTOS_PR";


        public static List<OperacionesEntity> getOperacionesVC(ParametersDTO searchParameters)
        {
            List<OperacionesEntity> Value = new List<OperacionesEntity>();

            // Crea el contexto genérico de base de datos
            GenericRepositoyDB contextDBRepository = new GenericRepositoyDB();

            try
            {
                // Carga los parámetros de entrada
                List<Parameter> inputParameters = new List<Parameter>
                {
                    new Parameter { name = "EN_IDOPERACION", type = OracleDbType.Double, value = searchParameters.IdOperacion },
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
                contextDBRepository.execProcedure(inputParameters, AM_GETOPERACIONESVC_PR, ref outParameters);

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
        private static List<OperacionesEntity> listGet(OracleDataReader dr)
        {
            List<OperacionesEntity> listValues = new List<OperacionesEntity>();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    if (dr.GetDecimal(1) > 0)
                    {
                        OperacionesEntity values = new OperacionesEntity();
                        values.TOTAL_FILAS = dr.GetDecimal(1);
                        values.ID_OPERACION = dr.GetDecimal(2);
                        values.ID_INSTRUMENTO = dr.GetString(3);
                        values.FEC_INICIO = dr.GetDateTime(4);
                        values.FEC_VENCIMIENTO = dr.GetDateTime(5);
                        values.MTO_CANTIDAD = dr.GetDecimal(6);
                        values.MTO_PRECIOMEDIO = dr.GetDecimal(7);
                        values.NOM_CONTRAPARTE = dr.GetString(8);
                        values.NOM_FONDO = dr.GetString(9);
                        values.MTO_TASA = dr.GetDecimal(10);
                        values.CAN_DIAS = dr.GetDecimal(11);
                        values.BP = dr.GetDecimal(12);
                        values.ID_CUENTADCV = dr.GetString(13);
                        values.MTO_PREMIO = dr.GetDecimal(14);
                        values.IND_ESTADO = dr.GetString(15);
                        values.ID_USUARIO = dr.GetString(16);
                        values.FEC_CREACION = dr.GetDateTime(17);
                        values.ID_ORIGEN = dr.GetString(18);
                        values.NOM_USUARIO = dr.GetString(19);
                        values.TIP_OPERACION = dr.GetString(20);
                        values.NOM_TIPOPERACION = dr.GetString(21);

                        listValues.Add(values);
                    }

                }
            }
            return listValues;
        }
        #endregion

        public static List<PrepagoDTO> getPrepagosVC(ParametersDTO searchParameters)
        {
            List<PrepagoDTO> Value = new List<PrepagoDTO>();

            // Crea el contexto genérico de base de datos
            GenericRepositoyDB contextDBRepository = new GenericRepositoyDB();

            try
            {
                // Carga los parámetros de entrada
                List<Parameter> inputParameters = new List<Parameter>
                {
                    new Parameter { name = "EN_IDOPERACION", type = OracleDbType.Double, value = searchParameters.IdOperacion }
                };

                // Carga los parámetros de salida
                List<Parameter> outParameters = new List<Parameter>
                {
                    new Parameter { name = "RC_DATOS", isCursor = true }
                };

                // Apertura de conexión
                contextDBRepository.openConnection();

                // Ejecuta procedimiento de Selección
                contextDBRepository.execProcedure(inputParameters, AM_GETPREPAGOSVC_PR, ref outParameters);

                // Cargar Datos
                Value = listGetPrepagos(((OracleDataReader)outParameters[0].value));


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

        #region ListgetPrepagos
        private static List<PrepagoDTO> listGetPrepagos(OracleDataReader dr)
        {
            List<PrepagoDTO> listValues = new List<PrepagoDTO>();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    PrepagoDTO values = new PrepagoDTO();
                    values.ID_OPERACION = dr.GetDecimal(0);
                    values.SEC_PREPAGO = dr.GetDecimal(1);
                    values.FEC_CREACION = dr.GetDateTime(2);
                    values.NOM_USUARIO = dr.GetString(3);
                    values.MTO_CANTIDAD = dr.GetDecimal(4);
                    values.MTO_PREPAGO = dr.GetDecimal(5);
                    values.IND_ESTADO = dr.GetString(6);
                    values.ID_USUARIO = dr.GetString(7);

                    listValues.Add(values);
                }
            }
            return listValues;
        }
        #endregion

        public static List<GarantiaDTO> getGarantiasVC(ParametersDTO searchParameters)
        {
            List<GarantiaDTO> Value = new List<GarantiaDTO>();

            // Crea el contexto genérico de base de datos
            GenericRepositoyDB contextDBRepository = new GenericRepositoyDB();

            try
            {
                // Carga los parámetros de entrada
                List<Parameter> inputParameters = new List<Parameter>
                {
                    new Parameter { name = "EN_IDOPERACION", type = OracleDbType.Double, value = searchParameters.IdOperacion }
                };

                // Carga los parámetros de salida
                List<Parameter> outParameters = new List<Parameter>
                {
                    new Parameter { name = "RC_DATOS", isCursor = true }
                };

                // Apertura de conexión
                contextDBRepository.openConnection();

                // Ejecuta procedimiento de Selección
                contextDBRepository.execProcedure(inputParameters, AM_GETGARANTIASVC_PR, ref outParameters);

                // Cargar Datos
                Value = listGetGarantias(((OracleDataReader)outParameters[0].value));


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

        #region ListgetPrepagos
        private static List<GarantiaDTO> listGetGarantias(OracleDataReader dr)
        {
            List<GarantiaDTO> listValues = new List<GarantiaDTO>();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    GarantiaDTO values = new GarantiaDTO();
                    values.ID_OPERACION = dr.GetDecimal(0);
                    values.SEC_VALORES = dr.GetDecimal(1);
                    values.ID_NEMO = dr.GetString(2);
                    values.FEC_VALOR = dr.GetDateTime(3);
                    values.MTO_VALOR = dr.GetDecimal(4);
                    values.MTO_CANTIDAD = dr.GetDecimal(5);
                    values.MTO_GARANTIA = dr.GetDecimal(6);
                    values.ID_USUARIO = dr.GetString(7);
                    values.FEC_CREACION = dr.GetDateTime(8);
                    values.IND_ESTADO = dr.GetString(9);

                    listValues.Add(values);
                }
            }
            return listValues;
        }
        #endregion


        public static List<ErrorDTO> grabaOperacionesVC(OperacionesEntity operacionesEntity)
        {
            List<ErrorDTO> Value = new List<ErrorDTO>();

            // Crea el contexto genérico de base de datos
            GenericRepositoyDB contextDBRepository = new GenericRepositoyDB();

            try
            {
                // Carga los parámetros de entrada
                List<Parameter> inputParameters = new List<Parameter>
                {
                    new Parameter { name = "EN_ID_OPERACION", type = OracleDbType.Double, value = operacionesEntity.ID_OPERACION },
                    new Parameter { name = "EN_TIP_OPERACION", type = OracleDbType.Double, value = operacionesEntity.TIP_OPERACION },
                    new Parameter { name = "EV_ID_INSTRUMENTO", type = OracleDbType.Varchar2, value = operacionesEntity.ID_INSTRUMENTO },
                    new Parameter { name = "EN_MTO_TASA", type = OracleDbType.Double, value = operacionesEntity.MTO_TASA },
                    new Parameter { name = "EV_FEC_INICIO", type = OracleDbType.Varchar2, value = operacionesEntity.FEC_INICIO.ToString("yyyyMMdd") },
                    new Parameter { name = "EV_FEC_VENCIMIENTO", type = OracleDbType.Varchar2, value = operacionesEntity.FEC_VENCIMIENTO.ToString("yyyyMMdd") },
                    new Parameter { name = "EN_MTO_CANTIDAD", type = OracleDbType.Double, value = operacionesEntity.MTO_CANTIDAD },
                    new Parameter { name = "EN_MTO_PRECIOMEDIO", type = OracleDbType.Double, value = operacionesEntity.MTO_PRECIOMEDIO },
                    //inputParameters.Add(new Parameter { name = "EN_ID_CONTRAPARTE", type = OracleDbType.Decimal, value = operacionesEntity.ID_CONTRAPARTE });
                    //inputParameters.Add(new Parameter { name = "EN_ID_FONDO", type = OracleDbType.Decimal, value = operacionesEntity.ID_FONDO });
                    new Parameter { name = "EV_NOM_CONTRAPARTE", type = OracleDbType.Varchar2, value = operacionesEntity.NOM_CONTRAPARTE },
                    new Parameter { name = "EV_NOM_FONDO", type = OracleDbType.Varchar2, value = operacionesEntity.NOM_FONDO },
                    new Parameter { name = "EN_CAN_DIAS", type = OracleDbType.Double, value = operacionesEntity.CAN_DIAS },
                    new Parameter { name = "EN_BP", type = OracleDbType.Double, value = operacionesEntity.BP },
                    new Parameter { name = "EV_ID_CUENTADCV", type = OracleDbType.Varchar2, value = operacionesEntity.ID_CUENTADCV },
                    new Parameter { name = "EN_MTO_PREMIO", type = OracleDbType.Double, value = operacionesEntity.MTO_PREMIO },
                    new Parameter { name = "EV_IND_ESTADO", type = OracleDbType.Varchar2, value = operacionesEntity.IND_ESTADO },
                    new Parameter { name = "EV_ID_USUARIO", type = OracleDbType.Varchar2, value = operacionesEntity.ID_USUARIO },
                    new Parameter { name = "EV_ID_ORIGEN", type = OracleDbType.Varchar2, value = operacionesEntity.ID_ORIGEN }
                };

                // Carga los parámetros de salida
                List<Parameter> outParameters = new List<Parameter>
                {
                    new Parameter { name = "RC_DATOS", isCursor = true }
                };

                // Apertura de conexión
                contextDBRepository.openConnection();

                // Ejecuta procedimiento de Selección
                contextDBRepository.execProcedure(inputParameters, AM_SAVEOPERACIONVC_PR, ref outParameters);

                // Cargar Datos
                Value = listGetSave(((OracleDataReader)outParameters[0].value));
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

        public static List<ErrorDTO> renovarOperacionesVC(OperacionesEntity operacionesEntity)
        {
            List<ErrorDTO> Value = new List<ErrorDTO>();

            // Crea el contexto genérico de base de datos
            GenericRepositoyDB contextDBRepository = new GenericRepositoyDB();

            try
            {
                // Carga los parámetros de entrada
                List<Parameter> inputParameters = new List<Parameter>
                {
                    new Parameter { name = "EN_ID_OPERACION", type = OracleDbType.Double, value = operacionesEntity.ID_OPERACION },
                    new Parameter { name = "EV_ID_USUARIO", type = OracleDbType.Varchar2, value = operacionesEntity.ID_USUARIO }
                };

                // Carga los parámetros de salida
                List<Parameter> outParameters = new List<Parameter>
                {
                    new Parameter { name = "RC_DATOS", isCursor = true }
                };

                // Apertura de conexión
                contextDBRepository.openConnection();

                // Ejecuta procedimiento de Selección
                contextDBRepository.execProcedure(inputParameters, AM_RENOVAROPERACIONVC_PR, ref outParameters);

                // Cargar Datos
                Value = listGetSave(((OracleDataReader)outParameters[0].value));
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
        public static List<ErrorDTO> vencerOperacionesVC(OperacionesEntity operacionesEntity)
        {
            List<ErrorDTO> Value = new List<ErrorDTO>();

            // Crea el contexto genérico de base de datos
            GenericRepositoyDB contextDBRepository = new GenericRepositoyDB();

            try
            {
                // Carga los parámetros de entrada
                List<Parameter> inputParameters = new List<Parameter>
                {
                    new Parameter { name = "EN_ID_OPERACION", type = OracleDbType.Double, value = operacionesEntity.ID_OPERACION },
                    new Parameter { name = "EV_ID_USUARIO", type = OracleDbType.Varchar2, value = operacionesEntity.ID_USUARIO }
                };

                // Carga los parámetros de salida
                List<Parameter> outParameters = new List<Parameter>
                {
                    new Parameter { name = "RC_DATOS", isCursor = true }
                };

                // Apertura de conexión
                contextDBRepository.openConnection();

                // Ejecuta procedimiento de Selección
                contextDBRepository.execProcedure(inputParameters, AM_VENCEROPERACIONVC_PR, ref outParameters);

                // Cargar Datos
                Value = listGetSave(((OracleDataReader)outParameters[0].value));
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

        #region listGetSave
        private static List<ErrorDTO> listGetSave(OracleDataReader dr)
        {
            List<ErrorDTO> listValues = new List<ErrorDTO>();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    ErrorDTO values = new ErrorDTO();
                    values.ID_ERROR = dr.GetDecimal(0);
                    values.DES_ERROR = values.ID_ERROR ==0 ? "" : dr.GetString(1);
                    values.ID_OPERACION = dr.GetDecimal(2);

                    listValues.Add(values);
                }
            }
            return listValues;
        }
        #endregion

        public static List<ErrorDTO> insertaPrepagoVC(PrepagoDTO prepagoDTO)
        {
            List<ErrorDTO> Value = new List<ErrorDTO>();

            // Crea el contexto genérico de base de datos
            GenericRepositoyDB contextDBRepository = new GenericRepositoyDB();

            try
            {
                // Carga los parámetros de entrada
                List<Parameter> inputParameters = new List<Parameter>
                {
                    new Parameter { name = "EN_ID_OPERACION", type = OracleDbType.Double, value = prepagoDTO.ID_OPERACION },
                    new Parameter { name = "EN_NUM_SECUENCIA", type = OracleDbType.Double, value = prepagoDTO.SEC_PREPAGO },
                    new Parameter { name = "EV_ID_USUARIO", type = OracleDbType.Varchar2, value = prepagoDTO.ID_USUARIO },
                    new Parameter { name = "EN_MTO_PREPAGO", type = OracleDbType.Double, value = prepagoDTO.MTO_PREPAGO },
                    new Parameter { name = "EN_MTO_CANTIDAD", type = OracleDbType.Double, value = prepagoDTO.MTO_CANTIDAD },
                    new Parameter { name = "EV_IND_ESTADO", type = OracleDbType.Varchar2, value = prepagoDTO.IND_ESTADO }
                };

                // Carga los parámetros de salida
                List<Parameter> outParameters = new List<Parameter>
                {
                    new Parameter { name = "RC_DATOS", isCursor = true }
                };

                // Apertura de conexión
                contextDBRepository.openConnection();

                // Ejecuta procedimiento de Selección
                contextDBRepository.execProcedure(inputParameters, AM_INSERTAPREPAGOVC_PR, ref outParameters);

                // Cargar Datos
                Value = listError(((OracleDataReader)outParameters[0].value));
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

        public static List<ErrorDTO> insertaGarantiaVC(GarantiaDTO garantiaDTO)
        {
            List<ErrorDTO> Value = new List<ErrorDTO>();

            // Crea el contexto genérico de base de datos
            GenericRepositoyDB contextDBRepository = new GenericRepositoyDB();

            try
            {
                // Carga los parámetros de entrada
                List<Parameter> inputParameters = new List<Parameter>
                {
                    new Parameter { name = "EN_ID_OPERACION", type = OracleDbType.Double, value = garantiaDTO.ID_OPERACION },
                    new Parameter { name = "EN_NUM_SECUENCIA", type = OracleDbType.Double, value = garantiaDTO.SEC_VALORES },
                    new Parameter { name = "EV_ID_USUARIO", type = OracleDbType.Varchar2, value = garantiaDTO.ID_USUARIO },
                    new Parameter { name = "EV_ID_NEMO", type = OracleDbType.Varchar2, value = garantiaDTO.ID_NEMO },
                    new Parameter { name = "EV_FEC_VALOR", type = OracleDbType.Varchar2, value = garantiaDTO.FEC_VALOR.ToString("yyyyMMdd") },
                    new Parameter { name = "EN_MTO_VALOR", type = OracleDbType.Double, value = garantiaDTO.MTO_VALOR },
                    new Parameter { name = "EN_MTO_CANTIDAD", type = OracleDbType.Double, value = garantiaDTO.MTO_CANTIDAD },
                    new Parameter { name = "EN_MTO_GARANTIA", type = OracleDbType.Double, value = garantiaDTO.MTO_GARANTIA },
                    new Parameter { name = "EV_IND_ESTADO", type = OracleDbType.Varchar2, value = garantiaDTO.IND_ESTADO }
                };

                // Carga los parámetros de salida
                List<Parameter> outParameters = new List<Parameter>
                {
                    new Parameter { name = "RC_DATOS", isCursor = true }
                };

                // Apertura de conexión
                contextDBRepository.openConnection();

                // Ejecuta procedimiento de Selección
                contextDBRepository.execProcedure(inputParameters, AM_INSERTAGARANTIAVC_PR, ref outParameters);

                // Cargar Datos
                Value = listError(((OracleDataReader)outParameters[0].value));
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


        #region listGetSave
        //private static List<ErrorDTO> listGetPrepago(OracleDataReader dr)
        //{
        //    List<ErrorDTO> listValues = new List<ErrorDTO>();

        //    if (dr.HasRows)
        //    {
        //        while (dr.Read())
        //        {
        //            ErrorDTO values = new ErrorDTO();
        //            values.ID_ERROR = dr.GetDecimal(0);
        //            values.DES_ERROR = values.ID_ERROR == 0 ? "" : dr.GetString(1);
        //            values.ID_OPERACION = dr.GetDecimal(2);

        //            listValues.Add(values);
        //        }
        //    }
        //    return listValues;
        //}
        #endregion

        public static List<ErrorDTO> eliminaPrepagoVC(PrepagoDTO prepagoDTO)
        {
            List<ErrorDTO> Value = new List<ErrorDTO>();

            // Crea el contexto genérico de base de datos
            GenericRepositoyDB contextDBRepository = new GenericRepositoyDB();

            try
            {
                // Carga los parámetros de entrada
                List<Parameter> inputParameters = new List<Parameter>
                {
                    new Parameter { name = "EN_ID_OPERACION", type = OracleDbType.Decimal, value = prepagoDTO.ID_OPERACION },
                    new Parameter { name = "EN_NUM_SECUENCIA", type = OracleDbType.Decimal, value = prepagoDTO.SEC_PREPAGO },
                    new Parameter { name = "EN_UNICO", type = OracleDbType.Decimal, value = prepagoDTO.MTO_CANTIDAD }
                };

                // Carga los parámetros de salida
                List<Parameter> outParameters = new List<Parameter>
                {
                    new Parameter { name = "RC_DATOS", isCursor = true }
                };

                // Apertura de conexión
                contextDBRepository.openConnection();

                // Ejecuta procedimiento de Selección
                contextDBRepository.execProcedure(inputParameters, AM_ELIMINAPREPAGOVC_PR, ref outParameters);

                // Cargar Datos
                Value = listError(((OracleDataReader)outParameters[0].value));
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

        public static List<ErrorDTO> eliminaGarantiaVC(GarantiaDTO garantiaDTO)
        {
            List<ErrorDTO> Value = new List<ErrorDTO>();

            // Crea el contexto genérico de base de datos
            GenericRepositoyDB contextDBRepository = new GenericRepositoyDB();

            try
            {
                // Carga los parámetros de entrada
                List<Parameter> inputParameters = new List<Parameter>
                {
                    new Parameter { name = "EN_ID_OPERACION", type = OracleDbType.Decimal, value = garantiaDTO.ID_OPERACION },
                    new Parameter { name = "EN_NUM_SECUENCIA", type = OracleDbType.Decimal, value = garantiaDTO.SEC_VALORES },
                    new Parameter { name = "EN_UNICO", type = OracleDbType.Decimal, value = garantiaDTO.MTO_CANTIDAD }
                };

                // Carga los parámetros de salida
                List<Parameter> outParameters = new List<Parameter>
                {
                    new Parameter { name = "RC_DATOS", isCursor = true }
                };

                // Apertura de conexión
                contextDBRepository.openConnection();

                // Ejecuta procedimiento de Selección
                contextDBRepository.execProcedure(inputParameters, AM_ELIMINAGARANTIAVC_PR, ref outParameters);

                // Cargar Datos
                Value = listError(((OracleDataReader)outParameters[0].value));
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

        private static List<ErrorDTO> listError(OracleDataReader dr)
        {
            List<ErrorDTO> listValues = new List<ErrorDTO>();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    ErrorDTO values = new ErrorDTO();
                    values.ID_ERROR = dr.GetDecimal(0);
                    values.DES_ERROR = values.ID_ERROR == 0 ? "" : dr.GetString(1);

                    listValues.Add(values);
                }
            }
            return listValues;
        }

        public static List<EventosDTO> getEventos(OperacionesEntity searchParameters)
        {
            List<EventosDTO> Value = new List<EventosDTO>();

            // Crea el contexto genérico de base de datos
            GenericRepositoyDB contextDBRepository = new GenericRepositoyDB();

            try
            {
                // Carga los parámetros de entrada
                List<Parameter> inputParameters = new List<Parameter>
                {
                    new Parameter { name = "EN_IDOPERACION", type = OracleDbType.Double, value = searchParameters.ID_OPERACION },
                    new Parameter { name = "EV_TIPOPERACION", type = OracleDbType.Varchar2, value = searchParameters.TIP_OPERACION }
                };

                // Carga los parámetros de salida
                List<Parameter> outParameters = new List<Parameter>
                {
                    new Parameter { name = "RC_DATOS", isCursor = true }
                };

                // Apertura de conexión
                contextDBRepository.openConnection();

                // Ejecuta procedimiento de Selección
                contextDBRepository.execProcedure(inputParameters, AM_GETEVENTOS_PR, ref outParameters);

                // Cargar Datos
                Value = listGetEventos(((OracleDataReader)outParameters[0].value));


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

        private static List<EventosDTO> listGetEventos(OracleDataReader dr)
        {
            List<EventosDTO> listValues = new List<EventosDTO>();
            //if (!dr.IsDBNull(dr.GetOrdinal("ProductState")))
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    EventosDTO values = new EventosDTO();

                    values.ID_OPERACION = dr.GetDecimal(0);
                    values.TIP_OPERACION = dr.GetString(1);
                    values.SEC_EVENTO = dr.GetDecimal(2);
                    values.ID_USUARIO = dr.GetString(3);
                    values.NOMBRE_USUARIO = dr.GetString(4);
                    values.FEC_CREACION = dr.GetDateTime(5);
                    try
                    { values.NOM_CAMPO = dr.GetString(6); }
                    catch
                    { values.NOM_CAMPO = ""; }
                    try
                    { values.VAL_CAMPO = dr.GetString(7); }
                    catch
                    { values.VAL_CAMPO = ""; }
                    try
                    { values.DES_EVENTO = dr.GetString(8); }
                    catch
                    { values.DES_EVENTO = ""; }

                    listValues.Add(values);
                }
            }
            return listValues;
        }


        public static List<FondosDCVDTO> getFondosDCV()
        {
            List<FondosDCVDTO> Value = new List<FondosDCVDTO>();

            // Crea el contexto genérico de base de datos
            GenericRepositoyDB contextDBRepository = new GenericRepositoyDB();

            try
            {
                List<Parameter> inputParameters = new List<Parameter>
                {
                    new Parameter { name = "EV_ACTIVO", type = OracleDbType.Varchar2, value = "1" }
                };

                // Carga los parámetros de salida
                List<Parameter> outParameters = new List<Parameter>
                {
                    new Parameter { name = "RC_DATOS", isCursor = true }
                };

                // Apertura de conexión
                contextDBRepository.openConnection();

                // Ejecuta procedimiento de Selección
                contextDBRepository.execProcedure(inputParameters, AM_GETFONDOSDCV_PR, ref outParameters);

                // Cargar Datos
                Value = listGetFondosDCV(((OracleDataReader)outParameters[0].value));


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

        #region ListgetPrepagos
        private static List<FondosDCVDTO> listGetFondosDCV(OracleDataReader dr)
        {
            List<FondosDCVDTO> listValues = new List<FondosDCVDTO>();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    FondosDCVDTO values = new FondosDCVDTO();
                    values.NOM_FONDO = dr.GetString(0);
                    values.BP_FONDO = dr.GetString(1);
                    values.CUENTA_DCV = dr.GetString(2);

                    listValues.Add(values);
                }
            }
            return listValues;
        }
        #endregion

        
        public static List<MailDTO> getMailVencimiento()
        {
            List<MailDTO> Value = new List<MailDTO>();

            // Crea el contexto genérico de base de datos
            GenericRepositoyDB contextDBRepository = new GenericRepositoyDB();

            try
            {
                List<Parameter> inputParameters = new List<Parameter>
                {
                    new Parameter { name = "EV_ACTIVO", type = OracleDbType.Varchar2, value = "1" }
                };

                // Carga los parámetros de salida
                List<Parameter> outParameters = new List<Parameter>
                {
                    new Parameter { name = "RC_DATOS", isCursor = true }
                };

                // Apertura de conexión
                contextDBRepository.openConnection();

                // Ejecuta procedimiento de Selección
                contextDBRepository.execProcedure(inputParameters, AM_GETMAILVCTO_PR, ref outParameters);

                // Cargar Datos
                Value = listGetMail(((OracleDataReader)outParameters[0].value));


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

        #region ListgetPrepagos
        private static List<MailDTO> listGetMail(OracleDataReader dr)
        {
            List<MailDTO> listValues = new List<MailDTO>();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    MailDTO values = new MailDTO();
                    values.Correo = dr.GetString(0);

                    listValues.Add(values);
                }
            }
            return listValues;
        }
        #endregion

    }
}