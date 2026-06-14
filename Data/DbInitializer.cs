using System;
using System.Linq;
using System.Threading.Tasks;
using CourseCatalog.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseCatalog.Api.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(CourseDbContext context)
        {
            // Apply migrations automatically if any are pending
            if ((await context.Database.GetPendingMigrationsAsync()).Any())
            {
                await context.Database.MigrateAsync();
            }
            else
            {
                await context.Database.EnsureCreatedAsync();
            }

            // Seed Courses
            if (!await context.Courses.AnyAsync())
            {
                var courses = new Course[]
                {
                    new Course { Name = "C# & .NET Core Developer", Fee = 5000000m, NumberOfSessions = 24, Level = "Beginner" },
                    new Course { Name = "Vue.js 3 Frontend Development", Fee = 4500000m, NumberOfSessions = 18, Level = "Intermediate" },
                    new Course { Name = "Advanced SQL Server & Database Design", Fee = 6000000m, NumberOfSessions = 20, Level = "Advanced" },
                    new Course { Name = "IELTS Academic Listening & Speaking", Fee = 8000000m, NumberOfSessions = 30, Level = "Intermediate" }
                };

                await context.Courses.AddRangeAsync(courses);
                await context.SaveChangesAsync();
            }

            // Seed Classrooms
            if (!await context.Classrooms.AnyAsync())
            {
                var classrooms = new Classroom[]
                {
                    new Classroom { RoomName = "Lab 101", Capacity = 25 },
                    new Classroom { RoomName = "Lab 102", Capacity = 30 },
                    new Classroom { RoomName = "Room 201 (Theory)", Capacity = 40 },
                    new Classroom { RoomName = "Room 202 (Theory)", Capacity = 40 },
                    new Classroom { RoomName = "Hall A", Capacity = 100 }
                };

                await context.Classrooms.AddRangeAsync(classrooms);
                await context.SaveChangesAsync();
            }
        }
    }
}
