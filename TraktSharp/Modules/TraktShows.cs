﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TraktSharp.Request.Shows;
using TraktSharp.Response;
using TraktSharp.Response.Shows;

namespace TraktSharp.Modules {

	public class TraktShows {

		public TraktShows(TraktClient client) {
			Client = client;
		}

		public TraktClient Client { get; private set; }

		public async Task<IEnumerable<TraktShow>> PopularAsync(ExtendedOptions extended = ExtendedOptions.Unspecified, int? page = null, int? limit = null) {
			return await new TraktShowsPopularRequest(Client) {
				Extended = extended,
				Pagination = new PaginationOptions(page, limit)
			}.SendAsync();
		}

		public async Task<IEnumerable<TraktShowsTrendingResponseItem>> TrendingAsync(ExtendedOptions extended = ExtendedOptions.Unspecified, int? page = null, int? limit = null) {
			return await new TraktShowsTrendingRequest(Client) {
				Extended = extended,
				Pagination = new PaginationOptions(page, limit)
			}.SendAsync();
		}

		public async Task<IEnumerable<TraktShowsUpdatesResponseItem>> UpdatesAsync(DateTime? startDate = null, ExtendedOptions extended = ExtendedOptions.Unspecified, int? page = null, int? limit = null) {
			return await new TraktShowsUpdatesRequest(Client) {
				StartDate = startDate,
				Extended = extended,
				Pagination = new PaginationOptions(page, limit)
			}.SendAsync();
		}

		public async Task<TraktShow> SummaryAsync(string id, ExtendedOptions extended = ExtendedOptions.Unspecified) {
			return await new TraktShowsSummaryRequest(Client) {
				Id = id,
				Extended = extended
			}.SendAsync();
		}

		public async Task<object> AliasesAsync(string id, ExtendedOptions extended = ExtendedOptions.Unspecified) {
			throw new NotImplementedException("From the Trakt API docs: Currently no data to back this. Will be completed when source data has it available.");
		}

		public async Task<IEnumerable<TraktShowsTranslationsResponseItem>> TranslationsAsync(string id, string language, ExtendedOptions extended = ExtendedOptions.Unspecified) {
			return await new TraktShowsTranslationsRequest(Client) {
				Id = id,
				Language = language,
				Extended = extended
			}.SendAsync();
		}

	}

}