using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermissionLevel = table.Column<int>(type: "int", nullable: false),
                    CanApproveObservations = table.Column<bool>(type: "bit", nullable: false),
                    CanManageEquipment = table.Column<bool>(type: "bit", nullable: false),
                    CanManageMembers = table.Column<bool>(type: "bit", nullable: false),
                    CanManageProjects = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MemberCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JoinDate = table.Column<DateOnly>(type: "date", nullable: false),
                    BirthYear = table.Column<short>(type: "smallint", nullable: true),
                    MemberStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AavsoObserverCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfileImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastLoginAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastLoginIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DATAPRODUCT_TYPES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DPT", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EQUIPMENT_CATEGORY",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EQUIPMENT_CATEGORY", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EVENT_TYPES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EVENT_TYPES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FORECAST_CATEGORIES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FORECAST_CATEGORIES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MEMBER_CONTACT_PREF",
                columns: table => new
                {
                    PrefId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Channel = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    ChannelAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    EventTypes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    QuietHoursStart = table.Column<byte>(type: "tinyint", nullable: true),
                    QuietHoursEnd = table.Column<byte>(type: "tinyint", nullable: true),
                    Timezone = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MEMBER_CONTACT_PREF", x => x.PrefId);
                });

            migrationBuilder.CreateTable(
                name: "MEMBER_ROLE_AUDIT",
                columns: table => new
                {
                    AuditId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Action = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    ActionDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "(sysdatetimeoffset())"),
                    ActionBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ExpiryDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MEMBER_ROLE_AUDIT", x => x.AuditId);
                });

            migrationBuilder.CreateTable(
                name: "NOTIFICATION_LOG",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipientId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Channel = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    ChannelAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EventType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    EntityType = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    EntityCode = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Body = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: "pending"),
                    ScheduledFor = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    SentAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    RetryCount = table.Column<byte>(type: "tinyint", nullable: false),
                    LastError = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "(sysdatetimeoffset())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NOTIFICATION_LOG", x => x.NotificationId);
                });

            migrationBuilder.CreateTable(
                name: "OBSERVATION_SESSION_TYPES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OST", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OBSERVATION_SITES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SiteType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LatitudeDeg = table.Column<decimal>(type: "decimal(9,6)", nullable: false),
                    LongitudeDeg = table.Column<decimal>(type: "decimal(9,6)", nullable: false),
                    AltitudeInMeters = table.Column<decimal>(type: "decimal(6,1)", nullable: false),
                    TimeZone = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    BortleClass = table.Column<decimal>(type: "decimal(2,1)", nullable: true),
                    SqmAvg = table.Column<decimal>(type: "decimal(6,3)", nullable: true),
                    LimitingMagAvg = table.Column<decimal>(type: "decimal(4,1)", nullable: true),
                    HorizonProfileUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LightPollutionNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AccessNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "(sysdatetimeoffset())"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "(sysdatetimeoffset())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OBSERVATION_SITES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OBSERVATION_TYPES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OT", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PROJECT_TYPE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROJECT_TYPE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TARGETS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    SimbadId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CommonName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MessierId = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    NgcId = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    IcId = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    MpcDesignation = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    VsxId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    HipId = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    GaiaDr3Id = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    ObjectTypeCode = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    ObjectTypeLabel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsSolarSystem = table.Column<bool>(type: "bit", nullable: false),
                    RaDeg = table.Column<decimal>(type: "decimal(13,9)", nullable: true),
                    DecDeg = table.Column<decimal>(type: "decimal(12,9)", nullable: true),
                    RaHms = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    DecDms = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    PmRaMasYr = table.Column<decimal>(type: "decimal(10,4)", nullable: true),
                    PmDecMasYr = table.Column<decimal>(type: "decimal(10,4)", nullable: true),
                    PmEpoch = table.Column<decimal>(type: "decimal(6,1)", nullable: true),
                    MagnitudeV = table.Column<decimal>(type: "decimal(6,3)", nullable: true),
                    MagnitudeB = table.Column<decimal>(type: "decimal(6,3)", nullable: true),
                    MagnitudeFilter = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    MagnitudeSystem = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    Constellation = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: true),
                    DistanceLy = table.Column<decimal>(type: "decimal(15,3)", nullable: true),
                    DistancePc = table.Column<decimal>(type: "decimal(15,6)", nullable: true),
                    AngularSizeArcmin = table.Column<decimal>(type: "decimal(8,3)", nullable: true),
                    PositionAngleDeg = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    RedshiftZ = table.Column<decimal>(type: "decimal(10,7)", nullable: true),
                    SpectralType = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    VariabilityType = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    VariabilityPeriodDays = table.Column<decimal>(type: "decimal(15,8)", nullable: true),
                    EpochMaxJd = table.Column<decimal>(type: "decimal(15,6)", nullable: true),
                    CatalogSource = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    CatalogUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "(sysdatetimeoffset())"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "(sysdatetimeoffset())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TARGETS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TASK_TYPE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TASK_TYPE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EQUIPMENTS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    OpticalDesign = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNumber = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    PurchaseDate = table.Column<DateOnly>(type: "date", nullable: true),
                    PurchasePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "operational"),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LoanedTo = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    LoanDueDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FitsTelescop = table.Column<string>(type: "varchar(68)", unicode: false, maxLength: 68, nullable: true),
                    FitsInstrume = table.Column<string>(type: "varchar(68)", unicode: false, maxLength: 68, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "(sysdatetimeoffset())"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "(sysdatetimeoffset())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EQUIPMENTS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EQ_Category",
                        column: x => x.CategoryId,
                        principalTable: "EQUIPMENT_CATEGORY",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FORECASTS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "proposed"),
                    PeriodYear = table.Column<short>(type: "smallint", nullable: false),
                    TargetDate = table.Column<DateOnly>(type: "date", nullable: true),
                    AchievedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    SuccessMetric = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "(sysdatetimeoffset())"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "(sysdatetimeoffset())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FORECASTS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FCST_Category",
                        column: x => x.CategoryId,
                        principalTable: "FORECAST_CATEGORIES",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FORECASTS_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OBSERVATION_SESSIONS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    StartTimeUTC = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EndTimeUTC = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    JulianDateStart = table.Column<decimal>(type: "decimal(15,6)", nullable: false),
                    JulianDateEnd = table.Column<decimal>(type: "decimal(15,6)", nullable: true),
                    LeadUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "planned"),
                    SessionTypeId = table.Column<int>(type: "int", nullable: false),
                    SeeingAntoniadi = table.Column<byte>(type: "tinyint", nullable: true),
                    SeeingFwhmArcsec = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Transparency = table.Column<byte>(type: "tinyint", nullable: true),
                    LimitingMagVis = table.Column<decimal>(type: "decimal(4,1)", nullable: true),
                    SqmReading = table.Column<decimal>(type: "decimal(6,3)", nullable: true),
                    BortleMeasured = table.Column<decimal>(type: "decimal(2,1)", nullable: true),
                    MoonPhasePct = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    MoonAltDeg = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    MoonsetUTC = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    TemperatureC = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    HumidityPct = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    WindSpeedKmh = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    WindDirectionDeg = table.Column<decimal>(type: "decimal(5,1)", nullable: true),
                    PressureHpa = table.Column<decimal>(type: "decimal(7,2)", nullable: true),
                    DewPointC = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    WeatherSummary = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "(sysdatetimeoffset())"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "(sysdatetimeoffset())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OBSERVATION_SESSIONS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SES_SessionType",
                        column: x => x.SessionTypeId,
                        principalTable: "OBSERVATION_SESSION_TYPES",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SES_Site",
                        column: x => x.SiteId,
                        principalTable: "OBSERVATION_SITES",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EVENTS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    EventTypeId = table.Column<int>(type: "int", nullable: false),
                    TargetId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    PeakDateUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    JdPeak = table.Column<decimal>(type: "decimal(15,6)", nullable: false),
                    EventStartUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    EventEndUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    VisibilityGlobal = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    RaPeakDeg = table.Column<decimal>(type: "decimal(13,9)", nullable: true),
                    DecPeakDeg = table.Column<decimal>(type: "decimal(12,9)", nullable: true),
                    MagnitudeAtPeak = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    DurationMinutes = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    Constellation = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: true),
                    Source = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SourceUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MpcDesignation = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    IsVisibleFromSite = table.Column<bool>(type: "bit", nullable: true),
                    AlertSent = table.Column<bool>(type: "bit", nullable: false),
                    AlertDaysBefore = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)7),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "(sysdatetimeoffset())"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "(sysdatetimeoffset())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EVENTS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EV_EventType",
                        column: x => x.EventTypeId,
                        principalTable: "EVENT_TYPES",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EV_Target",
                        column: x => x.TargetId,
                        principalTable: "TARGETS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PROJECTS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectTypeId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "draft"),
                    Priority = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "medium"),
                    Visibility = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false, defaultValue: "members_only"),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    TargetEndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ActualEndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ProjectLeadId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    TargetId = table.Column<int>(type: "int", nullable: true),
                    TotalIntegrationGoalH = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    TotalIntegrationAchievedH = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    RepositoryUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "(sysdatetimeoffset())"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "(sysdatetimeoffset())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROJECTS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PRJ_Target",
                        column: x => x.TargetId,
                        principalTable: "TARGETS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PRJ_Type",
                        column: x => x.ProjectTypeId,
                        principalTable: "PROJECT_TYPE",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PROJECTS_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EQUIPMENT_MAINTENANCE",
                columns: table => new
                {
                    MaintenanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipmentId = table.Column<int>(type: "int", nullable: false),
                    MaintenanceDate = table.Column<DateOnly>(type: "date", nullable: false),
                    MaintenanceType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PerformedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Result = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NextDueDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    AttachmentsUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "(sysdatetimeoffset())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EQUIPMENT_MAINTENANCE", x => x.MaintenanceId);
                    table.ForeignKey(
                        name: "FK_EM_Equipment",
                        column: x => x.EquipmentId,
                        principalTable: "EQUIPMENTS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OBSERVATIONS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObsId = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false),
                    SessionId = table.Column<int>(type: "int", nullable: false),
                    TargetId = table.Column<int>(type: "int", nullable: false),
                    TelescopeId = table.Column<int>(type: "int", nullable: false),
                    CameraId = table.Column<int>(type: "int", nullable: true),
                    FilterId = table.Column<int>(type: "int", nullable: true),
                    MountId = table.Column<int>(type: "int", nullable: true),
                    GuiderId = table.Column<int>(type: "int", nullable: true),
                    ObserverId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartTimeUTC = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EndTimeUTC = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    JdMid = table.Column<decimal>(type: "decimal(15,6)", nullable: false),
                    BjdTdb = table.Column<decimal>(type: "decimal(15,7)", nullable: true),
                    Timesys = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: "UTC"),
                    ExposureTimeS = table.Column<decimal>(type: "decimal(10,3)", nullable: true),
                    ObservationTypeId = table.Column<int>(type: "int", nullable: false),
                    DataproductTypeId = table.Column<int>(type: "int", nullable: false),
                    CalibLevel = table.Column<byte>(type: "tinyint", nullable: false),
                    ObsCollection = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    SRa = table.Column<decimal>(type: "decimal(13,9)", nullable: false),
                    SDec = table.Column<decimal>(type: "decimal(12,9)", nullable: false),
                    SFov = table.Column<decimal>(type: "decimal(8,5)", nullable: true),
                    AltDeg = table.Column<decimal>(type: "decimal(8,4)", nullable: true),
                    AzDeg = table.Column<decimal>(type: "decimal(8,4)", nullable: true),
                    Airmass = table.Column<decimal>(type: "decimal(6,4)", nullable: true),
                    FilterCode = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    WavelengthMinM = table.Column<decimal>(type: "decimal(15,12)", nullable: true),
                    WavelengthMaxM = table.Column<decimal>(type: "decimal(15,12)", nullable: true),
                    MagnitudeMeasured = table.Column<decimal>(type: "decimal(6,3)", nullable: true),
                    MagnitudeError = table.Column<decimal>(type: "decimal(5,3)", nullable: true),
                    MagnitudeFilter = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    MagnitudeSystem = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    FainterThan = table.Column<bool>(type: "bit", nullable: false),
                    ComparisonStarId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CheckStarId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    AavsoChartId = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    GuidingRmsArcsec = table.Column<decimal>(type: "decimal(6,3)", nullable: true),
                    PlateSolved = table.Column<bool>(type: "bit", nullable: false),
                    QualityRating = table.Column<byte>(type: "tinyint", nullable: true),
                    ResultSummary = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    PublishedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    PublishedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    ObsPublisherDid = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AccessUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FacilityName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    InstrumentName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "(sysdatetimeoffset())"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "(sysdatetimeoffset())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OBSERVATIONS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OBS_Camera",
                        column: x => x.CameraId,
                        principalTable: "EQUIPMENTS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OBS_DPType",
                        column: x => x.DataproductTypeId,
                        principalTable: "DATAPRODUCT_TYPES",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OBS_Filter",
                        column: x => x.FilterId,
                        principalTable: "EQUIPMENTS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OBS_Guider",
                        column: x => x.GuiderId,
                        principalTable: "EQUIPMENTS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OBS_Mount",
                        column: x => x.MountId,
                        principalTable: "EQUIPMENTS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OBS_ObsType",
                        column: x => x.ObservationTypeId,
                        principalTable: "OBSERVATION_TYPES",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OBS_Session",
                        column: x => x.SessionId,
                        principalTable: "OBSERVATION_SESSIONS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OBS_Target",
                        column: x => x.TargetId,
                        principalTable: "TARGETS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OBS_Telescope",
                        column: x => x.TelescopeId,
                        principalTable: "EQUIPMENTS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SESSION_MEMBERS",
                columns: table => new
                {
                    SessionId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SessionRole = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ArrivalTimeUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DepartureTimeUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SESSION_MEMBERS", x => new { x.SessionId, x.UserId });
                    table.ForeignKey(
                        name: "FK_SM_Session",
                        column: x => x.SessionId,
                        principalTable: "OBSERVATION_SESSIONS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EVENT_VISIBILITY",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    MinAltitudeDeg = table.Column<decimal>(type: "decimal(7,3)", nullable: true),
                    MaxAltitudeDeg = table.Column<decimal>(type: "decimal(7,3)", nullable: true),
                    BestViewingDirection = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    DurationMinutes = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    RiseTimeUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    SetTimeUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    BestViewingUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    AzimuthAtPeakDeg = table.Column<decimal>(type: "decimal(7,3)", nullable: true),
                    ComputedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "(sysdatetimeoffset())"),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EVENT_VISIBILITY", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EV2_Event",
                        column: x => x.EventId,
                        principalTable: "EVENTS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EV2_Site",
                        column: x => x.SiteId,
                        principalTable: "OBSERVATION_SITES",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FORECAST_PROJECT",
                columns: table => new
                {
                    ForecastId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    ContributionNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FORECAST_PROJECT", x => new { x.ForecastId, x.ProjectId });
                    table.ForeignKey(
                        name: "FK_FP_Forecast",
                        column: x => x.ForecastId,
                        principalTable: "FORECASTS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FP_Project",
                        column: x => x.ProjectId,
                        principalTable: "PROJECTS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MILESTONES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DueDate = table.Column<DateOnly>(type: "date", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CompletionDate = table.Column<DateOnly>(type: "date", nullable: true),
                    SortOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MILESTONES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MS_Project",
                        column: x => x.ProjectId,
                        principalTable: "PROJECTS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PROJECT_MEMBERS",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    JoinDate = table.Column<DateOnly>(type: "date", nullable: false),
                    LeftDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROJECT_MEMBERS", x => new { x.ProjectId, x.UserId });
                    table.ForeignKey(
                        name: "FK_PM_Project",
                        column: x => x.ProjectId,
                        principalTable: "PROJECTS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EVENT_OBSERVATION",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "int", nullable: false),
                    ObservationId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "(sysdatetimeoffset())"),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EVENT_OBSERVATION", x => new { x.EventId, x.ObservationId });
                    table.ForeignKey(
                        name: "FK_EO_Event",
                        column: x => x.EventId,
                        principalTable: "EVENTS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EO_Observation",
                        column: x => x.ObservationId,
                        principalTable: "OBSERVATIONS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EVENT_OBSERVATION_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IMAGE_RECORDS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    ObservationId = table.Column<int>(type: "int", nullable: false),
                    TargetId = table.Column<int>(type: "int", nullable: false),
                    CapturedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ProcessedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    ImageType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CaptureDateUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CalibLevel = table.Column<byte>(type: "tinyint", nullable: false),
                    FilterCode = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    TotalIntegrationS = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    FrameCount = table.Column<int>(type: "int", nullable: true),
                    PublicationStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "raw"),
                    IsShowcase = table.Column<bool>(type: "bit", nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PreviewUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FitsUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "(sysdatetimeoffset())"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "(sysdatetimeoffset())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IMAGE_RECORDS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IR_Observation",
                        column: x => x.ObservationId,
                        principalTable: "OBSERVATIONS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_IR_Target",
                        column: x => x.TargetId,
                        principalTable: "TARGETS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TASKS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskCode = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    MilestoneId = table.Column<int>(type: "int", nullable: true),
                    ParentTaskId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskTypeId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "backlog"),
                    Priority = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "medium"),
                    DueDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EstimatedHours = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    ActualHours = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "(sysdatetimeoffset())"),
                    CompletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "(sysdatetimeoffset())"),
                    SessionId = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TASKS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TASKS_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TSK_Milestone",
                        column: x => x.MilestoneId,
                        principalTable: "MILESTONES",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TSK_Parent",
                        column: x => x.ParentTaskId,
                        principalTable: "TASKS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TSK_Project",
                        column: x => x.ProjectId,
                        principalTable: "PROJECTS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TSK_Session",
                        column: x => x.SessionId,
                        principalTable: "OBSERVATION_SESSIONS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TSK_Type",
                        column: x => x.TaskTypeId,
                        principalTable: "TASK_TYPE",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TASK_ASSIGNMENT",
                columns: table => new
                {
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AssignedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    AssignedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "(sysdatetimeoffset())"),
                    IsLead = table.Column<bool>(type: "bit", nullable: false),
                    ConfirmedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TASK_ASSIGNMENT", x => new { x.TaskId, x.UserId });
                    table.ForeignKey(
                        name: "FK_TA_Task",
                        column: x => x.TaskId,
                        principalTable: "TASKS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CreatedByUserId",
                table: "AspNetUsers",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UK_DPT_Name",
                table: "DATAPRODUCT_TYPES",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_EQUIPMENT_CATEGORY_Name",
                table: "EQUIPMENT_CATEGORY",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EM_Date",
                table: "EQUIPMENT_MAINTENANCE",
                column: "MaintenanceDate",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_EM_EquipmentId",
                table: "EQUIPMENT_MAINTENANCE",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EQ_CategoryId",
                table: "EQUIPMENTS",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_EQ_Status",
                table: "EQUIPMENTS",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "UK_EQUIPMENTS_Code",
                table: "EQUIPMENTS",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EVENT_OBSERVATION_CreatedBy",
                table: "EVENT_OBSERVATION",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EVENT_OBSERVATION_ObservationId",
                table: "EVENT_OBSERVATION",
                column: "ObservationId");

            migrationBuilder.CreateIndex(
                name: "UK_ET_Name",
                table: "EVENT_TYPES",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EVENT_VISIBILITY_SiteId",
                table: "EVENT_VISIBILITY",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "UK_EV_EventSite",
                table: "EVENT_VISIBILITY",
                columns: new[] { "EventId", "SiteId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EV_PeakDate",
                table: "EVENTS",
                column: "PeakDateUtc");

            migrationBuilder.CreateIndex(
                name: "IX_EVENTS_EventTypeId",
                table: "EVENTS",
                column: "EventTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EVENTS_TargetId",
                table: "EVENTS",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "UK_EV_Code",
                table: "EVENTS",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_FC_Name",
                table: "FORECAST_CATEGORIES",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FORECAST_PROJECT_ProjectId",
                table: "FORECAST_PROJECT",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_FCST_PeriodYear",
                table: "FORECASTS",
                column: "PeriodYear");

            migrationBuilder.CreateIndex(
                name: "IX_FCST_Status",
                table: "FORECASTS",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_FORECASTS_CategoryId",
                table: "FORECASTS",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FORECASTS_CreatedBy",
                table: "FORECASTS",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "UK_FORECASTS_Code",
                table: "FORECASTS",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IR_ObservationId",
                table: "IMAGE_RECORDS",
                column: "ObservationId");

            migrationBuilder.CreateIndex(
                name: "IX_IR_PubStatus",
                table: "IMAGE_RECORDS",
                column: "PublicationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_IR_TargetId",
                table: "IMAGE_RECORDS",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "UK_IR_Code",
                table: "IMAGE_RECORDS",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_MCP_UserChannel",
                table: "MEMBER_CONTACT_PREF",
                columns: new[] { "UserId", "Channel" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MRA_Date",
                table: "MEMBER_ROLE_AUDIT",
                column: "ActionDate",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_MRA_RoleId",
                table: "MEMBER_ROLE_AUDIT",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_MRA_UserId",
                table: "MEMBER_ROLE_AUDIT",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MS_ProjectId",
                table: "MILESTONES",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "UK_OST_Name",
                table: "OBSERVATION_SESSION_TYPES",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OBSERVATION_SESSIONS_SessionTypeId",
                table: "OBSERVATION_SESSIONS",
                column: "SessionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SES_SiteId",
                table: "OBSERVATION_SESSIONS",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_SES_StartTimeUTC",
                table: "OBSERVATION_SESSIONS",
                column: "StartTimeUTC",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_SES_Status",
                table: "OBSERVATION_SESSIONS",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "UK_SES_Code",
                table: "OBSERVATION_SESSIONS",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_OS_Code",
                table: "OBSERVATION_SITES",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_OT_Name",
                table: "OBSERVATION_TYPES",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OBS_JdMid",
                table: "OBSERVATIONS",
                column: "JdMid");

            migrationBuilder.CreateIndex(
                name: "IX_OBS_ObserverId",
                table: "OBSERVATIONS",
                column: "ObserverId");

            migrationBuilder.CreateIndex(
                name: "IX_OBS_RaDec",
                table: "OBSERVATIONS",
                columns: new[] { "SRa", "SDec" });

            migrationBuilder.CreateIndex(
                name: "IX_OBS_SessionId",
                table: "OBSERVATIONS",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_OBS_TargetId",
                table: "OBSERVATIONS",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_OBSERVATIONS_CameraId",
                table: "OBSERVATIONS",
                column: "CameraId");

            migrationBuilder.CreateIndex(
                name: "IX_OBSERVATIONS_DataproductTypeId",
                table: "OBSERVATIONS",
                column: "DataproductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OBSERVATIONS_FilterId",
                table: "OBSERVATIONS",
                column: "FilterId");

            migrationBuilder.CreateIndex(
                name: "IX_OBSERVATIONS_GuiderId",
                table: "OBSERVATIONS",
                column: "GuiderId");

            migrationBuilder.CreateIndex(
                name: "IX_OBSERVATIONS_MountId",
                table: "OBSERVATIONS",
                column: "MountId");

            migrationBuilder.CreateIndex(
                name: "IX_OBSERVATIONS_ObservationTypeId",
                table: "OBSERVATIONS",
                column: "ObservationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OBSERVATIONS_TelescopeId",
                table: "OBSERVATIONS",
                column: "TelescopeId");

            migrationBuilder.CreateIndex(
                name: "UK_OBS_ObsId",
                table: "OBSERVATIONS",
                column: "ObsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PM_UserId",
                table: "PROJECT_MEMBERS",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UK_PT_Name",
                table: "PROJECT_TYPE",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PRJ_Status",
                table: "PROJECTS",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PRJ_TargetId",
                table: "PROJECTS",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_PROJECTS_CreatedBy",
                table: "PROJECTS",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PROJECTS_ProjectTypeId",
                table: "PROJECTS",
                column: "ProjectTypeId");

            migrationBuilder.CreateIndex(
                name: "UK_PRJ_Code",
                table: "PROJECTS",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SM_UserId",
                table: "SESSION_MEMBERS",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TGT_IsSolarSystem",
                table: "TARGETS",
                column: "IsSolarSystem");

            migrationBuilder.CreateIndex(
                name: "IX_TGT_ObjectTypeCode",
                table: "TARGETS",
                column: "ObjectTypeCode");

            migrationBuilder.CreateIndex(
                name: "IX_TGT_RaDec",
                table: "TARGETS",
                columns: new[] { "RaDeg", "DecDeg" });

            migrationBuilder.CreateIndex(
                name: "UK_TARGETS_Code",
                table: "TARGETS",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_TARGETS_SimbadId",
                table: "TARGETS",
                column: "SimbadId",
                unique: true,
                filter: "[SimbadId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TA_UserId",
                table: "TASK_ASSIGNMENT",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UK_TT_Name",
                table: "TASK_TYPE",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TASKS_CreatedBy",
                table: "TASKS",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TASKS_MilestoneId",
                table: "TASKS",
                column: "MilestoneId");

            migrationBuilder.CreateIndex(
                name: "IX_TASKS_ParentTaskId",
                table: "TASKS",
                column: "ParentTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TASKS_SessionId",
                table: "TASKS",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_TASKS_TaskTypeId",
                table: "TASKS",
                column: "TaskTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TSK_DueDate",
                table: "TASKS",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_TSK_ProjectId",
                table: "TASKS",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TSK_Status",
                table: "TASKS",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "UK_TSK_Code",
                table: "TASKS",
                column: "TaskCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "EQUIPMENT_MAINTENANCE");

            migrationBuilder.DropTable(
                name: "EVENT_OBSERVATION");

            migrationBuilder.DropTable(
                name: "EVENT_VISIBILITY");

            migrationBuilder.DropTable(
                name: "FORECAST_PROJECT");

            migrationBuilder.DropTable(
                name: "IMAGE_RECORDS");

            migrationBuilder.DropTable(
                name: "MEMBER_CONTACT_PREF");

            migrationBuilder.DropTable(
                name: "MEMBER_ROLE_AUDIT");

            migrationBuilder.DropTable(
                name: "NOTIFICATION_LOG");

            migrationBuilder.DropTable(
                name: "PROJECT_MEMBERS");

            migrationBuilder.DropTable(
                name: "SESSION_MEMBERS");

            migrationBuilder.DropTable(
                name: "TASK_ASSIGNMENT");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "EVENTS");

            migrationBuilder.DropTable(
                name: "FORECASTS");

            migrationBuilder.DropTable(
                name: "OBSERVATIONS");

            migrationBuilder.DropTable(
                name: "TASKS");

            migrationBuilder.DropTable(
                name: "EVENT_TYPES");

            migrationBuilder.DropTable(
                name: "FORECAST_CATEGORIES");

            migrationBuilder.DropTable(
                name: "EQUIPMENTS");

            migrationBuilder.DropTable(
                name: "DATAPRODUCT_TYPES");

            migrationBuilder.DropTable(
                name: "OBSERVATION_TYPES");

            migrationBuilder.DropTable(
                name: "MILESTONES");

            migrationBuilder.DropTable(
                name: "OBSERVATION_SESSIONS");

            migrationBuilder.DropTable(
                name: "TASK_TYPE");

            migrationBuilder.DropTable(
                name: "EQUIPMENT_CATEGORY");

            migrationBuilder.DropTable(
                name: "PROJECTS");

            migrationBuilder.DropTable(
                name: "OBSERVATION_SESSION_TYPES");

            migrationBuilder.DropTable(
                name: "OBSERVATION_SITES");

            migrationBuilder.DropTable(
                name: "TARGETS");

            migrationBuilder.DropTable(
                name: "PROJECT_TYPE");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
