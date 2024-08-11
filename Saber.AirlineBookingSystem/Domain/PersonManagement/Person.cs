using Saber.AirlineBookingSystem.Domain.Contracts;

namespace Saber.AirlineBookingSystem.Domain.PersonManagement
{
    public abstract class Person: IPerson
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }

        protected Person(string firstName, string lastName, string email)
        { 
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        public abstract string DisplayDetails();
    }
}
