using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_GP_VentaCorta.Models.Negocio
{
    public class LogManager
    {

        #region Constructor
        /// <summary>
        /// Constructor que inicializa el proceso Log4Net responsable de escribir los eventos de error.
        /// </summary>
        static LogManager()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        #endregion

        #region LogError
        /// <summary>
        /// Método para registrar en el log un evento de error.
        /// </summary>
        /// <param name="messageLog">Mensaje de Error a registrar</param>
        public static void LogError(string messageLog)
        {
            ILog logger = log4net.LogManager.GetLogger("LogEvent");
            logger.Error("IP[" + ObtieneIP() + "] " + messageLog);
        }
        #endregion
        public static void LogInfo(string messageLog)
        {
            ILog logger = log4net.LogManager.GetLogger("LogEvent");
            logger.Info("IP[" + ObtieneIP() + "] " + messageLog);
        }
        private static string ObtieneIP()
        {
            string sIP;
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["x-forwarded-for"]))
            {
                sIP = HttpContext.Current.Request.ServerVariables["x-forwarded-for"];
            }
            else if (!string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["X_FORWARDED_FOR"]))
            {
                sIP = HttpContext.Current.Request.ServerVariables["X_FORWARDED_FOR"];
            }
            else if (!string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
            {
                sIP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            else
            {
                sIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            return sIP;
        }
    }
}