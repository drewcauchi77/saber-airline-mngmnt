using Saber.AirlineBookingSystem.Domain.FlightManagement;
using Saber.AirlineBookingSystem.Domain.General;
using Saber.AirlineBookingSystem.Domain.PersonManagement.Passenger;
using System.Text.RegularExpressions;

namespace Saber.AirlineBookingSystem.Domain.BookingManagement
{
    public class BookingRepository
    {
        private readonly RepositoryUtilities _repoUtilities = new();
        private readonly string _directory = @"C:\Users\cauch\Documents\Saber.AirlineBookingSystem\Saber.AirlineBookingSystem\Repository\";
        private readonly string _fileName = "bookings.txt";

        public List<Booking> LoadBookingsFromFile()
        {
            List<Booking> bookingsList = [];

            _repoUtilities.LoadFileLines(_directory, _fileName, (string[] lines) =>
            {
                foreach (string line in lines)
                {
                    string[] bStrings = line.Split(';');
                    string pattern = @"\[(.*?)\]";
                    MatchCollection matches = Regex.Matches(line, pattern);

                    string bookingNo = bStrings[0];
                    
                    bool isParseSuccess = DateTime.TryParse(bStrings[1], out DateTime createTime);
                    if (!isParseSuccess) continue;

                    isParseSuccess = DateTime.TryParse(bStrings[2], out DateTime fulfillTime);
                    if (!isParseSuccess) continue;

                    string agentId = bStrings[3];

                    isParseSuccess = decimal.TryParse(bStrings[4], out decimal pricePaid);
                    if (!isParseSuccess) continue;

                    FlightRepository flightRepository = new();
                    List<Flight> flights = flightRepository.LoadFlightsFromFile();

                    string[] flightStrings = matches[0].Value.Replace("[", "").Replace("]", "").Split(';');
                    List<Flight> bookedFlights = [];

                    foreach (var flightId in flightStrings)
                    {
                        bookedFlights.Add(flights.FirstOrDefault(flight => flight.FlightId == int.Parse(flightId)));
                    }

                    PassengerRepository paxRepository = new();
                    List<Passenger> pax = paxRepository.LoadPassengersFromFile();

                    string[] paxStrings = matches[1].Value.Replace("[", "").Replace("]", "").Split(';');
                    List<Passenger> bookedPax = [];

                    foreach (var paxId in paxStrings)
                    {
                        bookedPax.Add(pax.FirstOrDefault(passenger => passenger.PassengerId.Equals(paxId)));
                    }

                    Booking booking = new(bookingNo, bookedFlights, bookedPax, createTime, fulfillTime, agentId, pricePaid);
                    bookingsList.Add(booking);
                }
            });

            return bookingsList;
        }

        public void SaveBookingToFile(Booking booking)
        {
            _repoUtilities.WriteFileLines(_directory, _fileName, (string toAppend) =>
            {
                toAppend = $"{booking.BookingNo};{booking.CreationTime};{booking.FulfilledTime};{booking.AgentId};{booking.Price};[";

                for (int i = 0; i < booking.Flights.Count; i++)
                {
                    toAppend += booking.Flights[i].FlightId;
                    if (i != booking.Flights.Count - 1) toAppend += ';';
                }

                toAppend += "];[";

                for (int i = 0; i < booking.Passengers.Count; i++)
                {
                    toAppend += booking.Passengers[i].PassengerId;
                    if (i != booking.Passengers.Count - 1) toAppend += ';';
                }

                toAppend += "];";

                File.AppendAllText(Path.Combine(_directory, _fileName), $"{toAppend}\n");

                PassengerRepository pr = new();
                pr.SavePassengersToFile(booking.Passengers);
            });
        }
    }
}
