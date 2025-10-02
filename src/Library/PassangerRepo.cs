using System.Collections.Generic;

namespace Library
{
    public class PassangerRepo : BaseRepo
    {
        private List<Passanger> passangers = new List<Passanger>();
        public void AddPassanger(Passanger passanger)
        {
            passangers.Add(passanger);
        }

        public void TweetPhoto()
        {
            
        }
    }
}