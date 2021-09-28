﻿using Newtonsoft.Json;

namespace Nhl.Api.Domain.Models.Event
{
	public class EventType
	{
		/// <summary>
		/// The identifier for the NHL event type <br/>
		/// Example: H
		/// </summary>
		[JsonProperty("id")]
		public string Id { get; set; }

		/// <summary>
		/// The description for the NHL event type <br/>
		/// Example: Hockey game
		/// </summary>
		[JsonProperty("description")]
		public string Description { get; set; }
	}
}
