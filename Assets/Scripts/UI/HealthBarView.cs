using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField] private Image _healthBar;

        public Image HealthBar => _healthBar;
    }
}