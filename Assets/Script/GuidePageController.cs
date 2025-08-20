using UnityEngine;

public class GuidePageController : MonoBehaviour
{
    public GameObject[] pages; // Drag Page1 to Page4 in order

    private int currentPageIndex = 0;

    void Start()
    {
        ShowPage(0); // Start with page 1
    }

    public void ShowPage(int pageIndex)
    {
        if (pageIndex < 0 || pageIndex >= pages.Length) return;

        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == pageIndex);
        }

        currentPageIndex = pageIndex;
    }

    // Optional: Next / Previous buttons
    public void NextPage()
    {
        int next = (currentPageIndex + 1) % pages.Length;
        ShowPage(next);
    }

    public void PreviousPage()
    {
        int prev = (currentPageIndex - 1 + pages.Length) % pages.Length;
        ShowPage(prev);
    }
}