using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using Text_Adventure_Game;

namespace Text_Adventure_Game
{
    internal class Player
    {
        #region Players details
        //palyers details
        protected FileHandling currentFileHandling = new FileHandling();
        protected CharacterClass characterClass = new CharacterClass(3);
        private double health = 0;
        private string username;

        private int score = 0;
        private string password;
        protected DungeonQuizzes dungeonQuizzes = new DungeonQuizzes();
        public Player(double health, string username, int score, string password)
        {
            this.health = health;
            this.username = username;
            this.score = score;
            this.password = password;

        }
        #endregion
        #region Getters and setters
        public string getUsername()
        {
            return username;
        }
        public void setUsername(string username)
        {
            this.username = username;
        }
        public string getPassword()
        {
            return password;
        }
        public void setPassword(string password)
        {
            this.password = password;
        }




        public double getHealth()
        {
            return health;
        }
        public void setHealth(double health)
        {
            this.health = health;
        }


  

        public int getScore()
        {
            return this.score;
        }
        public void setScore(int score)
        {
            this.score = score;
        }
        #endregion
  
    }
    class FriendsXml : Player
    {
        public FriendsXml(double health, string username, int score, string password) : base( health,  username,  score,  password)
        {
            
        }
        protected bool fileExists = false;
        protected bool duplicateUser = false;
        protected bool usernamePresent = false;
        
        //dictionary for the graph
        protected Dictionary<string, Dictionary<string, string>> friends = new Dictionary<string, Dictionary<string, string>>();

