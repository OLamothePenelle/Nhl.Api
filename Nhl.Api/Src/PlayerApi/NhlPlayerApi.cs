﻿using Nhl.Api.Common.Http;
using Nhl.Api.Common.Services;
using Nhl.Api.Enumerations.Game;
using Nhl.Api.Models.Enumerations.Player;
using Nhl.Api.Models.Game;
using Nhl.Api.Models.Player;
using Nhl.Api.Models.Season;
using System.Linq;
using System.Threading;

namespace Nhl.Api;

/// <summary>
/// The official unofficial NHL Player API providing various NHL information about players, draft prospects, rosters and more
/// </summary>
public class NhlPlayerApi : INhlPlayerApi
{
    private static readonly INhlApiHttpClient _nhlStatsApiHttpClient = new NhlStatsApiHttpClient();
    private static readonly INhlApiHttpClient _nhlEWebApiHttpClient = new NhlEApiHttpClient();
    private static readonly INhlApiHttpClient _nhlApiWebHttpClient = new NhlApiWebHttpClient();
    private static readonly INhlApiHttpClient _nhlSuggestionApiHttpClient = new NhlSuggestionApiHttpClient();
    private static readonly INhlApiHttpClient _nhlCmsHttpClient = new NhlCmsHttpClient();
    private static readonly ICachingService _cachingService = new CachingService();

    /// <summary>
    /// The official unofficial NHL Player API providing various NHL information about players, draft prospects, rosters and more
    /// </summary>
    public NhlPlayerApi()
    {

    }

    /// <summary>
    /// Returns any active or inactive NHL players based on the search query provided
    /// </summary>
    /// <param name="query">A search term to find NHL players, Example: "Jack Adams" or "Wayne Gretzky" or "Mats Sundin" </param>
    /// <param name="limit">A parameter to limit the number of search results returned when searching for a player</param>
    /// <param name="cancellationToken"> A cancellation token that can be used by other objects or threads to receive notice of cancellation</param>
    /// <returns>A collection of all NHL players based on the search query provided</returns>
    public async Task<List<PlayerSearchResult>> SearchAllPlayersAsync(string query, int limit = 25, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return new List<PlayerSearchResult>();
        }

