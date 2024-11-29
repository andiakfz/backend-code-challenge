using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Shared.Helpers
{
    public static class CalculationHelper
    {
        const double RadiusInKm = 6371;

        public static TimeSpan CalculateETA(double distanceKmh, double velocityKmh)
        {
            if (velocityKmh < 0)
            {
                throw new ArgumentException("Invalid velocity number");
            }

            double etaInHour = distanceKmh / velocityKmh;
            TimeSpan eta = TimeSpan.FromHours(etaInHour);
            return eta;
        }

        public static double CalculateGeoDistance(
            double shipLat, 
            double shipLon,
            double portLat,
            double portLon)
        {
            // USING HAVERSINE FORMULA
            double distLat = Radians(shipLat - portLat);
            double distLon = Radians(shipLon - portLon);

            double a = (Math.Sin(distLat / 2) * Math.Sin(distLat / 2)) + 
                        Math.Cos(Radians(shipLat)) * Math.Cos(Radians(portLat)) * 
                       (Math.Sin(distLon / 2) * Math.Sin(distLon / 2));
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return RadiusInKm * c;
        }

        public static double Radians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        public static double VelocityToKmh(double velocity)
        {
            return velocity * 3.6;
        }
    }
}
