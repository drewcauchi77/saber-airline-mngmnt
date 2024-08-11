using Saber.AirlineBookingSystem.Domain.BookingManagement;
using Saber.AirlineBookingSystem.Domain.FlightManagement;

namespace Saber.AirlineBookingSystem.Domain.General
{
    public class DomainService
    {
        private readonly Utilities _utilities = new();

        public void MainMenu()
        {
            int menuOption;

            do
            {
                _utilities.Log("******* Main Menu *******", "yellow");
                _utilities.Log("* 1. Booking Management *", "yellow");
                _utilities.Log("* 2. Flight Management  *", "yellow");
                _utilities.Log("* 99. Close Application *", "yellow");
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
                        BookingService bookingService = new();
                        bookingService.BookingMenu();
                        break;
                    case 2:
                        FlightService flightService = new();
                        flightService.FlightMenu();
                        break;
                    case 99:
                        _utilities.Log
                            ("Application is closing!", "red");
                        break;
                    default:
                        _utilities.Log("Please Try again!", "red");
                        break;
                }
            } while (menuOption != 99);
        }
    }
}
