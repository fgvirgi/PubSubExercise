using System.Net.Http.Headers;
using System.Net.Http.Json;
using TeacherPublishingDemo;

Console.WriteLine("Exemplify multiple publisher teachers!\n");

try
{
    const string apiUrl = "https://localhost:7033/";
    HttpClient client = new HttpClient();

    client.BaseAddress = new Uri(apiUrl);
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));

    Console.Title = "Teacher publishing demo";

    CancellationTokenSource cancelSource = new CancellationTokenSource();
    await ExemplifyMultipleTeachersPublishingAsync(client, cancelSource.Token);

    Console.WriteLine("\nPress Ctrl-C to exit!");
    Console.ReadKey();
}
catch (Exception ex)
{
    Console.WriteLine($"Exception occurred: {ex.Message}\n");
    Console.WriteLine("Press Ctrl-C to exit!");
    Console.ReadKey();
}

async Task ExemplifyMultipleTeachersPublishingAsync(HttpClient client, CancellationToken token)
{
    var teacherIds = Enumerable.Range(1, 10);

    ParallelOptions parallelOptions = new() { MaxDegreeOfParallelism = 5 };
    await Parallel.ForEachAsync(teacherIds, parallelOptions, async (teacherId, t1) =>
    {
        await Task.Delay(10000, token);

        var courses = TeacherDataProvider.GetRandomCourses(teacherId);
        foreach (var course in courses)
        {
            await Task.Delay(1000, token);

            try
            {
                Console.WriteLine($"{course.Teacher}: {course.Domain} : {course.Module} >> ... PUBLISHING ... {course.Title}");
                HttpResponseMessage response = await client.PostAsJsonAsync(
                    "teacher/publish/en", course);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{course.Teacher}: {course.Domain} : {course.Module} >> ... Exception occurred: {ex.Message}\n");
            }
        }
    });
}


