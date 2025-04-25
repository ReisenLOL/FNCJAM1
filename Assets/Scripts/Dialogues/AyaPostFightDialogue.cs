using Bremsengine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AyaPostFightDialogue : Dialogue
{
    protected override IEnumerator DialogueContents(int progress = 0)
    {
        Progress = 0;
        CharacterSelect(1);
        DrawDialogue("Aahhh! You’re definitely not faking that determination... and your weapons are too flashy for a fraud.");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(0);
        DrawDialogue("What made 'ya think I was the culprit in the first place?!");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(1);
        DrawDialogue("Well, you’ve certainly earned yourself a bold headline. 'Lone Weaponsmith Battles Blight - Is She the Key to the Cure?'");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(0);
        DrawDialogue("...So you're still just gonna publish this?");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(1);
        DrawDialogue("Of course! But don’t worry, you'll sound very cool after all of this. Probably.");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(0);
        DrawDialogue("Sigh... Well I guess it's not that bad, if you'll truely make me sound cool.");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(1);
        DrawDialogue("Indeed! But don’t think this is over. I’m keeping my lens on you, Kurogane Hozuki. Wherever the story goes, I’ll be there!");
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
        SceneManager.LoadScene(4);
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
