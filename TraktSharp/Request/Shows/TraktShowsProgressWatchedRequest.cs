﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using TraktSharp.Entities;

namespace TraktSharp.Request.Shows {

	public class TraktShowsProgressCollectionRequest : TraktGetRequest<TraktShowProgress> {

		public TraktShowsProgressCollectionRequest(TraktClient client) : base(client) { }

		protected override string PathTemplate { get { return "shows/{id}/progress/collection"; } }

		protected override OAuthRequirementOptions OAuthRequirement { get { return OAuthRequirementOptions.Required; } }

		protected override bool SupportsPagination { get { return false; } }

		public string Id { get; set; }

		protected override IEnumerable<KeyValuePair<string, string>> GetPathParameters(IEnumerable<KeyValuePair<string, string>> pathParameters) {
			return new Dictionary<string, string> {
				{ "id", Id }
			};
		}

		protected override void ValidateParameters() {
			if (string.IsNullOrEmpty(Id)) {
				throw new ArgumentException("Id not set.");
			}
		}

	}

}