using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Text_Adventure_Game.Dungeons;
using Text_Adventure_Game;

namespace Text_Adventure_Game
{
    internal class Dungeons
    {
        #region Struct
        //struct is to store the item's name and description that is in each individual dungeon

        public struct ItemDes
        {
            public string nameOfItem;
            public string description;



        }
        #endregion
        #region Dungoen class

        //all the items in the room
        protected const string stone = "stone";
        protected const string door = "door";
        protected string location = "dim-lit room";
        protected const string key = "key";
        protected const string chair = "chair";
        protected const string books = "books";
        protected const string cabinet = "locked cabinet";

        //returns the current dungeon room and the name of that room
        public virtual string getLocation() => location;
      

        //method that returns list of items in the room. The returned items will be cross checked against the users answer to validate it
        public virtual List<string> returnDictionary()
        {
            List<string> firstList = new List<string>();
            List<string> secondList = new List<string>();
            Dictionary<int, ItemDes> itemsInRoom = new Dictionary<int, ItemDes>();

            itemsInRoom.Add(1, new ItemDes() { nameOfItem = stone, description = "small round stone" });
            itemsInRoom.Add(-1, new ItemDes() { nameOfItem = door, description = "wooden and large" });
            itemsInRoom.Add(2, new ItemDes() { nameOfItem = key, description = "metal key" });
            itemsInRoom.Add(-2, new ItemDes() { nameOfItem = chair, description = "wooden chair" });
            itemsInRoom.Add(-3, new ItemDes() { nameOfItem = books, description = "books" });
            itemsInRoom.Add(-4, new ItemDes() { nameOfItem = cabinet, description = "Locked Cabinet" });



            //returns all the items in the dictionary that is greater than 0. This makes it more practicle as a door should not be able to add to inventory.
            foreach (int num in itemsInRoom.Keys)
            {
                ItemDes entry = itemsInRoom[num];
                if (num > 0)
                {
                    if (!firstList.Contains(itemsInRoom[num].nameOfItem))
                    {
                        firstList.Add(itemsInRoom[num].nameOfItem);

                    }

                }
            }


            return firstList;
        }
        // Function outputs the items in the room for the user. Also provides a more descriptive description of the items
        public virtual void ItemsInRoom(string view)
        {
            Dictionary<int, ItemDes> itemsInRoom = new Dictionary<int, ItemDes>();

            itemsInRoom.Add(1, new ItemDes() { nameOfItem = stone, description = "small round stone" });
            itemsInRoom.Add(-1, new ItemDes() { nameOfItem = door, description = "wooden and large" });
            itemsInRoom.Add(2, new ItemDes() { nameOfItem = key, description = "metal key" });
            itemsInRoom.Add(-2, new ItemDes() { nameOfItem = chair, description = "wooden chair" });
            itemsInRoom.Add(-3, new ItemDes() { nameOfItem = books, description = "books" });
            itemsInRoom.Add(-4, new ItemDes() { nameOfItem = cabinet, description = "Locked Cabinet" });
            //just outputs the items in the room
            if (view == "look")
            {

                foreach (ItemDes entry in itemsInRoom.Values)
                {

                    Console.WriteLine("Items in the room are: " + entry.nameOfItem);
                }
            }
            //output the item in the room as well as its description
            else if (view == "examine")
            {
                Console.WriteLine("Type in the item you want to examine");
                string examine = Console.ReadLine().ToLower();
                foreach (ItemDes entry in itemsInRoom.Values)
                {

                    if (examine == entry.nameOfItem)
                    {
                        Console.WriteLine($"\n{examine} -- Description: {entry.description}");
                    }

                }
            }
        }
    }
    #endregion



    #region Dun2 Using inheritance and Polymorphism
    class Dun2 : Dungeons
    {

        protected string turningMech = "turning-mechanism";
        //will output the location of the dungeon
        public override string getLocation()
        {
            location = "Icy - Cold Windy Room";
            return location;
        }
        // outputs the items in the room for the user. Also provides a more descriptive description of the items

