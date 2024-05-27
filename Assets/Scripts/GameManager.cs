using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject menuSet;
    public bool isAction;
    public GameObject player;

    void Update()
    {
        //Sub Menu
        if (Input.GetButtonDown("Cancel")){
            SubMenuActive();
        }
    }
    public void SubMenuActive()
    {
        if (menuSet.activeSelf)
            menuSet.SetActive(false);
        else
            menuSet.SetActive(true);
    }

    public void GameExit()
    {
        Application.Quit();
    }
}