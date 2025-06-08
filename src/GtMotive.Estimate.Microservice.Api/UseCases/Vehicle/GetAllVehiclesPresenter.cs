using System;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Vehicle
{
    public class GetAllVehiclesPresenter : IGetAllOutputPort
    {
        private GetAllOutput _output;

        public IActionResult ActionResult => new OkObjectResult(new
        {
            _output.Vehicles,
            _output.TotalCount
        });

        public void StandardHandle(GetAllOutput response)
        {
            _output = response ?? throw new ArgumentNullException(nameof(response));
        }
    }
}
