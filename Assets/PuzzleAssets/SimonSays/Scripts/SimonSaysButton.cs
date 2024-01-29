using UnityEngine;

public class SimonSaysButton : MonoBehaviour
{
    [SerializeField]
    private SimonSaysManager simonSaysManager;

    private Material buttonMaterial;

    private void Start()
    {
        buttonMaterial = GetComponent<Renderer>().material;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!simonSaysManager.buttonAlreadyPressed)
        {
            simonSaysManager.FlashSequence(SimonSaysManager.FlashMode.standard);
            simonSaysManager.buttonAlreadyPressed = true;
            buttonMaterial.SetColor("_Color", new Color32(200, 8, 21, 255));
        }
    }

    public void TurnOffButton()
    {
        simonSaysManager.buttonAlreadyPressed = true;
        buttonMaterial.SetColor("_Color", new Color32(200, 8, 21, 255));
    }
}
