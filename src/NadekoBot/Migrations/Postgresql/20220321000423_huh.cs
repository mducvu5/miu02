using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NadekoBot.Migrations.Postgresql
{
    public partial class huh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AutoCommands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CommandText = table.Column<string>(type: "text", nullable: true),
                    ChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    ChannelName = table.Column<string>(type: "text", nullable: true),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    GuildName = table.Column<string>(type: "text", nullable: true),
                    VoiceChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    VoiceChannelName = table.Column<string>(type: "text", nullable: true),
                    Interval = table.Column<int>(type: "integer", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoCommands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AutoTranslateChannels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    ChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    AutoDelete = table.Column<bool>(type: "boolean", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoTranslateChannels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BanTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BanTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Blacklist",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blacklist", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Extra = table.Column<string>(type: "text", nullable: false),
                    OtherId = table.Column<decimal>(type: "numeric(20,0)", nullable: true, defaultValueSql: "NULL"),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyTransactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscordPermOverrides",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Perm = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    Command = table.Column<string>(type: "text", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordPermOverrides", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Expressions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    Response = table.Column<string>(type: "text", nullable: true),
                    Trigger = table.Column<string>(type: "text", nullable: true),
                    AutoDeleteTrigger = table.Column<bool>(type: "boolean", nullable: false),
                    DmResponse = table.Column<bool>(type: "boolean", nullable: false),
                    ContainsAnywhere = table.Column<bool>(type: "boolean", nullable: false),
                    AllowTarget = table.Column<bool>(type: "boolean", nullable: false),
                    Reactions = table.Column<string>(type: "text", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expressions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GuildConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Prefix = table.Column<string>(type: "text", nullable: true),
                    DeleteMessageOnCommand = table.Column<bool>(type: "boolean", nullable: false),
                    AutoAssignRoleIds = table.Column<string>(type: "text", nullable: true),
                    AutoDeleteGreetMessagesTimer = table.Column<int>(type: "integer", nullable: false),
                    AutoDeleteByeMessagesTimer = table.Column<int>(type: "integer", nullable: false),
                    GreetMessageChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    ByeMessageChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    SendDmGreetMessage = table.Column<bool>(type: "boolean", nullable: false),
                    DmGreetMessageText = table.Column<string>(type: "text", nullable: true),
                    SendChannelGreetMessage = table.Column<bool>(type: "boolean", nullable: false),
                    ChannelGreetMessageText = table.Column<string>(type: "text", nullable: true),
                    SendChannelByeMessage = table.Column<bool>(type: "boolean", nullable: false),
                    ChannelByeMessageText = table.Column<string>(type: "text", nullable: true),
                    ExclusiveSelfAssignedRoles = table.Column<bool>(type: "boolean", nullable: false),
                    AutoDeleteSelfAssignedRoleMessages = table.Column<bool>(type: "boolean", nullable: false),
                    VerbosePermissions = table.Column<bool>(type: "boolean", nullable: false),
                    PermissionRole = table.Column<string>(type: "text", nullable: true),
                    FilterInvites = table.Column<bool>(type: "boolean", nullable: false),
                    FilterLinks = table.Column<bool>(type: "boolean", nullable: false),
                    FilterWords = table.Column<bool>(type: "boolean", nullable: false),
                    MuteRoleName = table.Column<string>(type: "text", nullable: true),
                    CleverbotEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    Locale = table.Column<string>(type: "text", nullable: true),
                    TimeZoneId = table.Column<string>(type: "text", nullable: true),
                    WarningsInitialized = table.Column<bool>(type: "boolean", nullable: false),
                    GameVoiceChannel = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    VerboseErrors = table.Column<bool>(type: "boolean", nullable: false),
                    NotifyStreamOffline = table.Column<bool>(type: "boolean", nullable: false),
                    WarnExpireHours = table.Column<int>(type: "integer", nullable: false),
                    WarnExpireAction = table.Column<int>(type: "integer", nullable: false),
                    SendBoostMessage = table.Column<bool>(type: "boolean", nullable: false),
                    BoostMessage = table.Column<string>(type: "text", nullable: true),
                    BoostMessageChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    BoostMessageDeleteAfter = table.Column<int>(type: "integer", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImageOnlyChannels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    ChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageOnlyChannels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    LogOtherId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    MessageUpdatedId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    MessageDeletedId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    UserJoinedId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    UserLeftId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    UserBannedId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    UserUnbannedId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    UserUpdatedId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    ChannelCreatedId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    ChannelDestroyedId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    ChannelUpdatedId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    UserMutedId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    LogUserPresenceId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    LogVoicePresenceId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    LogVoicePresenceTTSId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MusicPlayerSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    PlayerRepeat = table.Column<int>(type: "integer", nullable: false),
                    MusicChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    Volume = table.Column<int>(type: "integer", nullable: false, defaultValue: 100),
                    AutoDisconnect = table.Column<bool>(type: "boolean", nullable: false),
                    QualityPreset = table.Column<int>(type: "integer", nullable: false),
                    AutoPlay = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusicPlayerSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MusicPlaylists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Author = table.Column<string>(type: "text", nullable: true),
                    AuthorId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusicPlaylists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NsfwBlacklistedTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Tag = table.Column<string>(type: "text", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NsfwBlacklistedTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlantedCurrency",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: true),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    ChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    MessageId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantedCurrency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Poll",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    ChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Question = table.Column<string>(type: "text", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Poll", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Quotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Keyword = table.Column<string>(type: "text", nullable: false),
                    AuthorName = table.Column<string>(type: "text", nullable: false),
                    AuthorId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reminders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    When = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    ServerId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: true),
                    IsPrivate = table.Column<bool>(type: "boolean", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reminders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Repeaters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    ChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    LastMessageId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    Message = table.Column<string>(type: "text", nullable: true),
                    Interval = table.Column<TimeSpan>(type: "interval", nullable: false),
                    StartTimeOfDay = table.Column<TimeSpan>(type: "interval", nullable: true),
                    NoRedundant = table.Column<bool>(type: "boolean", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repeaters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RewardedUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    PatreonUserId = table.Column<string>(type: "text", nullable: true),
                    AmountRewardedThisMonth = table.Column<int>(type: "integer", nullable: false),
                    LastReward = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RewardedUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RotatingStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Status = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RotatingStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SelfAssignableRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    RoleId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Group = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    LevelRequirement = table.Column<int>(type: "integer", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelfAssignableRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserXpStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Xp = table.Column<int>(type: "integer", nullable: false),
                    AwardedXp = table.Column<int>(type: "integer", nullable: false),
                    NotifyOnLevelUp = table.Column<int>(type: "integer", nullable: false),
                    LastLevelUp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2017, 9, 21, 20, 53, 13, 307, DateTimeKind.Local)),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserXpStats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Warnings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: true),
                    Forgiven = table.Column<bool>(type: "boolean", nullable: false),
                    ForgivenBy = table.Column<string>(type: "text", nullable: true),
                    Moderator = table.Column<string>(type: "text", nullable: true),
                    Weight = table.Column<long>(type: "bigint", nullable: false, defaultValue: 1L),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warnings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AutoTranslateUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChannelId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Source = table.Column<string>(type: "text", nullable: true),
                    Target = table.Column<string>(type: "text", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoTranslateUsers", x => x.Id);
                    table.UniqueConstraint("AK_AutoTranslateUsers_ChannelId_UserId", x => new { x.ChannelId, x.UserId });
                    table.ForeignKey(
                        name: "FK_AutoTranslateUsers_AutoTranslateChannels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "AutoTranslateChannels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AntiAltSetting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildConfigId = table.Column<int>(type: "integer", nullable: false),
                    MinAge = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Action = table.Column<int>(type: "integer", nullable: false),
                    ActionDurationMinutes = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<decimal>(type: "numeric(20,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntiAltSetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AntiAltSetting_GuildConfigs_GuildConfigId",
                        column: x => x.GuildConfigId,
                        principalTable: "GuildConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AntiRaidSetting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildConfigId = table.Column<int>(type: "integer", nullable: false),
                    UserThreshold = table.Column<int>(type: "integer", nullable: false),
                    Seconds = table.Column<int>(type: "integer", nullable: false),
                    Action = table.Column<int>(type: "integer", nullable: false),
                    PunishDuration = table.Column<int>(type: "integer", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntiRaidSetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AntiRaidSetting_GuildConfigs_GuildConfigId",
                        column: x => x.GuildConfigId,
                        principalTable: "GuildConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AntiSpamSetting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildConfigId = table.Column<int>(type: "integer", nullable: false),
                    Action = table.Column<int>(type: "integer", nullable: false),
                    MessageThreshold = table.Column<int>(type: "integer", nullable: false),
                    MuteTime = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntiSpamSetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AntiSpamSetting_GuildConfigs_GuildConfigId",
                        column: x => x.GuildConfigId,
                        principalTable: "GuildConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommandAlias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Trigger = table.Column<string>(type: "text", nullable: true),
                    Mapping = table.Column<string>(type: "text", nullable: true),
                    GuildConfigId = table.Column<int>(type: "integer", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandAlias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommandAlias_GuildConfigs_GuildConfigId",
                        column: x => x.GuildConfigId,
                        principalTable: "GuildConfigs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CommandCooldown",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Seconds = table.Column<int>(type: "integer", nullable: false),
                    CommandName = table.Column<string>(type: "text", nullable: true),
                    GuildConfigId = table.Column<int>(type: "integer", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandCooldown", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommandCooldown_GuildConfigs_GuildConfigId",
                        column: x => x.GuildConfigId,
                        principalTable: "GuildConfigs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DelMsgOnCmdChannel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    GuildConfigId = table.Column<int>(type: "integer", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DelMsgOnCmdChannel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DelMsgOnCmdChannel_GuildConfigs_GuildConfigId",
                        column: x => x.GuildConfigId,
                        principalTable: "GuildConfigs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FeedSub",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildConfigId = table.Column<int>(type: "integer", nullable: false),
                    ChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedSub", x => x.Id);
                    table.UniqueConstraint("AK_FeedSub_GuildConfigId_Url", x => new { x.GuildConfigId, x.Url });
                    table.ForeignKey(
                        name: "FK_FeedSub_GuildConfigs_GuildConfigId",
                        column: x => x.GuildConfigId,
                        principalTable: "GuildConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilterChannelId",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    GuildConfigId = table.Column<int>(type: "integer", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilterChannelId", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FilterChannelId_GuildConfigs_GuildConfigId",
                        column: x => x.GuildConfigId,
                        principalTable: "GuildConfigs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FilteredWord",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Word = table.Column<string>(type: "text", nullable: true),
                    GuildConfigId = table.Column<int>(type: "integer", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilteredWord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FilteredWord_GuildConfigs_GuildConfigId",
                        column: x => x.GuildConfigId,
                        principalTable: "GuildConfigs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FilterLinksChannelId",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    GuildConfigId = table.Column<int>(type: "integer", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilterLinksChannelId", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FilterLinksChannelId_GuildConfigs_GuildConfigId",
                        column: x => x.GuildConfigId,
                        principalTable: "GuildConfigs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FilterWordsChannelId",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    GuildConfigId = table.Column<int>(type: "integer", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilterWordsChannelId", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FilterWordsChannelId_GuildConfigs_GuildConfigId",
                        column: x => x.GuildConfigId,
                        principalTable: "GuildConfigs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FollowedStream",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    ChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: true),
                    GuildConfigId = table.Column<int>(type: "integer", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowedStream", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FollowedStream_GuildConfigs_GuildConfigId",
                        column: x => x.GuildConfigId,
                        principalTable: "GuildConfigs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GCChannelId",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildConfigId = table.Column<int>(type: "integer", nullable: true),
                    ChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GCChannelId", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GCChannelId_GuildConfigs_GuildConfigId",
                        column: x => x.GuildConfigId,
                        principalTable: "GuildConfigs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GroupName",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildConfigId = table.Column<int>(type: "integer", nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupName", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupName_GuildConfigs_GuildConfigId",
                        column: x => x.GuildConfigId,
                        principalTable: "GuildConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MutedUserId",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    GuildConfigId = table.Column<int>(type: "integer", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MutedUserId", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MutedUserId_GuildConfigs_GuildConfigId",
                        column: x => x.GuildConfigId,
                        principalTable: "GuildConfigs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildConfigId = table.Column<int>(type: "integer", nullable: true),
                    Index = table.Column<int>(type: "integer", nullable: false),
                    PrimaryTarget = table.Column<int>(type: "integer", nullable: false),
                    PrimaryTargetId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    SecondaryTarget = table.Column<int>(type: "integer", nullable: false),
                    SecondaryTargetName = table.Column<string>(type: "text", nullable: true),
                    IsCustomCommand = table.Column<bool>(type: "boolean", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permissions_GuildConfigs_GuildConfigId",
                        column: x => x.GuildConfigId,
                        principalTable: "GuildConfigs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReactionRoleMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Index = table.Column<int>(type: "integer", nullable: false),
                    GuildConfigId = table.Column<int>(type: "integer", nullable: false),
                    ChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    MessageId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Exclusive = table.Column<bool>(type: "boolean", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReactionRoleMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReactionRoleMessage_GuildConfigs_GuildConfigId",
                        column: x => x.GuildConfigId,
                        principalTable: "GuildConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShopEntry",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Index = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    AuthorId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    RoleName = table.Column<string>(type: "text", nullable: true),
                    RoleId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    GuildConfigId = table.Column<int>(type: "integer", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopEntry_GuildConfigs_GuildConfigId",
                        column: x => x.GuildConfigId,
                        principalTable: "GuildConfigs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SlowmodeIgnoredRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    GuildConfigId = table.Column<int>(type: "integer", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SlowmodeIgnoredRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SlowmodeIgnoredRole_GuildConfigs_GuildConfigId",
                        column: x => x.GuildConfigId,
                        principalTable: "GuildConfigs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SlowmodeIgnoredUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    GuildConfigId = table.Column<int>(type: "integer", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SlowmodeIgnoredUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SlowmodeIgnoredUser_GuildConfigs_GuildConfigId",
                        column: x => x.GuildConfigId,
                        principalTable: "GuildConfigs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StreamRoleSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildConfigId = table.Column<int>(type: "integer", nullable: false),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false),
                    AddRoleId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    FromRoleId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Keyword = table.Column<string>(type: "text", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreamRoleSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StreamRoleSettings_GuildConfigs_GuildConfigId",
                        column: x => x.GuildConfigId,
                        principalTable: "GuildConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UnbanTimer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    UnbanAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuildConfigId = table.Column<int>(type: "integer", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnbanTimer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnbanTimer_GuildConfigs_GuildConfigId",
                        column: x => x.GuildConfigId,
                        principalTable: "GuildConfigs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UnmuteTimer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    UnmuteAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuildConfigId = table.Column<int>(type: "integer", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnmuteTimer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnmuteTimer_GuildConfigs_GuildConfigId",
                        column: x => x.GuildConfigId,
                        principalTable: "GuildConfigs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UnroleTimer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    RoleId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    UnbanAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuildConfigId = table.Column<int>(type: "integer", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnroleTimer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnroleTimer_GuildConfigs_GuildConfigId",
                        column: x => x.GuildConfigId,
                        principalTable: "GuildConfigs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VcRoleInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VoiceChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    RoleId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    GuildConfigId = table.Column<int>(type: "integer", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VcRoleInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VcRoleInfo_GuildConfigs_GuildConfigId",
                        column: x => x.GuildConfigId,
                        principalTable: "GuildConfigs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WarningPunishment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Count = table.Column<int>(type: "integer", nullable: false),
                    Punishment = table.Column<int>(type: "integer", nullable: false),
                    Time = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    GuildConfigId = table.Column<int>(type: "integer", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarningPunishment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarningPunishment_GuildConfigs_GuildConfigId",
                        column: x => x.GuildConfigId,
                        principalTable: "GuildConfigs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "XpSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildConfigId = table.Column<int>(type: "integer", nullable: false),
                    ServerExcluded = table.Column<bool>(type: "boolean", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XpSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_XpSettings_GuildConfigs_GuildConfigId",
                        column: x => x.GuildConfigId,
                        principalTable: "GuildConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IgnoredLogChannels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LogSettingId = table.Column<int>(type: "integer", nullable: false),
                    LogItemId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    ItemType = table.Column<int>(type: "integer", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IgnoredLogChannels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IgnoredLogChannels_LogSettings_LogSettingId",
                        column: x => x.LogSettingId,
                        principalTable: "LogSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IgnoredVoicePresenceCHannels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LogSettingId = table.Column<int>(type: "integer", nullable: true),
                    ChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IgnoredVoicePresenceCHannels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IgnoredVoicePresenceCHannels_LogSettings_LogSettingId",
                        column: x => x.LogSettingId,
                        principalTable: "LogSettings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlaylistSong",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Provider = table.Column<string>(type: "text", nullable: true),
                    ProviderType = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Uri = table.Column<string>(type: "text", nullable: true),
                    Query = table.Column<string>(type: "text", nullable: true),
                    MusicPlaylistId = table.Column<int>(type: "integer", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistSong", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlaylistSong_MusicPlaylists_MusicPlaylistId",
                        column: x => x.MusicPlaylistId,
                        principalTable: "MusicPlaylists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PollAnswer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Index = table.Column<int>(type: "integer", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: true),
                    PollId = table.Column<int>(type: "integer", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PollAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PollAnswer_Poll_PollId",
                        column: x => x.PollId,
                        principalTable: "Poll",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PollVote",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    VoteIndex = table.Column<int>(type: "integer", nullable: false),
                    PollId = table.Column<int>(type: "integer", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PollVote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PollVote_Poll_PollId",
                        column: x => x.PollId,
                        principalTable: "Poll",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AntiSpamIgnore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    AntiSpamSettingId = table.Column<int>(type: "integer", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntiSpamIgnore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AntiSpamIgnore_AntiSpamSetting_AntiSpamSettingId",
                        column: x => x.AntiSpamSettingId,
                        principalTable: "AntiSpamSetting",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReactionRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmoteName = table.Column<string>(type: "text", nullable: true),
                    RoleId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    ReactionRoleMessageId = table.Column<int>(type: "integer", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReactionRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReactionRole_ReactionRoleMessage_ReactionRoleMessageId",
                        column: x => x.ReactionRoleMessageId,
                        principalTable: "ReactionRoleMessage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShopEntryItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<string>(type: "text", nullable: true),
                    ShopEntryId = table.Column<int>(type: "integer", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopEntryItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopEntryItem_ShopEntry_ShopEntryId",
                        column: x => x.ShopEntryId,
                        principalTable: "ShopEntry",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StreamRoleBlacklistedUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: true),
                    StreamRoleSettingsId = table.Column<int>(type: "integer", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreamRoleBlacklistedUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StreamRoleBlacklistedUser_StreamRoleSettings_StreamRoleSett~",
                        column: x => x.StreamRoleSettingsId,
                        principalTable: "StreamRoleSettings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StreamRoleWhitelistedUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: true),
                    StreamRoleSettingsId = table.Column<int>(type: "integer", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreamRoleWhitelistedUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StreamRoleWhitelistedUser_StreamRoleSettings_StreamRoleSett~",
                        column: x => x.StreamRoleSettingsId,
                        principalTable: "StreamRoleSettings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExcludedItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    ItemType = table.Column<int>(type: "integer", nullable: false),
                    XpSettingsId = table.Column<int>(type: "integer", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcludedItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExcludedItem_XpSettings_XpSettingsId",
                        column: x => x.XpSettingsId,
                        principalTable: "XpSettings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "XpCurrencyReward",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    XpSettingsId = table.Column<int>(type: "integer", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XpCurrencyReward", x => x.Id);
                    table.ForeignKey(
                        name: "FK_XpCurrencyReward_XpSettings_XpSettingsId",
                        column: x => x.XpSettingsId,
                        principalTable: "XpSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "XpRoleReward",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    XpSettingsId = table.Column<int>(type: "integer", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Remove = table.Column<bool>(type: "boolean", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XpRoleReward", x => x.Id);
                    table.ForeignKey(
                        name: "FK_XpRoleReward_XpSettings_XpSettingsId",
                        column: x => x.XpSettingsId,
                        principalTable: "XpSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClubApplicants",
                columns: table => new
                {
                    ClubId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubApplicants", x => new { x.ClubId, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "ClubBans",
                columns: table => new
                {
                    ClubId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubBans", x => new { x.ClubId, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "Clubs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Discrim = table.Column<int>(type: "integer", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    MinimumLevelReq = table.Column<int>(type: "integer", nullable: false),
                    Xp = table.Column<int>(type: "integer", nullable: false),
                    OwnerId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clubs", x => x.Id);
                    table.UniqueConstraint("AK_Clubs_Name_Discrim", x => new { x.Name, x.Discrim });
                });

            migrationBuilder.CreateTable(
                name: "DiscordUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: true),
                    Discriminator = table.Column<string>(type: "text", nullable: true),
                    AvatarId = table.Column<string>(type: "text", nullable: true),
                    ClubId = table.Column<int>(type: "integer", nullable: true),
                    IsClubAdmin = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    TotalXp = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    LastLevelUp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2022, 3, 21, 0, 4, 23, 650, DateTimeKind.Utc).AddTicks(9075)),
                    LastXpGain = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "datetime('now', '-1 years')"),
                    NotifyOnLevelUp = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CurrencyAmount = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordUser", x => x.Id);
                    table.UniqueConstraint("AK_DiscordUser_UserId", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_DiscordUser_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WaifuInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WaifuId = table.Column<int>(type: "integer", nullable: false),
                    ClaimerId = table.Column<int>(type: "integer", nullable: true),
                    AffinityId = table.Column<int>(type: "integer", nullable: true),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaifuInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaifuInfo_DiscordUser_AffinityId",
                        column: x => x.AffinityId,
                        principalTable: "DiscordUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WaifuInfo_DiscordUser_ClaimerId",
                        column: x => x.ClaimerId,
                        principalTable: "DiscordUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WaifuInfo_DiscordUser_WaifuId",
                        column: x => x.WaifuId,
                        principalTable: "DiscordUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WaifuUpdates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    UpdateType = table.Column<int>(type: "integer", nullable: false),
                    OldId = table.Column<int>(type: "integer", nullable: true),
                    NewId = table.Column<int>(type: "integer", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaifuUpdates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaifuUpdates_DiscordUser_NewId",
                        column: x => x.NewId,
                        principalTable: "DiscordUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WaifuUpdates_DiscordUser_OldId",
                        column: x => x.OldId,
                        principalTable: "DiscordUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WaifuUpdates_DiscordUser_UserId",
                        column: x => x.UserId,
                        principalTable: "DiscordUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WaifuItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WaifuInfoId = table.Column<int>(type: "integer", nullable: true),
                    ItemEmoji = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaifuItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaifuItem_WaifuInfo_WaifuInfoId",
                        column: x => x.WaifuInfoId,
                        principalTable: "WaifuInfo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AntiAltSetting_GuildConfigId",
                table: "AntiAltSetting",
                column: "GuildConfigId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AntiRaidSetting_GuildConfigId",
                table: "AntiRaidSetting",
                column: "GuildConfigId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AntiSpamIgnore_AntiSpamSettingId",
                table: "AntiSpamIgnore",
                column: "AntiSpamSettingId");

            migrationBuilder.CreateIndex(
                name: "IX_AntiSpamSetting_GuildConfigId",
                table: "AntiSpamSetting",
                column: "GuildConfigId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AutoTranslateChannels_ChannelId",
                table: "AutoTranslateChannels",
                column: "ChannelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AutoTranslateChannels_GuildId",
                table: "AutoTranslateChannels",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_BanTemplates_GuildId",
                table: "BanTemplates",
                column: "GuildId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClubApplicants_UserId",
                table: "ClubApplicants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubBans_UserId",
                table: "ClubBans",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_OwnerId",
                table: "Clubs",
                column: "OwnerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommandAlias_GuildConfigId",
                table: "CommandAlias",
                column: "GuildConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_CommandCooldown_GuildConfigId",
                table: "CommandCooldown",
                column: "GuildConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyTransactions_UserId",
                table: "CurrencyTransactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DelMsgOnCmdChannel_GuildConfigId",
                table: "DelMsgOnCmdChannel",
                column: "GuildConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordPermOverrides_GuildId_Command",
                table: "DiscordPermOverrides",
                columns: new[] { "GuildId", "Command" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscordUser_ClubId",
                table: "DiscordUser",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordUser_CurrencyAmount",
                table: "DiscordUser",
                column: "CurrencyAmount");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordUser_TotalXp",
                table: "DiscordUser",
                column: "TotalXp");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordUser_UserId",
                table: "DiscordUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExcludedItem_XpSettingsId",
                table: "ExcludedItem",
                column: "XpSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_FilterChannelId_GuildConfigId",
                table: "FilterChannelId",
                column: "GuildConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_FilteredWord_GuildConfigId",
                table: "FilteredWord",
                column: "GuildConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_FilterLinksChannelId_GuildConfigId",
                table: "FilterLinksChannelId",
                column: "GuildConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_FilterWordsChannelId_GuildConfigId",
                table: "FilterWordsChannelId",
                column: "GuildConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowedStream_GuildConfigId",
                table: "FollowedStream",
                column: "GuildConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_GCChannelId_GuildConfigId",
                table: "GCChannelId",
                column: "GuildConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupName_GuildConfigId_Number",
                table: "GroupName",
                columns: new[] { "GuildConfigId", "Number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GuildConfigs_GuildId",
                table: "GuildConfigs",
                column: "GuildId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GuildConfigs_WarnExpireHours",
                table: "GuildConfigs",
                column: "WarnExpireHours");

            migrationBuilder.CreateIndex(
                name: "IX_IgnoredLogChannels_LogSettingId_LogItemId_ItemType",
                table: "IgnoredLogChannels",
                columns: new[] { "LogSettingId", "LogItemId", "ItemType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IgnoredVoicePresenceCHannels_LogSettingId",
                table: "IgnoredVoicePresenceCHannels",
                column: "LogSettingId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageOnlyChannels_ChannelId",
                table: "ImageOnlyChannels",
                column: "ChannelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LogSettings_GuildId",
                table: "LogSettings",
                column: "GuildId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MusicPlayerSettings_GuildId",
                table: "MusicPlayerSettings",
                column: "GuildId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MutedUserId_GuildConfigId",
                table: "MutedUserId",
                column: "GuildConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_NsfwBlacklistedTags_GuildId",
                table: "NsfwBlacklistedTags",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_GuildConfigId",
                table: "Permissions",
                column: "GuildConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantedCurrency_ChannelId",
                table: "PlantedCurrency",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantedCurrency_MessageId",
                table: "PlantedCurrency",
                column: "MessageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistSong_MusicPlaylistId",
                table: "PlaylistSong",
                column: "MusicPlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_Poll_GuildId",
                table: "Poll",
                column: "GuildId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PollAnswer_PollId",
                table: "PollAnswer",
                column: "PollId");

            migrationBuilder.CreateIndex(
                name: "IX_PollVote_PollId",
                table: "PollVote",
                column: "PollId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_GuildId",
                table: "Quotes",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_Keyword",
                table: "Quotes",
                column: "Keyword");

            migrationBuilder.CreateIndex(
                name: "IX_ReactionRole_ReactionRoleMessageId",
                table: "ReactionRole",
                column: "ReactionRoleMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_ReactionRoleMessage_GuildConfigId",
                table: "ReactionRoleMessage",
                column: "GuildConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_Reminders_When",
                table: "Reminders",
                column: "When");

            migrationBuilder.CreateIndex(
                name: "IX_RewardedUsers_PatreonUserId",
                table: "RewardedUsers",
                column: "PatreonUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SelfAssignableRoles_GuildId_RoleId",
                table: "SelfAssignableRoles",
                columns: new[] { "GuildId", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopEntry_GuildConfigId",
                table: "ShopEntry",
                column: "GuildConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopEntryItem_ShopEntryId",
                table: "ShopEntryItem",
                column: "ShopEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_SlowmodeIgnoredRole_GuildConfigId",
                table: "SlowmodeIgnoredRole",
                column: "GuildConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_SlowmodeIgnoredUser_GuildConfigId",
                table: "SlowmodeIgnoredUser",
                column: "GuildConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_StreamRoleBlacklistedUser_StreamRoleSettingsId",
                table: "StreamRoleBlacklistedUser",
                column: "StreamRoleSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_StreamRoleSettings_GuildConfigId",
                table: "StreamRoleSettings",
                column: "GuildConfigId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StreamRoleWhitelistedUser_StreamRoleSettingsId",
                table: "StreamRoleWhitelistedUser",
                column: "StreamRoleSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_UnbanTimer_GuildConfigId",
                table: "UnbanTimer",
                column: "GuildConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_UnmuteTimer_GuildConfigId",
                table: "UnmuteTimer",
                column: "GuildConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_UnroleTimer_GuildConfigId",
                table: "UnroleTimer",
                column: "GuildConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_UserXpStats_AwardedXp",
                table: "UserXpStats",
                column: "AwardedXp");

            migrationBuilder.CreateIndex(
                name: "IX_UserXpStats_GuildId",
                table: "UserXpStats",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_UserXpStats_UserId",
                table: "UserXpStats",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserXpStats_UserId_GuildId",
                table: "UserXpStats",
                columns: new[] { "UserId", "GuildId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserXpStats_Xp",
                table: "UserXpStats",
                column: "Xp");

            migrationBuilder.CreateIndex(
                name: "IX_VcRoleInfo_GuildConfigId",
                table: "VcRoleInfo",
                column: "GuildConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_WaifuInfo_AffinityId",
                table: "WaifuInfo",
                column: "AffinityId");

            migrationBuilder.CreateIndex(
                name: "IX_WaifuInfo_ClaimerId",
                table: "WaifuInfo",
                column: "ClaimerId");

            migrationBuilder.CreateIndex(
                name: "IX_WaifuInfo_Price",
                table: "WaifuInfo",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_WaifuInfo_WaifuId",
                table: "WaifuInfo",
                column: "WaifuId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WaifuItem_WaifuInfoId",
                table: "WaifuItem",
                column: "WaifuInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_WaifuUpdates_NewId",
                table: "WaifuUpdates",
                column: "NewId");

            migrationBuilder.CreateIndex(
                name: "IX_WaifuUpdates_OldId",
                table: "WaifuUpdates",
                column: "OldId");

            migrationBuilder.CreateIndex(
                name: "IX_WaifuUpdates_UserId",
                table: "WaifuUpdates",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WarningPunishment_GuildConfigId",
                table: "WarningPunishment",
                column: "GuildConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_Warnings_DateAdded",
                table: "Warnings",
                column: "DateAdded");

            migrationBuilder.CreateIndex(
                name: "IX_Warnings_GuildId",
                table: "Warnings",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_Warnings_UserId",
                table: "Warnings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_XpCurrencyReward_XpSettingsId",
                table: "XpCurrencyReward",
                column: "XpSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_XpRoleReward_XpSettingsId_Level",
                table: "XpRoleReward",
                columns: new[] { "XpSettingsId", "Level" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_XpSettings_GuildConfigId",
                table: "XpSettings",
                column: "GuildConfigId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClubApplicants_Clubs_ClubId",
                table: "ClubApplicants",
                column: "ClubId",
                principalTable: "Clubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClubApplicants_DiscordUser_UserId",
                table: "ClubApplicants",
                column: "UserId",
                principalTable: "DiscordUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClubBans_Clubs_ClubId",
                table: "ClubBans",
                column: "ClubId",
                principalTable: "Clubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClubBans_DiscordUser_UserId",
                table: "ClubBans",
                column: "UserId",
                principalTable: "DiscordUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Clubs_DiscordUser_OwnerId",
                table: "Clubs",
                column: "OwnerId",
                principalTable: "DiscordUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscordUser_Clubs_ClubId",
                table: "DiscordUser");

            migrationBuilder.DropTable(
                name: "AntiAltSetting");

            migrationBuilder.DropTable(
                name: "AntiRaidSetting");

            migrationBuilder.DropTable(
                name: "AntiSpamIgnore");

            migrationBuilder.DropTable(
                name: "AutoCommands");

            migrationBuilder.DropTable(
                name: "AutoTranslateUsers");

            migrationBuilder.DropTable(
                name: "BanTemplates");

            migrationBuilder.DropTable(
                name: "Blacklist");

            migrationBuilder.DropTable(
                name: "ClubApplicants");

            migrationBuilder.DropTable(
                name: "ClubBans");

            migrationBuilder.DropTable(
                name: "CommandAlias");

            migrationBuilder.DropTable(
                name: "CommandCooldown");

            migrationBuilder.DropTable(
                name: "CurrencyTransactions");

            migrationBuilder.DropTable(
                name: "DelMsgOnCmdChannel");

            migrationBuilder.DropTable(
                name: "DiscordPermOverrides");

            migrationBuilder.DropTable(
                name: "ExcludedItem");

            migrationBuilder.DropTable(
                name: "Expressions");

            migrationBuilder.DropTable(
                name: "FeedSub");

            migrationBuilder.DropTable(
                name: "FilterChannelId");

            migrationBuilder.DropTable(
                name: "FilteredWord");

            migrationBuilder.DropTable(
                name: "FilterLinksChannelId");

            migrationBuilder.DropTable(
                name: "FilterWordsChannelId");

            migrationBuilder.DropTable(
                name: "FollowedStream");

            migrationBuilder.DropTable(
                name: "GCChannelId");

            migrationBuilder.DropTable(
                name: "GroupName");

            migrationBuilder.DropTable(
                name: "IgnoredLogChannels");

            migrationBuilder.DropTable(
                name: "IgnoredVoicePresenceCHannels");

            migrationBuilder.DropTable(
                name: "ImageOnlyChannels");

            migrationBuilder.DropTable(
                name: "MusicPlayerSettings");

            migrationBuilder.DropTable(
                name: "MutedUserId");

            migrationBuilder.DropTable(
                name: "NsfwBlacklistedTags");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "PlantedCurrency");

            migrationBuilder.DropTable(
                name: "PlaylistSong");

            migrationBuilder.DropTable(
                name: "PollAnswer");

            migrationBuilder.DropTable(
                name: "PollVote");

            migrationBuilder.DropTable(
                name: "Quotes");

            migrationBuilder.DropTable(
                name: "ReactionRole");

            migrationBuilder.DropTable(
                name: "Reminders");

            migrationBuilder.DropTable(
                name: "Repeaters");

            migrationBuilder.DropTable(
                name: "RewardedUsers");

            migrationBuilder.DropTable(
                name: "RotatingStatus");

            migrationBuilder.DropTable(
                name: "SelfAssignableRoles");

            migrationBuilder.DropTable(
                name: "ShopEntryItem");

            migrationBuilder.DropTable(
                name: "SlowmodeIgnoredRole");

            migrationBuilder.DropTable(
                name: "SlowmodeIgnoredUser");

            migrationBuilder.DropTable(
                name: "StreamRoleBlacklistedUser");

            migrationBuilder.DropTable(
                name: "StreamRoleWhitelistedUser");

            migrationBuilder.DropTable(
                name: "UnbanTimer");

            migrationBuilder.DropTable(
                name: "UnmuteTimer");

            migrationBuilder.DropTable(
                name: "UnroleTimer");

            migrationBuilder.DropTable(
                name: "UserXpStats");

            migrationBuilder.DropTable(
                name: "VcRoleInfo");

            migrationBuilder.DropTable(
                name: "WaifuItem");

            migrationBuilder.DropTable(
                name: "WaifuUpdates");

            migrationBuilder.DropTable(
                name: "WarningPunishment");

            migrationBuilder.DropTable(
                name: "Warnings");

            migrationBuilder.DropTable(
                name: "XpCurrencyReward");

            migrationBuilder.DropTable(
                name: "XpRoleReward");

            migrationBuilder.DropTable(
                name: "AntiSpamSetting");

            migrationBuilder.DropTable(
                name: "AutoTranslateChannels");

            migrationBuilder.DropTable(
                name: "LogSettings");

            migrationBuilder.DropTable(
                name: "MusicPlaylists");

            migrationBuilder.DropTable(
                name: "Poll");

            migrationBuilder.DropTable(
                name: "ReactionRoleMessage");

            migrationBuilder.DropTable(
                name: "ShopEntry");

            migrationBuilder.DropTable(
                name: "StreamRoleSettings");

            migrationBuilder.DropTable(
                name: "WaifuInfo");

            migrationBuilder.DropTable(
                name: "XpSettings");

            migrationBuilder.DropTable(
                name: "GuildConfigs");

            migrationBuilder.DropTable(
                name: "Clubs");

            migrationBuilder.DropTable(
                name: "DiscordUser");
        }
    }
}
