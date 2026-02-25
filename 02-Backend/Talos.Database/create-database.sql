CREATE DATABASE Talos;
USE Talos;

CREATE TABLE package_manager(
id int primary key auto_increment,
name varchar(150),
unique key (name)
);

CREATE TABLE package(
id int primary key auto_increment,
name varchar(255) not null,
short_name varchar(100),
package_manager_id int not null,
foreign key (package_manager_id) references package_manager (id),
repository_url text,
official_documentation_url text,
last_scraped_at timestamp null,
is_active boolean default true,
create_at timestamp default current_timestamp,
update_at timestamp default current_timestamp on update current_timestamp,
unique key uk_package_name (name)
);

CREATE TABLE user(
id int primary key auto_increment,
user_name varchar(100),
email varchar(150),
tier enum('free', 'bussines', 'personal'),
private_template_limit int default 0,
create_at timestamp default current_timestamp
);

CREATE TABLE package_version(
id int primary key auto_increment,
package_id int not null,
foreign key (package_id) references package (id) on delete cascade,
version varchar(50) not null,
release_date timestamp null,
is_deprecated boolean default false,
deprecation_message text,
download_url text,
release_notes_url text,
create_at timestamp default current_timestamp,
unique key uk_package_version (package_id, version)
);

CREATE TABLE compatibility(
id int primary key auto_increment,
source_package_version_id int not null,
foreign key (source_package_version_id) references package_version(id) on delete cascade,
target_package_id int not null,
foreign key (target_package_id) references package(id) on delete cascade,
target_version_constraint varchar(100) not null,
compatibility_type enum('required', 'recommended', 'optional', 'conflict') default 'required',
compatibility_score int default 100,
confidence_level enum('high', 'medium', 'low') default 'medium',
detected_by enum('n8n_scraper', 'manual', 'user_report', 'ai_analysis') default 'n8n_scraper',
detection_date timestamp default current_timestamp,
notes text,
is_active boolean default true,
unique key uk_compatibility_unique (
source_package_version_id,
target_package_id,
target_version_constraint
)
);

CREATE TABLE template(
id int primary key auto_increment,
user_id int not null,
foreign key (user_id) references user (id) on delete cascade,
template_name varchar(150),
slug varchar(255) unique,
is_public boolean default false,
license_type varchar(50),
create_at timestamp default current_timestamp
);

CREATE TABLE template_dependencies(
id int primary key auto_increment,
template_id int not null,
foreign key (template_id) references template (id) on delete cascade,
package_id int not null,
foreign key (package_id) references package (id),
version_constraint varchar(100),
create_at timestamp default current_timestamp
);
