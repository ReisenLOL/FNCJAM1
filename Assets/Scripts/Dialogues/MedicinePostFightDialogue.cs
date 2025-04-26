using Bremsengine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MedicinePostFightDialogue : Dialogue
{
    protected override IEnumerator DialogueContents(int progress = 0)
    {
        Progress = 0;
        CharacterSelect(1);
        DrawDialogue("Ah… the steel you craft… it cuts deeper than I imagined. Perhaps my remedy was too potent.");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(0);
        DrawDialogue("I didn’t come here to fight for glory. I'm here to stop people from getting hurt.");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(1);
        DrawDialogue("Hurt? No… awakened. Without melancholy, one cannot appreciate joy. But I see now your strength outweighs my… experiment.");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(0);
        DrawDialogue("Y'know ya don’t have to spread misery to teach people appreciation.");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(1);
        DrawDialogue("Well, perhaps you’re right. And perhaps even a demon of medicine can learn compassion. Very well, I will lift the affliction.");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(0);
        DrawDialogue("Great. Now go back to whatever corner of Gensokyo you came from, leave the healing to those who care.");
        SetButton(0, "->").ContinueWhenPressed();

        yield return WaitForProgressAbove(NextContinueProgress);

        CharacterSelect(1);
        DrawDialogue("I shall... but remember, dear weaponsmith: sometimes the greatest wounds leave the deepest lessons.");
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
        SceneManager.LoadScene(5);
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
