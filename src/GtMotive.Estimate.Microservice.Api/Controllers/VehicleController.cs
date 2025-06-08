using System;
using System.ComponentModel;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.UseCases.Vehicle;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
#if DEBUG
    [AllowAnonymous]
#else
    [Authorize]
#endif
    public class VehicleController : ControllerBase
    {
        private readonly IUseCase<RentInput> _rentVehicleUseCase;
        private readonly IUseCase<CreateInput> _createVehicleUseCase;
        private readonly GetAllVehiclesPresenter _getAllVehiclesPresenter;
        private readonly RentVehiclePresenter _rentVehiclePresenter;
        private readonly GetAllUseCase _getAllVehiclesUseCase;
        private readonly IUseCase<ReturnInput> _returnVehicleUseCase;

        public VehicleController(
            IUseCase<CreateInput> createVehicleUseCase,
            IUseCase<RentInput> rentVehicleUseCase,
            GetAllUseCase getAllVehiclesUseCase,
            RentVehiclePresenter rentVehiclePresenter,
            GetAllVehiclesPresenter getAllVehiclesPresenter,
            IUseCase<ReturnInput> returnVehicleUseCase)
        {
            _createVehicleUseCase = createVehicleUseCase ?? throw new ArgumentNullException(nameof(createVehicleUseCase));
            _getAllVehiclesUseCase = getAllVehiclesUseCase ?? throw new ArgumentNullException(nameof(getAllVehiclesUseCase));
            _getAllVehiclesPresenter = getAllVehiclesPresenter ?? throw new ArgumentNullException(nameof(getAllVehiclesPresenter));
            _rentVehiclePresenter = rentVehiclePresenter ?? throw new ArgumentNullException(nameof(rentVehiclePresenter));
            _rentVehicleUseCase = rentVehicleUseCase ?? throw new ArgumentNullException(nameof(rentVehicleUseCase));
            _returnVehicleUseCase = returnVehicleUseCase ?? throw new ArgumentNullException(nameof(returnVehicleUseCase));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Description("Register a new vehicle on the company.")]
        public async Task<IActionResult> CreateVehicle([FromBody] CreateInput input)
        {
            try
            {
                await _createVehicleUseCase.Execute(input);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
        }

        [HttpGet]
        [Description("Get all vehicles registered on the company.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllVehicles()
        {
            try
            {
                await _getAllVehiclesUseCase.Execute();
                return _getAllVehiclesPresenter.ActionResult;
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("rent")]
        [Description("Mark as rented an available vehicle for a Client. Creates the client if doesn't exist.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RentVehicle([FromBody] RentInput input)
        {
            try
            {
                if (input == null)
                {
                    return BadRequest(new { error = "Input cannot be null." });
                }

                await _rentVehicleUseCase.Execute(input);
                return _rentVehiclePresenter.ActionResult;
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPut("return")]
        [Description("Mark as available a vehicle previously rented.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ReturnVehicle([FromBody] ReturnInput input)
        {
            try
            {
                await _returnVehicleUseCase.Execute(input);

                return Ok(new { message = "Vehicle returned successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}
