using Microsoft.AspNetCore.Mvc;
using Moq;
using SchoolAPI.Controllers;
using SchoolAPI.Models;
using SchoolAPI.Services;

namespace SchoolAPI.Tests.Unit;

[TestFixture]
public class ClassControllerTests
{
    private Mock<IClassService> _classServiceMock;
    private ClassController _controller;
    private List<Class> _classes;

    [SetUp]
    public void SetUp()
    {
        _classes = new List<Class>
        {
            new Class { Id = 1, Name = "Class 1" },
            new Class { Id = 2, Name = "Class 2" }
        };

        _classServiceMock = new Mock<IClassService>();
        _classServiceMock.Setup(x => x.GetClasses()).ReturnsAsync(_classes);
        _classServiceMock.Setup(x => x.ClassExists(It.IsAny<int>())).Returns<int>((id) => _classes.Exists(c => c.Id == id));
        _classServiceMock.Setup(x => x.AddClass(It.IsAny<Class>())).ReturnsAsync((Class c) => {
            _classes.Add(c); return c;
        });
        _classServiceMock.Setup(x => x.UpdateClass(It.IsAny<int>(), It.IsAny<Class>())).ReturnsAsync((int id, Class c) => {
            var classToUpdate = _classes.First(c => c.Id == id);
            classToUpdate = c;
            return classToUpdate;
        });
        _classServiceMock.Setup(x => x.DeleteClass(It.IsAny<int>())).Returns(Task.CompletedTask);

        _controller = new ClassController(_classServiceMock.Object);
    }

    [Test]
    public async Task GetClasses_ReturnsAllClasses()
    {
        var result = await _controller.GetClasses(); 
        var classes = ((result.Result as OkObjectResult).Value as IEnumerable<Class>);

        Assert.AreEqual(2, classes.Count());
    }

    [Test]
    public async Task PostClass_AddsClass_To_DbSet()
    {
        var newClass = new Class { Id = 3, Name = "Class 3" };
        await _controller.PostClass(newClass);

        _classServiceMock.Verify(s => s.AddClass(It.Is<Class>(x => x.Id == newClass.Id)), Times.Once);
    }

    [Test]
    public async Task PutClass_Updates_Class_In_DbSet()
    {
        var classToUpdate = new Class { Id = 2, Name = "Updated Class" };
        await _controller.PutClass(2, classToUpdate);

        _classServiceMock.Verify(s => s.UpdateClass(It.Is<int>(id => id == classToUpdate.Id), It.Is<Class>(x => x.Name == classToUpdate.Name)), Times.Once);
    }

    [Test]
    public async Task DeleteClass_Removes_Class_From_DbSet()
    {
        await _controller.DeleteClass(1);

        _classServiceMock.Verify(s => s.DeleteClass(It.Is<int>(id => id == 1)), Times.Once);
    }

    [Test]
    public void PutClass_NonExistentId_ReturnsNotFound()
    {
        var result = _controller.PutClass(99, new Class { Id = 99, Name = "NonExistent Class" }).Result;

        Assert.IsInstanceOf<NotFoundResult>(result);
    }

    [Test]
    public void DeleteClass_NonExistentId_ReturnsNotFound()
    {
        var result = _controller.DeleteClass(99).Result;

        Assert.IsInstanceOf<NotFoundResult>(result);
    }
}
