using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class dialogueManager : MonoBehaviour
{
    public static dialogueManager Instance;

    public TextMeshProUGUI dialogueArea;

    private Queue<DialogueLine> lines;

    public bool isDialogueActive = false;

    public float typingSpeed = 0.2f;

    public Animator animator;

    public Camera cam;
    public CameraController camControls;
    public PlayerController playerControls;
    private Vector3 lockedPosition;
    private Quaternion lockedRotation;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        lines = new Queue<DialogueLine>();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DisplayNextDialogueLine();
        }

        cam.transform.position = lockedPosition;
        cam.transform.rotation = lockedRotation;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        Time.timeScale = 0f; // Pause
        camControls.enabled = false;
        playerControls.enabled = false;

        if (cam != null)
        {
            lockedPosition = cam.transform.position; // Save the current cam position
            lockedRotation = cam.transform.rotation; // Save the current cam rotation
        }

        isDialogueActive = true;
        gameObject.SetActive(true);
        //animator.Play("show");

        lines.Clear();

        foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            lines.Enqueue(dialogueLine);
        }

        DisplayNextDialogueLine();
    }

    public void DisplayNextDialogueLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = lines.Dequeue();

        StopAllCoroutines();

        StartCoroutine(TypeSentence(currentLine));
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;

            float timer = 0f;
            while (timer < typingSpeed)
            {
                timer += Time.unscaledDeltaTime;
                yield return null;
            }
        }
    }

    void EndDialogue()
    {
        Time.timeScale = 1f; // Resume
        camControls.enabled = true;
        playerControls.enabled = true;

        isDialogueActive = false;
        //animator.Play("hide");
        gameObject.SetActive(false);
    }
}
