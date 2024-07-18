using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
    public class DialogueCharacter
    {
        public string name;
        public Sprite background;
        public Sprite imageLeft;
        [Range(0, 1)]
        public float leftImageOpacity = 0;
        [Range(0, 1)]
        public float leftImageBrightness = 1;
        
        public Sprite imageRight;
        [Range(0, 1)]
        public float RightImageOpacity = 0;
        [Range(0, 1)]
        public float RightImagebrightness = 1;

        public bool IsTextBoxActive = true;
    }

    [System.Serializable] 
    public class DialogueLine
    {
        public DialogueCharacter character;
        [TextArea(3, 10)]
        public string line;
    }

    [System.Serializable]
    public class Dialogue
    {
        public List<DialogueLine> dialogueLines = new List<DialogueLine>();
    }

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    private void Start() 
    {
        TriggerDialogue();
    }

    public void TriggerDialogue()
    {
        DialogueManager.instance.StartDialogue(dialogue);
    }
}
