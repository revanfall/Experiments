using Experiments.Models;
using Experiments.Models.DTO;
using Experiments.Repository;
using Experiments.Service;
using Microsoft.AspNetCore.Mvc;

namespace Experiments.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExperimentController : ControllerBase
    {
        private IRepository _repository;
        private readonly ILogger<ExperimentController> _logger;
        public ExperimentController(IRepository repository, ILogger<ExperimentController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("{experiment_key}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetExperiment([FromRoute(Name = "experiment_key")] string experimentKey, [FromQuery] string deviceToken)
        {
            var device = await _repository.GetDeviceByToken(deviceToken);
            if (device.Id == 0)
            {
                List<Option> options = new();
                Option option = new();
                switch(experimentKey)
                {
                    case "button_color":
                        options = await _repository.GetButtonColorOptions();
                        option = ExperimentCalculator.CalculateButtonColorExperimentResult(options);
                        break;

                    case "price":
                        options = await _repository.GetPriceOptions();
                        option = ExperimentCalculator.CalculatePriceExperimentResult(options);
                        break;

                    default:
                        var message = $"Can not calculate the experiment data based on key: {experimentKey}";
                        _logger.LogError(message);
                        return BadRequest(mes);
                }

                var experimentDTO = new ExperimentDTO()
                {
                    ExperimentKey = experimentKey,
                    AssignedValue = option.Id,
                    CreatedDate = DateTime.Now,
                };

                var experimentCreated = await _repository.CreateExperiment(experimentDTO);
                if(experimentCreated)
                {
                    var lastExperimentId = await _repository.GetLastExperimentId();
                    var deviceCreated = await _repository.CreateDevice(new DeviceDTO()
                    {
                        Token = deviceToken,
                        ExperimentId = lastExperimentId
                    });

                    if(deviceCreated)
                    {
                        return Ok(new ResponseDTO() { Key = experimentDTO.ExperimentKey, Value = option.ExpOption});
                    }
                }
            }
            else
            {
                return Ok(new ResponseDTO() { Key = device.Experiment.Key, Value = device.Experiment.Option});
            }

            return BadRequest();
        }

    }
}

