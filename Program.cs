using System.Collections;
using System.Dynamic;
using System.Net;
using System.Security.Principal;
using System.Security.Cryptography.X509Certificates;

bool RunningAsAdmin()
{
    using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
    {
        WindowsPrincipal principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }
}

void Display_Menu(bool runningasadmin)
{
    Console.WriteLine();
    Console.WriteLine("====================================");
    Console.WriteLine("1. List all services");
    Console.WriteLine("2. List stopped services");
    Console.WriteLine("3. List running services");
    Console.WriteLine("4. Search services by name");
    Console.WriteLine("5. Start service");
    if (runningasadmin)
    {
        Console.WriteLine("6. Stop service");
    } else
    {
        Console.WriteLine("6. Stop service {requires admin priviliges}");
    }
    Console.WriteLine("7. Watch a service");
    Console.WriteLine("10. Exit");
    Console.WriteLine("====================================");
    Console.WriteLine();
}


bool userisadmin = RunningAsAdmin();
ServiceReader reader = new();
ServiceManager manager = new();
ServiceWatcher watcher = new();
ServiceHelper helper = new();
List<ServiceRecord> services;

Console.WriteLine("=== Welcome to the service watcher! ===");
Display_Menu(userisadmin);

bool AppRunning = true;
while (AppRunning)
{
    services = reader.GetServices();
    Console.Write("Your choice: ");
    string? input = Console.ReadLine();
    string? query;
    ServiceRecord? service;
    List<ServiceRecord> results;
    switch (input)
    {
        case "1":
            helper.PrintServices(services);
            break;
        case "2":
            helper.PrintServices(helper.GetServicesBasedOnStatus(services, false));
            break;
        case "3":
            helper.PrintServices(helper.GetServicesBasedOnStatus(services, true));
            break;
        case "4":
                Console.Write("Search keyword:");
                query = Console.ReadLine(); // here
                if(string.IsNullOrWhiteSpace(query)){Console.WriteLine("No keyword provided"); break;}
                results = helper.SearchServiceKeyword(services, query); // here
                if (results.Count == 0)
            {
                Console.WriteLine($"No matches for keyword '{query}'");
            } else if (results.Count == 1)
            {
                helper.PrintServiceDetails(results[0]);
                Console.WriteLine($"Found one match for keyword '{query}'");
            } else {
                helper.PrintServices(results);
                Console.WriteLine($"Found {results.Count} services that match the keyword '{query}'");
            }
                break;
        case "5":
            service = helper.SelectService(services);
            if (service != null){manager.StartService(service.Name);}
            break;
        case "6":
            service = helper.SelectService(services);
            if (service != null){manager.StopService(service.Name);}
            break;
        case "7":
            service = helper.SelectService(services);
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

