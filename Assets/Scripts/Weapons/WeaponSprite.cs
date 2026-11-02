using UnityEngine;

public class WeaponSprite : MonoBehaviour
{
    protected SpriteRenderer sr;
    private FollowMouse mouseScript;

    [SerializeField] private GameObject muzzleFlash;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        mouseScript = transform.GetComponentInParent<FollowMouse>();
    }
    
    public virtual void AttackAnimation()
    {
        Instantiate(muzzleFlash, transform.position, transform.rotation);
    }

    private void Update()
    {
        sr.flipY = (mouseScript.LookVector.x < 0);
    }
}
