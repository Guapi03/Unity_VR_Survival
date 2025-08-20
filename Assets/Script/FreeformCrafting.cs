using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class FreeformCrafting : MonoBehaviour
{
    public GameObject craftedItemPrefab;
    public GameObject craftUIPrefab;

    private GameObject currentUI;
    private GameObject wood, stone;
    private XRGrabInteractable grab;
    private bool hasCrafted = false;
    private bool isHolding = false;

    public float detectionRadius = 0.6f;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        isHolding = true;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        isHolding = false;
        HideCraftUI();
    }

    void Update()
    {
        if (!isHolding || hasCrafted) return;

        Collider[] nearby = Physics.OverlapSphere(transform.position, detectionRadius);

        wood = null;
        stone = null;

        foreach (Collider col in nearby)
        {
            if (col.gameObject == this.gameObject) continue;

            CraftableComponent cc = col.GetComponent<CraftableComponent>();
            if (cc == null) continue;

            if (cc.itemType == "wood" && wood == null) wood = col.gameObject;
            if (cc.itemType == "stone" && stone == null) stone = col.gameObject;
        }

        if (wood && stone)
        {
            ShowCraftUI();
        }
        else
        {
            HideCraftUI();
        }
    }

    void ShowCraftUI()
    {
        if (currentUI != null) return;

        Transform cam = Camera.main.transform;
        Vector3 uiPos = cam.position + cam.forward * 0.8f;
        Quaternion uiRot = Quaternion.LookRotation(cam.forward);

        currentUI = Instantiate(craftUIPrefab, uiPos, uiRot);

        Button button = currentUI.GetComponentInChildren<Button>();
        if (button)
            button.onClick.AddListener(CraftItem);
    }

    void HideCraftUI()
    {
        if (currentUI != null)
        {
            Destroy(currentUI);
            currentUI = null;
        }
    }

    void CraftItem()
    {
        if (hasCrafted || wood == null || stone == null) return;

        Vector3 spawnPos = transform.position + Vector3.up * 0.2f;
        Instantiate(craftedItemPrefab, spawnPos, Quaternion.identity);

        Destroy(wood);
        Destroy(stone);
        Destroy(gameObject); // destroy rope
        HideCraftUI();

        hasCrafted = true;
        Debug.Log("🪓 Crafted Stone Axe with rope + wood + stone.");
    }
}