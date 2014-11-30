﻿using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TraktSharp.Entities;
using TraktSharp.Examples.Helpers;
using TraktSharp.Examples.Views;
using TraktSharp.Exceptions;
using TraktSharp.Helpers;
using TraktSharp.Serialization;

namespace TraktSharp.Examples.ViewModels {

	public class MainViewModel : ViewModelBase {

		private MainView _view;

		public MainViewModel() { }

		public MainViewModel(MainView view) {

			_view = view;

			Client = new TraktClient();
			TestRequestTypes = new ObservableCollection<string>(EnumsHelper.GetEnumInfo(typeof(TestRequests.TestRequestType)).Select(v => v.Value.Description));
			TryLoadState();
			PropertyChanged += (sender, e) => TrySaveState();

			Client.BeforeRequest += (sender, e) => {

				LastResponse = "Waiting...";
				NotifyPropertyChanged(this.GetMemberName(x => x.LastResponse));

				LastResponseJson = "Waiting...";
				NotifyPropertyChanged(this.GetMemberName(x => x.LastResponseJson));

				LastReturnedValue = "Waiting...";
				NotifyPropertyChanged(this.GetMemberName(x => x.LastReturnedValue));

				var sb = new StringBuilder();
				sb.AppendLine(string.Format("{0} {1} HTTP/{2}", e.Request.Method.ToString().ToUpper(), e.Request.RequestUri.AbsoluteUri, e.Request.Version));
				e.Request.Headers.Select(h => string.Format("{0}: {1}", h.Key, string.Join(",", h.Value))).ToList().ForEach(l => sb.AppendLine(l));
				sb.AppendLine();
				sb.AppendLine(e.RequestBody);
				LastRequest = sb.ToString();
				NotifyPropertyChanged(this.GetMemberName(x => x.LastRequest));

			};

			Client.AfterRequest += (sender, e) => {

				var sb = new StringBuilder();
				sb.AppendLine(string.Format("HTTP/{0} {1}",
					e.Response.Version,
					e.Response.Headers.First(h => h.Key.Equals("Status", StringComparison.InvariantCultureIgnoreCase)).Value.First().ToUpper()));
				e.Response.Headers.Select(h => string.Format("{0}: {1}", h.Key, string.Join(",", h.Value))).ToList().ForEach(l => sb.AppendLine(l));
				sb.AppendLine();
				sb.AppendLine(e.ResponseText);
				LastResponse = sb.ToString();
				NotifyPropertyChanged(this.GetMemberName(x => x.LastResponse));

				if (!string.IsNullOrEmpty(e.ResponseText)) {
					LastResponseJson = PrettyPrint(e.ResponseText);
				} else {
					LastResponseJson = "The response did not include a body";
				}
				NotifyPropertyChanged(this.GetMemberName(x => x.LastResponseJson));

			};

		}

		public TraktClient Client { get; private set; }

		public string Username {
			get { return Client.Authentication.Username; }
			set {
				Client.Authentication.Username = value;
				NotifyPropertyChanged();
			}
		}

		public string ClientId {
			get { return Client.Authentication.ClientId; }
			set {
				Client.Authentication.ClientId = value;
				NotifyPropertyChanged();
				NotifyPropertyChanged(this.GetMemberName(x => x.CanAuthorize));
			}
		}

		public string ClientSecret {
			get { return Client.Authentication.ClientSecret; }
			set {
				Client.Authentication.ClientSecret = value;
				NotifyPropertyChanged();
				NotifyPropertyChanged(this.GetMemberName(x => x.CanAuthorize));
			}
		}

		public string AuthorizationCode { get { return Client.Authentication.AuthorizationCode; } }

		public string LastRequest { get; private set; }

		public string LastResponse { get; private set; }

		public string LastResponseJson { get; private set; }

		public string LastReturnedValue { get; private set; }

		public ObservableCollection<string> TestRequestTypes { get; set; }

		private TestRequests.TestRequestType _selectedTestRequestType;
		public string SelectedTestRequestType {
			get { return EnumsHelper.GetDescription(_selectedTestRequestType); }
			set {
				_selectedTestRequestType = EnumsHelper.FromDescription<TestRequests.TestRequestType>(value);
				NotifyPropertyChanged();
			}
		}

		private int _selectedResponseTab;
		public int SelectedResponseTab {
			get { return _selectedResponseTab; }
			set {
				_selectedResponseTab = value;
				NotifyPropertyChanged();
			}
		}

		private int _selectedMainTab;
		public int SelectedMainTab {
			get { return _selectedMainTab; }
			set {
				_selectedMainTab = value;
				NotifyPropertyChanged();
			}
		}

		public TraktAccessToken AccessToken {
			get { return Client.Authentication.CurrentAccessToken; }
			set {
				Client.Authentication.CurrentAccessToken = value;
				NotifyPropertyChanged();
			}
		}

