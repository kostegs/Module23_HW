public interface IDamageable
{
    int GetCurrentHealth();

    int GetMaxHealth();

    void TakeDamage(int damage);
}
