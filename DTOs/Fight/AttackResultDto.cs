namespace RPG_Project.DTOs.Fight
{
    public class AttackResultDto
    {
        public string AttackName { get; set; }
        public int AttackHP { get; set; }
        public string OpponentName { get; set; }
        public int OpponentHP { get; set; }
        public int Damage { get; set; }
        public string AttackResultMessage { get; set; }
    }
}