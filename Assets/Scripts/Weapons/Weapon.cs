using UnityEngine;

public enum WeaponType
{
    Knife,
    Pistol,
    Shotgun,
    AR
}
public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponType weaponType;

    [SerializeField] private float attackCooldown;
    private float nextAttackTime;

    protected Player player;
    [SerializeField] protected float recoilForce;

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    protected bool IsOnCooldown()
    {
        if (Time.time < nextAttackTime) return true;

        nextAttackTime = Time.time + attackCooldown;
        return false;
    }

    public abstract void Attack();

    protected void KillIfEnemy(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            collider.gameObject.GetComponent<Enemy>().DieByWeapon(weaponType);
        }
    }
}
