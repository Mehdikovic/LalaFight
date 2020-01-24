using UnityEngine;

public interface IDamageable
{
    int RemainingHealthPercent { get; }
    int MaxHealth { get; }
    int CurrentHealth { get; }
    bool IsDead { get; }
    void TakeDamage(int bulletDamage, Vector3 hitPosition, Vector3 hitDirection);
}