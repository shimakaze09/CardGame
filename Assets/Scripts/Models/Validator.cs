using TheLiquidFire.Notifications;

public class Validator
{
    public Validator()
    {
        isValid = true;
    }

    public bool isValid { get; private set; }

    public void Invalidate()
    {
        isValid = false;
    }
}

public static class ValidatorExtensions
{
    public static bool Validate(this object target)
    {
        var validator = new Validator();
        var notificationName = Global.ValidateNotification(target.GetType());
        target.PostNotification(notificationName, validator);
        return validator.isValid;
    }
}