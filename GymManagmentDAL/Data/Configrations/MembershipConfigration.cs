using GymManagmentDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagmentDAL.Data.Configrations
{
    public class MembershipConfigration : IEntityTypeConfiguration<MemberShip>
    {
        public void Configure(EntityTypeBuilder<MemberShip> builder)
        {
            builder.Property(X => X.CreatedAt).HasColumnName("StartDate").HasDefaultValueSql("GETDATE()");


            builder.HasKey(x => x.Id);

        }
    }
}