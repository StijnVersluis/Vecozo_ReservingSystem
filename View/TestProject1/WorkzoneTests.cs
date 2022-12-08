namespace VecozoTests
{
    [TestClass]
    public class WorkzoneTests
    {
        [TestMethod]
        public void TestWorkzonesAvailable()
        {
            var dal = new WorkzoneDAL();
            WorkzoneContainer workzoneContainer = new WorkzoneContainer(dal);
            var workzone = workzoneContainer.GetById(1);
            var available = workzone.GetAvailableWorkspaces(new DateTime(2000, 12, 8, 12, 00, 00), dal);
            Assert.IsNotNull(available);
            Assert.AreEqual(workzone.Workspaces-1, available);
        }
    }
}
