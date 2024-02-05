using Document_Management_System.Manager.implementation;
using Document_Management_System.Manager.interfaces;
using Document_Management_System.Models.Entities;
using Document_Management_System.Models.Enum;

namespace Document_Management_System
{
    internal class Menu
    {
        IUserManager _user = new UserManager();
        IDocumentManager _documentManager = new DocumentManager();

        public void MainMenu()
        {
            bool isRunning = true;
            while (isRunning)
            {
                bool isLoggedIn = false;
                while (!isLoggedIn)
                {
                    Console.WriteLine("========================================================================");
                    Console.WriteLine("==      Welcome to the Ogun State MOE's Database Management system   ===");
                    Console.WriteLine("========================================================================");

                    ShowStartMenu(); //This show the Enum already defined
                    int userResponse = int.Parse(Console.ReadLine());

                    switch (userResponse)
                    {
                        case 1:
                            Console.Write("Enter firstname: ");
                            string firstname = Console.ReadLine();
                            Console.Write("Enter lastname: ");
                            string lastmame = Console.ReadLine();
                            Console.Write("Enter StaffNumber: ");
                            string staffNumber = Console.ReadLine();
                            Console.Write("Enter email: ");
                            string email = Console.ReadLine();
                            while (!(email.Contains("@") && email.Contains(".com")))
                            {
                                Console.Write("Enter a correct email: ");
                                email = Console.ReadLine();
                                if (email.Contains("@") && email.Contains(".com"))
                                {
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Enter a correct email in this format dms@gmail.com");
                                }
                            }
                            Console.Write("Enter department: ");
                            string department = Console.ReadLine();
                            Console.Write("Enter password: ");
                            string password = Console.ReadLine();
                            _user.Register(firstname, lastmame, staffNumber, email, department, password);
                            break;
                        case 2:
                            Console.Write("Enter Your registered email: ");
                            string loginStaffNumber = Console.ReadLine();
                            Console.Write("Enter password: ");
                            string loginPassword = Console.ReadLine();
                            isLoggedIn = _user.Login(loginStaffNumber, loginPassword);
                            break;
                        case 3:
                            Console.Write("Enter your registered email: ");
                            string regEmail = Console.ReadLine();
                            Console.Write("Enter your staffNumber: ");
                            string staffNo = Console.ReadLine();
                            _user.ForgottenPassword(regEmail, staffNo);
                            break;
                        case 4:
                            _user.WriteToFileBeforeExit();
                            _documentManager.WriteToFileBeforeExit();
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Wrong input");
                            break;
                    }


                    while (isLoggedIn)
                    {
                        var CurrentUser = _user.GetCurrentUser();
                        if (CurrentUser.IsAdmin != true)
                        {
                            Console.WriteLine("Press 1 to Add Document");
                            Console.WriteLine("Press 2 to Search for Document");
                            Console.WriteLine("Press 3 to View all documents in your Department");
                            Console.WriteLine("Press 4 to View the content of a document");
                            Console.WriteLine("Press 5 to Delete Document");
                            Console.WriteLine("Press 6 to Logout");

                            string choice = Console.ReadLine();

                            switch (choice)
                            {
                                case "1":
                                    Console.WriteLine("Enter the title of the document");
                                    string title = Console.ReadLine();
                                    Console.WriteLine("Enter the description of the document");
                                    string description = Console.ReadLine();
                                    Console.WriteLine("Enter the content of the document");
                                    string content = Console.ReadLine();

                                    _documentManager.AddDocument(title, description, content, CurrentUser.Department, CurrentUser.FirstName);
                                    break;
                                case "2":
                                    Console.WriteLine("Enter the title of the document");
                                    string titleOfDoc = Console.ReadLine();
                                    _documentManager.SearchDocument(titleOfDoc, CurrentUser.Department);
                                    break;
                                case "3":
                                    _documentManager.ViewAllDocumentOfADepartment(CurrentUser.Department);
                                    break;
                                case "4":
                                    Console.WriteLine("Enter the title of the document");
                                    string titleOfDocumentToBeViewed = Console.ReadLine();
                                    _documentManager.ViewContentofDocument(titleOfDocumentToBeViewed, CurrentUser);
                                    break;
                                case "5":
                                    Console.WriteLine("Enter the title of the document");
                                    string titleOfDocumentToDelete = Console.ReadLine();
                                    _documentManager.DeleteDocument(titleOfDocumentToDelete, CurrentUser);
                                    break;
                                case "6":
                                    Console.WriteLine("Logged out");
                                    isLoggedIn = false;
                                    break;
                                default:
                                    Console.WriteLine("You entered a wrong input");
                                    break;
                            }
                        }

                        else
                        {
                            Console.WriteLine("Press 1 to Add Document");
                            Console.WriteLine("Press 2 to Search for Document");
                            Console.WriteLine("Press 3 to View all documents of a Department");
                            Console.WriteLine("Press 4 to View all documents");
                            Console.WriteLine("Press 5 to View the content of a document");
                            Console.WriteLine("Press 6 to Delete Document");
                            Console.WriteLine("Press 7 to remove a staff");
                            Console.WriteLine("Press 8 to Display all staff with their details");
                            Console.WriteLine("Press 9 to Display all staff of a department");
                            Console.WriteLine("Press 0 to Logout");

                            string adminChoice = Console.ReadLine();

                            switch (adminChoice)
                            {
                                case "1":
                                    Console.WriteLine("Enter the title of the document");
                                    string title = Console.ReadLine();
                                    Console.WriteLine("Enter the description of the document");
                                    string description = Console.ReadLine();
                                    Console.WriteLine("Enter the content of the document");
                                    string content = Console.ReadLine();

                                    _documentManager.AddDocument(title, description, content, CurrentUser.Department, CurrentUser.FirstName);
                                    break;
                                case "2":
                                    Console.WriteLine("Enter the title of the document");
                                    string titleOfDoc = Console.ReadLine();
                                    _documentManager.SearchDocument(titleOfDoc, CurrentUser.Department);
                                    break;
                                case "3":
                                    Console.WriteLine("Enter the the department you want to view their documents");
                                    string department = Console.ReadLine();
                                    _documentManager.ViewAllDocumentOfADepartment(department);
                                    break;
                                case "4":
                                    _documentManager.ViewAllDocument(CurrentUser.IsAdmin);
                                    break;
                                case "5":
                                    Console.WriteLine("Enter the title of the document");
                                    string titleOfDocumentToBeViewed = Console.ReadLine();
                                    _documentManager.ViewContentofDocument(titleOfDocumentToBeViewed, CurrentUser);
                                    break;
                                case "6":
                                    Console.WriteLine("Enter the title of the document");
                                    string titleOfDocumentToDelete = Console.ReadLine();
                                    _documentManager.DeleteDocument(titleOfDocumentToDelete, CurrentUser);
                                    break;
                                case "7":
                                    Console.WriteLine("Enter the staffNumber of staff to remove");
                                    string staffNumber = Console.ReadLine();
                                    _user.RemoveStaff(staffNumber);
                                    break;
                                case "8":
                                    UserManager.DisplayContentOfSortedDictionary(_user.GetAllStaffDetails());
                                    break;
                                case "9":
                                    Console.WriteLine("Enter a department to view the registered staff");
                                    string departmentToView = Console.ReadLine();
                                    _user.ViewStaffByDepartment(departmentToView);
                                    break;
                                case "0":
                                    Console.WriteLine("Logged out");
                                    isLoggedIn = false;
                                    break;
                                default:
                                    Console.WriteLine("You entered a wrong input");
                                    break;
                            }
                        }
                    }
                }
            }

            void ShowStartMenu()
            {
                foreach (var x in Enum.GetValues(typeof(StartMenu)))
                {
                    Console.WriteLine($"Press {(int)x} to {x}");
                }
                Console.WriteLine();
            }
        }
    }
}
