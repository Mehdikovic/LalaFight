using UnityEngine;


namespace LalaFight
{
    public interface IDamageable
    {
        int remainingHealthPercent { get; }
        int maxHealth { get; }
        int currentHealth { get; }
        bool isDead { get; }
        void TakeDamage(int bulletDamage, Vector3 hitPosition, Vector3 hitDirection);
    }
}