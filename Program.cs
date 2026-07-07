void Display_Menu()
{
    Console.WriteLine("1. List all services");
    Console.WriteLine("2. List stopped services");
    Console.WriteLine("3. List running services");
    Console.WriteLine("4. Start stopped services");
    Console.WriteLine("10. Exit");

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
    for(int i = 0; i < services.Count(); i++)
    {
        Console.WriteLine($"{i}. {services[i].Name}");
    }
}

ServiceReader reader = new();
List<ServiceRecord> services;
List<ServiceRecord> stopped;
List<ServiceRecord> running;

Console.WriteLine("=== Welcome to the service watcher! ===");
Display_Menu();

bool AppRunning = true;
while (AppRunning)
{
    Console.WriteLine("Your choice: ");
    string? input = Console.ReadLine();
    switch (input)
    {
        case "1":
            services = reader.GetServices();
            PrintServices(services);
            break;
        case "2":
            services = reader.GetServices();
            stopped = GetServicesBasedOnStatus(services, false);
            PrintServices(stopped);
            break;
        case "3":
            services = reader.GetServices();
            running = GetServicesBasedOnStatus(services, true);
            PrintServices(running);
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

