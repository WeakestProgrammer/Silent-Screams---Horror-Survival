using UnityEngine;

namespace AdvancedHorrorFPS
{
    public class FlashLightScript : MonoBehaviour
    {
        public static FlashLightScript Instance;
        public Transform target;
        public bool isGrabbed = false;
        public float speed = 2.5f;
        public Transform positionTarget;
        public Light Light;
        public float BlueBattery = 100;
        public float DamageRate = 0.25f;
        public float BatterySpendNumber = 1;
        RaycastHit hit;
        public AudioSource audioSource;
        public Transform aimPoint;
        public LayerMask layerMask;

        void Awake()
        {
            Instance = this;
        }
        public void FlashLight_Decision(bool decision)
        {
            Light.enabled = decision;
            GameCanvas.Instance.Indicator_BlueLight.SetActive(decision);
        }

        private void Start()
        {
            isGrabbed = true;
            Light = GetComponent<Light>();
        }

        public void Grabbed()
        {
            if(AdvancedGameManager.Instance.controllerType == ControllerType.Mobile)
            {
                GameCanvas.Instance.Activate_FlashLightButton();
            }
            else
            {
                GameCanvas.Instance.Hide_Warning();
                GameCanvas.Instance.Show_Warning("Press F for Flashlight!");
            }
            positionTarget = GameObject.Find("FlashLightPoint").transform;
            if (AdvancedGameManager.Instance.showFPSHands)
            {
                HeroPlayerScript.Instance.FPSHands.SetActive(true);
            }
            isGrabbed = true;
        }
        private void Update()
        {
            if (!isGrabbed) return;

            if (AdvancedGameManager.Instance.controllerType == ControllerType.PcAndConsole)
            {
                if (Input.GetKeyUp(KeyCode.F))
                {
                    FlashLightScript.Instance.FlashLight_Decision(true);
                    AudioManager.Instance.Play_Flashlight_Open();
                }
                else if (Input.GetMouseButtonDown(1) && AdvancedGameManager.Instance.controllerType == ControllerType.PcAndConsole)
                {
                    GameCanvas.Instance.FlashLight_BlueEffect_Down();
                }
                else if (Input.GetMouseButtonUp(1) && AdvancedGameManager.Instance.controllerType == ControllerType.PcAndConsole)
                {
                    GameCanvas.Instance.FlashLight_BlueEffect_Up();
                }
            }
        }

        public void PlayAudioBlueLight()
        {
            audioSource.Play();
        }

        public void StopAudioBlueLight()
        {
            audioSource.Stop();
        }

        void LateUpdate()
        {
            if (!isGrabbed) return;
            Vector3 dir = target.position - transform.position;
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, speed * Time.deltaTime);
            transform.position = positionTarget.position;



            if (isGrabbed &&GameCanvas.Instance.Tapped && BlueBattery > 0)
            {
                BlueBattery = BlueBattery - Time.deltaTime * BatterySpendNumber *0.25f;
               
                var directionLeft2 = Quaternion.AngleAxis(20, aimPoint.transform.right * -1) * Vector3.forward;
                var directionLeft = Quaternion.AngleAxis(10, aimPoint.transform.right * -1) * Vector3.forward;
                var directionForward = aimPoint.TransformDirection(Vector3.forward);
                var directionRight = Quaternion.AngleAxis(10, aimPoint.transform.right) * Vector3.forward;
                var directionRight2 = Quaternion.AngleAxis(20, aimPoint.transform.right) * Vector3.forward;
                
            }
            else if (BlueBattery < 100)
            {
                BlueBattery = BlueBattery + Time.deltaTime * BatterySpendNumber;
                if (BlueBattery > 100)
                {
                    BlueBattery = 100;
                }
            }
            if (BlueBattery <= 0)
            {
                
                Light.enabled = false;
             
            }
            GameCanvas.Instance.Image_BlueLight.fillAmount = (BlueBattery / 100);
        }
    }
}