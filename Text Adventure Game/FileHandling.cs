using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using Text_Adventure_Game;

namespace Text_Adventure_Game
{
    internal class FileHandling
    {

        public string fieldWant = "";
        public int scorings = 0;
        public string leaderboard = "";
        public List<int> scoreList = new List<int>();
        public List<string> usernameList = new List<string>();
        //protected static Search_and_Sort<string> stringSearchAndSort = new Search_and_Sort<string>();
        //public List<int> passwordIndexes = new List<int>();
        public List<string> usersUsername = new List<string>();
        public int id = 0;





        public string[] invenarr = { "" };
        private string[] questionsArr = { "" };//////////
        public string usersScore = "";



        private string usernameCheckPoint = "";///////
        public string stackUsernameCheckPoint = "";
        public string characTypeField = "";
        //private string[] playerDetails;
        public string checking = "";
        #region getters and setters
        #region UsernameCheckPoint
        public string getUsernameCheckPoint()
        {
            return usernameCheckPoint;

        }
        public void SetUsernameCheckPoint(string username)
        {

            this.usernameCheckPoint = username;
        }
        #endregion


        #region invenarr
        public string[] getQuestionsArr()
        {
            return questionsArr;
        }
        public void setQuestionsArr(string[] questionsArr)
        {
            this.questionsArr = questionsArr;

        }
        #endregion

