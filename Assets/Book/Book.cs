using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    public List<Texture2D> pages;
    private int currentPageIndex = 0;
    [SerializeField] Texture2D defaultPage;
    [SerializeField] Transform leftPage;
    [SerializeField] Transform rightPage;
    private void Awake()
    {
        GameEvents.AquirePages += addPages;
    }

    private void addPages(object sender, AquirePagesArgs e)
    {
        pages.AddRange(e.pages);
        RefreshPages();
    }

    public void changePages(int changeBy)
    {
        if (0 <= currentPageIndex + changeBy && currentPageIndex + changeBy < pages.Count)
        {
            currentPageIndex += changeBy;
            GameEvents.InvokePlayAudio("pageflip");
        }
        RefreshPages();
    }

    private void RefreshPages()
    {
        try
        {
            leftPage.GetComponent<Renderer>().material.mainTexture = pages[currentPageIndex];
            if (currentPageIndex == pages.Count - 1)
            {
                rightPage.GetComponent<Renderer>().material.mainTexture = defaultPage;
            }
            else
            {
                rightPage.GetComponent<Renderer>().material.mainTexture = pages[currentPageIndex + 1];
            }
        } catch(Exception e)
        {

        }
    }
}
