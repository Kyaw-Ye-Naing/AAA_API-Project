using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AAA_API.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_confirmLeague",
                columns: table => new
                {
                    confirmLeagueId = table.Column<decimal>(type: "decimal(18, 0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    leagueId = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    rapidLeagueId = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    active = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_confirmLeague", x => x.confirmLeagueId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_credit",
                columns: table => new
                {
                    creditId = table.Column<decimal>(type: "decimal(18, 0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    postingNo = table.Column<string>(maxLength: 50, nullable: true),
                    userId = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    amount = table.Column<int>(nullable: true),
                    active = table.Column<bool>(nullable: true),
                    createdBy = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    createdDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Credit", x => x.creditId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_footballTeam",
                columns: table => new
                {
                    footballTeamId = table.Column<decimal>(type: "decimal(18, 0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rapidTeamId = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    footballTeam = table.Column<string>(maxLength: 50, nullable: true),
                    footballTeamMyan = table.Column<string>(maxLength: 50, nullable: true),
                    leagueId = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    active = table.Column<bool>(nullable: true),
                    createdBy = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    createdDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_footballTeam", x => x.footballTeamId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_gambling",
                columns: table => new
                {
                    gamblingId = table.Column<decimal>(type: "decimal(18, 0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    postingNo = table.Column<string>(maxLength: 50, nullable: true),
                    gamblingTypeId = table.Column<int>(nullable: true),
                    eventId = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    rapidEventId = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    teamCount = table.Column<int>(nullable: true),
                    amount = table.Column<int>(nullable: true),
                    active = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_singleGambling", x => x.gamblingId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_gamblingDetails",
                columns: table => new
                {
                    gamblingDetailsId = table.Column<decimal>(type: "decimal(18, 0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    gamblingId = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    leagueId = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    footballTeamId = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    under = table.Column<bool>(nullable: true),
                    overs = table.Column<bool>(nullable: true),
                    amount = table.Column<decimal>(type: "decimal(18, 0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_gamblingDetails", x => x.gamblingDetailsId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_gamblingType",
                columns: table => new
                {
                    gamblingTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    gamblingType = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_gamblingType", x => x.gamblingTypeId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_gamblingWin",
                columns: table => new
                {
                    gamblingWinId = table.Column<decimal>(type: "decimal(18, 0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    gamblingId = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    userId = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    goalResultId = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    winAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    gamblingTypeId = table.Column<int>(nullable: true),
                    active = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_gamblingWin", x => x.gamblingWinId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_goalResult",
                columns: table => new
                {
                    goalResultId = table.Column<decimal>(type: "decimal(18, 0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rapidEventId = table.Column<string>(fixedLength: true, maxLength: 10, nullable: true),
                    homeResult = table.Column<int>(nullable: true),
                    awayResult = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_goalResult", x => x.goalResultId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_handicap",
                columns: table => new
                {
                    handicapId = table.Column<decimal>(type: "decimal(18, 0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rapidEventId = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    homeOdd = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    homeHandicap = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    awayOdd = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    awayHandicap = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    eventDatetime = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_handicap", x => x.handicapId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_league",
                columns: table => new
                {
                    leagueId = table.Column<decimal>(type: "decimal(18, 0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    leagueName = table.Column<string>(maxLength: 50, nullable: true),
                    rapidLeagueId = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    active = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_league", x => x.leagueId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_preUpcomingEvent",
                columns: table => new
                {
                    preUpcommingEventId = table.Column<decimal>(type: "decimal(18, 0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rapidEventId = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    leagueId = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    homeTeamId = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    awayTeamId = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    eventDate = table.Column<DateTime>(type: "date", nullable: true),
                    eventTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    active = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_preUpcomingEvent", x => x.preUpcommingEventId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_role",
                columns: table => new
                {
                    roleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role = table.Column<string>(maxLength: 50, nullable: true),
                    active = table.Column<bool>(nullable: true),
                    discription = table.Column<string>(maxLength: 50, nullable: true),
                    createdBy = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    createdDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_role", x => x.roleId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_upcomingEvent",
                columns: table => new
                {
                    upcomingEventId = table.Column<decimal>(type: "decimal(18, 0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rapidEventId = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    leagueId = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    homeTeamId = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    awayTeamId = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    eventDate = table.Column<DateTime>(type: "date", nullable: true),
                    eventTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    active = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_upComingEvent", x => x.upcomingEventId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_user",
                columns: table => new
                {
                    userId = table.Column<decimal>(type: "decimal(18, 0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(maxLength: 50, nullable: true),
                    password = table.Column<string>(maxLength: 250, nullable: true),
                    @lock = table.Column<bool>(name: "lock", nullable: true),
                    roleId = table.Column<int>(nullable: true),
                    mobile = table.Column<string>(maxLength: 50, nullable: true),
                    sharePercent = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    betLimitForMix = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    betLimitForSingle = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    singleBetCommission5 = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    singleBetCommission8 = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    mixBetCommission2count15 = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    mixBetCommission3count20 = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    mixBetCommission4count20 = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    mixBetCommission5count20 = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    mixBetCommission6count20 = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    mixBetCommission7count20 = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    mixBetCommission8count20 = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    mixBetCommission9count25 = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    mixBetCommission10count25 = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    mixBetCommission11count25 = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    createdBy = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    createdDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_user", x => x.userId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_userBalance",
                columns: table => new
                {
                    userBalanceId = table.Column<decimal>(type: "decimal(18, 0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    postingNo = table.Column<string>(maxLength: 50, nullable: true),
                    userId = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    inward = table.Column<int>(nullable: true),
                    outward = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_userBalance", x => x.userBalanceId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_userPosting",
                columns: table => new
                {
                    userPostingId = table.Column<decimal>(type: "decimal(18, 0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    postingNo = table.Column<string>(maxLength: 50, nullable: true),
                    userId = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    Inward = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    outward = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    active = table.Column<bool>(nullable: true),
                    createdBy = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    createdDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_userPosting", x => x.userPostingId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_confirmLeague");

            migrationBuilder.DropTable(
                name: "tbl_credit");

            migrationBuilder.DropTable(
                name: "tbl_footballTeam");

            migrationBuilder.DropTable(
                name: "tbl_gambling");

            migrationBuilder.DropTable(
                name: "tbl_gamblingDetails");

            migrationBuilder.DropTable(
                name: "tbl_gamblingType");

            migrationBuilder.DropTable(
                name: "tbl_gamblingWin");

            migrationBuilder.DropTable(
                name: "tbl_goalResult");

            migrationBuilder.DropTable(
                name: "tbl_handicap");

            migrationBuilder.DropTable(
                name: "tbl_league");

            migrationBuilder.DropTable(
                name: "tbl_preUpcomingEvent");

            migrationBuilder.DropTable(
                name: "tbl_role");

            migrationBuilder.DropTable(
                name: "tbl_upcomingEvent");

            migrationBuilder.DropTable(
                name: "tbl_user");

            migrationBuilder.DropTable(
                name: "tbl_userBalance");

            migrationBuilder.DropTable(
                name: "tbl_userPosting");
        }
    }
}
