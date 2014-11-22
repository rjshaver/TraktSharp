﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace TraktSharp.Request {

	public abstract class TraktDeleteByIdRequest : TraktDeleteRequest {

		protected TraktDeleteByIdRequest(TraktClient client) : base(client) { }

		public string Id { get; set; }

		protected override IEnumerable<KeyValuePair<string, string>> GetPathParameters(IEnumerable<KeyValuePair<string, string>> pathParameters) {
			return new Dictionary<string, string> {
				{"id", Id}
			};
		}

		protected override void ValidateParameters() {
			if (string.IsNullOrEmpty(Id)) {
				throw new ArgumentException("Id not set.");
			}
		}

	}

}