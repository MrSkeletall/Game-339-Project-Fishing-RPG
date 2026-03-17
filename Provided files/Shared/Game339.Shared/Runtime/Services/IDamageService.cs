using Game339.Shared.Models;

namespace Game339.Shared.Services
{
    public interface IDamageService
    {
        int CalculateDamage(Character attacker, Character defender);
        void ApplyDamage(Character defender, int damage);
    }
}