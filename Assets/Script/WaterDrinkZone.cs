using UnityEngine;
using UnityEngine.UI;

public class WaterDrinkZone : MonoBehaviour
{
    public GameObject drinkUIPrefab;
    public AudioClip drinkSFX;

    private GameObject currentUI;
    private bool isInWater = false;
    private AudioSource audioSource;
    private PlayerVitals playerVitals;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        playerVitals = FindObjectOfType<PlayerVitals>();
        if (playerVitals == null)
        {
            Debug.LogError("❌ PlayerVitals not found in scene!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isInWater && other.CompareTag("PlayerHand"))
        {
            ShowUI();
            isInWater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            HideUI();
            isInWater = false;
        }
    }

    void ShowUI()
    {
        if (currentUI != null) return;

        Transform cam = Camera.main.transform;
        Vector3 uiPos = cam.position + cam.forward * 0.6f;
        Quaternion uiRot = Quaternion.LookRotation(uiPos - cam.position);

        currentUI = Instantiate(drinkUIPrefab, uiPos, uiRot);

        Button button = currentUI.GetComponentInChildren<Button>();
        if (button)
        {
            button.onClick.AddListener(() =>
            {
                if (playerVitals != null)
                {
                    playerVitals.RestoreThirst(20f); // 根据你的设计调整数值
                }

                if (drinkSFX != null)
                {
                    audioSource.PlayOneShot(drinkSFX);
                }

                Destroy(currentUI);
                currentUI = null;
                isInWater = false;
            });
        }
    }

    void HideUI()
    {
        if (currentUI)
        {
            Destroy(currentUI);
            currentUI = null;
        }
    }
}