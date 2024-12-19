using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class keypadInteraction : MonoBehaviour
{
    public TextMeshProUGUI targetText;

    public void ChangeText()
    {
        if (targetText != null)
        {
            targetText.text = "Button Clicked!";
        }
    }
}
