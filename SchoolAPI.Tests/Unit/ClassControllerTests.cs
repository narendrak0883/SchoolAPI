using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using SchoolAPI.Controllers;
using SchoolAPI.EFCore;
using SchoolAPI.Models;

namespace SchoolAPI.Tests.Unit;

[TestFixture]
public class ClassControllerTests
{
    private Mock<DbSet<Class>> _mockSet;
    private Mock<SchoolContext> _mockContext;
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

        _mockSet = new Mock<DbSet<Class>>();
        _mockSet.As<IQueryable<Class>>().Setup(m => m.Provider).Returns(_classes.AsQueryable().Provider);
        _mockSet.As<IQueryable<Class>>().Setup(m => m.Expression).Returns(_classes.AsQueryable().Expression);
        _mockSet.As<IQueryable<Class>>().Setup(m => m.ElementType).Returns(_classes.AsQueryable().ElementType);
        _mockSet.As<IQueryable<Class>>().Setup(m => m.GetEnumerator())
            .Returns(_classes.AsQueryable().GetEnumerator());

        _mockContext = new Mock<SchoolContext>();
        _mockContext.Setup(c => c.Classes).Returns(_mockSet.Object);

        _controller = new ClassController(_mockContext.Object);
    }

    [Test]
    public async Task GetClasses_ReturnsAllClasses()
    {
        var result = await _controller.GetClasses();
        Assert.AreEqual(2, result.Value.Count());
    }

    [Test]
    public async Task PostClass_AddsClass_To_DbSet()
    {
        await _controller.PostClass(new Class { Id = 3, Name = "Class 3" });
        _mockSet.Verify(m => m.Add(It.IsAny<Class>()), Times.Once());
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    public async Task PutClass_Updates_Class_In_DbSet()
    {
        await _controller.PutClass(2, new Class { Id = 2, Name = "Updated Class" });
        _mockContext.Verify(m => m.Entry(It.IsAny<Class>()), Times.Once());
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    public async Task DeleteClass_Removes_Class_From_DbSet()
    {
        await _controller.DeleteClass(1);
        _mockSet.Verify(m => m.Remove(It.IsAny<Class>()), Times.Once());
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }
    [Test]
    public void PutClass_NonExistentId_ThrowsNotFound()
    {
        var result = _controller.PutClass(99, new Class { Id = 99, Name = "NonExistent Class" }).Result;
        Assert.IsInstanceOf<NotFoundResult>(result);
    }

    [Test]
    public void DeleteClass_NonExistentId_ThrowsNotFound()
    {
        var result = _controller.DeleteClass(99).Result;
        Assert.IsInstanceOf<NotFoundResult>(result);
    }


}