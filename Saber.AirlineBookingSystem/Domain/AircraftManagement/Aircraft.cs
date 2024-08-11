using Saber.AirlineBookingSystem.Domain.Enums;

namespace Saber.AirlineBookingSystem.Domain.AircraftManagement
{
    public class Aircraft
    {
        public string AirlineName { get; set; } = string.Empty;
        public AircraftType AircraftModel { get; set; }
        public int SeatCapacity { get; set; }
    }
}
