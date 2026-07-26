using System.Collections;
using System.Dynamic;
using System.Net;
using System.ServiceProcess;
using System.Security.Cryptography.X509Certificates;

ServiceReader reader = new();
ServiceManager manager = new();
ServiceWatcher watcher = new();
ServiceHelper helper = new();
Utils utils = new();
Response response = new();
bool userisadmin = utils.RunningAsAdmin();
List<ServiceRecord> services;

Console.WriteLine("=== Welcome to the service watcher! ===");
utils.Display_Menu(userisadmin);

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
            helper.PrintServices(helper.GetServicesBasedOnStatus(services, ServiceControllerStatus.Stopped));
            break;
        case "3":
            helper.PrintServices(helper.GetServicesBasedOnStatus(services, ServiceControllerStatus.Running));
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
            if (service == null){Console.WriteLine("No services selected"); break;}
            response = manager.StartService(service.Name);
            utils.PrintResponse(response);
            break;
        case "6":
            service = helper.SelectService(services);
            if (service == null){Console.WriteLine("No services selected"); break;}
            response = manager.StopService(service.Name);
            utils.PrintResponse(response);
            break;
        case "7":
            service = helper.SelectService(services);
            if (service == null){Console.WriteLine("No services selected"); break;}
            response = manager.RestartService(service.Name);
            utils.PrintResponse(response);
            break;
        case "8":
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

