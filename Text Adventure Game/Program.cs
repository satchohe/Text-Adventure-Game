using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;


namespace Text_Adventure_Game
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //fghj

            DungeonQuizzes dungeonQuizzes = new DungeonQuizzes();
            SQLHandling hash = new SQLHandling("steve", "steve");
            //hash.createDbTable("CREATE table tblUsers(ID INT primary key, USERNAME VARCHAR(50), OVERFLOW INT NULL)\r\n");

            //hash.createDbTable("CREATE table tblMessage2(TOTAL_ID INT primary key, TEXT VARCHAR(MAX))");
            //hash.deleteTable();
            //hash.addUser(2,"me",2);
            // hash.getUser("SELECT * FROM tblMessage2", "message");
            dungeonQuizzes.Login();
            //hash.deleteTable("DROP DATABASE tblMessage2") ;
            //hash.createDbTable();


        }
       
     
    }



}
