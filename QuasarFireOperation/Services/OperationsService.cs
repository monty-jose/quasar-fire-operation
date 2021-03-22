using Microsoft.EntityFrameworkCore;
using QuasarFireOperation.Common;
using QuasarFireOperation.Context;
using QuasarFireOperation.Entities;
using QuasarFireOperation.Models;
using QuasarFireOperation.Repository;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace QuasarFireOperation.Services
{
    public class OperationsService
    {
        private readonly DataAccessRepository dataAccess;

        public OperationsService(DataAccessRepository dataAccess)
        {
            this.dataAccess =  dataAccess;
        }

        public ResultDTO TopSecretResponse(List<SatelliteMessageDTO> requestSatelliteList)
        {
            ResultDTO result = new ResultDTO();

            try
            {
                if (IsValidRequest(requestSatelliteList))
                {
                    List<MessagesSecret> messagesSecretIdList = dataAccess.SaveMessagesList(requestSatelliteList);

                    if (messagesSecretIdList.Count == 3)
                    {
                        result = FindMessageLocation(requestSatelliteList[0]);
                    }
                    else
                    {
                        result.error = true;
                        result.statusResponse = (int)Constant.StatusResponse.ERROR_PETICION;
                    }

                    ResponseEntity responseEntity = dataAccess.SaveResponse(result.response.position, result.response.message, result.statusResponse);
                    dataAccess.UpdateMessagesProcess(messagesSecretIdList, responseEntity.id);

                }
                else
                {
                    result.error = true;
                }

            }
            catch (Exception)
            {
                result.error = true;
            }

            return result;
        }

        public ResultDTO TopSecretSplitPost(TopSecretSplitRequestDTO requestSatellite, string satellite_name)
        {
            ResultDTO result = new ResultDTO();

            try
            {
                List<SatelliteMessageDTO> satelliteMessageList = new List<SatelliteMessageDTO>();
                satelliteMessageList.Add(new SatelliteMessageDTO()
                {
                    name     = satellite_name,
                    distance = requestSatellite.distance,
                    message  = requestSatellite.message
                });

                List<MessagesSecret> messagesSecretIdList = dataAccess.SaveMessagesList(satelliteMessageList);
                result.error = false;

            }
            catch (Exception)
            {
                result.error = true;
            }           

            return result;                
        }

        public ResultDTO TopSecretSplitGet()
        {
            ResultDTO result = new ResultDTO();
            result.error     = true;

            try
            {
                List<MessageDTO> lastMessagesList = dataAccess.GetLastMessages((int)Constant.NumberRow.ONE_ROW);

                if (lastMessagesList.Count > 0)
                {
                    Satellites satellite = dataAccess.GetSatelliteById(lastMessagesList[0].satelite_id);
                    if (satellite != null)
                    {
                        SatelliteMessageDTO satelliteMessage = new SatelliteMessageDTO()
                        {
                            name     = satellite.name,
                            distance = lastMessagesList[0].distances,
                            message  = lastMessagesList[0].message_array.Split(',')
                        };

                        result = FindMessageLocation(satelliteMessage);

                        //VERIFICAR ESTOOOOO COMO ACTUALIZAAR LOS MENSAJES GUARDADOS
                        ResponseEntity responseEntity = dataAccess.SaveResponse(result.response.position, result.response.message, result.statusResponse);
                        //dataAccess.UpdateMessagesProcess(messagesSecretIdList, responseEntity.id);
                    }
                }
            }
            catch (Exception)
            {
                result.error = true;
            }
                       
            return result;
        }

        public ResultDTO FindMessageLocation(SatelliteMessageDTO satelliteMessage)
        {
            ResultDTO result = new ResultDTO();

            string[] firstMessage = satelliteMessage.message;
            string message        = GetMessage(firstMessage);

            if (!String.IsNullOrEmpty(message))
            {
                PositionDTO location = GetLocation(satelliteMessage.distance);

                if (location != null)
                {
                    result.response          = new ResponseDTO();
                    result.response.message  = message;
                    result.response.position = location;
                    result.error             = false;
                    result.statusResponse    = (int)Constant.StatusResponse.SUCCESS_SENT;
                }
                else
                {
                    result.error          = true;
                    result.statusResponse = (int)Constant.StatusResponse.ERROR_LOCATION;
                }

            }
            else
            {
                result.error = true;
                result.statusResponse = (int)Constant.StatusResponse.ERROR_LOCATION;
            }
            return result;
        }
               
        public string GetMessage(string[] messages)
        {
            string[] msg ;
            string msgReturn    = String.Empty;
            string[] msgCompare = messages; //tomo el tamaño del mensaje que recibo

            List<MessageDTO> lastMessagesList = dataAccess.GetLastMessages((int)Constant.NumberRow.TWO_ROWS);

            if (lastMessagesList.Count > 1)
            {
                foreach (var item in lastMessagesList)
                {
                    msg        = item.message_array.Split(',');
                    msgCompare = UtilService.WordsCompare(msgCompare, msg);
                }

                msgReturn = UtilService.FormatText(msgCompare);
            } 
            return msgReturn;
        }

        public PositionDTO GetLocation(double distance)
        {
            InformationMessageSatelliteDTO infoFirstSatellite = dataAccess.GetSatelliteLocation(distance);
            PositionDTO shipPosition = null;

            if (infoFirstSatellite != null)
            {
                List<InformationMessageSatelliteDTO> infoAllSatelliteList = dataAccess.GetLastSatellite((int)Constant.NumberRow.TWO_ROWS, infoFirstSatellite.id);
                infoAllSatelliteList.Add(infoFirstSatellite);

                if (infoAllSatelliteList.Count == 3)
                {
                    shipPosition = UtilService.GetLocationByTrilateration(infoAllSatelliteList[(int)Constant.NumberSatellite.ONE], infoAllSatelliteList[(int)Constant.NumberSatellite.TWO], infoAllSatelliteList[(int)Constant.NumberSatellite.THREE]);
                }
            }

            return shipPosition;

        }

        public bool IsValidRequest(List<SatelliteMessageDTO> requestSatelliteList)
        {
            foreach (var item in requestSatelliteList)
            {
                if (item.distance == 0)
                    return false;
                if (item.message == null)
                    return false;                
            }
            return true;
        }
    }
}
