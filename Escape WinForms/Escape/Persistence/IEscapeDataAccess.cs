using System;
using System.Threading.Tasks;

namespace Escape.Persistence
{
    public interface IEscapeDataAccess
    {
        Task<EscapeTable> LoadAsync(string path);
        Task SaveAsync(string path, EscapeTable table);
    }
}