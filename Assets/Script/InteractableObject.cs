using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string ItemName = "Unnamed Item";

    public string GetItemName()
    {
        return ItemName;
    }
}
