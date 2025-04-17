using Core.Extensions;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace Bremsengine
{
    public class TestDialogue : Dialogue
    {
        protected override IEnumerator DialogueContents(int progress = 0)
        {/*
            DrawDialogue("Test Text");
            SetButton(0, "Yes", TestFeature).SetProgressWhenPressed();
            yield return new WaitForSeconds(0.15f);

            yield return Wait;
            DrawDialogue("Mewo mewo");
            SetButton(0, "Yes").SetProgressWhenPressed();
            SetButton(1, "+100 money", TestFeature);
            SetButton(2, "NOOO", SpawnBoss).SetForceEndWhenPressed();
            yield return new WaitForSeconds(0.15f);

            yield return Wait;
            DrawDialogue("yooo");
            SetButton(2, "Bro").SetProgressWhenPressed();
            StartSubroutine("Test Range", TestRange());
            yield return new WaitForSeconds(0.15f);

            yield return Wait;
            TryEndSubroutine("Test Range");
            DrawDialogue("jao");
            SetButton(2, "Close", SpawnBoss).SetProgressWhenPressed();
            yield return new WaitForSeconds(0.15f);

            yield return Wait;
            */
            yield return new WaitForSeconds(1f);
            ForceEndDialogue();
        }
        private void SpawnBoss()
        {
            DialogueEventBus.TriggerEvent(EventKeys.Skeletron);
        }
        private IEnumerator TestRange()
        {
            string add = " ";
            foreach (var item in 30f.StepFromTo(-100f, 360f))
            {
                add += item + " ";
            }
            foreach (char item in add.StringChop())
            {
                activeText.AddText(item);
                yield return Helper.GetWaitForSeconds(1f / 30f);
            }
        }
        protected override void WhenStartDialogue(int progress)
        {

        }
        protected override void WhenEndDialogue(int dialogueEnding)
        {

        }
        private void TestFeature()
        {
            activeText.AddText(" 100 money :)");
            UnityEngine.Debug.Log("100 moneys fortnite burger");
        }

    }
}
