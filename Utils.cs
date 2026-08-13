using System;
using System.Net;
using System.Security.Principal;

class Utils
{ 
    public bool RunningAsAdmin()
    {
        using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
        {
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }

    public void Display_Menu(bool runningasadmin)
    {
        Console.WriteLine();
        Console.WriteLine("==================================================");
        Console.WriteLine("  0. Show this menu");
        Console.WriteLine("  1. List all services");
        Console.WriteLine("  2. List stopped services");
        Console.WriteLine("  3. List running services");
        Console.WriteLine("  4. Search services by name");
        Console.WriteLine("  5. Start service");
        if (runningasadmin)
        {
            Console.WriteLine("  6. Stop service");
            Console.WriteLine("  7. Restart service");
        } else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  6. Stop service {requires admin priviliges}");
            Console.WriteLine("  7. Restart service {requires admin priviliges}");
            Console.ResetColor();
        }
        Console.WriteLine("  8. Watch a service");
        Console.WriteLine("  9. Refresh services");
        Console.WriteLine(" 10. Exit");
        Console.WriteLine("==================================================");
        Console.WriteLine();
    }

    public void PrintResponse(Response res)
    {
        if (res.Success)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Successfully completed process {res.Operation} on service {res.ServiceName}");
        } else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Failed to perform process {res.Operation} on service {res.ServiceName}: {res.Message}");
        }
        Console.ResetColor();
    }

}