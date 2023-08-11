namespace Assets.Fight.Element
{
    public static class ElementManager
    {
        private static readonly float _normalModifier = 1f;
        private static readonly float _increasedModifier = 2f;
        private static readonly float _loweredModifier = 0.5f;

        public static float GetDamageModifier(Element attack, Element defender)
        {
            if (attack == defender)
                return _normalModifier;

            int firstStep = 1;
            int secondStep = 2;
            int positiveFirstStep = 4;
            int positiveSecondStep = 3;

            bool firstCondition = (defender - firstStep >= 0 ? defender - firstStep : defender + positiveFirstStep) == attack;
            bool secondCondition = (defender - secondStep >= 0 ? defender - secondStep : defender + positiveSecondStep) == attack;

            if (firstCondition || secondCondition)
                return _increasedModifier;

            return _loweredModifier;
        }
    }

    public enum Element
    {
        Fire = 0,
        Tree = 1,
        Water = 2,
        Metal = 3,
        Stone = 4,
    }
}