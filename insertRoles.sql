SET NOCOUNT ON;

DECLARE @Now DATETIMEOFFSET = SYSUTCDATETIME();

IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE RoleCode = 'SUPER_ADMIN')
BEGIN
    INSERT INTO AspNetRoles
    (
        Id,
        Name,
        NormalizedName,
        ConcurrencyStamp,
        RoleCode,
        Description,
        PermissionLevel,
        CanApproveObservations,
        CanManageEquipment,
        CanManageMembers,
        CanManageProjects,
        IsActive,
        CreatedAt
    )
    VALUES
    (
        NEWID(),
        'SuperAdmin',
        'SUPERADMIN',
        NEWID(),
        'SUPER_ADMIN',
        'System administrator with full access',
        100,
        1,
        1,
        1,
        1,
        1,
        @Now
    );
END

IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE RoleCode = 'ADMIN')
BEGIN
    INSERT INTO AspNetRoles
    (
        Id,
        Name,
        NormalizedName,
        ConcurrencyStamp,
        RoleCode,
        Description,
        PermissionLevel,
        CanApproveObservations,
        CanManageEquipment,
        CanManageMembers,
        CanManageProjects,
        IsActive,
        CreatedAt
    )
    VALUES
    (
        NEWID(),
        'Admin',
        'ADMIN',
        NEWID(),
        'ADMIN',
        'Club administrator',
        90,
        1,
        1,
        1,
        1,
        1,
        @Now
    );
END

IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE RoleCode = 'BOARD_MEMBER')
BEGIN
    INSERT INTO AspNetRoles
    (
        Id,
        Name,
        NormalizedName,
        ConcurrencyStamp,
        RoleCode,
        Description,
        PermissionLevel,
        CanApproveObservations,
        CanManageEquipment,
        CanManageMembers,
        CanManageProjects,
        IsActive,
        CreatedAt
    )
    VALUES
    (
        NEWID(),
        'BoardMember',
        'BOARDMEMBER',
        NEWID(),
        'BOARD_MEMBER',
        'Board member',
        70,
        1,
        0,
        0,
        1,
        1,
        @Now
    );
END

IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE RoleCode = 'EVENT_MANAGER')
BEGIN
    INSERT INTO AspNetRoles
    (
        Id,
        Name,
        NormalizedName,
        ConcurrencyStamp,
        RoleCode,
        Description,
        PermissionLevel,
        CanApproveObservations,
        CanManageEquipment,
        CanManageMembers,
        CanManageProjects,
        IsActive,
        CreatedAt
    )
    VALUES
    (
        NEWID(),
        'EventManager',
        'EVENTMANAGER',
        NEWID(),
        'EVENT_MANAGER',
        'Manages events and activities',
        60,
        0,
        0,
        0,
        1,
        1,
        @Now
    );
END

IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE RoleCode = 'INVENTORY_MANAGER')
BEGIN
    INSERT INTO AspNetRoles
    (
        Id,
        Name,
        NormalizedName,
        ConcurrencyStamp,
        RoleCode,
        Description,
        PermissionLevel,
        CanApproveObservations,
        CanManageEquipment,
        CanManageMembers,
        CanManageProjects,
        IsActive,
        CreatedAt
    )
    VALUES
    (
        NEWID(),
        'InventoryManager',
        'INVENTORYMANAGER',
        NEWID(),
        'INVENTORY_MANAGER',
        'Manages telescopes and equipment',
        50,
        0,
        1,
        0,
        0,
        1,
        @Now
    );
END

IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE RoleCode = 'MEMBER')
BEGIN
    INSERT INTO AspNetRoles
    (
        Id,
        Name,
        NormalizedName,
        ConcurrencyStamp,
        RoleCode,
        Description,
        PermissionLevel,
        CanApproveObservations,
        CanManageEquipment,
        CanManageMembers,
        CanManageProjects,
        IsActive,
        CreatedAt
    )
    VALUES
    (
        NEWID(),
        'Member',
        'MEMBER',
        NEWID(),
        'MEMBER',
        'Regular club member',
        10,
        0,
        0,
        0,
        0,
        1,
        @Now
    );
END

IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE RoleCode = 'GUEST')
BEGIN
    INSERT INTO AspNetRoles
    (
        Id,
        Name,
        NormalizedName,
        ConcurrencyStamp,
        RoleCode,
        Description,
        PermissionLevel,
        CanApproveObservations,
        CanManageEquipment,
        CanManageMembers,
        CanManageProjects,
        IsActive,
        CreatedAt
    )
    VALUES
    (
        NEWID(),
        'Guest',
        'GUEST',
        NEWID(),
        'GUEST',
        'Guest user',
        1,
        0,
        0,
        0,
        0,
        1,
        @Now
    );
END

IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE RoleCode = 'SERVICE_ACCOUNT')
BEGIN
    INSERT INTO AspNetRoles
    (
        Id,
        Name,
        NormalizedName,
        ConcurrencyStamp,
        RoleCode,
        Description,
        PermissionLevel,
        CanApproveObservations,
        CanManageEquipment,
        CanManageMembers,
        CanManageProjects,
        IsActive,
        CreatedAt
    )
    VALUES
    (
        NEWID(),
        'ServiceAccount',
        'SERVICEACCOUNT',
        NEWID(),
        'SERVICE_ACCOUNT',
        'Machine-to-machine service account',
        20,
        0,
        0,
        0,
        0,
        1,
        @Now
    );
END