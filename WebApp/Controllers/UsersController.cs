using ApiDomain.Entities;
using ApiDomain.Repositories.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Dtos.User;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Produces("application/json")]
    public class UsersController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _repository;

        public UsersController(IMapper mapper, IUserRepository repository) 
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>All Users.</returns>
        /// <response code="200">Returns all Users.</response>
        [HttpGet]
        public Task<List<UserOutputDto>> GetAllUsers() => 
            _mapper.ProjectTo<UserOutputDto>(_repository.GetAll()).ToListAsync();

        /// <summary>
        /// Get a specific user by ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve</param>
        /// <returns>The user with the specified ID.</returns>
        /// <response code="200">Returns the user with the specified ID.</response>
        /// <response code="404">The user is not found.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserOutputDto>> GetUser(int id)
        {
            var user = await _repository.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var userDto = _mapper.Map<UserOutputDto>(user);
            return Ok(userDto);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user">The user object to be created</param>
        /// <returns>The created user.</returns>
        /// <response code="201">The user is successfully added.</response>
        /// <response code="400">The user data is invalid.</response>
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserCreateDto user) 
        {
            await _repository.AddAsync(_mapper.Map<User>(user));

            return Created();
        } 

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="updatedUser">The user object containing the updated details.</param>
        /// <returns>The updated user.</returns>
        /// <response code="200">The operation of updating is successful.</response>
        /// <response code="404">The user is not found.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserCreateDto updatedUser)
        {
            var existingUser = await _repository.FindAsync(id);

            if (existingUser == null)
            {
                return NotFound();
            }

            _mapper.Map(updatedUser, existingUser);
            await _repository.UpdateAsync(existingUser);
            return Ok();
        }

        /// <summary>
        /// Deletes a user by its id.
        /// </summary>
        /// <param name="id">The ID of the user delete.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        /// <response code="204">The user is successfully deleted.</response>
        /// <response code="404">The user is not found.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var existingUser = await _repository.FindAsync(id);

            if (existingUser == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(existingUser);

            return NoContent();
        }
    }
}
