using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class PickableCherryTree : MonoBehaviour
{
    [Header("Drop Settings")]
    public GameObject cherryPrefab;
    public GameObject ropePrefab;
    public Transform dropPoint;

    [Header("Harvest UI")]
    public GameObject harvestUIPanel;

    [Header("Respawn")]
    public float respawnDelay = 600f;

    [Header("Harvest Feedback")]
    public AudioClip harvestSound;

    private GameObject currentUI;
    private bool isHarvesting = false;

    private Vector3 originalPos;
    private Quaternion originalRot;
    private Collider treeCollider;
    private Renderer[] renderers;
    private AudioSource audioSource;

    void Start()
    {
        originalPos = transform.position;
        originalRot = transform.rotation;

        treeCollider = GetComponent<Collider>();
        renderers = GetComponentsInChildren<Renderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand") && !isHarvesting)
        {
            ShowHarvestUI();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            if (currentUI) Destroy(currentUI);
        }
    }

    void ShowHarvestUI()
    {
        if (currentUI || isHarvesting) return;

        Transform cam = Camera.main.transform;
        Vector3 uiPos = cam.position + cam.forward * 0.8f;
        Quaternion uiRot = Quaternion.LookRotation(cam.forward);

        currentUI = Instantiate(harvestUIPanel, uiPos, uiRot);
        currentUI.GetComponentInChildren<TextMeshProUGUI>().text = "Press to Harvest";

        Button button = currentUI.GetComponentInChildren<Button>();
        button.onClick.AddListener(() =>
        {
            StartCoroutine(StartHarvest());
        });
    }

    IEnumerator StartHarvest()
    {
        isHarvesting = true;

        if (currentUI)
        {
            currentUI.GetComponentInChildren<TextMeshProUGUI>().text = "Harvesting...";
            Destroy(currentUI, 3f);
        }
        // 🔊 Play harvest sound
        if (harvestSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(harvestSound);
        }


        yield return new WaitForSeconds(3f);

        // Drop items
        Instantiate(cherryPrefab, dropPoint.position + Vector3.left * 0.2f, Quaternion.identity);
        Instantiate(ropePrefab, dropPoint.position + Vector3.right * 0.2f, Quaternion.identity);



        // Hide visuals and collider
        foreach (Renderer r in renderers)
            r.enabled = false;

        if (treeCollider)
            treeCollider.enabled = false;

        StartCoroutine(RespawnTree());
    }

    IEnumerator RespawnTree()
    {
        yield return new WaitForSeconds(respawnDelay);

        transform.position = originalPos;
        transform.rotation = originalRot;
        isHarvesting = false;

        foreach (Renderer r in renderers)
            r.enabled = true;

        if (treeCollider)
            treeCollider.enabled = true;

        Debug.Log("🌳 Berry tree respawned.");
    }
}