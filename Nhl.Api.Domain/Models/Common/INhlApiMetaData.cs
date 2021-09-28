﻿using Newtonsoft.Json;

namespace Nhl.Api.Domain.Models.Common
{
	public interface INhlApiMetaData
	{
		[JsonProperty("id")]
		int Id { get; set; }

		[JsonProperty("name")]
		string Name { get; set; }

		[JsonProperty("link")]
		string Link { get; set; }
	}
}