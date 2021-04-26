public class CrystalPool : ObjectPool
{
    public static CrystalPool Instance;

    private void Awake()
    {
        Instance = this;
    }
}
