using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class playerInteractions : MonoBehaviour
{
    [Header("Key Bools")]
    public bool key1;
    public bool key2;

    private bool keyCheck;
    private string keyName;
    public bool allowOpen;
    public GameObject keyObject;

    private bool fuseCheck;
    public GameObject fuseObject;

    private bool keypadCheck;
    private GameObject keypadInterfaceHolder;

    private void Start()
    {
        keypadInterfaceHolder = GameObject.Find("KeypadInterface");

        keypadInterfaceHolder.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Key"))
        {
            keyCheck = true;
            keyName = other.gameObject.name;
            keyObject = other.gameObject;
        }
        else
        {
            keyCheck = false;
        }

        if (other.gameObject.CompareTag("Keypad"))
        {
            Debug.Log("Keypad Detected");
            keypadCheck = other.gameObject.CompareTag("Keypad");
        }
        else
        {
            keypadCheck = false;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Keypad"))
        {
            keypadInterfaceHolder.SetActive(false);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && keyCheck==true)
        {
            getKey();
        }

        if (Input.GetKeyDown(KeyCode.E) && keypadCheck == true)
        {
            showKeypad();
        }
    }

    private void getKey()
    {
        if (keyName == "Key1")
        {
            Debug.Log("Key 1 Obtained! and a close-up of the Key is presented");
            Destroy(keyObject);
            key1 = true;
        }

        if (keyName == "Key2")
        {
            Debug.Log("Key 2 Obtained! and a close-up of the Key is presented");
            Destroy(keyObject);
            key2 = true;
        }
    }

    private void showKeypad()
    {
        //KeypadInterface will show
        keypadInterfaceHolder.SetActive(true);
    }
}