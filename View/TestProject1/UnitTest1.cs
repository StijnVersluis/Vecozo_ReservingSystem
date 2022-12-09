
using DataLayer;
using System.Data;
using System.Resources;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        ReservationContainer rCont = new ReservationContainer(new ReservationDAL());
        [TestMethod]
        public void TestCheckIfWorkzoneAvailable()
        {
            DateTime arriving = new DateTime(2022, 11, 25, 12, 0, 0);
            DateTime leaving = new DateTime(2022, 11, 25, 14, 0, 0);
            var reservations = rCont.GetReservationsWithinTimeFrame(arriving, leaving).Where(reservation => reservation.Workzone_id == 1).ToList();
            Assert.IsNotNull(reservations);
            Assert.AreEqual(0, reservations.Count);
        }

        [TestMethod]
        public void TestUserAvailable()
        {
            string datetimeString = "08/12/2022 11:20:00";
            DateTime datetime = DateTime.Parse(datetimeString);

        }
    }
}