﻿using BlogAPI.Models.Wraps;
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

        [Authorize(Roles = "Admin")]
        public async Task<string> Index()
        {
            string id = HttpContext.User.FindFirstValue("Id");
            string role = HttpContext.User.FindFirstValue(ClaimTypes.Role);
            string name = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            string email = HttpContext.User.FindFirstValue(ClaimTypes.Email);

            return "TestMethod";
        }

        [Route("AddPost"), HttpPost, Authorize(Roles = "Poster, Admin")]
        public async Task<UserResponse> AddPost([FromBody] UserRequest userRequest)
        {
            if (string.IsNullOrWhiteSpace(userRequest.MessageRequest))
            {
                return new UserResponse("Empty message");
            }

            string id = HttpContext.User.FindFirstValue("Id");
            var user = (User)await _service.GetById<User>(new Guid(id));

            if (user == null)
            {
                return new UserResponse($"Id: {id} - not found");
            }

            var post = new Publication(userRequest.MessageRequest, user);

            await _service.AddPublication(post);

            return new UserResponse("Success");
        }


        [Route("GetPosts"), HttpGet, Authorize]
        public async Task<IEnumerable<ItemResponce>> GetPosts()
        {
            var result = await _service.GetAllPublication();
            return new UserResponseWithPosts(result).Publications;
        }



        [Route("GetPostsByHashTag"), HttpPost, Authorize]
        public async Task<IEnumerable<ItemResponce>> GetPostsByHashTag([FromBody]UserRequest userRequest)
        {
            if (string.IsNullOrWhiteSpace(userRequest.MessageRequest))
            {
                return new UserResponseWithPosts(new List<Publication>()).Publications;
            }
            try
            {
                var coll = await _service.GetByHashTag(userRequest.MessageRequest);
                return new UserResponseWithPosts(coll).Publications;
            }
            catch (KeyNotFoundException e)
            {
                return new UserResponseWithPosts(new List<Publication>()).Publications;
            }
        }

        [Route("RemovePost"), HttpPost, Authorize(Roles = "Admin")]
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
