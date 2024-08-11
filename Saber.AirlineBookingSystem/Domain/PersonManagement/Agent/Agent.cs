using Saber.AirlineBookingSystem.Domain.Enums;
using System.Reflection.Metadata.Ecma335;

namespace Saber.AirlineBookingSystem.Domain.PersonManagement.Agent
{
    public class Agent : Person
    {
        public string AgentID { get; set; }
        private AgentType AgentType { get; set; }

        public Agent(string firstName, string lastName, string email, string agentId, AgentType agentType)
            : base(firstName, lastName, email)
        {
            AgentID = agentId;
            AgentType = agentType;
        }

        public override string DisplayDetails()
        {
            return $"Hello {FirstName} {LastName} - {AgentType} \nAgent ID: {AgentID} \nEmail: {Email}";
        }
    }
}
