using System.Security.Claims;

namespace SchoolAPI.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SchoolId { get; set; }
        public School School { get; set; }
        public ICollection<Class> Classes { get; set; }
    }
}
