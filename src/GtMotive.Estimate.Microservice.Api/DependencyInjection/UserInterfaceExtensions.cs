using GtMotive.Estimate.Microservice.Api.UseCases.Vehicle;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles;
using Microsoft.Extensions.DependencyInjection;

namespace GtMotive.Estimate.Microservice.Api.DependencyInjection
{
    public static class UserInterfaceExtensions
    {
        public static IServiceCollection AddPresenters(this IServiceCollection services)
        {
            services.AddScoped<GetAllVehiclesPresenter>();
            services.AddScoped<IGetAllOutputPort>(sp => sp.GetRequiredService<GetAllVehiclesPresenter>());

            services.AddScoped<RentVehiclePresenter>();
            services.AddScoped<IRentOutputPort>(sp => sp.GetRequiredService<RentVehiclePresenter>());

            return services;
        }
    }
}
