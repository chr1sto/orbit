using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Orbit.Application.ViewModels
{
    public class NewsPostViewModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "A Title/Caption is Required")]
        [MinLength(5)]
        [MaxLength(120)]
        [DisplayName("Title")]
        public string Caption { get; set; }
        public string Content { get; set; }
        public string ImageUrlSmallTile { get; set; }
        public string ImageUrlBigTile { get; set; }
        public string ImageUrlBanner { get; set; }
        public string ForumPostUrl { get; set; }
        public string Tags { get; set; }
        public bool Public { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
