using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RPG_Project.Data;
using RPG_Project.DTOs;
using RPG_Project.DTOs.Fight;
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
            var characters = await _dBContext.Characters
            .Include(x => x.Weapon)
            .Include(x => x.CharacterSkill).ThenInclude(x => x.Skill)
            .AsNoTracking().ToListAsync();

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

        public async Task<ServiceResponse<GetCharacterDto>> AddSkill(AddSkillDto newSkill)
        {
            _log.LogInformation("Start Add Skill process.");
            var skill = await _dBContext.Skills.FirstOrDefaultAsync(x => x.Name == newSkill.Name.Trim());

            if (!(skill is null))
            {
                _log.LogError("Duplicated Skill Name.");
                return ResponseResult.Failure<GetCharacterDto>("Duplicated Skill Name.");
            }

            _log.LogInformation("Add New Skill.");
            var addSkill = new Skill
            {
                Name = newSkill.Name,
                Damage = newSkill.Damage,
            };

            _dBContext.Skills.Add(addSkill);
            await _dBContext.SaveChangesAsync();
            _log.LogInformation("Success.");

            var dto = _mapper.Map<GetCharacterDto>(addSkill);

            _log.LogInformation("End.");
            return ResponseResult.Success(dto);
        }

        public async Task<ServiceResponse<AttackResultDto>> WeaponAtk(WeaponAtkDto request)
        {
            try
            {
                _log.LogInformation("Start  process.");
                var attacker = await _dBContext.Characters
                .Include(x => x.Weapon)
                .FirstOrDefaultAsync(x => x.Id == request.AttackerId);

                if (attacker is null)
                {
                    var msg = $"This attackerId {request.AttackerId} not found.";
                    _log.LogError(msg);
                    return ResponseResult.Failure<AttackResultDto>(msg);
                }

                var opponent = await _dBContext.Characters
                .Include(x => x.Weapon)
                .FirstOrDefaultAsync(x => x.Id == request.OpponentId);

                if (opponent is null)
                {
                    var msg = $"This attackerId {request.OpponentId} not found.";
                    _log.LogError(msg);
                    return ResponseResult.Failure<AttackResultDto>(msg);
                }

                int damage;
                damage = attacker.Weapon.Damage + attacker.Strength;
                damage -= opponent.Defense;

                if (damage > 0)
                {
                    opponent.HitPoints -= damage;
                }

                string atkResultMessage;
                if (opponent.HitPoints <= 0)
                {
                    atkResultMessage = $"{opponent.Name} id dead.";
                }
                else
                {
                    atkResultMessage = $"{opponent.Name} HP Remain {opponent.HitPoints}";
                }

                _dBContext.Characters.Update(opponent);
                await _dBContext.SaveChangesAsync();

                var dto = new AttackResultDto
                {
                    AttackName = attacker.Name,
                    AttackHP = attacker.HitPoints,
                    OpponentName = opponent.Name,
                    OpponentHP = opponent.HitPoints,
                    Damage = damage,
                    AttackResultMessage = atkResultMessage
                };
                _log.LogInformation("End  process.");
                return ResponseResult.Success(dto);
            }
            catch (System.Exception ex)
            {
                _log.LogError(ex.Message);
                return ResponseResult.Failure<AttackResultDto>(ex.Message);
            }
        }



        public async Task<ServiceResponse<AttackResultDto>> SkillAtk(SkillAtkDto request)
        {
            try
            {
                _log.LogInformation("Start  process.");
                var attacker = await _dBContext.Characters
                .Include(x => x.Weapon)
                .Include(x => x.CharacterSkill).ThenInclude(x => x.Skill)
                .FirstOrDefaultAsync(x => x.Id == request.AttackerId);

                if (attacker is null)
                {
                    var msg = $"This attackerId {request.AttackerId} not found.";
                    _log.LogError(msg);
                    return ResponseResult.Failure<AttackResultDto>(msg);
                }

                var opponent = await _dBContext.Characters
                .Include(x => x.Weapon)
                .FirstOrDefaultAsync(x => x.Id == request.OpponentId);

                if (opponent is null)
                {
                    var msg = $"This attackerId {request.OpponentId} not found.";
                    _log.LogError(msg);
                    return ResponseResult.Failure<AttackResultDto>(msg);
                }

                var charSkill = await _dBContext.CharacterSkills
                .Include(x => x.Skill)
                .FirstOrDefaultAsync(x => x.CharacterId == request.AttackerId && x.SkillId == request.SkillId);

                if (charSkill is null)
                {
                    var msg = $"This Attacker doesn't know this skill {request.OpponentId}.";
                    _log.LogError(msg);
                    return ResponseResult.Failure<AttackResultDto>(msg);
                }

                int damage;
                damage = charSkill.Skill.Damage + attacker.Intelligence;
                damage -= opponent.Defense;

                if (damage > 0)
                {
                    opponent.HitPoints -= damage;
                }

                string atkResultMessage;
                if (opponent.HitPoints <= 0)
                {
                    atkResultMessage = $"{opponent.Name} id dead.";
                }
                else
                {
                    atkResultMessage = $"{opponent.Name} HP Remain {opponent.HitPoints}";
                }

                _dBContext.Characters.Update(opponent);
                await _dBContext.SaveChangesAsync();

                var dto = new AttackResultDto
                {
                    AttackName = attacker.Name,
                    AttackHP = attacker.HitPoints,
                    OpponentName = opponent.Name,
                    OpponentHP = opponent.HitPoints,
                    Damage = damage,
                    AttackResultMessage = atkResultMessage
                };

                _log.LogInformation("End  process.");
                return ResponseResult.Success(dto);
            }
            catch (System.Exception ex)
            {
                _log.LogError(ex.Message);
                return ResponseResult.Failure<AttackResultDto>(ex.Message);
            }
        }

        public async Task<ServiceResponse<GetCharacterDto>> RemoveWeapon(int characterId)
        {
            try
            {
                _log.LogInformation("Start process.");
                var character = await _dBContext.Characters
                .Include(x => x.Weapon)
                .Include(x => x.CharacterSkill).ThenInclude(x => x.Skill)
                .FirstOrDefaultAsync(x => x.Id == characterId);

                if (character is null)
                {
                    return ResponseResult.Failure<GetCharacterDto>("Character not found.");
                }

                var weapon = await _dBContext.Weapons.Where(x => x.CharacterId == characterId).FirstOrDefaultAsync();
                if (weapon is null)
                {
                    return ResponseResult.Failure<GetCharacterDto>("Weapon not found.");
                }

                _dBContext.Weapons.Remove(weapon);
                await _dBContext.SaveChangesAsync();

                var dto = _mapper.Map<GetCharacterDto>(character);

                _log.LogInformation("End process.");
                return ResponseResult.Success(dto);
            }
            catch (System.Exception ex)
            {

                _log.LogError(ex.Message);
                return ResponseResult.Failure<GetCharacterDto>(ex.Message);
            }
        }

        public async Task<ServiceResponse<GetCharacterDto>> RemoveSkill(int characterId)
        {
            try
            {
                _log.LogInformation("Start process.");
                var character = await _dBContext.Characters
                .Include(x => x.Weapon)
                .Include(x => x.CharacterSkill).ThenInclude(x => x.Skill)
                .FirstOrDefaultAsync(x => x.Id == characterId);

                if (character is null)
                {
                    return ResponseResult.Failure<GetCharacterDto>("Character not found.");
                }

                var charSkill = await _dBContext.CharacterSkills.Where(x => x.CharacterId == characterId).ToListAsync();
                if (charSkill is null)
                {
                    return ResponseResult.Failure<GetCharacterDto>("Skill not found.");
                }

                _dBContext.CharacterSkills.RemoveRange(charSkill);
                await _dBContext.SaveChangesAsync();

                var dto = _mapper.Map<GetCharacterDto>(character);

                _log.LogInformation("End process.");
                return ResponseResult.Success(dto);
            }
            catch (System.Exception ex)
            {
                _log.LogError(ex.Message);
                return ResponseResult.Failure<GetCharacterDto>(ex.Message);
            }
        }
    }
}