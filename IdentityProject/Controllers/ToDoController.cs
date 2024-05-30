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
            if (User.Identity is { IsAuthenticated: true })
            {
                var user = User.Identity.Name;

                if (user == null)
                {
                    return NotFound("User does not found!!!");
                }

                var response = await _service.GetToDoList(user);
                if (response.Any())
                {
                    return Ok(response);
                }

                return StatusCode(StatusCodes.Status404NotFound, $"User does not have notes.");
            }

            return StatusCode(StatusCodes.Status401Unauthorized);
        }

        [HttpGet("NotesByTitle")]
        public async Task<IActionResult> GetNotesByTitle(string title)
        {
            if (User.Identity is { IsAuthenticated: true })
            {
                var userName = User.Identity.Name;

                if (userName == null)
                {
                    return NotFound("User does not found!!!");
                }

                var response = await _service.GetNotesByTitle(title, userName);
                if (response.Any())
                {
                    return Ok(response);
                }

                return StatusCode(StatusCodes.Status404NotFound, $"There is no notes under {title} title");
            }

            return StatusCode(StatusCodes.Status401Unauthorized);
        }

        [HttpGet("Note/{Id:int}")]
        public async Task<IActionResult> GetUserToDoList([Range(0, int.MaxValue)] int Id)
        {
            if (User.Identity is { IsAuthenticated: true })
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

                return Ok(response);
            }

            return StatusCode(StatusCodes.Status401Unauthorized);
        }

        [HttpDelete("RemoveNote/{Id:int}")]
        public async Task<IActionResult> RemoveNote([Range(0, int.MaxValue)] int Id)
        {
            if (User.Identity is { IsAuthenticated: true })
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

                return Ok(response);
            }

            return StatusCode(StatusCodes.Status401Unauthorized);
        }

        [HttpPut("EditNote")]
        public async Task<IActionResult> EditNote(ToDoUpdateDto editNote)
        {
            if (editNote == null || editNote.Id <= 0)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, $"Not acceptable arguments.");
            }

            if (User.Identity is { IsAuthenticated: true })
            {
                var userName = User.Identity.Name;

                if (userName == null)
                {
                    return NotFound("User does not found!!!");
                }

                editNote.UserName = userName;
                var response = await _service.EditNote(editNote);
                return Ok(response);
            }

            return StatusCode(StatusCodes.Status401Unauthorized);
        }

        [HttpPost("InsertNote")]
        public async Task<IActionResult> InsertNote(ToDoBase newNote)
        {
            if (User.Identity is { IsAuthenticated: true })
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

                return Ok(response);
            }

            return StatusCode(StatusCodes.Status401Unauthorized);
        }
    }
}
