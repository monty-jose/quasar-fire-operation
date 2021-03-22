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
            ResultDTO result     = new ResultDTO();           

            if (IsValidRequest(requestSatelliteList))
            {
                PositionDTO location = null;
                int statusResponse   = 0;
                string message       = String.Empty;

                List<MessagesSecret> messagesSecretIdList = dataAccess.SaveMessagesList(requestSatelliteList);
             
                if (messagesSecretIdList.Count == 3)
                {
                    string[] firstMessage = requestSatelliteList[0].message;
                    message               = GetMessage(firstMessage);

                    if (!String.IsNullOrEmpty(message))
                    {                       
                        location  = GetLocation(requestSatelliteList[0].distance);

                        if (location != null)
                        {   
                            result.response          = new ResponseDTO();
                            result.response.message  = message;
                            result.response.position = location;
                            result.error             = false;
                            statusResponse           = Constant.StatusResponse.SUCCESS_SENT;
                        }
                        else
                        {
                            result.error   = true;
                            statusResponse = Constant.StatusResponse.ERROR_LOCATION;
                        }

                    }
                    else 
                    {
                        result.error   = true;
                        statusResponse = Constant.StatusResponse.ERROR_LOCATION;
                    }
                }
                else 
                {
                    result.error    = true;
                    statusResponse  = Constant.StatusResponse.ERROR_PETICION;
                }

                ResponseEntity responseEntity = dataAccess.SaveResponse(location, message, statusResponse);
                dataAccess.UpdateMessagesProcess(messagesSecretIdList, responseEntity.id);
                
            }
            else
            {
                result.error = true;
            }

            return result;
        }

        public ResponseDTO MessageResponse()
        {
            ResponseDTO response = new ResponseDTO();

            List<MessageDTO> lastMessagesList = dataAccess.GetLastMessages(1);

            string [] lastMessage = lastMessagesList[0].message_array.Split(',');

            PositionDTO position = this.GetLocation(lastMessagesList[0].distances);
            response.message     = this.GetMessage(lastMessage);
            response.position    = position;

            return response;
        }

        public string GetMessage(string[] messages)
        {
            string[] msg ;
            string msgReturn    = String.Empty;
            string[] msgCompare = messages; //tomo el tamaño del mensaje que recibo

            List<MessageDTO> lastMessagesList = dataAccess.GetLastMessages(2);

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
                List<InformationMessageSatelliteDTO> infoAllSatelliteList = dataAccess.GetLastSatellite(2, infoFirstSatellite.id);
                infoAllSatelliteList.Add(infoFirstSatellite);

                if (infoAllSatelliteList.Count == 3)
                {
                    shipPosition = UtilService.GetLocationByTrilateration(infoAllSatelliteList[0], infoAllSatelliteList[1], infoAllSatelliteList[2]);                    
                }
            }

            return shipPosition;

        }

        public bool IsValidRequest(List<SatelliteMessageDTO> requestSatelliteList)
        {
            foreach (var item in requestSatelliteList)
            {
                if (item.distance == null)
                    return false;
                if (item.message == null)
                    return false;                
            }
            return true;
        }
    }
}
