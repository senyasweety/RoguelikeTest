using System.Collections.Generic;

namespace Assets.Person.PersonStates
{
    public class UnitStateAttack : IUnitState
    {
        private readonly Unit _player;
        //private readonly Unit[] _enemies;

        public UnitStateAttack(Unit player, IEnumerable<Unit> enemies)
        {
            _player = player;
            //_enemies = enemies;
        }

        public void Exit()
        {
            throw new System.NotImplementedException();
        }

        public void Enter()
        {
            throw new System.NotImplementedException();
        }
    }
}