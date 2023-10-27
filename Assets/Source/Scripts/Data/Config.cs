public class Config
{
    public Config(WeaponDataSO weaponConfig, ArrowDataSO arrowConfig)
    {
        WeaponConfig = weaponConfig;
        ArrowConfig = arrowConfig;

        Instance = this;
    }

    public static Config Instance { get; private set; }
    public WeaponDataSO WeaponConfig {  get; private set; }
    public ArrowDataSO ArrowConfig { get; private set; }
    public int CoinsForEnemy { get; private set; } = 10;
}