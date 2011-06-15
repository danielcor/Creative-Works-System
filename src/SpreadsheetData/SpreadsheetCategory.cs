using System;
using System.Collections.Generic;
using System.Linq;
using Google.GData.Client;
using Google.GData.Spreadsheets;

namespace SpreadsheetData
{
	public class InitCategory
	{
		public Guid Id { get; set; }
		public String Name { get; set; }

		public IEnumerable<SpreadsheetTag> Tags { get; set; }
		public IEnumerable<SpreadsheetResource> Resources { get; set; }

		public static InitCategory SelectCategory
		{
			get
			{
				return new InitCategory { Name = "Click to select one or more", Id = Guid.Empty };
			}
		}

		// Google spreadsheet variables
		// TODO: These need to come from a config file that's not put into open source!  :P
		private static readonly string[] LoginCredentials = new[] { @"", @"" };

		private static Dictionary<Guid, InitCategory> _idDictionary;

		public static Dictionary<Guid, InitCategory> IdDictionary
		{
			get { return _idDictionary ?? (_idDictionary = AllCategories().ToDictionary(cat => cat.Id)); }
		}

		private static Dictionary<string, InitCategory> _nameDictionary;

		public static Dictionary<string, InitCategory> NameDictionary
		{
			get { return _nameDictionary ?? (_nameDictionary = AllCategories().ToDictionary(cat => cat.Name)); }
		}

		private static InitCategory[] _allCategories;

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "CategoriesTagsMaterials"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "SpreadsheetQuery")]
		public static IEnumerable<InitCategory> AllCategories()
		{
			try
			{
				if (_allCategories == null)
				{
					// These are all of the steps used in the spreadsheet sample app, just condensed.
					var ssService = new SpreadsheetsService("MueVue Category Management App");
					ssService.setUserCredentials(LoginCredentials[0], LoginCredentials[1]);

					// Find the right worksheet...
					var entries = ssService.Query(new SpreadsheetQuery()).Entries;
					if (entries == null)
					{
						throw new InvalidOperationException("Query(new SpreadsheetQuery()) failed.");
					}
					var ssAtomEntry =
						entries.FirstOrDefault(
							e =>
							e.Title.Text.Equals("CategoriesTagsMaterials", StringComparison.InvariantCultureIgnoreCase));
					if (ssAtomEntry == null)
					{
						throw new InvalidOperationException("Failed to retrieve spreadsheet CategoriesTagsMaterials.");
					}
					string categoriesWorksheetUrl =
						ssAtomEntry.Links.FindService(GDataSpreadsheetsNameTable.WorksheetRel,
													  AtomLink.ATOM_TYPE).HRef.Content;
					var workSheetFeed = ssService.Query(new WorksheetQuery(categoriesWorksheetUrl));
					var categoriesWorksheet =
						workSheetFeed.Entries.FirstOrDefault(
							ae => ae.Title.Text.Equals("Categories", StringComparison.InvariantCultureIgnoreCase));
					if (categoriesWorksheet == null)
					{
						throw new InvalidOperationException("Failed to retrieve worksheet Categories.");
					}
					// Then get the cells.
					string categoriesCellFeedUrl =
						categoriesWorksheet.Links.FindService(GDataSpreadsheetsNameTable.CellRel,
															  AtomLink.ATOM_TYPE).HRef.Content;

					var catFeed = ssService.Query(new CellQuery(categoriesCellFeedUrl));
					if (catFeed == null)
					{
						throw new InvalidOperationException("Failed to retrieve cells.");
					}

					var catsAndGuids = new List<string[]>();


					// TODO: make the indices flexible
					for (uint i = 2; i <= 24; i++)
					{
						CellEntry cat = catFeed[i, 1];
						CellEntry guid = catFeed[i, 2];
						catsAndGuids.Add(new[] { cat.Value, guid.Value });
					}

					Console.WriteLine(catsAndGuids.Count);

					_allCategories = catsAndGuids.Select(
							cat =>
							new InitCategory { Id = Guid.Parse(cat[1]), Name = cat[0], Tags = null, Resources = null }).ToArray();
				}
				return _allCategories;
			}
			catch (Exception e)
			{
				Console.WriteLine("Worksheet query failed. \n" + e.Message);
				return null;
			}
		}
	}

	public class SpreadsheetTag
	{
		public Guid Id { get; set; }
		public String Name { get; set; }

		public IEnumerable<InitCategory> Categories { get; set; }
	}

	public class SpreadsheetResource
	{
		public Guid Id { get; set; }
		public String Name { get; set; }
		public InitCategory Category { get; set; }
	}
}
