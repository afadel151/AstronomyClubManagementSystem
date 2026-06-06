using System.Text;
using Data.Entities;
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
    public virtual DbSet<Equipment> Equipments { get; set; }
    public virtual DbSet<EquipmentMaintenance> EquipmentMaintenances { get; set; }
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
    public virtual DbSet<Data.Entities.Task> Tasks { get; set; }
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
            entity.Property(u => u.MemberStatus).HasConversion(CreateSnakeCaseEnumConverter<MemberStatusEnum>());
            entity.Property(u => u.JoinDate)
                .HasConversion(
                    v => v.ToDateTime(TimeOnly.MinValue),
                    v => DateOnly.FromDateTime(v));
        });

        modelBuilder.Entity<RefreshToken>(entity =>
       {
           entity.HasKey(e => e.TokenId);
           entity.HasIndex(e => e.Token).IsUnique().HasDatabaseName("UK_REFRESH_TOKEN");
           entity.HasIndex(e => e.UserId).HasDatabaseName("IX_RT_UserId");
           entity.HasIndex(e => e.IsActive).HasDatabaseName("IX_RT_IsActive");

           entity.HasOne(d => d.User)
                 .WithMany(p => p.RefreshTokens)
                 .HasForeignKey(e => e.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
       });

        modelBuilder.Entity<DataproductType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_DPT");
        });

        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.OpticalDesign).HasConversion(CreateNullableEquipmentOpticalDesignConverter());
            entity.Property(e => e.Status).HasDefaultValue(EquipmentStatusEnum.Operational);
            entity.Property(e => e.Status).HasConversion(CreateSnakeCaseEnumConverter<EquipmentStatusEnum>());
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.Category).WithMany(p => p.Equipment)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EQ_Category");
        });

        modelBuilder.Entity<EquipmentMaintenance>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.MaintenanceType).HasConversion(CreateSnakeCaseEnumConverter<EquipmentMaintenanceTypeEnum>());
            entity.Property(e => e.Result).HasConversion(CreateSnakeCaseEnumConverter<EquipmentMaintenanceResultEnum>());

            entity.HasOne(d => d.Equipment).WithMany(p => p.EquipmentMaintenances)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EM_Equipment");
        });
        modelBuilder.Entity<EquipmentCompatibility>(entity =>
        {
            entity.HasKey(e => new { e.EquipmentId, e.CompatibleWithId });

            entity.HasOne(ec => ec.Accessory)
                .WithMany()
                .HasForeignKey(ec => ec.EquipmentId)
                .OnDelete(DeleteBehavior.NoAction);   

            entity.HasOne(ec => ec.CompatibleEquipment)
                .WithMany()
                .HasForeignKey(ec => ec.CompatibleWithId)
                .OnDelete(DeleteBehavior.NoAction);   
        });
        modelBuilder.Entity<Event>(entity =>
        {
            entity.Property(e => e.AlertDaysBefore).HasDefaultValue((byte)7);
            entity.Property(e => e.Constellation).IsFixedLength();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.VisibilityGlobal).HasConversion(CreateNullableSnakeCaseEnumConverter<EventGlobalVisibilityEnum>());
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
            entity.Property(e => e.Status).HasConversion(CreateSnakeCaseEnumConverter<ForecastStatusEnum>());
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
            entity.Property(e => e.ImageType).HasConversion(CreateSnakeCaseEnumConverter<ImageTypeEnum>());
            entity.Property(e => e.PublicationStatus).HasConversion(CreateSnakeCaseEnumConverter<ImagePublicationStatusEnum>());
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
            entity.Property(e => e.Channel).HasConversion(CreateSnakeCaseEnumConverter<ContactChannelEnum>());
            entity.HasOne<ApplicationUser>()
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<MemberRoleAudit>(entity =>
        {
            entity.Property(e => e.ActionDate).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.Action).HasConversion(CreateSnakeCaseEnumConverter<MemberRoleAuditActionEnum>());
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
            entity.Property(e => e.Channel).HasConversion(CreateSnakeCaseEnumConverter<ContactChannelEnum>());
            entity.Property(e => e.Status).HasConversion(CreateSnakeCaseEnumConverter<NotificationStatusEnum>());
            entity.Property(e => e.Status).HasDefaultValue(NotificationStatusEnum.Pending);
        });

        modelBuilder.Entity<Observation>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.Timesys).HasConversion(CreateObservationTimeSystemConverter());
            entity.Property(e => e.MagnitudeSystem).HasConversion(CreateNullableMagnitudeSystemConverter());
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
            entity.Property(e => e.Status).HasConversion(CreateSnakeCaseEnumConverter<ObservationSessionStatusEnum>());
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
            entity.Property(e => e.SiteType).HasConversion(CreateSnakeCaseEnumConverter<ObservationSiteTypeEnum>());
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
        });

        modelBuilder.Entity<ObservationType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_OT");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.Priority).HasConversion(CreateSnakeCaseEnumConverter<ProjectPriorityEnum>());
            entity.Property(e => e.Priority).HasDefaultValue(ProjectPriorityEnum.Medium);
            entity.Property(e => e.Status).HasConversion(CreateSnakeCaseEnumConverter<ProjectStatusEnum>());
            entity.Property(e => e.Status).HasDefaultValue(ProjectStatusEnum.Draft);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.Visibility).HasConversion(CreateSnakeCaseEnumConverter<ProjectVisibilityEnum>());
            entity.Property(e => e.Visibility).HasDefaultValue(ProjectVisibilityEnum.MembersOnly);

            entity.HasOne(d => d.ProjectType).WithMany(p => p.Projects)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PRJ_Type");

            entity.HasOne(d => d.Target).WithMany(p => p.Projects).HasConstraintName("FK_PRJ_Target");
        });

        modelBuilder.Entity<ProjectMember>(entity =>
        {
            entity.Property(e => e.Role).HasConversion(CreateProjectMemberRoleConverter());
            entity.HasOne(d => d.Project).WithMany(p => p.ProjectMembers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PM_Project");
        });

        modelBuilder.Entity<SessionMember>(entity =>
        {
            entity.Property(e => e.SessionRole).HasConversion(CreateSnakeCaseEnumConverter<SessionMemberRoleEnum>());
            entity.HasOne(d => d.Session).WithMany(p => p.SessionMembers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SM_Session");
        });

        modelBuilder.Entity<Target>(entity =>
        {
            entity.Property(e => e.Constellation).IsFixedLength();
            entity.Property(e => e.MagnitudeSystem).HasConversion(CreateNullableMagnitudeSystemConverter());
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
        });

        modelBuilder.Entity<Data.Entities.Task>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.Priority).HasConversion(CreateSnakeCaseEnumConverter<TaskPriorityEnum>());
            entity.Property(e => e.Priority).HasDefaultValue(TaskPriorityEnum.Medium);
            entity.Property(e => e.Status).HasConversion(CreateSnakeCaseEnumConverter<TaskStatusEnum>());
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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    private static ValueConverter<TEnum, string> CreateSnakeCaseEnumConverter<TEnum>() where TEnum : struct, Enum
        => new(
            value => ConvertSnakeCaseEnumToProvider(value),
            value => ConvertSnakeCaseEnumFromProvider<TEnum>(value));

    private static ValueConverter<TEnum?, string?> CreateNullableSnakeCaseEnumConverter<TEnum>() where TEnum : struct, Enum
        => new(
            value => ConvertNullableSnakeCaseEnumToProvider(value),
            value => ConvertNullableSnakeCaseEnumFromProvider<TEnum>(value));

    private static ValueConverter<EquipmentOpticalDesignEnum?, string?> CreateNullableEquipmentOpticalDesignConverter()
        => new(
            value => ConvertNullableEquipmentOpticalDesignToProvider(value),
            value => ConvertNullableEquipmentOpticalDesignFromProvider(value));

    private static ValueConverter<ProjectMemberRoleEnum, string> CreateProjectMemberRoleConverter()
        => new(
            value => ConvertProjectMemberRoleToProvider(value),
            value => ConvertProjectMemberRoleFromProvider(value));

    private static ValueConverter<ObservationTimeSystemEnum, string> CreateObservationTimeSystemConverter()
        => new(
            value => ConvertObservationTimeSystemToProvider(value),
            value => ConvertObservationTimeSystemFromProvider(value));

    private static ValueConverter<MagnitudeSystemEnum?, string?> CreateNullableMagnitudeSystemConverter()
        => new(
            value => ConvertNullableMagnitudeSystemToProvider(value),
            value => ConvertNullableMagnitudeSystemFromProvider(value));

    private static string ConvertSnakeCaseEnumToProvider<TEnum>(TEnum value) where TEnum : struct, Enum
        => ToSnakeCase(value.ToString());

    private static TEnum ConvertSnakeCaseEnumFromProvider<TEnum>(string value) where TEnum : struct, Enum
        => ParseEnum<TEnum>(value);

    private static string? ConvertNullableSnakeCaseEnumToProvider<TEnum>(TEnum? value) where TEnum : struct, Enum
        => value.HasValue ? ToSnakeCase(value.Value.ToString()) : null;

    private static TEnum? ConvertNullableSnakeCaseEnumFromProvider<TEnum>(string? value) where TEnum : struct, Enum
        => value is null ? null : ParseEnum<TEnum>(value);

    private static string? ConvertNullableEquipmentOpticalDesignToProvider(EquipmentOpticalDesignEnum? value)
        => value is null ? null : ConvertEquipmentOpticalDesignToProvider(value.Value);

    private static EquipmentOpticalDesignEnum? ConvertNullableEquipmentOpticalDesignFromProvider(string? value)
        => value is null ? null : ConvertEquipmentOpticalDesignFromProvider(value);

    private static string ConvertEquipmentOpticalDesignToProvider(EquipmentOpticalDesignEnum value) => value switch
    {
        EquipmentOpticalDesignEnum.Newtonian => "Newtonian",
        EquipmentOpticalDesignEnum.Sct => "SCT",
        EquipmentOpticalDesignEnum.Refractor => "Refractor",
        EquipmentOpticalDesignEnum.RitcheyChretien => "Ritchey-Chrétien",
        EquipmentOpticalDesignEnum.MaksutovCassegrain => "Maksutov-Cassegrain",
        EquipmentOpticalDesignEnum.MaksutovNewtonian => "Maksutov-Newtonian",
        EquipmentOpticalDesignEnum.Astrograph => "Astrograph",
        EquipmentOpticalDesignEnum.Dobsonian => "Dobsonian",
        EquipmentOpticalDesignEnum.Cassegrain => "Cassegrain",
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
    };

    private static EquipmentOpticalDesignEnum ConvertEquipmentOpticalDesignFromProvider(string value) => value switch
    {
        "Newtonian" => EquipmentOpticalDesignEnum.Newtonian,
        "SCT" => EquipmentOpticalDesignEnum.Sct,
        "Refractor" => EquipmentOpticalDesignEnum.Refractor,
        "Ritchey-Chrétien" => EquipmentOpticalDesignEnum.RitcheyChretien,
        "Maksutov-Cassegrain" => EquipmentOpticalDesignEnum.MaksutovCassegrain,
        "Maksutov-Newtonian" => EquipmentOpticalDesignEnum.MaksutovNewtonian,
        "Astrograph" => EquipmentOpticalDesignEnum.Astrograph,
        "Dobsonian" => EquipmentOpticalDesignEnum.Dobsonian,
        "Cassegrain" => EquipmentOpticalDesignEnum.Cassegrain,
        _ => throw new InvalidOperationException($"Unsupported EquipmentOpticalDesignEnum value '{value}'.")
    };

    private static string ConvertProjectMemberRoleToProvider(ProjectMemberRoleEnum value) => value switch
    {
        ProjectMemberRoleEnum.Lead => "lead",
        ProjectMemberRoleEnum.CoLead => "co-lead",
        ProjectMemberRoleEnum.Contributor => "contributor",
        ProjectMemberRoleEnum.Reviewer => "reviewer",
        ProjectMemberRoleEnum.ObserverOnly => "observer_only",
        ProjectMemberRoleEnum.Advisor => "advisor",
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
    };

    private static ProjectMemberRoleEnum ConvertProjectMemberRoleFromProvider(string value) => value switch
    {
        "lead" => ProjectMemberRoleEnum.Lead,
        "co-lead" => ProjectMemberRoleEnum.CoLead,
        "contributor" => ProjectMemberRoleEnum.Contributor,
        "reviewer" => ProjectMemberRoleEnum.Reviewer,
        "observer_only" => ProjectMemberRoleEnum.ObserverOnly,
        "advisor" => ProjectMemberRoleEnum.Advisor,
        _ => throw new InvalidOperationException($"Unsupported ProjectMemberRoleEnum value '{value}'.")
    };

    private static string ConvertObservationTimeSystemToProvider(ObservationTimeSystemEnum value) => value switch
    {
        ObservationTimeSystemEnum.UTC => "UTC",
        ObservationTimeSystemEnum.TT => "TT",
        ObservationTimeSystemEnum.TDB => "TDB",
        ObservationTimeSystemEnum.TCB => "TCB",
        ObservationTimeSystemEnum.TAI => "TAI",
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
    };

    private static ObservationTimeSystemEnum ConvertObservationTimeSystemFromProvider(string value) => value switch
    {
        "UTC" => ObservationTimeSystemEnum.UTC,
        "TT" => ObservationTimeSystemEnum.TT,
        "TDB" => ObservationTimeSystemEnum.TDB,
        "TCB" => ObservationTimeSystemEnum.TCB,
        "TAI" => ObservationTimeSystemEnum.TAI,
        _ => throw new InvalidOperationException($"Unsupported ObservationTimeSystemEnum value '{value}'.")
    };

    private static string? ConvertNullableMagnitudeSystemToProvider(MagnitudeSystemEnum? value)
        => value.HasValue ? ConvertMagnitudeSystemToProvider(value.Value) : null;

    private static MagnitudeSystemEnum? ConvertNullableMagnitudeSystemFromProvider(string? value)
        => value is null ? null : ConvertMagnitudeSystemFromProvider(value);

    private static string ConvertMagnitudeSystemToProvider(MagnitudeSystemEnum value) => value switch
    {
        MagnitudeSystemEnum.Vega => "Vega",
        MagnitudeSystemEnum.AB => "AB",
        MagnitudeSystemEnum.ST => "ST",
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
    };

    private static MagnitudeSystemEnum ConvertMagnitudeSystemFromProvider(string value) => value switch
    {
        "Vega" => MagnitudeSystemEnum.Vega,
        "AB" => MagnitudeSystemEnum.AB,
        "ST" => MagnitudeSystemEnum.ST,
        _ => throw new InvalidOperationException($"Unsupported MagnitudeSystemEnum value '{value}'.")
    };

    private static string ToSnakeCase(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        var builder = new StringBuilder(value.Length + 8);
        for (var index = 0; index < value.Length; index++)
        {
            var current = value[index];
            if (char.IsUpper(current) && index > 0 && value[index - 1] != '_' && (char.IsLower(value[index - 1]) || (index + 1 < value.Length && char.IsLower(value[index + 1]))))
            {
                builder.Append('_');
            }

            builder.Append(char.ToLowerInvariant(current));
        }

        return builder.ToString();
    }

    private static TEnum ParseEnum<TEnum>(string? value) where TEnum : struct, Enum
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        var normalizedValue = NormalizeEnumValue(value);
        foreach (var enumName in Enum.GetNames<TEnum>())
        {
            if (NormalizeEnumValue(enumName) == normalizedValue)
            {
                return Enum.Parse<TEnum>(enumName);
            }
        }

        throw new InvalidOperationException($"Unsupported {typeof(TEnum).Name} value '{value}'.");
    }

    private static string NormalizeEnumValue(string value)
        => new string(value.Where(char.IsLetterOrDigit).ToArray()).ToUpperInvariant();
}
