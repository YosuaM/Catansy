using Catansy.Applicaton.DTOs.Game;
using Catansy.Applicaton.Services.Implementation.Game;
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
        private readonly IIdleProgressService _idleProgressService;

        public CharacterController(
            ICharacterService characterService,
            IIdleProgressService idleProgressService
        )
        {
            _characterService = characterService;
            _idleProgressService = idleProgressService;
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

        [HttpPost("{characterId}/claim-rewards")]
        public async Task<IActionResult> ClaimIdleRewards(Guid characterId)
        {
            var accountId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            try
            {
                var result = await _idleProgressService.ClaimRewardsAsync(accountId, characterId);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
