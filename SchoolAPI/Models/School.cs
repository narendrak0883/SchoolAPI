using FluentValidation;

namespace SchoolAPI.Models
{
    public class School
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Student> Students { get; set; }
        public ICollection<Teacher> Teachers { get; set; }
        public ICollection<Class> Classes { get; set; }

        public class SchoolValidator : AbstractValidator<School>
        {
            public SchoolValidator()
            {
                RuleFor(x => x.Name).NotEmpty().WithMessage("Please specify a name");
            }
        }
    }

}
