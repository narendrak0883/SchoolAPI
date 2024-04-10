using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using SchoolAPI.Controllers;
using SchoolAPI.EFCore;
using SchoolAPI.Models;
using SchoolAPI.Services;

namespace SchoolAPI.Tests.Unit;

[TestFixture]
public class StudentControllerTests
{
    private Mock<IStudentService> _studentServiceMock;
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

        _studentServiceMock = new Mock<IStudentService>();
        _studentServiceMock.Setup(service => service.GetStudents()).ReturnsAsync(_students);
        _studentServiceMock.Setup(service => service.AddStudent(It.IsAny<Student>())).ReturnsAsync((Student s) => { _students.Add(s); return s; });
        _studentServiceMock.Setup(service => service.UpdateStudent(It.IsAny<int>(), It.IsAny<Student>())).Callback<int, Student>((id, s) => { _students[_students.FindIndex(student => student.Id == id)] = s; });
        _studentServiceMock.Setup(service => service.DeleteStudent(It.IsAny<int>())).Callback<int>((id) => { _students.Remove(_students.Find(student => student.Id == id)); });
        _studentServiceMock.Setup(service => service.StudentExists(It.IsAny<int>())).Returns<int>((id) => _students.Exists(s => s.Id == id));

        _controller = new StudentController(_studentServiceMock.Object);
    }

    [Test]
    public async Task GetStudents_ReturnsAllStudents()
    {
        var result = await _controller.GetStudents();
        var students = ((result.Result as OkObjectResult).Value as IEnumerable<Student>);

        Assert.AreEqual(2, students.Count());
    }

    [Test]
    public async Task PostStudent_AddsStudent_To_List()
    {
        var newStudent = new Student { Id = 3, Name = "Student 3" };
        await _controller.PostStudent(newStudent);

        _studentServiceMock.Verify(s => s.AddStudent(It.Is<Student>(x => x.Id == newStudent.Id)), Times.Once);
    }

    [Test]
    public async Task PutStudent_Updates_Student_In_List()
    {
        var studentToUpdate = new Student { Id = 2, Name = "Updated Student" };
        await _controller.PutStudent(2, studentToUpdate);

        _studentServiceMock.Verify(s => s.UpdateStudent(It.Is<int>(id => id == studentToUpdate.Id), It.Is<Student>(x => x.Name == studentToUpdate.Name)), Times.Once);
    }

    [Test]
    public async Task DeleteStudent_Removes_Student_From_List()
    {
        await _controller.DeleteStudent(1);

        _studentServiceMock.Verify(s => s.DeleteStudent(It.Is<int>(id => id == 1)), Times.Once);
    }
}
