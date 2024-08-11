using Saber.AirlineBookingSystem.Domain.Enums;
using Saber.AirlineBookingSystem.Domain.General;

namespace Saber.AirlineBookingSystem.Domain.FlightManagement
{
    public class FlightService
    {
        public List<Flight> flights = [];
        private readonly Utilities _utilities = new();
        private readonly FlightRepository _flightRepo = new();

        public FlightService()
        { 
            flights = _flightRepo.LoadFlightsFromFile();
        }

        public void FlightMenu()
        {
            int menuOption;

            do
            {
                _utilities.Log("****** Flight Menu ******", "yellow");
                _utilities.Log("* 1. View All Flights   *", "yellow");
                _utilities.Log("* 2. View Next Flight   *", "yellow");
                _utilities.Log("* 99. Back to Main Menu *", "yellow");
                _utilities.Log("*************************", "yellow");

                _utilities.Log("Choose an option: ", "yellow", false);
                bool isOptionIntParseable = int.TryParse(Console.ReadLine(), out menuOption);

                if (!isOptionIntParseable) 
                {
                    _utilities.Log("Please Try again!", "red");
                    continue;
                }

                Console.Clear();
                switch (menuOption)
                {
                    case 1:
                        ViewAllFlights();
                        break;
                    case 2:
                        ViewNextFlight();
                        break;
                    case 99:
                        // Go Back
                        break;
                    default:
                        _utilities.Log("Please Try again!", "red");
                        break;
                }
            } while (menuOption != 99);
        }

        private void ViewAllFlights()
        {
            flights.ForEach(flight => _utilities.Log($"{flight.DisplayFlightDetailsFull()}\n", "yellow"));
        }

        private void ViewNextFlight()
        {
            _utilities.ShowEnumList<FlightNumberType>();

            FlightNumberType flightNumber = _utilities.GetValidEnumInput<FlightNumberType>(
                "Choose an option from the above list: ",
                "Please enter a valid ID - try again!"
            );

            Flight? nextFlight = flights.FirstOrDefault(flight => flight.FlightNumber == flightNumber);

            if (nextFlight != null)
                _utilities.Log($"The next flight {nextFlight.FlightNumber} is: \n\n{nextFlight.DisplayFlightDetailsFull()} \n", "yellow");
            else
                _utilities.Log("\nNo flight found for the selected flight number! \n", "red");
        }

        public string DecreaseSeatsOnBookedFlight(List<Flight> flights, int noOfPax)
        {
            flights.ForEach(flight => flight.DecreaseAvailableSeats(noOfPax));
            return _flightRepo.UpdateFlightsInFile(flights, noOfPax);
        }
    }
}
