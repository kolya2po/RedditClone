namespace Forum.WebApi.Models.Community
{
    public class UpdateCommunityDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string About { get; set; }
        public string CreationDate { get; set; }
    }
}
