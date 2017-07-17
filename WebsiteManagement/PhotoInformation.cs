using System;

namespace WebsiteManagement
{
    public class PhotoInformation
    {
        public string photoId { get; set; }
        public string photoName { get; set; }
        public string photoFileLocation { get; set; }
        public string photoFileName { get; set; }
        public string photoDescription { get; set; }
        public int photoOrderNumber { get; set; }
        public int photoWidth { get; set; }

        public string categoryId { get; set; }
        public string categoryName { get; set; }
    }
}
