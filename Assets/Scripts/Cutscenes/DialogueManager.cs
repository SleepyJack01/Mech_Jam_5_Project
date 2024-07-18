using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public Image backgroundImage;
    public Image leftCharacterImage;
    public Image rightCharacterImage;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;

    private Queue<DialogueLine> lines;

    public bool isDialogueActive = false;
    public float textSpeed = 0.02f;

    public GameObject dialogueContainer;
    public GameObject textBox;
    public Animator sceneTransition;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        lines = new Queue<DialogueLine>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        isDialogueActive = true;

        dialogueContainer.SetActive(true);

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
        
        textBox.SetActive(currentLine.character.IsTextBoxActive);
        backgroundImage.sprite = currentLine.character.background;
        leftCharacterImage.sprite = currentLine.character.imageLeft;
        leftCharacterImage.color = new Color(currentLine.character.leftImageBrightness, currentLine.character.leftImageBrightness, currentLine.character.leftImageBrightness,currentLine.character.leftImageOpacity);
        rightCharacterImage.sprite = currentLine.character.imageRight;
        rightCharacterImage.color = new Color(currentLine.character.RightImagebrightness, currentLine.character.RightImagebrightness, currentLine.character.RightImagebrightness, currentLine.character.RightImageOpacity);
        characterName.text = currentLine.character.name;

        StopAllCoroutines();

        StartCoroutine(TypeSentence(currentLine));
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        dialogueContainer.SetActive(false);
        CutsceneManager.instance.NextLevel();
    }

    public void OnInputPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DisplayNextDialogueLine();
        }
    }
}
