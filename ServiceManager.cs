using System.Runtime.CompilerServices;
using System.ServiceProcess;

enum ServiceOperation
{
    Start,
    Stop,
    Restart
}
class Response {
    public bool Success { get; init; }
    public string ServiceName { get; init; } = "";
    public ServiceOperation Operation { get; init; }
    public string Message { get; init; } = "";
    public static Response OK(string service, ServiceOperation operation) => new(){Success = true, ServiceName = service, Operation = operation};
    public static Response FAIL(string service, ServiceOperation operation, string message) => new(){Success = false, ServiceName = service, Operation = operation, Message = message};

}
class ServiceManager
{
    public Response StartService(string serviceName)
    {
        try {
            using ServiceController service = new(serviceName);
            if (service.Status == ServiceControllerStatus.Running){return Response.OK(serviceName, ServiceOperation.Start);}
            service.Start();
            service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(15));
            return Response.OK(serviceName, ServiceOperation.Start);
        } catch (Exception ex){
            return Response.FAIL(serviceName, ServiceOperation.Start, ex.Message);
        }
    }

    public Response StopService(string serviceName)
    {
        try {
            using ServiceController service = new(serviceName);
            if (service.Status == ServiceControllerStatus.Stopped){return Response.OK(serviceName, ServiceOperation.Stop);}
            if (!service.CanStop){return Response.FAIL(serviceName, ServiceOperation.Stop, "Service can't be stopped");}
            service.Stop();
            service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(15));
            return Response.OK(serviceName, ServiceOperation.Stop);
        } catch (Exception ex){
            return Response.FAIL(serviceName, ServiceOperation.Stop, ex.Message);
        }
    }

    public Response RestartService(string serviceName)
    {
        try {
            using ServiceController service = new(serviceName);
            if (service.Status != ServiceControllerStatus.Stopped) {
                if (!service.CanStop) {
                    return Response.FAIL(serviceName, ServiceOperation.Restart, "Service can't be stopped, so restart is not possible");
                }
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(15));
            }
            service.Start();
            service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(15));
            return Response.OK(serviceName, ServiceOperation.Restart);
        } catch (Exception ex) {
            return Response.FAIL(serviceName, ServiceOperation.Restart, ex.Message);  
        }
    }
}