using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Adventure_Game
{
    class Messaging
    {

        public string username;
        public string friendUser;
        public int id;
        public int score;
        public string password;
        
        Messaging(string username, string friendUser, int id, int score, string password)
        {
            this.username = username;
            this.friendUser = friendUser;
            this.id = id;
            this.score = score;
            this.password = password;
        }


    }
}
