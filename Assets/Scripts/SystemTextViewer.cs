using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum SystemType { Money = 0, Build,Lcation,Archor,Wizard,Cannon,Sword}

public class SystemTextViewer : MonoBehaviour
{
    private TextMeshProUGUI textSystem;
    private TMPAlpha tmpAlpha;

    private void Awake()
    {
        textSystem = GetComponent<TextMeshProUGUI>();
        tmpAlpha = GetComponent<TMPAlpha>();
    }

    public void PrintText(SystemType type)
    {
        switch (type)
        {
            case SystemType.Money:
                textSystem.text = "System: Not enough money..";
                break;
            case SystemType.Build:
                textSystem.text = "System: Invalid build tower...";
                break;
            case SystemType.Lcation:
                textSystem.text = "System: Plese Select Location";
                break;
            case SystemType.Archor:
                textSystem.text = "Archor!";
                break;
            case SystemType.Wizard:
                textSystem.text = "Wizard!";
                break;
            case SystemType.Sword:
                textSystem.text = "Sword!";
                break;
            case SystemType.Cannon:
                textSystem.text = "Cannon!";
                break;
        }
        tmpAlpha.FadeOut();
    }
}
