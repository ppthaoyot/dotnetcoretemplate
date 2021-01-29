using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RPG_Project.DTOs;
using RPG_Project.Services.Character;

namespace RPG_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _charService;

        public CharacterController(ICharacterService charService)
        {
            _charService = charService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllCharacters()
        {
            return Ok(await _charService.GetAllCharacter());
        }

        [HttpGet("{characterId}")]
        public async Task<IActionResult> GetCharacterById(int characterId)
        {
            return Ok(await _charService.GetCharacterById(characterId));
        }

        [HttpPost("addWeapon")]
        public async Task<IActionResult> AddWeapon(AddWeaponDto newWeapon)
        {
            return Ok(await _charService.AddWeapon(newWeapon));
        }

        [HttpPost("addcharacterskill")]
        public async Task<IActionResult> AddCharacterSkill(AddCharacterSkillDto newSkill)
        {
            return Ok(await _charService.AddCharacterSkill(newSkill));
        }
    }
}