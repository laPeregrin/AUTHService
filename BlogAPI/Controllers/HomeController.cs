using BlogAPI.Models.Wraps;
using DataBaseStaff.Models;
using DataBaseStaff.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly BllLayer _service;

        public HomeController(BllLayer service)
        {
            _service = service;
        }

        [Authorize]
        public async Task<string> Index()
        {
            string id = HttpContext.User.FindFirstValue("Id");
            string role = HttpContext.User.FindFirstValue(ClaimTypes.Role);
            string name = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            string email = HttpContext.User.FindFirstValue(ClaimTypes.Email);

            return "TestMethod";
        }

        [Route("AddPost"), HttpPost, Authorize(Roles = "Poster, Admin")]
        public async Task<UserResponce> AddPost([FromBody] UserRequest userRequest)
        {
            if (string.IsNullOrWhiteSpace(userRequest.MessageRequest))
            {
                return new UserResponce("Empty message");
            }

            string id = HttpContext.User.FindFirstValue("Id");
            var user = (User)await _service.GetById<User>(new Guid(id));

            if (user == null)
            {
                return new UserResponce($"Id: {id} - not found");
            }

            var post = new Publication(userRequest.MessageRequest, user);

            await _service.AddPublication(post);

            return new UserResponce("Success");
        }


        [Route("GetPosts"), HttpGet, Authorize]
        public async Task<IEnumerable<Publication>> GetPosts()
        {
            return await _service.GetAllPublication();
        }



        [Route("GetPostsByHashTag"), HttpGet, Authorize]
        public async Task<IActionResult> GetPostsByHashTag([FromBody] UserRequest userRequest)
        {
            if (string.IsNullOrWhiteSpace(userRequest.MessageRequest))
            {
                return NotFound();
            }
            try
            {
                var coll = await _service.GetByHashTag(userRequest.MessageRequest);
                return Ok(new UserResponceWithPosts(coll));
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new UserResponce("cant find by this hashtag"));
            }
        }

        [Route("RemovePost"), HttpDelete, Authorize(Roles = "Admin")]
        public async Task<string> RemovePost([FromBody] UserRequest userRequest)
        {

            if (string.IsNullOrWhiteSpace(userRequest.MessageRequest))
            {
                return "i cant searching by nothing lol";
            }
            try
            {
                await _service.RemovePostById(new Guid(userRequest.MessageRequest));

                return "SUCCESS!";
            }
            catch (ArgumentNullException e)
            {
                return "i cant find post by this id";
            }
        }




        #region Underface
        private string MessageError()
        {
            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));

            StringBuilder line = new StringBuilder();
            foreach (var item in errorMessages)
            {
                line.Append($"{item}, ");
            }
            return line.ToString();
        }
        #endregion
    }
}
