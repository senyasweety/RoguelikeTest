using UnityEngine;

namespace Assets.UI
{
    public class CurtainView : MonoBehaviour
    {
        [SerializeField] private Curtain _curtain;

        private void Awake()
        {
            _curtain.gameObject.SetActive(true);
        }
    }
}