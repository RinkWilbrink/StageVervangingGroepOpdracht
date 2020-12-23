using System.Collections.Generic;

using UnityEngine;

public class PageManagerThomas : MonoBehaviour
{
    public int CurrentPage;
    public List<GameObject> Pages;
    public GameObject NextButton, PreviousButton;

    private void Start()
    {
        PreviousButton.SetActive(false);  
    }

    public void AddPage(GameObject pageObject)
    {
        Pages.Add(pageObject);
    }

    public void ShowFirst()
    {
        Pages[0].SetActive(true);
    }

    public void Next()
    {
        Pages[CurrentPage].SetActive(false);

        if (CurrentPage < Pages.Count - 1)
        {
            CurrentPage += 1;
            PreviousButton.SetActive(true);
        } 

        if (CurrentPage == Pages.Count - 1)
            NextButton.SetActive(false);

        Pages[CurrentPage].SetActive(true);
    }

    public void Previous()
    {
        Pages[CurrentPage].SetActive(false);

        if (CurrentPage > 0)
        {
            CurrentPage -= 1;
            NextButton.SetActive(true);
        }


        if (CurrentPage == 0)
            PreviousButton.SetActive(false);

        Pages[CurrentPage].SetActive(true);
    }



}