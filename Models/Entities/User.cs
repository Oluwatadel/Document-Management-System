using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Document_Management_System.Models.Entities
{
    internal class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StaffNumber { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }

        public User(string firstName, string lastname, string staffnumber, string email, string department, string password, bool isAdmin = false)
        {
            FirstName = firstName;
            LastName = lastname;
            Email = email;
            StaffNumber = $"OG{staffnumber}";
            Department = department;
            Password = password;
            IsAdmin = isAdmin;
        }

        public User()
        {

        }


        public override string ToString()
        {
            return $"{FirstName}\t\t{LastName}\t\t{StaffNumber}\t\t{Email}\t\t{Department}\t\t{Password}\t\t{IsAdmin}";
        }
    }
}
