namespace CourseCatalog.Api.DTOs
{
    public class ClassroomDto
    {
        public int Id { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public int Capacity { get; set; }
    }
}
