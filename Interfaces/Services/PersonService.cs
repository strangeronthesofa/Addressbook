using Interfaces.Interfaces;
using System.Diagnostics;

namespace Interfaces.Services;

public class PersonService
{
    private readonly List<IPerson> _listOfEntries;
    private readonly IFileServices _fileServices;

    public PersonService(List<IPerson> listOfEntries, IFileServices fileServices)
    {
        _listOfEntries = listOfEntries ?? new List<IPerson>();
        _fileServices = fileServices;
    }

    public bool AddPerson(IPerson person)
    {
        try
        {
            if (!_listOfEntries.Any(x => x.Email == person.Email))
            {
                _listOfEntries.Add(person);
                _fileServices.SaveListToJson(_listOfEntries);
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex) 
        {
            Debug.WriteLine(ex);
            return false;
        }
    }

    public List<IPerson> GetListOfEntries()
    {
        return _listOfEntries;
    }

    public bool RemovePerson(string email)
    {
        IPerson personToRemove = _listOfEntries.FirstOrDefault(person => person.Email == email)!;
        if (personToRemove != null)
        {
            _listOfEntries.Remove(personToRemove);
            _fileServices.SaveListToJson(_listOfEntries);
            return true;
        }
        else
        {
            return false;
        }
    }

}
