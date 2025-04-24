using Bremsengine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YoumuPostFightDialogue : Dialogue
{
    protected override IEnumerator DialogueContents(int progress = 0)
    {
        Progress = 0;
        CharacterSelect(1);
        DrawDialogue("Hah... you weren't lying after all. That wasn't just a fluke");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(0);
        DrawDialogue("Ya see? I don't start fights, I want to end 'em!");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(1);
        DrawDialogue("Well... You’re rough around the edges, but… there's resolve in your strikes. I can respect that.");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(0);
        DrawDialogue("Hm? I guess I'll just take that as a compliment. So… can I keep going?");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(1);
        DrawDialogue("I won’t stop you. But, whatever’s causing this isn’t something ordinary. Even spirits are uneasy.");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(0);
        DrawDialogue("Yeah, no kidding! Fairies acting rowdier than the usual, villagers coughing up sparkles… It’s quite the mess.");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(1);
        DrawDialogue("Good luck, Kurogane Hozuki. If we meet again, I hope it’s not with swords drawn.");
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
        PlayerItemData.instance.playerLevel = FindFirstObjectByType<PlayerLevelManager>().level;
        PlayerItemData.instance.powerAmount = FindFirstObjectByType<PlayerLevelManager>().currentPower;
        SceneManager.LoadScene(2);
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
