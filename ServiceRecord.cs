using System.ServiceProcess;
class ServiceRecord
{
    public string Name { get; set; } = "";
    public string DisplayName { get; set; } = "";

    public ServiceControllerStatus Status { get; set; }

    public bool CanStop { get; set; }

    public bool CanPause { get; set; }

    public int RestartCount { get; set; }

}