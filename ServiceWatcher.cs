using System.ServiceProcess;

class ServiceWatcher
{
    public void Watch(string serviceName)
    {
        using ServiceController service = new(serviceName);

        ServiceControllerStatus previousStatus = service.Status;
        ServiceControllerStatus temppreviousstatus;

        Console.WriteLine();
        Console.WriteLine($"Watching '{service.DisplayName}'");
        Console.WriteLine("Press Q to stop watching.");
        Console.WriteLine();
        Console.WriteLine($"{DateTime.Now:T} : {previousStatus}");
        //int secondspassed = 0;
        while (true)
        {
            if (Console.KeyAvailable){if (Console.ReadKey(true).Key == ConsoleKey.Q){break;}}

            service.Refresh();

            if (service.Status != previousStatus)
            {
                temppreviousstatus = previousStatus;
                previousStatus = service.Status;
                Console.ForegroundColor = GetColor(previousStatus);
                Console.WriteLine($"{DateTime.Now:T} : {temppreviousstatus} -> {previousStatus}");
                Console.ResetColor();
            }
            /*i++;
            if (secondspassed % 5 == 0)
            {
                Console.WriteLine($"{DateTime.Now:T} : {previousStatus}");
            }
            if (secondspassed == 2147483646)
            {
                secondspassed = 0;
            }*/
            Thread.Sleep(1000);
        }
    }

    private ConsoleColor GetColor(ServiceControllerStatus status)
    {
        return status switch
        {
            ServiceControllerStatus.Running => ConsoleColor.Green,
            ServiceControllerStatus.Stopped => ConsoleColor.Red,
            ServiceControllerStatus.Paused => ConsoleColor.Yellow,
            _ => ConsoleColor.Cyan
        };
    }
}