using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using SchoolAPI.Controllers;
using SchoolAPI.EFCore;
using SchoolAPI.Models;

namespace SchoolAPI.Tests.Unit;

[TestFixture]
public class StudentControllerTests
{
    private Mock<DbSet<Student>> _mockSet;
    private Mock<SchoolContext> _mockContext;
    private StudentController _controller;
    private List<Student> _students;

    [SetUp]
    public void SetUp()
    {
        _students = new List<Student>
        {
            new Student { Id = 1, Name = "Student 1" },
            new Student { Id = 2, Name = "Student 2" }
        };

        _mockSet = new Mock<DbSet<Student>>();
        _mockSet.As<IQueryable<Student>>().Setup(m => m.Provider).Returns(_students.AsQueryable().Provider);
        _mockSet.As<IQueryable<Student>>().Setup(m => m.Expression).Returns(_students.AsQueryable().Expression);
        _mockSet.As<IQueryable<Student>>().Setup(m => m.ElementType).Returns(_students.AsQueryable().ElementType);
        _mockSet.As<IQueryable<Student>>().Setup(m => m.GetEnumerator()).Returns(_students.AsQueryable().GetEnumerator());

        _mockContext = new Mock<SchoolContext>();
        _mockContext.Setup(c => c.Students).Returns(_mockSet.Object);

        _controller = new StudentController(_mockContext.Object);
    }

    [Test]
    public async Task GetStudents_ReturnsAllStudents()
    {
        var result = await _controller.GetStudents();
        Assert.AreEqual(2, result.Value.Count());
    }

    [Test]
    public async Task PostStudent_AddsStudent_To_DbSet()
    {
        await _controller.PostStudent(new Student { Id = 3, Name = "Student 3" });
        _mockSet.Verify(m => m.Add(It.IsAny<Student>()), Times.Once());
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    public async Task PutStudent_Updates_Student_In_DbSet()
    {
        await _controller.PutStudent(2, new Student { Id = 2, Name = "Updated Student" });
        _mockContext.Verify(m => m.Entry(It.IsAny<Student>()), Times.Once());
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    public async Task DeleteStudent_Removes_Student_From_DbSet()
    {
        await _controller.DeleteStudent(1);
        _mockSet.Verify(m => m.Remove(It.IsAny<Student>()), Times.Once());
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    public void PutStudnt_NonExistentId_ThrowsNotFound()
    {
        var result = _controller.PutStudent(99, new Student { Id = 99, Name = "NonExistent Student" }).Result;
        Assert.IsInstanceOf<NotFoundResult>(result);
    }

    [Test]
    public void DeleteStudent_NonExistentId_ThrowsNotFound()
    {
        var result = _controller.DeleteStudent(99).Result;
        Assert.IsInstanceOf<NotFoundResult>(result);
    }

    [Test]
    public async Task PostStudent_Null_ThrowsBadRequest()
    {
        var result = await _controller.PostStudent(null);
        Assert.IsInstanceOf<BadRequestResult>(result.Result);
    }

    [Test]
    public async Task PutStudent_IdDoesNotMatch_ThrowsBadRequest()
    {
        var result = await _controller.PutStudent(99, new Student { Id = 1, Name = "Existing Student" });
        Assert.IsInstanceOf<BadRequestResult>(result);
    }

    [Test]
    public async Task PutStudent_Null_ThrowsBadRequest()
    {
        var result = await _controller.PutStudent(1, null);
        Assert.IsInstanceOf<BadRequestResult>(result);
    }

    [Test]
    public async Task PutStudent_NonExistentId_ThrowsNotFound()
    {
        var result = await _controller.PutStudent(99, new Student { Id = 99, Name = "NonExistent Student" });
        Assert.IsInstanceOf<NotFoundResult>(result);
    }

}