namespace Library.Domain.Models
{
    public class Book
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string Title { get; set; }
        public Author Author { get; set; }
    }
}