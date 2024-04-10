﻿namespace SchoolAPI.Models
{
    public class Class
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SchoolId { get; set; }
        public School School { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public ICollection<Student> Students { get; set; }
    }
}
