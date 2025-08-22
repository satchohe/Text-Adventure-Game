using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using static System.Net.Mime.MediaTypeNames;
using System.Threading;

namespace Text_Adventure_Game
{
    internal class SQLHandling
    {
        private string password;
        private string username;
        private string conversation = "";
        //returns the conversation of the users
        public string getConversation() => conversation;
        //sets the conversations between users
        private void setConversation(string text, string currentUsername, bool addUsername)
        {
        //deletes previous chat so that the database does not get too big
            if(conversation.Length >= 200)
            {
                conversation = "Previous chat was deleted\n";
            }
            //apends the users username to their text
            if (addUsername)
            {

                conversation = conversation + "\n" + currentUsername + ": " + text;
            }
           
            else
            {
                conversation = conversation + "\n" + text;
            }
        }
        public SQLHandling(string username, string password)
        {
            this.password = password;
            this.username = username;
        }
        //login and sign up
        public bool userAccount(bool newAccount)
        {
            
            //dictionary to that stores all the users details from the sql database
            Dictionary<int, Dictionary<string, int>> usersDetails = new Dictionary<int, Dictionary<string, int>>();


            bool found = false;
            if (newAccount == false)
            {
             
                int hashedPass = hashFunc(password);
                //List of all the users id's
                List<int> idList = getUser("SELECT ID FROM tblUsers;", "int").Item1;
                //List of all users username
                List<string> usersUsernameListList = getUser("SELECT USERNAME FROM tblUsers;", "string").Item2;
                //List of all the overflows.
                List<int> overflowList = getUser("SELECT OVERFLOW FROM tblUsers;", "overflowInt").Item3;

                //iterating through all the users id's
                for (int i = 0; i < idList.Count; i++)
                {
                    // creating keys based on the users id.
                    usersDetails.Add(idList[i], new Dictionary<string, int>());
                    //adding values to the keys
                    usersDetails[idList[i]].Add(usersUsernameListList[i], overflowList[i]);

                }
                //checking if the entrys do not contain the hashed passwords as keys.
                if (!usersDetails.ContainsKey(hashedPass))
                {
                    Console.Clear();
                    Console.WriteLine("Incorrect username or password");
                }
                else
                {

                    //checking to see if the the entered username matches that of the corrosponding id.
                    if (usersDetails[hashedPass].ContainsKey(username))
                    {
                        return true;
                    }                  
                    else
                    {
                        //checking for the first occurance of the currently hashed users password in the idList
                        int index =  overflowList.IndexOf(hashedPass);
                        if(index > -1)
                        {
                            for (int i = index; i < overflowList.Count; i++)
                            {
                                if (overflowList[i] == hashedPass)
                                {
                                    if (usersDetails[idList[i]].ContainsKey(username))
                                    {

                                        return true;
                                    }
                                }
                            }
                        }
                        //if index variable returns a -1 than the password does not exist.
                        else
                        {
                            DungeonQuizzes dq = new DungeonQuizzes();
                            Console.Clear();
                            Console.WriteLine("Your username or password is incorrect.");

                            dq.Login();
                            return false;
                        }
                        
                  
                       
                     



                    }
                }


            }
            else
            {

     
                int hashedPass = hashFunc(password);
                //list of all the id's in the sql database
                List<int> idList = getUser("SELECT ID FROM tblUsers;", "int").Item1;

                //checking if the hashed password is in the idList. If it is then add then it will add the hashed password to the overflow in database,
                //store it as null
                if (idList.Contains(hashedPass))
                {
                    int newhashedPass = hashedPass;
                    //keeps addding 1 to the hashedpass until its no longer the same as any item in the idList
                    while(idList.Contains(newhashedPass))
                    {
                        newhashedPass = newhashedPass + 1;
                    }
                    addUser(newhashedPass, username, hashedPass);

                }
                else
                {
                    addUser(hashedPass, username, null);
                }
                return true;

            }
            return false;
        }
        //subroutine that hashes
        public  int hashFunc(string password)
        {
            byte[] passwordASCII = ASCIIEncoding.ASCII.GetBytes(password);
            int asciiTotal = 0;
            foreach (var letter in passwordASCII)
            {
                asciiTotal = asciiTotal + letter;

            }
            int hashedPass = asciiTotal % 5;
            return hashedPass;
        }
        const string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;";


