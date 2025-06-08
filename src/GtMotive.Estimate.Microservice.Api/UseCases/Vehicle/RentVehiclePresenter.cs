using System;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Vehicle
{
    public class RentVehiclePresenter : IRentOutputPort
    {
        private RentOutput _output;

        public IActionResult ActionResult => new OkObjectResult(new
        {
            _output.Vehicle
        });

        public void StandardHandle(RentOutput response)
        {
            _output = response ?? throw new ArgumentNullException(nameof(response));
        }
    }
}
