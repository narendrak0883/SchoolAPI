using Microsoft.AspNetCore.Mvc;
using Moq;
using SchoolAPI.Controllers;
using SchoolAPI.Models;
using SchoolAPI.Services;

namespace SchoolAPI.Tests.Unit;

[TestFixture]
public class SchoolControllerTests
{
    private SchoolController _controller;
    private Mock<ISchoolService> _schoolServiceMock;
    private List<School> _schools;

    [SetUp]
    public void Setup()
    {
        _schools = new List<School>
        {
            new School { Id = 1, Name = "School 1"},
            new School { Id = 2, Name = "School 2"}
        };

        _schoolServiceMock = new Mock<ISchoolService>();

        _schoolServiceMock.Setup(service => service.GetSchools()).ReturnsAsync(_schools);

        _schoolServiceMock.Setup(service => service.GetSchool(It.IsAny<int>())).ReturnsAsync((int id) => _schools.First(s => s.Id == id));

        _controller = new SchoolController(_schoolServiceMock.Object);
    }

    [Test]
    public async Task GetSchools_ReturnsCorrectNumberOfSchools()
    {
        var actionResult = await _controller.GetSchools();
        var result = actionResult.Result as OkObjectResult;

        Assert.IsNotNull(result);
        var schools = result.Value as List<School>;

        Assert.That(schools, Is.TypeOf<List<School>>());
        Assert.That(schools.Count, Is.EqualTo(2));
    }

    [Test]
    public void GetSchool_ReturnsCorrectSchool()
    {
        var result = _controller.GetSchool(1).Result;

        // Check that the result is a School and has correct Id
        Assert.That(result.Value, Is.TypeOf<School>());
        Assert.That(result.Value.Id, Is.EqualTo(1));
    }

    [Test]
    public void GetSchool_ReturnsNotFound_ForInvalidId()
    {
        // Assuming there is no school with ID 999
        _schoolServiceMock.Setup(service => service.GetSchool(It.IsAny<int>())).ReturnsAsync((School)null);

        var result = _controller.GetSchool(999).Result;

        // Check that the result is NotFound
        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
    }
}

