using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using SchoolAPI.Controllers;
using SchoolAPI.EFCore;
using SchoolAPI.Models;

namespace SchoolAPI.Tests.Unit;

[TestFixture]
public class TeacherControllerTests
{
    private Mock<DbSet<Teacher>> _mockSet;
    private Mock<SchoolContext> _mockContext;
    private TeacherController _controller;
    private List<Teacher> _teachers;

    [SetUp]
    public void SetUp()
    {
        _teachers = new List<Teacher>
        {
            new Teacher { Id = 1, Name = "Teacher 1" },
            new Teacher { Id = 2, Name = "Teacher 2" },
        };

        _mockSet = new Mock<DbSet<Teacher>>();
        _mockSet.As<IQueryable<Teacher>>().Setup(m => m.Provider).Returns(_teachers.AsQueryable().Provider);
        _mockSet.As<IQueryable<Teacher>>().Setup(m => m.Expression).Returns(_teachers.AsQueryable().Expression);
        _mockSet.As<IQueryable<Teacher>>().Setup(m => m.ElementType).Returns(_teachers.AsQueryable().ElementType);
        _mockSet.As<IQueryable<Teacher>>().Setup(m => m.GetEnumerator()).Returns(_teachers.AsQueryable().GetEnumerator());

        _mockContext = new Mock<SchoolContext>();
        _mockContext.Setup(c => c.Teachers).Returns(_mockSet.Object);

        _controller = new TeacherController(_mockContext.Object);
    }

    [Test]
    public async Task GetTeachers_ReturnsAllTeachers()
    {
        var result = await _controller.GetTeachers();
        Assert.AreEqual(2, result.Value.Count());
    }

    [Test]
    public async Task DeleteTeacher_RemovesTeacher_From_DbSet()
    {
        await _controller.DeleteTeacher(1);
        _mockSet.Verify(m => m.Remove(It.IsAny<Teacher>()), Times.Once());
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    public async Task PostTeacher_AddsTeacher_To_DbSet()
    {
        await _controller.PostTeacher(new Teacher { Id = 3, Name = "Teacher 3" });
        _mockSet.Verify(m => m.Add(It.IsAny<Teacher>()), Times.Once());
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    public async Task PutTeacher_Updates_Teacher_In_DbSet()
    {
        await _controller.PutTeacher(2, new Teacher { Id = 2, Name = "Updated Teacher" });
        _mockContext.Verify(m => m.Entry(It.IsAny<Teacher>()), Times.Once());
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }
}
