using ComponentsStore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentsStore.DataAccess.Configurations
{
    public class ComponentConfiguration : IEntityTypeConfiguration<ComponentEntity>
    {
        public void Configure(EntityTypeBuilder<ComponentEntity> builder)
        {
            builder.HasKey(x => x.id);
            builder.Property(b => b.name)
                .IsRequired();
            builder.Property(b => b.description)
                .IsRequired();
        }
    }
}
