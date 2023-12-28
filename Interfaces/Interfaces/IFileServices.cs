namespace Interfaces.Interfaces;

public interface IFileServices
{
    List<IPerson> LoadListFromJson();
    void SaveListToJson(List<IPerson> listOfEntries);
}
