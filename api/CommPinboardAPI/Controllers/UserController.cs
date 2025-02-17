using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CommPinboardAPI.Dtos;
using CommPinboardAPI.Entities;
using CommPinboardAPI.Helpers;
using CommPinboardAPI.Helpers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CommPinboardAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserHelper _helper;
        private readonly IMapper _mapper;
        public UserController(IUserHelper helper, IMapper mapper)
        {
            _helper = helper;
            _mapper = mapper;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateUser(string username, string password)
        {
            try
            {
                var result = await _helper.AuthenticateUser(username, password);
                if(result == null)return Unauthorized("Invalid username and password");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> Get(){
            var result = await _helper.GetAll();

            return Ok(new ResponseDto{
                Data = result,
                Message = "Successfully retrived"
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add(UsersDto payload){
            var toEntity = _mapper.Map<User>(payload);
            var result = await _helper.Add(toEntity);

            return Ok(new ResponseDto{
                Data = result,
                Message = "Successfully created"
            });
        }

        [HttpPut]
        public async Task<IActionResult> Update(UsersDto oldUser)
        {
            var toEntity = _mapper.Map<User>(oldUser);
            var result = await _helper.Update(oldUser.ExternalId, toEntity);

            return Ok(new ResponseDto{
                Data = result,
                Message = "Successfully Updated"
            });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid externalId){
            await _helper.Delete(externalId);
            if(_helper.Get(externalId) != null){
                return BadRequest("Deletion unsuccessful");
            }

            return Ok(new ResponseDto{Message = "Deletion successful"});
        }
    }
}