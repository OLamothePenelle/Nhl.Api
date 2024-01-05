﻿using Nhl.Api.Common.Exceptions;
using Nhl.Api.Common.Extensions;
using Nhl.Api.Common.Http;
using Nhl.Api.Enumerations.Game;
using Nhl.Api.Enumerations.Statistic;
using Nhl.Api.Models.Enumerations.Player;
using Nhl.Api.Models.Enumerations.Team;
using Nhl.Api.Models.Season;
using Nhl.Api.Models.Statistics;
using Nhl.Api.Models.Team;
using Nhl.Api.Services;
using System.Linq;
using System.Threading;

namespace Nhl.Api;

/// <summary>
/// The official unofficial NHL Statistics API providing various NHL information about in-depth player statistics, team statistics and more
/// </summary>
public class NhlStatisticsApi : INhlStatisticsApi
{
    private static readonly INhlTeamService _nhlTeamService = new NhlTeamService();
    private static readonly INhlApiHttpClient _nhlApiWebHttpClient = new NhlApiWebHttpClient();
    private static readonly INhlGameApi _nhlGameApi = new NhlGameApi();
    private static readonly INhlLeagueApi _nhlLeagueApi = new NhlLeagueApi();
    private static readonly INhlPlayerApi _nhlPlayerApi = new NhlPlayerApi();


    /// <summary>
    /// The official unofficial NHL Statistics API providing various NHL information about in-depth player statistics, team statistics and more
    /// </summary>
    public NhlStatisticsApi()
    {

    }

    /// <summary>
    /// Returns the NHL goalie statistics leaders in the NHL for a specific goalie statistic type based on the game type and season year 
    /// </summary>
    /// <param name="goalieStatisticsType">A player statistics type, <see cref="GoalieStatisticsType"/> for all the types of statistics</param>
    /// <param name="seasonYear">The NHL season year to retrieve the player statistics leaders for, see <see cref="SeasonYear"/> for more information on valid season years</param>
    /// <param name="gameType">The NHL game type to retrieve the player statistics leaders for, see <see cref="GameType"/> for more information on valid game types</param>
    /// <param name="limit">The limit to the number of results returned when reviewing the NHL player statistics</param>
    /// <param name="cancellationToken"> A cancellation token that can be used by other objects or threads to receive notice of cancellation</param>
    /// <returns> Returns the current NHL player statistics leaders in the NHL for a specific player statistic type </returns>
    public async Task<GoalieStatisticLeaders> GetGoalieStatisticsLeadersAsync(GoalieStatisticsType goalieStatisticsType, GameType gameType, string seasonYear, int limit = 25, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(seasonYear))
        {
            throw new ArgumentException("A season year must be provided to retrieve the NHL player statistics leaders for");
        }

        if (seasonYear.Length != 8)
        {
            throw new ArgumentException("The season year must be 8 digits in length");
        }

        if (limit < 1)
        {
            throw new ArgumentException("The limit must be more than or equal to 1");
        }

