﻿using System;
using System.Linq;
using TraktSharp.Entities.RequestBody.OAuth;
using TraktSharp.Entities.Response.OAuth;

namespace TraktSharp.Request.OAuth {

	public class TraktOAuthTokenRequest : TraktPostRequest<TraktOAuthTokenResponse, TraktOAuthTokenRequestBody> {

		public TraktOAuthTokenRequest(TraktClient client) : base(client) { }

		protected override string PathTemplate { get { return "oauth/token"; } }

		protected override void ValidateParameters() {
			base.ValidateParameters();
			if (string.IsNullOrEmpty(RequestBody.Code)) {
				throw new ArgumentException("AuthorizationCode not set. This is usually set by calling ParseAuthorizationResponse().");
			}
			if (string.IsNullOrEmpty(RequestBody.ClientId)) {
				throw new ArgumentException("ClientId not set.");
			}
			if (string.IsNullOrEmpty(RequestBody.ClientSecret)) {
				throw new ArgumentException("ClientSecret not set.");
			}
			if (string.IsNullOrEmpty(RequestBody.RedirectUri)) {
				throw new ArgumentException("RedirectUri not set.");
			}
		}

	}

}