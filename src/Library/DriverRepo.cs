using System.Collections.Generic;

namespace Library
{
    public class DriverRepo : BaseRepo
    {
        private List<Driver> drivers = new List<Driver>();
        public void AddDriver(Driver driver)
        {
            drivers.Add(driver);
        }

        public void TweetPhoto()
        {
            
        }
    }
}