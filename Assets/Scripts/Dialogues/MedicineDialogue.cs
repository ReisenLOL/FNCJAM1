using Bremsengine;
using System.Collections;
using UnityEngine;

public class MedicineDialogue : Dialogue
{
    protected override IEnumerator DialogueContents(int progress = 0)
    {
        Progress = 0;
        CharacterSelect(0);
        DrawDialogue("So, you're the mastermind behind all of this... The fairies going haywire and the sickness in the village?");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        Progress = 0;
        CharacterSelect(1);
        DrawDialogue("Mastermind huh? No, not at all, I just gave those fairies and humans a little nudge. People get so upset when things change, don’t they?");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(0);
        DrawDialogue("Seriously! They're sick! People are suffering! What did the villagers ever do to you??");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(1);
        DrawDialogue("Nothing. That’s the problem. They see illness, and they ignore it. They see fairies, and they swat them away. I simply made it so they couldn’t look away anymore.");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(0);
        DrawDialogue("...And ya call that justice?");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(1);
        DrawDialogue("No, I call it balance. Don’t you think it’s fair for humans to feel the poison they’ve left in the world? This garden was once beautiful... until they trampled it.");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(0);
        DrawDialogue("What?! You're outta your mind! You're not helping anyone here, you're just spreading pain!");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(1);
        DrawDialogue("Oh but isn’t that what medicine does? A little pain before you get better. Though in this case, I don't believe they deserve to recover");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(0);
        DrawDialogue("Is that so? I see. Then I’ll be the antidote to your sickness.");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(1);
        DrawDialogue("Hah! Try not to wilt, little weed.");
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