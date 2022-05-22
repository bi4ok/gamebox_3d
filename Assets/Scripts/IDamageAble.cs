interface IDamageAble
{
    void TakeDamage(float damage, string damageFrom, bool knockback=false);

    bool DiedByDamage();
}
