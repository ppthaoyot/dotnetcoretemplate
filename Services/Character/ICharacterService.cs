using System.Collections.Generic;
using System.Threading.Tasks;
using RPG_Project.DTOs;
using RPG_Project.DTOs.Fight;
using RPG_Project.Models;

namespace RPG_Project.Services.Character
{
    public interface ICharacterService
    {
        Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacter();
        Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int characterId);
        Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon);
        Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newSkill);
        Task<ServiceResponse<GetCharacterDto>> AddCharacter(AddCharacterDto newCharacter);
        Task<ServiceResponse<GetCharacterDto>> AddSkill(AddSkillDto newSkill);
        Task<ServiceResponse<AttackResultDto>> WeaponAtk(WeaponAtkDto request);
        Task<ServiceResponse<AttackResultDto>> SkillAtk(SkillAtkDto request);
        Task<ServiceResponse<GetCharacterDto>> RemoveWeapon(int characterId);
        Task<ServiceResponse<GetCharacterDto>> RemoveSkill(int characterId);

    }
}