namespace library.Models
{
    public class Book
    {
        public long BookId { get; set; }          // book_id   BIGINT, the primary key
        public string Title { get; set; } = "";   // title     VARCHAR(150) NOT NULL
        public string? Category { get; set; }     // category  VARCHAR(40), may be NULL
        public decimal? Price { get; set; }       // price     NUMERIC(7,2), may be NULL

    }
}
