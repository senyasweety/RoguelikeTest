using System;
using Assets.Fight;
using Assets.Fight.Element;
using Assets.Person;
using UnityEngine;

namespace Assets.Utils
{
    public class EnemyViewChooser : IDisposable
    {
        private readonly IElementsDamagePanel _elementsDamagePanel;
        private UnitAttackView _attackView;
        private bool _haveClick;
        private Element _element;

        public EnemyViewChooser(IElementsDamagePanel elementsDamagePanel)
        {
            _elementsDamagePanel = elementsDamagePanel;
            _elementsDamagePanel.Exit.onClick.AddListener(HidePanel);
            _elementsDamagePanel.FireElementInfoLine.InfoInLine.ButtonAttack.onClick.AddListener(() =>
                GetTrue(_elementsDamagePanel.FireElementInfoLine.Element));
            _elementsDamagePanel.MetalElementInfoLine.InfoInLine.ButtonAttack.onClick.AddListener(() =>
                GetTrue(_elementsDamagePanel.MetalElementInfoLine.Element));
            _elementsDamagePanel.StoneElementInfoLine.InfoInLine.ButtonAttack.onClick.AddListener(() =>
                GetTrue(_elementsDamagePanel.StoneElementInfoLine.Element));
            _elementsDamagePanel.TreeElementInfoLine.InfoInLine.ButtonAttack.onClick.AddListener(() =>
                GetTrue(_elementsDamagePanel.TreeElementInfoLine.Element));
            _elementsDamagePanel.WaterElementInfoLine.InfoInLine.ButtonAttack.onClick.AddListener(() =>
                GetTrue(_elementsDamagePanel.WaterElementInfoLine.Element));
        }

        public Element Element => _element;
        public UnitAttackView AttackView => _attackView;

        public void Dispose()
        {
            _elementsDamagePanel.FireElementInfoLine.InfoInLine.ButtonAttack.onClick.RemoveListener(() =>
                GetTrue(_elementsDamagePanel.FireElementInfoLine.Element));
            _elementsDamagePanel.MetalElementInfoLine.InfoInLine.ButtonAttack.onClick.RemoveListener(() =>
                GetTrue(_elementsDamagePanel.MetalElementInfoLine.Element));
            _elementsDamagePanel.StoneElementInfoLine.InfoInLine.ButtonAttack.onClick.RemoveListener(() =>
                GetTrue(_elementsDamagePanel.StoneElementInfoLine.Element));
            _elementsDamagePanel.TreeElementInfoLine.InfoInLine.ButtonAttack.onClick.RemoveListener(() =>
                GetTrue(_elementsDamagePanel.TreeElementInfoLine.Element));
            _elementsDamagePanel.WaterElementInfoLine.InfoInLine.ButtonAttack.onClick.RemoveListener(() =>
                GetTrue(_elementsDamagePanel.WaterElementInfoLine.Element));
        }

        public bool TryChooseEnemy()
        {
            if (TryGetEnemyView(out UnitAttackView unitAttackView))
            {
                _attackView = unitAttackView;
                _elementsDamagePanel.ShowPanel();
            }

            return _haveClick;
        }

        private void HidePanel() =>
            _elementsDamagePanel.HidePanel();

        // Переименовать
        private void GetTrue(Element element)
        {
            _element = element;
            _haveClick = true;
            HidePanel();
        }

        private bool TryGetEnemyView(out UnitAttackView data)
        {
            data = null;

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

                if (hit.collider != null && hit.collider.TryGetComponent(out UnitAttackView selectedObject))
                {
                    data = selectedObject;
                    return true;
                }
            }

            return false;
        }
    }
}