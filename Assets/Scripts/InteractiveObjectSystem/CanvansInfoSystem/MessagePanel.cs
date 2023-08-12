using System;
using System.Linq;
using Assets.Enemy;
using Assets.Interface;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.InteractiveObjectSystem.CanvasInfoSystem
{
    public class MessagePanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _lable;
        [SerializeField] private TMP_Text _damageInfo;
        [SerializeField] private TMP_Text _armorInfo;
        [SerializeField] private TMP_Text _countInfo;
        [SerializeField] private Image _attactElementImage;
        [SerializeField] private Image _defendElementImage;
        [SerializeField] private ElementsSpriteView _elementsSprite;
        [SerializeField] private Button _buttonYes;
        [SerializeField] private Button _buttonNo;
        [SerializeField] private GameObject _panel;

        private const string DamageInfo = "Урон: ";
        private const string ArmorInfo = "Броня: ";
        private const string CountInfo = "Количество: ";
        public event Action<bool> UserResponse;

        private void Awake()
        {
            _buttonYes.onClick.AddListener(CallPositiveResponse);
            _buttonNo.onClick.AddListener(CallNegativeResponse);
            _panel.SetActive(false);
        }

        private void OnDestroy()
        {
            _buttonYes.onClick.RemoveListener(CallPositiveResponse);
            _buttonNo.onClick.RemoveListener(CallNegativeResponse);
        }
        
        public void ShowPanel(InteractiveObject interactiveObject)
        {
            gameObject.SetActive(true);

            _attactElementImage.gameObject.SetActive(false);
            _defendElementImage.gameObject.SetActive(false);

            if (interactiveObject.TryGetComponent(out EnemyPresenter enemyPresenter))
                ShowEnemyPanel(enemyPresenter);
            else if (interactiveObject.TryGetComponent(out InteractiveLootObject lootObject))
                ShowRandomEventPanel(lootObject);
            else if (interactiveObject.TryGetComponent(out InteractiveRandomEventObject randomEventObject))
                ShowRandomEventPanel(randomEventObject);

            _panel.SetActive(true);
        }

        private void ShowRandomEventPanel(InteractiveRandomEventObject randomEventObject)
        {
            // _lable.text = data.Name;
            //
            // _fullInfo.text = data.Data;
            // _fullInfo.gameObject.SetActive(true);
        }

        private void ShowRandomEventPanel(InteractiveLootObject lootObject)
        {
            // _lable.text = data.Name;
            //
            // _fullInfo.text = data.Data;
            // _fullInfo.gameObject.SetActive(true);
        }

        private void ShowEnemyPanel(EnemyPresenter enemyPresenter)
        {
            _lable.text = null;
            
            if (enemyPresenter.Enemy.Select(x => x.IsBoss).FirstOrDefault())
                _lable.text = "BOSS: ";

            _lable.text += enemyPresenter.EnemyView.Name;
            _damageInfo.text = DamageInfo + enemyPresenter.Enemy.Select(x => x.Weapon).FirstOrDefault().Damage;
            
            int armorValue = (int)enemyPresenter.Enemy.Select(x => x.Armor).FirstOrDefault().Body.Value + (int)enemyPresenter.Enemy.Select(x => x.Armor).FirstOrDefault().Body.Value; 

            _armorInfo.text = ArmorInfo + armorValue;
            _countInfo.text = CountInfo + enemyPresenter.Enemy.Count;
            
            _attactElementImage.gameObject.SetActive(true);
            _defendElementImage.gameObject.SetActive(true);

            _attactElementImage.sprite = _elementsSprite.GetElementSprite(enemyPresenter.Enemy.Select(x => x.Weapon.Element).FirstOrDefault());
            _defendElementImage.sprite = _elementsSprite.GetElementSprite(enemyPresenter.Enemy.Select(x => x.Armor.Body.Element).FirstOrDefault());
        }

        #region ShowPanel
        // private void ShowBossPanel(InteractiveBossObject bossObject)
        // {
        //     _lable.text =  $"BOSS: {bossObject.name}";
        //     
        //     _lable.text = bossObject.Name;
        //     _damageInfo.text = DamageInfo + bossObject.Enemy.Select(x => x.Weapon).FirstOrDefault().Damage;
        //     
        //     int armorValue = (int)bossObject.Enemy.Select(x => x.Armor).FirstOrDefault().Body.Value + (int)bossObject.Enemy.Select(x => x.Armor).FirstOrDefault().Body.Value; 
        //
        //     _armorInfo.text = ArmorInfo + armorValue;
        //     _countInfo.text = CountInfo + bossObject.Enemy.Count;
        //     
        //     _attactElementImage.gameObject.SetActive(true);
        //     _defendElementImage.gameObject.SetActive(true);
        //
        //     _attactElementImage.sprite = _elementsSprite.GetElementSprite(bossObject.Enemy.Select(x => x.Weapon.Element).FirstOrDefault());
        //     _defendElementImage.sprite = _elementsSprite.GetElementSprite(bossObject.Enemy.Select(x => x.Armor.Body.Element).FirstOrDefault());
        // }

        

        // public void ShowPanel(ObjectType objectType, InteractiveObjectData data)
        // {
        //     _enemyInfo.gameObject.SetActive(false);
        //     _fullInfo.gameObject.SetActive(false);
        //     _elementImage.gameObject.SetActive(false);
        //
        //     switch (objectType)
        //     {
        //         case ObjectType.Enemy:
        //             ShowEnemyPanel(data);
        //             break;
        //         case ObjectType.RandomEvent:
        //             ShowRandomEventPanel(data);
        //             break;
        //         case ObjectType.Loot:
        //             ShowRandomEventPanel(data);
        //             break;
        //         case ObjectType.Boos:
        //             ShowBossPanel(data);
        //             break;
        //         default:
        //             throw new ArgumentOutOfRangeException();
        //     }
        //
        //     _panel.SetActive(true);
        // }
        // private void ShowEnemyPanel(InteractiveObjectData data)
        // {
        //     _lable.text = data.Name;
        //
        //     _enemyInfo.text = data.Data;
        //     _enemyInfo.gameObject.SetActive(true);
        //
        //     _elementImage.sprite = _elementsSprite.GetElementSprite(data.Element);
        //     _elementImage.gameObject.SetActive(true);
        // }
        // private void ShowBossPanel(InteractiveObjectData data)
        // {
        //     _lable.text = $"BOSS: {data.Name}";
        //
        //     _enemyInfo.text = data.Data;
        //     _enemyInfo.gameObject.SetActive(true);
        //
        //     _elementImage.sprite = _elementsSprite.GetElementSprite(data.Element);
        //     _elementImage.gameObject.SetActive(true);
        // }
        // private void ShowRandomEventPanel(InteractiveObjectData data)
        // {
        //     _lable.text = data.Name;
        //
        //     _fullInfo.text = data.Data;
        //     _fullInfo.gameObject.SetActive(true);
        // }
        #endregion

        private void CallPositiveResponse()
        {
            UserResponse?.Invoke(true);
            _panel.SetActive(false);
        }

        private void CallNegativeResponse()
        {
            UserResponse?.Invoke(false);
            _panel.SetActive(false);
        }
    }
}