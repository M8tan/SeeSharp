using System.ServiceProcess;

class ServiceHelper {
    public List<ServiceRecord> GetServicesBasedOnStatus(List<ServiceRecord> services, ServiceControllerStatus status)
    {   
        return services.Where(s => s.Status == status).ToList();
    }

/* Might be useful some time
    public List<ServiceRecord> GetServicesBasedOnStopability(List<ServiceRecord> services, bool canstop)
    {   
        return services.Where(s => s.CanStop == canstop).ToList();
    }    
*/ 
    public List<ServiceRecord> SearchServiceKeyword(List<ServiceRecord> services, string query)
    {   
        return services.Where(s => s.Name.Contains(query, StringComparison.OrdinalIgnoreCase) || s.DisplayName.Contains(query, StringComparison.OrdinalIgnoreCase)).OrderBy(s => s.DisplayName).ToList();
    }

    public ServiceRecord? SelectService(List<ServiceRecord> services)
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

    public void PrintServices(List<ServiceRecord> services)
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

    public void PrintServiceDetails(ServiceRecord service)
    {
        Console.WriteLine();
        Console.WriteLine($"Display Name : {service.DisplayName}");
        Console.WriteLine($"Service Name : {service.Name}");
        Console.WriteLine($"Status       : {service.Status}");
        Console.WriteLine($"Can Stop     : {service.CanStop}");
        Console.WriteLine($"Can Pause    : {service.CanPause}");
        Console.WriteLine();
    }
}