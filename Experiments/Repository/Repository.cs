using Experiments.Models;
using Experiments.Models.DTO;
using Microsoft.Data.SqlClient;
using System.Globalization;

namespace Experiments.Repository
{
    public class Repository : IRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<Repository> _logger;

        public Repository(ILogger<Repository> logger)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .Build();

            _connectionString = config["ConnectionStrings:DefaultConnection"];
            _logger = logger;
        }

        public async Task<bool> CreateExperiment(ExperimentDTO experiment)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"INSERT INTO Experiments(ExperimentKey, AssignedValue, CreatedDate) 
                                    VALUES (@ExperimentKey, @AssignedValue, @CreatedDate)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ExperimentKey", experiment.ExperimentKey);
                        command.Parameters.AddWithValue("@AssignedValue", experiment.AssignedValue);
                        command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

                        var res = await command.ExecuteNonQueryAsync();

                        return res > 0;
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

            return false;
        }

        public async Task<bool> CreateDevice(DeviceDTO device)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"INSERT INTO ClientDevices(Token, ExperimentId) 
                                    VALUES (@Token, @ExperimentId)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Token", device.Token);
                        command.Parameters.AddWithValue("@ExperimentId", device.ExperimentId);

                        return await command.ExecuteNonQueryAsync() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }

        public async Task<Device> GetDeviceByToken(string token)
        {
            var result = new Device();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"SELECT c.DeviceId, c.Token, e.ExperimentKey,o.ExpOption, e.CreatedDate
                                    FROM ClientDevices c
                                    INNER JOIN Experiments e ON c.ExperimentId = e.ExperimentId
                                    INNER JOIN Options o on e.AssignedValue = o.Id
                                    WHERE c.Token = @Token";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Token", token);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                result.Id = reader.GetInt32(0);
                                result.Token = reader.GetString(1);
                                result.Experiment = new Experiment()
                                {
                                    Key = reader.GetString(2),
                                    Option = reader.GetString(3),
                                    CreatedAt = reader.GetDateTime(4),
                                };

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return result;
        }

        public async Task<List<Option>> GetPriceOptions()
        {
            var options = new List<Option>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = "SELECT TOP 4 * FROM Options ORDER BY Id DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                options.Add(new Option
                                {
                                    Id = reader.GetInt32(0),
                                    ExpOption = reader.GetString(1)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

            return options;
        }

        public async Task<List<Option>> GetButtonColorOptions()
        {
            var options = new List<Option>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"SELECT TOP 3 * FROM Options";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                options.Add(new Option
                                {
                                    Id = reader.GetInt32(0),
                                    ExpOption = reader.GetString(1)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

            return options;
        }

        public async Task<int> GetLastExperimentId()
        {
            int res = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"SELECT TOP 1 ExperimentId FROM Experiments ORDER BY ExperimentId DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                res = reader.GetInt32(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

            return res;
        }

        public async Task<List<StatisticValue>> GetExperimentStatistic()
        {
            var values = new List<StatisticValue>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"SELECT e.ExperimentKey,
                                        o.ExpOption AS AssignedValue,
                                        FORMAT(COUNT(*) * 100.0 / SUM(COUNT(*)) OVER (PARTITION BY E.ExperimentKey), '0.##') AS Percentage
                                        FROM Experiments AS e
                                        JOIN Options AS o ON e.AssignedValue = o.Id
                                        GROUP BY e.ExperimentKey, o.ExpOption
                                        ORDER BY e.ExperimentKey, o.ExpOption";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                values.Add(new StatisticValue()
                                {
                                    ExperimentKey = reader.GetString(0),
                                    AssignedValue = reader.GetString(1),
                                    Probability = double.Parse(reader.GetString(2), NumberStyles.Float, CultureInfo.InvariantCulture)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

            return values;
        }
    }
}

