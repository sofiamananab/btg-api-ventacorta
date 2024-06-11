using API_GP_VentaCorta.Models.Datos.DTO;
using API_GP_VentaCorta.Models.Datos.DTO.Operaciones;
using API_GP_VentaCorta.Models.Datos.Repositorio.OperacionesVC;
using API_GP_VentaCorta.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_GP_VentaCorta.Models.Negocio.OperacionesVC
{
    public class OperacionesBO
    {
        public static List<OperacionesEntity> getOperacionesVC(ParametersDTO searchParameters)
        {
            List<OperacionesEntity> operacionesEntity = Operaciones.getOperacionesVC(searchParameters);

            //la consulta es por una operacion, se traen los datos anexos prepagos/garantias
            if (searchParameters.IdOperacion != -1)
            {
                if (operacionesEntity.Count > 0)
                {
                    if (searchParameters.IdOperacion == -2)
                    {
                        //llenar todo lo encontrado con garantias y prepagos

                        foreach (OperacionesEntity operaciones in operacionesEntity)
                        {
                            searchParameters.IdOperacion = operaciones.ID_OPERACION;
                            operaciones.prepagoDTO = Operaciones.getPrepagosVC(searchParameters);
                            operaciones.garantiaDTO = Operaciones.getGarantiasVC(searchParameters);
                        }
                    }
                    else
                    {
                        operacionesEntity[0].prepagoDTO = Operaciones.getPrepagosVC(searchParameters);
                        operacionesEntity[0].garantiaDTO = Operaciones.getGarantiasVC(searchParameters);
                    }
                }
            }

            return operacionesEntity;
        }

        public static List<ErrorDTO> grabaOperacionesVC(OperacionesEntity operacionesEntity)
        {
            List<ErrorDTO> listErroDTO = Operaciones.grabaOperacionesVC(operacionesEntity);
            //se debe grabar prepagos y garantias segun id operacion!

            if (listErroDTO[0].ID_ERROR == 0)
            {
                try
                {
                    //eliminar la no continuidad  1-3 -> se elimina el 2 y tambien la cola
                    eliminarPrepagosVC(operacionesEntity);
                    //renumerar ?
                    //insertar --> procedimiento pregunta si no existe inserta
                    insertarPrepagosVC(operacionesEntity);
                }
                catch (Exception e)
                {
                    listErroDTO[0].ID_ERROR = 1;
                    listErroDTO[0].DES_ERROR = "Prepagos:" + e.Message.ToString() ;
                }
                try
                {
                    //eliminar la no continuidad  1-3 -> se elimina el 2 y tambien la cola
                    eliminarGarantiasVC(operacionesEntity);
                    //renumerar ?
                    //insertar --> procedimiento pregunta si no existe inserta
                    insertarGarantiasVC(operacionesEntity);
                }
                catch (Exception e)
                {
                    listErroDTO[0].ID_ERROR = 1;
                    listErroDTO[0].DES_ERROR = "Garantias:" + e.Message.ToString();
                }
            }

            return listErroDTO;
        }
        private static void eliminarPrepagosVC(OperacionesEntity operacionesEntity)
        {
            decimal nSecuencia = 0;

            if (operacionesEntity.prepagoDTO == null) { return; }
            if (operacionesEntity.prepagoDTO.Count == 0) {
                //elimina la cola
                Operaciones.eliminaPrepagoVC(new PrepagoDTO() { ID_OPERACION = operacionesEntity.ID_OPERACION, SEC_PREPAGO = nSecuencia, MTO_CANTIDAD = 1 });
                return; }

            //recorrer prepago
            //eliminar la no continuidad  1-3 -> se elimina el 2

            List<PrepagoDTO> listaEliminar = new List<PrepagoDTO>();


            foreach (PrepagoDTO prepago in operacionesEntity.prepagoDTO)
            {
                if (prepago.SEC_PREPAGO != nSecuencia)
                {
                    if ((prepago.SEC_PREPAGO - nSecuencia) >= 2)
                    {
                        for (decimal j = nSecuencia + 1; j < prepago.SEC_PREPAGO - 1; j++)
                        {
                            listaEliminar.Add(new PrepagoDTO() { ID_OPERACION = operacionesEntity.ID_OPERACION, SEC_PREPAGO = j, MTO_CANTIDAD = 0 });
                        }
                    }
                    nSecuencia = prepago.SEC_PREPAGO;
                }
            }
            //elimina la no continuidad
            foreach (PrepagoDTO prepagoEliminar in listaEliminar)
            {
                Operaciones.eliminaPrepagoVC(prepagoEliminar);
            }
            //elimina la cola
            Operaciones.eliminaPrepagoVC(new PrepagoDTO() { ID_OPERACION = operacionesEntity.ID_OPERACION, SEC_PREPAGO = nSecuencia, MTO_CANTIDAD = 1 });
        }
        private static void insertarPrepagosVC(OperacionesEntity operacionesEntity)
        {
            if (operacionesEntity.prepagoDTO == null) { return; }
            if (operacionesEntity.prepagoDTO.Count == 0) { return; }

            decimal nSecuencia = 0;

            foreach (PrepagoDTO prepago in operacionesEntity.prepagoDTO)
            {
                ++nSecuencia;
                prepago.SEC_PREPAGO = nSecuencia; //renumera
                Operaciones.insertaPrepagoVC(prepago);
            }
        }
        private static void eliminarGarantiasVC(OperacionesEntity operacionesEntity)
        {
            decimal nSecuencia = 0;

            if (operacionesEntity.garantiaDTO == null) { return; }
            if (operacionesEntity.garantiaDTO.Count == 0) {
                Operaciones.eliminaGarantiaVC(new GarantiaDTO() { ID_OPERACION = operacionesEntity.ID_OPERACION, SEC_VALORES = nSecuencia, MTO_CANTIDAD = 1 });
                return; }

            //recorrer prepago
            //eliminar la no continuidad  1-3 -> se elimina el 2

            List<GarantiaDTO> listaEliminar = new List<GarantiaDTO>();


            foreach (GarantiaDTO garantia in operacionesEntity.garantiaDTO)
            {
                if (garantia.SEC_VALORES != nSecuencia)
                {
                    if ((garantia.SEC_VALORES - nSecuencia) >= 2)
                    {
                        for (decimal j = nSecuencia + 1; j < garantia.SEC_VALORES - 1; j++)
                        {
                            listaEliminar.Add(new GarantiaDTO() { ID_OPERACION = operacionesEntity.ID_OPERACION, SEC_VALORES = j, MTO_CANTIDAD = 0 });
                        }
                    }
                    nSecuencia = garantia.SEC_VALORES;
                }
            }
            //elimina la no continuidad
            foreach (GarantiaDTO garantiaEliminar in listaEliminar)
            {
                Operaciones.eliminaGarantiaVC(garantiaEliminar);
            }
            //elimina la cola
            Operaciones.eliminaGarantiaVC(new GarantiaDTO() { ID_OPERACION = operacionesEntity.ID_OPERACION, SEC_VALORES = nSecuencia, MTO_CANTIDAD = 1 });
        }
        private static void insertarGarantiasVC(OperacionesEntity operacionesEntity)
        {
            if (operacionesEntity.garantiaDTO == null) { return; }
            if (operacionesEntity.garantiaDTO.Count == 0) { return; }

            decimal nSecuencia = 0;

            foreach (GarantiaDTO garantia in operacionesEntity.garantiaDTO)
            {
                ++nSecuencia;
                garantia.SEC_VALORES = nSecuencia; //renumera
                Operaciones.insertaGarantiaVC(garantia);
            }
        }
        public static List<ErrorDTO> renovarOperacionesVC(OperacionesEntity operacionesEntity)
        {

            return Operaciones.renovarOperacionesVC(operacionesEntity);
        }
        public static List<ErrorDTO> vencerOperacionesVC(OperacionesEntity operacionesEntity)
        {

            return Operaciones.vencerOperacionesVC(operacionesEntity);
        }

        public static List<EventosDTO> getEventos(OperacionesEntity searchParameters)
        {
            List<EventosDTO> operacionesEntity = Operaciones.getEventos(searchParameters);

            return operacionesEntity;
        }
        public static List<FondosDCVDTO> getFondosDCV()
        {
            List<FondosDCVDTO> operacionesEntity = Operaciones.getFondosDCV();

            return operacionesEntity;
        }

        public static List<MailDTO> getMailVencimiento()
        {
            return Operaciones.getMailVencimiento();
        }
        

    }
}