﻿using Nhl.Api.Common.Exceptions;
using Nhl.Api.Common.Http;
using Nhl.Api.Models.Enumerations.Team;
using Nhl.Api.Models.Game;
using Nhl.Api.Models.Player;
using Nhl.Api.Models.Season;
using Nhl.Api.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nhl.Api
{
    /// <summary>
    /// The official unofficial NHL Game API providing various NHL information game information, game schedules, live game feeds and more
    /// </summary>
    public class NhlGameApi : INhlGameApi
    {
        private static readonly INhlApiHttpClient _nhlStatsApiHttpClient = new NhlStatsApiHttpClient();
        private static readonly INhlApiHttpClient _nhlShiftChartHttpClient = new NhlShiftChartHttpClient();
        private static readonly INhlLeagueApi _nhlLeagueApi = new NhlLeagueApi();
        private static readonly INhlGameService _nhlGameService = new NhlGameService();

        /// <summary>
        /// The official unofficial NHL Game API providing various NHL information game information, game schedules, live game feeds and more
        /// </summary>
        public NhlGameApi()
        {

        }

        /// <summary>
        /// Returns the box score content for an NHL game
        /// </summary>
        /// <param name="gameId">The game id, Example: 2021020087</param>
        /// <returns>Returns information about the current score, penalties, players, team statistics and more</returns>
        public async Task<Boxscore> GetBoxScoreByIdAsync(int gameId)
        {
            return await _nhlStatsApiHttpClient.GetAsync<Boxscore>($"/game/{gameId}/boxscore");
        }

        /// <summary>
        /// Return's today's the NHL game schedule and it will provide today's current NHL game schedule 
        /// </summary>
        /// <param name="gameScheduleConfiguration">A configuration for the NHL game schedule to include various points of additional information</param>
        /// <returns>NHL game schedule, see <see cref="GameSchedule"/> for more information</returns>
        public async Task<GameSchedule> GetGameScheduleAsync(GameScheduleConfiguration gameScheduleConfiguration = null)
        {
            var httpRequestUri = "/schedule";
            if (gameScheduleConfiguration != null)
            {
                httpRequestUri = _nhlGameService.SetGameScheduleConfiguration(httpRequestUri, gameScheduleConfiguration);
            }

            return await _nhlStatsApiHttpClient.GetAsync<GameSchedule>(httpRequestUri);
        }

        /// <summary>
        /// Return's the NHL game schedule based on the provided <see cref="DateTime"/>. If the date is null, it will provide today's current NHL game schedule 
        /// </summary>
        /// <param name="date">The requested date for the NHL game schedule</param>
        /// <param name="gameScheduleConfiguration">A configuration for the NHL game schedule to include various points of additional information</param>
        /// <returns>NHL game schedule, see <see cref="GameSchedule"/> for more information</returns>
        public async Task<GameSchedule> GetGameScheduleByDateAsync(DateTime? date, GameScheduleConfiguration gameScheduleConfiguration = null)
        {
            var httpRequestUri = (date.HasValue ? $"/schedule?date={date.Value:yyyy-MM-dd}" : "/schedule");

            if (gameScheduleConfiguration != null)
            {
                httpRequestUri = _nhlGameService.SetGameScheduleConfiguration(httpRequestUri, gameScheduleConfiguration);
            }

            return await _nhlStatsApiHttpClient.GetAsync<GameSchedule>(httpRequestUri.ToString());
        }

        /// <summary>
        /// Return's the NHL game schedule based on the provided year, month and day
        /// </summary>
        /// <param name="year">The requested year for the NHL game schedule</param>
        /// <param name="month">The requested month for the NHL game schedule</param>
        /// <param name="day">The requested day for the NHL game schedule</param>
        /// <param name="gameScheduleConfiguration">A configuration for the NHL game schedule to include various points of additional information</param>
        /// <returns>NHL game schedule, see <see cref="GameSchedule"/> for more information</returns>
        public async Task<GameSchedule> GetGameScheduleByDateAsync(int year, int month, int day, GameScheduleConfiguration gameScheduleConfiguration = null)
        {
            var httpRequestUri = $"/schedule?date={year}-{month}-{day}";
            if (gameScheduleConfiguration != null)
            {
                httpRequestUri = _nhlGameService.SetGameScheduleConfiguration(httpRequestUri, gameScheduleConfiguration);
            }

            return await _nhlStatsApiHttpClient.GetAsync<GameSchedule>(httpRequestUri);
        }

        /// <summary>
        /// Return's the NHL game schedule for the specified team based on the provided start date and end date
        /// </summary>
        /// <param name="team">The NHL team id, Example: <see cref="TeamEnum.AnaheimDucks"/></param>
        /// <param name="startDate">The starting date for the NHL team game schedule, see <see cref="LeagueSeasonDates"/> for start dates of NHL seasons, Example: 2017-01-01</param>
        /// <param name="endDate">The ending date for the NHL team game schedule, see <see cref="LeagueSeasonDates"/> for start dates of NHL seasons, Example: 1988-06-01</param>
        /// <param name="gameScheduleConfiguration">A configuration for the NHL game schedule to include various points of additional information</param>
        /// <returns>Returns all of the NHL team's game schedules based on the selected start and end dates</returns>
        public async Task<GameSchedule> GetGameScheduleForTeamByDateAsync(TeamEnum team, DateTime startDate, DateTime endDate, GameScheduleConfiguration gameScheduleConfiguration = null)
        {
            var httpRequestUri = $"/schedule?teamId={(int)team}&startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";
            if (gameScheduleConfiguration != null)
            {
                httpRequestUri = _nhlGameService.SetGameScheduleConfiguration(httpRequestUri, gameScheduleConfiguration);
            }

            return await _nhlStatsApiHttpClient.GetAsync<GameSchedule>(httpRequestUri);
        }

        /// <summary>
        /// Return's the NHL game schedule for the specified team based on the provided start date and end date
        /// </summary>
        /// <param name="teamId">The NHL team id, Example: 1</param>
        /// <param name="startDate">The starting date for the NHL team game schedule, see <see cref="LeagueSeasonDates"/> for start dates of NHL seasons, Example: 2017-01-01</param>
        /// <param name="endDate">The ending date for the NHL team game schedule, see <see cref="LeagueSeasonDates"/> for start dates of NHL seasons, Example: 1988-06-01</param>
        /// <param name="gameScheduleConfiguration">A configuration for the NHL game schedule to include various points of additional information</param>
        /// <returns>Returns all of the NHL team's game schedules based on the selected start and end dates</returns>
        public async Task<GameSchedule> GetGameScheduleForTeamByDateAsync(int teamId, DateTime startDate, DateTime endDate, GameScheduleConfiguration gameScheduleConfiguration = null)
        {
            var httpRequestUri = $"/schedule?teamId={teamId}&startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";
            if (gameScheduleConfiguration != null)
            {
                httpRequestUri = _nhlGameService.SetGameScheduleConfiguration(httpRequestUri, gameScheduleConfiguration);
            }

            return await _nhlStatsApiHttpClient.GetAsync<GameSchedule>(httpRequestUri);
        }

        /// <summary>
        /// Return's the entire collection of NHL game schedules for the specified season
        /// </summary>
        /// <param name="seasonYear">The NHL season year, Example: 19992000, see <see cref="SeasonYear"/> for more information</param>
        /// <param name="includePlayoffGames">Includes all of the NHL playoff games, default value is false</param>
        /// <param name="gameScheduleConfiguration">A configuration for the NHL game schedule to include various points of additional information</param>
        /// <returns>Returns all of the NHL team's game schedules based on the selected NHL season</returns>
        public async Task<GameSchedule> GetGameScheduleBySeasonAsync(string seasonYear, bool includePlayoffGames = false, GameScheduleConfiguration gameScheduleConfiguration = null)
        {
            if (string.IsNullOrEmpty(seasonYear))
            {
                throw new ArgumentNullException(nameof(seasonYear));
            }

            if (seasonYear.Length > 8)
            {
                throw new ArgumentException($"{nameof(seasonYear)} is not a valid season year format");
            }

            var selectedSeason = await _nhlLeagueApi.GetSeasonByYearAsync(seasonYear);
            if (selectedSeason == null)
            {
                throw new InvalidSeasonException($"{seasonYear} is not a valid NHL season");
            }

            var startDate = selectedSeason.RegularSeasonStartDate;
            var endDate = includePlayoffGames ? selectedSeason.SeasonEndDate : selectedSeason.RegularSeasonEndDate;

            var httpRequestUri = $"/schedule?&startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";
            if (gameScheduleConfiguration != null)
            {
                httpRequestUri = _nhlGameService.SetGameScheduleConfiguration(httpRequestUri, gameScheduleConfiguration);
            }

            return await _nhlStatsApiHttpClient.GetAsync<GameSchedule>(httpRequestUri);
        }

        /// <summary>
        /// Returns all of the valid NHL game statuses of an NHL game
        /// </summary>
        /// <returns>A collection of NHL game statues, see <see cref="GameStatus"/> for more information</returns>
        public async Task<List<GameStatus>> GetGameStatusesAsync()
        {
            return await _nhlStatsApiHttpClient.GetAsync<List<GameStatus>>($"/gameStatus");
        }

        /// <summary>
        /// Returns all of the NHL game types within a season and within special events
        /// </summary>
        /// <returns>A collection of NHL and other sporting event game types, see <see cref="GameType"/> for more information </returns>
        public async Task<List<GameType>> GetGameTypesAsync()
        {
            return await _nhlStatsApiHttpClient.GetAsync<List<GameType>>($"/gameTypes");
        }

        /// <summary>
        /// Returns the line score content for an NHL game
        /// </summary>
        /// <param name="gameId">The game id, Example: 2021020087</param>
        /// <returns>Returns information about the current score, strength of the play, time remaining, shots on goal and more</returns>
        public async Task<Linescore> GetLineScoreByIdAsync(int gameId)
        {
            return await _nhlStatsApiHttpClient.GetAsync<Linescore>($"/game/{gameId}/linescore");
        }

        /// <summary>
        /// Returns the live game feed content for an NHL game
        /// </summary>
        /// <param name="gameId">The live game feed id, Example: 2021020087</param>
        /// <param name="liveGameFeedConfiguration">The NHL live game feed event configuration settings for NHL live game feed updates</param>
        /// <returns>A detailed collection of information about play by play details, scores, teams, coaches, on ice statistics, real-time updates and more</returns>
        public async Task<LiveGameFeedResult> GetLiveGameFeedByIdAsync(int gameId, LiveGameFeedConfiguration liveGameFeedConfiguration = null)
        {
            var liveGameFeed = await _nhlStatsApiHttpClient.GetAsync<LiveGameFeed>($"/game/{gameId}/feed/live");

            _nhlGameService.SetCorrectedRinkSideLiveGameFeed(liveGameFeed);
            await _nhlGameService.SetActivePlayersOnIceForAllPlaysAsync(liveGameFeed);

            if (liveGameFeedConfiguration == null)
            {
                liveGameFeedConfiguration = new LiveGameFeedConfiguration();
            }

            var liveGameFeedContent = liveGameFeedConfiguration.IncludeContent ? await GetLiveGameFeedContentByIdAsync(gameId) : null;

            return new LiveGameFeedResult
            {
                LiveGameFeedContent = liveGameFeedContent,
                Configuration = liveGameFeedConfiguration,
                LiveGameFeed = liveGameFeed
            };
        }

        /// <summary>
        /// Returns a collection of all the different types of playoff tournaments in the NHL 
        /// </summary>
        /// <returns>A collection of tournament types, see <see cref="PlayoffTournamentType"/> for more information</returns>
        public async Task<PlayoffTournamentType> GetPlayoffTournamentTypesAsync()
        {
            return await _nhlStatsApiHttpClient.GetAsync<PlayoffTournamentType>($"/tournaments/playoffs");
        }

        /// <summary>
        /// Returns a collection of all the play types within the duration of an NHL game
        /// </summary>
        /// <returns>A collection of distinct play types, see <see cref="PlayType"/> for more information</returns>
        public async Task<List<PlayType>> GetPlayTypesAsync()
        {
            return await _nhlStatsApiHttpClient.GetAsync<List<PlayType>>($"/playTypes");
        }

        /// <summary>
        /// Returns a collection of all the different types of tournaments in the hockey
        /// </summary>
        /// <returns>A collection of tournament types, see <see cref="TournamentType"/> for more information</returns>
        public async Task<List<TournamentType>> GetTournamentTypesAsync()
        {
            return await _nhlStatsApiHttpClient.GetAsync<List<TournamentType>>($"/tournamentTypes");
        }

        /// <summary>
        /// Returns all of the individual shifts of each NHL player for a specific NHL game id
        /// </summary>
        /// <param name="gameId">The game id, Example: 2021020087</param>
        /// <returns>A collection of all the NHL player game shifts for a specific game, including start and end times, on ice duration and more</returns>
        public async Task<LiveGameFeedPlayerShifts> GetLiveGameFeedPlayerShiftsAsync(int gameId)
        {
            return await _nhlShiftChartHttpClient.GetAsync<LiveGameFeedPlayerShifts>($"?cayenneExp=gameId={gameId}");
        }

        /// <summary>
        /// Returns a collection of NHL live game feed content including highlights, media coverage, images, videos and more
        /// </summary>
        /// <param name="gameId">The game id, Example: 2021020087</param>
        /// <returns>A collection of images, video and information from a specific NHL game</returns>
        public async Task<LiveGameFeedContent> GetLiveGameFeedContentByIdAsync(int gameId)
        {
            return await _nhlStatsApiHttpClient.GetAsync<LiveGameFeedContent>($"/game/{gameId}/content");
        }
    }
}
