using TravelReservationSystem.Data;
using System.Linq;

namespace TravelReservationSystem.Helpers
{
    public static class AdminHelper
    {
        public static int GetPendingReservationCount(AppDbContext context)
        {
            return context.Reservations.Count(r => r.Status == "Pending");
        }
    }
}