using UnityEngine;

public class Knife : Weapon
{
    [SerializeField] private float attackRadius;
    [SerializeField] private WeaponSprite sprite;

    protected override void SpecificAttackBehaviour()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position + 0.5f * transform.right, attackRadius);

        foreach (Collider2D hit in hits)
        {
            DealDamageIfEnemy(hit);
        }

        // anim
        sprite.Attack();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + 0.5f * transform.right, attackRadius);
    }

}
