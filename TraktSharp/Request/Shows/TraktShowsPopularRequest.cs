﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using TraktSharp.Entities;

namespace TraktSharp.Request.Shows {

	[Serializable]
	public class TraktShowsPopularRequest : TraktGetRequest<IEnumerable<TraktShow>> {

		public TraktShowsPopularRequest(TraktClient client) : base(client) { }

		protected override string PathTemplate { get { return "shows/popular"; } }

		protected override OAuthRequirementOptions OAuthRequirement { get { return OAuthRequirementOptions.NotRequired; } }

		protected override bool SupportsPagination { get { return true; } }

	}

}