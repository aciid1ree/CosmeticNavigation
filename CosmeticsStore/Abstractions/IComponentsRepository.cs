using CosmeticsStore.Models;

namespace CosmeticsStore.Core.Abstractions
{
    public interface IComponentsRepository
    {
        Task<Guid> Create(Components Component);
        Task <List<Components>> GetAll();
        Task<List<Components>> GetAnswer(IEnumerable<string> textArray);
    }
}
