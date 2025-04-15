using Bremsengine;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    // "let me speak to the dialogue manager"
    public DialogueRunner dialogueRunner;
    public void StartDialogue()
    {
        DialogueRunner.SetDialogueVisibility(true);
    }
}
