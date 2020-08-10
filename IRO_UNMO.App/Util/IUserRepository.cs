
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace IRO_UNMO.App.Util
{
    public interface IUserRepository
    {
        void LogCurrentUser();
    }
}