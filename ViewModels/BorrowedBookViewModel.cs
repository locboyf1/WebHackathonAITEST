namespace WebHackathon.ViewModels
{
    public class BorrowedBookViewModel
    {
        public int BorrowID { get; set; }
        public int BookID { get; set; }
        public string BookTitle { get; set; }
        public string Desc { get; set; }    
        public string AuthorName { get; set; }
        public string UserName { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string? Image {  get; set; } 
    }
}
