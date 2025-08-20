using UnityEngine;
using System.Collections;

public class ChoppableTree : MonoBehaviour
{
    [Header("Chop Settings")]
    public int maxHits = 3;

    [Header("Log Drop")]
    public GameObject logPrefab;
    public Transform dropPoint; // Optional empty GameObject near tree base

    [Header("Audio")]
    public AudioClip chopSound;
    public AudioClip fallSound;

    private int hitCount = 0;
    private AudioSource audioSource;
    private bool hasFallen = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Chop()
    {
        if (hasFallen) return;

        hitCount++;
        Debug.Log($"🌲 Tree hit {hitCount}/{maxHits}");

        if (chopSound && audioSource)
            audioSource.PlayOneShot(chopSound);

        if (hitCount >= maxHits)
        {
            FallTree();
        }
    }

    void FallTree()
    {
        hasFallen = true;

        if (fallSound && audioSource)
            audioSource.PlayOneShot(fallSound);

        // Add Rigidbody to fall naturally
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();

        rb.mass = 30;
        rb.angularDrag = 1;

        // Drop logs at drop point (or tree base)
        Vector3 basePos = dropPoint ? dropPoint.position : transform.position;

        for (int i = 0; i < 3; i++)
        {
            Vector3 offset = new Vector3(i * 0.5f, 0.2f, Random.Range(-0.3f, 0.3f));
            Instantiate(logPrefab, basePos + offset, Quaternion.identity);
        }

        // Destroy tree after short delay
        StartCoroutine(DestroyAfterFall());
    }

    IEnumerator DestroyAfterFall()
    {
        yield return new WaitForSeconds(Random.Range(1f, 2f));
        Destroy(gameObject);
    }
}