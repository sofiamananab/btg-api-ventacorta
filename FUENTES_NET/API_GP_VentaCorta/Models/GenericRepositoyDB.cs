using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;


namespace API_GP_VentaCorta.Models
{
    public class GenericRepositoyDB
    {
        #region Const
        private const string CONVERT_MESSAGE = "No se pudo convertir el tipo";
        #endregion

        #region Variables
        protected OracleConnection connection;
        protected OracleCommand command;
        protected OracleTransaction transaction;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase que inicializa los objetos de conexión a la BD y el objeto commando para ejecutar los procedimientos almacenados.
        /// </summary>
        public GenericRepositoyDB()
        {
            connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ORADBCONN"].ConnectionString);

            command = new OracleCommand();
            command.Connection = connection;
        }
        #endregion

        #region Objects Provider
        #region addParameter
        /// <summary>
        /// Método que agrega un parámetro al objeto command. Este método se ejecutará N veces como tantos parámetros tenga el procedimiento almacenado.
        /// </summary>
        /// <param name="parameterName">Nombre del Parámetro</param>
        /// <param name="value">Valor del Parámetro</param>
        public void addParameter(string parameterName, object value, OracleDbType type)
        {
            OracleParameter parameter = new OracleParameter(parameterName, type);
            parameter.Direction = ParameterDirection.Input;
            parameter.Value = value;

            //OracleParameter parameter = new OracleParameter(parameterName, value);
            command.Parameters.Add(parameter);
        }

        //public void addParameter(string parameterName, DbType type, ParameterDirection parameterDirection, int size)
        //parameter.OracleDbType = DbTypeToOracleDbType(type);
        public void addParameter(string parameterName, OracleDbType type, ParameterDirection parameterDirection, int size)
        {
            //Solo si es que tiene seteado un largo de retorno
            if (size > 0)
            {
                OracleParameter parameter = new OracleParameter();
                parameter.ParameterName = parameterName;
                parameter.Size = size;
                parameter.Direction = parameterDirection;
                parameter.OracleDbType = type;
                command.Parameters.Add(parameter);
            }
            else
            {
                addParameter(parameterName, type, parameterDirection);
            }

        }

        /// <summary>
        /// Método que agrega un parámetro al objeto command. Este método se ejecutará N veces como tantos parámetros tenga el procedimiento almacenado.
        /// </summary>
        /// <param name="parameterName">Nombre del Parámetro</param>
        /// <param name="type">Tipo de Dato del parámetro (Proveedor .Net)</param>
        /// <param name="parameterDirection">Indica si el parámetro es de Entrada o Salida</param>
        public void addParameter(string parameterName, DbType type, ParameterDirection parameterDirection)
        {
            OracleParameter parameter = new OracleParameter();
            parameter.ParameterName = parameterName;
            parameter.Direction = parameterDirection;
            parameter.OracleDbType = DbTypeToOracleDbType(type);
            command.Parameters.Add(parameter);
        }

        /// <summary>
        /// Método que agrega un parámetro al objeto command. Este método se ejecutará N veces como tantos parámetros tenga el procedimiento almacenado.
        /// </summary>
        /// <param name="parameterName">Nombre del Parámetro</param>
        /// <param name="type">Tipo de Dato del parámetro (Proveedor Oracle)</param>
        /// <param name="parameterDirection">Indica si el parámetro es de Entrada o Salida</param>
        public void addParameter(string parameterName, OracleDbType type, ParameterDirection parameterDirection)
        {
            OracleParameter parameter = new OracleParameter();
            parameter.ParameterName = parameterName;
            parameter.Direction = parameterDirection;
            parameter.OracleDbType = type;
            command.Parameters.Add(parameter);
        }
        #endregion

        #region prepareCommand
        /// <summary>
        /// Método que setea el objeto comando que ejecutará la sentencia SQL o procedimiento almacenado.
        /// </summary>
        /// <param name="commandType">Tipo del Comando a ejecutar Ej. sentencia SQL o procedimiento almacenado</param>
        /// <param name="commandText">Glosa que contiene una sentencia SQL o procedimiento almacenado</param>
        public void prepareCommand(CommandType commandType, string commandText)
        {
            command.Parameters.Clear();
            command.CommandType = commandType;
            command.CommandText = commandText;
        }
        #endregion

        #region executeCommand
        /// <summary>
        /// Método que ejecuta la sentencia SQL o procedimiento almacenado.
        /// </summary>
        public void executeCommand()
        {
            command.ExecuteNonQuery();
        }
        #endregion
        #endregion

