public interface iDamageable
{
    /// <summary>
    /// Salud actual de la entidad
    /// </summary>
    float currentHealth { get; }

    /// <summary>
    /// Salud maxima de la entidad
    /// </summary>
    float maxHealth { get; }

    /// <summary>
    /// Valor entre 0 y 1 que representa el porciento de salud de la entidad
    /// </summary>
    float healthPercent { get; }

    /// <summary>
    /// Applica cierta cantidad de daño a la salud de la entidad
    /// </summary>
    /// <param name="damage2apply"></param>
    void ApplyDamage(float damage2apply);

    /// <summary>
    /// Recuperar cierta cantidad de salud
    /// </summary>
    /// <param name="healthToRecover"></param>
    void Recover(float healthToRecover);

    /// <summary>
    /// Morir
    /// </summary>
    void Die();
}
