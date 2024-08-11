using Saber.AirlineBookingSystem.Domain.AircraftManagement;
using Saber.AirlineBookingSystem.Domain.Enums;

namespace Saber.AirlineBookingSystem.Domain.FlightManagement
{
    public class Flight
    {
        public int FlightId { get; private set; }
        public FlightNumberType FlightNumber { get; private set; }
        public Aircraft Aircraft { get; private set; }
        public string DepartureAirport { get; private set; }
        public string ArrivalAirport { get; private set; }
        public DateTime DepartureTime { get; private set; }
        public DateTime ArrivalTime { get; private set; }
        public int RemainingSeats { get; private set; }
        public decimal TotalKilometers { get; private set; }

        public Flight(int flightId, FlightNumberType flightNumber, Aircraft aircraft, string departAirport, string arriveAirport, DateTime departTime, DateTime arriveTime, int remainingSeats, decimal totalKilometers)
        { 
            FlightId = flightId;
            FlightNumber = flightNumber;
            Aircraft = aircraft;
            DepartureAirport = departAirport;
            ArrivalAirport = arriveAirport;
            DepartureTime = departTime;
            ArrivalTime = arriveTime;
            RemainingSeats = remainingSeats;
            TotalKilometers = totalKilometers;
        }

        public void DecreaseAvailableSeats(int amount)
        {
            if (RemainingSeats >= amount)
                RemainingSeats -= amount;
            else
                Console.WriteLine($"Seats cannot be booked - there are not enough available seats");
        }

        public string DisplayFlightDetailsShort(bool withId = false)
        { 
            return $"{(withId ? $"(ID: {FlightId}) " : "")}{FlightNumber} : {DepartureAirport} - {ArrivalAirport} \n" +
                $"{DepartureTime} - {ArrivalTime} \n{RemainingSeats} seats available!";
        }

        public string DisplayFlightDetailsFull()
        {
            return $"{FlightNumber} : {DepartureAirport} - {ArrivalAirport} \n" +
                $"Departure from {DepartureAirport}: {DepartureTime} \nArrival at {ArrivalAirport}: {ArrivalTime} \n" +
                $"Operated by {Aircraft.AirlineName} ({Aircraft.AircraftModel}) \n{RemainingSeats}/{Aircraft.SeatCapacity} seats available!";
        }

        public decimal SeatPrice()
        {
            int takenSeats = Aircraft.SeatCapacity - RemainingSeats;

            decimal kmCost = 0.1M * TotalKilometers; // BaseFarePerKm * TotalKilometers
            decimal demandCost = 0.5M * (takenSeats / Aircraft.SeatCapacity) * kmCost; // DemandFactor * (TakenSeats / SeatCapacity) * CostPerKm
            decimal surchargeCost = 14.67M;

            return Math.Round(kmCost + demandCost + surchargeCost, 2);
        }
    }
}
