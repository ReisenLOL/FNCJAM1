using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Core.Input;
using Core.Extensions;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;
namespace Bremsengine
{
    [DefaultExecutionOrder(5)]
    public class DialogueRunner : MonoBehaviour
    {
        static DialogueRunner Instance;
        [SerializeField] List<Dialogue.DialogueButton> dialogueButtons;
        [SerializeField] DialogueText dialogueText;
        [SerializeField] TMP_Text dialogueTextComponent;
        [SerializeField] GameObject dialogueContainer;
        [SerializeField] List<Image> characterSprites = new();

        public static List<Dialogue.DialogueButton> GetButtons() => Instance.dialogueButtons.ToList(); //lazy it should just copy this as a new list. it is to not affect the original (idk maybe doesnt matter).
        private void Awake()
        {
            Instance = this;
            Dialogue.BindDialogueText(dialogueText);
            Dialogue.BindRunner(this);
            DialogueText.SetDialogueTextRenderer(dialogueTextComponent);
            dialogueContainer.SetActive(false);
        }
        public static void SetDialogueVisibility(bool state)
        {
            Instance.dialogueContainer.SetActive(state);
            foreach (var item in Instance.characterSprites)
            {
                item.sprite = null;
            }
        }
        public static DialogueRunner BoxVisibility(bool state)
        {
            Instance.dialogueContainer.SetActive(state);
            return Instance;
        }
        public static DialogueRunner SetCharacterSprite(int index, Sprite s)
        {
            Instance.characterSprites[index].sprite = s;
            return Instance;
        }
        public static DialogueRunner SetCharacterFocus(int index)
        {
            foreach (var item in Instance.characterSprites)
            {
                item.color = item.color.Opacity(170);
            }
            Instance.characterSprites[index].color = Instance.characterSprites[index].color.Opacity(255);
            return Instance;
        }
        public void PressButton(int index)
        {
            Dialogue.PressButton(index);
        }
        private void Start()
        {

        }
        private void Update()
        {
            if (Gamepad.current == null)
                return;
            float readValue = Gamepad.current.rightTrigger.ReadValue();
            if (readValue > 0.5f)
            {
                Dialogue.PressContinueInput();
            }
        }
        private void ContinueInput()
        {

        }
        private void OnDestroy()
        {

        }
    }
}