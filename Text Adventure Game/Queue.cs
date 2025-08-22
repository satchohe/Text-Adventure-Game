using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Adventure_Game
{
    internal class Queue
    {
        
        public int[,] items = new int[5,2];
        public int size;
        public int front = 0;
        public int rear = 0;
        Dictionary<string, int> itemsDic = new Dictionary<string, int>();
       public void updateDic(string dungeon, bool switching)
        {

            int count = 1;
            int pastPriority = 0;
            if(switching == true)
            {
                pastPriority = items[rear, 1];
                items[rear + 1, 1] = items[rear, 1];
                items[rear, 1] = pastPriority;

                deQueue();
               


            }
            
            
        }

        public bool Enqueue(int item, int priority)
        {


            if (rear < 4)
            {
               
                items[rear, 0] = item;
                items[rear, 1] = priority;
                rear++;
                Showing();

                return true;
            }
            else
            {
                return false;
            }


        }
        public int deQueue()
        {

            if (front < rear)
            {

                int dequeuedString = items[front, 0];
                items[front, 0] = -1;
                items[front, 1] = -1;
                front = front + 1;

                if (front == rear)
                {

                    return 0;
                }


                return dequeuedString;

            }
            else if (front == rear)
            {
                return 0;

            }
            foreach(int i in items)
            {
                Console.WriteLine(i);
            }


            return 0;



        }
        public List<string> showItems()
        {
            List<string> show = new List<string>();
            show.Clear();
            

            //show.Add(items[front]);
            if (front < rear)
            {
                
                if (items[front, 0] == 1)
                {
                    show.Add("dun2");
                }
                else if (items[front, 0] == 2)
                {
                    show.Add("dun3");
                }
                else if (items[front, 0] == 3)
                {
                    show.Add("dun4");
                }
                else if (items[front, 0] == 4)
                {
                    show.Add("dun5");
                }

                front++;

            }

            foreach (string i in show)
            {
                Console.WriteLine("SDGF "  + i);
            }
            return show;

        }
        public void checkpoint(string items)
        {
            List<int> show = new List<int>();
            show.Clear();
          
            

            //show.Add(items[front, 0]);
            if (front < rear)
            {
                if (items.ToString() == "Text_Adventure_Game.Dungeons")
                {
                    Enqueue(0, 0);

                }
                else if (items.ToString() == "Text_Adventure_Game.Dun2")
                {
                    Enqueue(1, 1);
                }
                else if (items.ToString() == "Text_Adventure_Game.Dun3")
                {
                    Enqueue(2, 2);
                }
                else if (items.ToString() == "Text_Adventure_Game.Dun4")
                {
                    Enqueue(3, 3);
                }
                else if (items.ToString() == "Text_Adventure_Game.Dun5")
                {
                    Enqueue(4, 4);
                }


            }
            


        }
        public void Showing()
        {
            for (int i = 0; i < items.GetLength(0); i++)
            {
                for (int j = 0; j < items.GetLength(1); j++)
                {
                    Console.Write("{0} ", items[i, j]);
                }
                Console.WriteLine();
            }
        }

    }
    
}

