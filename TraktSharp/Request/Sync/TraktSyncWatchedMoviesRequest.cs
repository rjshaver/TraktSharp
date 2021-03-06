﻿using System;
using System.Collections.Generic;
using System.Linq;
using TraktSharp.Entities.Response;
using TraktSharp.Enums;

namespace TraktSharp.Request.Sync {

	internal class TraktSyncWatchedMoviesRequest : TraktGetRequest<IEnumerable<TraktWatchedMoviesResponseItem>> {

		internal TraktSyncWatchedMoviesRequest(TraktClient client) : base(client) { }

		protected override string PathTemplate { get { return "sync/watched/movies"; } }

		protected override TraktAuthenticationRequirement AuthenticationRequirement { get { return TraktAuthenticationRequirement.Required; } }

	}

}