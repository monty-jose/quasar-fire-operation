using Microsoft.EntityFrameworkCore;
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

        public IEnumerable<MessagesSecret> Message()
        {
            return dataAccess.GetAllMessage();
        }

        public ResponseDTO MessageResponse()
        {
            ResponseDTO response = new ResponseDTO();

            List<MessageDTO> lastMessagesList = dataAccess.GetLastMessages(1);

            string [] lastMessage = lastMessagesList[0].message_array.Split(',');
            
            response.message    = this.GetMessage(lastMessage);
            response.position   = this.GetLocation(lastMessagesList[0].distances);

            return response;
        }

        public string GetMessage(string[] messages)
        {
            string[] msg ;            
            string[] msgCompare = new string[messages.Length]; //tomo el tamaño del mensaje que recibo

            List<MessageDTO> lastMessagesList = dataAccess.GetLastMessages(2);

            foreach (var item in lastMessagesList)
            {
                msg        = item.message_array.Split(',');
                msgCompare = UtilService.WordsCompare(messages, msg);
            }

            //string msgReturn = String.IsNullOrEmpty(msgCompare) == null ? UtilService.FormatText(msgCompare): String.Empty;

            string msgReturn = UtilService.FormatText(msgCompare);

            return msgReturn;
        }

        public PositionDTO GetLocation(double distance)
        {
            //PositionDTO position = UtilService.getLocationByTrilateration();

            PositionDTO position = new PositionDTO();
            position.x = 850;
            position.y = 500;

            return position;

        }
        
    }
}
