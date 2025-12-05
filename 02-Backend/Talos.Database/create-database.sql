CREATE DATABASE Talos;
USE Talos;

CREATE TABLE PackageManager(
Id int primary key auto_increment,
Name varchar(150),
unique key (Name)
);

CREATE TABLE User(
Id int primary key auto_increment,
UserName varchar(100) not null,
Email varchar(150) not null,
PasswordHash varchar(255) not null,
AvatarUrl text null,
SignalrConnectionId varchar(255) null,
LastConnectionId varchar(255) null,
IsOnline boolean default false,
LastSeenAt timestamp null,
LastIpAddress varchar(45) null,
Tier enum('free', 'business', 'personal') default 'free',
PrivateTemplateLimit int default 2,
CurrentPrivateTemplates int default 0,
EmailVerified boolean default false,
CreatedAt timestamp default current_timestamp,
UpdatedAt timestamp default current_timestamp on update current_timestamp,
DeletedAt timestamp null,
Role varchar(100),
unique key UkUserEmail (Email),
unique key UkUserName (UserName),
index IdxUserTier (Tier),
index IdxUserOnline (IsOnline)
);

CREATE TABLE Package(
Id int primary key auto_increment,
Name varchar(255) not null,
ShortName varchar(100),
PackageManagerId int not null,
foreign key (PackageManagerId) references PackageManager (Id),
RepositoryUrl text,
OfficialDocumentationUrl text,
LastScrapedAt timestamp null,
IsActive boolean default true,
CreatedAt timestamp default current_timestamp,
UpdatedAt timestamp default current_timestamp on update current_timestamp,
LatestVersion varchar(50) null,
unique key UkPackageName (Name)
);

CREATE TABLE PackageVersion(
Id int primary key auto_increment,
PackageId int not null,
foreign key (PackageId) references Package (Id) on delete cascade,
Version varchar(50) not null,
ReleaseDate timestamp null,
IsDeprecated boolean default false,
DeprecationMessage text,
DownloadUrl text,
ReleaseNotesUrl text,
CreatedAt timestamp default current_timestamp,
unique key UkPackageVersion (PackageId, Version)
);

CREATE TABLE Compatibility(
Id int primary key auto_increment,
SourcePackageVersionId int not null,
foreign key (SourcePackageVersionId) references PackageVersion(Id) on delete cascade,
TargetPackageId int not null,
foreign key (TargetPackageId) references Package(Id) on delete cascade,
TargetVersionConstraint varchar(100) not null,
CompatibilityType enum('required', 'recommended', 'optional', 'conflict') default 'required',
CompatibilityScore int default 100,
ConfidenceLevel enum('high', 'medium', 'low') default 'medium',
DetectedBy enum('n8n_scraper', 'manual', 'user_report', 'ai_analysis') default 'n8n_scraper',
DetectionDate timestamp default current_timestamp,
Notes text,
IsActive boolean default true,
unique key UkCompatibilityUnique (
SourcePackageVersionId,
TargetPackageId,
TargetVersionConstraint
)
);

CREATE TABLE Template(
Id int primary key auto_increment,
UserId int not null,
foreign key (UserId) references User (Id) on delete cascade,
TemplateName varchar(150),
Slug varchar(255) unique,
IsPublic boolean default false,
LicenseType varchar(50),
CreatedAt timestamp default current_timestamp
);

CREATE TABLE TemplateDependencies(
Id int primary key auto_increment,
TemplateId int not null,
foreign key (TemplateId) references Template (Id) on delete cascade,
PackageId int not null,
foreign key (PackageId) references Package (Id),
VersionConstraint varchar(100),
CreatedAt timestamp default current_timestamp
);

CREATE TABLE Tag(
Id int primary key auto_increment,
Name varchar (100) not null,
Description text,
Color varchar(7) default '#3B82F6',
IsSystemTag boolean default false,
CreatedAt timestamp default current_timestamp,
UpdatedKey timestamp default current_timestamp on update current_timestamp,
unique key UkTagName (Name)
);

CREATE TABLE Notification(
Id int primary key auto_increment,
UserId int not null,
foreign key (UserId) references User (Id) on delete cascade,
TagId int,
foreign key (TagId) references Tag (Id) on delete set null,
Title varchar(255) not null,
Message text not null,
Payload json,
IsRead boolean default false,
IsArchived boolean default false,
Priority enum('low', 'medium', 'high', 'critical') default 'medium',
ExpiresAt timestamp null,
CreatedAt timestamp default current_timestamp,
ReadAt timestamp null,
ArchivedAt timestamp null,
index IdxNotificationUser (UserId),
index IdxNotificationRead (IsRead),
index IdxNotificationArchived (IsArchived),
index IdxNotificationCreated (CreatedAt desc)
);

CREATE TABLE UserNotificationPreference(
Id int primary key auto_increment,
UserId int not null,
foreign key (UserId) references User (Id) on delete cascade,
TagId int not null,
foreign key (TagId) references Tag (Id) on delete cascade,
ViaEmail boolean default true,
ViaPush boolean default true,
ViaWeb boolean default true,
IsMuted boolean default false,
CreatedAt timestamp default current_timestamp,
UpdateAt timestamp default current_timestamp on update current_timestamp,
unique key UkUserTagPreference (UserId, TagId)
);
