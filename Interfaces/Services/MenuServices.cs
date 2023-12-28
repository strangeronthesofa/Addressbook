using Interfaces.Interfaces;
using Interfaces.Models;



namespace Interfaces.Services;

public class MenuServices
{
    private readonly PersonService _personService;
    private readonly IFileServices _fileServices;
    private readonly List<IPerson> _listOfEntries;

    public MenuServices(List<IPerson> listOfEntries, IFileServices fileServices, PersonService personService)
    {
        _personService = personService;
        _fileServices = fileServices;
        _listOfEntries = listOfEntries;
    }

    public void Menu()
    {
        while (true)
        {
            Console.Clear();
            MenuTitle("Main Menu");
            Console.WriteLine("1. Add person");
            Console.WriteLine("2. Print persons");
            Console.WriteLine("3. Print Specifics");
            Console.WriteLine("4. Delete user");
            Console.WriteLine("0. Quit");
            Console.WriteLine();
            Console.Write("Pick your choice: ");
            string userChoice = Console.ReadLine()!;

            switch (userChoice)
            {
                case "1":
                    AddPerson();
                    break;
                case "2":
                    PrintAllEntries();
                    break;
                case "3":
                    PrintSpecifics();
                    break;
                case "4":
                    RemovePersonByEmail();
                    break;
                case "0":
                    Environment.Exit(0);
                    break;
                default:
                    Console.Clear();
                    Console.Write("Invalid input. \nPlease enter a valid input. \n\nPress Enter to continue...");
                    Console.ReadKey();
                    break;
            }
        }
        
    }

    // Main Methods //
    public void AddPerson()
    {
        Console.Clear();
        MenuTitle("Add a person to the address book");
        string userFirstName = GetUserInput("First name");
        string userLastName = GetUserInput("Last name");
        string userEmail = GetUserInput("Email");
        string userStreetAddress = GetUserInput("Street address");

        IPerson person = CreatePerson(userFirstName, userLastName, userEmail, userStreetAddress);

        DisplayAddPersonResult(person);

        Console.ReadKey();
        Console.Clear();
    }

    private void ClearConsoleAndGetList(out List<IPerson> listOfEntries, out int personIndex)
    {
        listOfEntries = _fileServices.LoadListFromJson() ?? new List<IPerson>();
        personIndex = 0;
        Console.Clear();
    }

    public void PrintAllEntries()
    {
        ClearConsoleAndGetList(out var listOfEntries, out var personIndex);
        Console.Clear();
        MenuTitle("People in your address book");
        if (listOfEntries.Count == 0)
        {
            Console.WriteLine("No entries found.");
        }
        else
        {
            foreach (var person in listOfEntries)
            {
                personIndex++;
                Console.WriteLine($"{personIndex}. {person.FirstName}");
            }
        }

        Console.WriteLine();
        Console.Write("Press Enter to continue...");
        Console.ReadKey();
    }

    public void PrintSpecifics()
    {
        ClearConsoleAndGetList(out var listOfEntries, out var personIndex);

        if (listOfEntries.Count == 0)
        {
            Console.WriteLine("No entries available.");
            Console.Write("Press Enter to continue...");
            Console.ReadKey();
            return;
        }

        MenuTitle("Look up person in address book");
        DisplayPersonList(listOfEntries);

        int userEntry = GetUserInputForEntry();

        if (IsValidEntry(userEntry, listOfEntries))
        {
            DisplayPersonDetails(listOfEntries[userEntry - 1]);
        }
        else
        {
            Console.WriteLine("Invalid entry.");
        }

        Console.Write("Press Enter to continue...");
        Console.ReadKey();
    }

    public void RemovePersonByEmail()
    {
        Console.Clear();
        MenuTitle("Remove person from address book");
        Console.Write("Please enter the e-mail address of the person you want to remove: ");
        string userInput = Console.ReadLine()!;
        bool removalResult = _personService.RemovePerson(userInput);

        if (removalResult)
        {
            Console.WriteLine("Person removed successfully.");
        }
        else
        {
            Console.WriteLine($"Person with email {userInput} not found.");
        }
        Console.ReadKey();
    }

    // Helper Methods //
    private string GetUserInput(string prompt)
    {
        Console.Write($"{prompt}: ");
        return Console.ReadLine()!;
    }

    private IPerson CreatePerson(string firstName, string lastName, string email, string streetAddress)
    {
        return new Person
        {
            FirstName = ModifyString(firstName),
            LastName = ModifyString(lastName),
            Email = email,
            StreetAddress = streetAddress
        };
    }

    private void DisplayAddPersonResult(IPerson person)
    {
        bool addResult = _personService.AddPerson(person);
        Console.Clear();

        if (addResult)
        {
            Console.WriteLine($"{person.FirstName} added successfully.");
        }
        else
        {
            Console.WriteLine($"{person.FirstName} was not added.");
        }
    }

    public string ModifyString(string userInput)
    {
        if (string.IsNullOrEmpty(userInput))
        {
            return string.Empty;
        }

        char firstChar = char.ToUpper(userInput[0]);
        string modifiedName = firstChar + userInput.Substring(1);
        return modifiedName;
    }

    private void DisplayPersonList(List<IPerson> listOfEntries)
    {
        int personIndex = 0;
        foreach (var person in listOfEntries)
        {
            personIndex++;
            Console.WriteLine($"{personIndex}. {person.FirstName}");
        }
    }

    private int GetUserInputForEntry()
    {
        Console.Write("\nPlease choose which entry to print: ");
        string userInput = Console.ReadLine()!;

        if (int.TryParse(userInput, out int result))
        {
            return result;
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid integer.");
            return GetUserInputForEntry();
        }
    }

    private bool IsValidEntry(int userEntry, List<IPerson> listOfEntries)
    {
        return userEntry >= 1 && userEntry <= listOfEntries.Count;
    }

    private void DisplayPersonDetails(IPerson person)
    {
        Console.Clear();
        Console.WriteLine($"Full name:\t {person.FirstName} {person.LastName}");
        Console.WriteLine($"Email Address:\t {person.Email}");
        Console.WriteLine($"Street Address:\t {person.StreetAddress}");
        Console.WriteLine($"Person ID:\t {person.Id}");
        Console.WriteLine();
    }

    public void MenuTitle(string title)
    {
        Console.Clear();
        Console.WriteLine($"## {title} ##");
        Console.WriteLine();
    }
}
