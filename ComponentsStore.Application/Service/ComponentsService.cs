using CosmeticsStore.Core.Abstractions;
using CosmeticsStore.Models;

namespace ComponentsStore.Application.Service
{
    public class ComponentsService : IComponentsService
    {
        private readonly IComponentsRepository _componentRepository;
        public ComponentsService(IComponentsRepository componentRepository)
        {
            _componentRepository = componentRepository;
        }
        public async Task<List<Components>> GetAllComponents()
        {
            return await _componentRepository.GetAll();
        }

        public async Task<Guid> CreateComponents(Components component)
        {
            return await _componentRepository.Create(component);
        }
        public async Task<List<Components>> GetAnswerComponents(IEnumerable<string> textArray)
        {
            return await _componentRepository.GetAnswer(textArray);
        }
    }
}
