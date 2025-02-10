using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System.Linq;

public class keypadInteraction : MonoBehaviour
{
    public GameObject keypadInterfaceHolder;
    private string keypadCode;
    private string keypadInputCode;
    public bool keypadUnlock;
    private bool hasPlayedCorrectSound = false;

    private void Start()
    {
        //keypadInterfaceHolder = GameObject.Find("KeypadInterface");
        keypadUnlock = false;
        keypadCode = "1234";
        keypadInputCode = string.Empty;
    }

    public void press0()
    {
        FindObjectOfType<audioManager>().Play("keypress");
        keypadInputCode = keypadInputCode + "0";
    }

    public void press1()
    {
        FindObjectOfType<audioManager>().Play("keypress");
        keypadInputCode = keypadInputCode + "1";
    }

    public void press2()
    {
        FindObjectOfType<audioManager>().Play("keypress");
        keypadInputCode = keypadInputCode + "2";
    }

    public void press3()
    {
        FindObjectOfType<audioManager>().Play("keypress");
        keypadInputCode = keypadInputCode + "3";
    }

    public void press4()
    {
        FindObjectOfType<audioManager>().Play("keypress");
        keypadInputCode = keypadInputCode + "4";
    }

    public void press5()
    {
        FindObjectOfType<audioManager>().Play("keypress");
        keypadInputCode = keypadInputCode + "5";
    }

    public void press6()
    {
        FindObjectOfType<audioManager>().Play("keypress");
        keypadInputCode = keypadInputCode + "6";
    }

    public void press7()
    {
        FindObjectOfType<audioManager>().Play("keypress");
        keypadInputCode = keypadInputCode + "7";
    }

    public void press8()
    {
        FindObjectOfType<audioManager>().Play("keypress");
        keypadInputCode = keypadInputCode + "8";
    }

    public void press9()
    {
        FindObjectOfType<audioManager>().Play("keypress");
        keypadInputCode = keypadInputCode + "9";
    }

    public void Update()
    {
        if (keypadInputCode == keypadCode && !hasPlayedCorrectSound)
        {
            var correctSound = FindObjectOfType<audioManager>().sounds.FirstOrDefault(s => s.name == "correctCode");
            if (correctSound != null && !correctSound.source.isPlaying)
            {
                FindObjectOfType<audioManager>().Play("correctCode");
                hasPlayedCorrectSound = true; // Set the flag to true after playing the sound
            }

            keypadInterfaceHolder.SetActive(false);
            keypadUnlock = true;
        }

        if (keypadInputCode.Length == 4 && keypadInputCode != keypadCode)
        {
            var wrongSound = FindObjectOfType<audioManager>().sounds.FirstOrDefault(s => s.name == "wrongCode");
            if (wrongSound != null && !wrongSound.source.isPlaying)
            {
                FindObjectOfType<audioManager>().Play("wrongCode");
            }

            keypadInputCode = string.Empty;
        }
    }
}
