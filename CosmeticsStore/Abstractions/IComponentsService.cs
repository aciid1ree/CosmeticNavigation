using CosmeticsStore.Models;

namespace ComponentsStore.Application.Service
{
    public interface IComponentsService
    {
        Task<Guid> CreateComponents(Components component);
        Task<List<Components>> GetAllComponents();
        Task<List<Components>> GetAnswerComponents(IEnumerable<string> textArray);
    }
}