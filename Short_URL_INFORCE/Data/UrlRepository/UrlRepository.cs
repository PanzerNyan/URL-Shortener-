using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using Short_URL_INFORCE.Data;
using Short_URL_INFORCE.Models;
using Short_URL_INFORCE.Services;

namespace Short_URL_INFORCE.Data.UrlRepository
{
    public class UrlRepository
    {
        private readonly AppDBContext _context;
        private readonly HashidsService _hashidsService;

        public UrlRepository(AppDBContext context, HashidsService hashidsService)
        {
            _context = context;
            _hashidsService = hashidsService;
        }

        // Get all URL
        public IEnumerable<URL> GetAllUrls()
        {
            return _context.URLs.AsNoTracking().ToList();
        }

        // Get all info about URL
        public URL? GetUrlById(int id)
        {
            return _context.URLs.AsNoTracking().FirstOrDefault(u => u.ID == id);
        }

        // Create new URL
        public URL CreateUrl(string originalUrl, string userId)
        {
            var uri = new Uri(originalUrl);

            var newUrl = new URL
            {
                FullUrl = originalUrl,
                BaseUrl = $"{uri.Scheme}://{uri.Host}",
                PathUrl = uri.PathAndQuery,
                UserID = userId,
                CreationDate = DateTime.Now
            };

            _context.URLs.Add(newUrl);
            _context.SaveChanges(); // Save to get ID

            // Generate Short
            newUrl.ShortUrl = _hashidsService.Encode(newUrl.ID);
            _context.SaveChanges(); // Save with short

            return newUrl;
        }

        // Delete URL
        public bool DeleteUrl(int id, string userID, bool isAdmin)
        {
            var url = _context.URLs.FirstOrDefault(u => u.ID == id);
            if (url == null) return false;

            if (!isAdmin && url.UserID != userID) return false; // Check if authorized

            _context.URLs.Remove(url);
            _context.SaveChanges();
            return true;
        }

        // Get original from short
        public string? GetUrlByShortUrl(string shortUrl)
        {
            return _context.URLs.Where(u => u.ShortUrl == shortUrl)
                                .Select(u => u.FullUrl)
                                .FirstOrDefault();
        }
    }
}
