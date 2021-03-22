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
    public class DataAccessRepository
    {
        private readonly AppDbContext _context;

        public DataAccessRepository(AppDbContext context)
        {
            this._context = context;
        }

        public List<MessagesSecret> SaveMessagesList(List<SatelliteMessageDTO> satellites)
        {
            List<MessagesSecret> messagesList = new List<MessagesSecret>();

            foreach (SatelliteMessageDTO satelliteMenssage in satellites)
            {
                //buscar en bd el id del satelite
                Satellites satellite = this.GetSatelliteByName(satelliteMenssage.name);

                var msg = new MessagesSecret()
                {
                    satelite_id  = satellite.id,
                    distance     = satelliteMenssage.distance,
                    message      = UtilService.ArrayToString(satelliteMenssage.message),
                    process      = 0,
                    date_process = DateTime.Now
                };

                // guardo el mensaje en bs de datos y retorno el id para despues actualizar el campo process
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
                                    "WHERE process = 0 " +
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
            }
            catch (System.Exception ex)
            {
                conn.Close();
            }
            finally
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

        public ResponseEntity SaveResponse(PositionDTO position, string _message, int _status_id)
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
                                    "WHERE distance = " + distance + " AND process = 0" +
                                    "ORDER BY date_process DESC";
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
            }
            catch (System.Exception ex)
            {
                conn.Close();
                throw;
            }
            finally
            {
                conn.Close();
            }

            return informationMessageSatellite;
        }

        public List<InformationMessageSatelliteDTO> GetLastSatellite(int countMessage, int satelliteId)
        {
            List<InformationMessageSatelliteDTO> informationSatelliteList = new List<InformationMessageSatelliteDTO>();

            var conn = _context.Database.GetDbConnection();
            try
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    string query = "SELECT TOP "+ countMessage + " S.id, distance, S.position_x, S.position_y  " +
                                    "FROM MESSAGES_SECRET M INNER JOIN SATELLITES S ON (S.id=M.satelite_id) " +
                                    "WHERE satelite_id != " + satelliteId + " AND process = 0" +
                                    "ORDER BY date_process DESC";

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
            }
            catch (System.Exception ex)
            {
                conn.Close();
                throw;
            }
            finally
            {
                conn.Close();
            }

            return informationSatelliteList;
        }


    }
}
