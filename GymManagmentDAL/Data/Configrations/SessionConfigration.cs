using GymManagmentDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagmentDAL.Data.Configrations
{
    public class SessionConfigration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable("Sessions");

            builder.HasOne(x => x.SessionCategory)
                   .WithMany(X => X.Sessions)
                   .HasForeignKey(x => x.CategoryId);

            builder.HasOne(x => x.SessionTrainer)
                   .WithMany(x => x.TrainerSession)
                   .HasForeignKey(x => x.TrainerId);
        }
    }
}