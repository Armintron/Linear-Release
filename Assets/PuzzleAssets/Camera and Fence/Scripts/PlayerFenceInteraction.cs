using UnityEngine;

public class PlayerFenceInteraction : MonoBehaviour
{
    // Plays a zap when hitting an electrified fence
    [SerializeField]
    private FenceManager fenceManager;
    [SerializeField]
    private AudioClip zapSound;

    // When colliding with something, check if its a player, and if so, "hurt" the player
    private void OnCollisionEnter(Collision collision)
    {
        if (!fenceManager.electricIsOn) return;

        GameObject hit = collision.gameObject;

        if (hit.tag.Equals("capsule"))
            hit.GetComponentInParent<PlayerPain>().PushFromPain(collision.contacts[0].normal, zapSound);
    }
}
