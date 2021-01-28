using System.Collections.Generic;
using System.Threading.Tasks;
using RPG_Project.DTOs;
using RPG_Project.Models;

namespace RPG_Project.Services.Character
{
    public interface ICharacterService
    {
        Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacter();
        Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int characterId);
        Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon);
        Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newSkill);

    }
}