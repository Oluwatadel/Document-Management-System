using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Document_Management_System.Models.Entities;

namespace Document_Management_System.Manager.interfaces
{
    internal interface IDocumentManager
    {
        void AddDocument(string title, string description, string content, string department, string author);
        void DeleteDocument(string title, User user);
        //void EditDocument(string title, string department
        void SearchDocument(string title, string department);
        SortedList<string, string> ViewAllDocument(bool isAdmin);
        void ViewAllDocumentOfADepartment(string department);
        void ViewContentofDocument(string title, User user);
        void WriteToFileBeforeExit();


    }
}
