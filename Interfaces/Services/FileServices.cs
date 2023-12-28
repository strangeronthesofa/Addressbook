using Interfaces.Interfaces;
using Interfaces.Models;
using System.Diagnostics;
using System.Text.Json;

namespace Interfaces.Services;

public class FileServices : IFileServices
{
    private string _filePath;

    public FileServices(string filePath)
    {
        _filePath = filePath;
    }

    public List<IPerson> LoadListFromJson()
    {
        try
        {
            Debug.WriteLine($"Attempting to load file: {_filePath}");

            string jsonContent = File.ReadAllText(_filePath);
            List<Person> result = JsonSerializer.Deserialize<List<Person>>(jsonContent)!;

            List<IPerson> listOfEntries = result.Cast<IPerson>().ToList();

            Debug.WriteLine($"Successfully loaded {_filePath}. Number of entries: {listOfEntries.Count}");

            foreach (var person in listOfEntries)
            {
                Debug.WriteLine($"Id: {person.Id}, FirstName: {person.FirstName}, LastName: {person.LastName}, Email: {person.Email}");
            }

            return listOfEntries;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading file {_filePath}: {ex}");
            return null!;
        }
    }

    public void SaveListToJson(List<IPerson> listOfEntries)
    {
        try
        {
            if (!string.IsNullOrEmpty(_filePath))
            {
                using (StreamWriter writer = new StreamWriter(_filePath, false))
                {
                    string jsonContent = JsonSerializer.Serialize(listOfEntries);
                    writer.Write(jsonContent);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error saving file: {ex.Message}");
        }
    }


}
