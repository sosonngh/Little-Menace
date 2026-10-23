using UnityEngine;
public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    private float nextAttackTime;

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
            collider.gameObject.GetComponent<Enemy>().Die();
        }
    }
}
