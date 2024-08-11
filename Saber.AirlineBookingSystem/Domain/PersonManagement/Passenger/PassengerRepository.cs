using Saber.AirlineBookingSystem.Domain.AircraftManagement;
using Saber.AirlineBookingSystem.Domain.BookingManagement;
using Saber.AirlineBookingSystem.Domain.Enums;
using Saber.AirlineBookingSystem.Domain.FlightManagement;
using Saber.AirlineBookingSystem.Domain.General;

namespace Saber.AirlineBookingSystem.Domain.PersonManagement.Passenger
{
    public class PassengerRepository
    {
        private readonly RepositoryUtilities _repoUtilities = new();
        private readonly string _directory = @"C:\Users\cauch\Documents\Saber.AirlineBookingSystem\Saber.AirlineBookingSystem\Repository\";
        private readonly string _fileName = "pax.txt";

        public List<Passenger> LoadPassengersFromFile()
        {
            List<Passenger> paxList = [];

            _repoUtilities.LoadFileLines(_directory, _fileName, (string[] lines) =>
            {
                foreach (string line in lines)
                {
                    string[] pStrings = line.Split(';');

                    string passengerId = pStrings[0];

                    bool isParseSuccess = Enum.TryParse(pStrings[1], out TitleType title);
                    if (!isParseSuccess) continue;

                    string firstName = pStrings[2];
                    string lastName = pStrings[3];

                    isParseSuccess = DateOnly.TryParse(pStrings[4], out DateOnly dob);
                    if (!isParseSuccess) continue;

                    string email = pStrings[5];
                    string phone = pStrings[6];
                    string flyerNo = pStrings[7];

                    Passenger passenger = new(firstName, lastName, email, title, dob, phone, flyerNo, passengerId);
                    paxList.Add(passenger);
                }
            });

            return paxList.ToList();
        }

        public void SavePassengersToFile(List<Passenger> pax)
        {
            _repoUtilities.WriteFileLines(_directory, _fileName, (string toAppend) =>
            {
                pax.ForEach(p =>
                {
                    toAppend += $"{p.PassengerId};{p.Title};{p.FirstName};{p.LastName};{p.DateOfBirth};{p.Email};{p.PhoneNumber};{p.FrequentFlyerNo}\n";
                });

                File.AppendAllText(Path.Combine(_directory, _fileName), $"{toAppend}");
            });
        }
    }
}
