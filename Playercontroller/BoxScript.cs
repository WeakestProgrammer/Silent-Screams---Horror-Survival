using UnityEngine;

namespace AdvancedHorrorFPS
{
    public class BoxScript : MonoBehaviour
    {
        public bool isHolding = false;
        Rigidbody rigidbody;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        public void Interact()
        {
            if (!isHolding && !HeroPlayerScript.Instance.isHoldingBox)
            {
                isHolding = true;
                HeroPlayerScript.Instance.isHoldingBox = true;
                rigidbody.isKinematic = true;
                rigidbody.useGravity = false;
                transform.parent = TouchpadFPSLook.Instance.transform;
                transform.localPosition = transform.localPosition + transform.up * 0.2f;
                AudioManager.Instance.Play_Item_Grab();
            }
            else if(isHolding && HeroPlayerScript.Instance.isHoldingBox)
            {
                isHolding = false;
                HeroPlayerScript.Instance.isHoldingBox = false;
                rigidbody.isKinematic = false;
                rigidbody.useGravity = true;
                transform.parent = null;
                AudioManager.Instance.Play_Item_Grab();
            }
        }
    }
}
