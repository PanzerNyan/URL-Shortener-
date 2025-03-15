using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;


namespace Short_URL_INFORCE.Models
{
    public class URL
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string FullUrl { get; set; } // full link like link.com/title/page
        public string BaseUrl { get; set; } // only link.com
        public string PathUrl { get; set; } // only /title/page
        public string? ShortUrl { get; set; }//short version like /ttlpg


        public string? Description { get; set; }

        public DateTime CreationDate { get; set; }

        public string UserID { get; set; }

        
        public IdentityUser UrlUser { get; set; }

    }
}
