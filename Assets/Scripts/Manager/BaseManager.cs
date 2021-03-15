/// <summary>
/// Manager的基类
/// </summary>
public class BaseManager
{
    // manager获取facade单一实例
    protected Facade facade;

    public BaseManager(Facade facade)
    {
        this.facade = facade;
    }

    public virtual void OnInit()
    {

    }

    public virtual void OnDestroy()
    {

    }
}
