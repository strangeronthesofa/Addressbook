using Interfaces.Interfaces;
using Interfaces.Models;
using Interfaces.Services;
using Moq;


public class PersonService_Tests
{
    [Fact]
    public void AddPerson_WhenPersonDoesNotExist_ReturnsTrueAndSavesToFile()
    {
        // Arrange
        var fileServicesMock = new Mock<IFileServices>();
        var personService = new PersonService(new List<IPerson>(), fileServicesMock.Object);
        var personToAdd = new Person { FirstName = "Testar", LastName = "Testarsson", Email = "testar@example.com" };

        fileServicesMock.Setup(fs => fs.LoadListFromJson()).Returns(new List<IPerson>());

        // Act
        bool result = personService.AddPerson(personToAdd);

        // Assert
        Assert.True(result);

        fileServicesMock.Verify(fs => fs.SaveListToJson(It.Is<List<IPerson>>(list => list.Count == 1 && list[0].Email == personToAdd.Email)), Times.Once);
    }

    [Fact]
    public void AddPerson_WhenPersonExists_ReturnsFalseAndDoesNotSaveToFile()
    {
        // Arrange
        var existingPerson = new Person { FirstName = "Existing", LastName = "Person", Email = "existing@example.com" };
        var fileServicesMock = new Mock<IFileServices>();
        var personService = new PersonService(new List<IPerson> { existingPerson }, fileServicesMock.Object);
        var personToAdd = new Person { FirstName = "Testar", LastName = "Testarsson", Email = "existing@example.com" };
        fileServicesMock.Setup(fs => fs.LoadListFromJson()).Returns(new List<IPerson> { existingPerson });

        // Act
        bool result = personService.AddPerson(personToAdd);

        // Assert
        Assert.False(result);

        fileServicesMock.Verify(fs => fs.SaveListToJson(It.IsAny<List<IPerson>>()), Times.Never);
    }
}
