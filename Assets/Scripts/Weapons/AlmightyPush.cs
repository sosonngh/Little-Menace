using UnityEngine;

public class AlmightyPush : Weapon
{
    [SerializeField] private float radius;

    [SerializeField] public float shakeDuration;
    [SerializeField] private float shakeMagnitude;

    protected override void SpecificAttackBehaviour()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D hit in hits)
        {
            DealDamageIfEnemy(hit);
        }

        GameManager.Instance.ShakeCamera(shakeDuration, shakeMagnitude);
    }
}
