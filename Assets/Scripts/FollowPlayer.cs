using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerPos;
    private bool isFollowing;

    public void SetIsFollowing(bool state)
    {
        isFollowing = state;
    }

    void Update()
    {
        if (!isFollowing) return;

        transform.position = new Vector3(playerPos.position.x, playerPos.position.y, transform.position.z);
    }
}
