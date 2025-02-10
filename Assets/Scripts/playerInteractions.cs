using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class playerInteractions : MonoBehaviour
{
    [Header("Key Bools")]
    public bool key1;
    public bool key2;
    public bool key3;
    public bool key4;

    private bool keyCheck; //Not really necessary but dont remove because i dont know what logic calls it anymore
    private string keyName;
    public bool allowOpen;
    public GameObject keyObject;

    public bool keypadCheck;
    private GameObject keypadInterfaceHolder;
    public GameObject interactNotiHolder;

    public GameObject letterObj;
    public bool letterCheck;

    public FuseHandler fuseHandler;

    private void Start()
    {
        keypadInterfaceHolder = GameObject.Find("KeypadInterface");
        interactNotiHolder = GameObject.Find("InteractNoti");
        letterObj = GameObject.Find("LetterImg");

        keypadInterfaceHolder.SetActive(false);
        letterObj.SetActive(false);
        interactNotiHolder.SetActive(false);
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
            keypadCheck = other.gameObject.CompareTag("Keypad");
        }
        else
        {
            keypadCheck = false;
        }

        if (other.gameObject.CompareTag("Letter"))
        {
            letterCheck = other.gameObject.CompareTag("Letter");
        }
        else
        {
            letterCheck = false;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Keypad"))
        {
            keypadCheck = false;
            keypadInterfaceHolder.SetActive(false);
            interactNotiHolder.SetActive(false);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (other.gameObject.CompareTag("Letter"))
        {
            letterCheck = false;
            letterObj.SetActive(false);
            interactNotiHolder.SetActive(false);
        }

        if (other.gameObject.CompareTag("Key"))
        {
            keyCheck = false;
            keyObject = null;
        }
    }

    public void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.E) && keyCheck==true)
        {
            getKey();
            interactNotiHolder.SetActive(false);
        }*/ //Dont need this anymore, dont delete, who knows what error might pop-up

        if (Input.GetKeyDown(KeyCode.E) && keypadCheck == true && fuseHandler.fuseOn == true)
        {
            showKeypad();
        }

        if (Input.GetKeyDown(KeyCode.E) && letterCheck == true)
        {
            showLetter();
        }
    }

    public void getKey()
    {
        if (keyName == "SecurityHomeKey")
        {
            FindObjectOfType<audioManager>().Play("keyChain");
            Debug.Log("SecurityHouseKey Obtained! and a close-up of the Key is presented");
            interactNotiHolder.SetActive(false);
            Destroy(keyObject);
            key1 = true;
        }

        if (keyName == "MaintenanceKey1")
        {
            FindObjectOfType<audioManager>().Play("keyChain");
            Debug.Log("MaintenanceKey Obtained! and a close-up of the Key is presented");
            interactNotiHolder.SetActive(false);
            Destroy(keyObject);
            key2 = true;
        }

        if (keyName == "HomeKey")
        {
            FindObjectOfType<audioManager>().Play("keyChain");
            Debug.Log("HomeKey Obtained! and a close-up of the Key is presented");
            interactNotiHolder.SetActive(false);
            Destroy(keyObject);
            key3 = true;
        }

        if (keyName == "MaintenanceKey2")
        {
            FindObjectOfType<audioManager>().Play("keyChain");
            Debug.Log("MaintenanceKey Obtained! and a close-up of the Key is presented");
            interactNotiHolder.SetActive(false);
            Destroy(keyObject);
            key4 = true;
        }
    }

    private void showKeypad()
    {
        Debug.Log("Something");
        keypadInterfaceHolder.SetActive(true); //KeypadInterface will show

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void showLetter()
    {
        FindObjectOfType<audioManager>().Play("letterShow");
        letterObj.SetActive(true);
    }

    private void popUpHandler()
    {
        if (keypadInterfaceHolder.activeSelf == true)
        {
            interactNotiHolder.SetActive(false);
        }
    }
}