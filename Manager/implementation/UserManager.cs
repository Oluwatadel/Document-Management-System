using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using Document_Management_System.Manager.interfaces;
using Document_Management_System.Models.Entities;

namespace Document_Management_System.Manager.implementation
{
    internal class UserManager : IUserManager
    {
        // private readonly User admin;
        private User currentLoggedInUser;
        private User admin = new User("Admin", "Admin", "1234", "admin@gmail.com", "Admin", "qwerty", true);
        private readonly IList<User> users = new List<User>();



        private SortedDictionary<string, string> allUsers = new();
        private static string UserFileName = @"C:\Users\AIRIS TECHNOVATION\OneDrive\CodeLearnersHub\Csharp-Journey\ConsoleAPP\Final DMS\Files\User.txt";




        public UserManager()
        {
            LoadContentFromFileOnStartUp();
        }

        
        public void AddAdmin(User admin)
        {
            var user = users.SingleOrDefault(x => x == admin);

            if(user == null)
            {
                users.Add(admin);
            }
        }
        public SortedDictionary<string, string> GetAllStaffDetails()
        {
            foreach (var user in users)
            {
                allUsers.Add(user.Department, $"{user.LastName + " " + user.FirstName}.....{user.StaffNumber}......{user.Password}");
            }
            return allUsers;
        }

        public static void DisplayContentOfSortedDictionary(SortedDictionary<string, string> list)
        {
            foreach (var user in list)
            {
                Console.WriteLine($"{user.Key}  {user.Value}");
            }
        }

        public User GetCurrentUser()
        {
            return currentLoggedInUser;
        }

        public bool Login(string email, string password)
        {
            User useR = null;
            foreach (var user in users)
            {
                if (user.Email == email && user.Password == password)
                {
                    useR = user;
                    break;
                }
            }

            if (useR == null)
            {
                Console.WriteLine("Invalid Credential !!!");
                return false;
            }
            else
            {
                Console.WriteLine("Login successful");
                currentLoggedInUser = useR;
                return true;
            }
        }

        public void Register(string firstname, string lastname, string staffNumber, string email, string department, string password)
        {
            User user1 = null;
            foreach (User user in users)
            {
                if (staffNumber.Equals(user.StaffNumber) || email.Equals(user.Email))
                {
                    user1 = user;
                    break;
                }
            }
            if (user1 != null)
            {
                Console.WriteLine($"user with username with this credential already exist");
            }
            else
            {
                user1 = new User(firstname, lastname, staffNumber, email, department, password);
                users.Add(user1);
                Console.WriteLine("user registered succesfully");

                var fileExist = File.Exists(UserFileName);
                if (!fileExist)
                {
                    File.Create(UserFileName);
                    using (StreamWriter str = new StreamWriter(UserFileName, true))
                    {
                        foreach (var use1 in users)
                        {
                            str.WriteLine(use1.ToString());
                        }
                    }
                }
                else
                {
                    File.WriteAllText(UserFileName, string.Empty);
                    using(StreamWriter str = new StreamWriter(UserFileName, true))
                    {
                        foreach (var use1 in users)
                        {
                            str.WriteLine(use1.ToString());
                        }
                    }
                } 
            }
            
        }

        public void RemoveStaff(string staffnumber)
        {
            var x = users.SingleOrDefault(a => a.StaffNumber == staffnumber);
            users.Remove(x);

            File.WriteAllText(UserFileName, string.Empty);
            using (StreamWriter userStw = new StreamWriter(UserFileName, true))
            {
                foreach(var user in users)
                {
                    userStw.WriteLine(user.ToString());
                }
            }
        }

        public void ViewStaffByDepartment(string department)
        {
            foreach (var user in users)
            {
                if (user.Department == department)
                {
                    Console.WriteLine($"{user.LastName} {user.FirstName}.....{user.StaffNumber}..{user.Email}...{user.Password}");
                }
            }
        }



        private User ConverToUserObj(string userFromFile)
        {
            var str = userFromFile.Split("\t\t");
            return new User()
            {
                FirstName = str[0],
                LastName = str[1],
                StaffNumber = str[2],
                Email = str[3],
                Department = str[4],
                Password = str[5],
                IsAdmin = bool.Parse(str[6])


            };
        }

        private IList<User> LoadContentFromFileOnStartUp()
        {
            try
            {
                var fileExist = File.Exists(UserFileName);
                if (!fileExist) //Here file doesnt exist which means a new file will be created
                {
                    File.Create(UserFileName);
                }
                else
                {
                    using (StreamReader stream = new StreamReader(UserFileName))
                    {
                        while (stream.Peek() != -1)
                        {
                            users.Add(ConverToUserObj(stream.ReadLine()));
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return users;
        }

        void IUserManager.ForgottenPassword(string email, string staffNumber)
        {
            var user = users.FirstOrDefault(a => a.Email == email && a.StaffNumber == staffNumber);

            Console.WriteLine($"Your password is {user.Password}");

            //foreach (var user in users)
            //{
            //    if (user.Email == email && user.StaffNumber == staffNumber)
            //    {
            //        Console.WriteLine($"Your password is {user.Password}");
            //    }
            //}
        }

        void IUserManager.WriteToFileBeforeExit()
        {
            File.WriteAllText(UserFileName, string.Empty);
            foreach (var x in users)
            {
                using (StreamWriter str = new StreamWriter(UserFileName, true))
                {
                    str.WriteLine(x.ToString());
                }
            }
        }
    }
}
