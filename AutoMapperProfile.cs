﻿using AutoMapper;
using RPG_Project.DTOs;
using RPG_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Project
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Source , Destination
            CreateMap<Character, GetCharacterDto>().ForMember(x => x.Skills, x => x.MapFrom(x => x.CharacterSkill.Select(cs => cs.Skill)));
            CreateMap<Weapon, GetWeaponDto>();
            CreateMap<Skill, GetSkillDto>();
        }
    }
}