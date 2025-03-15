using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims; 
using Microsoft.AspNetCore.Authorization; 
using System; 

using Short_URL_INFORCE.Data.UrlRepository;
using Short_URL_INFORCE.Models;



namespace Short_URL_INFORCE.Controllers
{
    [Route("api/urls")]
    [ApiController]
    public class UrlController : ControllerBase
    {
        private readonly UrlRepository _urlRepository;

        public UrlController(UrlRepository urlRepository)
        {
            _urlRepository = urlRepository;
        }

        

        //Get all Urls
        [HttpGet]
        public ActionResult<List<URL>> GetAllUrls()
        {
            var urls = _urlRepository.GetAllUrls();
            return Ok(urls); 
        }

        //Get by ID
        [HttpGet("{id:int}")]
        public ActionResult<URL> GetUrlById(int id)
        {
            var url = _urlRepository.GetUrlById(id);
            if (url == null)
            {
                return NotFound();
            }
            return url;
        }

        //Create new URL
        [HttpPost]
        //[Authorize] // Only Authorized
        public ActionResult<URL> CreateUrl([FromBody] UrlCreateDto urlDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // GetId from token
            if (userId == null)
            {
                return Unauthorized("User Unauthorized .");
            }

            var newUrl = _urlRepository.CreateUrl(urlDto.OriginalUrl, userId);

            if (newUrl == null)
            {
                return BadRequest("Url Creation error.");
            }

            return CreatedAtAction(nameof(GetUrlById), new { id = newUrl.ID }, newUrl);
        }

        //Delete Selected
        [HttpDelete("{id:int}")]
        [Authorize]
        public ActionResult DeleteUrl(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Get ID from token
            var isAdmin = User.IsInRole("Admin");

            Console.WriteLine($"Delete request: id={id}, userId={userId}, isAdmin={isAdmin}");

            var result = _urlRepository.DeleteUrl(id, userId, isAdmin);

            if (!result)
            {
                return Forbid();
            }

            return Ok(new { message = "URL deleted successfully." });
        }

        //Get full url by short
        [HttpGet("{shortUrl}")]
        public IActionResult RedirectToOriginalUrl([FromRoute] string shortUrl)
        {
            var originalUrl = _urlRepository.GetUrlByShortUrl(shortUrl);

            if (originalUrl == null)
            {
                return NotFound("Short URL not found.");
            }

            return Redirect(originalUrl); 
        }
    }
}
