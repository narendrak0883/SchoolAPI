using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using SchoolAPI.Controllers;
using SchoolAPI.EFCore;
using SchoolAPI.Models;
using SchoolAPI.Services;

namespace SchoolAPI.Tests.Unit;

[TestFixture]
public class TeacherControllerTests
{
    private Mock<ITeacherService> _teacherServiceMock;
    private TeacherController _controller;
    private List<Teacher> _teachers;

    [SetUp]
    public void SetUp()
    {
        _teachers = new List<Teacher>
        {
            new Teacher { Id = 1, Name = "Teacher 1" },
            new Teacher { Id = 2, Name = "Teacher 2" }
        };

        _teacherServiceMock = new Mock<ITeacherService>();
        _teacherServiceMock.Setup(service => service.GetTeachers()).ReturnsAsync(_teachers);
        _teacherServiceMock.Setup(service => service.TeacherExists(It.IsAny<int>())).Returns<int>((id) => _teachers.Exists(t => t.Id == id));
        _teacherServiceMock.Setup(service => service.AddTeacher(It.IsAny<Teacher>())).ReturnsAsync((Teacher t) => { _teachers.Add(t); return t; });
        _teacherServiceMock.Setup(service => service.UpdateTeacher(It.IsAny<int>(), It.IsAny<Teacher>())).Callback<int, Teacher>((id, t) => { _teachers[_teachers.FindIndex(teacher => teacher.Id == id)] = t; });
        _teacherServiceMock.Setup(service => service.DeleteTeacher(It.IsAny<int>())).Callback<int>((id) => { _teachers.Remove(_teachers.Find(teacher => teacher.Id == id)); });

        _controller = new TeacherController(_teacherServiceMock.Object);
    }

    [Test]
    public async Task GetTeachers_ReturnsAllTeachers()
    {
        var result = await _controller.GetTeachers();
        var teachers = ((result.Result as OkObjectResult).Value as IEnumerable<Teacher>);

        Assert.AreEqual(2, teachers.Count());
    }

    [Test]
    public async Task PostTeacher_AddsTeacher_To_DbSet()
    {
        var newTeacher = new Teacher { Id = 3, Name = "Teacher 3" };
        await _controller.PostTeacher(newTeacher);

        _teacherServiceMock.Verify(s => s.AddTeacher(It.Is<Teacher>(x => x.Id == newTeacher.Id)), Times.Once);
    }

    [Test]
    public async Task PutTeacher_Updates_Teacher_In_DbSet()
    {
        var teacherToUpdate = new Teacher { Id = 2, Name = "Updated Teacher" };
        await _controller.PutTeacher(2, teacherToUpdate);

        _teacherServiceMock.Verify(s => s.UpdateTeacher(It.Is<int>(id => id == teacherToUpdate.Id), It.Is<Teacher>(x => x.Name == teacherToUpdate.Name)), Times.Once);
    }

    [Test]
    public async Task DeleteTeacher_RemovesTeacher_From_DbSet()
    {
        await _controller.DeleteTeacher(1);

        _teacherServiceMock.Verify(s => s.DeleteTeacher(It.Is<int>(id => id == 1)), Times.Once);
    }

}
