using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.Widgets
{
    public class RaycastTarget : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            // Обработка события клика на панели
            Debug.Log("Панель была нажата");
        }
    }
}