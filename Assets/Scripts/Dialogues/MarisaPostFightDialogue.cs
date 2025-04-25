using Bremsengine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MarisaPostFightDialogue : Dialogue
{
    protected override IEnumerator DialogueContents(int progress = 0)
    {
        Progress = 0;
        CharacterSelect(1);
        DrawDialogue("Alright, alright! You pack a mean punch. Or... sword. Or whatever strange weapons ya got.");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(0);
        DrawDialogue("You really just wanted to fight for fun, huh?");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(1);
        DrawDialogue("Well, yeah! But you're serious about the village thing. Guess I got caught up in it.");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(0);
        DrawDialogue("You wanted to take some of my weapons right? Lets turn this around on you. Let me have a look at that little furnace on ya");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(1);
        DrawDialogue("Hey, thats my schtick y'know! And this thing powers the flashy stuff I know best!");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(0);
        DrawDialogue("I just wanna see how it works, I'll give it back to ya when I'm done with it.");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(1);
        DrawDialogue("Fine, fine. Well, you've earned it after all. Besides, I’m quite curious what you’ll do with it.");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(1);
        DrawDialogue("Anyway, good luck on your journey, Kurogane. Whatever's behind this mess, it's probably gonna get much more strange.");
        SetButton(0, "->", SwitchScene).SetForceEndWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);
    }
    [ContextMenu("Switch Stage")]
    private void SwitchScene()
    {
        Item[] itemList = FindObjectsByType<Item>(FindObjectsSortMode.None);
        foreach (Item item in itemList)
        {
            if (item.TryGetComponent(out WeaponAttack isWeaponAttack))
            {
                PlayerItemData.instance.equippedItems.Add(new PlayerItemData.EquippedItemData(isWeaponAttack.ItemName, isWeaponAttack.level));
            }
            if (item.TryGetComponent(out Passive isPassive))
            {
                PlayerItemData.instance.equippedItems.Add(new PlayerItemData.EquippedItemData(isPassive.ItemName, isPassive.level));
            }
        }
        PlayerLevelManager levelManager = FindAnyObjectByType<PlayerLevelManager>();
        PlayerItemData.instance.playerLevel = levelManager.level;
        PlayerItemData.instance.powerAmount = levelManager.currentPower;
        PlayerItemData.instance.requiredPowerToNextLevel = levelManager.requiredPowerToNextLevel;
        PlayerItemData.instance.requiredPowerToNextLevelMin = levelManager.requiredPowerToNextLevelMin;
        SceneManager.LoadScene(3);
    }
    /*private void BeansButton()
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
    }
}