		public async void Authorize() {
			var authorizeViewModel = new AuthorizeViewModel(Client);
			var window = new AuthorizeView(authorizeViewModel);
			window.ShowDialog();
			NotifyPropertyChanged(this.GetMemberName(x => x.AuthorizationCode));
			AccessToken = await Client.Authentication.GetAccessToken();
		}

		public object CanAuthorize {
			get { return !string.IsNullOrEmpty(ClientId) && !string.IsNullOrEmpty(ClientSecret); }
		}

		public async void TestRequest() {
			object result;
			try {
				result = await TestRequests.ExecuteTestRequest(Client, _selectedTestRequestType);
			} catch (Exception ex) {
				result = ex;
			}
			var sb = new StringBuilder();
			if (result != null) {
				sb.AppendLine(result.GetType().ToString());
				sb.AppendLine();
				sb.AppendLine(PrettyPrint(result));
			} else {
				sb.AppendLine("This method does not have a return value");
			}
			LastReturnedValue = sb.ToString();
			NotifyPropertyChanged(this.GetMemberName(x => x.LastReturnedValue));
		}

		public void Closing() {
			TrySaveState();
		}

		public static string StateSerializationPath {
			get {
				var assemblyPath = Assembly.GetExecutingAssembly().Location;
				return Path.Combine(Path.GetDirectoryName(assemblyPath), string.Format("{0}.state", Path.GetFileNameWithoutExtension(assemblyPath)));
			}
		}

		public void TryLoadState() {
			try {
				var result = JsonConvert.DeserializeObject<SavedState>(File.ReadAllText(StateSerializationPath));
				if (result.WindowWidth > 0 && result.WindowHeight > 0) {
					if (result.WindowHeight > _view.MinHeight) { _view.Height = result.WindowHeight; }
					if (result.WindowWidth > _view.MinWidth) { _view.Width = result.WindowWidth; }
					_view.Left = result.WindowLeft;
					_view.Top = result.WindowTop;
					_view.WindowState = result.WindowState;
				}
				AccessToken = result.AccessToken;
				Username = result.Username;
				ClientId = result.ClientId;
				ClientSecret = result.ClientSecret;
				SelectedMainTab = result.SelectedMainTab;
				SelectedResponseTab = result.SelectedResponseTab;
				SelectedTestRequestType = result.SelectedTestRequestType;
			} catch { }
		}

		public void TrySaveState() {
			try {
				FileSystemHelper.EnsureParentDirectoryExists(StateSerializationPath);
				File.WriteAllText(StateSerializationPath, JsonConvert.SerializeObject(new SavedState {
					WindowHeight = _view.Height,
					WindowWidth = _view.Width,
					WindowLeft = _view.Left,
					WindowTop = _view.Top,
					WindowState = _view.WindowState,
					AccessToken = AccessToken,
					Username = Username,
					ClientId = ClientId,
					ClientSecret = ClientSecret,
					SelectedMainTab = SelectedMainTab,
					SelectedResponseTab = SelectedResponseTab,
					SelectedTestRequestType = SelectedTestRequestType
				}, Formatting.Indented), Encoding.UTF8);
			} catch { }
		}

		private string PrettyPrint(string json) {
			try { return PrettyPrint(JArray.Parse(json)); } catch { }
			try { return PrettyPrint(JObject.Parse(json)); } catch { }
			return string.Empty;
		}

		private string PrettyPrint(object obj) {
			try {
				var ex = obj as Exception;
				if (ex != null) {
					var sb = new StringBuilder();
					sb.AppendLine(string.Format("Message: {0}", ex.Message));
					sb.AppendLine(string.Format("Source: {0}", ex.Source));
					var traktEx = ex as TraktException;
					if (traktEx != null) {
						sb.AppendLine(string.Format("Trakt Error Type: {0}", traktEx.TraktErrorType));
						var traktConflictEx = ex as TraktConflictException;
						if (traktConflictEx != null) {
							sb.AppendLine(string.Format("Expires At: {0}", traktConflictEx.ExpiresAt));
						}
					}
					sb.AppendLine();
					sb.AppendLine(string.Format("Stack Trace:\r\n\r\n{0}", ex.StackTrace));
					if (ex.InnerException != null) {
						sb.AppendLine(string.Format("Inner Exception Message: {0}", ex.InnerException.Message));
						sb.AppendLine(string.Format("Inner Exception Source: {0}", ex.InnerException.Source));
						sb.AppendLine();
						sb.AppendLine(string.Format("Inner Exception Stack Trace:\r\n\r\n{0}", ex.InnerException.StackTrace));
					}
					return sb.ToString();
				}
				return JsonConvert.SerializeObject(obj, new JsonSerializerSettings {
					Formatting = Formatting.Indented,
					ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
					ContractResolver = new OriginalPropertyNamesContractResolver() { IgnoreSerializableInterface = true, IgnoreSerializableAttribute = true }
				});
			} catch { }
			return string.Empty;
		}

	}

}