namespace Entities
{
    public class Album : BaseEntity
    {
        public string Artist { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal Price { get; set; }
        public float Discount { get; set; }
        public bool IsContinued { get; set; }
    }
}
