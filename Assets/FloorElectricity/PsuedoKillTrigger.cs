using UnityEngine;

public class PsuedoKillTrigger : MonoBehaviour
{
    public Teleportation teleportationScript;
    public GameObject player;
    public AudioClip zapSound;

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.name.Contains("Capsule"))
        {
            // play the zapping sound at the position they hit the collider
            AudioSource.PlayClipAtPoint(zapSound, col.GetContact(0).point);

            // teleport them to respective pad
            teleportationScript.DoInteraction(col.gameObject);
            player.GetComponent<PlayerPain>().Death();
        }
    }
}
