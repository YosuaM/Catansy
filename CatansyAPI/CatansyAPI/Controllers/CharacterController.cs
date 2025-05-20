using Catansy.Applicaton.DTOs.Auth;
using Catansy.Applicaton.Services.Interfaces.Game;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Catansy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyCharacters()
        {
            var accountId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var characters = await _characterService.GetCharactersForAccountAsync(accountId);
            return Ok(characters);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCharacter([FromBody] CharacterCreateRequest request)
        {
            try
            {
                var accountId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var character = await _characterService.CreateCharacterAsync(accountId, request);
                return Ok(character);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{characterId}")]
        public async Task<IActionResult> GetCharacterById(Guid characterId)
        {
            var accountId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var character = await _characterService.GetCharacterByIdAsync(accountId, characterId);

            if (character == null)
                return NotFound();

            return Ok(character);
        }
    }
}
