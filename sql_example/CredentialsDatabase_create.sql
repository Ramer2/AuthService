-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2025-09-29 21:24:45.333

-- tables
-- Table: Permission
CREATE TABLE Permission (
    PermissionId uniqueidentifier  NOT NULL DEFAULT newid(),
    PermissionName varchar(50)  NOT NULL,
    CONSTRAINT AK_1 UNIQUE (PermissionName),
    CONSTRAINT Permission_pk PRIMARY KEY  (PermissionId)
);

-- Table: Role
CREATE TABLE Role (
    RoleId uniqueidentifier  NOT NULL DEFAULT newid(),
    RoleName varchar(50)  NOT NULL,
    CONSTRAINT AK_0 UNIQUE (RoleName),
    CONSTRAINT Role_pk PRIMARY KEY  (RoleId)
);

-- Table: User
CREATE TABLE "User" (
    UserId uniqueidentifier  NOT NULL DEFAULT newid(),
    Email varchar(50)  NOT NULL,
    Username varchar(50)  NOT NULL,
    HashedPassword varbinary(512)  NOT NULL,
    CreatedAt datetime2  NULL DEFAULT sysutcdatetime(),
    IsActive bit  NULL DEFAULT 1,
    CONSTRAINT AK_2 UNIQUE (Email),
    CONSTRAINT AK_3 UNIQUE (Username),
    CONSTRAINT User_pk PRIMARY KEY  (UserId)
);

-- Table: UserPermission
CREATE TABLE UserPermission (
    UserId uniqueidentifier  NOT NULL,
    PermissionId uniqueidentifier  NOT NULL,
    CONSTRAINT UserPermission_pk PRIMARY KEY  (UserId,PermissionId)
);

-- Table: UserRole
CREATE TABLE UserRole (
    UserId uniqueidentifier  NOT NULL,
    RoleId uniqueidentifier  NOT NULL,
    CONSTRAINT UserRole_pk PRIMARY KEY  (UserId,RoleId)
);

-- foreign keys
-- Reference: FK_0 (table: UserRole)
ALTER TABLE UserRole ADD CONSTRAINT FK_0
    FOREIGN KEY (UserId)
    REFERENCES "User" (UserId);

-- Reference: FK_1 (table: UserRole)
ALTER TABLE UserRole ADD CONSTRAINT FK_1
    FOREIGN KEY (RoleId)
    REFERENCES Role (RoleId);

-- Reference: FK_2 (table: UserPermission)
ALTER TABLE UserPermission ADD CONSTRAINT FK_2
    FOREIGN KEY (UserId)
    REFERENCES "User" (UserId);

-- Reference: FK_3 (table: UserPermission)
ALTER TABLE UserPermission ADD CONSTRAINT FK_3
    FOREIGN KEY (PermissionId)
    REFERENCES Permission (PermissionId);

-- End of file.

