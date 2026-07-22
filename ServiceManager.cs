using System.ServiceProcess;

class ServiceManager
{
    public bool StartService(string serviceName)
    {
        try
        {
            using ServiceController service = new(serviceName);

            if (service.Status == ServiceControllerStatus.Running)
                return true;

            service.Start();
            service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(15));

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error starting service: {ex.Message}");
            return false;
        }
    }

    public bool StopService(string serviceName)
    {
        try
        {
            using ServiceController service = new(serviceName);
            if (service.Status == ServiceControllerStatus.Stopped)
                return true;
            if (!service.CanStop)
            {
                Console.WriteLine("This service cannot be stopped.");
                return false;
            }

            service.Stop();
            service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(15));

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error stopping service: {ex.Message}");
            return false;
        }
    }

    public bool RestartService(string serviceName)
    {
        try
        {
            using ServiceController service = new(serviceName);

            if (service.Status != ServiceControllerStatus.Stopped)
            {
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(15));
            }

            service.Start();
            service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(15));

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error restarting service: {ex.Message}");
            return false;
        }
    }
}