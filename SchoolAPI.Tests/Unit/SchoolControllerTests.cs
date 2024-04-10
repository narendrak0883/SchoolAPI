using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using SchoolAPI.Controllers;
using SchoolAPI.EFCore;
using SchoolAPI.Models;

namespace SchoolAPI.Tests.Unit;

[TestFixture]
public class SchoolControllerTests
{
    private SchoolController _controller;
    private Mock<SchoolContext> _mockContext;
    private List<School> _schools;

    [SetUp]
    public void Setup()
    {
        _mockContext = new Mock<SchoolContext>();
        _schools = new List<School>
        {
            new School { Id = 1, Name = "School 1"},
            new School { Id = 2, Name = "School 2"}
        };

        _mockContext.Setup(m => m.Schools).Returns((DbSet<School>)_schools.AsQueryable());
        _controller = new SchoolController(_mockContext.Object);
    }

    [Test]
    public void GetSchools_ReturnsCorrectNumberOfSchools()
    {
        var result = _controller.GetSchools();

        // Check that the result is a list of Schools and has correct count
        Assert.That(result.Result.Value, Is.TypeOf<List<School>>());
        Assert.That(result.Result.Value.Count, Is.EqualTo(2));
    }

    [Test]
    public void GetSchool_ReturnsCorrectSchool()
    {
        var result = _controller.GetSchool(1);

        // Check that the result is a School and has correct Id
        Assert.That(result.Result.Value, Is.TypeOf<School>());
        Assert.That(result.Result.Value.Id, Is.EqualTo(1));
    }

    [Test]
    public void GetSchool_ReturnsNotFound_ForInvalidId()
    {
        var result = _controller.GetSchool(999);

        // Check that the result is NotFound
        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
    }
}