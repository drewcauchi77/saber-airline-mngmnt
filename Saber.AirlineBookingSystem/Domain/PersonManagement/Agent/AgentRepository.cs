using Saber.AirlineBookingSystem.Domain.Enums;
using Saber.AirlineBookingSystem.Domain.General;

namespace Saber.AirlineBookingSystem.Domain.PersonManagement.Agent
{
    public class AgentRepository
    {
        private readonly RepositoryUtilities _repoUtilities = new();
        private readonly Utilities _utilities = new();
        private readonly string _directory = @"C:\Users\cauch\Documents\Saber.AirlineBookingSystem\Saber.AirlineBookingSystem\Repository\";
        private readonly string _fileName = "agents.txt";

        public void LoadAgentDetailsFromFile()
        {
            Agent? agent = null;
            string agentId = string.Empty;
            bool isAgentFound = false;

            _repoUtilities.LoadFileLines(_directory, _fileName, (lines) =>
            {
                do
                {
                    _utilities.Log("Enter your agent ID: ", "yellow", false);
                    agentId = Console.ReadLine() ?? string.Empty;
                    if(string.IsNullOrWhiteSpace(agentId) || agentId.Length != 6) 
                        _utilities.Log($"The agent ID entered is in incorrect format.\n", "red");

                    foreach (string line in lines)
                    {
                        string[] aStrings = line.Split(';');

                        string id = aStrings[0];
                        if (id == agentId.ToUpper())
                        {
                            string agentFirstName = aStrings[1];
                            string agentLastName = aStrings[2];
                            string agentEmail = aStrings[3];
                            bool isParseSuccess = Enum.TryParse(aStrings[4], out AgentType agentType);
                            if (!isParseSuccess) continue;

                            agent = new(agentFirstName, agentLastName, agentEmail, id, agentType);
                            isAgentFound = true;
                            break;
                        }
                    }

                    if (!isAgentFound) _utilities.Log($"Agent ID not found. Please try again.\n", "red");
                } while (!isAgentFound);
            });

            if (agent != null)
            {
                Console.Clear();
                AgentService.CurrentAgent = agent;
                _utilities.Log($"{agent.DisplayDetails()}\n", "green");
            }
        }
    }
}