        #region verifying users username and sending it to the AddXMLRecord
        //this method outputs the users friends as well as class the addfriends function
        public void Friends(string viewOrAddFriends, string currentUsername)
        {
            Console.Clear();
             List<string> duplicateFriend = new List<string>();
           
            //try catch is for if the user enters views friends but has no friends added. 
            try
            {
                if (viewOrAddFriends == "view")
                {
                    //checking if the user has ay friends by checking for their entry in the graph
                    if(!friends.ContainsKey(currentUsername))
                    {
                        Console.WriteLine("You have no friends added");
                        Thread.Sleep(2000);
                        dungeonQuizzes.returnMenu(getUsername());
                    }
                    //foreaching through key of the entry of the user
                    Console.WriteLine("Friends:\n");
                    foreach (string key in friends[getUsername()].Keys)
                    {
                        //outputting the users frineds list
                        if (!duplicateFriend.Contains(key))
                        {
                            //avoids the users username
                            if (key != getUsername())
                            {
                                Console.WriteLine("" + key + ": " + friends[getUsername()][key]);

                            }

                        }
  
                        duplicateFriend.Add(key);

                    }
                    Console.WriteLine("\nClick any key to go back to main menu");

                    Console.ReadKey();
                    dungeonQuizzes.returnMenu(getUsername());
                   
                    
                   
                }
                //adding users friends
                else if (viewOrAddFriends == "add")
                {
                    Console.WriteLine("Enter in the username of the friend you want to add");
                    string find = Console.ReadLine();

                    string[] values = new string[3];
                    //checking to see if the enterd friend exits

                    if (currentFileHandling.readRecord(find, "usersDetails.csv", 1))
                    {


                        //checking if the user has enterd in their own username
                        while (find == getUsername())
                        {
                            duplicateUser = true;
                            Console.WriteLine("You cannot enter in your own username\nTry again");
                            find = Console.ReadLine();
                            // adds the users friend to the friends dictionary if it is not its own username or empty
                            if (find != getUsername() && find != "")
                            {
                                Friends("add", getUsername());
                            }
                        }
                        //if the user enters a username other than there own
                        if (find != getUsername())
                        {
                            //checking if a key with the users username has already been created
                            if (!friends.ContainsKey(getUsername()))
                            {
                                //adds a new entry to the graph
                                friends.Add(getUsername(), new Dictionary<string, string>());

                            }
                            if (!friends[getUsername()].ContainsKey(find))
                            {
                                //checking if the user has already added that friend before
                                foreach (string key in friends[getUsername()].Keys)
                                {
                                    if (find == key)
                                    {
                                        Console.WriteLine("{0} has already been added to your friends list. Going back to main menu....", find);
                                        Thread.Sleep(2000);
                                        duplicateUser = true;
                                        dungeonQuizzes.returnMenu(getUsername());
                                    }
                                }

                                //adding the username to friends list if they have not entered that name before
                                if (duplicateUser == false)
                                {
                                    //if the file does not exist, it creates a new one
                                    if (!File.Exists("friends.xml"))
                                    {
                                        CreateXMLFile();
                                    }
                                    else
                                    {
                                        SQLHandling sqlHandling = new SQLHandling(getUsername(), getPassword());
                                        currentFileHandling.OutputField(find, "usersDetails.csv", 1, 3);
                                        //inserting the friends username so that the total of the users and firends id can be added to the messaging database
                                        sqlHandling.InsertTotalId(find);

                                        values[0] = getUsername();
                                        values[1] = find;
                                        values[2] = currentFileHandling.characTypeField;
                                        //checking if the user has its own entry in the dictionary
                                        if (friends.ContainsKey(getUsername()))
                                        {
                                            //checking to see if the file exits

                                            if (fileExists == true)
                                            {
                                                friends[getUsername()].Add(find, currentFileHandling.characTypeField);
                                                fileExists = false;

                                            }

                                            // populating the dictionary
                                            friends[getUsername()].Add(find, values[2]);

                                            foreach (var key in friends[getUsername()].Keys)
                                            {

                                                AddXMLRecord(key, friends[getUsername()][key]);
                                            }
                                            Console.WriteLine("Player found....Adding to friends list");

                                            Thread.Sleep(1500);
                                            dungeonQuizzes.returnMenu(getUsername());


                                        }
                                        //adds users username to the dictionary if they don't already have an entry
                                        else
                                        {
                                            //adding new entry to dictionary
                                            friends.Add(getUsername(), new Dictionary<string, string>());
                                            //adding friend to the particular entry
                                            friends[getUsername()].Add(find, currentFileHandling.characTypeField);

                                            //adding all the details to the xml file
                                            foreach (string key in friends[getUsername()].Keys)
                                            {
                                                AddXMLRecord(key, friends[getUsername()][key]);
                                            }


                                        }
                                    }

                                }
                            }
                            else
                            {
                                Console.WriteLine("{0} is already in your friends list. Going back to main menu....", find);
                                Thread.Sleep(2000);
                                dungeonQuizzes.returnMenu(getUsername());
                            }

                        }

                    }
                    else
                    {
                        // while friend username entered does not exist in the usersDetails.csv
                        while (!currentFileHandling.readRecord(find, "usersDetails.csv", 1))
                        {
                            string options = "Player's name you are looking for is  not found. Choose from the options below";
                            string[] optionsArr = { "View Friends", "Add Friends", "Back" };
                            KeyboardControls keyboardControls = new KeyboardControls(options, optionsArr);
                            int selected = keyboardControls.Run();
                            if (selected == 0)
                            {
                                Friends("view", getUsername());

                            }
                            else if (selected == 1)
                            {
                                Friends("add", getUsername());

                            }
                            else
                            {
                                dungeonQuizzes.returnMenu(getUsername());
                            }
                        }

                    }


                }

            }
            catch (Exception ex)
            {
                //user asked to type in friend again or go back to main menu

                Console.WriteLine("Error " + ex.Message);
               
            }
        }
        #endregion
        #region Creating a new XML file if it does not exist.
        private void CreateXMLFile()
        {
            //creating an XML file. This function is called if the "friends.XML" does not exist
            fileExists = true;
            Console.Clear();

            Console.WriteLine("Friends list not found. Starting again");
             XmlDocument doc = new XmlDocument();
            
            //appending to the XML doc
            XmlElement root = doc.CreateElement("FRIENDS");
            doc.AppendChild(root);
            doc.Save("friends.xml");

            string option = "Would you like to view or add a friend";
            string[] optionsArr = { "View", "Add" };
            //cheking if user wants to add or view friends
            KeyboardControls keyboardControls = new KeyboardControls(option, optionsArr);
            int selected = keyboardControls.Run();
            if(selected == 0)
            {
                Friends("add", getUsername());
            }
            else
            {
                Friends("view", getUsername());

            }

        }
        #endregion
        #region Adding/editing friends username details to xml files
        //method that adds data to the xml file
        private void AddXMLRecord(string friendsUsername, string friendCharacter)
        {


            XmlDocument doc = new XmlDocument();
            doc.Load("friends.xml");

            XmlNodeList user = doc.SelectNodes("FRIENDS/USERNAME");
            int count = 0;
            //checking if the users' username is present in the xml file
            foreach (XmlNode node in user)
            {

                if (node.ChildNodes[0].InnerText == getUsername())
                {
                    usernamePresent = true;
                }
                count++;
            }

            //checking if the users username  is already present in the USERNAME node XML. This is so that duplicate usernames dont get added
            if (usernamePresent)
            {
                //deletes the xml file
                DeletXmlRecord();

                doc.Load("friends.xml");
                //storing it in the root variable to act as a reference later on
                XmlNode root = doc.SelectSingleNode("FRIENDS");

                //creating an element 
                XmlElement XmlUserame = doc.CreateElement("USERNAME");
                //appending the users username to the tag
                XmlUserame.InnerText = getUsername();
                root.AppendChild(XmlUserame);

                // the usernameId will count how many firends the user has and each user will have a number place
                XmlAttribute usernameId = doc.CreateAttribute("id");
                
                //counts all the movie elements
                usernameId.Value = doc.SelectNodes("FRIENDS/FRIEND/FRIENDUSERNAME").Count.ToString();
                XmlUserame.Attributes.Append(usernameId);

                //adding all the items in the dictionary to the xml file
                foreach (string key in friends[getUsername()].Keys)
                {
                    XmlElement XmlFriendsUserame = doc.CreateElement("FRIENDUSERNAME");
                    XmlFriendsUserame.InnerText = key;
                    XmlUserame.AppendChild(XmlFriendsUserame);

                    XmlAttribute friendsUsernameId = doc.CreateAttribute("id");
                    //counts  elements
                    friendsUsernameId.Value = doc.SelectNodes("FRIENDS/FRIEND/FRIENDUSERNAME").Count.ToString();
                    XmlFriendsUserame.Attributes.Append(friendsUsernameId);


                    XmlElement XmlCharacter = doc.CreateElement("CHARACTER");
                    XmlCharacter.InnerText = friends[getUsername()][key];
                    XmlFriendsUserame.AppendChild(XmlCharacter);

                }

                doc.Save("friends.xml");

            }
            //creating the new nodes 
            else
            {
                doc.Load("friends.xml");
                XmlNode root = doc.SelectSingleNode("FRIENDS");

                //creating tags 
                XmlElement XmlUserame = doc.CreateElement("USERNAME");
                //adding the users username to that tag
                XmlUserame.InnerText = getUsername();
                root.AppendChild(XmlUserame);

                XmlAttribute usernameId = doc.CreateAttribute("id");
                usernameId.Value = doc.SelectNodes("FRIENDS/FRIEND/FRIENDUSERNAME").Count.ToString();
                XmlUserame.Attributes.Append(usernameId);

                //adding the users username to the xml file under the users friends
                XmlElement XmlFriendsUserame = doc.CreateElement("FRIENDUSERNAME");
                XmlFriendsUserame.InnerText = friendsUsername;
                XmlUserame.AppendChild(XmlFriendsUserame);
                //the friendsUsernameId will have the count of the place it is in the usernames friends.
                XmlAttribute friendsUsernameId = doc.CreateAttribute("id");
                friendsUsernameId.Value = doc.SelectNodes("FRIENDS/FRIEND/FRIENDUSERNAME").Count.ToString();
                XmlFriendsUserame.Attributes.Append(friendsUsernameId);

                //adding the character of the friend to the xml file
                XmlElement XmlCharacter = doc.CreateElement("CHARACTER");
                XmlCharacter.InnerText = friendCharacter;
                XmlFriendsUserame.AppendChild(XmlCharacter);
                doc.Save("friends.xml");
            }

        }
        #endregion
        #region Reading a xml file and adding items to the graph
        //reading the xml file
        public void ReadXMLRecord()
        {
            //try catch is for if the file does not exist.
            try
            {
                //checking if the file exist
                if (File.Exists("friends.xml"))
                {

                    XmlDocument doc = new XmlDocument();
                    doc.Load("friends.xml");

                    int amountOfUsers = 0;
                    XmlNodeList user = doc.SelectNodes("FRIENDS/USERNAME");
                    bool usernamePresent = false;
                    //checking if the users username is in the xml file
                    foreach (XmlNode node in user)
                    {
                        if (node.ChildNodes[amountOfUsers].InnerText == getUsername())
                        {
                            usernamePresent = true;
                        }

                    }
                    //checking if users  username is in the xml file
                    if (usernamePresent == true)
                    {

                        XmlNodeList nodes = doc.SelectNodes("FRIENDS/USERNAME/FRIENDUSERNAME");
                        XmlNodeList nodesForGraph = doc.SelectNodes("FRIENDS/USERNAME");
                        //adding the usernames stored in the xml file to the graph
                        foreach (XmlNode node in nodesForGraph)
                        {
                            friends.Add(node.ChildNodes[0].Value, new Dictionary<string, string>());
                        }
                        //adding the friends of each username stored to the graph

                        foreach (XmlNode node in nodes)
                        {
                            //adding the friends to the entry to the users entry
                            friends[getUsername()].Add(node.ChildNodes[0].Value, node.ChildNodes[1].InnerText);

                        }

                    }
                    doc.Save("friends.xml");


                }
                else
                {
                    CreateXMLFile();
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error: " + ex.Message);
            }


        }
        #endregion
        #region Deleting xml file
        //deleting xml record
        private void DeletXmlRecord()
        {
            //checking if file exists
            if (File.Exists("friends.xml"))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("friends.xml");
                var root = doc.SelectSingleNode("FRIENDS");
                var tgtNode = root.SelectSingleNode("USERNAME[@id = 0]");
                root.RemoveChild(tgtNode);
                doc.Save("friends.xml");
            }

        }
        #endregion

