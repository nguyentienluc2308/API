namespace CourseCatalog.Api.DTOs
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Fee { get; set; }
        public int NumberOfSessions { get; set; }
        public string Level { get; set; } = string.Empty;
    }
}
