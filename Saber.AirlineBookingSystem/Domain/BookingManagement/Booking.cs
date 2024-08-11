using Saber.AirlineBookingSystem.Domain.FlightManagement;
using Saber.AirlineBookingSystem.Domain.PersonManagement.Agent;
using Saber.AirlineBookingSystem.Domain.PersonManagement.Passenger;

namespace Saber.AirlineBookingSystem.Domain.BookingManagement
{
    public class Booking
    {
        public string BookingNo { get; private set; }
        public List<Flight> Flights { get; private set; }
        public List<Passenger> Passengers { get; private set; }
        public DateTime CreationTime { get; private set; }
        public DateTime FulfilledTime { get; private set; }
        public string AgentId { get; private set; }
        public decimal Price { get; private set; }

        public Booking(List<Flight> flights, List<Passenger> pax, string agentId, decimal price)
        {
            BookingNo = CreateBookingNo();
            Flights = flights;
            Passengers = pax;
            CreationTime = DateTime.Now;
            FulfilledTime = DateTime.Now.AddSeconds(100);
            AgentId = agentId;
            Price = price;
        }

        public Booking(string bookingNo, List<Flight> flights, List<Passenger> pax, DateTime createTime, DateTime fulfillTime, string agentId, decimal price)
        {
            BookingNo = bookingNo;
            Flights = flights;
            Passengers = pax;
            CreationTime = createTime;
            FulfilledTime = fulfillTime;
            AgentId = agentId;
            Price = price;
        }

        private string CreateBookingNo()
        {
            char[] charArray = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            string bookingNo = string.Empty;

            for (int i = 0; i < 6; i++)
            {
                Random random = new();
                int rnd = random.Next(0, 26);
                bookingNo = $"{bookingNo}{charArray[rnd]}";
            }

            return bookingNo;
        }

        public string ShowBookingDetails()
        {
            string toAppend = $"{BookingNo} (EUR{Price}) \n";
            Passengers.ForEach(pax => toAppend += $"{pax.DisplayDetails()} \n");
            toAppend += "\n";
            Flights.ForEach(flight => toAppend += $"{flight.DisplayFlightDetailsShort()} \n");
            toAppend += "\n";
            toAppend += $"Booking created: {CreationTime} by {AgentId} \n";
            toAppend += $"Booking issued: {FulfilledTime}";

            return toAppend;
        }
    }
}
