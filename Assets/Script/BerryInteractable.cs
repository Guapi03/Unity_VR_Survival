using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BerryInteractable : MonoBehaviour
{
    public float hungerRestoreAmount = 10f;
    public float autoEatDistance = 0.3f;
    public AudioClip eatSound; // 🔊 Sound to play when eaten

    private XRGrabInteractable grab;
    private bool isEaten = false;
    private AudioSource audioSource;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrabbed);

        // Try get or create AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void OnGrabbed(SelectEnterEventArgs args)
    {
        // Optional: eat immediately on grab
    }

    void Update()
    {
        if (isEaten) return;

        Transform cam = Camera.main.transform;
        float dist = Vector3.Distance(transform.position, cam.position);

        if (dist <= autoEatDistance)
        {
            EatBerry();
        }
    }

    void EatBerry()
    {
        if (isEaten) return;

        PlayerVitals player = FindObjectOfType<PlayerVitals>();
        if (player)
        {
            player.RestoreHunger(hungerRestoreAmount);
        }

        if (eatSound && audioSource)
        {
            audioSource.PlayOneShot(eatSound);
        }

        isEaten = true;

        // Delay destroy to allow sound to play
        Destroy(gameObject, eatSound ? eatSound.length : 0.1f);
    }
}