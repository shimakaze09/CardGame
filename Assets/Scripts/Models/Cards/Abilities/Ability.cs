using TheLiquidFire.AspectContainer;

public class Ability : Container, IAspect
{
    public Card card => container as Card;
    public string actionName { get; set; }
    public object userInfo { get; set; }
    public IContainer container { get; set; }
}