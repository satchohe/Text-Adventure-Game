using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Text_Adventure_Game
{
    internal class KeyboardControls
    {
        private string text;
        private string[] options;
        private int index = 0;

        public  KeyboardControls(string text, string[] options)
        {
            this.text = text;
            this.options = options;
            index = 0;

        }
        //outputs the options in the colour needed and the star next to the current option
        public void DisplayOptions()
        {
        
            Console.WriteLine(text);
            for (int i = 0; i < options.Length; i++)
            {
                string currentOption = options[i];
                string prefix;
                //only for the menu
                if (options.Contains("Play"))
                {
                    //turns the text yellow that the user is currently on
                    if (i == index)
                    {
                        prefix = "*";
                        Console.ForegroundColor = ConsoleColor.Yellow;


                    }
                    //if its not the current option it just turns the text white and background black
                    else
                    {
                        prefix = " ";
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                }
                else
                {
                    //turns the text blue that the user is currently on

                    if (i == index)
                    {
                        prefix = "*";
                        Console.ForegroundColor = ConsoleColor.Blue;
                        //Console.BackgroundColor = ConsoleColor.White;


                    }
                    //if its not the current option it just turns the text white and background black
                    else
                    {
                        prefix = " ";
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                }
                if (!options.Contains("Mage"))
                {
                    Console.WriteLine("{0}{1}", prefix, currentOption);

                }
                else
                {
                    Console.Write("{0}{1} ", prefix, currentOption);

                }

            }
            Console.ResetColor();
        }
        public int Run()
        {
            ConsoleKey keyPressed;
            do
            {
                Console.Clear();

                if (options.Contains("Mage"))
                {
                    Console.WriteLine("Hero info\n");

                    Warroir warroir = new Warroir(3);
                    Monk monk = new Monk(3);
                    Mage mage = new Mage(4);

                    //showing the users perks and skills
                    //Mage
                   string heroText =  mage.displayChar();
                    Console.WriteLine(heroText);

                    //monk

                    heroText = monk.displayChar();
                    Console.WriteLine(heroText);



                    //Waroir
                    heroText = warroir.displayChar();
                    Console.WriteLine(heroText);

                    Console.WriteLine("\n");

                }
                DisplayOptions();
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;
                //if its character optoins
                if (options.Contains("Mage"))
                {
                    //increases index if right arrow is clicked
                    if(keyPressed == ConsoleKey.RightArrow)
                    {
                        index++;
                        if (index == options.Length)
                        {
                            index = 0;
                        }
                    }
                    //minus index if left arrow is clicked
                    else if (keyPressed == ConsoleKey.LeftArrow)
                    {
                        index--;
                        if (index == -1)
                        {
                            index = options.Length - 1;
                        }
                    }
                    else if (keyPressed == ConsoleKey.Enter)
                    {
                        return index;
                    }
                }
                //index decreases if the arrow is up
                else
                {
                    if (keyPressed == ConsoleKey.UpArrow)
                    {
                        index--;
                        if (index == -1)
                        {
                            index = options.Length - 1;
                        }
                    }
                    //index increasing if the arrow decreases

                    else if (keyPressed == ConsoleKey.DownArrow)
                    {
                        index++;
                        if (index == options.Length)
                        {
                            index = 0;
                        }
                    }
                    else if (keyPressed == ConsoleKey.Enter)
                    {
                        return index;
                    }

                }

            } while (keyPressed != ConsoleKey.Enter);
            return index;
        }
    }
}
