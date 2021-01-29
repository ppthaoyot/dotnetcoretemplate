using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RPG_Project.Data;
using RPG_Project.DTOs;
using RPG_Project.Models;

namespace RPG_Project.Services.Character
{
    public class CharacterService : ICharacterService
    {
        private readonly AppDBContext _dBContext;
        private readonly IMapper _mapper;
        private readonly ILogger _log;

        public CharacterService(AppDBContext dBContext, IMapper mapper, ILogger<CharacterService> log)
        {
            _dBContext = dBContext;
            _mapper = mapper;
            _log = log;
        }


        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacter()
        {
            var characters = await _dBContext.Characters.Include(x => x.Weapon).AsNoTracking().ToListAsync();

            var dto = _mapper.Map<List<GetCharacterDto>>(characters);

            return ResponseResult.Success(dto);
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int characterId)
        {
            var character = await _dBContext.Characters.Include(x => x.Weapon).FirstOrDefaultAsync(x => x.Id == characterId);

            if (character == null)
            {
                return ResponseResult.Failure<GetCharacterDto>("Character not found.");
            }

            var dto = _mapper.Map<GetCharacterDto>(character);

            return ResponseResult.Success(dto);
        }

        public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon)
        {
            var character = await _dBContext.Characters.Include(x => x.Weapon).FirstOrDefaultAsync(x => x.Id == newWeapon.CharacterId);
            if (character == null)
            {
                return ResponseResult.Failure<GetCharacterDto>("Character not found.");

            }

            var weapon = new Weapon
            {
                Name = newWeapon.Name,
                Damage = newWeapon.Damage,
                Character = character
            };

            _dBContext.Weapons.Add(weapon);
            await _dBContext.SaveChangesAsync();

            var dto = _mapper.Map<GetCharacterDto>(character);
            return ResponseResult.Success(dto);
        }

        public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newSkill)
        {
            try
            {


                _log.LogInformation("Start Add CharacterSkill process.");
                var character = await _dBContext.Characters
                .Include(x => x.Weapon)
                .Include(x => x.CharacterSkill).ThenInclude(x => x.Skill)
                .FirstOrDefaultAsync(x => x.Id == newSkill.CharacterId);

                if (character == null)
                {
                    _log.LogError("Character not found.");
                    return ResponseResult.Failure<GetCharacterDto>("Character not found.");
                }
                _log.LogInformation("Character found.");

                var skill = await _dBContext.Skills.FirstOrDefaultAsync(x => x.Id == newSkill.SkillId);

                if (skill == null)
                {
                    _log.LogError("Skill not found.");
                    return ResponseResult.Failure<GetCharacterDto>("Skill not found.");
                }
                _log.LogInformation("Skill found.");

                var characterSkill = new CharacterSkill
                {
                    Character = character,
                    Skill = skill
                };

                _dBContext.CharacterSkills.Add(characterSkill);
                await _dBContext.SaveChangesAsync();

                _log.LogInformation("Success.");

                var dto = _mapper.Map<GetCharacterDto>(character);

                _log.LogInformation("End.");

                return ResponseResult.Success(dto);
            }
            catch (System.Exception ex)
            {

                _log.LogError(ex.Message);
                return ResponseResult.Failure<GetCharacterDto>(ex.Message);
            }
        }

        public async Task<ServiceResponse<GetCharacterDto>> AddCharacter(AddCharacterDto newCharacter)
        {
            _log.LogInformation("Start Add Character process.");
            var character = await _dBContext.Characters.FirstOrDefaultAsync(x => x.Name == newCharacter.Name.Trim());

            if (!(character is null))
            {
                _log.LogError("Duplicated Character Name.");
                return ResponseResult.Failure<GetCharacterDto>("Duplicated Character Name.");
            }

            _log.LogInformation("Add New Character.");
            var addCharacter = new Models.Character
            {
                Name = newCharacter.Name,
                HitPoints = newCharacter.HitPoints,
                Strength = newCharacter.Strength,
                Defense = newCharacter.Defense,
                Intelligence = newCharacter.Intelligence
            };

            _dBContext.Characters.Add(addCharacter);
            await _dBContext.SaveChangesAsync();
            _log.LogInformation("Success.");

            var dto = _mapper.Map<GetCharacterDto>(addCharacter);

            _log.LogInformation("End.");
            return ResponseResult.Success(dto);
        }
    }
}