public class Config
{
    public Config(WeaponConfigSO weaponConfig, ArrowConfigSO arrowConfig)
    {
        WeaponConfig = weaponConfig;
        ArrowConfig = arrowConfig;

        Instance = this;
    }

    public static Config Instance { get; private set; }
    public WeaponConfigSO WeaponConfig {  get; private set; }
    public ArrowConfigSO ArrowConfig { get; private set; }
}