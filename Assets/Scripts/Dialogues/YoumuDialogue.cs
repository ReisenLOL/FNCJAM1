using Bremsengine;
using System.Collections;
using UnityEngine;

public class YoumuDialogue : Dialogue
{
    protected override IEnumerator DialogueContents(int progress = 0)
    {
        Progress = 0;
        CharacterSelect(1);
        DrawDialogue("Halt. State your name and purpose. This area is under my patrol.");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(0);
        DrawDialogue("Names Kurogane Hozuki. Just passing through, y'know. See, there’s been some strange activity near the village and I’m trying to figure out what’s causing it.");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(1);
        DrawDialogue("Strange activity you say? You mean like the fairies going wild? And that illness spreading among humans?");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(0);
        DrawDialogue("Yes, exactly! I was thinking someone from the Netherworld would know. I'm not accusing you ofcourse, just lookin' around.");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(1);
        DrawDialogue("Hmmm... So you're a villager right? You came all this way here, alone, ALL of those weapons on you, and no real explanation? Real suspicious.");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(0);
        DrawDialogue("What the- No! Why would I poison my own village?!");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(1);
        DrawDialogue("So, you're not the cause huh? I see. If that's really the case, then show me your strength, to see if you're really strong enough to handle it");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(0);
        DrawDialogue("Is this really that necessary? I don't want to fight you!");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(1);
        DrawDialogue("Then don’t hold back. En garde!");
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
