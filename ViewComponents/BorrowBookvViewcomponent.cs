using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using WebHackathon.Models;
using WebHackathon.Utilities;
using WebHackathon.ViewModels;

namespace WebHackathon.ViewComponents
{
    [ViewComponent(Name = "BorrowBookvView")]
    public class BorrowBookvViewcomponent : ViewComponent
    {
        private readonly DbHackathonContext _context;
        public BorrowBookvViewcomponent(DbHackathonContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var bookborrow = (from bb in _context.TbBorrows
                              join b in _context.TbBooks on bb.BookId equals b.BookId
                              join u in _context.TbUsers on bb.UserId equals u.UserId
                              join a in _context.TbAuthors on b.AuthorId equals a.AuthorId
                              where u.UserId == Function._userid
                              select new BorrowedBookViewModel
                              {
                                  BorrowID =  bb.BorrowId,
                                  BookTitle = b.Title,
                                  BookID = b.BookId,
                                  Desc = b.Description,
                                  AuthorName = a.Name,
                                  UserName = u.Name,
                                  Image = b.Image,
                              }).ToList();
            return await Task.FromResult<IViewComponentResult>(View(bookborrow));
        }
    
    }
}