        return await _nhlApiWebHttpClient.GetAsync<GoalieStatisticLeaders>($"/goalie-stats-leaders/{seasonYear}/{(int)gameType}?categories={goalieStatisticsType.GetEnumMemberValue()}&limit={limit}", cancellationToken);
    }

    /// <summary>
    /// Returns the current NHL player statistics leaders in the NHL for a specific player statistic type
    /// </summary>
    /// <param name="playerStatisticsType">A player statistics type, <see cref="PlayerStatisticsType"/> for all the types of statistics</param>
    /// <param name="seasonYear">The NHL season year to retrieve the player statistics leaders for, see <see cref="SeasonYear"/> for more information on valid season years</param>
    /// <param name="gameType">The NHL game type to retrieve the player statistics leaders for, see <see cref="GameType"/> for more information on valid game types</param>
    /// <param name="limit">The limit to the number of results returned when reviewing the NHL player statistics</param>
    /// <param name="cancellationToken"> A cancellation token that can be used by other objects or threads to receive notice of cancellation</param>
    /// <returns> Returns the current NHL player statistics leaders in the NHL for a specific player statistic type </returns>
    public async Task<PlayerStatisticLeaders> GetSkaterStatisticsLeadersAsync(PlayerStatisticsType playerStatisticsType, GameType gameType, string seasonYear, int limit = 25, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(seasonYear))
        {
            throw new ArgumentException("A season year must be provided to retrieve the NHL player statistics leaders for");
        }

        if (seasonYear.Length != 8)
        {
            throw new ArgumentException("The season year must be 8 digits in length");
        }

        if (limit < 1)
        {
            throw new ArgumentException("The limit must be more than or equal to 1");
        }

        return await _nhlApiWebHttpClient.GetAsync<PlayerStatisticLeaders>($"/skater-stats-leaders/{seasonYear}/{(int)gameType}?categories={playerStatisticsType.GetEnumMemberValue()}&limit={limit}", cancellationToken);
    }

    /// <summary>
    /// Returns the NHL team statistics for individual players for a specific NHL team statistic type based on the game type and season year
    /// </summary>
    /// <param name="team">The team enumeration identifier, specifying which the NHL team, <see cref="TeamEnum"/> for more information </param>
    /// <param name="seasonYear">The NHL season year to retrieve the team statistics, see <see cref="SeasonYear"/> for more information on valid season years</param>
    /// <param name="gameType">The NHL game type to retrieve the team statistics, see <see cref="GameType"/> for more information on valid game types</param>
    /// <param name="cancellationToken"> A cancellation token that can be used by other objects or threads to receive notice of cancellation</param>
    /// <returns>The NHL team season statistics for the specified season and game type</returns>
    public async Task<TeamSeasonStatistics> GetTeamStatisticsBySeasonAndGameTypeAsync(TeamEnum team, string seasonYear, GameType gameType, CancellationToken cancellationToken = default)
    {
        var teamAbbreviaton = _nhlTeamService.GetTeamCodeIdentfierByTeamEnumeration(team);
        return await _nhlApiWebHttpClient.GetAsync<TeamSeasonStatistics>($"/club-stats/{teamAbbreviaton}/{seasonYear}/{(int)gameType}", cancellationToken);
    }

    /// <summary>
    /// Returns the NHL team statistics for individual players for a specific NHL team statistic type based on the game type and season year
    /// </summary>
    /// <param name="teamId">The NHL team identifier, specifying which the NHL team, Example: 55 - Seattle Kraken </param>
    /// <param name="seasonYear">The NHL season year to retrieve the team statistics, see <see cref="SeasonYear"/> for more information on valid season years</param>
    /// <param name="gameType">The NHL game type to retrieve the team statistics, see <see cref="GameType"/> for more information on valid game types</param>
    /// <param name="cancellationToken"> A cancellation token that can be used by other objects or threads to receive notice of cancellation</param>
    /// <returns>The NHL team season statistics for the specified season and game type</returns>
    public async Task<TeamSeasonStatistics> GetTeamStatisticsBySeasonAndGameTypeAsync(int teamId, string seasonYear, GameType gameType, CancellationToken cancellationToken = default)
    {
        var teamAbbreviaton = _nhlTeamService.GetTeamCodeIdentfierByTeamId(teamId);
        return await _nhlApiWebHttpClient.GetAsync<TeamSeasonStatistics>($"/club-stats/{teamAbbreviaton}/{seasonYear}/{(int)gameType}", cancellationToken);
    }

    /// <summary>
    /// Returns all the NHL team valid game types for all valid NHL seasons for the selected NHL team 
    /// </summary>
    /// <param name="team">The team enumeration identifier, specifying which the NHL team, <see cref="TeamEnum"/> for more information </param>
    /// <param name="cancellationToken"> A cancellation token that can be used by other objects or threads to receive notice of cancellation</param>
    /// <returns> Returns all the NHL team valid game types for all valid NHL seasons for the selected NHL team </returns>
    public async Task<List<TeamStatisticsSeason>> GetTeamStatisticsBySeasonAsync(TeamEnum team, CancellationToken cancellationToken = default)
    {
        var teamAbbreviaton = _nhlTeamService.GetTeamCodeIdentfierByTeamEnumeration(team);
        return await _nhlApiWebHttpClient.GetAsync<List<TeamStatisticsSeason>>($"/club-stats-season/{teamAbbreviaton}", cancellationToken);
    }

    /// <summary>
    /// Returns all the NHL team valid game types for all valid NHL seasons for the selected NHL team 
    /// </summary>
    /// <param name="teamId">The NHL team identifier, specifying which the NHL team, Example: 55 - Seattle Kraken </param>
    /// <param name="cancellationToken"> A cancellation token that can be used by other objects or threads to receive notice of cancellation</param>
    /// <returns> Returns all the NHL team valid game types for all valid NHL seasons for the selected NHL team </returns>
    public async Task<List<TeamStatisticsSeason>> GetTeamStatisticsBySeasonAsync(int teamId, CancellationToken cancellationToken = default)
    {
        var teamAbbreviaton = _nhlTeamService.GetTeamCodeIdentfierByTeamId(teamId);
        return await _nhlApiWebHttpClient.GetAsync<List<TeamStatisticsSeason>>($"/club-stats-season/{teamAbbreviaton}", cancellationToken);
    }

    /// <summary>
    /// Returns the number of face offs won by a player for a specific season and game type
    /// </summary>
    /// <param name="playerEnum">The player enumeration identifier, specifying which the NHL player, <see cref="PlayerEnum"/> for more information </param>
    /// <param name="playerGameCenterStatistic">The NHL player game center statistic type, <see cref="PlayerGameCenterStatistic"/> for more information on valid game center statistics</param>
    /// <param name="seasonYear">The NHL season year to retrieve the team statistics, see <see cref="SeasonYear"/> for more information on valid season years</param>
    /// <param name="cancellationToken"> A cancellation token that can be used by other objects or threads to receive notice of cancellation</param>
    /// <returns>Returns the number of face offs won by a player for a specific season and game type</returns>
    public async Task<int> GetTotalPlayerStatisticValueByTypeAndSeasonAsync(PlayerEnum playerEnum, PlayerGameCenterStatistic playerGameCenterStatistic, string seasonYear, CancellationToken cancellationToken = default)
    {
        var statisticTotal = 0;
        if (string.IsNullOrWhiteSpace(seasonYear) || seasonYear.Length != 8)
        {
            throw new ArgumentException("The season year provided is invalid");
        }

        // Get player
        var player = await _nhlPlayerApi.GetPlayerInformationAsync(playerEnum, cancellationToken);
        if (player.Position == "G")
        {
            throw new InvalidPlayerPositionException($"The player id {playerEnum} provided is a goaltender and is not a valid player");
        }

        // Get team season schedule
        var schedule = await _nhlLeagueApi.GetTeamScheduleBySeasonAsync(player.CurrentTeamAbbrev, seasonYear, cancellationToken);

        // Create tasks to retrieve game information
        var tasks = schedule.Games.Select(async game =>
        {
            // Count number of game events where player won a faceoff
            var gameCenterPlayByPlay = await _nhlGameApi.GetGameCenterPlayByPlayByGameIdAsync(game.Id, cancellationToken);
            return gameCenterPlayByPlay.Plays.Count(play =>
            {
                var playerId = (int)playerEnum;
                return playerGameCenterStatistic switch
                {
                    PlayerGameCenterStatistic.FaceOffWon => play.TypeDescKey == "faceoff" && play.Details.WinningPlayerId == playerId,
                    PlayerGameCenterStatistic.FaceOffLost => play.TypeDescKey == "faceoff" && play.Details.LosingPlayerId == playerId,
                    PlayerGameCenterStatistic.HitGiven => play.TypeDescKey == "hit" && play.Details.HittingPlayerId == playerId,
                    PlayerGameCenterStatistic.HitReceived => play.TypeDescKey == "hit" && play.Details.HitteePlayerId == playerId,
                    PlayerGameCenterStatistic.ShotOnGoal => play.TypeDescKey == "shot-on-goal" && play.Details.ShootingPlayerId == playerId,
                    PlayerGameCenterStatistic.MissedShot => play.TypeDescKey == "missed-shot" && play.Details.ShootingPlayerId == playerId,
                    PlayerGameCenterStatistic.BlockedShot => play.TypeDescKey == "blocked-shot" && play.Details.BlockingPlayerId == playerId,
                    PlayerGameCenterStatistic.Giveaway => play.TypeDescKey == "giveaway" && play.Details.PlayerId == playerId,
                    PlayerGameCenterStatistic.Penalty => play.TypeDescKey == "penalty" && play.Details.DrawnByPlayerId == playerId,
                    PlayerGameCenterStatistic.Takeaway => play.TypeDescKey == "takeaway" && play.Details.PlayerId == playerId,
                    _ => false,
                };
            });
        });

        // Wait for all tasks to complete
        var results = await Task.WhenAll(tasks);

        // Sum up the results
        statisticTotal = results.Sum();

        return statisticTotal;
    }

    /// <summary>
    /// Returns the number of face offs won by a player for a specific season and game type
    /// </summary>
    /// <param name="playerId">The NHL player identifier, specifying which the NHL player, Example: 8478402 - Connor McDavid </param>
    /// <param name="playerGameCenterStatistic">The NHL player game center statistic type, <see cref="PlayerGameCenterStatistic"/> for more information on valid game center statistics</param>
    /// <param name="seasonYear">The NHL season year to retrieve the team statistics, see <see cref="SeasonYear"/> for more information on valid season years</param>
    /// <param name="cancellationToken">A cancellation token to cancel the asynchronous operation</param>
    /// <returns>Returns the number of face offs won by a player for a specific season and game type</returns>
    public async Task<int> GetTotalPlayerStatisticValueByTypeAndSeasonAsync(int playerId, PlayerGameCenterStatistic playerGameCenterStatistic,  string seasonYear, CancellationToken cancellationToken = default)
    {
        var statisticTotal = 0;
        if (string.IsNullOrWhiteSpace(seasonYear) || seasonYear.Length != 8)
        {
            throw new ArgumentException("The season year provided is invalid");
        }

        // Get player
        var player = await _nhlPlayerApi.GetPlayerInformationAsync(playerId, cancellationToken);
        if (player.Position == "G")
        {
            throw new InvalidPlayerPositionException($"The player id {playerId} provided is a goaltender and is not a valid player");
        }

        // Get team season schedule
        var schedule = await _nhlLeagueApi.GetTeamScheduleBySeasonAsync(player.CurrentTeamAbbrev, seasonYear, cancellationToken);

        // Create tasks to retrieve game information
        var tasks = schedule.Games.Select(async game =>
        {
            // Count number of game events where player won a faceoff
            var gameCenterPlayByPlay = await _nhlGameApi.GetGameCenterPlayByPlayByGameIdAsync(game.Id, cancellationToken);
            return gameCenterPlayByPlay.Plays.Count(play => 
            {
                return playerGameCenterStatistic switch
                {
                    PlayerGameCenterStatistic.FaceOffWon => play.TypeDescKey == "faceoff" && play.Details.WinningPlayerId == playerId,
                    PlayerGameCenterStatistic.FaceOffLost => play.TypeDescKey == "faceoff" && play.Details.LosingPlayerId == playerId,
                    PlayerGameCenterStatistic.HitGiven => play.TypeDescKey == "hit" && play.Details.HittingPlayerId == playerId,
                    PlayerGameCenterStatistic.HitReceived => play.TypeDescKey == "hit" && play.Details.HitteePlayerId == playerId,
                    PlayerGameCenterStatistic.ShotOnGoal => play.TypeDescKey == "shot-on-goal" && play.Details.ShootingPlayerId == playerId,
                    PlayerGameCenterStatistic.MissedShot => play.TypeDescKey == "missed-shot" && play.Details.ShootingPlayerId == playerId,
                    PlayerGameCenterStatistic.BlockedShot => play.TypeDescKey == "blocked-shot" && play.Details.BlockingPlayerId == playerId,
                    PlayerGameCenterStatistic.Giveaway => play.TypeDescKey == "giveaway" && play.Details.PlayerId == playerId,
                    PlayerGameCenterStatistic.Penalty => play.TypeDescKey == "penalty" && play.Details.DrawnByPlayerId == playerId,
                    PlayerGameCenterStatistic.Takeaway => play.TypeDescKey == "takeaway" && play.Details.PlayerId == playerId,
                    _ => false,
                };
            });
        });

        // Wait for all tasks to complete
        var results = await Task.WhenAll(tasks);

        // Sum up the results
        statisticTotal = results.Sum();

        return statisticTotal;
    }
}
