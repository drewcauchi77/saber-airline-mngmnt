using Saber.AirlineBookingSystem.Domain.General;
using Saber.AirlineBookingSystem.Domain.PersonManagement.Agent;

PrintWelcome();

AgentRepository ar = new();
ar.LoadAgentDetailsFromFile();

DomainService ds = new();
ds.MainMenu();

#region Welcome

static void PrintWelcome()
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine(@"
                                            bbbbbbbb                                                    
           SSSSSSSSSSSSSSS                  b::::::b                                                    
         SS:::::::::::::::S                 b::::::b                                                    
        S:::::SSSSSS::::::S                 b::::::b                                                    
        S:::::S     SSSSSSS                  b:::::b                                                    
        S:::::S              aaaaaaaaaaaaa   b:::::bbbbbbbbb        eeeeeeeeeeee    rrrrr   rrrrrrrrr   
        S:::::S              a::::::::::::a  b::::::::::::::bb    ee::::::::::::ee  r::::rrr:::::::::r  
         S::::SSSS           aaaaaaaaa:::::a b::::::::::::::::b  e::::::eeeee:::::eer:::::::::::::::::r 
          SS::::::SSSSS               a::::a b:::::bbbbb:::::::be::::::e     e:::::err::::::rrrrr::::::r
            SSS::::::::SS      aaaaaaa:::::a b:::::b    b::::::be:::::::eeeee::::::e r:::::r     r:::::r
               SSSSSS::::S   aa::::::::::::a b:::::b     b:::::be:::::::::::::::::e  r:::::r     rrrrrrr
                    S:::::S a::::aaaa::::::a b:::::b     b:::::be::::::eeeeeeeeeee   r:::::r            
                    S:::::Sa::::a    a:::::a b:::::b     b:::::be:::::::e            r:::::r            
        SSSSSSS     S:::::Sa::::a    a:::::a b:::::bbbbbb::::::be::::::::e           r:::::r            
        S::::::SSSSSS:::::Sa:::::aaaa::::::a b::::::::::::::::b  e::::::::eeeeeeee   r:::::r            
        S:::::::::::::::SS  a::::::::::aa:::ab:::::::::::::::b    ee:::::::::::::e   r:::::r            
         SSSSSSSSSSSSSSS     aaaaaaaaaa  aaaabbbbbbbbbbbbbbbb       eeeeeeeeeeeeee   rrrrrrr            
    ");
    Console.ResetColor();
    Console.Write("Press Enter to start application ");
    Console.ReadLine();
    Console.Clear();
}

#endregion