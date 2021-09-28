﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nhl.Api.Domain.Models.Division
{
	public class LeagueDivisions
	{
		/// <summary>
		/// A collection of all of the NHL divisions, see <see cref="Division"/> for more information
		/// </summary>
		[JsonProperty("divisions")]
		public List<Division> Divisions { get; set; } = new List<Division>();
	}
}