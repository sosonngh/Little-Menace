using UnityEngine;

public class EnemyBoss : Enemy
{
    [SerializeField] private float maxHealth;
    private float curHealth;

    protected override void Awake()
    {
        base.Awake();
        curHealth = maxHealth;
    }

    public void TakeDamage(float damage, WeaponType weapon)
    {
        curHealth -= damage;

        GameManager.Instance.UpdateBossHealth(curHealth, maxHealth);
        
        if (curHealth <= 0)
        {
            DieByWeapon(weapon);
        }
    }

}
