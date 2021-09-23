﻿using Newtonsoft.Json;

namespace Nhl.Api.Models.Team
{
	public class Team
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("link")]
		public string Link { get; set; }

		[JsonProperty("venue")]
		public Nhl.Api.Models.Venue.Venue Venue { get; set; }

		[JsonProperty("abbreviation")]
		public string Abbreviation { get; set; }

		[JsonProperty("teamName")]
		public string TeamName { get; set; }

		[JsonProperty("locationName")]
		public string LocationName { get; set; }

		[JsonProperty("firstYearOfPlay")]
		public string FirstYearOfPlay { get; set; }

		[JsonProperty("division")]
		public Nhl.Api.Models.Division.Division Division { get; set; }

		[JsonProperty("conference")]
		public Nhl.Api.Models.Conference.Conference Conference { get; set; }

		[JsonProperty("franchise")]
		public Nhl.Api.Models.Franchise.Franchise Franchise { get; set; }

		[JsonProperty("shortName")]
		public string ShortName { get; set; }

		[JsonProperty("officialSiteUrl")]
		public string OfficialSiteUrl { get; set; }

		[JsonProperty("franchiseId")]
		public int FranchiseId { get; set; }

		[JsonProperty("active")]
		public bool Active { get; set; }
	}
}