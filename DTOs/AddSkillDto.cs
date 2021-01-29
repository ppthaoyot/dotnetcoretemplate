using System.ComponentModel.DataAnnotations;
using RPG_Project.Validations;

namespace RPG_Project.DTOs
{
    public class AddSkillDto
    {
        [FirstLetterUpperCaseAttribute]
        [Required]
        public string Name { get; set; }
        [Required]
        public int Damage { get; set; }

    }
}