using UnityEngine;

public class ViewconeDetection : MonoBehaviour {

	private const string ObjectTag = "capsule";
	public Transform Character;
	public Transform whereToTeleOnDeath;

	void Start () 
	{
		if (Character == null)
			Debug.LogError(ObjectTag + " viewcone character property is not set!");
	}

	public void ObjectSpotted (Collider col) 
	{
		if (col.CompareTag(ObjectTag))
		{
			RaycastHit newHit;
			Debug.DrawRay(transform.position, col.transform.position - transform.position);

			if (Physics.Raycast (new Ray(transform.position, col.transform.position - transform.position), out newHit))
				if (newHit.collider.CompareTag(ObjectTag))
					// Teleport and do the "animation" of fading the player in and out
					Character.GetComponent<PlayerPain>().Death(whereToTeleOnDeath.position);
		}
	}
}
