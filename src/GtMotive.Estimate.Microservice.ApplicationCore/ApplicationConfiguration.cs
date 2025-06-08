using System;
using System.Diagnostics.CodeAnalysis;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.Handler;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.Messages;
using GtMotive.Estimate.Microservice.Domain.Validations;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

[assembly: CLSCompliant(false)]

namespace GtMotive.Estimate.Microservice.ApplicationCore
{
    /// <summary>
    /// Adds Use Cases classes.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ApplicationConfiguration
    {
        /// <summary>
        /// Adds Use Cases to the ServiceCollection.
        /// </summary>
        /// <param name="services">Service Collection.</param>
        /// <returns>The modified instance.</returns>
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            services.AddScoped<GetAllUseCase>();
            services.AddScoped<IUseCase<CreateInput>, CreateUseCase>();
            services.AddScoped<IUseCase<RentInput>, RentUseCase>();
            services.AddScoped<IUseCase<ReturnInput>, ReturnUseCase>();

            services.AddScoped<IRentalValidationService, RentalValidationService>();
            services.AddScoped<IVehicleValidationsService, VehicleValidationService>();

            services.AddScoped<IRequestHandler<CreateVehicleCmd, CreateVehicleResponse>, CreateVehicleHandler>();

            return services;
        }
    }
}
