using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Text_Adventure_Game
{
     class CharacterClass
    {
        public enum CharacterTypes { Mage, Monk, Warrior };
        //the defualt is set to warrior. This is for if for whatever reason the user doesnt choose when they sign in

        public CharacterTypes currentChar = CharacterTypes.Warrior;

        protected double health;
        protected string skillName;
        protected string skillPerk;
        public string heroType;


        public CharacterClass(double health) 
        {
            this.health = health;
        }
      
        public void setHealth(double usersHealth) => this.health = usersHealth;
        public double getHealth() => health;
       
        //base class that returns the normal health minus damage
        public virtual double Perks()
        {
            if (health - 1 <= 0)
            {
                return 0;
            }
    
            return (health -= 1);
            
        }

    }
    class Warroir : CharacterClass
    {
        public Warroir(double health) : base(health)
        {
            this.health = health;
            heroType = "Warroir";
        }

     //outputs perks
        public string displayChar()
        {

            skillName = "Tilted Scales";
            skillPerk = "Skill Perks: 25% less damage.";
            string text = "\n\n" + heroType + "\nSkill Name: " + skillName + "\nSkill Perks: " + skillPerk;
            return text;
        }

        //Performs the perks

        public override double Perks()
        {
            //this is where the users special skill for their different characters activated
            if (health - 0.75 <= 0)
            {
                return 0;
            }
           
               
             return (health -= 0.75);
 

        }
       

    }
    class Monk : CharacterClass
    {
        public Monk(double health) : base(health)
        {
            this.health = health;
            heroType = "Monk";

        }
        //displays the users skills
        public string displayChar()
        {
            //monk
            skillName = " Rinne Rebirth";
            skillPerk = "Skill Perks: 50% chance of revival after death";

            string text = "\n\n" + heroType + "\nSkill Name: " + skillName + "\nSkill Perks: " + skillPerk;
            return text;
        }

        //performs the perks
        public override double Perks()
        {
            DungeonQuizzes dungeonQuizzes = new DungeonQuizzes();
            if (health <= 0)
            {
                Random rand = new Random();
                int randRevive = rand.Next(1, 3);
                if (randRevive == 1)
                {
                   dungeonQuizzes.Print("Using Your Rinne Rebirth skill, you have been revived.", ConsoleColor.DarkCyan);
                    return (1);


                }
                return (0);


            }
            else
            {

                if (health - 1 <= 0)
                {
                    return 0;
                }
                return (health -= 1);


            }
        }
    }
    class Mage : CharacterClass
    {
        public Mage(double health) : base(health)
        {
            this.health = health;
            heroType = "Mage";
        }
    
        //changed
        public string displayChar()
        {
           
            string text;
            skillName = "Magic Advantage";
            skillPerk = "Start with an extra life point";
            //Mage
            
            text = "\n\n" + heroType + "\nSkill Name: " + skillName + "\nSkill Perks: " + skillPerk;
            return text;
        }


    }

}