        #region Connection Operations
        #region openConnection
        /// <summary>
        /// Método que abre la conexión de la BD.
        /// </summary>
        public void openConnection()
        {
            connection.Open();
        }
        #endregion

        #region closeConnection
        /// <summary>
        /// Método que cierra la conexión de la BD.
        /// </summary>
        public void closeConnection()
        {
            if (connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }
        }
        #endregion
        #endregion

        #region Transaction Operations
        #region beginTransaction
        /// <summary>
        /// Método que inicia una Transacción.
        /// </summary>
        public void beginTransaction()
        {
            if (connection.State == ConnectionState.Open)
            {
                transaction = connection.BeginTransaction();
            }
        }
        #endregion

        #region commitTransaction
        /// <summary>
        /// Método que persiste los cambios en las entidades afectadas.
        /// </summary>
        public void commitTransaction()
        {
            if (connection.State == ConnectionState.Open)
            {
                transaction.Commit();
            }
        }
        #endregion

        #region rollbackTransaction
        /// <summary>
        /// Método que revierte los cambios en las entidades afectadas.
        /// </summary>
        public void rollbackTransaction()
        {
            if (connection.State == ConnectionState.Open)
            {
                transaction.Rollback();
            }
        }
        #endregion
        #endregion

        #region Database Operations
        #region execProcedure
        /// <summary>
        /// Método que ejecuta un comando de procedimiento almacenado con parámetros de entrada y salida.
        /// </summary>
        /// <param name="inputParameters">Lista de los parámetros de entrada.</param>
        /// <param name="commandText">Nombre del procedimiento almacenado.</param>
        /// <param name="outParameters">Lista de los parámetros de Salida.</param>
        public void execProcedure(List<Parameter> inputParameters, string commandText, ref List<Parameter> outParameters)
        {
            // Setea el objeto comando que ejecuta el procedimiento almacenado
            prepareCommand(CommandType.StoredProcedure, commandText);

            // Carga parámetros de entrada
            if (inputParameters != null)
            {
                foreach (Parameter param in inputParameters)
                {
                    addParameter(param.name, param.value, param.type);
                }
            }
            // Carga parámetros de salida
            if (outParameters != null)
            {
                foreach (Parameter param in outParameters)
                {
                    // Valida si el parámetro es un cursor para asignar el tipo correspondiente
                    if (param.isCursor)
                    {
                        addParameter(param.name, OracleDbType.RefCursor, ParameterDirection.Output);
                    }
                    else
                    {
                        addParameter(param.name, param.type, ParameterDirection.Output, param.size);
                    }
                }
            }
            // Ejacuta procedimiento almacenado
            executeCommand();

            // Asigna los valores de los parámetros de salida retornado por el procedimiento almacenado
            foreach (Parameter param in outParameters)
            {
                if (param.isCursor)
                {
                    OracleDataReader odr = ((OracleRefCursor)command.Parameters[param.name].Value).GetDataReader();
                    IDataReader dr = (IDataReader)odr;
                    param.value = dr;
                }
                else
                {
                    param.value = command.Parameters[param.name].Value;
                }
            }
        }

        /// <summary>
        /// Método que ejecuta un comando de procedimiento almacenado sin retorno de parámetros de salida.
        /// </summary>
        /// <param name="inputParameters">Lista de los parámetros de entrada.</param>
        /// <param name="commandText">Nombre del procedimiento almacenado.</param>
        public void execProcedure(List<Parameter> inputParameters, string commandText)
        {
            // Setea el objeto comando que ejecuta el procedimiento almacenado
            prepareCommand(CommandType.StoredProcedure, commandText);

            // Carga parámetros de entrada
            if (inputParameters != null)
            {
                foreach (Parameter param in inputParameters)
                {
                    addParameter(param.name, param.value, OracleDbType.Varchar2);
                }
            }
            // Ejacuta procedimiento almacenado
            executeCommand();
        }
        #endregion

        #region execFunction
        /// <summary>
        /// Método que ejecuta un comando de procedimiento almacenado con retorno de un valor.
        /// </summary>
        /// <param name="inputParameters">Lista de los parámetros de entrada.</param>
        /// <param name="commandText">Nombre del procedimiento almacenado.</param>
        /// <param name="retorno">Parámetro de Retorno.</param>
        /// <param name="output">Parámetro de Salida.</param>
        public void execFunction(List<Parameter> inputParameters, string commandText, ref OracleParameter retorno, OracleParameter output)
        {
            // Setea el objeto comando que ejecuta
            prepareCommand(CommandType.StoredProcedure, commandText);

            retorno = command.Parameters.Add(output);

            // Carga parámetros de entrada
            if (inputParameters != null)
            {
                foreach (Parameter param in inputParameters)
                {
                    addParameter(param.name, param.value, OracleDbType.Varchar2);
                }
            }

            retorno.Direction = ParameterDirection.ReturnValue;
            command.ExecuteNonQuery();
        }
        #endregion

