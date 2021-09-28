using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nhl.Api.Domain.Models.Draft;
using System.Linq;
using System.Threading.Tasks;

namespace Nhl.Api.Tests
{
	[TestClass]
	public class DraftTests
	{

		[TestMethod]
		public async Task TestGetDraftByYear()
		{
			// Arrange
			INhlApi nhlApi = new NhlApi();

			// Act
			var leagueStandingTypes = await nhlApi.GetDraftByYear(DraftYear.draftYear2010);

			// Assert
			Assert.IsNotNull(leagueStandingTypes);
			CollectionAssert.AllItemsAreNotNull(leagueStandingTypes.Drafts);

			foreach (var draft in leagueStandingTypes.Drafts)
			{
				Assert.IsNotNull(draft.DraftYear);

				foreach (var round in draft.Rounds)
				{
					Assert.IsNotNull(round.DraftRound);
					Assert.IsNotNull(round.Picks);
					Assert.IsNotNull(round.RoundNumber);

					foreach (var pick in round.Picks)
					{
						Assert.IsNotNull(pick.PickInRound);
						Assert.IsNotNull(pick.PickOverall);
						Assert.IsNotNull(pick.Prospect);
						Assert.IsNotNull(pick.Prospect.Id);
						Assert.IsNotNull(pick.Prospect.Link);
						Assert.IsNotNull(pick.Team);
						Assert.IsNotNull(pick.Year);
					}
				}
			}
		}

		[TestMethod]
		public async Task TestGetProspectsAsnyc()
		{
			// Arrange
			INhlApi nhlApi = new NhlApi();

			// Act
			var leagueProspects = await nhlApi.GetLeagueProspectsAsync();

			// Assert
			Assert.IsNotNull(leagueProspects);
			CollectionAssert.AllItemsAreNotNull(leagueProspects);

			foreach (var prospect in leagueProspects.Take(10))
			{
				Assert.IsNotNull(prospect.BirthCountry);
				Assert.IsNotNull(prospect.DraftStatus);
				Assert.IsNotNull(prospect.ShootsCatches);
				Assert.IsNotNull(prospect.Weight);
				Assert.IsNotNull(prospect.FullName);
				Assert.IsNotNull(prospect.Height);
				Assert.IsNotNull(prospect.Id);
				Assert.IsNotNull(prospect.AmateurLeague);
				Assert.IsNotNull(prospect.AmateurTeam);
				Assert.IsNotNull(prospect.BirthCity);
				Assert.IsNotNull(prospect.FullName);
				Assert.IsNotNull(prospect.FirstName);
				Assert.IsNotNull(prospect.LastName);
			}
		}


		[TestMethod]
		public async Task TestGetProspectsByIdAsnyc()
		{
			// Arrange
			INhlApi nhlApi = new NhlApi();

			// Act
			var leagueProspectId = (await nhlApi.GetLeagueProspectsAsync()).First().Id;
			var prospect = await nhlApi.GetLeagueProspectByIdAsync(leagueProspectId);

			// Assert
			Assert.IsNotNull(prospect.BirthCountry);
			Assert.IsNotNull(prospect.DraftStatus);
			Assert.IsNotNull(prospect.ShootsCatches);
			Assert.IsNotNull(prospect.Weight);
			Assert.IsNotNull(prospect.FullName);
			Assert.IsNotNull(prospect.Height);
			Assert.IsNotNull(prospect.Id);
			Assert.IsNotNull(prospect.AmateurLeague);
			Assert.IsNotNull(prospect.AmateurTeam);
			Assert.IsNotNull(prospect.BirthCity);
			Assert.IsNotNull(prospect.FullName);
			Assert.IsNotNull(prospect.FirstName);
			Assert.IsNotNull(prospect.LastName);
		}
	}
}