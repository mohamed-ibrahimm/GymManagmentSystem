using GymManagmentDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagmentDAL.Data.Configrations
{
    public class HealthRecordConfigration : IEntityTypeConfiguration<HealthRecord>
    {
        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {
            builder.ToTable("Members").HasKey(X => X.Id);

            builder.HasOne<Member>().WithOne(X => X.healthRecord).HasForeignKey<HealthRecord>(x => x.Id);
            builder.Ignore(x => x.CreatedAt);

        }
    }
}