        #region Messaging
        // finding friend and talking to them
        public void Message()
        {
            Console.WriteLine("Enter in the username of the friend you want to talk to");
            string find = Console.ReadLine();
            //checking for an empty dictionary which will be able to let the program know if the friend graph is empty.
            if(friends.Count <= 0)
            {
                Console.WriteLine("You have to be friends with them first before talking to them");
                Thread.Sleep(2000);
                dungeonQuizzes.returnMenu(getUsername());
            }
            //checking if the friend already added to their freinds graph entry
            if (!friends[getUsername()].ContainsKey(find))
            {
                //can only message firends 
                Console.WriteLine("You have to be friends with them first before talking to them");
                Thread.Sleep(2000);
                dungeonQuizzes.returnMenu(getUsername());
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Enter back[B] to stop messaging.");
               
                SQLHandling sqlHandling = new SQLHandling(getUsername(), getPassword());

                sqlHandling.ReadingMessages(find);

                Console.WriteLine("\nEnter in your message:\n");

                while (true)
                {
                    Console.Write("> ");
                    string message = Console.ReadLine();
                    if (message == "back" || message == "b"  || message == "B")
                    {

                        dungeonQuizzes.returnMenu(getUsername());
                    }
                    //sends the message to the method so it can be updated on the database.
                    sqlHandling.UpdateMessages(find, message);
                }
            }
        }
        #endregion
    }
}
