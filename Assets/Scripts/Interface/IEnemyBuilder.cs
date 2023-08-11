namespace Assets.Enemy
{
    public interface IEnemyBuilder
    {
        Enemy BuildMedic();

        Enemy BuildEasyUnit();

        Enemy BuildNormalUnit();

        Enemy BuildBoss();
    }
}