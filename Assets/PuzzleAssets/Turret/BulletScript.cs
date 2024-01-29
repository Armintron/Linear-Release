using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletScript : MonoBehaviour
{
    public Vector3 teleportLocationOnDeath; 
    public AudioClip onImpactSound;
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("capsule"))
        {
            other.gameObject.GetComponentInParent<PlayerPain>().Death(this.teleportLocationOnDeath);
        }
        AudioSource.PlayClipAtPoint(onImpactSound, this.transform.position);
        Destroy(this.gameObject);
    }
}
