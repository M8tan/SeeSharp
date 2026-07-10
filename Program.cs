void Display_Menu()
{
    Console.WriteLine();
    Console.WriteLine("====================================");
    Console.WriteLine("1. List all services");
    Console.WriteLine("2. List stopped services");
    Console.WriteLine("3. List running services");
    Console.WriteLine("4. Start stopped services");
    Console.WriteLine("10. Exit");
    Console.WriteLine("====================================");
    Console.WriteLine();
}

List<ServiceRecord> GetServicesBasedOnStatus(List<ServiceRecord> services, bool running)
{   
    List<ServiceRecord> result = new();
    foreach(var s in services)
    {
        if(s.IsRunning == running)
        {
            result.Add(s);
        }
    }
    return result;
}

void PrintServices(List<ServiceRecord> services)
{
    Console.WriteLine();
    Console.WriteLine("Id   Status     Display Name");
    Console.WriteLine("-------------------------------------------");
    for(int i = 0; i < services.Count(); i++)
    {
        Console.WriteLine($"{i,-4} {services[i].Status,-10} {services[i].DisplayName}");
    }
    Console.WriteLine();
}

void PrintServiceDetails(ServiceRecord service)
{
    Console.WriteLine();
    Console.WriteLine($"Display Name : {service.DisplayName}");
    Console.WriteLine($"Service Name : {service.Name}");
    Console.WriteLine($"Status       : {service.Status}");
    Console.WriteLine($"Can Stop     : {service.CanStop}");
    Console.WriteLine($"Can Pause    : {service.CanPause}");
    Console.WriteLine();
}



ServiceReader reader = new();
List<ServiceRecord> services;

Console.WriteLine("=== Welcome to the service watcher! ===");
Display_Menu();

bool AppRunning = true;
while (AppRunning)
{
    services = reader.GetServices();
    Console.WriteLine("Your choice: ");
    string? input = Console.ReadLine();
    switch (input)
    {
        case "1":
            PrintServices(services);
            break;
        case "2":
            PrintServices(GetServicesBasedOnStatus(services, false));
            break;
        case "3":
            PrintServices(GetServicesBasedOnStatus(services, true));
            break;
        case "4":
            PrintServiceDetails(services[100]);
            break;
        case "10":
            Console.WriteLine("OK!");
            AppRunning = false;
            break;
        default:
            Console.WriteLine($"Unknown option - {input}");
            break;
    }
}

