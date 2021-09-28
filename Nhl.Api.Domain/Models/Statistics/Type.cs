﻿using Newtonsoft.Json;
using Nhl.Api.Domain.Models.Game;

namespace Nhl.Api.Domain.Models.Statistics
{
	public class Type
	{
		/// <summary>
		/// The display name for the NHL statistic type <br/>
		/// Example: statsSingleSeason
		/// </summary>
		[JsonProperty("displayName")]
		public string DisplayName { get; set; }

		/// <summary>
		/// The NHL game type for the NHL statistics
		/// </summary>
		[JsonProperty("gameType")]
		public GameType GameType { get; set; }
	}
}