using Bremsengine;
using System.Collections;
using UnityEngine;

public class AyaDialogue : Dialogue
{
    protected override IEnumerator DialogueContents(int progress = 0)
    {
        Progress = 0;
        CharacterSelect(1);
        DrawDialogue("Wait, wait! I finally caught up to you!");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(0);
        DrawDialogue("Huh? Have you been following me the entire time?!");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(1);
        DrawDialogue("Why yes! I mean, come on! Mysterious weapon-creating human from the village, slicing through erratic fairies and causing a ruckus wherever she goes? That’s front page material!");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(0);
        DrawDialogue("N-No! I'm not trying to cause any ruckus! I'm trying to stop it!");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(1);
        DrawDialogue("Details, details. But my readers need answers. What’s your connection to the incident? Do your weapons happen to spread the illness? Is the sickness a metaphor for society?");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(0);
        DrawDialogue("No! None of those! I'm tryin' to fix things before more people get hurt!");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(1);
        DrawDialogue("But what is this? Sounds like a cover-up! Guess I’ll have to test your sincerity, with a duel, of course!");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(0);
        DrawDialogue("You're fightin' me just for a newspaper story?");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(1);
        DrawDialogue("Not just any story! A real Shameimaru exclusive! Now, smile for the camera!");
        SetButton(0, "->").SetForceEndWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);
    }
    /*private void WowButton()
    {
        GeneralManager.FunnyExplosion(PlayerUnit.Player.CurrentPosition, 3f);
    }
    private void BeansButton()
    {
        if (text != null)
        {
            text.AddText("BEANS ");
        }
    } */
    protected override void WhenEndDialogue(int dialogueEnding)
    {

    }
    protected override void WhenStartDialogue(int progress)
    {
        Debug.Log(Progress);
    }
}
