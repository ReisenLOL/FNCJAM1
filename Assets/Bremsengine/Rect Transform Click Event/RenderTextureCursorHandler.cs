using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Bremsengine
{
    public enum PointerButton
    {
        Left,
        Right,
        Middle
    }
    public class RenderTextureCursorHandler : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerExitHandler, IPointerEnterHandler, IPointerUpHandler
    {
        [SerializeField] RectTransform renderTexture;
        Camera renderCamera;

        static Transform controllerReference;

        public delegate void OnClick(Vector2 worldPosition, PointerButton pressType);
        public static event OnClick ClickDown;
        public static event OnClick ClickUp;
        static bool IsPressed;
        private static Vector2 lastCursorWorldPosition;
        static PointerEventData lastPointerData;
        Vector2 lastControllerInput;
        static bool internalWasHovering;
        public static Vector2 CursorWorldPosition => lastCursorWorldPosition;
        public static bool IsHovering { get; private set; }
        private void Start()
        {
            renderCamera = Camera.main;
            SceneManager.activeSceneChanged += (Scene s, Scene ss) => { renderCamera = Camera.main; };
        }
        public static void SetControllerReference(Transform t)
        {
            controllerReference = t;
        }
        private void RebuildCurrentPosition()
        {
            bool TryReadRightStick(out Vector2 input)
            {
                input = Vector2.zero;
                if (controllerReference != null && Gamepad.current != null)
                {
                    input = Gamepad.current.rightStick.ReadValue();
                }
                return input.magnitude >= 0.25f;
            }
            if (TryReadRightStick(out Vector2 stick))
            {
                IsHovering = true;
                IsPressed = true;
                stick = stick.normalized;
                lastControllerInput = stick;
                lastCursorWorldPosition = (Vector2)controllerReference.position + stick;
                ClickDown.Invoke(lastCursorWorldPosition, PointerButton.Left);
                return;
            }
            else
            {
                if (lastControllerInput.magnitude >= 0.25f)
                {
                    IsPressed = false;
                    IsHovering = internalWasHovering;
                    lastControllerInput = Vector2.zero;
                    ClickUp.Invoke((Vector2)controllerReference.position + lastControllerInput, PointerButton.Left);
                    return;
                }
            }
            if (lastPointerData != null && RenderTextureContainsMousePosition(out Vector2 click, lastPointerData, renderTexture))
            {
                ScaleRenderClickToCameraWorldPosition(out Vector2 w, click, Camera.main);
                lastCursorWorldPosition = w;
            }
        }
        private void Update()
        {
            RebuildCurrentPosition();
        }
        private void Awake()
        {
            IsPressed = false;
        }
        public void OnPointerMove(PointerEventData eventData)
        {
            if (RenderTextureContainsMousePosition(out Vector2 click, eventData, renderTexture))
            {
                ScaleRenderClickToCameraWorldPosition(out Vector2 worldPosition, click, Camera.main);
                lastCursorWorldPosition = worldPosition;
                lastPointerData = eventData;
            }
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            IsHovering = false;
            if (IsPressed)
            {
                TriggerPressEvent(eventData, ClickUp);
            }
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            IsHovering = true;
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            TriggerPressEvent(eventData, ClickUp);
            IsPressed = false;
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            TriggerPressEvent(eventData, ClickDown);
            IsPressed = true;
        }
        private void TriggerPressEvent(PointerEventData eventData, OnClick action)
        {
            if (RenderTextureContainsMousePosition(out Vector2 click, eventData, renderTexture))
            {
                ScaleRenderClickToCameraWorldPosition(out Vector2 worldPosition, click, Camera.main);
                PointerButton pressType = PointerButton.Left;
                switch (eventData.button)
                {
                    case PointerEventData.InputButton.Left:
                        break;
                    case PointerEventData.InputButton.Right:
                        pressType = PointerButton.Right;
                        break;
                    case PointerEventData.InputButton.Middle:
                        pressType = PointerButton.Middle;
                        break;
                    default:
                        break;
                }
                action?.Invoke(worldPosition, pressType);
            }
        }
        private void ScaleRenderClickToCameraWorldPosition(out Vector2 worldPosition, Vector2 normalizedClick, Camera fallbackCamera)
        {
            if (renderCamera == null)
            {
                renderCamera = fallbackCamera;
            }
            if (renderCamera == null)
            {
                worldPosition = lastCursorWorldPosition;
                return;
            }
            Vector2 cameraSize = Vector2.zero;
            cameraSize.y = renderCamera.orthographicSize * 2f;
            cameraSize.x = cameraSize.y * renderCamera.aspect;
            worldPosition = new Vector2(normalizedClick.x * cameraSize.x, normalizedClick.y * cameraSize.y) + (Vector2)renderCamera.transform.position;
            worldPosition -= cameraSize * 0.5f;
        }
        private bool RenderTextureContainsMousePosition(out Vector2 normalizedPosition, PointerEventData pointer, RectTransform rendererRect)
        {
            internalWasHovering = false;
            normalizedPosition = Vector2.zero;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rendererRect, pointer.position, pointer.pressEventCamera, out var localPosition))
            {
                return false;
            }
            normalizedPosition = Rect.PointToNormalized(rendererRect.rect, localPosition);
            internalWasHovering = true;
            return true;
        }

    }
}