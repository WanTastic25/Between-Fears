using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DialogueLine
{
    [TextArea(3, 10)]
    public string line;
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}

public class dialogueTrigger : MonoBehaviour
{
    public GameObject player;
    public playerInteractions playerInteractions;
    public Dialogue dialogue;
    private bool localTrigger;
    private GameObject interactNotiHolder;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        playerInteractions = player.GetComponent<playerInteractions>();

        interactNotiHolder = playerInteractions.interactNotiHolder;
        //interactNotiHolder = GameObject.Find("InteractNoti");
    }

    private void Update()
    {
        if (localTrigger && Input.GetKeyDown(KeyCode.E) && this.gameObject.CompareTag("Key"))
        {
            TriggerDialogue();
            playerInteractions.getKey();
        }

        if (localTrigger && Input.GetKeyDown(KeyCode.E))
        {
            TriggerDialogue();
        }
    }

    public void TriggerDialogue()
    {
        dialogueManager.Instance.StartDialogue(dialogue);
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.tag == "Player")
        {
            interactNotiHolder.SetActive(true);
            localTrigger = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "Player")
        {
            interactNotiHolder.SetActive(false);
            localTrigger = false;
        }
    }
}
