﻿using System;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;

namespace TraktSharp.Entities {

	[Serializable]
	public class TraktEpisodeIds {

		[JsonProperty(PropertyName = "trakt")]
		public int? Trakt { get; set; }

		[JsonProperty(PropertyName = "tvdb")]
		public int? Tvdb { get; set; }

		[JsonProperty(PropertyName = "imdb")]
		public string Imdb { get; set; }

		[JsonProperty(PropertyName = "tmdb")]
		public int? Tmdb { get; set; }

		[JsonProperty(PropertyName = "tvrage")]
		public int? TvRage { get; set; }

		public bool HasAnyValuesSet() {
			return Trakt.HasValue || Tvdb.HasValue || !string.IsNullOrEmpty(Imdb) || Tmdb.HasValue || TvRage.HasValue;
		}

		public string GetBestId() {
			if (Trakt.GetValueOrDefault() > 0) { return Trakt.GetValueOrDefault().ToString(CultureInfo.InvariantCulture); }
			if (!string.IsNullOrEmpty(Imdb)) { return Imdb; }
			if (Tvdb.GetValueOrDefault() > 0) { return Tvdb.GetValueOrDefault().ToString(CultureInfo.InvariantCulture); }
			if (Tmdb.GetValueOrDefault() > 0) { return Tmdb.GetValueOrDefault().ToString(CultureInfo.InvariantCulture); }
			if (TvRage.GetValueOrDefault() > 0) { return TvRage.GetValueOrDefault().ToString(CultureInfo.InvariantCulture); }
			return string.Empty;
		}

	}

}