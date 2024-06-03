using IdentityServiceProject.Dtos;
using IdentityServiceProject.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace IdentityProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoService _service;

        public ToDoController(IToDoService service)
        {
            _service = service;
        }

        [HttpGet("ToDoList")]
        public async Task<IActionResult> GetUserToDoList()
        {
            var user = User.Identity.Name;

            if (user == null)
            {
                return NotFound("User does not found!!!");
            }

            var response = await _service.GetToDoList(user);
            if (response.Any())
            {
                return StatusCode(StatusCodes.Status200OK, response);
            }

            return StatusCode(StatusCodes.Status404NotFound, $"User does not have notes.");
        }

        [HttpGet("NotesByTitle")]
        public async Task<IActionResult> GetNotesByTitle(string title)
        {
            var userName = User.Identity.Name;

            if (userName == null)
            {
                return NotFound("User does not found!!!");
            }

            var response = await _service.GetNotesByTitle(title, userName);
            if (response.Any())
            {
                return StatusCode(StatusCodes.Status200OK, response);
            }

            return StatusCode(StatusCodes.Status404NotFound, $"There is no notes under {title} title");
        }

        [HttpGet("Note/{Id:int}")]
        public async Task<IActionResult> GetUserToDoList([Range(0, int.MaxValue)] int Id)
        {
            var userName = User.Identity.Name;

            if (userName == null)
            {
                return NotFound("User does not found!!!");
            }

            ToDoRemoveDto note = new ToDoRemoveDto
            {
                UserName = userName,
                Id = Id
            };
            var response = await _service.GetNote(note);

            if (response == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, $"Note Does not exist!!");
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpDelete("RemoveNote/{Id:int}")]
        public async Task<IActionResult> RemoveNote([Range(0, int.MaxValue)] int Id)
        {
            var userName = User.Identity.Name;

            if (userName == null)
            {
                return NotFound("User does not found!!!");
            }

            ToDoRemoveDto note = new ToDoRemoveDto
            {
                UserName = userName,
                Id = Id
            };
            var response = await _service.RemoveNote(note);

            if (response == -1)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPut("EditNote")]
        public async Task<IActionResult> EditNote(ToDoUpdateDto editNote)
        {
            if (editNote == null || editNote.Id <= 0)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, $"Not acceptable arguments.");
            }

            var userName = User.Identity.Name;

            if (userName == null)
            {
                return NotFound("User does not found!!!");
            }

            editNote.UserName = userName;
            var response = await _service.EditNote(editNote);
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPost("InsertNote")]
        public async Task<IActionResult> InsertNote(ToDoBase newNote)
        {
            var userName = User.Identity.Name;

            if (userName == null)
            {
                return NotFound("User does not found!!!");
            }

            newNote.UserName = userName;
            var response = await _service.InsertNote(newNote);
            if (response == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }
    }
}
