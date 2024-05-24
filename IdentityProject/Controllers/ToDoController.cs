using IdentityServiceProject.Dtos;
using IdentityServiceProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ToDoController : ControllerBase
    {
        private readonly ToDoService _service;

        public ToDoController(ToDoService service)
        {
            _service = service;
        }

        [HttpGet("ToDoList")]
        public IResult GetUserToDoList()
        {
            if (User.Identity is { IsAuthenticated: true })
            {
                var user = User.Identity.Name;

                if (user == null)
                {
                    return Results.NotFound();
                }

                var response = _service.GetToDoList(user);
                return Results.Ok(response);
            }

            return Results.NotFound();
        }

        [HttpGet("NotesByTitle")]
        public IResult GetNotesByTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return Results.Problem();
            }

            if (User.Identity is { IsAuthenticated: true })
            {
                var userName = User.Identity.Name;

                if (userName == null)
                {
                    return Results.NotFound();
                }

                var response = _service.GetNotesByTitle(title, userName);
                return Results.Ok(response);
            }

            return Results.NotFound();
        }

        [HttpGet("Note/{Id:int}")]
        public IResult GetUserToDoList(int Id)
        {
            var response = _service.GetNote(Id);
            return Results.Ok(response);
        }

        [HttpDelete("RemoveNote/{Id:int}")]
        public IResult RemoveNote(int Id)
        {
            if (User.Identity is { IsAuthenticated: true })
            {
                var userName = User.Identity.Name;

                if (userName == null)
                {
                    return Results.NotFound();
                }

                var response = _service.RemoveNote(Id, userName);
                return Results.Ok(response);
            }

            return Results.NotFound(Id);
        }

        [HttpPut("EditNote")]
        public IResult EditNote(ToDoUpdateDto editNote)
        {
            var response = _service.UpdateNote(editNote);
            return Results.Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("InsertNote")]
        public IResult InsertNote(ToDoInsertDto newNote)
        {
            if (User.Identity is { IsAuthenticated: true })
            {
                var userName = User.Identity.Name;

                if (userName == null)
                {
                    return Results.NotFound();
                }

                ToDoServiceInsertDto note = new ToDoServiceInsertDto
                {
                    UserName = userName,
                    Title = newNote.Title,
                    DeadlineDateTime = newNote.DeadlineDateTime,
                    Description = newNote.Description,
                };

                var response = _service.InsertNote(note);
                return Results.Ok(response);
            }

            return Results.Problem("Failed to insert new note!!!");
        }
    }
}
