
using DataLayer;
using System.Data;

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
            var reservations = rCont.GetReservationsWithinTimeFrameFromWorkzone(1, arriving, leaving);
            Assert.IsNotNull(reservations);
            Assert.AreEqual(4, reservations.Count);
        }
    }
}