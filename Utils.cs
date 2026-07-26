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


}