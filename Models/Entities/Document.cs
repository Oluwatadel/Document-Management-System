using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Document_Management_System.Models.Entities
{
    public class Document
    {

        public string DocumentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Department { get; set; }
        public DateTime TimeCreated { get; set; }
        public string Author { get; set; }
        public bool IsLocked { get; set; }
        public string password { get; set; }

        public Document(string title, string description, string content, string department, string author)
        {
            DocumentId = Guid.NewGuid().ToString().Substring(0, 6);
            Title = title;
            Description = description;
            Content = content;
            Department = department;
            TimeCreated = DateTime.Now;
            Author = author;
            IsLocked = false;
            password = string.Empty;


        }

        public Document()
        {

        }


    }

}
