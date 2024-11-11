namespace UserRoleAPI.BusinessLayer.Services
{
    public interface ILogService
    {
        void Log(string level, string message, object exception = null, object properties = null);
    }

}