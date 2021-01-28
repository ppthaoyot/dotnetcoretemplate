using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RPG_Project.Models
{
    [Table("Skill")]
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Damage { get; set; }
        public int CharacterId { get; set; }
        public List<CharacterSkill> CharacterSkill { get; set; }
    }
}