using System.ServiceProcess;
class Response {
    public bool CompletedSuccessfully { get; set; }
    public string ServiceName { get; set; } = "";
    public string ProcessType { get; set; } = "";
    public string Message { get; set; } = "";
}
class ServiceManager
{
    public Response StartService(string serviceName)
    {
        Response response = new();
        response.ServiceName = serviceName;
        response.ProcessType = "Start";
        try
        {
            using ServiceController service = new(serviceName);

            if (service.Status == ServiceControllerStatus.Running)
            {
                response.CompletedSuccessfully = true;
                return response;
            }

            service.Start();
            service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(15));
            
            response.CompletedSuccessfully = true;
            return response;
            
        }
        catch (Exception ex)
        {
            response.CompletedSuccessfully = false;
            response.Message = $"Error starting service: {ex.Message}";
            return response;
        }
    }

    public Response StopService(string serviceName)
    {
        Response response = new();
        response.ServiceName = serviceName;
        response.ProcessType = "Stop";
        try
        {
            using ServiceController service = new(serviceName);
            if (service.Status == ServiceControllerStatus.Stopped){
                response.CompletedSuccessfully = true;
                return response;
            }
            if (!service.CanStop)
            {
                response.CompletedSuccessfully = false;
                response.Message = "This service cannot be stopped";
                return response;
            }

            service.Stop();
            service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(15));

            response.CompletedSuccessfully = true;
            return response;
        }
        catch (Exception ex)
        {
            response.CompletedSuccessfully = false;
            response.Message = $"Error stopping service: {ex.Message}";
            return response;
            
        }
    }

    public Response RestartService(string serviceName)
    {
        Response response = new();
        response.ServiceName = serviceName;
        response.ProcessType = "Restart";
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

            response.CompletedSuccessfully = true;
            return response;
        }
        catch (Exception ex)
        {
            response.CompletedSuccessfully = false;
            response.Message = $"Error restarting service: {ex.Message}";
            return response;
        }
    }
}