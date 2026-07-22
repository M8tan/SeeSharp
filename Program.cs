using System.Collections;
using System.Dynamic;
using System.Net;
using System.Security.Cryptography.X509Certificates;

void Display_Menu()
{
    Console.WriteLine();
    Console.WriteLine("====================================");
    Console.WriteLine("1. List all services");
    Console.WriteLine("2. List stopped services");
    Console.WriteLine("3. List running services");
    Console.WriteLine("4. Search services by name");
    Console.WriteLine("5. Start service");
    Console.WriteLine("6. Stop service {requires admin priviliges}");
    Console.WriteLine("7. Watch service");
    Console.WriteLine("10. Exit");
    Console.WriteLine("====================================");
    Console.WriteLine();
}

List<ServiceRecord> GetServicesBasedOnStatus(List<ServiceRecord> services, bool running)
{   
    return services.Where(s => s.IsRunning == running).ToList();
}

/*
List<ServiceRecord> GetServicesBasedOnStopability(List<ServiceRecord> services, bool canstop)
{   
    return services.Where(s => s.CanStop == canstop).ToList();
}
*/

List<ServiceRecord> SearchServiceKeyword(List<ServiceRecord> services, string query)
{   
    return services.Where(s => s.Name.Contains(query, StringComparison.OrdinalIgnoreCase) || s.DisplayName.Contains(query, StringComparison.OrdinalIgnoreCase)).OrderBy(s => s.DisplayName).ToList();
}

ServiceRecord? SelectService(List<ServiceRecord> services)
{
    Console.Write("Search service: ");
    string? query = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(query)){return null;}
    var results = SearchServiceKeyword(services, query);

    if (results.Count == 0)
    {
        Console.WriteLine($"No matches for '{query}'");
        return null;
    }

    PrintServices(results);

    Console.Write("Enter ID: ");

    if (!int.TryParse(Console.ReadLine(), out int id)){return null;}

    if (id < 1 || id > results.Count){string idoutofrangetext = (id < 1) ? "too low" : "too high"; Console.WriteLine($"Out of range: {id} is {idoutofrangetext}"); return null;}

    return results[id - 1];
}

void PrintServices(List<ServiceRecord> services)
{
    Console.WriteLine();
    Console.WriteLine("Id   Status     Service Name                                  Display Name");
    Console.WriteLine("--------------------------------------------------------------------------------------------------------------------");
    for(int i = 0; i < services.Count(); i++)
    {
        Console.WriteLine($"{i+1,-4} {services[i].Status,-10} {services[i].Name,-45} {services[i].DisplayName}");
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
ServiceManager manager = new();
ServiceWatcher watcher = new();
List<ServiceRecord> services;

Console.WriteLine("=== Welcome to the service watcher! ===");
Display_Menu();

bool AppRunning = true;
while (AppRunning)
{
    services = reader.GetServices();
    Console.Write("Your choice: ");
    string? input = Console.ReadLine();
    string? query;
    //int id;
    ServiceRecord service;
    List<ServiceRecord> results;
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
                Console.Write("Search keyword:");
                query = Console.ReadLine(); // here
                if(string.IsNullOrWhiteSpace(query)){Console.WriteLine("No keyword provided"); break;}
                results = SearchServiceKeyword(services, query); // here
                if (results.Count == 0)
            {
                Console.WriteLine($"No matches for keyword '{query}'");
            } else if (results.Count == 1)
            {
                PrintServiceDetails(results[0]);
                Console.WriteLine($"Found one match for keyword '{query}'");
            } else {
                PrintServices(results);
                Console.WriteLine($"Found {results.Count} services that match the keyword '{query}'");
            }
                break;
        case "5":
            service = SelectService(services);
            if (service != null){manager.StartService(service.Name);}
            break;
        case "6":
            service = SelectService(services);
            if (service != null){manager.StopService(service.Name);}
            break;
        case "7":
            service = SelectService(services);
            if (service != null){watcher.Watch(service.Name);}
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