        return await _nhlSuggestionApiHttpClient.GetAsync<List<PlayerSearchResult>>($"/search/player?culture=en-us&q={query}&limit={limit}", cancellationToken);
    }

    /// <summary>
    /// Returns only active NHL players based on the search query provided
    /// </summary>
    /// <param name="query">A search term to find NHL players, Example: "Owen Power" or "Carter Hart" or "Nathan MacKinnon" </param>
    /// <param name="limit">A parameter to limit the number of search results returned when searching for a player</param>
    /// <param name="cancellationToken"> A cancellation token that can be used by other objects or threads to receive notice of cancellation</param>
    /// <returns>A collection of all NHL players based on the search query provided</returns>
    public async Task<List<PlayerSearchResult>> SearchAllActivePlayersAsync(string query, int limit = 25, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return new List<PlayerSearchResult>();
        }

        return await _nhlSuggestionApiHttpClient.GetAsync<List<PlayerSearchResult>>($"/search/player?culture=en-us&q={query}&active=true&limit={limit}", cancellationToken);
    }

    /// <summary>
    /// Returns the NHL player's head shot image by the selected size
    /// </summary>
    /// <param name="player">An NHL player id, Example: 8478402 - Connor McDavid, see <see cref="PlayerEnum"/> for more information on NHL players</param>
    /// <param name="playerHeadshotImageSize">The size of the head shot image, see <see cref="PlayerHeadshotImageSize"/> for more information </param>
    /// <param name="cancellationToken"> A cancellation token that can be used by other objects or threads to receive notice of cancellation</param>
    /// <returns>A URI endpoint with the image of an NHL player head shot image</returns>
    public async Task<byte[]> GetPlayerHeadshotImageAsync(PlayerEnum player, PlayerHeadshotImageSize playerHeadshotImageSize = PlayerHeadshotImageSize.Small, CancellationToken cancellationToken = default)
    {
        return playerHeadshotImageSize switch
        {
            PlayerHeadshotImageSize.Small => await _nhlCmsHttpClient.GetByteArrayAsync($"images/headshots/current/168x168/{(int)player}.png", cancellationToken),
            PlayerHeadshotImageSize.Medium => await _nhlCmsHttpClient.GetByteArrayAsync($"images/headshots/current/168x168/{(int)player}@2x.png", cancellationToken),
            PlayerHeadshotImageSize.Large => await _nhlCmsHttpClient.GetByteArrayAsync($"images/headshots/current/168x168/{(int)player}@3x.png", cancellationToken),
            _ => null,
        };
    }

    /// <summary>
    /// Returns the NHL player's head shot image by the selected size
    /// </summary>
    /// <param name="playerId">An NHL player id, Example: 8478402 - Connor McDavid</param>
    /// <param name="playerHeadshotImageSize">The size of the head shot image, see <see cref="PlayerHeadshotImageSize"/> for more information </param>
    /// <param name="cancellationToken"> A cancellation token that can be used by other objects or threads to receive notice of cancellation</param>
    /// <returns>A URI endpoint with the image of an NHL player head shot image</returns>
    public async Task<byte[]> GetPlayerHeadshotImageAsync(int playerId, PlayerHeadshotImageSize playerHeadshotImageSize = PlayerHeadshotImageSize.Small, CancellationToken cancellationToken = default)
    {
        return playerHeadshotImageSize switch
        {
            PlayerHeadshotImageSize.Small => await _nhlCmsHttpClient.GetByteArrayAsync($"images/headshots/current/168x168/{playerId}.png", cancellationToken),
            PlayerHeadshotImageSize.Medium => await _nhlCmsHttpClient.GetByteArrayAsync($"images/headshots/current/168x168/{playerId}@2x.png", cancellationToken),
            PlayerHeadshotImageSize.Large => await _nhlCmsHttpClient.GetByteArrayAsync($"images/headshots/current/168x168/{playerId}@3x.png", cancellationToken),
            _ => null,
        };
    }

    /// <summary>
    /// The player season game log for an NHL player for a specific season and game type including stats such as goals, assists, points, plus/minus and more
    /// </summary>
    /// <param name="player">An NHL player id, Example: 8478402 - Connor McDavid, see <see cref="PlayerEnum"/> for more information on NHL players</param>
    /// <param name="seasonYear">The season year parameter for determining the season for the season, <see cref="SeasonYear"/> for all available seasons</param>
    /// <param name="gameType">The game type parameter for determining the game type for the type of player season logs</param>
    /// <param name="cancellationToken"> A cancellation token that can be used by other objects or threads to receive notice of cancellation</param>
    /// <returns>The collection of player season game logs with each game played including statistics, all available season and more</returns>
    public async Task<PlayerSeasonGameLog> GetPlayerSeasonGameLogsBySeasonAndGameTypeAsync(PlayerEnum player, string seasonYear, GameType gameType, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(seasonYear))
        {
            throw new ArgumentException($"The {nameof(seasonYear)} parameter is required", nameof(seasonYear));
        }

        if (seasonYear.Length != 8)
        {
            throw new ArgumentException($"The {nameof(seasonYear)} parameter must be in the format of yyyyyyyy, example: 20232024", nameof(seasonYear));
        }

        return await _nhlApiWebHttpClient.GetAsync<PlayerSeasonGameLog>($"/player/{(int)player}/game-log/{seasonYear}/{(int)gameType}", cancellationToken);
    }

    /// <summary>
    /// The player season game log for an NHL player for a specific season and game type including stats such as goals, assists, points, plus/minus and more
    /// </summary>
    /// <param name="playerId">An NHL player id, Example: 8478402 - Connor McDavid</param>
    /// <param name="seasonYear">The season year parameter for determining the season for the season, <see cref="SeasonYear"/> for all available seasons</param>
    /// <param name="gameType">The game type parameter for determining the game type for the type of player season logs</param>
    /// <param name="cancellationToken"> A cancellation token that can be used by other objects or threads to receive notice of cancellation</param>
    /// <returns>The collection of player season game logs with each game played including statistics, all available season and more</returns>
    public async Task<PlayerSeasonGameLog> GetPlayerSeasonGameLogsBySeasonAndGameTypeAsync(int playerId, string seasonYear, GameType gameType, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(seasonYear))
        {
            throw new ArgumentException($"The {nameof(seasonYear)} parameter is required", nameof(seasonYear));
        }

        if (seasonYear.Length != 8)
        {
            throw new ArgumentException($"The {nameof(seasonYear)} parameter must be in the format of yyyyyyyy, example: 20232024", nameof(seasonYear));
        }

        return await _nhlApiWebHttpClient.GetAsync<PlayerSeasonGameLog>($"/player/{playerId}/game-log/{seasonYear}/{(int)gameType}", cancellationToken);
    }

    /// <summary>
    /// The player season game log for an NHL player for a specific season and game type including stats such as goals, assists, points, plus/minus and more
    /// </summary>
    /// <param name="player">An NHL player id, Example: 8478402 - Connor McDavid, see <see cref="PlayerEnum"/> for more information on NHL players</param>
    /// <param name="seasonYear">The season year parameter for determining the season for the season, <see cref="SeasonYear"/> for all available seasons</param>
    /// <param name="gameType">The game type parameter for determining the game type for the type of player season logs</param>
    /// <param name="cancellationToken"> A cancellation token that can be used by other objects or threads to receive notice of cancellation</param>
    /// <returns>The collection of player season game logs with each game played including statistics, all available season and more</returns>
    public async Task<GoalieSeasonGameLog> GetGoalieSeasonGameLogsBySeasonAndGameTypeAsync(PlayerEnum player, string seasonYear, GameType gameType, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(seasonYear))
        {
            throw new ArgumentException($"The {nameof(seasonYear)} parameter is required", nameof(seasonYear));
        }

        if (seasonYear.Length != 8)
        {
            throw new ArgumentException($"The {nameof(seasonYear)} parameter must be in the format of yyyyyyyy, example: 20232024", nameof(seasonYear));
        }

        return await _nhlApiWebHttpClient.GetAsync<GoalieSeasonGameLog>($"/player/{(int)player}/game-log/{seasonYear}/{(int)gameType}", cancellationToken);
    }

    /// <summary>
    /// The player season game log for an NHL player for a specific season and game type including stats such as goals, assists, points, plus/minus and more
    /// </summary>
    /// <param name="playerId">An NHL player id, Example: 8478402 - Connor McDavid</param>
    /// <param name="seasonYear">The season year parameter for determining the season for the season, <see cref="SeasonYear"/> for all available seasons</param>
    /// <param name="gameType">The game type parameter for determining the game type for the type of player season logs</param>
    /// <param name="cancellationToken"> A cancellation token that can be used by other objects or threads to receive notice of cancellation</param>
    /// <returns>The collection of player season game logs with each game played including statistics, all available season and more</returns>
    public async Task<GoalieSeasonGameLog> GetGoalieSeasonGameLogsBySeasonAndGameTypeAsync(int playerId, string seasonYear, GameType gameType, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(seasonYear))
        {
            throw new ArgumentException($"The {nameof(seasonYear)} parameter is required", nameof(seasonYear));
        }

        if (seasonYear.Length != 8)
        {
            throw new ArgumentException($"The {nameof(seasonYear)} parameter must be in the format of yyyyyyyy, example: 20232024", nameof(seasonYear));
        }

        return await _nhlApiWebHttpClient.GetAsync<GoalieSeasonGameLog>($"/player/{playerId}/game-log/{seasonYear}/{(int)gameType}", cancellationToken);
    }

    /// <summary>
    /// Returns the NHL goalie's profile information including their birth date, birth city, height, weight, position and much more
    /// </summary>
    /// <param name="playerId">An NHL player id, Example: 8480313 - Logan Thompson</param>
    /// <param name="cancellationToken"> A cancellation token that can be used by other objects or threads to receive notice of cancellation</param>
    /// <returns>Returns the NHL player's profile information </returns>
    public async Task<GoalieProfile> GetGoalieInformationAsync(int playerId, CancellationToken cancellationToken = default)
    {
        var goalieProfile = await _nhlApiWebHttpClient.GetAsync<GoalieProfile>($"/player/{playerId}/landing", cancellationToken);
        if (goalieProfile.Position != PlayerPositionEnum.G.ToString())
        {
            throw new ArgumentException($"The {nameof(playerId)} parameter must be a goalie, see {nameof(PlayerEnum)} for more information on NHL goalie's", nameof(playerId));
        }

        return goalieProfile;
    }

    /// <summary>
    /// Returns the NHL goalie's profile information including their birth date, birth city, height, weight, position and much more
    /// </summary>
    /// <param name="player">An NHL player id, Example: 8480313 - Logan Thompson, see <see cref="PlayerEnum"/> for more information on NHL players</param>
    /// <param name="cancellationToken"> A cancellation token that can be used by other objects or threads to receive notice of cancellation</param>
    /// <returns>Returns the NHL player's profile information</returns>
    public async Task<GoalieProfile> GetGoalieInformationAsync(PlayerEnum player, CancellationToken cancellationToken = default)
    {
        var goalieProfile = await _nhlApiWebHttpClient.GetAsync<GoalieProfile>($"/player/{(int)player}/landing", cancellationToken);
        if (goalieProfile.Position != PlayerPositionEnum.G.ToString())
        {
            throw new ArgumentException($"The {nameof(player)} parameter must be a goalie, see {nameof(PlayerEnum)} for more information on NHL goalie's", nameof(player));
        }

        return goalieProfile;
    }

    /// <summary>
    /// Returns the NHL player's profile information including their birth date, birth city, height, weight, position and much more
    /// </summary>
    /// <param name="playerId">An NHL player id, Example: 8478402 - Connor McDavid</param>
    /// <param name="cancellationToken"> A cancellation token that can be used by other objects or threads to receive notice of cancellation</param>
    /// <returns>Returns the NHL player's profile information </returns>
    public async Task<PlayerProfile> GetPlayerInformationAsync(int playerId, CancellationToken cancellationToken = default)
    {
        return await _nhlApiWebHttpClient.GetAsync<PlayerProfile>($"/player/{playerId}/landing", cancellationToken);
    }

    /// <summary>
    /// Returns the NHL player's profile information including their birth date, birth city, height, weight, position and much more
    /// </summary>
    /// <param name="player">An NHL player id, Example: 8478402 - Connor McDavid, see <see cref="PlayerEnum"/> for more information on NHL players</param>
    /// <param name="cancellationToken"> A cancellation token that can be used by other objects or threads to receive notice of cancellation</param>
    /// <returns>Returns the NHL player's profile information</returns>
    public async Task<PlayerProfile> GetPlayerInformationAsync(PlayerEnum player, CancellationToken cancellationToken = default)
    {
        return await _nhlApiWebHttpClient.GetAsync<PlayerProfile>($"/player/{(int)player}/landing", cancellationToken);

    }

    /// <summary>
    /// Returns the NHL player's in the spotlight based on their recent performances 
    /// </summary>
    /// <param name="cancellationToken"> A cancellation token that can be used by other objects or threads to receive notice of cancellation</param>
    /// <returns>A collection of players and their information for players in the NHL spotlight</returns>
    public async Task<List<PlayerSpotlight>> GetPlayerSpotlightAsync(CancellationToken cancellationToken = default)
    {
        return await _nhlApiWebHttpClient.GetAsync<List<PlayerSpotlight>>("/player-spotlight", cancellationToken);
    }

    /// <summary>
    /// Disposes and releases all unneeded resources for the NHL player API
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public void Dispose() => _cachingService?.Dispose();

    /// <summary>
    /// Returns all the NHL players to ever play in the NHL
    /// </summary>
    /// <param name="cancellationToken"> A cancellation token that can be used by other objects or threads to receive notice of cancellation</param>
    /// <returns>Returns all the NHL players to ever play in the NHL</returns>
    public async Task<List<PlayerDataSearchResult>> GetAllPlayersAsync(CancellationToken cancellationToken = default)
    {
        var startCount = 0;
        var playerSearchResultsTasks = new List<Task<PlayerData>>();

        var response = _nhlEWebApiHttpClient.GetAsync<PlayerData>($"/players?start={startCount}", cancellationToken).Result;
        var total = response.Total;

        while (startCount <= total)
        {
            playerSearchResultsTasks.Add(_nhlEWebApiHttpClient.GetAsync<PlayerData>($"/players?start={startCount}", cancellationToken));
            startCount += 5;
        }

        return (await Task.WhenAll(playerSearchResultsTasks)).SelectMany(playerData => playerData.Data).ToList();

    }
}
