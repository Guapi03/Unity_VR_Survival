using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class VRSelectionManager : MonoBehaviour
{
    public XRRayInteractor rayInteractor;
    public TextMeshProUGUI objectNameDisplay;

    void Update()
    {
        if (rayInteractor == null || objectNameDisplay == null)
            return;

        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();
            if (interactable != null)
            {
                objectNameDisplay.text = interactable.GetItemName();
            }
            else
            {
                objectNameDisplay.text = "";
            }
        }
        else
        {
            objectNameDisplay.text = "";
        }
    }
}