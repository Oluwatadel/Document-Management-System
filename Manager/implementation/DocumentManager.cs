using System;
using System.Collections.Generic;
using Document_Management_System.Manager.interfaces;
using Document_Management_System.Models.Entities;

namespace Document_Management_System.Manager.implementation
{
    internal class DocumentManager : IDocumentManager
    {
        private static string DocumentFileName = @"C:\Users\AIRIS TECHNOVATION\OneDrive\CodeLearnersHub\Csharp-Journey\ConsoleAPP\Final DMS\Files\Document.txt";
        private SortedList<string, string> AdminListOfAllDocument = new();
        Document document = new Document();
        private readonly IList<Document> documents = new List<Document>();


        public DocumentManager()
        {
            LoadAllDocumentOnStartUpFromFile();
        }

        //Add Documents
        public void AddDocument(string title, string description, string content, string department, string author)
        {
            Document doc = new Document(title, description, content, department, author);
            Console.WriteLine("Do you want to lock your file\nEnter Y or N");
            string input = Console.ReadLine().ToUpper();
            switch (input)
            {
                case "Y":
                    Console.WriteLine("Enter the password to Lock the file");
                    doc.password = Console.ReadLine();
                    Console.WriteLine("Notice!!! Ensure to keep your password in a safe place");
                    doc.IsLocked = true;
                    break;
                case "N":
                    doc.IsLocked = false;
                    break;
                default:
                    Console.WriteLine("Wrong Input! File would be saved without locking it");
                    break;
            }

            try
            {
                documents.Add(doc);
                File.WriteAllText(DocumentFileName, string.Empty);
                using (StreamWriter str = new StreamWriter(DocumentFileName, true))
                {
                    foreach (var docu in documents)
                    {
                        str.WriteLine(docu.ToString());
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"Error!!!\n{e.Message}");
            }
        }

        //Delete Document from the list of the document
        public void DeleteDocument(string title, User user)
        {
            foreach (var document in documents)
            {
                if (user.IsAdmin)
                {
                    documents.Remove(document);
                    Console.WriteLine($"{title} is deleted");
                }
                else if (user.Department == document.Department)
                {
                    if (document.IsLocked == true && document.Author == user.FirstName)
                    {
                        documents.Remove(document);
                        Console.WriteLine($"{title} is deleted");
                    }
                    else if (document.IsLocked == true && document.Author != user.FirstName)
                    {
                        Console.Write("Enter the lock pass of this document...");
                        string lockPass = Console.ReadLine();
                        if (document.password == lockPass)
                        {
                            documents.Remove(document);
                            Console.WriteLine($"{title} is deleted");
                        }
                        else
                        {
                            Console.WriteLine("You dont have permission to delete this file!!!");
                        }
                    }
                    else if (document.IsLocked == false)
                    {
                        documents.Remove(document);
                        Console.WriteLine($"{title} is deleted");
                    }
                }
                else
                {
                    Console.WriteLine($"There is no book titled {title}");
                }
            }

            //Writing update to file...first by emptying the file and rewrite what is in the list of document
            File.WriteAllText(DocumentFileName, string.Empty);
            using (StreamWriter str = new StreamWriter(DocumentFileName, true))
            {
                foreach (var document in documents)
                {
                    str.WriteLine(document.ToString());
                }
            }
        }




        //public void EditDocument(string title, string department)
        //{
        //    throw new NotImplementedException();
        //}

        //Search Document
        public void SearchDocument(string title, string department)
        {
            foreach (var document in documents)
            {
                if (document.Title == title && document.Department == department)
                {
                    Console.WriteLine($"{document.DocumentId}\t{document.Title}\n{document.TimeCreated}\tAuthor: {document.Author}");
                }
                else if (document.Title == title && document.Department != department)
                {
                    Console.WriteLine("You dont have permission to view document that does not belong to your department");
                }
                else
                {
                    Console.WriteLine($"There is no document with the title {title}");
                }
            }
        }


        SortedList<string, string> IDocumentManager.ViewAllDocument(bool isAdmin)
        {
            foreach (var document in documents)
            {
                var DocInfo = $"{document.DocumentId}....{document.Title}....Author:{document.Author}...{document.TimeCreated}";
                AdminListOfAllDocument.Add(document.Department, DocInfo);
            }
            return AdminListOfAllDocument;

        }


        static void DisplayAllDocumentsToAdmin(SortedList<string, string> allDocument)
        {
            foreach (var document in allDocument)
            {
                Console.WriteLine($"{document.Key}\t{document.Value}");
            }
        }



        public void ViewContentofDocument(string title, User user)
        {
            foreach (var document in documents)
            {
                if (document.Department != user.Department)
                {
                    Console.WriteLine("This document does not belong to your department");
                }

                if (user.IsAdmin == true || title == document.Title && user.Department == document.Department && document.Author == user.FirstName)
                {
                    Console.WriteLine(document.Content);
                }
                else if (title == document.Title && user.Department == document.Department && document.Author != user.FirstName && document.IsLocked == true)
                {
                    Console.WriteLine("Enter the pass code for this document to view the content of the document");
                    string passcode = Console.ReadLine();
                    int count = 0;
                    while (count <= 3)
                    {
                        if (count == 3)
                        {
                            Console.WriteLine("You have entered a wrong password thrice !!!"); //Next feature == I intend to bar the person on the next wrong input after the count
                        }
                        if (passcode == document.password)
                        {
                            Console.WriteLine(document.Content);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Sorry You enter a wrong Password");
                        }
                        count++;
                    }
                }

            }
        }


        void ViewAllDocumentOfADepartment(string department)
        {
            foreach (var document in documents)
            {
                if (document.Department == department)
                {
                    Console.WriteLine($"{document.DocumentId}....{document.Title}....{document.TimeCreated}....{document.Author}");
                }
            }
        }

        public override string ToString()
        {
            return $"{document.DocumentId}\t\t{document.Title}\t\t{document.Description}\t\t{document.Content}\t\t{document.Department}\t\t{document.Author}\t\t{document.TimeCreated}\t\t{document.IsLocked}\t\t{document.password}";
        }

        private Document ConverToDocument(string str)
        {
            var s = str.Split("\t\t");
            return new Document()
            {
                DocumentId = s[0],
                Title = s[1],
                Description = s[2],
                Content = s[3],
                Department = s[4],
                Author = s[5],
                TimeCreated = DateTime.Parse(s[6]),
                IsLocked = bool.Parse(s[7]),
                password = s[8]

            };
        }

        public IList<Document> LoadAllDocumentOnStartUpFromFile()
        {
            try
            {
                var fileExist = File.Exists(DocumentFileName);
                if (!fileExist) //Here file doesnt exist which means a new file will be created
                {
                    File.Create(DocumentFileName);
                }
                else
                {
                    using (StreamReader readDataFromFileOnStartUp = new StreamReader(DocumentFileName))
                    {
                        while (readDataFromFileOnStartUp.Peek() != -1)
                        {
                            documents.Add(ConverToDocument(readDataFromFileOnStartUp.ReadLine()));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return documents;
        }

        void IDocumentManager.ViewAllDocumentOfADepartment(string department)
        {
            foreach (var document in documents)
            {
                if (document.Department == department)
                {
                    Console.WriteLine($"{document.DocumentId}....{document.Title}...{document.Author}...{document.TimeCreated}...{document.password}");
                }
            }
        }

        void IDocumentManager.WriteToFileBeforeExit()
        {
            File.WriteAllText(DocumentFileName, string.Empty);
            foreach (var doc in documents)
            {
                using (StreamWriter str = new StreamWriter(DocumentFileName, true))
                {
                    str.WriteLine(document.ToString());
                }
            }
        }
    }
}
