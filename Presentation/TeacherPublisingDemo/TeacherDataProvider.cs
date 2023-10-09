namespace TeacherPublishingDemo
{
    //Remark: this is just a dummy data provider
    internal static class TeacherDataProvider
    {
        private static readonly Dictionary<string, string[]> AllModules = new();
        private static readonly string[] Domains;

        static TeacherDataProvider()
        {
            AllModules.Add(
                "Computing and maths",
                new[]{"Artificial Intelligence Concepts",
                    "Step into Maths", "Machine Learning",
                    "Python Programming", "Generative AI", "C# Programming"

                });
            AllModules.Add(
                "Philosophy",
                new[] { "Thinking About Science", "Philosophy and Art", "Living in a Material World" });

            Domains = AllModules.Keys.ToArray();
        }

        public static IEnumerable<Course> GetRandomCourses(int teacherId)
        {
            var domain = GetRandomDomain(teacherId);
            var modulesInDomain = AllModules[domain];

            var start = teacherId % modulesInDomain.Length;
            var modulesToTeach = modulesInDomain.Skip(start).Take(2);

            List<Course> courses = new List<Course>();
            foreach (var module in modulesToTeach)
            {
                courses.AddRange(
                    Enumerable.Range(1, 3).Select(part => new Course
                    {
                        Teacher = $"Teacher_{teacherId}",
                        Domain = domain,
                        Module = module,
                        Title = $"{module}: PART {part}",
                        Content = $"{module}: PART {part}"
                    })
                );
            }
            return courses;
        }

        private static string GetRandomDomain(int teacherId)
        {
            return Domains[teacherId % Domains.Length];
        }
    }
}
