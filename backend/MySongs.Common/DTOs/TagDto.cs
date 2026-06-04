namespace MySongs.Common.DTOs
{
    public class TagDto
    {
        public int TagId { get; set; }
        public string TagName { get; set; }
        public TagType TagType { get; set; } = TagType.General;
    }
}
