using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Fight
{
    public class ElementInfoLine : MonoBehaviour, IElementInfoLine
    {
        [SerializeField] private Element.Element _element;

        [SerializeField] private LineInfo _lineInfo;

        public Element.Element Element => _element;
        public ILineInfo InfoInLine => _lineInfo;

        [Serializable]
        class LineInfo : ILineInfo
        {
            [SerializeField] private TMP_Text _damage;
            [SerializeField] private TMP_Text _chanceToSplash;
            [SerializeField] private TMP_Text _chanceCriticalDamage;
            [SerializeField] private TMP_Text _valueModifier;
            [SerializeField] private Button _buttonAttack;

            public TMP_Text Damage => _damage;

            public TMP_Text ChanceToSplash => _chanceToSplash;

            public TMP_Text ChanceCriticalDamage => _chanceCriticalDamage;

            public TMP_Text ValueModifier => _valueModifier;

            public Button ButtonAttack => _buttonAttack;
        }

    }

    public interface ILineInfo
    {
        public TMP_Text Damage { get; }

        public TMP_Text ChanceToSplash { get; }

        public TMP_Text ChanceCriticalDamage { get; }

        public TMP_Text ValueModifier { get; }
        public Button ButtonAttack { get; }
    }

    public interface IElementInfoLine
    {
        public Element.Element Element { get; }

        public ILineInfo InfoInLine { get; }
    }
}