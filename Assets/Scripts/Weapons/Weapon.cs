using UnityEngine;

public enum WeaponType
{
    Knife,
    Pistol,
    Shotgun,
    AR,
    AlmightyPush
}
public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponType weaponType;

    [SerializeField] private float attackCooldown;
    private float nextAttackTime;

    protected Player player;
    [SerializeField] protected float recoilForce;

    [SerializeField] private int maxAmmo;
    private int curAmmo;

    private void Start()
    {
        Reload();
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    protected void KillIfEnemy(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            collider.gameObject.GetComponent<Enemy>().DieByWeapon(weaponType);
            GameManager.Instance.IncrementKillCount();
        }
    }

    public void Attack()
    {
        if (Time.time < nextAttackTime) return;

        if (curAmmo == 0) return;

        SpecificAttackBehaviour();

        curAmmo--;

        nextAttackTime = Time.time + attackCooldown;
    }

    public void Reload()
    {
        if (curAmmo == maxAmmo) return;

        curAmmo = maxAmmo;
    }

    protected abstract void SpecificAttackBehaviour();
}
