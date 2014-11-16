﻿using System;
using System.Linq;
using Newtonsoft.Json;

namespace TraktSharp.Response {

	public class TraktShowImages {

		public TraktShowImages() {
			Banner = new TraktImage();
			ClearArt = new TraktImage();
			FanArt = new TraktImageSet();
			Logo = new TraktImage();
			Poster = new TraktImageSet();
			Thumb = new TraktImage();
		}

		[JsonProperty(PropertyName = "banner")]
		public TraktImage Banner { get; set; }

		[JsonProperty(PropertyName = "clearart")]
		public TraktImage ClearArt { get; set; }

		[JsonProperty(PropertyName = "fanart")]
		public TraktImageSet FanArt { get; set; }

		[JsonProperty(PropertyName = "logo")]
		public TraktImage Logo { get; set; }

		[JsonProperty(PropertyName = "poster")]
		public TraktImageSet Poster { get; set; }

		[JsonProperty(PropertyName = "thumb")]
		public TraktImage Thumb { get; set; }

	}

}