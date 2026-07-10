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

                IsRunning = service.Status == ServiceControllerStatus.Running,
                Status = service.Status.ToString(),

                CanStop = service.CanStop,
                CanPause = service.CanPauseAndContinue,

                RestartCount = 0
            });
        }

        return services;
    }
}