        public override void ItemsInRoom(string view)
        {
            Dictionary<int, ItemDes> itemsInRoom = new Dictionary<int, ItemDes>();

            itemsInRoom.Add(1, new ItemDes() { nameOfItem = stone, description = "stone covered in snow" });
            itemsInRoom.Add(-1, new ItemDes() { nameOfItem = door, description = "icy door" });
            itemsInRoom.Add(2, new ItemDes() { nameOfItem = turningMech, description = "Black turning-mechanism" });
            itemsInRoom.Add(3, new ItemDes() { nameOfItem = chair, description = "wooden chair" });
            itemsInRoom.Add(-3, new ItemDes() { nameOfItem = books, description = "Ice covered books" });

            if (view == "look")
            {
                foreach (ItemDes entry in itemsInRoom.Values)
                {

                    Console.WriteLine("Items in the room are: " + entry.nameOfItem);
                }
            }
            else if (view == "examine")
            {
                Console.WriteLine("Type in the item you want to examine");
                string examine = Console.ReadLine().ToLower();
                foreach (ItemDes entry in itemsInRoom.Values)
                {
                    if (examine == entry.nameOfItem)
                    {
                        Console.WriteLine($"\n{examine} -- Description: {entry.description}");
                    }
                }
            }

        }
        //method that returns list of items in the room. The returned items will be cross checked against the users answer to validate it

        public override List<string> returnDictionary()
        {
            List<string> firstList = new List<string>();
            List<string> secondList = new List<string>();
            Dictionary<int, ItemDes> itemsInRoom = new Dictionary<int, ItemDes>();

            itemsInRoom.Add(1, new ItemDes() { nameOfItem = stone, description = "stone covered in snow" });
            itemsInRoom.Add(-1, new ItemDes() { nameOfItem = door, description = "icy door" });
            itemsInRoom.Add(2, new ItemDes() { nameOfItem = turningMech, description = "Black turning mechanism" });
            itemsInRoom.Add(3, new ItemDes() { nameOfItem = chair, description = "wooden chair" });
            itemsInRoom.Add(-3, new ItemDes() { nameOfItem = books, description = "Ice covered books" });

            firstList.Clear();
            secondList.Clear();

            //returns all the items in the dictionary that is greater than 0. This makes it more practicle as a door should not be able to add to inventory.
            foreach (int num in itemsInRoom.Keys)
            {
                ItemDes entry = itemsInRoom[num];
                if (num > 0)
                {
                    if (!firstList.Contains(itemsInRoom[num].nameOfItem))
                    {
                        firstList.Add(itemsInRoom[num].nameOfItem);

                    }


                }
            }

            return firstList;
        }
    }
    #endregion

    #region dungeon 3
    class Dun3 : Dungeons
    {
        protected string bookShelf = "book shelf";
        protected string rug = "rug";


        //returns the location of the room
        public override string getLocation()
        {
            location = "Forest -- green scenery";
            return location;
        }
        // outputs the items in the room for the user. Also provides a more descriptive description of the items

        public override void ItemsInRoom(string view)
        {
            Dictionary<int, ItemDes> itemsInRoom = new Dictionary<int, ItemDes>();

            itemsInRoom.Add(1, new ItemDes() { nameOfItem = stone, description = "stone covered in moth" });
            itemsInRoom.Add(-1, new ItemDes() { nameOfItem = door, description = "moss coverd door" });
            itemsInRoom.Add(3, new ItemDes() { nameOfItem = chair, description = "wooden chair with vines wrapped around them" });
            itemsInRoom.Add(-3, new ItemDes() { nameOfItem = bookShelf, description = "small bookshelf filled with 9 books" });
            itemsInRoom.Add(2, new ItemDes() { nameOfItem = rug, description = "rug decorated with hooks" });


            if (view == "look")
            {
                foreach (ItemDes entry in itemsInRoom.Values)
                {

                    Console.WriteLine("Items in the room are: " + entry.nameOfItem);
                }
            }
            else if (view == "examine")
            {
                Console.WriteLine("Type in the item you want to examine");
                string examine = Console.ReadLine().ToLower();
                foreach (ItemDes entry in itemsInRoom.Values)
                {
                    if (examine == entry.nameOfItem)
                    {
                        Console.WriteLine($"\n{examine} -- Description: {entry.description}");
                    }
                }
            }

        }
        //method that returns list of items in the room. The returned items will be cross checked against the users answer to validate it

