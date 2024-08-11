using Saber.AirlineBookingSystem.Domain.Enums;
using Saber.AirlineBookingSystem.Domain.FlightManagement;
using Saber.AirlineBookingSystem.Domain.General;
using Saber.AirlineBookingSystem.Domain.PersonManagement.Agent;
using Saber.AirlineBookingSystem.Domain.PersonManagement.Passenger;

namespace Saber.AirlineBookingSystem.Domain.BookingManagement
{
    public class BookingService
    {
        private List<Booking> bookings = [];
        private readonly Utilities _utilities = new();
        private readonly BookingRepository _bookingRepo = new();

        public BookingService() 
        {
            bookings = _bookingRepo.LoadBookingsFromFile();
        }

        public void BookingMenu()
        {
            int menuOption;

            do
            {
                _utilities.Log("****** Booking Menu ******", "yellow");
                _utilities.Log("* 1. Create New Booking  *", "yellow");
                _utilities.Log("* 2. Search Booking      *", "yellow");
                _utilities.Log("* 99. Back to Main Menu  *", "yellow");
                _utilities.Log("**************************", "yellow");

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
                        CreateNewBooking();
                        break;
                    case 2:
                        SearchBooking();
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

        private void CreateNewBooking()
        {
            FlightService fs = new();
            List<Flight> bookedFlights = [];

            int travelType = _utilities.GetValidIntInput(
                "Enter whether the booking is One-Way (0) or with Return (1): ",
                input => input == 0 || input == 1,
                "You have not made a correct selection - try again!"
            );

            int noOfPassengers = _utilities.GetValidIntInput(
                "Enter the number of passengers on the booking: ",
                input => input > 0,
                "You entered an incorrect amount - try again!"
            );

            if (noOfPassengers > 7)
            {
                _utilities.Log("You cannot book more than 7 passengers on 1 booking - Please advise client to call the Groups department!", "red");
                return;
            }

            bookedFlights.Add(ChooseFlights(2));
            if (travelType == 1) bookedFlights.Add(ChooseFlights(2, true, bookedFlights[0]));

            if ((travelType == 0 && bookedFlights[0] != null) || (travelType == 1 && bookedFlights[0] != null && bookedFlights[1] != null))
            {
                List<Passenger> pax = GetPassengerDetails(noOfPassengers);

                decimal flightPrice = Math.Round(bookedFlights.Sum(flight => flight.SeatPrice()) * noOfPassengers, 2);

                Console.Clear();
                _utilities.Log("\nBooking Details: ", "green");
                bookedFlights.ForEach(f => _utilities.Log(f.DisplayFlightDetailsShort()));
                pax.ForEach(p => _utilities.Log(p.DisplayDetails()));
                _utilities.Log($@"Total Price: EUR{flightPrice}" + "\n", "green");

                string response = fs.DecreaseSeatsOnBookedFlight(bookedFlights, pax.Count);

                Booking? booking = null;
                if (response == "OK")
                {
                    booking = new(bookedFlights, pax, AgentService.CurrentAgent.AgentID, flightPrice);
                    BookingRepository br = new();
                    br.SaveBookingToFile(booking);
                    _utilities.Log($"Booking has been created successfully with reference {booking.BookingNo}!\n", "green");
                }
                else
                    _utilities.Log("System error - Booking cannot be created!", "red");
            }
            else
            {
                _utilities.Log("Not enough flights for the selected travel type can be selected!", "red");
            }
        }

        private Flight ChooseFlights(int noOfPassengers, bool isInboundFlight = false, Flight? outboundFlight = null)
        {
            FlightService fs = new();
            List<Flight> validFlights;
            string departureAirport = string.Empty;

            if (!isInboundFlight)
            {
                departureAirport = _utilities.GetValidStringInput(
                    "Enter the airport code of departure for the outbound segment: ",
                    input => !string.IsNullOrWhiteSpace(input),
                    "Please enter a valid departure airport - try again!"
                );
            }

            if (!isInboundFlight)
            {
                validFlights = fs.flights.Where(flight => flight.DepartureAirport.Equals(departureAirport.ToUpper())
                    && flight.RemainingSeats >= noOfPassengers).ToList();
            }
            else
            {
                validFlights = fs.flights.Where(flight => flight.DepartureAirport.Equals(outboundFlight.ArrivalAirport)
                    && flight.ArrivalAirport.Equals(outboundFlight.DepartureAirport) && flight.DepartureTime > outboundFlight.ArrivalTime
                    && flight.RemainingSeats >= noOfPassengers).ToList();
            }

            _utilities.Log($"\n{(isInboundFlight ? "Inbound" : "Outbound")} Flight Selection", "green");

            if (validFlights != null && validFlights.Count > 0)
            {
                validFlights.ForEach(f => _utilities.Log($"\n{f.DisplayFlightDetailsShort(true)}\n", "yellow"));

                int selectedFlightId = _utilities.GetValidIntInput(
                    $"Enter the ID (in brackets) of the {(isInboundFlight ? "Inbound" : "Outbound")} flight selected: ",
                    input => validFlights.Any(flight => flight.FlightId == input) == true && input != -1,
                    "You have not made a correct selection - try again!"
                );

                return validFlights.FirstOrDefault(flight => flight.FlightId == selectedFlightId);
            }
            else
            {
                _utilities.Log("\nNo flights found from the entered departure airport!\n", "red");
                return null;
            }
        }

        private List<Passenger> GetPassengerDetails(int noOfPassengers)
        {
            List<Passenger> pax = [];

            for (int i = 0; i < noOfPassengers; i++)
            {
                _utilities.Log($"\nEnter Passenger {i + 1} Details", "green");

                _utilities.ShowEnumList<TitleType>();
                TitleType title = _utilities.GetValidEnumInput<TitleType>(
                    "Choose passenger's title from the above list: ",
                    "Please enter a valid ID - try again!"
                );

                string firstName = _utilities.GetValidStringInput(
                    "Enter the passenger's first name: ",
                    input => input.All(char.IsLetter) && !string.IsNullOrWhiteSpace(input),
                    "You entered an invalid first name - try again!"
                ).Trim().ToUpper();

                string lastName = _utilities.GetValidStringInput(
                    "Enter the passenger's last name: ",
                    input => input.All(char.IsLetter) && !string.IsNullOrWhiteSpace(input),
                    "You entered an invalid last name - try again!"
                ).Trim().ToUpper();

                string email = _utilities.GetValidEmailInput(
                    "Enter the passenger's email: ",
                    input => !string.IsNullOrWhiteSpace(input),
                    "You entered an invalid email - try again!"
                ).Trim().ToUpper();

                DateOnly dob = _utilities.GetValidDateOnlyInput(
                    "Enter the passenger's date of birth: ",
                    "You entered an invalid date of birth - try again!"
                );

                string phone = _utilities.GetValidStringInput(
                    "Enter the passenger's phone number: ",
                    input => input.All(char.IsDigit) && !string.IsNullOrWhiteSpace(input),
                    "You entered an invalid phone number - try again!"
                ).Trim();

                string freqFlyerNo = _utilities.GetValidStringInput(
                    "Enter the passenger's frequent flyer number: ",
                    input => input.All(char.IsLetterOrDigit) || string.IsNullOrWhiteSpace(input),
                    "You entered an invalid flyer number - try again!"
                ).Trim().ToUpper();

                pax.Add(new Passenger(firstName, lastName, email, title, dob, phone, freqFlyerNo));
            }

            return pax;
        }

        private void SearchBooking()
        {
            string bookingCode = _utilities.GetValidStringInput(
                "Enter the booking number that you are searching: ",
                input => !string.IsNullOrWhiteSpace(input) && input.Length == 6,
                "Please enter a valid booking number - try again!"
            );

            Booking? booking = bookings.FirstOrDefault(booking => booking.BookingNo.Equals(bookingCode, StringComparison.CurrentCultureIgnoreCase));

            if (booking != null)
                _utilities.Log(booking.ShowBookingDetails(), "yellow");
            else
                _utilities.Log("There are no bookings with the entered booking number", "red");
        }
    }
}
