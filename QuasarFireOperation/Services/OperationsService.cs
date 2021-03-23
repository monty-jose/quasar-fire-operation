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
                List<MessagesSecret> messagesSecretIdList = dataAccess.SaveMessagesList(requestSatelliteList);

                if (messagesSecretIdList.Count == 3)
                {
                    result = FindMessageLocation(requestSatelliteList[(int)Constant.SatelliteNumber.ONE]);
                }
                else
                {
                    result.error = true;
                    result.statusResponse = (int)Constant.StatusResponse.ERROR_PETICION;
                }

                ResponseEntity responseEntity = dataAccess.SaveResponseEntity(result.response.position, result.response.message, result.statusResponse);
                dataAccess.UpdateMessagesProcess(messagesSecretIdList, responseEntity.id);
            }
            catch (Exception ex)
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
                List<MessageDTO> lastMessagesList = dataAccess.GetLastMessages((int)Constant.NumberRow.THREE_ROWS);

                if (lastMessagesList.Count == 3)
                {
                    Satellites satellite = dataAccess.GetSatelliteById(lastMessagesList[(int)Constant.SatelliteNumber.THREE].satelite_id);
                    if (satellite != null)
                    {
                        SatelliteMessageDTO satelliteMessage = new SatelliteMessageDTO()
                        {
                            name     = satellite.name,
                            distance = lastMessagesList[(int)Constant.SatelliteNumber.THREE].distances,
                            message  = lastMessagesList[(int)Constant.SatelliteNumber.THREE].message_array.Split(',')
                        };

                        result = FindMessageLocation(satelliteMessage);
                        ResponseEntity responseEntity = dataAccess.SaveResponseEntity(result.response.position, result.response.message, result.statusResponse);                        
                    }
                }
            }
            catch (Exception ex)
            {
                result.error = true;
            }
                       
            return result;
        }

        public void UpdateSecretList(List<MessageDTO> lastMessagesList, int idResponse)
        {
            List<MessagesSecret> msgSecretList = new List<MessagesSecret>();

            foreach (var item in lastMessagesList)
            {
                var message = new MessagesSecret()
                {
                    id = item.id
                };
                msgSecretList.Add(message);
            }
            dataAccess.UpdateMessagesProcess(msgSecretList, idResponse);
        }

        public ResultDTO FindMessageLocation(SatelliteMessageDTO satelliteMessage)
        {
            ResultDTO result = new ResultDTO();
            result.response  = new ResponseDTO();

            result.response.message  = String.Empty;
            result.response.position = null;

            string[] firstMessage = satelliteMessage.message;
            string message        = GetMessage(firstMessage);

            if (!String.IsNullOrEmpty(message))
            {
                PositionDTO location = GetLocation(satelliteMessage.distance);

                if (location != null)
                {
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
                result.statusResponse = (int)Constant.StatusResponse.ERROR_MESSAGE;
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
                List<InformationMessageSatelliteDTO> infoAllSatelliteList = dataAccess.GetLastSatellite(infoFirstSatellite.id);
                infoAllSatelliteList.Add(infoFirstSatellite);

                if (infoAllSatelliteList.Count == 3)
                {
                    shipPosition = UtilService.GetLocationByTrilateration(infoAllSatelliteList[(int)Constant.SatelliteNumber.ONE], infoAllSatelliteList[(int)Constant.SatelliteNumber.TWO], infoAllSatelliteList[(int)Constant.SatelliteNumber.THREE]);
                }
            }

            return shipPosition;

        }

        public bool IsValidRequestList(List<SatelliteMessageDTO> requestSatelliteList)
        {
            foreach (var item in requestSatelliteList)
            {
                if (String.IsNullOrEmpty(item.name))
                    return false;
                if (item.distance == 0)
                    return false;
                if (item.message == null)
                    return false;                
            }
            return true;
        }

        public bool IsValidRequest(TopSecretSplitRequestDTO requestSatellite)
        {            
            if (requestSatellite.distance == 0)
                return false;
            if (requestSatellite.message == null)
                return false;
            
            return true;
        }
    }
}