        public override List<string> returnDictionary()
        {
            List<string> firstList = new List<string>();
            List<string> secondList = new List<string>();
            Dictionary<int, ItemDes> itemsInRoom = new Dictionary<int, ItemDes>();


            itemsInRoom.Add(1, new ItemDes() { nameOfItem = stone, description = "stone covered in moth" });
            itemsInRoom.Add(-1, new ItemDes() { nameOfItem = door, description = "moss coverd door" });
            itemsInRoom.Add(3, new ItemDes() { nameOfItem = chair, description = "wooden chair with vines wrapped around them" });
            itemsInRoom.Add(-3, new ItemDes() { nameOfItem = bookShelf, description = "small bookshelf filled with 9 books" });
            itemsInRoom.Add(2, new ItemDes() { nameOfItem = rug, description = "rug decorated with hooks" });



            firstList.Clear();
            secondList.Clear();

            //returns all the items in the dictionary that is greater than 0. This makes it more practicle as a door should not be able to add to inventory.
            int count = 0;
            foreach (int num in itemsInRoom.Keys)
            {
                ItemDes entry = itemsInRoom[num];
                if (num > 0)
                {
                    if (!firstList.Contains(itemsInRoom[num].nameOfItem))
                    {
                        firstList.Add(itemsInRoom[num].nameOfItem);

                    }

                }
            }
            return firstList;
        }
    }
    #endregion
    #region dungeon 4
    class Dun4 : Dungeons
    {
        Dictionary<int, ItemDes> itemsInRoom = new Dictionary<int, ItemDes>();

        protected string contacts = "contacts";
        public override string getLocation()
        {
            location = "Bleak dark room";
            return location;
        }
        // outputs the items in the room for the user. Also provides a more descriptive description of the items

        public override void ItemsInRoom(string view)
        {


            itemsInRoom.Add(-1, new ItemDes() { nameOfItem = door, description = "brittle  door" });
            itemsInRoom.Add(2, new ItemDes() { nameOfItem = contacts, description = "paper with a list of names" });

            if (view == "look")
            {
                foreach (ItemDes entry in itemsInRoom.Values)
                {

                    Console.WriteLine("Items in the room are: " + entry.nameOfItem);
                }
            }
            else if (view == "examine")
            {
                Console.WriteLine("Type in the item you want to examine");
                string examine = Console.ReadLine().ToLower();
                foreach (ItemDes entry in itemsInRoom.Values)
                {
                    if (examine == entry.nameOfItem)
                    {
                        Console.WriteLine($"\n{examine} -- Description: {entry.description}");
                    }
                }
            }

        }
        //method that returns list of items in the room. The returned items will be cross checked against the users answer to validate it

