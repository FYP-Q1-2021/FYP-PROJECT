public class GeyserPool : ObjectPool
{
    public static GeyserPool Instance;

    private void Awake()
    {
        Instance = this;
    }
}
