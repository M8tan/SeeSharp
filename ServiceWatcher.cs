using System.ServiceProcess;

class ServiceWatcher
{
    public void Watch(string serviceName)
    {
        using ServiceController service = new(serviceName);

        ServiceControllerStatus previousStatus = service.Status;

        Console.WriteLine();
        Console.WriteLine($"Watching '{service.DisplayName}'");
        Console.WriteLine("Press Q to stop watching.");
        Console.WriteLine();
        Console.WriteLine($"{DateTime.Now:T} : {previousStatus}");
        //int i = 0;
        while (true)
        {
            if (Console.KeyAvailable){if (Console.ReadKey(true).Key == ConsoleKey.Q){break;}}

            service.Refresh();

            if (service.Status != previousStatus)
            {
                previousStatus = service.Status;

                Console.ForegroundColor = GetColor(previousStatus);

                Console.WriteLine($"{DateTime.Now:T} : {previousStatus}");

                Console.ResetColor();
            }
            /*i++;
            if (i % 5 == 0)
            {
                Console.WriteLine($"{DateTime.Now:T} : {previousStatus}");
            }
            if (i == 2147483646)
            {
                i = 0;
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