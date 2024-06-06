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
            var response = await _service.GetToDoList();
            if (response.Any())
            {
                return StatusCode(StatusCodes.Status200OK, response);
            }

            return StatusCode(StatusCodes.Status404NotFound, $"User does not have notes.");
        }

        [HttpGet("NotesByTitle")]
        public async Task<IActionResult> GetNotesByTitle(string title)
        {
            var response = await _service.GetNotesByTitle(title);
            if (response.Any())
            {
                return StatusCode(StatusCodes.Status200OK, response);
            }

            return StatusCode(StatusCodes.Status404NotFound, $"There is no notes under {title} title");
        }

        [HttpGet("Note/{Id:int}")]
        public async Task<IActionResult> GetUserToDoList([Range(0, int.MaxValue)] int Id)
        {
            var response = await _service.GetNote(Id);

            if (response == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, $"Note Does not exist!!");
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpDelete("RemoveNote/{Id:int}")]
        public async Task<IActionResult> RemoveNote([Range(0, int.MaxValue)] int Id)
        {
            var response = await _service.RemoveNote(Id);

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

            var response = await _service.EditNote(editNote);
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPost("InsertNote")]
        public async Task<IActionResult> InsertNote(ToDoBase newNote)
        {
            var response = await _service.InsertNote(newNote);
            if (response == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }
    }
}
