
using System;
using System.Threading;
using Text_Adventure_Game;

namespace Text_Adventure_Game
{
    internal class Stack
    {
        //this class object will be used to get the username in csv files
        protected  FileHandling currentFileHandling = new FileHandling();
        //this object will be used to get the users current username in the game
        protected DungeonQuizzes dungeonQuizzes= new DungeonQuizzes();
        public string[] items;
        public int size;
        public int top = -1;
        public Stack(int stackSize)
        {
            this.size = stackSize;
            this.items = new string[this.size];
        }
        //adds item to stack
        public void push(string itemsAdd)
        {
            
            //if the stack is full
            if (top >= 5)
            {
                Console.WriteLine("Error: " + "stack is full");
            }
            else
            {
                //incrementing the top every time an item wants to be added and adding that item to that position in the array
                this.top++;

                items[this.top] = itemsAdd;

            }


        }
        //removes item from stack
        public void pop()
        {
            //if the inventory is empty
            if (top < 0)
            {
                Console.WriteLine("Questions ended");
            }
            else
            {
                
                //removing the top item in the array
                string removing = items[this.top];
                Console.WriteLine("questions: " + removing);
                items[this.top] = null;
                top--;
            }

        }
        //views item in stack
        public void peak()
        {
            //if top is at its defualt position then there is no items in the stack
            if (top <= -1)
            {
                Console.WriteLine("\nYou have no items to view\n");
            }
            else
            {
                //outputting the item at the top of the stack.

                string peak = items[this.top];

                Console.WriteLine(peak);
            }


        }
        //this will write the inventory to file
        public void stackWriteToCSV(string username)
        {
            
            string[] arr = items;
            const string filepath = "questions.csv";
            //checking to see if the users username is already in the csv file.
            currentFileHandling.readRecord(dungeonQuizzes.usernameConnection, filepath, 1);
            //checking if the users username already exists in the stack
            if ((currentFileHandling.stackUsernameCheckPoint == dungeonQuizzes.usernameConnection) && currentFileHandling.stackUsernameCheckPoint != "")
            {

                stackEditCSV();
            }
            else
            {

                currentFileHandling.addRecord(username, items[0], items[1], items[2], items[3], items[4], filepath);

            }

        }
        //edits the csv file
        public void stackEditCSV()
        {
            
            //writes to csv
            const string filepath = "questions.csv";
            currentFileHandling.editRecord(dungeonQuizzes.usernameConnection, filepath, 1, dungeonQuizzes.usernameConnection, items[0], items[1], items[2], items[3], items[4]);


        }


    }
}
