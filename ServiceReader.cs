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
                Name = service.DisplayName,
                IsRunning = service.Status == ServiceControllerStatus.Running,
                RestartCount = 0
            });
        }

        return services;
    }
}