        public void createDbTable(string sqlStatement = "CREATE table tblUsers(ID INT primary key, USERNAME VARCHAR(50), OVERFLOW INT NULL)")
        {
            
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            try
            {
                //checking state of the connection
                if(sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }
                SqlCommand sqlCommand = new SqlCommand(sqlStatement, sqlConnection);
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error: " + ex.Message);
            }
            finally { 
                sqlConnection.Close();
            }
        }
        //adds the users and its details to the database
        public void addUser(int id, string username, int? overflow)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            string sqlStatement = "INSERT INTO tblUsers(ID, USERNAME, OVERFLOW) VALUES(@id, @username, @overflow);";
            try
            {
                using (var sqlCommand = new SqlCommand(sqlStatement, sqlConnection))
                {
                    //checking if the connection state is closed
                    if (sqlConnection.State == ConnectionState.Closed)
                    {
                        sqlConnection.Open();
                    }
                    sqlCommand.Parameters.AddWithValue("@id", id);
                    sqlCommand.Parameters.AddWithValue("@username", username);

                    // storing a null value if there is no overflow when adding the user's details to the database 
                    if(overflow == null)
                    {
                        sqlCommand.Parameters.AddWithValue("@overflow", DBNull.Value);

                    }
                    else
                    {
                        sqlCommand.Parameters.AddWithValue("@overflow", overflow);

                    }
                    sqlCommand.ExecuteNonQuery();


                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
           

        }
        //reading values from the database
        public (List<int>, List<string>, List<int>) getUser(string sqlStatement, string datatype)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            //stores id
            List<int> idList = new List<int>();
            //stores usernames
            List<string> usersUsernameList = new List<string>();
            // stores the overflow list
            List<int> overflowList = new List<int>();

            try
            {
                
                using (var sqlCommand = new SqlCommand(sqlStatement, sqlConnection))
                {
                    sqlConnection.Open();
             
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();


                    //reading data
                    while (sqlDataReader.Read())
                    {
                       //checking each datatype so it knows which list to add to
                       if(datatype == "int")
                       {
                            idList.Add(sqlDataReader.GetInt32(0));
                       }
                        else if(datatype== "string") 
                        {
                            usersUsernameList.Add(sqlDataReader.GetString(0));

                        }
                        else if(datatype== "overflowInt") 
                        {
                            //checking for null values and adding -1 to the list if it is null
                            object o = sqlDataReader["OVERFLOW"];
                            if (o == DBNull.Value)
                            {
                                overflowList.Add(-1);

                            }
                            else
                            {

                                overflowList.Add(sqlDataReader.GetInt32(0));

                            }
                        }
                     
                        else
                        {
                            //checking for null values and only outputting values that are not null
                             Console.Write($"{sqlDataReader.GetInt32(0)} {sqlDataReader.GetString(1)} ");
                            object o = sqlDataReader["OVERFLOW"];
                            if (o == DBNull.Value)
                            {
                                Console.Write("\n");
                            }
                            else
                            {

                                 Console.Write($"{sqlDataReader.GetInt32(2)}\n");
                            }



                        }

                    }

                    return (idList, usersUsernameList, overflowList);

                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
            return (idList, usersUsernameList, overflowList);

        }
        //updating the database for login system
        public void UpdateUser(string sqlStatement)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //sqlStatement = "UPDATE tblUsers* from ";
            try
            {

                using (var sqlCommand = new SqlCommand(sqlStatement, sqlConnection))
                {
                    if (sqlConnection.State == ConnectionState.Closed)
                    {
                        sqlConnection.Open();
                    }
                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }
        //deleting the database
        public void deleteTable(string sqlStatement)
        {

            SqlConnection sqlConnection = new SqlConnection(connectionString);
             //sqlStatement= "DELETE FROM tblUsers";
             //sqlStatement = "DELETE FROM tblUsers WHERE USERNAME = @usernmae";

            try
            {

                using (var sqlCommand = new SqlCommand(sqlStatement, sqlConnection))
                {
                    if (sqlConnection.State == ConnectionState.Closed)
                    {
                        sqlConnection.Open();
                    }
                    // sqlCommand.Parameters.AddWithValue("@username", username);
                    sqlCommand.ExecuteNonQuery();



                }
                //creates a database after its being deleted
                createDbTable();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }
       
        //Checking if an index exists in the database
        private bool IdExists(int totalId)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            try
            {
                //statement counts how many times that index appears in the database.
                string sqlStatement = "SELECT COUNT(*) FROM tblMessage2 WHERE TOTAL_ID = @totalUsername;";
                int idExists = 0;
                using (var sqlCommand = new SqlCommand(sqlStatement, sqlConnection))
                {
                    if (sqlConnection.State == ConnectionState.Closed)
                    {
                        sqlConnection.Open();
                    }

                    sqlCommand.Parameters.AddWithValue("@totalUsername", totalId);
                  
                    //stores -1 if the id does not exist
                    idExists = (int)sqlCommand.ExecuteScalar();
                }
                //if idExists is > 0 then the index exists.
                if (idExists > 0)
                {
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
            return false;

        }
        //reading all the messages from the users conversation from the database
        public void ReadingMessages(string friendsUsername)
        {

            SqlConnection sqlConnection = new SqlConnection(connectionString);

            try
            {

                List<int> idList = getUser("SELECT ID FROM tblUsers;", "int").Item1;
                //List of all users username
                List<string> usersUsernameListList = getUser("SELECT USERNAME FROM tblUsers;", "string").Item2;
                //finding the index of the users username from the idList  and storing it in usersId.
                int usersId = idList[IndexOfId(usersUsernameListList, username)];
               
                //finding the index of the friendsId  and storing it in friendsId
                int friendsId = idList[IndexOfId(usersUsernameListList, friendsUsername)];
                //adds the two id's together
                int totalId = usersId + friendsId;


                string sqlStatement = "SELECT * FROM tblMessage2 WHERE TOTAL_ID = @totalId;";


                //checking if the total id of both users is present in the sql database
                if (!IdExists(totalId))
                {
                    Console.WriteLine("You have to be friends with them before trying to talk with the person");
                    Thread.Sleep(2000);
                    DungeonQuizzes dungeonQuizzes = new DungeonQuizzes();
                    dungeonQuizzes.returnMenu(username);
                }
                //reading the messages
                using (var sqlCommand = new SqlCommand(sqlStatement, sqlConnection))
                {
                    sqlConnection.Open();

                    sqlCommand.Parameters.AddWithValue("@totalId", totalId);
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();


                    while (sqlDataReader.Read())
                    {
                        //error control for null values
                        object o = sqlDataReader["TEXT"];
                        if (o != DBNull.Value)
                        {
                            Console.WriteLine(sqlDataReader.GetString(1));
                        }
                        setConversation(sqlDataReader.GetString(1), username, false);

                    }
                }
             
               
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }

        }
        //returns the index of a username in a string list
        private int IndexOfId(List<string> usersUsernameListList, string username)
        {
       
            return usersUsernameListList.IndexOf(username);
                
        }
        // updates the messages on the database
        public void UpdateMessages(string friendsUsername, string text)
        {//list of id from the database
           List<int> idList = getUser("SELECT ID FROM tblUsers;", "int").Item1;
            //List of all users username
            List<string> usersUsernameListList = getUser("SELECT USERNAME FROM tblUsers;", "string").Item2;

            //finding the index of the users username from the idList  and storing it in usersId.
            int usersId = idList[IndexOfId(usersUsernameListList, username)];

            //finding the index of the friendsId  and storing it in friendsId
            int friendsId = idList[IndexOfId(usersUsernameListList, friendsUsername)];

            int totalId = usersId + friendsId;

            SqlConnection sqlConnection = new SqlConnection(connectionString);

            string sqlStatement = "UPDATE  tblMessage2 SET Text = @Message WHERE TOTAL_ID = @id;";
            try
            {
                //checking if  Id exists, if it does it save the conversation
                if (IdExists(totalId))
                {
                    using (var sqlCommand = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        sqlConnection.Open();
                        sqlCommand.Parameters.AddWithValue("@id", totalId);
                        setConversation(text, username, true);
                        sqlCommand.Parameters.AddWithValue("@Message", getConversation());
                        sqlCommand.ExecuteNonQuery();

                    }
                }
                else
                {
                    Console.WriteLine("You are not friends with this person");
                    Thread.Sleep(2000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }
        //putting the total of the users and friends id's to the database.
        public void InsertTotalId( string friendsUsername)
        {
            List<int> idList = getUser("SELECT ID FROM tblUsers;", "int").Item1;
            //List of all users username
            List<string> usersUsernameListList = getUser("SELECT USERNAME FROM tblUsers;", "string").Item2;
            //finding the index of the users username from the idList  and storing it in usersId.
            int usersId = idList[IndexOfId(usersUsernameListList, username)];

            //finding the index of the friendsId  and storing it in friendsId
           
            int friendsId = idList[IndexOfId(usersUsernameListList, friendsUsername)];

            int totalId = usersId + friendsId;
            SqlConnection sqlConnection = new SqlConnection(connectionString);


            string sqlStatement = "INSERT INTO tblMessage2(TOTAL_ID) VALUES(@id);";
            //create a new record in the database only if its not present
            if(IdExists(totalId) == false)
            {
                try
                {

                    using (var sqlCommand = new SqlCommand(sqlStatement, sqlConnection))
                    {
                        sqlConnection.Open();
                        sqlCommand.Parameters.AddWithValue("@id", totalId);
                        sqlCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    sqlConnection.Close();
                }

            }

        }
      

    }
}
