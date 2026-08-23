using System.ServiceProcess;
class ServiceReader
{
    public List<ServiceRecord> GetServices()
    {
        List<ServiceRecord> services = new();
        foreach (ServiceController service in ServiceController.GetServices())
        {
            services.Add(new ServiceRecord
            {
                Name = service.ServiceName,
                DisplayName = service.DisplayName,
                Status = service.Status,
                CanStop = service.CanStop,
                CanPause = service.CanPauseAndContinue,

            });
        }
        return services;
    }
}