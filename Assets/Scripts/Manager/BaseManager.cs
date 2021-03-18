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

    /// <summary>
    /// facade销毁时调用
    /// </summary>
    public virtual void OnDestroy()
    {

    }

    /// <summary>
    /// 每帧调用，在facade调用，用于异步响应主线程
    /// </summary>
    public virtual void Update()
    {

    }
}
