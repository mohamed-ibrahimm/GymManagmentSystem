using GymManagmentDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagmentDAL.Data.Configrations
{
    public class MemberSessionConfigration : IEntityTypeConfiguration<MemberSession>
    {
        public void Configure(EntityTypeBuilder<MemberSession> builder)
        {
            builder.Property(X => X.CreatedAt).HasColumnName("BookingDate").HasDefaultValueSql("GETDATE()");

            builder.HasKey(x => new
            {
                x.MemberId,
                x.SessionId
            });
            builder.Ignore(X => X.Id);
        }
    }
}
