using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IST_Submission_Form.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Novell.Directory.Ldap;
using System.Linq;

namespace IST_Submission_Form.Pages
{
    public class RProjectDetails : PageModel
    {
        [BindProperty]
        public Submission Submission { get; set; }
        [BindProperty]
        public List<Comment> RequesterComments { get; set; }
        public List<Comment> DeveloperComments { get; set; }
        private readonly SubmissionContext _SubmissionContext;
        private readonly StaffDirectoryContext _StaffDirectory;

        public RProjectDetails(SubmissionContext SubmissionContext, StaffDirectoryContext StaffDirectory)
        {
            _SubmissionContext = SubmissionContext;
            _StaffDirectory = StaffDirectory;
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Submission = await _SubmissionContext.Submissions.FirstOrDefaultAsync(m => m.ID == id);
            RequesterComments = await _SubmissionContext.Comments.Where(c => c.CreatedByID == Submission.RequesterID && c.SubmissionID == Submission.ID).ToListAsync();

            if (Submission == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int ID, string Body)
        {
            if (!ModelState.IsValid)
                return Page();
            
            var name = _StaffDirectory.Staff.AsNoTracking().Where(s => s.LoginID == User.FindFirst("username").Value).First();
            Comment Comment = new Comment();

            // Adding values to fields automatically. These fields are not on the form for users to see and update.
            Comment.SubmissionID = Submission.ID;
            Comment.CreatedByName = name.FName + " " + name.LName;
            Comment.Body = Body;
            Comment.CreatedByID = name.EmployeeID;
            Comment.CreatedAt = DateTime.Now;
            _SubmissionContext.Comments.Add(Comment);

            await _SubmissionContext.SaveChangesAsync();

            return RedirectToPage("RProjectDetails", new { ID = ID });
        }
    }
}