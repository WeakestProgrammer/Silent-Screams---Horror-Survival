using UnityEngine;

namespace AdvancedHorrorFPS
{
    public class AdvancedGameManager : MonoBehaviour
    {
        [Tooltip("Player will die when Enemy AI catches him OR Health Bar will appear and Player will die when his health is run out.")]
        public GameType gameType;

        [Tooltip("According to your choice, Input Controllers will be activated automatically. Keyboard and Mouse for PC and Console Option. Touchpad and Joystick for Mobile.")]
        public ControllerType controllerType;
        public bool canJump;
        public bool canCarryBoxes;

        [Tooltip("Depending on your choice, FPS Hand will be visible or not visible.")]
        public bool showFPSHands;

        [Tooltip("Depending on your choice, Blink Effect on Interactable Objects will be active or passive.")]
        public bool blinkOnInteractableObjects = true;

        public static AdvancedGameManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            if (!canJump)
            {
                GameCanvas.Instance.Button_Jump.SetActive(false);
            }
            else
            {
                GameCanvas.Instance.Button_Jump.SetActive(true);
            }

            if (gameType == GameType.DieWhenYouAreCaught)
            {
                GameCanvas.Instance.Panel_Health.SetActive(false);
            }
            else if (gameType == GameType.DieWhenYourHealthIsRunOut)
            {
                GameCanvas.Instance.Panel_Health.SetActive(true);
            }

            if(controllerType == ControllerType.Mobile)
            {
                GameCanvas.Instance.Controller_Joystick.SetActive(true);
                GameCanvas.Instance.Controller_Touchpad.SetActive(true);
            }
            else
            {
                GameCanvas.Instance.Controller_Joystick.SetActive(false);
                GameCanvas.Instance.Controller_Touchpad.SetActive(false);
                GameCanvas.Instance.Button_Jump.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    public enum GameType
    {
        DieWhenYouAreCaught,
        DieWhenYourHealthIsRunOut
    }
    public enum ControllerType
    {
        PcAndConsole,
        Mobile
    }
}