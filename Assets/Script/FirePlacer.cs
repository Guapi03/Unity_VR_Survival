using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FirePlacer : MonoBehaviour
{
    public GameObject firePrefab;
    public float woodCheckRadius = 1.5f;
    public int woodRequiredToStart = 3;
    public float burnTimePerWood = 20f;

    private bool canStartFire = true;
    private GameObject currentFire;
    private List<BurnableWood> burningWood = new List<BurnableWood>();

    void OnTriggerStay(Collider other)
    {
        if (!canStartFire || currentFire != null) return;

        if (other.CompareTag("Steel"))
        {
            Vector3 contactPoint = transform.position;
            List<BurnableWood> woodList = GetNearbyWood(contactPoint);

            if (woodList.Count >= woodRequiredToStart)
            {
                StartCoroutine(StartFire(contactPoint + Vector3.up * 0.1f, woodList));
                canStartFire = false;
            }
            else
            {
                Debug.Log("🔥 Not enough wood to start fire.");
            }
        }
    }

    List<BurnableWood> GetNearbyWood(Vector3 center)
    {
        List<BurnableWood> nearbyWood = new List<BurnableWood>();
        Collider[] colliders = Physics.OverlapSphere(center, woodCheckRadius);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Wood"))
            {
                BurnableWood wood = col.GetComponent<BurnableWood>();
                if (wood != null && !wood.isBurning)
                {
                    nearbyWood.Add(wood);
                }
            }
        }

        return nearbyWood;
    }

    IEnumerator StartFire(Vector3 firePos, List<BurnableWood> woodToBurn)
    {
        currentFire = Instantiate(firePrefab, firePos, Quaternion.identity);
        Debug.Log($"🔥 Fire started with {woodToBurn.Count} wood logs!");

        burningWood.Clear();

        for (int i = 0; i < woodToBurn.Count; i++)
        {
            BurnableWood wood = woodToBurn[i];
            wood.StartBurning();
            burningWood.Add(wood);

            yield return new WaitForSeconds(burnTimePerWood);

            if (wood != null)
            {
                Destroy(wood.gameObject);
                Debug.Log($"🪵 Wood {i + 1} burned.");
            }
        }

        Destroy(currentFire);
        currentFire = null;
        burningWood.Clear();
        canStartFire = true;

        Debug.Log("🔥 Fire ended.");
    }
}