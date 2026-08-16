using System.Collections;
using System.Dynamic;
using System.Net;
using System.ServiceProcess;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Collections.Generic;

if (!OperatingSystem.IsWindows())
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("OS is not compatibale with this app");
    Console.ResetColor();
    return;
}

ServiceReader reader = new();
ServiceManager manager = new();
ServiceWatcher watcher = new();
ServiceHelper helper = new();
Utils utils = new();
Response response = new();
bool userisadmin = utils.RunningAsAdmin();
List<ServiceRecord>? services;
services = reader.GetServices();
List<ServiceRecord>? test = new();
if (services.Count <= 0)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("No services found");
    Console.ResetColor();
    return;
}

Console.ForegroundColor = ConsoleColor.DarkCyan;
Console.WriteLine("=== Welcome to the service watcher! ===");
Console.ResetColor();
utils.Display_Menu(userisadmin);


bool AppRunning = true;
while (AppRunning)
{
    Console.Write("Your choice: ");
    string? input = Console.ReadLine();
    string? query;
    ServiceRecord? service;
    List<ServiceRecord> results;
    switch (input)
    {
        case "0":
            utils.Display_Menu(userisadmin);
            break;
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
                query = Console.ReadLine();
                if(string.IsNullOrWhiteSpace(query)){Console.WriteLine("No keyword provided"); break;}
                results = helper.SearchServiceKeyword(services, query);
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
            if (service == null){Console.WriteLine("No service selected"); break;}
            response = manager.StartService(service.Name);
            utils.PrintResponse(response);
            if (response.Success){services = reader.GetServices();}
            break;
        case "6":
            if (!userisadmin)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Stopping a service requires admin permissions :)");
                Console.ResetColor();
                break;
            }
            service = helper.SelectService(services);
            if (service == null){Console.WriteLine("No service selected"); break;}
            response = manager.StopService(service.Name);
            utils.PrintResponse(response);
            if (response.Success){services = reader.GetServices();}
            break;
        case "7":
            if (!userisadmin)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Restarting a service requires admin permissions :)");
                Console.ResetColor();
                break;
            }
            service = helper.SelectService(services);
            if (service == null){Console.WriteLine("No service selected"); break;}
            response = manager.RestartService(service.Name);
            utils.PrintResponse(response);
            if (response.Success){services = reader.GetServices();}
            break;
        case "8":
            service = helper.SelectService(services);
            if (service != null){watcher.Watch(service.Name);}
            break;
        case "9":
            services = reader.GetServices();
            break;
        case "10":
            Console.Write("OK!\nThanks for using this app :)");
            AppRunning = false;
            break;
        default:
            Console.WriteLine($"Unknown option - {input}");
            break;
    }
}

