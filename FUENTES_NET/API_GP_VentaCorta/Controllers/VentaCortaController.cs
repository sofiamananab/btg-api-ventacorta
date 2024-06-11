using API_GP_VentaCorta.Models.Datos.DTO;
using API_GP_VentaCorta.Models.Datos.DTO.Operaciones;
using API_GP_VentaCorta.Models.DTO;
using API_GP_VentaCorta.Models.Negocio;
using API_GP_VentaCorta.Models.Negocio.OperacionesVC;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API_GP_VentaCorta.Controllers
{
    public class VentaCortaController : ApiController
    {
        //// GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}
        [HttpPost]
        // GET api/<controller>/5
        [Route("getDatosArchivoValorizacion")]
        public HttpResponseMessage getDatosArchivoValorizacion(ParametersDTO searchParameters)
        {

            DTOAPIResponse dTOAPIResponse = new DTOAPIResponse();
            List<ArchivoValorizacion> listValues;//= new List<ArchivoValorizacion>();
            DateTime fecha = DateTime.Now;
            string parametros = Newtonsoft.Json.JsonConvert.SerializeObject(searchParameters);
            string cadenaResult;

            try
            {
                LogManager.LogInfo("API_GP_VentaCorta VentaCortaController (getDatosArchivoValorizacion) : Inicio " + Newtonsoft.Json.JsonConvert.SerializeObject(searchParameters));

                listValues = ValorizacionRepository.getDatosArchivoValorizacion(searchParameters);
                dTOAPIResponse.data = Newtonsoft.Json.JsonConvert.SerializeObject(listValues);
                dTOAPIResponse.code = "0";
                dTOAPIResponse.message = "";
                dTOAPIResponse.timestamp = fecha.ToString("o", CultureInfo.InvariantCulture);
                dTOAPIResponse.param = parametros;
            }
            catch (Exception ex)
            {

                //    LogManager.LogError("API_SADCO_GARANTIAS  estadoCuenta Error: " + ex.Message);
                dTOAPIResponse.code = "1";
                dTOAPIResponse.message = ex.Message;
                dTOAPIResponse.timestamp = fecha.ToString("o", CultureInfo.InvariantCulture);
                dTOAPIResponse.param = parametros;
            }

            cadenaResult = Newtonsoft.Json.JsonConvert.SerializeObject(dTOAPIResponse);

            LogManager.LogInfo("API_GP_VentaCorta VentaCortaController (getDatosArchivoValorizacion) : salida " + Newtonsoft.Json.JsonConvert.SerializeObject(cadenaResult));


            return new HttpResponseMessage((dTOAPIResponse.code != "0" ? HttpStatusCode.InternalServerError : HttpStatusCode.OK))
            {
                Content = new StringContent(cadenaResult, System.Text.Encoding.UTF8, "application/json")
            };
        }
        [HttpPost]
        // GET api/<controller>/5
        [Route("getNemoValor")]
        public HttpResponseMessage getNemoValor(ArchivoValorizacion archivoValorizacion)
        {

            DTOAPIResponse dTOAPIResponse = new DTOAPIResponse();
            List<ArchivoValorizacion> listValues;//= new List<ArchivoValorizacion>();
            DateTime fecha = DateTime.Now;
            string parametros = Newtonsoft.Json.JsonConvert.SerializeObject(archivoValorizacion);
            string cadenaResult;

            try
            {
                LogManager.LogInfo("API_GP_VentaCorta VentaCortaController (getNemoValor) : Inicio " + Newtonsoft.Json.JsonConvert.SerializeObject(archivoValorizacion));

                listValues = ValorizacionRepository.getNemoValor(archivoValorizacion.NOM_NEMOTECNICO);
                dTOAPIResponse.data = Newtonsoft.Json.JsonConvert.SerializeObject(listValues);
                dTOAPIResponse.code = "0";
                dTOAPIResponse.message = "";
                dTOAPIResponse.timestamp = fecha.ToString("o", CultureInfo.InvariantCulture);
                dTOAPIResponse.param = parametros;
            }
            catch (Exception ex)
            {

                //    LogManager.LogError("API_SADCO_GARANTIAS  estadoCuenta Error: " + ex.Message);
                dTOAPIResponse.code = "1";
                dTOAPIResponse.message = ex.Message;
                dTOAPIResponse.timestamp = fecha.ToString("o", CultureInfo.InvariantCulture);
                dTOAPIResponse.param = parametros;
            }

            cadenaResult = Newtonsoft.Json.JsonConvert.SerializeObject(dTOAPIResponse);

            LogManager.LogInfo("API_GP_VentaCorta VentaCortaController (getNemoValor) : salida " + Newtonsoft.Json.JsonConvert.SerializeObject(cadenaResult));


            return new HttpResponseMessage((dTOAPIResponse.code != "0" ? HttpStatusCode.InternalServerError : HttpStatusCode.OK))
            {
                Content = new StringContent(cadenaResult, System.Text.Encoding.UTF8, "application/json")
            };
        }

        
        [HttpPost]
        // GET api/<controller>/5
        [Route("getNemoAutocomplete")]
        public HttpResponseMessage getNemoAutocomplete(ArchivoValorizacion archivoValorizacion)
        {

            DTOAPIResponse dTOAPIResponse = new DTOAPIResponse();
            List<ArchivoValorizacion> listValues;//= new List<ArchivoValorizacion>();
            DateTime fecha = DateTime.Now;
            string parametros = Newtonsoft.Json.JsonConvert.SerializeObject(archivoValorizacion);
            string cadenaResult;

            try
            {
                LogManager.LogInfo("API_GP_VentaCorta VentaCortaController (getNemoAutocomplete) : Inicio " + Newtonsoft.Json.JsonConvert.SerializeObject(archivoValorizacion));

                listValues = ValorizacionRepository.getNemoAutocomplete(archivoValorizacion.NOM_NEMOTECNICO);
                dTOAPIResponse.data = Newtonsoft.Json.JsonConvert.SerializeObject(listValues);
                dTOAPIResponse.code = "0";
                dTOAPIResponse.message = "";
                dTOAPIResponse.timestamp = fecha.ToString("o", CultureInfo.InvariantCulture);
                dTOAPIResponse.param = parametros;
            }
            catch (Exception ex)
            {

                //    LogManager.LogError("API_SADCO_GARANTIAS  estadoCuenta Error: " + ex.Message);
                dTOAPIResponse.code = "1";
                dTOAPIResponse.message = ex.Message;
                dTOAPIResponse.timestamp = fecha.ToString("o", CultureInfo.InvariantCulture);
                dTOAPIResponse.param = parametros;
            }

            cadenaResult = Newtonsoft.Json.JsonConvert.SerializeObject(dTOAPIResponse);

            LogManager.LogInfo("API_GP_VentaCorta VentaCortaController (getNemoAutocomplete) : salida " + Newtonsoft.Json.JsonConvert.SerializeObject(cadenaResult));


            return new HttpResponseMessage((dTOAPIResponse.code != "0" ? HttpStatusCode.InternalServerError : HttpStatusCode.OK))
            {
                Content = new StringContent(cadenaResult, System.Text.Encoding.UTF8, "application/json")
            };
        }


        [HttpPost]
        [Route("CargaArchivoValorizacion")]
        // POST api/<controller>
        public HttpResponseMessage CargaArchivoValorizacion(ArchivoDTO cargaArchivoParametros)
        {
            List<ErrorEntity> listaError;//= new List<ErrorEntity>();
            DTOAPIResponse dTOAPIResponse = new DTOAPIResponse();
            DateTime fecha = DateTime.Now;
            string parametros = Newtonsoft.Json.JsonConvert.SerializeObject(cargaArchivoParametros);
            string cadenaResult;

            try
            {
                LogManager.LogInfo("API_GP_VentaCorta VentaCortaController (CargaArchivoValorizacion) : Inicio " + Newtonsoft.Json.JsonConvert.SerializeObject(cargaArchivoParametros));
                listaError = ValorizacionRepository.DesactivaCargaArchivoValorizacion("0");

                dTOAPIResponse = ValorizacionRepository.CargaArchivoValorizacion(cargaArchivoParametros);
                //dTOAPIResponse.code = "0";
                //dTOAPIResponse.message = "";
                dTOAPIResponse.timestamp = fecha.ToString("o", CultureInfo.InvariantCulture);
                dTOAPIResponse.param = parametros;
            }
            catch (Exception ex)
            {
                listaError = ValorizacionRepository.DesactivaCargaArchivoValorizacion("1");
                //    LogManager.LogError("API_SADCO_GARANTIAS  estadoCuenta Error: " + ex.Message);
                dTOAPIResponse.code = "1";
                dTOAPIResponse.message = ex.Message;
                dTOAPIResponse.timestamp = fecha.ToString("o", CultureInfo.InvariantCulture);
                dTOAPIResponse.param = parametros;
            }

            cadenaResult = Newtonsoft.Json.JsonConvert.SerializeObject(dTOAPIResponse);
            LogManager.LogInfo("API_GP_VentaCorta VentaCortaController (CargaArchivoValorizacion) : salida " + Newtonsoft.Json.JsonConvert.SerializeObject(cadenaResult)); 

//            return new HttpResponseMessage((dTOAPIResponse.code != "0" ? HttpStatusCode.InternalServerError : HttpStatusCode.OK))
            return new HttpResponseMessage( HttpStatusCode.OK)
            {
                Content = new StringContent(cadenaResult, System.Text.Encoding.UTF8, "application/json")
            };
        }

        // PUT api/<controller>/5
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}

        [HttpPost]
        // GET api/<controller>/5
        [Route("getVCOperaciones")]
        public HttpResponseMessage getVCOperaciones(ParametersDTO searchParameters)
        {

            DTOAPIResponse dTOAPIResponse = new DTOAPIResponse();
            List<OperacionesEntity> listValues;// = new List<OperacionesEntity>();
            DateTime fecha = DateTime.Now;
            string parametros = Newtonsoft.Json.JsonConvert.SerializeObject(searchParameters);
            string cadenaResult;

            try
            {
                LogManager.LogInfo("API_GP_VentaCorta VentaCortaController (getVCOperaciones) : Inicio " + Newtonsoft.Json.JsonConvert.SerializeObject(searchParameters));

                listValues = OperacionesBO.getOperacionesVC(searchParameters);
                dTOAPIResponse.data = Newtonsoft.Json.JsonConvert.SerializeObject(listValues);
                dTOAPIResponse.code = "0";
                dTOAPIResponse.message = "";
                dTOAPIResponse.timestamp = fecha.ToString("o", CultureInfo.InvariantCulture);
                dTOAPIResponse.param = parametros;
            }
            catch (Exception ex)
            {

                //    LogManager.LogError("API_SADCO_GARANTIAS  estadoCuenta Error: " + ex.Message);
                dTOAPIResponse.code = "1";
                dTOAPIResponse.message = ex.Message;
                dTOAPIResponse.timestamp = fecha.ToString("o", CultureInfo.InvariantCulture);
                dTOAPIResponse.param = parametros;
            }

            cadenaResult = Newtonsoft.Json.JsonConvert.SerializeObject(dTOAPIResponse);

            LogManager.LogInfo("API_GP_VentaCorta VentaCortaController (getVCOperaciones) : salida " + Newtonsoft.Json.JsonConvert.SerializeObject(cadenaResult));


            return new HttpResponseMessage((dTOAPIResponse.code != "0" ? HttpStatusCode.InternalServerError : HttpStatusCode.OK))
            {
                Content = new StringContent(cadenaResult, System.Text.Encoding.UTF8, "application/json")
            };
        }

        [HttpPost]
        // GET api/<controller>/5
        [Route("GrabaVCOperaciones")]
        public HttpResponseMessage GrabaVCOperaciones(OperacionesEntity operacionesEntity)
        {

            DTOAPIResponse dTOAPIResponse = new DTOAPIResponse();
            List<ErrorDTO> listValues;//= new List<ErrorDTO>();
            DateTime fecha = DateTime.Now;
            string parametros = Newtonsoft.Json.JsonConvert.SerializeObject(operacionesEntity);
            string cadenaResult;

            try
            {
                LogManager.LogInfo("API_GP_VentaCorta VentaCortaController (GrabaVCOperaciones) : Inicio " + Newtonsoft.Json.JsonConvert.SerializeObject(operacionesEntity));

                listValues = OperacionesBO.grabaOperacionesVC(operacionesEntity);


                dTOAPIResponse.data = Newtonsoft.Json.JsonConvert.SerializeObject(listValues);
                dTOAPIResponse.code = "0";
                dTOAPIResponse.message = "";
                dTOAPIResponse.timestamp = fecha.ToString("o", CultureInfo.InvariantCulture);
                dTOAPIResponse.param = parametros;
            }
            catch (Exception ex)
            {

                //    LogManager.LogError("API_SADCO_GARANTIAS  estadoCuenta Error: " + ex.Message);
                dTOAPIResponse.code = "1";
                dTOAPIResponse.message = ex.Message;
                dTOAPIResponse.timestamp = fecha.ToString("o", CultureInfo.InvariantCulture);
                dTOAPIResponse.param = parametros;
            }

            cadenaResult = Newtonsoft.Json.JsonConvert.SerializeObject(dTOAPIResponse);

            LogManager.LogInfo("API_GP_VentaCorta VentaCortaController (GrabaVCOperaciones) : salida " + Newtonsoft.Json.JsonConvert.SerializeObject(cadenaResult));


            return new HttpResponseMessage((dTOAPIResponse.code != "0" ? HttpStatusCode.InternalServerError : HttpStatusCode.OK))
            {
                Content = new StringContent(cadenaResult, System.Text.Encoding.UTF8, "application/json")
            };
        }

        [HttpPost]
        // GET api/<controller>/5
        [Route("RenovarVCOperaciones")]
        public HttpResponseMessage RenovarVCOperaciones(OperacionesEntity operacionesEntity)
        {

            DTOAPIResponse dTOAPIResponse = new DTOAPIResponse();
            List<ErrorDTO> listValues;//= new List<ErrorDTO>();
            DateTime fecha = DateTime.Now;
            string parametros = Newtonsoft.Json.JsonConvert.SerializeObject(operacionesEntity);
            string cadenaResult;

            try
            {
                LogManager.LogInfo("API_GP_VentaCorta VentaCortaController (RenovarVCOperaciones) : Inicio " + Newtonsoft.Json.JsonConvert.SerializeObject(operacionesEntity));

                listValues = OperacionesBO.renovarOperacionesVC(operacionesEntity);


                dTOAPIResponse.data = Newtonsoft.Json.JsonConvert.SerializeObject(listValues);
                dTOAPIResponse.code = "0";
                dTOAPIResponse.message = "";
                dTOAPIResponse.timestamp = fecha.ToString("o", CultureInfo.InvariantCulture);
                dTOAPIResponse.param = parametros;
            }
            catch (Exception ex)
            {

                //    LogManager.LogError("API_SADCO_GARANTIAS  estadoCuenta Error: " + ex.Message);
                dTOAPIResponse.code = "1";
                dTOAPIResponse.message = ex.Message;
                dTOAPIResponse.timestamp = fecha.ToString("o", CultureInfo.InvariantCulture);
                dTOAPIResponse.param = parametros;
            }

            cadenaResult = Newtonsoft.Json.JsonConvert.SerializeObject(dTOAPIResponse);

            LogManager.LogInfo("API_GP_VentaCorta VentaCortaController (RenovarVCOperaciones) : salida " + Newtonsoft.Json.JsonConvert.SerializeObject(cadenaResult));


            return new HttpResponseMessage((dTOAPIResponse.code != "0" ? HttpStatusCode.InternalServerError : HttpStatusCode.OK))
            {
                Content = new StringContent(cadenaResult, System.Text.Encoding.UTF8, "application/json")
            };
        }
        [HttpPost]
        // GET api/<controller>/5
        [Route("VencerVCOperaciones")]
        public HttpResponseMessage VencerVCOperaciones(OperacionesEntity operacionesEntity)
        {

            DTOAPIResponse dTOAPIResponse = new DTOAPIResponse();
            List<ErrorDTO> listValues;//= new List<ErrorDTO>();
            DateTime fecha = DateTime.Now;
            string parametros = Newtonsoft.Json.JsonConvert.SerializeObject(operacionesEntity);
            string cadenaResult;

            try
            {
                LogManager.LogInfo("API_GP_VentaCorta VentaCortaController (VencerVCOperaciones) : Inicio " + Newtonsoft.Json.JsonConvert.SerializeObject(operacionesEntity));

                listValues = OperacionesBO.vencerOperacionesVC(operacionesEntity);


                dTOAPIResponse.data = Newtonsoft.Json.JsonConvert.SerializeObject(listValues);
                dTOAPIResponse.code = "0";
                dTOAPIResponse.message = "";
                dTOAPIResponse.timestamp = fecha.ToString("o", CultureInfo.InvariantCulture);
                dTOAPIResponse.param = parametros;
            }
            catch (Exception ex)
            {

                //    LogManager.LogError("API_SADCO_GARANTIAS  estadoCuenta Error: " + ex.Message);
                dTOAPIResponse.code = "1";
                dTOAPIResponse.message = ex.Message;
                dTOAPIResponse.timestamp = fecha.ToString("o", CultureInfo.InvariantCulture);
                dTOAPIResponse.param = parametros;
            }

            cadenaResult = Newtonsoft.Json.JsonConvert.SerializeObject(dTOAPIResponse);

            LogManager.LogInfo("API_GP_VentaCorta VentaCortaController (VencerVCOperaciones) : salida " + Newtonsoft.Json.JsonConvert.SerializeObject(cadenaResult));


            return new HttpResponseMessage((dTOAPIResponse.code != "0" ? HttpStatusCode.InternalServerError : HttpStatusCode.OK))
            {
                Content = new StringContent(cadenaResult, System.Text.Encoding.UTF8, "application/json")
            };
        }

        [HttpPost]
        // GET api/<controller>/5
        [Route("getEventos")]
        public HttpResponseMessage getEventos(OperacionesEntity searchParameters)
        {

            DTOAPIResponse dTOAPIResponse = new DTOAPIResponse();
            List<EventosDTO> listValues;//= new List<EventosDTO>();
            DateTime fecha = DateTime.Now;
            string parametros = Newtonsoft.Json.JsonConvert.SerializeObject(searchParameters);
            string cadenaResult;

            try
            {
                LogManager.LogInfo("API_GP_VentaCorta VentaCortaController (getEventos) : Inicio " + Newtonsoft.Json.JsonConvert.SerializeObject(searchParameters));

                listValues = OperacionesBO.getEventos(searchParameters);
                dTOAPIResponse.data = Newtonsoft.Json.JsonConvert.SerializeObject(listValues);
                dTOAPIResponse.code = "0";
                dTOAPIResponse.message = "";
                dTOAPIResponse.timestamp = fecha.ToString("o", CultureInfo.InvariantCulture);
                dTOAPIResponse.param = parametros;
            }
            catch (Exception ex)
            {

                //    LogManager.LogError("API_SADCO_GARANTIAS  estadoCuenta Error: " + ex.Message);
                dTOAPIResponse.code = "1";
                dTOAPIResponse.message = ex.Message;
                dTOAPIResponse.timestamp = fecha.ToString("o", CultureInfo.InvariantCulture);
                dTOAPIResponse.param = parametros;
            }

            cadenaResult = Newtonsoft.Json.JsonConvert.SerializeObject(dTOAPIResponse);

            LogManager.LogInfo("API_GP_VentaCorta VentaCortaController (getEventos) : salida " + Newtonsoft.Json.JsonConvert.SerializeObject(cadenaResult));


            return new HttpResponseMessage((dTOAPIResponse.code != "0" ? HttpStatusCode.InternalServerError : HttpStatusCode.OK))
            {
                Content = new StringContent(cadenaResult, System.Text.Encoding.UTF8, "application/json")
            };
        }

        [HttpPost]
        // GET api/<controller>/5
        [Route("getFondosDCV")]
        public HttpResponseMessage getFondosDCV()
        {

            DTOAPIResponse dTOAPIResponse = new DTOAPIResponse();
            List<FondosDCVDTO> listValues;// = new List<OperacionesEntity>();
            DateTime fecha = DateTime.Now;
            string cadenaResult;

            try
            {
                LogManager.LogInfo("API_GP_VentaCorta VentaCortaController (getFondosDCV) : Inicio ");

                listValues = OperacionesBO.getFondosDCV();
                dTOAPIResponse.data = Newtonsoft.Json.JsonConvert.SerializeObject(listValues);
                dTOAPIResponse.code = "0";
                dTOAPIResponse.message = "";
                dTOAPIResponse.timestamp = fecha.ToString("o", CultureInfo.InvariantCulture);
                dTOAPIResponse.param = "";
            }
            catch (Exception ex)
            {

                //    LogManager.LogError("API_SADCO_GARANTIAS  estadoCuenta Error: " + ex.Message);
                dTOAPIResponse.code = "1";
                dTOAPIResponse.message = ex.Message;
                dTOAPIResponse.timestamp = fecha.ToString("o", CultureInfo.InvariantCulture);
                dTOAPIResponse.param = "";
            }

            cadenaResult = Newtonsoft.Json.JsonConvert.SerializeObject(dTOAPIResponse);

            LogManager.LogInfo("API_GP_VentaCorta VentaCortaController (getFondosDCV) : salida " + Newtonsoft.Json.JsonConvert.SerializeObject(cadenaResult));


            return new HttpResponseMessage((dTOAPIResponse.code != "0" ? HttpStatusCode.InternalServerError : HttpStatusCode.OK))
            {
                Content = new StringContent(cadenaResult, System.Text.Encoding.UTF8, "application/json")
            };
        }

        [HttpPost]
        // GET api/<controller>/5
        [Route("getMailVencimiento")]
        public HttpResponseMessage getMailVencimiento()
        {

            DTOAPIResponse dTOAPIResponse = new DTOAPIResponse();
            List<MailDTO> listValues;// = new List<OperacionesEntity>();
            DateTime fecha = DateTime.Now;
            string cadenaResult;

            try
            {
                LogManager.LogInfo("API_GP_VentaCorta VentaCortaController (getMailVencimiento) : Inicio ");

                listValues = OperacionesBO.getMailVencimiento();
                dTOAPIResponse.data = Newtonsoft.Json.JsonConvert.SerializeObject(listValues);
                dTOAPIResponse.code = "0";
                dTOAPIResponse.message = "";
                dTOAPIResponse.timestamp = fecha.ToString("o", CultureInfo.InvariantCulture);
                dTOAPIResponse.param = "";
            }
            catch (Exception ex)
            {

                //    LogManager.LogError("API_SADCO_GARANTIAS  estadoCuenta Error: " + ex.Message);
                dTOAPIResponse.code = "1";
                dTOAPIResponse.message = ex.Message;
                dTOAPIResponse.timestamp = fecha.ToString("o", CultureInfo.InvariantCulture);
                dTOAPIResponse.param = "";
            }

            cadenaResult = Newtonsoft.Json.JsonConvert.SerializeObject(dTOAPIResponse);

            LogManager.LogInfo("API_GP_VentaCorta VentaCortaController (getMailVencimiento) : salida " + Newtonsoft.Json.JsonConvert.SerializeObject(cadenaResult));


            return new HttpResponseMessage((dTOAPIResponse.code != "0" ? HttpStatusCode.InternalServerError : HttpStatusCode.OK))
            {
                Content = new StringContent(cadenaResult, System.Text.Encoding.UTF8, "application/json")
            };
        }


    }
}
