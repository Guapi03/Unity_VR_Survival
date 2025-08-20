using UnityEngine;

public class AxeHit : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Tree"))
        {
            ChoppableTree tree = collision.gameObject.GetComponent<ChoppableTree>();
            if (tree != null)
            {
                tree.Chop();
            }
        }
    }
}