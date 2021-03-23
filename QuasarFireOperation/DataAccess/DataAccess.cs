using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuasarFireOperation.Context;
using QuasarFireOperation.Entities;
using QuasarFireOperation.Models;
using QuasarFireOperation.Services;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace QuasarFireOperation.Repository
{
    public class DataAccess
    {
        private readonly AppDbContext _context;

        public DataAccess(AppDbContext context)
        {
            this._context = context;
        }

        public List<MessagesSecret> SaveMessagesList(List<SatelliteMessageDTO> satellites)
        {
            List<MessagesSecret> messagesList = new List<MessagesSecret>();

            foreach (SatelliteMessageDTO satelliteMenssage in satellites)
            {                
                Satellites satellite = this.GetSatelliteByName(satelliteMenssage.name);

                var msg = new MessagesSecret()
                {
                    satelite_id  = satellite.id,
                    distance     = satelliteMenssage.distance,
                    message      = UtilService.ArrayToString(satelliteMenssage.message).ToLower(),
                    process      = 0,
                    date_process = DateTime.Now
                };
                var entity = this.SaveMessagesSecret(msg);

                messagesList.Add(msg);

            }

            return messagesList;
        }

        public List<MessageDTO> GetLastMessages( int countMessage)
        {
            List<MessageDTO> lastMenssages = new List<MessageDTO>();

            var conn = _context.Database.GetDbConnection();
            try
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    string query = "SELECT TOP "+ countMessage + " M.id, satelite_id, distance, message " +
                                    "FROM MESSAGES_SECRET M  " +                                    
                                    "ORDER BY M.id DESC";
                    command.CommandText = query;
                    DbDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            MessageDTO msg = new MessageDTO
                            {
                                id              = (int)reader[0],
                                satelite_id     = (int)reader[1],
                                distances       = (double)reader[2],
                                message_array   = reader.GetString(3)
                            };
                            lastMenssages.Add(msg);
                        }
                    }
                    reader.Dispose();
                }
                conn.Close();
            }
            catch (System.Exception ex)
            {
                conn.Close();
            }
            
            return lastMenssages;
        }
                
        public Satellites GetSatelliteById(int id)
        {
            Satellites satellite = _context.Satellites.FirstOrDefault(p => p.id == id);
            return satellite;
        }

        public Satellites GetSatelliteByName(string name)
        {
            Satellites satellite = _context.Satellites.FirstOrDefault(p => p.name == name);
            return satellite;
        }

        public MessagesSecret SaveMessagesSecret(MessagesSecret messageSecret)
        {
            _context.Add(messageSecret);
            _context.SaveChanges();
            return messageSecret;
        }

        public ResponseEntity SaveResponseEntity(PositionDTO position, string _message, int _status_id)
        {
            ResponseEntity response = new ResponseEntity()
            {
                x_location = position == null ? 0 : position.x,
                y_location = position == null ? 0 : position.y,
                message    = _message,
                status_id  = _status_id

            };

            _context.Add(response);
            _context.SaveChanges();

            return response;                 
        }

        public void UpdateMessagesProcess(List<MessagesSecret> idList, int responseId)
        {
            foreach (var message in idList)
            {
                message.process     = 1;
                message.response_id = responseId;

                _context.Update(message);
                _context.SaveChanges();
            }            
        }

        public InformationMessageSatelliteDTO GetSatelliteLocation(double distance)
        {
            InformationMessageSatelliteDTO informationMessageSatellite = null;

            var conn = _context.Database.GetDbConnection();
            try
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    string query = "SELECT TOP 1 S.id, distance, S.position_x, S.position_y  " +
                                    "FROM MESSAGES_SECRET M INNER JOIN SATELLITES S ON (S.id=M.satelite_id) " +
                                    "WHERE distance = " + distance + " " +
                                    "ORDER BY M.id DESC";
                    command.CommandText = query;
                    DbDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            informationMessageSatellite = new InformationMessageSatelliteDTO
                            {
                                id          = (int)reader[0],
                                distance    = (double)reader[1],
                                x_position  = (double)reader[2],
                                y_position  = (double)reader[3]
                            };
                        }
                    }
                    reader.Dispose();
                }
                conn.Close();
            }
            catch (System.Exception ex)
            {
                conn.Close();
            }

            return informationMessageSatellite;
        }

        public List<InformationMessageSatelliteDTO> GetLastSatellite(int satelliteId)
        {
            List<InformationMessageSatelliteDTO> informationSatelliteList = new List<InformationMessageSatelliteDTO>();

            var conn = _context.Database.GetDbConnection();
            try
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    string query = "SELECT S.id, distance, S.position_x, S.position_y  " +
                                    "FROM MESSAGES_SECRET M INNER JOIN SATELLITES S ON (S.id=M.satelite_id) " +
                                    "WHERE satelite_id != " + satelliteId + 
                                    " AND  M.id  in (SELECT Max(id) " +
                                                    "FROM MESSAGES_SECRET M " +
                                                    "GROUP BY satelite_id) " +
                                    "ORDER BY M.id DESC";

                    command.CommandText = query;
                    DbDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var informationMessageSatellite = new InformationMessageSatelliteDTO
                            {
                                id          = (int)reader[0],
                                distance    = (double)reader[1],
                                x_position  = (double)reader[2],
                                y_position  = (double)reader[3]
                            };

                            informationSatelliteList.Add(informationMessageSatellite);
                        }
                    }
                    reader.Dispose();
                }
                conn.Close();
            }
            catch (System.Exception ex)
            {
                conn.Close();              
            }

            return informationSatelliteList;
        }


    }
}
