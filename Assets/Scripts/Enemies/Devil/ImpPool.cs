public class ImpPool : ObjectPool
{
    public static ImpPool Instance;

    private void Awake()
    {
        Instance = this;
    }
}
