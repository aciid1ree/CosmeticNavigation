using ComponentsStore.DataAccess.Entities;
using CosmeticsStore.Core.Abstractions;
using CosmeticsStore.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace ComponentsStore.DataAccess.Repositories
{
    public class ComponentsRepository : IComponentsRepository
    {
        private readonly ComponentsStoreDbContext _context;
        public ComponentsRepository(ComponentsStoreDbContext context)
        {
            _context = context;
        }
        public async Task<List<Components>> GetAll()
        {
            var ComponentsEntities = await _context.Components
                .AsNoTracking()
                .ToListAsync();
            var components = ComponentsEntities
                .Select(b => Components.Create(b.id, b.name, b.description).Component)
                .ToList();
            return components;
        }
        public async Task<List<Components>> GetAnswer(IEnumerable<string> searchItems)
        {
            var query = new StringBuilder();
            foreach (var item in searchItems)
            {
                query.Append($"name IS NOT NULL AND '{item}' % name OR ");
            }
            var sql = $@"
                SELECT id, name, description
                FROM ""Components""
                WHERE {query.ToString().TrimEnd(" OR ".ToCharArray())}
                LIMIT 50;
            ";
            var componentsEntities = await _context.Components
                .FromSqlRaw(sql)
                .ToListAsync();
            var components = componentsEntities
                .Select(c => Components.Create(c.id, c.name, c.description).Component)
                .ToList();
            return components;
        }
        public async Task<Guid> Create(Components component)
        {
            var componentsEntity = new ComponentEntity
            {
                id = component.id,
                name = component.name,
                description = component.description,
            };
            await _context.Components.AddAsync(componentsEntity);
            await _context.SaveChangesAsync();
            return component.id;
        }

    }
}
