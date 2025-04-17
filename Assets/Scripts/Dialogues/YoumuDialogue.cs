using Bremsengine;
using System.Collections;
using UnityEngine;

public class YoumuDialogue : Dialogue
{
    protected override IEnumerator DialogueContents(int progress = 0)
    {
        Progress = 0;
        CharacterSelect(1);
        DrawDialogue("I Am youmu, the strongest in the residence of the funnygarden");
        SetButton(0, "Wow", WowButton).ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(0);
        DrawDialogue("Wow i cant believe theres this many beans!");
        SetButton(0, "Wow").ContinueWhenPressed();
        SetButton(1, "BEANS!", BeansButton);

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(1);
        DrawDialogue("Anyway time to die lmoa");
        SetButton(0, "Eat this lol!").SetForceEndWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);
    }
    private void WowButton()
    {
        GeneralManager.FunnyExplosion(PlayerUnit.Player.CurrentPosition, 3f);
    }
    private void BeansButton()
    {
        if (text != null)
        {
            text.AddText("BEANS ");
        }
    }
    protected override void WhenEndDialogue(int dialogueEnding)
    {

    }
    protected override void WhenStartDialogue(int progress)
    {
        Debug.Log(Progress);
    }
}
