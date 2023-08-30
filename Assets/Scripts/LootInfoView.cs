using System;
using Assets.Scripts.InteractiveObjectSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class LootInfoView : InfoView
    {
        [Header("блок атакующего элемента")]
        [SerializeField] private Image _attackElement;
        [SerializeField] private Image _attackImage;

        [Header("блок защитного элемента")]
        [SerializeField] private Image _defendElement;
        [SerializeField] private Image _defendImage;

        [Header("блок названия")]
        [SerializeField] private TMP_Text _name;
        
        [Header("блок описания")]
        [SerializeField] private TMP_Text _data;
        
        public void Show(InteractiveLootObject lootObject)
        {
            gameObject.SetActive(true);
            // _lable.text = data.Name;
            //
            // _fullInfo.text = data.Data;
            // _fullInfo.gameObject.SetActive(true);
        }
    }
}