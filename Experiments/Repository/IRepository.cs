using Experiments.Models;
using Experiments.Models.DTO;

namespace Experiments.Repository
{
    public interface IRepository
    {
        /// <summary>
        /// Creates a new experiment record in the database.
        /// Inserts a new experiment record into the database with the provided information.
        /// </summary>
        /// <param name="experiment">The experiment information to be inserted.</param>
        /// <returns>True if the experiment was successfully created, otherwise false.</returns>
        Task<bool> CreateExperiment(ExperimentDTO experiment);

        /// <summary>
        /// Creates a new device record in the database.
        /// </summary>
        /// <param name="device">The device information to be inserted.</param>
        /// <returns>True if the device was successfully created, otherwise false.</returns>
        Task<bool> CreateDevice(DeviceDTO device);

        /// <summary>
        /// Retrieves a device record from the database by its token.
        /// </summary>
        /// <param name="token">The token of the device to be retrieved.</param>
        /// <returns>The device information if found, or an empty device if not found.</returns>
        Task<Device> GetDeviceByToken(string token);

        /// <summary>
        /// Retrieves the list of button color options from the database.
        /// </summary>
        /// <returns>The list of button color options.</returns>
        Task<List<Option>> GetButtonColorOptions();

        /// <summary>
        /// Retrieves the list of price options from the database.
        /// </summary>
        /// <returns>The list of price options.</returns>
        Task<List<Option>> GetPriceOptions();

        /// <summary>
        /// Retrieves the ID of the last created experiment from the database.
        /// </summary>
        /// <returns>The ID of the last created experiment.</returns>
        Task<int> GetLastExperimentId();

        /// <summary>
        /// Retrieves experiment statistics from the database.
        /// </summary>
        /// <returns>A list of statistic values containing experiment key, assigned value, and probability.</returns>
        Task<List<StatisticValue>> GetExperimentStatistic();
    }
}
