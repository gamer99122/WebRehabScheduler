namespace WebRehabScheduler.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SkipTherapistAuthorizationAttribute : Attribute
    {
    }
}
