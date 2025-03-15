using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Short_URL_INFORCE.Data;
using Short_URL_INFORCE.Models;

namespace Short_URL_INFORCE.Controllers
{
    [Route("api/about")]
    [ApiController]
    public class AboutController : ControllerBase
    {
        private readonly AppDBContext _context;

        public AboutController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAboutText()
        {
            var aboutText = _context.About.FirstOrDefault();
            return Ok(new { text = aboutText?.Text ?? "Default about text." });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateAboutText([FromBody] About about)
        {
            var existingText = _context.About.FirstOrDefault();
            if (existingText != null)
            {
                existingText.Text = about.Text;
            }
            else
            {
                _context.About.Add(new About { Text = about.Text });
            }

            _context.SaveChanges();
            return Ok();
        }
    }
}