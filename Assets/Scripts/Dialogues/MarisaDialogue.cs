using Bremsengine;
using System.Collections;
using UnityEngine;

public class MarisaDialogue : Dialogue
{
    protected override IEnumerator DialogueContents(int progress = 0)
    {
        Progress = 0;
        CharacterSelect(1);
        DrawDialogue("Yo! You're that girl that's been stirrin' up all the noise lately, eh?");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(0);
        DrawDialogue("Hm?");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(1);
        DrawDialogue("Y'know, stormin' through the woods, clobbering fairies, throwin' all those strange weapons all over. Gotta say, you’re makin’ quite the impression!");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(0);
        DrawDialogue("Ah yup. I'm trying to get to the bottom of what’s messin’ with the Human Village. The fairies are acting up, and people are getting real sick.");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(1);
        DrawDialogue("Ah, that sounds exactly like an incident. Y'sure you're up for the task to resolve this? I like your guts. But I’ve seen a lot of incidents, and sometimes ya gotta blast your way to the truth.");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(0);
        DrawDialogue("Err... Are you challenging me?");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(1);
        DrawDialogue("Yup! Oh, and might I take a closer look at some of those weapons after we fight. For my own research, of course.");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(0);
        DrawDialogue("Wh- You're just gonna steal from me?!");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(1);
        DrawDialogue("Why ofcourse so! Now what are we waiting for, let's go!");
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