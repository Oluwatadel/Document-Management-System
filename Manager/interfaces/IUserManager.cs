using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Document_Management_System.Models.Entities;

namespace Document_Management_System.Manager.interfaces
{
    internal interface IUserManager
    {
        bool Login(string email, string password);
        void Register(string firstname, string lastname, string staffNumber, string email, string department, string password);
        void RemoveStaff(string staffnumber);
        void ForgottenPassword(string email, string staffNumber);
        SortedDictionary<string, string> GetAllStaffDetails();
        void ViewStaffByDepartment(string department);
        User GetCurrentUser();
        void WriteToFileBeforeExit();




    }
}
