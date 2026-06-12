using System.Text;
using Data.Entities.Generated;
using Data.Entities.Enums;
using Data.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Data.Context;

public partial class AstroClubDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public AstroClubDbContext(DbContextOptions<AstroClubDbContext> options)
        : base(options)
    {
    }

    // ── Identity companions ────────────────────────────────────────────────
    public virtual DbSet<MemberRoleAudit> MemberRoleAudits { get; set; }
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
    public virtual DbSet<MemberContactPref> MemberContactPrefs { get; set; }

    // ── Equipment ─────────────────────────────────────────────────────────
    public virtual DbSet<EquipmentCategory> EquipmentCategories { get; set; }
    public virtual DbSet<EquipmentBrand> EquipmentBrands { get; set; }
    public virtual DbSet<EquipmentModel> EquipmentModels { get; set; }
    public virtual DbSet<Equipment> Equipments { get; set; }
    public virtual DbSet<EquipmentMaintenance> EquipmentMaintenances { get; set; }
    public virtual DbSet<EquipmentUpload> EquipmentUploads { get; set; }
    public virtual DbSet<EquipmentCompatibility> EquipmentCompatibilities { get; set; }

    // ── Observation sites ─────────────────────────────────────────────────
    public virtual DbSet<ObservationSite> ObservationSites { get; set; }
    // ── Celestial catalog ─────────────────────────────────────────────────
    public virtual DbSet<Target> Targets { get; set; }

    // ── Observations ──────────────────────────────────────────────────────
    public virtual DbSet<ObservationSessionType> ObservationSessionTypes { get; set; }
    public virtual DbSet<ObservationType> ObservationTypes { get; set; }
    public virtual DbSet<DataproductType> DataproductTypes { get; set; }
    public virtual DbSet<ObservationSession> ObservationSessions { get; set; }
    public virtual DbSet<SessionMember> SessionMembers { get; set; }
    public virtual DbSet<Observation> Observations { get; set; }

    // ── Astrophotography ──────────────────────────────────────────────────
    public virtual DbSet<ImageRecord> ImageRecords { get; set; }

    // ── Project management ────────────────────────────────────────────────
    public virtual DbSet<ProjectType> ProjectTypes { get; set; }
    public virtual DbSet<Project> Projects { get; set; }
    public virtual DbSet<Milestone> Milestones { get; set; }
    public virtual DbSet<TaskType> TaskTypes { get; set; }
    public virtual DbSet<Data.Entities.Generated.Task> Tasks { get; set; }
    public virtual DbSet<TaskAssignment> TaskAssignments { get; set; }
    public virtual DbSet<ProjectMember> ProjectMembers { get; set; }


    // ── Events & calendar ─────────────────────────────────────────────────
    public virtual DbSet<EventType> EventTypes { get; set; }
    public virtual DbSet<Event> Events { get; set; }
    public virtual DbSet<EventVisibility> EventVisibilities { get; set; }
    public virtual DbSet<EventObservation> EventObservations { get; set; }


    // ── Forecasts ─────────────────────────────────────────────────────────
    public virtual DbSet<ForecastCategory> ForecastCategories { get; set; }
    public virtual DbSet<Forecast> Forecasts { get; set; }
    public virtual DbSet<ForecastProject> ForecastProjects { get; set; }

    // ── Notifications ─────────────────────────────────────────────────────
    public virtual DbSet<NotificationLog> NotificationLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.HasOne(u => u.CreatedByUser)
                .WithMany()
                .HasForeignKey(u => u.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);
            entity.Property(u => u.JoinDate)
                .HasConversion(
                    v => v.ToDateTime(TimeOnly.MinValue),
                    v => DateOnly.FromDateTime(v));
        });

        modelBuilder.Entity<RefreshToken>(entity =>
       {
           entity.HasKey(e => e.TokenId);
           entity.HasOne(d => d.User)
                 .WithMany(p => p.RefreshTokens)
                 .HasForeignKey(e => e.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
       });

        modelBuilder.Entity<DataproductType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_DPT");
        });


        modelBuilder.Entity<EquipmentUpload>(entity =>
        {
            entity.HasOne(d => d.Equipment).WithMany(p => p.EquipmentUploads)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EQ_Upload");
        });
        modelBuilder.Entity<EquipmentModel>(entity =>
        {
            entity.HasOne(d => d.EquipmentCategory).WithMany(p => p.EquipmentModels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EQM_Category");

            entity.HasOne(d => d.EquipmentBrand).WithMany(p => p.EquipmentModels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EQM_Brand");
        });
        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.Property(e => e.Status).HasDefaultValue(EquipmentStatusEnum.Operational);

            entity.HasOne(d => d.EquipmentModel).WithMany(p => p.Equipments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EQ_Model");
        });

        modelBuilder.Entity<EquipmentMaintenance>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.Equipment).WithMany(p => p.EquipmentMaintenances)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EM_Equipment");
        });
        modelBuilder.Entity<EquipmentCompatibility>(entity =>
        {
            entity.HasKey(e => new { e.AccessoryId, e.CompatibleWithId });

            entity.HasOne(ec => ec.Accessory)
                .WithMany(em => em.Compatibilities)
                .HasForeignKey(ec => ec.AccessoryId)
                .OnDelete(DeleteBehavior.NoAction);   

            entity.HasOne(ec => ec.CompatibleWith)
                .WithMany(em => em.CompatibleWith)
                .HasForeignKey(ec => ec.CompatibleWithId)
                .OnDelete(DeleteBehavior.NoAction);   
        });
        modelBuilder.Entity<Event>(entity =>
        {
            entity.Property(e => e.AlertDaysBefore).HasDefaultValue((byte)7);
            entity.Property(e => e.Constellation).IsFixedLength();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.EventType).WithMany(p => p.Events)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EV_EventType");

            entity.HasOne(d => d.Target).WithMany(p => p.Events).HasConstraintName("FK_EV_Target");
        });

        modelBuilder.Entity<EventObservation>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.Event).WithMany(p => p.EventObservations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EO_Event");

            entity.HasOne(d => d.Observation).WithMany(p => p.EventObservations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EO_Observation");
        });

        modelBuilder.Entity<EventVisibility>(entity =>
        {
            entity.Property(e => e.ComputedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.Event).WithMany(p => p.EventVisibilities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EV2_Event");

            entity.HasOne(d => d.Site).WithMany(p => p.EventVisibilities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EV2_Site");
        });

        modelBuilder.Entity<Forecast>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.Status).HasDefaultValue(ForecastStatusEnum.Proposed);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.Category).WithMany(p => p.Forecasts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FCST_Category");
        });

        modelBuilder.Entity<ForecastProject>(entity =>
        {
            entity.HasOne(d => d.Forecast).WithMany(p => p.ForecastProjects)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FP_Forecast");

            entity.HasOne(d => d.Project).WithMany(p => p.ForecastProjects)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FP_Project");
        });

        modelBuilder.Entity<ImageRecord>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.PublicationStatus).HasDefaultValue(ImagePublicationStatusEnum.Raw);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.Observation).WithMany(p => p.ImageRecords)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IR_Observation");

            entity.HasOne(d => d.Target).WithMany(p => p.ImageRecords)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IR_Target");
        });

        modelBuilder.Entity<MemberContactPref>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.HasOne<ApplicationUser>()
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<MemberRoleAudit>(entity =>
        {
            entity.Property(e => e.ActionDate).HasDefaultValueSql("(sysdatetimeoffset())");
        });

        modelBuilder.Entity<Milestone>(entity =>
        {
            entity.Property(e => e.SortOrder).HasDefaultValue((short)1);

            entity.HasOne(d => d.Project).WithMany(p => p.Milestones)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MS_Project");
        });

        modelBuilder.Entity<NotificationLog>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.Status).HasDefaultValue(NotificationStatusEnum.Pending);
        });

        modelBuilder.Entity<Observation>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.Timesys).HasDefaultValue(ObservationTimeSystemEnum.UTC);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.Camera).WithMany(p => p.ObservationCameras).HasConstraintName("FK_OBS_Camera");

            entity.HasOne(d => d.DataproductType).WithMany(p => p.Observations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OBS_DPType");

            entity.HasOne(d => d.Filter).WithMany(p => p.ObservationFilters).HasConstraintName("FK_OBS_Filter");

            entity.HasOne(d => d.Guider).WithMany(p => p.ObservationGuiders).HasConstraintName("FK_OBS_Guider");

            entity.HasOne(d => d.Mount).WithMany(p => p.ObservationMounts).HasConstraintName("FK_OBS_Mount");

            entity.HasOne(d => d.ObservationType).WithMany(p => p.Observations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OBS_ObsType");

            entity.HasOne(d => d.Session).WithMany(p => p.Observations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OBS_Session");

            entity.HasOne(d => d.Target).WithMany(p => p.Observations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OBS_Target");

            entity.HasOne(d => d.Telescope).WithMany(p => p.ObservationTelescopes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OBS_Telescope");
        });

        modelBuilder.Entity<ObservationSession>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.Status).HasDefaultValue(ObservationSessionStatusEnum.Planned);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.SessionType).WithMany(p => p.ObservationSessions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SES_SessionType");

            entity.HasOne(d => d.Site).WithMany(p => p.ObservationSessions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SES_Site");
        });

        modelBuilder.Entity<ObservationSessionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_OST");
        });

        modelBuilder.Entity<ObservationSite>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
        });

        modelBuilder.Entity<ObservationType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_OT");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.Priority).HasDefaultValue(ProjectPriorityEnum.Medium);
            entity.Property(e => e.Status).HasDefaultValue(ProjectStatusEnum.Draft);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.Visibility).HasDefaultValue(ProjectVisibilityEnum.MembersOnly);

            entity.HasOne(d => d.ProjectType).WithMany(p => p.Projects)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PRJ_Type");

            entity.HasOne(d => d.Target).WithMany(p => p.Projects).HasConstraintName("FK_PRJ_Target");
        });

        modelBuilder.Entity<ProjectMember>(entity =>
        {
            entity.HasOne(d => d.Project).WithMany(p => p.ProjectMembers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PM_Project");
        });

        modelBuilder.Entity<SessionMember>(entity =>
        {
            entity.HasOne(d => d.Session).WithMany(p => p.SessionMembers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SM_Session");
        });

        modelBuilder.Entity<Target>(entity =>
        {
            entity.Property(e => e.Constellation).IsFixedLength();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
        });

        modelBuilder.Entity<Data.Entities.Generated.Task>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.Priority).HasDefaultValue(TaskPriorityEnum.Medium);
            entity.Property(e => e.Status).HasDefaultValue(TaskStatusEnum.Backlog);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.Milestone).WithMany(p => p.Tasks).HasConstraintName("FK_TSK_Milestone");

            entity.HasOne(d => d.ParentTask).WithMany(p => p.InverseParentTask).HasConstraintName("FK_TSK_Parent");

            entity.HasOne(d => d.Project).WithMany(p => p.Tasks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TSK_Project");

            entity.HasOne(d => d.Session).WithMany(p => p.Tasks).HasConstraintName("FK_TSK_Session");

            entity.HasOne(d => d.TaskType).WithMany(p => p.Tasks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TSK_Type");
        });

        modelBuilder.Entity<TaskAssignment>(entity =>
        {
            entity.Property(e => e.AssignedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.Task).WithMany(p => p.TaskAssignments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TA_Task");
        });

    }

}
