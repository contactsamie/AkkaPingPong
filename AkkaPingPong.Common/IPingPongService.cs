using System.Threading.Tasks;

namespace AkkaPingPong.Common
{
    public interface IPingPongService
    {
        void Execute();
        Task<bool> ExecuteAsync();
    }
}