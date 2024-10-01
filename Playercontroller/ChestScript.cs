using UnityEngine;

namespace AdvancedHorrorFPS
{
    public class ChestScript : MonoBehaviour
    {
        public bool isOpened = false;
        public GameObject Cover;
        public Collider collider;
        public AnimationCurve ease;
        public GameObject itemInChest;

        private void Start()
        {
            if (itemInChest != null)
            {
                itemInChest.GetComponent<Collider>().enabled = false;
            }
        }
    }
}