        #endregion
        #region Add to record
        //adding to record on csv
        public void addRecord(string username ="", string characterType="" ,string score="", string health="", string password = "", string id = "", string filepath = "usersDetails.csv")
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@filepath, true))
                {
                    //coma seperating each data to fit csv file format
                    if(filepath == "usersDetails.csv")
                    {
                        file.WriteLine(username + "," + characterType + "," + score + "," + health);

                    }
                    else if(filepath == "questions.csv")
                    {
                        file.WriteLine(username + "," + characterType + "," + score + "," + health + "," + password + "," + id);

                    }
                    else
                    {
                        file.WriteLine(username + "," + characterType + "," + score + "," + health + "," + password + "," + id);

                    }

                }
            }
            catch (Exception ex)
            {
                //if the file cant be found it creates a new one
                if (!File.Exists(filepath))
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@filepath, true))
                    {
                        file.WriteLine(username + "," + characterType + "," + score + "," + health);

                    }


                }

                else
                {
                    Console.WriteLine("Error: " + ex.Message);

                }


            }
        }
        #endregion
        //reading from record on csv
        #region Read from record
        public bool readRecord(string searchTerm, string filepath, int positionOfSearchTerm)
        {

            positionOfSearchTerm--;
            // try catch is if file does not exists it creates it.
            try
            {
                //stores all the lines into a string array
                string[] lines = System.IO.File.ReadAllLines(@filepath);
                //for looping through all the lines in the file
                for (int i = 0; i < lines.Length; i++)
                {
                    //splitting each line in the file into indivual strings
                    string[] fields = lines[i].Split(',');

                    //avoids the headings of the files
                    if (fields[1] != "Username")
                    {
                        usersUsername.Add(fields[1]);

                    }
                    //checking to see if the searchTerm matches
                    if (recordMatches(searchTerm, fields, positionOfSearchTerm))
                    {
                        if (recordMatches(searchTerm, fields, positionOfSearchTerm))
                        {
                            int count = 0;
                            
                            if (filepath == "questions.csv" && count < 4)
                            {
                                //storing all the users fields and storing it. This will be called in the main program
                                setQuestionsArr(fields);
                                invenarr = fields;
                               
                                SetUsernameCheckPoint(fields[0]);

                                return true;

                            }
                            else if (filepath == "usersDetails.csv")
                            {

                                
                                //storing all the users fields and storing it. This will be called in the main program
                                leaderboard = "Username: " + fields[0] + "   score: " + fields[2];
                                characTypeField = fields[1];
                                 usersScore = fields[2];
                                invenarr = fields;
                                usernameCheckPoint = fields[0];
                              
                                return true;

                            }
                            else if(filepath == "inventory.csv")
                            {
                                //the fields are stored in the string array invenarr
                                invenarr = fields;
                                //this stores the username 
                                Thread.Sleep(2000);
                                SetUsernameCheckPoint(fields[0]);
                                return true;



                            }
                            else
                            {
                               
                                invenarr = fields;
                                SetUsernameCheckPoint(fields[0]);
                                return true;
                            }

                           
                        }
                    }
                }


                return false;
               

            }
            catch (Exception ex)
            {
                if (!File.Exists(filepath))
                {
                    File.WriteAllText(filepath, null);
                    return false;


                }
               
                else
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return false;
                }

            }

        }
        //returns true or false depending on whether or not the search term matches the specific place in the record
        public bool recordMatches(string searchTerm, string[] record, int positionSearchTerm)
        {


            if (record[positionSearchTerm].Equals(searchTerm))
            {
                return true;
            }
            return false;

        }
        #endregion
        //deleting a record from csv
        #region Delete 
        public bool deleteRecord(string searchTerm, string filepath, int positionOfSearhTerm)
        {
            positionOfSearhTerm--;
            const string tempFile = "temp.txt";
            //write every record we want to keep then delete the old file then rename the temp file to what it was called 
            bool deleted = false;
            //checks if file exits, if not it creates it
            try
            {
                string[] lines = System.IO.File.ReadAllLines(filepath);
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] fields = lines[i].Split(',');
                    //if record matches search term and is not deleted then you add it to a new file because you dont want to delete it
                    if (!(recordMatches(searchTerm, fields, positionOfSearhTerm)) || deleted) // only deletes the first record that matches
                    {
                        addRecord(fields[0], fields[1], fields[2], fields[3], fields[4], fields[5], @tempFile);
                    }
                    else
                    {
                        deleted = true;
                        return true;
                    }
                }
                File.Delete(filepath);
                //moving the temp file file to be what the old file file path was
                System.IO.File.Move(tempFile, filepath);
            }
            catch (Exception ex)
            {

                if (!File.Exists(filepath))
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return false;
                }

            }
            return false;
        }
        #endregion
        //editing csv file
        #region Edit File
        public void editRecord(string searchTerm, string filepath, int positionOfSearchTerm, string charecterAndUser, string fieldOne= "", string fieldTwo = "", string fieldThree = "", string fieldFour = "", string fieldFive = "")
        {
            //position is -- to make it more user freindly as people are more likely to pass in a one as first term so it minus it the position the computer would read
            positionOfSearchTerm--;
            //file path for temperary file
            const string tempFile = "temp.txt";
            //once we edit a record we dont want to edit it anymore
            bool edited = false;
          int count = 0;

                string[] lines = System.IO.File.ReadAllLines(filepath);
            

                //find the record to be edited
            for (int i = 0; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split(',');
                // if the records dont match then what we are trying to edit we dont edit that record and write it to the temp file.
                if (!(recordMatches(searchTerm, fields, positionOfSearchTerm))) 
                {
                    if (filepath == "usersDetails.csv")
                    {
                        //fields are the current lines we are reading from the lines array
                        addRecord(fields[0], fields[1], fields[2], fields[3], tempFile); 

                    }
                    else if (filepath == "inventory.csv")
                    {

                    //fields are the current lines we are reading from the lines array
                        addRecord(fields[0], fields[1], fields[2], fields[3], fields[4], fields[5], tempFile); 

                    }
                    else if (filepath == "questions.csv")
                    {
                    //fields are the current lines we are reading from the lines array

                        addRecord(fields[0], fields[1], fields[2], fields[3], fields[4], fields[5], tempFile);
                 
                    }
                }
                else
                {
                    //if it is edited
                    if (!edited)
                    {

                    addRecord(charecterAndUser, fieldOne, fieldTwo, fieldThree, fieldFour, fieldFive, tempFile);
                    edited = true;
                    }
                }
            }
                //deletes the old file
                File.Delete(@filepath);
            //Rename new file setting it to whatever the old file path was.
           
                System.IO.File.Move(tempFile, filepath);

            
           



        }
        #endregion
        //reading multiple csv files
        #region read Multiple Records
        public string[] readMultipleRecords(string searchTerm, string filepath, int positionOfSearchTerm)
        {
            positionOfSearchTerm--;
            List<string> records = new List<string>();
            //ArrayList records = new ArrayList();
            string[] recordNotFound = { "Not found" };
            //try catch is so if the record is not found it does not throw an error
            try
            {
                string[] lines = System.IO.File.ReadAllLines(@filepath);
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] fields = lines[i].Split(',');
                    if (recordMatches(searchTerm, fields, positionOfSearchTerm))
                    {
                        records.Add(lines[i]);
                    }

                }
                //this makes sure that if the record is null it does not throw an error
                if (records[0] == null)
                {
                    return recordNotFound;
                }
                return (records.ToArray());
            }
            catch (Exception)
            {
                return recordNotFound;


            }


        }
        #endregion
        //verify login
        #region Verify Login
        //verifies the users enterd details
        public bool VerifyLogin(string username, string password, string filepath)
        {
            try
            {
                //holds all the lines in the file
                string[] lines = System.IO.File.ReadAllLines(@filepath);
                //looping through each line in the lines string array
                for (int i = 0; i < lines.Length; i++)
                {
                    //splitting each line in the file into indivual strings
                    string[] field = lines[i].Split(',');
                    //checking if the field matches the username and password.
                    //the index 0 in the csv file is where the username is stored and the index 1 is where the password is stored.
                    if (field[0].Equals(username) && field[1].Equals(password))
                    {
                        return true;

                    }
                }
                return false;

            }
            catch (Exception ex)
            {
                //creates a file if the file does not exist
                if (!File.Exists(filepath))
                {
                    return false;


                }
                else
                {
                    //goes  ack to main program where the user will be asked again to login or sign in
                    Console.WriteLine("Error: " + ex.Message);
                    return false;
                }

            }


        }
        #endregion
        //outputting a specific field on a csv file
        #region output a field
        public void OutputField(string searchTerm, string filepath, int positionOfSearchTerm, int positionOfSearchTermWanted)
        {
            positionOfSearchTerm--;
            positionOfSearchTermWanted--;
            bool foundWanted = false;

            try
            {
                string[] lines = File.ReadAllLines(@filepath);
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] record = lines[i].Split(',');

                    if (recordMatchesForSearching(searchTerm, record, positionOfSearchTerm, positionOfSearchTermWanted))
                    {
                        if (recordMatchesForSearching(searchTerm, record, positionOfSearchTerm, positionOfSearchTermWanted))
                        {
                            foundWanted = true;
                            

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error : " + ex.Message);
            }
        }
        public bool recordMatchesForSearching(string searchTerm, string[] record, int positionSearchTerm, int positionOfSearchTermWanted)
        {
            
            if (record[positionSearchTerm].Equals(searchTerm))
            {
                fieldWant = record[positionOfSearchTermWanted];
                id = int.Parse(record[positionOfSearchTermWanted]);

                return true;
            }
            return false;

        }


        #endregion
        //returning a field
        public void returnRecord(string filepath, int positionOfRecord)
        {
            positionOfRecord--;
            // try catch is if file does not exists it creates it.
            try
            {
                //stores all the lines into a string array
                string[] lines = System.IO.File.ReadAllLines(@filepath);

                for (int i = 0; i < lines.Length; i++)
                {
                    //splitting each line in the file into indivual strings

                    string[] fields = lines[i].Split(',');
                    
                    //checking if it's not the heading 'Username'
                    if (fields[1] != "Username")
                    {
                        usersUsername.Add(fields[1]);

                    }
                }


                // return false;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        //returning a field from a csv file
        #region Returning a field

        public bool returningFields(string filepath, int positionOfSearchTerm, bool isInt)
        {
            positionOfSearchTerm--;
            bool foundWanted = false;
            try
            {
                string[] fields = { "" };

                string[] lines = System.IO.File.ReadAllLines(@filepath);
                for (int i = 0; i < lines.Length; i++)
                {
                    fields = lines[i].Split(',');

                    AddingReturningFieldsToList(fields, positionOfSearchTerm, isInt);
                }
            }
            catch (Exception ex)
            {
                if (!File.Exists(filepath))
                {
                    Console.WriteLine("File needed not found");
                    File.WriteAllText(filepath, null);
                    return false;


                }
                else
                {
                    return false; 
                }
            }
            return false;
        }
        //adding the scores to scoreList
        public void AddingReturningFieldsToList(string[] record, int positionSearchTerm, bool isInt)
        {
            bool successfullyParsed = false;
            //checks if user is looking  a  int order of int or string.
            if (isInt == true)
            {
                int recordCount = 0;
                int num = 0;
                foreach (var item in record[positionSearchTerm])
                {
                    if(recordCount == record[positionSearchTerm].Length)
                    {
                        break;
                    }
                    successfullyParsed = int.TryParse(item.ToString(), out num);
                    if (successfullyParsed == false)
                    {
                        continue;
                    }
                    else
                    {
                       scoreList.Add(num);


                    }
                    recordCount++;
                }
            
            }
            else
            {
                if(record[0] != "Username")
                {
                    usernameList.Add(record[0]);

                }
              


            }
           

        }

        #endregion

    }

}
