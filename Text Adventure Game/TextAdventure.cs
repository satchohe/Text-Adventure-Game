using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Text_Adventure_Game
{
    internal class TextAdventure
    {

        #region password 
        static string checkPassword(string enterPassword)
        {
            try
            {
                string input = "";
                Console.Write(enterPassword);
                do
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    // If backspace is not entered  
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
                            if (string.IsNullOrWhiteSpace(input))
                            {
                                Console.WriteLine("");
                                Console.WriteLine("Cannot enter nothing.");
                                checkPassword(enterPassword);
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
                throw ex;
            }
        }


        #endregion

        #region Login
        //login and sign in.

        private static void Login()
        {

            FileHandling currentLoginFileHandling = new FileHandling();
            FileHandling currentFileHandling = new FileHandling();
            Stack currentStack = new Stack(6);


            int usersScore;
            int usersuserId;
            double usersHealth;
            const string filepath = "login.csv";
            string options = "Would you like to login [L] or Register[R]";
            bool check = false;
            string usernameConnection = "";
            List<string> invenItems = new List<string>();
            do
            {

                Console.WriteLine(options);
                string logOrSign = Console.ReadLine().ToLower();

                //login procedure. The user enters the username and it writes it to csv file.
                if (logOrSign == "login" || logOrSign == "l")
                {
                    Console.WriteLine("Enter in your username:", ConsoleColor.White, false);
                    string username = Console.ReadLine();

                    string repeat = "Enter in the password: ";
                    string password = checkPassword(repeat);
                    // the entered username and password along with the file path is passed to the method that cross checks against the "login.csv" file to check if the details enters matches. A true or false is returned.
                    bool successful = currentLoginFileHandling.VerifyLogin(username, password, filepath);
                    while (successful == false)
                    {
                        Console.WriteLine("Your username or password is incorrect. Try again or type Back[B] to start again\n", ConsoleColor.White, false);
                        Console.WriteLine("Enter in your username:", ConsoleColor.White, false);
                        username = Console.ReadLine();
                        if (username == "B" || username == "b")
                        {
                            Login();
                        }
                        else
                        {
                            Console.WriteLine("Enter in your password:", ConsoleColor.White, false);

                            password = Console.ReadLine();
                            if (password == "B" || password == "b")
                            {
                                Login();
                            }
                            else
                            {
                                successful = currentLoginFileHandling.VerifyLogin(username, password, filepath);

                            }
                        }



                    }
                    //adding current user info to the player class




                    //userId of the user is stored in the userId player class
                    currentLoginFileHandling.OutputField(username, filepath, 1, 3);

                    usersuserId = int.Parse(currentLoginFileHandling.fieldWant);

                    //health of the user is stored in the health player class
                    currentLoginFileHandling.OutputField(username, filepath, 1, 6);
                    usersHealth = double.Parse(currentLoginFileHandling.fieldWant);

                    //score of the user is stored in the score player class
                    currentLoginFileHandling.OutputField(username, filepath, 1, 5);

                    usersScore = int.Parse(currentLoginFileHandling.fieldWant);
                    Player currentPlayer = new Player(usersHealth, username, usersuserId, usersScore, password);



                    //adding the users class.
                    currentLoginFileHandling.OutputField(username, filepath, 1, 4);

                    if (currentLoginFileHandling.fieldWant == "Monk")
                    {
                        currentPlayer.currentClass = Player.PlayerClass.Monk;
                    }
                    else if (currentFileHandling.fieldWant == "Mage")
                    {
                        currentPlayer.currentClass = Player.PlayerClass.Mage;
                    }
                    else if (currentFileHandling.fieldWant == "Warrior")
                    {
                        currentPlayer.currentClass = Player.PlayerClass.Warrior;

                    }
                    const string invenFilepath = "inventory.csv";

                    //reading the inventory record
                    currentFileHandling.readRecord(currentPlayer.getUsername(), invenFilepath, 1);
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
                    currentFileHandling.readRecord(currentPlayer.getUsername(), "questions.csv", 1);


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

                    if (emptyStackIndexcount > 0)
                    {
                        currentStack.top = 4 - (emptyStackIndexcount);

                    }

                    else
                    {
                        currentStack.top = -1;
                    }

                    currentStack.top = emptyStackIndexcount - 2;

                    //adding all the items in the freinds.xml file to the graph
                    currentPlayer.ReadXMLRecord();
                    Play();

                    #endregion

                    check = true;

                }
                else if (logOrSign == "register" || logOrSign == "r")
                {

                    Console.WriteLine("Enter in a username:", ConsoleColor.White, false);
                    string username = Console.ReadLine();
                    while (username == "" || username.Contains(","))
                    {

                        Console.WriteLine("please enter in a correct format. Meaning no commas.\nEnter in a valuserId username:");
                        username = Console.ReadLine();
                    }

                    string repeat = "Enter in the password: ";
                    string password = checkPassword(repeat);
                    string repeat2 = "Enter your password again: ";
                    string passwordRepeate = checkPassword(repeat2);

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

                    currentLoginFileHandling.readRecord(username, filepath, 1);

                    while (currentLoginFileHandling.found == true)
                    {
                        Console.WriteLine("Enter another username, this one is taken");
                        username = Console.ReadLine();

                        currentLoginFileHandling.readRecord(username, filepath, 1);
                    }

                    #region Creating user userId 
                    //creating game userId
                    Random rand = new Random();
                    int userId = rand.Next(0, 10000);
                    Console.WriteLine(currentLoginFileHandling.readRecord(userId.ToString(), filepath, 3));
                    //this creates a new userId if it matches one on the file already.
                    Player currentPlayer = new Player(3,username, userId,0,password);

                    while (currentLoginFileHandling.found == true)
                    {
                        Console.WriteLine(currentLoginFileHandling.readRecord(userId.ToString(), filepath, 3));

                    }
                    //adding details to player class

                    while (true)
                    {
                        
                        int input = int.Parse(Console.ReadLine());
                        if ( CharecterType() == 1)
                        {
                            currentPlayer.currentClass = Player.PlayerClass.Mage;
                            currentFileHandling.addRecord(currentPlayer.getUsername(), currentPlayer.getPassword(), currentPlayer.getId().ToString(), currentPlayer.currentClass.ToString(), 0.ToString(), 4.ToString(), filepath);
                            currentPlayer.setUsername(username);
                            currentPlayer.setPassword(password);
                            currentPlayer.setId(userId);
                            currentPlayer.setHealth(4);
                            currentPlayer.setScore(0);
                            Play();



                        }
                        else if (CharecterType() == 2)
                        {
                            currentPlayer.currentClass = Player.PlayerClass.Monk;
                            currentFileHandling.addRecord(username, password, userId.ToString(), currentPlayer.currentClass.ToString(), 0.ToString(), 3.ToString(), filepath);
                            currentPlayer.setUsername(username);
                            currentPlayer.setPassword(password);
                            currentPlayer.setId(userId);
                            currentPlayer.setHealth(3);
                            currentPlayer.setScore(0);
                            Play();



                        }
                        else if (CharecterType() == 3)
                        {
                            currentPlayer.currentClass = Player.PlayerClass.Warrior;
                            currentFileHandling.addRecord(username, password, userId.ToString(), currentPlayer.currentClass.ToString(), 0.ToString(), 3.ToString(), filepath);
                            currentPlayer.setUsername(username);
                            currentPlayer.setPassword(password);
                            currentPlayer.setId(userId);
                            currentPlayer.setHealth(3);
                            currentPlayer.setScore(0);
                            Play();

                        }
                        else
                        {
                            Console.WriteLine("Please choose an existing charater");
                            CharecterType();
                            
                        }

                    }
                    check = true;
                    #endregion

                }

            } while (check == false);

        }

        #endregion
        #region The game
        //the game itself.
        private static void Play()
        {
            const string filepath = "login.csv";
            bool nextLevel = false;
            bool level1 = false;
            while (nextLevel == false)
            {
                //The introduction to the game
                #region players score is 0
                if (current == 0)
                {
                    #region Outputting the introduction of the game
                    //exception checks if the user has entered in a value in except for a double
                    try
                    {
                        List<double> fractionList = new List<double>();
                        fractionList.Add(0.33);
                        fractionList.Add(0.40);
                        fractionList.Add(0.50);
                        fractionList.Add(0.75);
                        List<double> userFractionList = new List<double>();

                        string question = "write the fractions in decimals to 2 decimal place from smallest to largest. 3/4 --  6/12 -- 1/3 and 2/5";
                        Print($"LOCATION: {dungeons.location}\n", ConsoleColor.Magenta);
                        Print("You slowly open your eyes and find yourself lying on a cold moist  surface. A sudden wave of nausea hits you, forcing" +
                            "you to flip over and vomit on the floor.You hold your head feeling dizzy. You look down and find that you were asleep on a stone wall." +
                            "Confused, you look around, scanning the surrounding areas. As you stand up, you see a half apron around your waist and inside you can" +
                            "feel some things.\nYou can't even remember your own name.\nThere is writing on the wall and books. One of the books are open and a question is written " + question, ConsoleColor.White, true, 0);
                        //looping till the answer is right or users life is 0
                        while (level1 == false)
                        {
                            userFractionList.Clear();

                            for (int i = 0; i < 4; i++)
                            {
                                Print(i + ". Enter in the decimal");
                                double fraction = double.Parse(Console.ReadLine());
                                if (fraction == 0.40)
                                {
                                    userFractionList.Add(0.4);

                                }
                                else if (fraction == 0.50)
                                {
                                    userFractionList.Add(0.5);

                                }
                                else
                                {
                                    userFractionList.Add(fraction);

                                }

                            }
                            doubleSearchAndSort.itemList = fractionList;
                            int correctCount = 0;
                            foreach (double fraction in userFractionList)
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
                                Thread.Sleep(1500);
                                currentStack.Pushy(question);
                                currentPlayer.score += 1;
                                if (currentPlayer.currentClass == Player.PlayerClass.Mage)
                                {
                                    currentFileHandling.editRecord(currentPlayer.username, filepath, 1, currentPlayer.username, currentPlayer.password, currentPlayer.id.ToString(), currentPlayer.currentClass.ToString(), currentPlayer.score.ToString(), 4.ToString());

                                }
                                else
                                {
                                    currentFileHandling.editRecord(currentPlayer.username, filepath, 1, currentPlayer.username, currentPlayer.password, currentPlayer.id.ToString(), currentPlayer.currentClass.ToString(), currentPlayer.score.ToString(), 3.ToString());
                                }
                                Print(invenItems.Count.ToString(), ConsoleColor.DarkRed);
                                //writng items to csv files
                                string[] tempInven = new string[100];
                                for (int i = 0; i < invenItems.Count; i++)
                                {
                                    tempInven[i] = invenItems[i];
                                }
                                writeToCSV(currentPlayer.username, tempInven[0], tempInven[1], tempInven[2], tempInven[3], tempInven[4], "inventory.csv");
                                currentStack.stackWriteToCSV();
                                level1 = true;
                            }
                            else
                            {
                                SpecailEffects(currentPlayer.currentClass.ToString());

                                Print("Incorrect. A book hits your head. You loose a life. Lives: " + currentPlayer.health);
                            }

                            if (currentPlayer.health <= 1)
                            {
                                Print("You have no more lives", ConsoleColor.DarkRed);

                                Thread.Sleep(2000);
                                SpecailEffects(currentPlayer.currentClass.ToString());
                                Exit();
                            }
                        }

                    }
                    catch
                    {

                        Console.WriteLine("Incorrect input. Try again");


                    }


                    #endregion
                }
                #endregion

                #region PLayer score is 1
                else if (currentPlayer.score == 1)
                {
                    Print("\nAcross the room their is a board locked behind a glass case.\nYou see some writing but you cant really make it out through the thick glass.");
                    const string question = "What number goes up and doesn't come down";

                    string answer;
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
                            Print("\n> ");
                            answer = Console.ReadLine().ToLower();
                            //looping till the answer is right or user life is 0
                            while (answer != "age")
                            {//checking the user's input to see what the required output should be
                                switch (answer)
                                {
                                    case "help":
                                    case "h":
                                        Help();
                                        break;

                                    case "inventory":
                                    case "i":

                                        Inventory();

                                        break;
                                    case "look":
                                    case "l":
                                        LookAndExamine("look");
                                        Print("\nEnter in your answer\n> ");
                                        answer = Console.ReadLine().ToLower();
                                        break;
                                    case "examine":
                                    case "e":
                                        LookAndExamine("examine");
                                        Print("\nEnter in your answer\n> ");
                                        answer = Console.ReadLine().ToLower();
                                        break;
                                    case "exit":
                                        Exit();
                                        break;
                                    default:
                                        SpecailEffects(currentPlayer.currentClass.ToString());
                                        Print("\nA hole suddenly appears from the cabinet and poison spews onto you. You skin starts to burn as you have " + currentPlayer.health + " health left\nAnswer again\n> ", ConsoleColor.Red);//minus health

                                        answer = Console.ReadLine().ToLower();
                                        break;
                                }
                                //what is done if the user has no more lives
                                if (currentPlayer.health <= 1)
                                {
                                    Print("You have no more lives", ConsoleColor.DarkRed);

                                    SpecailEffects(currentPlayer.currentClass.ToString());
                                    Exit();
                                }

                            }
                            //saving the users current data if they win and writing them to file.
                            if (answer == "age")
                            {
                                currentStack.Pushy(question);
                                currentPlayer.score += 1;
                                Print("The wooden door swings open", ConsoleColor.Green);
                                if (currentPlayer.currentClass == Player.PlayerClass.Mage)
                                {
                                    currentFileHandling.editRecord(currentPlayer.username, filepath, 1, currentPlayer.username, currentPlayer.password, currentPlayer.id.ToString(), currentPlayer.currentClass.ToString(), currentPlayer.score.ToString(), 4.ToString());

                                }
                                else
                                {
                                    currentFileHandling.editRecord(currentPlayer.username, filepath, 1, currentPlayer.username, currentPlayer.password, currentPlayer.id.ToString(), currentPlayer.currentClass.ToString(), currentPlayer.score.ToString(), 3.ToString());
                                }
                                //writng items to csv files
                                string[] tempInven = new string[100];
                                for (int i = 0; i < invenItems.Count; i++)
                                {
                                    tempInven[i] = invenItems[i];
                                }
                                writeToCSV(currentPlayer.username, tempInven[0], tempInven[1], tempInven[2], tempInven[3], tempInven[4], "inventory.csv");
                                currentStack.stackWriteToCSV();

                            }
                        }
                        else
                        {

                            Print("\nYou dont have the means to open the case\n", ConsoleColor.DarkMagenta);
                            Print("> ");
                            Input(Console.ReadLine().ToLower());
                        }
                    }
                    else
                    {

                        Print("\nYou dont have the means to open the case\n", ConsoleColor.DarkMagenta);
                        Print("> ");
                        Input(Console.ReadLine().ToLower());
                    }




                }
                #endregion
                #region Players score is 2
                else if (currentPlayer.score == 2)
                {

                    Dun2 dun2 = new Dun2();
                    Print("\n" + dun2.getLocation() + "\n\n\n", ConsoleColor.Blue);
                    Print("As soon as you walk in you are met with a blast of cold air. You seee the cause of this cold air, an open window but this one is unusal. It's mechanical in a way that it winds to shut but the " +
                        "turning device is not attached to it.On the wall hangs a baord showing the temperature slowly decreassing. You have to close the window before you freeze.");

                    const string question = "I am an odd number; take away an alphabet and I become even. What number am I?";
                    string answer;
                    // adding the users inventory item to the stringsearchandSort class so that any type of search is possible
                    stringSearchAndSort.itemList = invenItems;
                    stringSearchAndSort.find = "turning-mechanism";
                    //checking if the inventory is empty. This is so that the linear search below does not throw an error
                    if (invenItems.Count > 0)
                    {//searching the users inventory to see if they have the necceasry items to advace.
                        if (stringSearchAndSort.LinearSearch())
                        {
                            Print("You go to attach the device onto the window and shut it. Over time you feel the air starting to warm up. A brick from the wall pops out and pen and paper comes out from it. On the paper it says.\n" + question + "\n");
                            Print("\n> ");
                            answer = Console.ReadLine().ToLower();
                            while (answer != "seven" && answer != "7")
                            {

                                switch (answer)
                                {
                                    case "help":
                                    case "h":
                                        Help();
                                        break;

                                    case "inventory":
                                    case "i":

                                        Inventory();

                                        break;
                                    case "look":
                                    case "l":
                                        LookAndExamine("look");
                                        Print("\nEnter in your answer\n> ");
                                        answer = Console.ReadLine().ToLower();
                                        break;
                                    case "examine":
                                    case "e":
                                        LookAndExamine("examine");
                                        Print("\nEnter in your answer\n> ");
                                        answer = Console.ReadLine().ToLower();
                                        break;
                                    case "exit":
                                        Exit();
                                        break;
                                    default:
                                        SpecailEffects(currentPlayer.currentClass.ToString());
                                        Print("Incorrect! The temperature suddenly droops. You now have " + currentPlayer.health + "lives left\nAnswer again\n", ConsoleColor.Red);
                                        answer = Console.ReadLine().ToLower();
                                        break;
                                }
                                if (currentPlayer.health <= 1)
                                {
                                    Print("You have no more lives. You have died from hyperthermia", ConsoleColor.DarkRed);
                                    SpecailEffects(currentPlayer.currentClass.ToString());
                                    Exit();
                                }
                            }
                            //saving the users deatils and writing them to file
                            if (answer == "seven" || answer == "7")
                            {
                                Print("Correct\n The door flings open.");
                                currentPlayer.score += 1;
                                currentStack.Pushy(question);
                                Print("The wooden door swings open");
                                if (currentPlayer.currentClass == Player.PlayerClass.Mage)
                                {
                                    currentFileHandling.editRecord(currentPlayer.username, filepath, 1, currentPlayer.username, currentPlayer.password, currentPlayer.id.ToString(), currentPlayer.currentClass.ToString(), currentPlayer.score.ToString(), 4.ToString());

                                }
                                else
                                {
                                    currentFileHandling.editRecord(currentPlayer.username, filepath, 1, currentPlayer.username, currentPlayer.password, currentPlayer.id.ToString(), currentPlayer.currentClass.ToString(), currentPlayer.score.ToString(), 3.ToString());
                                }
                                string[] tempInven = new string[1000];

                                for (int i = 0; i < invenItems.Count; i++)
                                {
                                    tempInven[i] = invenItems[i];
                                }
                                writeToCSV(currentPlayer.username, tempInven[0], tempInven[1], tempInven[2], tempInven[3], tempInven[4], "inventory.csv");
                                currentStack.stackWriteToCSV();

                            }

                        }
                        //this is what is outputted if the user does not have the neccesary item in the inventory.
                        else
                        {
                            Print("\nYou dont have the means to close the window\n", ConsoleColor.DarkMagenta);
                            Print("> ");
                            Input(Console.ReadLine().ToLower());
                        }
                    }
                    //this is if the users invetory is empty
                    else
                    {
                        Print("\nYou dont have the means to close the window\n", ConsoleColor.DarkMagenta);
                        Print("> ");
                        Input(Console.ReadLine().ToLower());
                    }


                    // Console.Clear();

                }
                #endregion
                //third dungeon
                #region Players score is 3
                else if (currentPlayer.score == 3)
                {
                    Dun3 dun3 = new Dun3();
                    Print("\n" + dun3.getLocation() + "\n", ConsoleColor.Green);
                    Print("");

                    const string question = "There are several books on a bookshelf. How many books are on the shelf if one book is the 4th from the left and the 6th from the right?";
                    string answer;
                    Print("You walk through and instantly feel warmer. The door shuts close behind you. You look down to find yourself standing on grass and moss. This room looks is a forest with roaming animals.  Naield to a tree their is a board with buttons to type on. It says:\n" + question);
                    Print("\n> ");
                    answer = Console.ReadLine().ToLower();
                    while (answer != "nine" && answer != "9")
                    {
                        switch (answer)
                        {
                            case "help":
                            case "h":
                                Help();
                                break;

                            case "inventory":
                            case "i":

                                Inventory();

                                break;
                            case "look":
                            case "l":
                                LookAndExamine("look");
                                Print("\nEnter in your answer\n> ");
                                answer = Console.ReadLine().ToLower();
                                break;
                            case "examine":
                            case "e":
                                LookAndExamine("examine");
                                Print("\nEnter in your answer\n> ");
                                answer = Console.ReadLine().ToLower();
                                break;
                            case "exit":
                                Exit();
                                break;
                            default:
                                SpecailEffects(currentPlayer.currentClass.ToString());
                                Print("Incorrect! One of the nearby animals comes to attack you. You now have " + currentPlayer.health + "lives left\nAnswer again\n", ConsoleColor.Red);
                                answer = Console.ReadLine().ToLower();
                                break;
                        }
                        if (currentPlayer.health <= 1)
                        {
                            Print("You have no more lives.", ConsoleColor.DarkRed);
                            SpecailEffects(currentPlayer.currentClass.ToString());
                            Exit();
                        }
                    }
                    if (answer == "nine" || answer == "9")
                    {
                        Print("Correct\n The door flings open.", ConsoleColor.Green);
                        currentPlayer.score += 1;
                        currentStack.Pushy(question);
                        Print("The wooden door swings open", ConsoleColor.Green);
                        if (currentPlayer.currentClass == Player.PlayerClass.Mage)
                        {
                            currentFileHandling.editRecord(currentPlayer.username, filepath, 1, currentPlayer.username, currentPlayer.password, currentPlayer.id.ToString(), currentPlayer.currentClass.ToString(), currentPlayer.score.ToString(), 4.ToString());

                        }
                        else
                        {
                            currentFileHandling.editRecord(currentPlayer.username, filepath, 1, currentPlayer.username, currentPlayer.password, currentPlayer.id.ToString(), currentPlayer.currentClass.ToString(), currentPlayer.score.ToString(), 3.ToString());
                        }
                        string[] tempInven = new string[1000];
                        for (int i = 0; i < invenItems.Count; i++)
                        {
                            tempInven[i] = invenItems[i];
                        }
                        writeToCSV(currentPlayer.username, tempInven[0], tempInven[1], tempInven[2], tempInven[3], tempInven[4], "inventory.csv");
                        currentStack.stackWriteToCSV();

                    }



                }
                #endregion
                //4th dungeon
                #region Dungeon 4
                else if (currentPlayer.score == 4)
                {
                    Dun4 dun4 = new Dun4();
                    Print("\n" + dun4.getLocation() + "\n", ConsoleColor.DarkGray);
                    Print("");

                    const string question = "I have a ring but no finger. What am I?";
                    string answer;
                    Print("You walk through and you are instantly met with darkness. The door shuts close behind you. A lightbulb switches on and you can see a little note on the floor. It says:\n " + question);
                    Print("\n> ");
                    answer = Console.ReadLine().ToLower();
                    while (answer != "telephone" && answer != "phone")
                    {
                        switch (answer)
                        {
                            case "help":
                            case "h":
                                Help();
                                break;

                            case "inventory":
                            case "i":

                                Inventory();

                                break;
                            case "look":
                            case "l":
                                LookAndExamine("look");
                                Print("\nEnter in your answer\n> ");
                                answer = Console.ReadLine().ToLower();
                                break;
                            case "examine":
                            case "e":
                                LookAndExamine("examine");
                                Print("\nEnter in your answer\n> ");
                                answer = Console.ReadLine().ToLower();
                                break;
                            case "exit":
                                Exit();
                                break;
                            default:
                                SpecailEffects(currentPlayer.currentClass.ToString());
                                Print("Incorrect! A bat from a nearby corner comes to attack you. You now have " + currentPlayer.health + "lives left\nAnswer again\n", ConsoleColor.Red);
                                answer = Console.ReadLine().ToLower();
                                break;
                        }
                        if (currentPlayer.health <= 1)
                        {
                            Print("You have no more lives.", ConsoleColor.DarkRed);
                            SpecailEffects(currentPlayer.currentClass.ToString());
                            Exit();
                        }
                    }
                    if (answer == "telephone" || answer == "phone")
                    {
                        Print("Correct\nThe door flings open.", ConsoleColor.Green);
                        currentPlayer.score += 1;
                        currentStack.Pushy(question);
                        if (currentPlayer.currentClass == Player.PlayerClass.Mage)
                        {
                            currentFileHandling.editRecord(currentPlayer.username, filepath, 1, currentPlayer.username, currentPlayer.password, currentPlayer.id.ToString(), currentPlayer.currentClass.ToString(), currentPlayer.score.ToString(), 4.ToString());

                        }
                        else
                        {
                            currentFileHandling.editRecord(currentPlayer.username, filepath, 1, currentPlayer.username, currentPlayer.password, currentPlayer.id.ToString(), currentPlayer.currentClass.ToString(), currentPlayer.score.ToString(), 3.ToString());
                        }
                        string[] tempInven = new string[1000];
                        for (int i = 0; i < invenItems.Count; i++)
                        {
                            tempInven[i] = invenItems[i];
                        }
                        writeToCSV(currentPlayer.username, tempInven[0], tempInven[1], tempInven[2], tempInven[3], tempInven[4], "inventory.csv");
                        currentStack.stackWriteToCSV();


                    }



                }
                #endregion
                //5th dungone -- final dungeon
                #region Dungeon 5 -- final dungeon
                else if (currentPlayer.score == 5)
                {
                    Dun5 dun5 = new Dun5();
                    Print("\n" + dun5.getLocation() + "\n", ConsoleColor.DarkYellow);
                    Print("");
                    const string question = "I have a face but no eyes and hands but no arms. What am I?";
                    string answer;
                    Print("You walk into a bright room. The door slowly shuts close behind you. Light is coming out from decorated window panes and the floor is filled with red carpet. Every object is gold plated. You go to the table in the middle of the room and on it their is a board that says:\nThis is the last level. " + question);
                    Print("\n> ");
                    answer = Console.ReadLine().ToLower();
                    while (answer != "clock")
                    {
                        switch (answer)
                        {
                            case "help":
                            case "h":
                                Help();
                                break;

                            case "inventory":
                            case "i":

                                Inventory();

                                break;
                            case "look":
                            case "l":
                                LookAndExamine("look");
                                Print("\nEnter in your answer\n> ");
                                answer = Console.ReadLine().ToLower();
                                break;
                            case "examine":
                            case "e":
                                LookAndExamine("examine");
                                Print("\nEnter in your answer\n> ");
                                answer = Console.ReadLine().ToLower();
                                break;
                            case "exit":
                                Exit();
                                break;
                            default:
                                SpecailEffects(currentPlayer.currentClass.ToString());
                                Print("Incorrect! An arrow is released from a cross bow hung on the wall " + currentPlayer.health + "lives left\nAnswer again\n", ConsoleColor.Red);
                                answer = Console.ReadLine().ToLower();
                                break;
                        }
                        if (currentPlayer.health <= 1)
                        {
                            Print("You have no more lives.", ConsoleColor.DarkRed);
                            SpecailEffects(currentPlayer.currentClass.ToString());
                            Exit();
                        }
                    }
                    if (answer == "clock")
                    {
                        foreach (string item in invenItems)
                        {
                            if (item == "chess")
                            {
                                Print("You have beaten the dungeon and have decided to take the treasure chest");
                            }
                        }
                        Print("Correct\nYou have won!", ConsoleColor.Green);

                        currentPlayer.score += 1;
                        currentStack.Pushy(question);
                        if (currentPlayer.currentClass == Player.PlayerClass.Mage)
                        {
                            currentFileHandling.editRecord(currentPlayer.username, filepath, 1, currentPlayer.username, currentPlayer.password, currentPlayer.id.ToString(), currentPlayer.currentClass.ToString(), currentPlayer.score.ToString(), 4.ToString());

                        }
                        else
                        {
                            currentFileHandling.editRecord(currentPlayer.username, filepath, 1, currentPlayer.username, currentPlayer.password, currentPlayer.id.ToString(), currentPlayer.currentClass.ToString(), currentPlayer.score.ToString(), 3.ToString());
                        }
                        string[] tempInven = new string[1000];
                        for (int i = 0; i < invenItems.Count; i++)
                        {
                            tempInven[i] = invenItems[i];
                        }
                        writeToCSV(currentPlayer.username, tempInven[0], tempInven[1], tempInven[2], tempInven[3], tempInven[4], "inventory.csv");
                        currentStack.stackWriteToCSV();
                        Print("Would you like to see all the questions you have answered?", ConsoleColor.White, false);
                        string outputQuestions = Console.ReadLine().ToLower();
                        if (outputQuestions == "yes")
                        {
                            for (int i = 0; i < currentStack.items.Length; i++)
                            {
                                currentStack.Poppy();
                            }

                            Print("Click enter to exit");
                            Console.ReadLine();
                            Exit();
                        }
                        else
                        {
                            ////////////////////need to delete items from inventory and questions in stack.
                            Console.Clear();
                            const string won = "You have beaten the game";
                            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (won.Length / 2)) + "}", won));

                            Console.WriteLine("Would you like to play again? Type yes or no");
                            string playAgain = Console.ReadLine().ToLower();

                            if (playAgain == "yes")
                            {
                                currentPlayer.score = 0;
                                Menu();
                            }
                            else if (playAgain == "no")
                            {
                                Console.Clear();
                                Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (won.Length / 2)) + "}", won));
                                Console.Read();
                            }

                            else
                            {
                                while (playAgain != "yes" || playAgain != "no")
                                {
                                    Console.WriteLine("Please enter a valid input of either yes or no");
                                    playAgain = Console.ReadLine().ToLower();

                                    if (playAgain == "yes")
                                    {
                                        currentPlayer.score = 0;
                                        Menu();
                                    }
                                    else if (playAgain == "no")
                                    {
                                        Console.Clear();
                                        Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (won.Length / 2)) + "}", won));
                                        Console.Read();
                                    }
                                }
                            }
                            Print("Click enter to exit");
                            Console.ReadLine();
                            Exit();
                        }


                    }



                }
                #endregion
                #region user has won
                //this is outputted if the user has won
                else if (currentPlayer.score >= 6)
                {
                    Console.Clear();
                    const string won = "You have beaten the game";
                    Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (won.Length / 2)) + "}", won));
                    Console.WriteLine("Would you like to play again? Type yes or no");
                    string playAgain = Console.ReadLine().ToLower();

                    if (playAgain == "yes")
                    {
                        currentPlayer.score = 0;
                        Menu();
                    }
                    else if (playAgain == "no")
                    {
                        Console.Clear();
                        Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (won.Length / 2)) + "}", won));
                        Console.Read();
                    }

                    else
                    {
                        while (playAgain != "yes" || playAgain != "no")
                        {
                            Console.WriteLine("Please enter a valid input of either yes or no");
                            playAgain = Console.ReadLine().ToLower();

                            if (playAgain == "yes")
                            {
                                currentPlayer.score = 0;
                                Menu();
                            }
                            else if (playAgain == "no")
                            {
                                Console.Clear();
                                Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (won.Length / 2)) + "}", won));
                                Console.Read();
                            }
                        }
                    }



                }
                #endregion



            }
        }
        #endregion

        #region User character types
        //this is where the user sees its options for its character type and sets the users character.
        public static int  CharecterType()
        {
            Console.Clear();
            //Waroir
            Console.WriteLine("Character info\n");
            Console.WriteLine("Warroir", ConsoleColor.Blue);
            Console.WriteLine("Skill Name: Tilted Scales", ConsoleColor.White);


            Console.WriteLine("Skill Perks: 25% less damage.");
            //monk

            Console.WriteLine("\n\nMonk", ConsoleColor.Blue);
            Console.WriteLine("Skill Name: Rinne Rebirth", ConsoleColor.White);

            Console.WriteLine("Skill Perks: 50% chance of revival after death", ConsoleColor.White);

            //Mage
            Console.WriteLine("\n\nMage", ConsoleColor.Blue);
            Console.WriteLine("Skill Name: Magical Advantage", ConsoleColor.White);
            Console.WriteLine("Skill Perks: Start with an extra life point", ConsoleColor.White);



            Console.WriteLine("\n\nChoose from: Mage[1], Monk[2] or Warrior[3]");
            
            return int.Parse(Console.ReadLine());

        }
        #endregion

    }
}
