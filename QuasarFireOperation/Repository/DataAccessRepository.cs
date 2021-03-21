using Microsoft.EntityFrameworkCore;
using QuasarFireOperation.Context;
using QuasarFireOperation.Entities;
using QuasarFireOperation.Models;
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
                
        public IEnumerable<MessagesSecret> GetAllMessage()
        {
            return _context.Messages.ToList();
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
                                    "FROM MESSAGES_SECRET M INNER JOIN SATELLITES S ON (S.id=M.satelite_id) " +
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

        public List<PositionDTO> GetLastLocation(int countMessage)
        {
            List<PositionDTO> lastLocation = new List<PositionDTO>();

            var conn = _context.Database.GetDbConnection();
            try
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    string query = "SELECT TOP " + countMessage + " M.id, distance, S.position_x, S.position_y  " +
                                    "FROM MESSAGES_SECRET M INNER JOIN SATELLITES S ON (S.id=M.satelite_id) " +
                                    "ORDER BY date_process DESC";
                    command.CommandText = query;
                    DbDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            PositionDTO msg = new PositionDTO
                            {
                                x = (int)reader[0],
                                y = (int)reader[1]
                            };
                            lastLocation.Add(msg);
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

            return lastLocation;
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
    }
}
