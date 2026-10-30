using UnityEngine;

public class WeaponSprite : MonoBehaviour
{
    protected SpriteRenderer sr;
    private Quaternion direction;

    [SerializeField] GameObject muzzleFlash;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void SetFlip(bool state)
    {
        sr.flipY = state;
    }

    public void SetDirection(Quaternion direction)
    {
        this.direction = direction;
    }
    
    public virtual void AttackAnimation()
    {
        // im crine
        Instantiate(muzzleFlash, (Vector2)transform.position + new Vector2(0.4f, 0), direction);
    }
}
