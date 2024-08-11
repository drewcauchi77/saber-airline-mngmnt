using Saber.AirlineBookingSystem.Domain.AircraftManagement;
using Saber.AirlineBookingSystem.Domain.Enums;
using Saber.AirlineBookingSystem.Domain.General;

namespace Saber.AirlineBookingSystem.Domain.FlightManagement
{
    public class FlightRepository
    {
        private readonly RepositoryUtilities _repoUtilities = new();
        private readonly string _directory = @"C:\Users\cauch\Documents\Saber.AirlineBookingSystem\Saber.AirlineBookingSystem\Repository\";
        private readonly string _fileName = "flights.txt";

        public List<Flight> LoadFlightsFromFile()
        {
            List<Flight> flightsList = [];

            _repoUtilities.LoadFileLines(_directory, _fileName, (string[] lines) =>
            {
                foreach (string line in lines)
                {
                    string[] fStrings = line.Split(';');

                    bool isParseSuccess = int.TryParse(fStrings[0], out int id);
                    if (!isParseSuccess) continue;

                    isParseSuccess = Enum.TryParse(fStrings[1], out FlightNumberType flightNo);
                    if (!isParseSuccess) continue;

                    string airline = fStrings[2];

                    isParseSuccess = Enum.TryParse(fStrings[3], out AircraftType planeModel);
                    if (!isParseSuccess) continue;

                    bool isDateTimeParseable = DateTime.TryParse(fStrings[4], out DateTime departureDate);
                    if (!isDateTimeParseable) continue;

                    isDateTimeParseable = DateTime.TryParse(fStrings[5], out DateTime arrivalDate);
                    if (!isDateTimeParseable) continue;

                    string departureAirportCode = fStrings[6];
                    string arrivalAirportCode = fStrings[7];

                    isParseSuccess = int.TryParse(fStrings[8], out int totalSeats);
                    if (!isParseSuccess) continue;

                    isParseSuccess = int.TryParse(fStrings[9], out int availableSeats);
                    if (!isParseSuccess) continue;

                    isParseSuccess = decimal.TryParse(fStrings[10], out decimal totalKilometers);
                    if (!isParseSuccess) continue;

                    Flight flight = new(id, flightNo, new Aircraft()
                    {
                        AirlineName = airline,
                        AircraftModel = planeModel,
                        SeatCapacity = totalSeats
                    }, departureAirportCode, arrivalAirportCode, departureDate, arrivalDate, availableSeats, totalKilometers);

                    flightsList.Add(flight);
                }
            });

            return flightsList.OrderBy(f => f.DepartureTime).ToList();
        }

        public string UpdateFlightsInFile(List<Flight> bookedFlights, int noOfPax)
        {
            bool hasError = false;

            _repoUtilities.LoadFileLines(_directory, _fileName, (string[] lines) =>
            {
                bookedFlights.ForEach(flight =>
                {
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string[] fStrings = lines[i].Split(';');

                        bool isParseSuccess = int.TryParse(fStrings[0], out int id);
                        if (!isParseSuccess) continue;

                        if (id == flight.FlightId)
                        {
                            isParseSuccess = int.TryParse(fStrings[9], out int availableSeats);
                            if (!isParseSuccess)
                            {
                                hasError = true;
                                continue;
                            }

                            fStrings[9] = $"{availableSeats - noOfPax}";
                            lines[i] = string.Join(';', fStrings);
                            break;
                        }
                    }
                });

                File.WriteAllLines(Path.Combine(_directory, _fileName), lines);
            });

            return hasError ? "ERROR" : "OK";
        }
    }
}
