using Saber.AirlineBookingSystem.Domain.Enums;
using System.Text;

namespace Saber.AirlineBookingSystem.Domain.PersonManagement.Passenger
{
    public class Passenger : Person
    {
        public string PassengerId { get; private set; }
        public TitleType Title { get; private set; }
        public DateOnly DateOfBirth { get; private set; }
        public string PhoneNumber { get; private set; }
        public string? FrequentFlyerNo { get; private set; }

        public Passenger(string firstName, string lastName, string email, TitleType title, DateOnly dob, string phoneNumber, string freqFlyerNo)
            : base(firstName, lastName, email)
        {
            PassengerId = CreateYoutubeVideoLikeId();
            Title = title;
            DateOfBirth = dob;
            PhoneNumber = phoneNumber;
            FrequentFlyerNo = freqFlyerNo;
        }
        public Passenger(string firstName, string lastName, string email, TitleType title, DateOnly dob, string phoneNumber, string freqFlyerNo, string id)
            : base(firstName, lastName, email)
        {
            PassengerId = id;
            Title = title;
            DateOfBirth = dob;
            PhoneNumber = phoneNumber;
            FrequentFlyerNo = freqFlyerNo;
        }

        public override string DisplayDetails()
        {
            return $"{Title}. {FirstName} {LastName} ({FrequentFlyerNo})";
        }

        private string CreateYoutubeVideoLikeId()
        { 
            StringBuilder builder = new();
            Enumerable
                .Range(65, 26)
                .Select(e => ((char)e).ToString())
                .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
                .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
                .OrderBy(e => Guid.NewGuid())
                .Take(11)
                .ToList().ForEach(e => builder.Append(e));

            return builder.ToString();
        }
    }
}
