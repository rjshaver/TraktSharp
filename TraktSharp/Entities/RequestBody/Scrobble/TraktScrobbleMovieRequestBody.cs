﻿using System;
using System.Linq;
using Newtonsoft.Json;
using TraktSharp.ExtensionMethods;

namespace TraktSharp.Entities.RequestBody.Scrobble {

	[Serializable]
	public class TraktScrobbleMovieRequestBody {

		[JsonProperty(PropertyName = "movie")]
		public TraktMovie Movie { get; set; }

		[JsonProperty(PropertyName = "progress")]
		public float Progress { get; set; }

		[JsonProperty(PropertyName = "app_version")]
		public string AppVersion { get; set; }

		[JsonProperty(PropertyName = "app_date")]
		public string AppDateString { get { return AppDate.ToTraktApiFormat(); } }

		[JsonIgnore]
		public DateTime? AppDate { get; set; }

	}

}