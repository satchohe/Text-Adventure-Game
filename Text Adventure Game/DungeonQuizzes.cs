using Microsoft.SqlServer.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Text_Adventure_Game
{
    internal class DungeonQuizzes
    {
        public  string usernameConnection = "";
        protected  Dungeons dungeons = new Dungeons();
        protected FileHandling currentFileHandling = new FileHandling();
        protected CharacterClass characterClass = new CharacterClass(3);
        protected Search_and_Sort<double> doubleSearchAndSort = new Search_and_Sort<double>();
        protected Search_and_Sort<string> stringSearchAndSort = new Search_and_Sort<string>();
        protected List<string> invenItems = new List<string>();

        public int arrTop;




        #region Login
        //login and sign in.


        public void Login()
        {
     
            int usersScore;
            double usersHealth;
            const string filepath = "usersDetails.csv";
            string options = "Choose from the options below";
            bool check = false;
            string usernameConnection = "";

            string[] optionsArr = { "Login", "Registor" };
            KeyboardControls keyboardControls = new KeyboardControls(options, optionsArr);
            int choice = keyboardControls.Run();

            ////////edited from here
            do
            {
                Console.Clear();

                //login procedure. The user enters the username and it writes it to csv file.
                if (choice == 0)
                {
                    Console.WriteLine("Enter in your username:", ConsoleColor.White, false);
                    string usernameLogin = Console.ReadLine();

                    string repeat = "Enter in the password: ";
                    string password = ReplacePasswordLetters(repeat);
                    SQLHandling sqlHandling = new SQLHandling(usernameLogin, password);

                    if (sqlHandling.userAccount(false))
                    {


                        //health of the user is fetched
                        currentFileHandling.OutputField(usernameLogin, filepath, 1, 4);
                        if(currentFileHandling.fieldWant == null || currentFileHandling.fieldWant == "")
                        {
                            Console.WriteLine("Not welcome Welcome back "   , ConsoleColor.Green);


                        }
                        else
                        {
                            Console.WriteLine("Welcome back " + usernameLogin, ConsoleColor.Green);
                        }



                            //data is stored here
                            usersHealth = double.Parse(currentFileHandling.fieldWant);

                        //fetches the users score
                        currentFileHandling.OutputField(usernameLogin, filepath, 1, 3);
                        
                        //stores it here
                        usersScore = int.Parse(currentFileHandling.fieldWant);

                        const string invenFilepath = "inventory.csv";

                        //reading the inventory record
                        currentFileHandling.readRecord(usernameLogin, invenFilepath, 1);
                        usernameConnection = currentFileHandling.getUsernameCheckPoint();
                        //adding the items in the inventory.csv file to the inventory list

                        if (currentFileHandling.invenarr.Length > 1)
                        {
                            for (int i = 1; i <= currentFileHandling.invenarr.Length - 1; i++)
                            {
                                if (currentFileHandling.invenarr[i] == null || currentFileHandling.invenarr[i] == "")
                                {

                                }
                                else
                                {
                                    invenItems.Add(currentFileHandling.invenarr[i]);

                                }

                            }


                        }
                        string chechingNullValues;
                        //this counts how many items in the stack and inventory list are free for the user. 
                        int emptyStackIndexcount = 0;

                        //reading the questions.csv file and adding it to the stack

                        #region fetching data for the stack
                        currentFileHandling.readRecord(usernameLogin, "questions.csv", 1);
                        Stack currentStack = new Stack(6);


                        if (currentFileHandling.getQuestionsArr().Length > 1)
                        {
                            for (int i = 1; i <= currentFileHandling.invenarr.Length - 1; i++)
                            {
                                currentStack.items[i - 1] = currentFileHandling.invenarr[i];
                            }
                        }
                        foreach (string item in currentFileHandling.getQuestionsArr())
                        {
                            chechingNullValues = item;
                            if (chechingNullValues == null || chechingNullValues == "")
                            {
                                emptyStackIndexcount++;
                            }

                        }
                        currentStack.top = emptyStackIndexcount > 0 ? 4 - (emptyStackIndexcount) : currentStack.top = -1;

            
                        currentStack.top = emptyStackIndexcount - 2;

                        //adding all the items in the freinds.xml file to the graph
                        Dungeons dun1 = new Dungeons();
                        Dun2 dun2 = new Dun2();
                        Dun3 dun3 = new Dun3();
                        Dun4 dun4 = new Dun4();
                        Dun5 dun5 = new Dun5();


                        Menu(usersHealth, usernameLogin, usersScore, password);
                    }
                    else
                    {
                        Login();
                    }
                


                    #endregion

                    check = true;

                }
                else if (choice == 1)
                {
                    FileHandling currentFileHandling = new FileHandling();

                    Console.WriteLine("Enter in a username:", ConsoleColor.White, false);
                    string usernameSignIn = Console.ReadLine();
                    while (usernameSignIn == "" || usernameSignIn.Contains(","))
                    {

                        Console.WriteLine("please enter in a correct format. Meaning no commas.\nEnter in a valid username:");
                        usernameSignIn = Console.ReadLine();
                    }

                    string repeat = "Enter in the password: ";
                    string password = ReplacePasswordLetters(repeat);
                    while(password.Length > 16)
                    {
                        Console.WriteLine("password is longer than 16 characters\nEnter password again: ");
                        password = Console.ReadLine();
                        
                    }
                
                    string repeat2 = "Confirm your password: ";
                    string passwordRepeate = ReplacePasswordLetters(repeat2);
                   

                    if (password != passwordRepeate)
                    {
                        Console.WriteLine("Passwords dont match");
                        Thread.Sleep(2000);
                        Console.Clear();
                        Login();
                    }
                    while (password == null || password.Contains(",") || password == "b" || password == "B")
                    {

                        Console.WriteLine("please enter in a correct format. Meaning no commas.\nEnter in a valuserId password:");
                        password = Console.ReadLine();
                    }
                    //checks if the user with that username has already been enterd.

                    SQLHandling sqlHandling = new SQLHandling(usernameSignIn, password);

                    while (sqlHandling.getUser("SELECT USERNAME FROM tblUsers;", "string").Item2.Contains(usernameSignIn))
                    {
                        Console.WriteLine("Enter another username, this one is taken");
                        usernameSignIn = Console.ReadLine();

                        currentFileHandling.readRecord(usernameSignIn, filepath, 1);
                    }

                   

                    //adding details inventory csv file
                    currentFileHandling.addRecord(usernameSignIn, "", "", "","","", "inventory.csv");
                    //adding details to the userDetails csv file
                   int character = CharecterType();
                    sqlHandling.userAccount(true);
                    Thread.Sleep(2000);
                    if (character == 0)
                    {
                        Mage mage = new Mage(4);
                        currentFileHandling.addRecord(usernameSignIn, mage.heroType, 0.ToString(), mage.getHealth().ToString(),"", filepath);
                        

                        Console.WriteLine("\nAccount created.\nLog into your new account");
                        Thread.Sleep(2000);
                        Console.Clear();
                        Login();



                    }
                    else if (character == 1)
                    {
                        Monk monk = new Monk(3);
                        currentFileHandling.addRecord(usernameSignIn, monk.heroType, 0.ToString(), monk.getHealth().ToString(),"",  filepath);
                        Console.WriteLine("\nAccount created.\nLog into your new account");
                        Thread.Sleep(2000);
                        Login();


                    }

                    else if (character == 2)
                    {
                        Warroir warroir = new Warroir(3);
                        currentFileHandling.addRecord(usernameSignIn, warroir.heroType, 0.ToString(), warroir.getHealth().ToString(),"", filepath);
                        Console.WriteLine("\nAccount created.\nLog into your new account");
                        Thread.Sleep(2000);
                        Login();

                    }


                }



            } while (check == false);

        }

        #endregion
        #region Menu
        private void Menu(double usersHealth = 0, string username = null, int usersScore = 0, string password = "")
        {
            Console.Clear();

            string options = "\nWelcome To Dungeon Escape!\nType from:\n";
            string[] optionsArr = { "Play", "Help", "Leaderboard", "Friends", "Logout" };
            //Asking the user to choose and using a do while loop to constantly ask if menuOptions does not match from the options. 

            KeyboardControls keyboardControls = new KeyboardControls(options, optionsArr);
            int selected = keyboardControls.Run();

            switch (selected)
            {
                case 0:
                    Play(usersHealth, username,  usersScore, password);
                    break;
                case 1:
                    Help();
                    break;
                case 2:
                    LeaderBoard();
                    break;
                case 3:
                    AddFreinds(usersHealth, username, usersScore, password);
                    break;
                    case 4:
                    Login();
                    break;

            }



        }
        #endregion
        #region The game
        //the game itself.
        private void Play(double health = 0, string username = "", int score = 0, string password = "")
        {
            
            Player currentPlayer = new Player(health, username, score, password);
            FriendsXml friendsXml = new FriendsXml(health, username, score, password);
            Stack currentStack = new Stack(6);


            const string filepath = "usersDetails.csv";
            bool nextLevel = false;
            bool level1 = true;

            
            //creating a users characters at run time
            currentFileHandling.OutputField(username, "usersDetails.csv", 1, 4);
            if (currentFileHandling.fieldWant == "Monk")
            {
                Monk monk = new Monk(3);
                characterClass.currentChar = CharacterClass.CharacterTypes.Monk;

            }
            else if (currentFileHandling.fieldWant == "Mage")
            {
                Mage mage = new Mage(4);

                characterClass.currentChar = CharacterClass.CharacterTypes.Mage;
            }
            else if (currentFileHandling.fieldWant == "Warrior")
            {
                Warroir warroir = new Warroir(3);
                characterClass.currentChar = CharacterClass.CharacterTypes.Warrior;

            }

            while (nextLevel == false)
            {


                //The introduction to the game
                #region players score is 0
                if (currentPlayer.getScore() == 0)
                {
                     #region Outputting the introduction of the game
                    //exception checks if the user has entered in a value in except for a double
                    List<double> fractionList = new List<double>();
                    fractionList.Add(0.33);
                    fractionList.Add(0.40);
                    fractionList.Add(0.50);
                    fractionList.Add(0.75);
                    List<double> usersFractionList = new List<double>();

                    string question = "write the fractions in decimals to 2 decimal place from smallest to largest. 3/4 --  6/12 -- 1/3 and 2/5";
                    Print($"LOCATION: {dungeons.getLocation()}\n", ConsoleColor.Magenta);
                    Print("You slowly open your eyes and find yourself lying on a cold moist  surface. A sudden wave of nausea hits you, forcing" +
                        "you to flip over and vomit on the floor.You hold your head feeling dizzy. You look down and find that you were asleep on a stone wall." +
                        "Confused, you look around, scanning the surrounding areas. As you stand up, you see a half apron around your waist and inside you can" +
                        "feel some things.\nYou can't even remember your own name.\nThere is writing on the wall and books. One of the books are open and a question is written " + question, ConsoleColor.White, true, 0);
                    //looping till the answer is right or users life is 0
                    while (level1)
                    {
                        usersFractionList.Clear();

                        for (int i = 1; i <= 4; i++)
                        {
                            Print(i + ". Enter in the decimal");
                            string answer = Console.ReadLine();
                            //checking if the numbers entered are decimal
                            double number;
                            if (double.TryParse(answer, out number))
                            {
                                if (number == 0.40)
                                {
                                    usersFractionList.Add(0.4);

                                }
                                else if (number == 0.50)
                                {
                                    usersFractionList.Add(0.5);

                                }
                                else
                                {
                                    usersFractionList.Add(number);

                                }
                            }
                            else
                            {
                                Console.WriteLine("Please enter in the fractions as decimals");
                                Thread.Sleep(2500);
                                Console.Clear();
                            }
                        }
                        doubleSearchAndSort.itemList = fractionList;
                        int correctCount = 0;
                        foreach (double fraction in usersFractionList)
                        {
                            foreach (double fraction2 in doubleSearchAndSort.BubbleSort())
                            {
                                if (fraction == fraction2)
                                {
                                    correctCount++;
                                }
                            }
                        }
                        if (correctCount == 4)
                        {

                            Print("correct", ConsoleColor.Green);
                       
                            currentStack.push(question);
                            currentPlayer.setScore(1);

                            if (characterClass.currentChar == CharacterClass.CharacterTypes.Mage)
                            {
                                currentFileHandling.editRecord(currentPlayer.getUsername(), filepath, 1, currentPlayer.getUsername(), characterClass.currentChar.ToString(), currentPlayer.getScore().ToString(), 4.ToString());

                            }
                            else
                            {
                                currentFileHandling.editRecord(currentPlayer.getUsername(), filepath, 1, currentPlayer.getUsername(), characterClass.currentChar.ToString(), currentPlayer.getScore().ToString(), 3.ToString());
                            }

                            //writng items to csv files
                            string[] tempInven = new string[100];
                            if(invenItems.Count > 0)
                            {
                                for (int i = 0; i < invenItems.Count; i++)
                                {
                                    tempInven[i] = invenItems[i];
                                }

                                writeToCSV(currentPlayer.getUsername(), tempInven[0], tempInven[1], tempInven[2], tempInven[3], tempInven[4], "inventory.csv");
                            }
                            
                            currentStack.stackWriteToCSV(username);


                            break;

                        }
                        else
                        {
                            currentPlayer.setHealth(SpecailEffects(characterClass.currentChar.ToString(), currentPlayer.getHealth()));

                            Print("Incorrect. A book hits your head. You loose a life. Lives: " + currentPlayer.getHealth());
                        }

                        if (currentPlayer.getHealth() <= 1)
                        {
                            Print("You have no more lives", ConsoleColor.DarkRed);

                            currentPlayer.setHealth(SpecailEffects(characterClass.currentChar.ToString(), currentPlayer.getHealth()));

                            Exit(currentPlayer.getUsername(),  characterClass.currentChar.ToString(), currentPlayer.getScore());
                        }
                    }

                    #endregion
                }
                #endregion

                #region PLayer score is 1
                else if (currentPlayer.getScore() == 1)
                {


                    Print("\nAcross the room their is a board locked behind a glass case.\nYou see some writing but you cant really make it out through the thick glass.");
                    const string question = "What number goes up and doesn't come down";
                    bool wrongAnswer = false;
                    string answer = "";
                    // adding the inventory items and the find key word to the itemList in the stringSearchAndSort class

                    stringSearchAndSort.itemList = invenItems;
                    stringSearchAndSort.find = "key";
                    //checking if the inventory is empty. This stops the linearsearch below from throwing an error.
                    if (invenItems.Count > 0)
                    {
                        //using linearSearch to check if the users inventory has the neccesary item in their inventory to continue
                        if (stringSearchAndSort.LinearSearch())
                        {
                            Print("\nYou go to the cabinet and attempt to unlock it with a key.\nYour successful. In the cabinet their are a bunch of letters and numbers.  You read what the question is:\n" + question + "\n");
                            //looping till the answer is right or user life is 0
                            while (answer != "age")
                            {//checking the user's input to see what the required output should be

                                Print("\n> ");
                                answer = Console.ReadLine().ToLower();
                                wrongAnswer = Input(answer, currentPlayer.getScore(), currentPlayer.getUsername(), true);
                                if (wrongAnswer == false  && answer != "age")
                                {
                                    Print("\nA hole suddenly appears from the cabinet and poison spews onto you. " +
                                            "You skin starts to burn as you have " + currentPlayer.getHealth() +
                                            " health left\nAnswer again\n> ", ConsoleColor.Red);//minus health

                                }
                             

                               
                                //what is done if the user has no more lives
                                if (currentPlayer.getHealth() <= 1)
                                {
                                    Print("You have no more lives", ConsoleColor.DarkRed);

                                    currentPlayer.setHealth(SpecailEffects(characterClass.currentChar.ToString(), currentPlayer.getHealth()));
                                    Exit(currentPlayer.getUsername(), characterClass.currentChar.ToString(), currentPlayer.getScore());
                                }

                            }
                            //saving the users current data if they win and writing them to file.
                            if (answer == "age")
                            {
                                currentPlayer.setScore(score +  NextLevel(username, filepath, currentPlayer.getScore()));
                                currentStack.push(question);
                                currentStack.stackWriteToCSV(username);
                            }
                        }
                        else
                        {

                            Print("\nYou dont have the means to open the case\n", ConsoleColor.DarkMagenta);
                            Print("> ");
                            Input(Console.ReadLine().ToLower(), score, username);
                        }
                    }
                    else
                    {

                        Print("\nYou dont have the means to open the case\n", ConsoleColor.DarkMagenta);
                        Print("> ");
                        Input(Console.ReadLine().ToLower(), score, username);
                    }




                }
                #endregion
                #region Players score is 2
                else if (currentPlayer.getScore() == 2)
                {
                    

                    Dun2 dun2 = new Dun2();
                    Print("\n" + dun2.getLocation() + "\n\n\n", ConsoleColor.Blue);
                    Print("As soon as you walk in you are met with a blast of cold air. You seee the cause of this cold air, an open window but this one is unusal. It's mechanical in a way that it winds to shut but the " +
                        "turning device is not attached to it.On the wall hangs a baord showing the temperature slowly decreassing. You have to close the window before you freeze.");

                    const string question = "I am an odd number; take away an alphabet and I become even. What number am I?";
                    string answer = "";

                    // adding the users inventory item to the stringsearchandSort class so that any type of search is possible
                    stringSearchAndSort.itemList = invenItems;
                    stringSearchAndSort.find = "turning-mechanism";
                    //checking if the inventory is empty. This is so that the linear search below does not throw an error
                    if (invenItems.Count > 0)
                    {//searching the users inventory to see if they have the necceasry items to advace.
                        if (stringSearchAndSort.LinearSearch())
                        {
                            Print("You go to attach the device onto the window and shut it. Over time you feel the air starting to warm up. A brick from the wall pops out and pen and paper comes out from it. On the paper it says.\n" + question + "\n");
                           
                            bool wrongAnswer = false;

                            while (answer != "seven" && answer != "7")
                            {
                                Print("\n> ");
                                answer = Console.ReadLine().ToLower();
                                wrongAnswer =  Input(answer, currentPlayer.getScore(), currentPlayer.getUsername(), true);
                                if (wrongAnswer == false && (answer != "seven" || answer != "7"))
                                {
                                    currentPlayer.setHealth(SpecailEffects(characterClass.currentChar.ToString(), currentPlayer.getHealth()));
                                    Print("Incorrect! One of the nearby animals comes to attack you. You now have " + currentPlayer.getHealth() + " live(s) left\nAnswer again\n", ConsoleColor.Red);


                                }
                                if (currentPlayer.getHealth() <= 1)
                                {
                                    Print("You have no more lives. You have died from hyperthermia", ConsoleColor.DarkRed);
                                    currentPlayer.setHealth(SpecailEffects(characterClass.currentChar.ToString(), currentPlayer.getHealth()));
                                    Exit(currentPlayer.getUsername(), characterClass.currentChar.ToString(), currentPlayer.getScore());
                                }
                            }
                            //saving the users deatils and writing them to file
                            if (answer == "seven" || answer == "7")
                            {
                                currentPlayer.setScore(score + NextLevel(username, filepath, currentPlayer.getScore()));
                                currentStack.push(question);
                                currentStack.stackWriteToCSV(username);

                            }

                        }
                        //this is what is outputted if the user does not have the neccesary item in the inventory.
                        else
                        {
                            Print("\nYou dont have the means to close the window\n", ConsoleColor.DarkMagenta);
                            Print("> ");
                            Input(Console.ReadLine().ToLower(), score, username);
                        }
                    }
                    //this is if the users invetory is empty
                    else
                    {
                        Print("\nYou dont have the means to close the window\n", ConsoleColor.DarkMagenta);
                        Print("> ");
                        Input(Console.ReadLine().ToLower(), score, username);
                    }


                    // 

                }
                #endregion
                //third dungeon
                #region Players score is 3
                else if (currentPlayer.getScore() == 3)
                {


                    Dun3 dun3 = new Dun3();
                    Print("\n" + dun3.getLocation() + "\n", ConsoleColor.Green);
                    Print("");
                    bool wrongAnswer = false;
                    const string question = "There are several books on a bookshelf. How many books are on the shelf if one book is the 4th from the left and the 6th from the right?";
                    string answer = "";
                    Print("You walk through and instantly feel warmer. The door shuts close behind you. You look down to find yourself standing on grass and moss. This room looks is a forest with roaming animals.  Naield to a tree their is a board with buttons to type on. It says:\n" + question);
                    while (answer != "nine" && answer != "9")
                    {

                        Print("\n> ");
                        answer = Console.ReadLine().ToLower();
                        wrongAnswer = Input(answer, currentPlayer.getScore(), currentPlayer.getUsername(), true);
                        if (wrongAnswer == false && (answer != "nine" || answer != "9"))
                        {
                            currentPlayer.setHealth(SpecailEffects(characterClass.currentChar.ToString(), currentPlayer.getHealth()));
                            Print("Incorrect! One of the nearby animals comes to attack you. You now have " + currentPlayer.getHealth() + " live(s) left\nAnswer again\n", ConsoleColor.Red);


                        }             
                        if (currentPlayer.getHealth() <= 1)
                        {
                            Print("You have no more lives.", ConsoleColor.DarkRed);
                            SpecailEffects(characterClass.currentChar.ToString(), currentPlayer.getHealth());
                            Exit(currentPlayer.getUsername(), characterClass.currentChar.ToString(), currentPlayer.getScore());
                        }
                    }
                    if (answer == "nine" || answer == "9")
                    {
                        currentPlayer.setScore(score + NextLevel(username, filepath, currentPlayer.getScore()));
                        currentStack.push(question);
                        currentStack.stackWriteToCSV(username);

                    }



                }
                #endregion
                //4th dungeon
                #region Dungeon 4
                else if (currentPlayer.getScore() == 4)
                {


                    Dun4 dun4 = new Dun4();
                    Print("\n" + dun4.getLocation() + "\n", ConsoleColor.DarkGray);
                    Print("");
                    bool wrongAnswer = false;
                    const string question = "I have a ring but no finger. What am I?";
                    string answer;
                    Print("You walk through and you are instantly met with darkness. The door shuts close behind you. A lightbulb switches on and you can see a little note on the floor. It says:\n " + question);
                    answer = Console.ReadLine().ToLower();
                    while (answer != "telephone" && answer != "phone")
                    {
                        Print("\n> ");
                        answer = Console.ReadLine().ToLower();
                        wrongAnswer = Input(answer, currentPlayer.getScore(), currentPlayer.getUsername(), true);
                        if (wrongAnswer == false && (answer != "telephone" || answer != "phone"))
                        {
                            currentPlayer.setHealth(SpecailEffects(characterClass.currentChar.ToString(), currentPlayer.getHealth()));
                            Print("Incorrect! One of the nearby animals comes to attack you. You now have " + currentPlayer.getHealth() + " live(s) left\nAnswer again\n", ConsoleColor.Red);


                        }       
                        if (currentPlayer.getHealth() <= 1)
                        {
                            Print("You have no more lives.", ConsoleColor.DarkRed);
                            SpecailEffects(characterClass.currentChar.ToString(), currentPlayer.getHealth());
                            Exit(currentPlayer.getUsername(), characterClass.currentChar.ToString(), currentPlayer.getScore());
                        }
                    }
                    if (answer == "telephone" || answer == "phone")
                    {
                        currentPlayer.setScore(score + NextLevel(username, filepath, currentPlayer.getScore()));
                        currentStack.push(question);
                        currentStack.stackWriteToCSV(username);

                    }



                }
                #endregion
                //5th dungone -- final dungeon
                #region Dungeon 5 -- final dungeon
                else if (currentPlayer.getScore() == 5)
                {


                    Dun5 dun5 = new Dun5();
                    Print("\n" + dun5.getLocation() + "\n", ConsoleColor.DarkYellow);
                    Print("");

                    const string question = "I have a face but no eyes and hands but no arms. What am I?";
                    string answer;
                    bool wrongAnswer = false;
                    Print("You walk into a bright room. The door slowly shuts close behind you. Light is coming out from decorated window panes and the floor is filled with red carpet. Every object is gold plated. You go to the table in the middle of the room and on it their is a board that says:\nThis is the last level. " + question);
                    answer = Console.ReadLine().ToLower();
                    while (answer != "clock")
                    {

                        Print("\n> ");
                        answer = Console.ReadLine().ToLower();
                        wrongAnswer = Input(answer, currentPlayer.getScore(), currentPlayer.getUsername(), true);
                        if (wrongAnswer == false && (answer != "clock"))
                        {
                            currentPlayer.setHealth(SpecailEffects(characterClass.currentChar.ToString(), currentPlayer.getHealth()));
                            Print("Incorrect! An arrow is released from a cross bow hung on the wall " + currentPlayer.getHealth() + "live(s) left\nAnswer again\n", ConsoleColor.Red);

                        }
                        if (currentPlayer.getHealth() <= 1)
                        {
                            Print("You have no more lives.", ConsoleColor.DarkRed);
                            SpecailEffects(characterClass.currentChar.ToString(), currentPlayer.getHealth());
                            Exit(currentPlayer.getUsername(), characterClass.currentChar.ToString(), currentPlayer.getScore());
                        }
                    }
                    if (answer == "clock")
                    {
                        if (invenItems.Contains("chess"))
                        {
                            Print("You have beaten the dungeon and have decided to take the treasure chest");

                        }
                       
                        Print("Correct\nYou have won!", ConsoleColor.Green);

                        currentPlayer.setScore(score + NextLevel(username, filepath, currentPlayer.getScore()));
                        currentStack.push(question);
                        currentStack.stackWriteToCSV(username);


                        Print("Would you like to see all the questions you have answered?", ConsoleColor.White, false);
                        string outputQuestions = Console.ReadLine().ToLower();
                        if (outputQuestions == "yes")
                        {
                            for (int i = 0; i < currentStack.items.Length; i++)
                            {
                                currentStack.pop();
                            }
                            ConsoleKeyInfo cki;
                            while (true)
                            {

                                Print("Click enter to exit");
                                cki = Console.ReadKey();
                                if (cki.Key == ConsoleKey.Escape)
                                {
                                    break;

                                }
                            }
                            Exit(currentPlayer.getUsername(), characterClass.currentChar.ToString(), currentPlayer.getScore());
                        }
                        else
                        {
                            Console.Clear();
                            const string won = "You have beaten the game";
                            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (won.Length / 2)) + "}", won));

                            string playAgain = "Would you like to play again? Choose yes or no";
                            string[] optionsArr = { "Yes", "No" };


                            KeyboardControls keyboardControls = new KeyboardControls(playAgain, optionsArr);
                            int seleceted = keyboardControls.Run();

                            if (seleceted == 0)
                            {
                                currentPlayer.setScore(0);
                                Array.Clear(currentStack.items, 0, currentStack.items.Length);
                                currentStack.top = -1;
                                invenItems.Clear();
                                currentStack.stackWriteToCSV(username);

                                Menu();
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (won.Length / 2)) + "}", won));
                                Console.Read();
                            }
                            Print("Click enter to exit");
                            Console.ReadLine();
                            Exit(currentPlayer.getUsername(), characterClass.currentChar.ToString(), currentPlayer.getScore());
                        }


                    }



                }
                #endregion
                #region user has won
                //this is outputted if the user has won
                else if (currentPlayer.getScore() >= 6)
                {
                    Console.Clear();
                    const string won = "You have beaten the game";
                    Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (won.Length / 2)) + "}", won));

                    string playAgain = "Would you like to play again? Choose yes or no";
                    string[] optionsArr = { "Yes", "No" };


                    KeyboardControls keyboardControls = new KeyboardControls(playAgain, optionsArr);
                    int seleceted = keyboardControls.Run();

                    if (seleceted == 0)
                    {
                        currentPlayer.setScore(0);
                        Array.Clear(currentStack.items, 0, currentStack.items.Length);
                        currentStack.top = -1;
                        invenItems.Clear();

                        currentStack.stackWriteToCSV(username);

                        Menu();
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (won.Length / 2)) + "}", won));
                        Console.Read();
                    }
                    Print("Click enter to exit");
                    Console.ReadLine();
                    Exit(currentPlayer.getUsername(), characterClass.currentChar.ToString(), currentPlayer.getScore());

                }
                #endregion



            }
        }
        #endregion

        #region Inventory System
        private void Inventory(int usersScore)
        {


            Console.WriteLine("\n\nAdd\nView Items -- outputs all the items in your inventory\n" +
            "Remove -- removes specified item from your inventory\n");

            //splitting the users input into an array then checking the index 0 of that array to see if the user wants to add, view or remove from their inventory
            string options = "Do you want to Add, View Items or remove [item] your inventory\n";
            string[] Choices = { "Add Item", "View Items", "Remove Item" };
            KeyboardControls keyboardControls = new KeyboardControls(options, Choices);
            int selected = keyboardControls.Run();

           
            if (selected == 0)
            {
                //try catch stops the program from crashing if the user trys to enter in an item that is not in the room
                try
                {
                    //checking to see if item is already in the inventory
                    List<string> possibleInvenItems = new List<string>(WhichClass("", usersScore, false));
                        

                    int count = 0;
                    string[] optionsArr = new string[possibleInvenItems.Count];
                    int indexRef = -1;
                    options = "Click enter on the items you want to add to your inventory";
                    List<int> duplicates = new List<int>();

                    foreach (string words in possibleInvenItems)
                    {
                        if (invenItems.Contains(words))
                        {

                            optionsArr[count] = words + "[Equipped]";
                            indexRef = count;
                            duplicates.Add(count);

                        }
                        else
                        {

                            optionsArr[count] = words;
                        }
                        count++;

                    }

                    
                    KeyboardControls keyboardControls2 = new KeyboardControls(options, optionsArr);
                    selected = keyboardControls2.Run();
               
                    //checking if list is full
                    if (arrTop >= 4)
                    {
                        Console.WriteLine("Your inventory is full.");
                        Thread.Sleep(1500);


                    }
                    else
                    {
                       
                        //checkig if the item selected is not already in the users inventory
                        if (!invenItems.Contains(possibleInvenItems[selected]))
                        {
                            arrTop++;
                            invenItems.Add(optionsArr[selected]);
                        }
                        else
                        {
                            Console.WriteLine("You have already equipped that item");
                            Thread.Sleep(2000);
                        }


                    }

                       
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    Thread.Sleep(3000);
                    Console.Clear();

                }
            }
            //items in the inventory is outputted
            else if (selected == 1)
            {
                foreach (string item in invenItems)
                {
                    Console.WriteLine("item: " + item);
                }
                
            }
            //items are removed from the inventory
            else
            {
                options = "\nChoose which item to remove";
                if(invenItems.Count > 0)
                {
                    KeyboardControls removeItems = new KeyboardControls(options, invenItems.ToArray());
                    selected = removeItems.Run();
                    invenItems.Remove(invenItems[selected]);


                }
                else
                {
                    Console.WriteLine("Your inventory is empty");
                    Thread.Sleep(2000);
                }




            }
           

            


        }




        #endregion
        #region LookAndExamine
        //this checks if the user just wants the items of the current dungeon room to be outptted or a more detailed description of a particular item in the current room

        private void LookAndExamine(string lookOrExamine, int userScore)
        {
            while (true)
            {
                if (lookOrExamine == "look")
                {
                    WhichClass(lookOrExamine, userScore);
                    break;
                }
                else if (lookOrExamine == "examine")
                {
                    WhichClass(lookOrExamine, userScore);
                    break;
                }
                else
                {
                    Print("Do you want to look or examine");
                    lookOrExamine = Console.ReadLine().ToLower();
                }
            }
            dungeons.ItemsInRoom("view");
        }
        #endregion
        
        #region Which Class
        //checks which dungeon the user is currently in based on their score and fetches the item(s) from that partuclar dungeon class

        private List<string> WhichClass(string option, int userScore, bool look = true)
        {
            if (look)
            {
                if (userScore < 2)
                {
                    dungeons.ItemsInRoom(option);
                }
                else if (userScore == 2)
                {
                    Dun2 dun2 = new Dun2();
                    dun2.ItemsInRoom(option);
                }
                else if (userScore == 3)
                {
                    Dun3 dun3 = new Dun3();
                    dun3.ItemsInRoom(option);
                }
                else if (userScore == 4)
                {
                    Dun4 dun4 = new Dun4();
                    dun4.ItemsInRoom(option);

                }
                else if (userScore == 5)
                {
                    Dun5 dun5 = new Dun5();
                    dun5.ItemsInRoom(option);

                }
                else
                {
                    Console.WriteLine("\nYou have already completed the game");
                }
            }
            else
            {
                if (userScore < 2)
                {
                   return dungeons.returnDictionary();
                    
                }
                else if (userScore == 2)
                {
                    Dun2 dun2 = new Dun2();
                    return dun2.returnDictionary();
                }
                else if (userScore == 3)
                {
                    Dun3 dun3 = new Dun3();
                    return dun3.returnDictionary();
                }
                else if (userScore == 4)
                {
                    Dun4 dun4 = new Dun4();
                    return dun4.returnDictionary();

                }
                else if (userScore == 5)
                {
                    Dun5 dun5 = new Dun5();
                    return dun5.returnDictionary();

                }
                else
                {
                    Console.WriteLine("\nYou have already completed the game");
                }

            }
            List<string> list = new List<string>();
            return list;
            

        }
        #endregion

        

        #region Input checkpoint
        //this us a checkpoint for the user input. It returns false if it "input" is not one of case options below
        private  bool Input(string input, int usersScore, string username, bool answerToQuestion = false)
        {
            if (answerToQuestion)
            {
                switch (input)
                {
                    case "help":
                    case "h":
                        Help();
                        break;

                    case "inventory":
                    case "i":
                        
                        Inventory(usersScore);

                        break;
                    case "look":
                    case "l":
                        LookAndExamine("look", usersScore);
                       
                        break;
                    case "examine":
                    case "e":
                        LookAndExamine("examine", usersScore);
                        
                        break;
                    case "exit":
                        Exit(username, characterClass.currentChar.ToString(), usersScore);
                        break;
                    default:
                        return false;
                        
                }
            }
            else
            {
                switch (input)
                {
                    case "help":
                    case "h":
                        Help();
                        break;

                    case "inventory":
                    case "i":
                        Inventory(usersScore);
                        break;
                    case "look":
                    case "l":
                        LookAndExamine("look", usersScore);
                        break;
                    case "examine":
                    case "e":
                        LookAndExamine("examine", usersScore);
                        break;

                    case "exit":
                        Thread.Sleep(20000);

                        Exit(username, characterClass.currentChar.ToString(), usersScore);
                        break;
                    default:
                        string answer = Console.ReadLine();
                        Console.WriteLine("Incorrect Input");
                        Thread.Sleep(2500);
                        Console.Clear();
                        return false;

                }
            }
           
            return false;
        }
        #endregion

       
        //leaderboard
        #region LEADER BOARD
        private  void LeaderBoard()
        {
            Search_and_Sort<int> intSearchAndSort = new Search_and_Sort<int>();
            FileHandling currentFileHandling = new FileHandling();
            Console.Clear();
            Dictionary<string, int> usernameAndScores = new Dictionary<string, int>();

            const string filepath = "usersDetails.csv";

            currentFileHandling.returningFields(filepath, 1, false);
            List<string> usernameList = new List<string>();
            usernameList = currentFileHandling.usernameList;




            List<int> orderdLeaderboard = new List<int>();
            string usersScores;
            Console.WriteLine("\n");
            string leaderboardHeader = "LEADERBOARD";
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (leaderboardHeader.Length / 2)) + "}", leaderboardHeader));
            Console.Write("\n");

            currentFileHandling.returningFields(filepath, 5, true);
            for (int i = 0; i < usernameList.Count; i++)
            {
                if (usernameAndScores.ContainsKey(usernameList[i]))
                {
                    usernameAndScores.Add(usernameList[i], currentFileHandling.scoreList[i]);

                }
            }

            foreach (KeyValuePair<string, int> ff in usernameAndScores)
            {
                Console.WriteLine("Key: {0}, Value: {1}", ff.Key, ff.Value);
            }



            //ordering it with a merge sort 
            foreach (int items in intSearchAndSort.MergeSort(currentFileHandling.scoreList.ToArray()))
            {

                orderdLeaderboard.Add(items);
            }


            //reading the scores of the merge sort
            for (int j = orderdLeaderboard.Count - 1; j >= 0; j--)
            {

                currentFileHandling.readRecord(orderdLeaderboard[j].ToString(), filepath, 5);
                usersScores = string.Join(" ", currentFileHandling.leaderboard);
                Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (usersScores.Length / 2)) + "}", usersScores));
            }
            Print("\n\n\n Type back to go back to go to main menu");
            string keyboardControls = Console.ReadLine().ToLower();
            if (keyboardControls == "back")
            {
                Menu();
            }
            else
            {
                while (keyboardControls != "back")
                {
                    Print("\n Type back to go back to the menu ");
                    keyboardControls = Console.ReadLine().ToLower();
                    if (keyboardControls == "back")
                    {
                        Menu();
                    }
                }
            }

        }
        #endregion
        // inventory. Its where the user adds, removes or views items in their inventory  
       
        #region exit
        //saves all the users info and exits the game
        private int NextLevel(string username, string filepath, int userScore )
        {
            Print("Correct\n The door flings open.");
            Print("The wooden door swings open");
            if (characterClass.currentChar == CharacterClass.CharacterTypes.Mage)
            {
                currentFileHandling.editRecord(username, filepath, 1, username.ToString(), characterClass.currentChar.ToString(), userScore.ToString(), 4.ToString());

            }
            else
            {
                currentFileHandling.editRecord(username, filepath, 1, username, characterClass.currentChar.ToString(), userScore.ToString(), 3.ToString());
            }
            string[] tempInven = new string[1000];

            for (int i = 0; i < invenItems.Count; i++)
            {
                tempInven[i] = invenItems[i];
            }
            writeToCSV(username, tempInven[0], tempInven[1], tempInven[2], tempInven[3], tempInven[4], "inventory.csv");
            return 1;
        }
        private  void Exit(string username, string currentChar, int score)
        {
            FileHandling currentFileHandling = new FileHandling();
            Console.Clear();
            Console.WriteLine("Exiting....");
            const string filepath = "usersDetails.csv";
        
            if (currentChar == "Mage")
            {

                currentFileHandling.editRecord(username, filepath, 1, username, currentChar.ToString(), score.ToString(), 4.ToString());


            }
            else
            {
                currentFileHandling.editRecord(username, filepath, 1, username, currentChar.ToString(), score.ToString(), 3.ToString());


            }
            string[] tempInven = new string[1000];

            for (int i = 0; i < invenItems.Count; i++)
            {
                tempInven[i] = invenItems[i];
            }
          

            writeToCSV(username, tempInven[0], tempInven[1], tempInven[2], tempInven[3], tempInven[4], "inventory.csv");
            Thread.Sleep(1500);

            Environment.Exit(0);
        }
        #endregion
        #region Print
        // checkpoint for all outputs. This is where colours of the text is assigned and its speed of which the program outputs, giving it a typing effect
        public  void Print(string text, ConsoleColor color = ConsoleColor.White, bool newLine = true, int speed = 10)
        {


            foreach (char letter in text)
            {
                if (letter == '>')
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;

                    Console.Write(letter);
                    Thread.Sleep(speed);

                }
                else
                {

                    Console.ForegroundColor = color;
                    Console.Write(letter);
                    Thread.Sleep(speed);
                }


            }
            //bumps it down to a new line
            if (newLine)
            {
                Console.WriteLine();

            }
            else
            {
                Console.Write(" ");

            }
        }
        #endregion
        #region User character types
        //this is where the user sees its options for its character type and sets the users character.
        public  int CharecterType()
        {
            Console.Clear();
           


            string options = "Choose from the options below\n";
            string[] optionsArr = { "Mage", "Monk", "Warroir" };
            KeyboardControls keyboardControls = new KeyboardControls(options, optionsArr);
            

            return keyboardControls.Run();

        }
        #endregion

        #region CharacterType Effects in work
        private  double SpecailEffects(string character, double health)
        {
            
          
            //this is where the users special skill for their different characters activated
            if (character == CharacterClass.CharacterTypes.Warrior.ToString())
            {
                Warroir warroir = new Warroir(3);
                warroir.setHealth(health);
                warroir.Perks();
                return warroir.getHealth();
            }
            else if (character == CharacterClass.CharacterTypes.Monk.ToString())
            {
                Monk monk = new Monk(3);
                monk.setHealth(health);
                monk.Perks();
                return monk.getHealth();
            }
            else
            {
                Mage mage = new Mage(4);
                mage.setHealth(health);
                mage.Perks();
                return mage.getHealth();
            }
            
        }
        #endregion
        //this is the method that asks the parses the users friend into the player class.
        #region Add friends
        //users frends is passed to the freinds function in the player class
        private  void AddFreinds(double health, string username, int score, string password)
        {
            FriendsXml friendsXml = new FriendsXml(health, username, score, password );
            friendsXml.ReadXMLRecord();

            string option = "Choose from the options below";
            string[] optionsArr = { "View Friends", "Add a Friend", "Message a Friend", "Back"};
            KeyboardControls keyboardControls = new KeyboardControls(option, optionsArr);
            int selected = keyboardControls.Run();

            if(selected == 0)
            {
                friendsXml.Friends("view", username);

            }
            else if(selected == 1)
            {
                friendsXml.Friends("add", username);

            }
            else if(selected == 2)
            {
                friendsXml.Message();

            }
            else
            {
                Console.Clear();
                Menu();
            }
            
     

        }
        #endregion
        //the help instructions
        #region Help 
        private void Help()
        {
            Console.Clear();
            //rules
            Print("\n\nType 'look' or 'l' for the items in the room to be outputted\nType 'inventory' or 'i' to check the items in your inventory or add to it. You only have 5 spaces for your inventory.\nType 'examine' or 'e' to examine and item\nWhen you see this icon (>) then thats when you type. Type exit to exit the game\n\n");

            while (true)
            {
                Print("Type back or the letter 'b' to go back to the menu.");

                string start = Console.ReadLine().ToLower();
                if (start == "back" || start == "b")
                {
                    Menu();
               
                }
               
                Thread.Sleep(1500);
                Console.Clear();

            }
        }
        #endregion

        #region Saving to csv file
        //writing and editing  csv file
        private void writeToCSV(string username, string item1, string item2, string item3, string item4, string item5, string filepath)
        {
            FileHandling currentFileHandling = new FileHandling();

            filepath = "inventory.csv";
            currentFileHandling.readRecord(username, filepath, 1);
            usernameConnection = currentFileHandling.getUsernameCheckPoint();
            //if the username already exits it edits instead of writing a new line in the csv file
            if (usernameConnection == username)
            {
                EditCSV(username, item1, item2, item3, item4, item5, filepath);
            }
            //writes a new line in the csv file
            else
            {
                currentFileHandling.addRecord(username, item1, item2, item3, item4, item5, filepath);
            }
        }
        private  void EditCSV(string username, string item1, string item2, string item3, string item4, string item5, string filepath)
        {
            FileHandling currentFileHandling = new FileHandling();
            currentFileHandling.editRecord(username, filepath, 1, username, item1, item2, item3, item4, item5);



        }

        #endregion
        #region password 
         private string ReplacePasswordLetters(string enterPassword)
         {
            try
            {
                string input = "";
                Console.Write(enterPassword);
                do
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    // If backspace and the enter button is not entered 
                    if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                    {

                        input += key.KeyChar;
                        Console.Write("*");
                    }
                    else
                    {
                        //if their is backspace
                        if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                        {
                            input = input.Substring(0, (input.Length - 1));
                            Console.Write("\b \b");
                        }
                        else if (key.Key == ConsoleKey.Enter)
                        {
                            //checking if the input is null of just got spaces
                            if (string.IsNullOrWhiteSpace(input))
                            {
                                Console.WriteLine("");
                                Console.WriteLine("You cannot enter nothing.");
                                ReplacePasswordLetters(enterPassword);
                                break;
                            }
                            else
                            {
                                Console.WriteLine("");
                                break;
                            }
                        }
                    }
                } while (true);
                return input;

            }
            catch (Exception ex)
            {
               
                Console.WriteLine( ex.Message);
                return "?";
            }
        }


        #endregion
        #region returning Play method for classes
        public void returnPlayMethod() => Play();
        #endregion
        #region Returning Menu For classes

        public void returnMenu(string username) => Menu(0, username);
   
        #endregion
        #region return Users
        public string getUsername() => usernameConnection;

        #endregion

      


    }
}