        public override List<string> returnDictionary()
        {
            List<string> firstList = new List<string>();
            List<string> secondList = new List<string>();

            Dictionary<int, ItemDes> itemsInRoom = new Dictionary<int, ItemDes>();

            itemsInRoom.Add(-1, new ItemDes() { nameOfItem = door, description = "brittle  door" });
            itemsInRoom.Add(2, new ItemDes() { nameOfItem = contacts, description = "paper with a list of names" });


            //returns all the items in the dictionary that is greater than 0. This makes it more practicle as a door should not be able to add to invetory.
            firstList.Clear();
            secondList.Clear();

            //returns all the items in the dictionary that is greater than 0. This makes it more practicle as a door should not be able to add to inventory.
            int count = 0;
            foreach (int num in itemsInRoom.Keys)
            {
                ItemDes entry = itemsInRoom[num];
                if (num > 0)
                {
                    if (!firstList.Contains(itemsInRoom[num].nameOfItem))
                    {
                        firstList.Add(itemsInRoom[num].nameOfItem);

                    }

                }
            }
            return firstList;
        }
    }
    #endregion
    #region Dungeon 5 - final dungeon
    class Dun5 : Dungeons
    {
        protected string chest = "treasure chest";
        protected string crossbow = "crossbow";
        protected string table = "table";

        Dictionary<int, ItemDes> itemsInRoom = new Dictionary<int, ItemDes>();

        public override string getLocation()
        {
            location = "Gold decorated room";
            return location;
        }
        // outputs the items in the room for the user. Also provides a more descriptive description of the items

        public override void ItemsInRoom(string view)
        {


            itemsInRoom.Add(-1, new ItemDes() { nameOfItem = door, description = "Gold decorated door" });
            itemsInRoom.Add(-2, new ItemDes() { nameOfItem = chair, description = "Golden throne" });
            itemsInRoom.Add(1, new ItemDes() { nameOfItem = stone, description = "Bolder plated in gold" });
            itemsInRoom.Add(2, new ItemDes() { nameOfItem = chest, description = "Chess filled with gold" });
            itemsInRoom.Add(-3, new ItemDes() { nameOfItem = crossbow, description = "Gold crossbow hanging on a wall" });
            itemsInRoom.Add(-4, new ItemDes() { nameOfItem = table, description = "Gold table in the middle of the room" });



            if (view == "look")
            {
                foreach (ItemDes entry in itemsInRoom.Values)
                {

                    Console.WriteLine("Items in the room are: " + entry.nameOfItem);
                }
            }
            else if (view == "examine")
            {
                Console.WriteLine("Type in the item you want to examine");
                string examine = Console.ReadLine().ToLower();
                foreach (ItemDes entry in itemsInRoom.Values)
                {
                    if (examine == entry.nameOfItem)
                    {
                        Console.WriteLine($"\n{examine} -- Description: {entry.description}");
                    }
                }
            }

        }
        //method that returns list of items in the room. The returned items will be cross checked against the users answer to validate it

        public override List<string> returnDictionary()
        {

            List<string> firstList = new List<string>();
            List<string> secondList = new List<string>();
            Dictionary<int, ItemDes> itemsInRoom = new Dictionary<int, ItemDes>();


            itemsInRoom.Add(-1, new ItemDes() { nameOfItem = door, description = "Gold decorated door" });
            itemsInRoom.Add(-2, new ItemDes() { nameOfItem = chair, description = "Golden throne" });
            itemsInRoom.Add(1, new ItemDes() { nameOfItem = stone, description = "Bolder plated in gold" });
            itemsInRoom.Add(2, new ItemDes() { nameOfItem = chest, description = "Chess filled with gold" });
            itemsInRoom.Add(-3, new ItemDes() { nameOfItem = crossbow, description = "Gold crossbow hanging on a wall" });
            itemsInRoom.Add(-4, new ItemDes() { nameOfItem = table, description = "Gold table in the middle of the room" });

            firstList.Clear();
            secondList.Clear();

            //returns all the items in the dictionary that is greater than 0. This makes it more practicle as a door should not be able to add to inventory.
            int count = 0;
            foreach (int num in itemsInRoom.Keys)
            {
                ItemDes entry = itemsInRoom[num];
                if (num > 0)
                {
                    if (!firstList.Contains(itemsInRoom[num].nameOfItem))
                    {
                        firstList.Add(itemsInRoom[num].nameOfItem);

                    }

                }
            }
            return firstList;
        }
    }
}
     #endregion