        #region execView
        /// <summary>
        /// Método que ejecuta una consulta a las vistas.
        /// </summary> 
        /// <param name="commandText">Nombre de la vista.</param>
        /// <param name="retorno">lista de Retorno.</param>
        /// <param name="output">Parámetro de Salida.</param>
        public void execView(string commandText, ref DataSet retorno)
        {
            // Setea el objeto comando que ejecuta
            prepareCommand(CommandType.StoredProcedure, commandText);

            OracleDataAdapter adapter = new OracleDataAdapter(command);
            adapter.Fill(retorno);

            executeCommand();
        }
        #endregion
        #endregion

        #region Helpers
        #region OracleDbTypeToDbType
        /// <summary>
        /// Método para convertir un tipo de datos del proveedor Oracle a .Net.
        /// </summary>
        /// <param name="oraDbType">Tipo de dato del proveedor Oracle</param>
        public DbType OracleDbTypeToDbType(OracleDbType oraDbType)
        {
            switch (oraDbType)
            {
                case OracleDbType.Varchar2:
                case OracleDbType.Char:
                case OracleDbType.NChar:
                case OracleDbType.NVarchar2:
                case OracleDbType.XmlType:
                    return DbType.String;
                case OracleDbType.Int32:
                    return DbType.Int32;
                case OracleDbType.Single:
                    return DbType.Single;
                case OracleDbType.Double:
                    return DbType.Double;
                case OracleDbType.Decimal:
                    return DbType.Decimal;
                case OracleDbType.Date:
                case OracleDbType.TimeStamp:
                case OracleDbType.TimeStampTZ:
                case OracleDbType.TimeStampLTZ:
                    return DbType.DateTime;
                case OracleDbType.IntervalDS:
                    return DbType.Time;
                case OracleDbType.IntervalYM:
                    return DbType.Int32;
                case OracleDbType.Long:
                case OracleDbType.Clob:
                    return DbType.String;
                case OracleDbType.Raw:
                case OracleDbType.LongRaw:
                case OracleDbType.Blob:
                case OracleDbType.BFile:
                    return DbType.Binary;
                case OracleDbType.RefCursor:
                //case OracleDbType.Array:
                //case OracleDbType.Object:
                //case OracleDbType.Ref:
                //case OracleDbType.Boolean:
                //    return DbType.Boolean;
                default:
                    throw new NotSupportedException(CONVERT_MESSAGE + oraDbType.ToString().ToUpper());
            }
        }
        #endregion

        #region DbTypeToOracleDbType
        /// <summary>
        /// Método para convertir un tipo de datos del proveedor .Net a Oracle.
        /// </summary>
        /// <param name="oraDbType">Tipo de dato del proveedor .Net</param>
        private OracleDbType DbTypeToOracleDbType(DbType dbType)
        {
            switch (dbType)
            {
                case DbType.AnsiString:
                case DbType.String:
                    return OracleDbType.Varchar2;
                case DbType.AnsiStringFixedLength:
                case DbType.StringFixedLength:
                    return OracleDbType.Char;
                case DbType.Byte:
                case DbType.Int16:
                case DbType.SByte:
                case DbType.UInt16:
                case DbType.Int32:
                    return OracleDbType.Int32;
                case DbType.Single:
                    return OracleDbType.Single;
                case DbType.Double:
                    return OracleDbType.Double;
                case DbType.Date:
                    return OracleDbType.Date;
                case DbType.DateTime:
                    return OracleDbType.TimeStamp;
                case DbType.Time:
                    return OracleDbType.IntervalDS;
                case DbType.Binary:
                    return OracleDbType.Blob;
                //case DbType.Boolean:
                //    return OracleDbType.Boolean;
                case DbType.Int64:
                case DbType.UInt64:
                case DbType.VarNumeric:
                case DbType.Decimal:
                case DbType.Currency:
                    return OracleDbType.Decimal;
                //case DbType.Object:
                //    return OracleDbType.Object;
                case DbType.Guid:
                    return OracleDbType.Raw;
                default:
                    throw new NotSupportedException(CONVERT_MESSAGE + dbType.ToString().ToUpper());
            }
        }
        #endregion
        #endregion
    }
}