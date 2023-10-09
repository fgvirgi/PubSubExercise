using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TeacherPublisherAPI.Domain.Entities;
using TeacherPublisherAPI.Services;

namespace TeacherPublisherAPI.Controllers
{
    [ApiController]
    [Route("teacher")]
    public class TeacherPublisherController : ControllerBase
    {
        private readonly ITeacherPublisherService _service;
        private readonly ILogger<ITeacherPublisherService> _logger;

        public TeacherPublisherController(ITeacherPublisherService service, ILogger<ITeacherPublisherService> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("publish/{targetLanguageCode}")]
        public async Task<ActionResult> Publish([FromBody] Course course, [FromRoute] string targetLanguageCode,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Received: {JsonSerializer.Serialize(course)}");
            try
            {
                await _service.PublishCourse(course, targetLanguageCode, cancellationToken);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unhandled exception while publishing the course '{course.Title}'");
                return StatusCode(500, $"Exception: {ex.Message}");
            }
        }
    }
}