using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Text.Json;
using System.Text;
using System.Net.Http.Json;
using BlazorApp1.Shared;

namespace BlazorApp1
{
    public class UILib
    {
        #region ------------- Types and constants -------------------------------------------------
        public delegate void StateHasChangedHandler();
        public delegate void ActionAfterReloginHandler();
        public const int MILLISECONDS = 1;
		#endregion



        #region ------------- Properties ----------------------------------------------------------
        public HttpClient HttpClientResults => _httpClient;
        public object Controller   { get; set; }
		public object SelectedRow  { get; set; }

		public object Controller2  { get; set; }
		public object SelectedRow2 { get; set; }
		public object SelectedRow3 { get; set; }

		public long   CustomField1 { get; set; }
		public long   CustomField2 { get; set; }


		public string BackUrl { get; set; }
		#endregion



        #region ------------- Fields --------------------------------------------------------------
	    private static IJSRuntime      _jsRuntime;
		private StateHasChangedHandler _stateHasChangedHandler;
		private UserStateProvider      _userState;
		private HttpClient             _httpClient;
        private int                    _selectedNavLink = 1;
        private int                    _navLinkCount = 1;
		#endregion



		#region ------------- Init  ---------------------------------------------------------------
		public UILib()
	    {
            _jsRuntime              = null;
            _stateHasChangedHandler = null;
            _httpClient             = null;
	    }

		public UILib(IJSRuntime             jsRuntime, 
                     StateHasChangedHandler stateHasChangedHandler, 
                     UserStateProvider      userState,
                     HttpClient             httpClient, 
                     string                 defaultLanguage = "")
	    {
            //_i18n = new Internationalization();
            //
            //if (!string.IsNullOrWhiteSpace(defaultLanguage))
            //    _i18n.Language = defaultLanguage;
            //else
            //    if (!string.IsNullOrWhiteSpace(userState.EntryLanguage))
            //        _i18n.Language = userState.EntryLanguage;
            //    else
            //        _i18n.Language = userState?.CurrentUser?.Language;
            //if (string.IsNullOrWhiteSpace(userState.OrderProcess.Language))
            //    userState.OrderProcess.Language = _i18n.Language;
        
            _jsRuntime              = jsRuntime;
            _stateHasChangedHandler = stateHasChangedHandler;
            _userState              = userState;
            _httpClient             = httpClient;
            
            //if (jsRuntime != null)
            //    _manager            = new LoginManager(jsRuntime, 
            //                            delegate() { stateHasChangedHandler(); }, 
            //                            this, httpClient, userState, RedirectUser);
	    }

        private void RedirectUser(string url)
		{
		}
		#endregion



		#region ------------- State ---------------------------------------------------------------
		//public void UpdateState(StateHasChangedHandler stateHasChangedHandler)
	    //{
        //    _stateHasChangedHandler = stateHasChangedHandler;
        //}
        //
		//public void UpdateState(IJSRuntime             jsRuntime, 
        //                        StateHasChangedHandler stateHasChangedHandler, 
        //                        UserStateProvider      userState,
        //                        HttpClient             httpClient, 
        //                        string                 defaultLanguage = "")
	    //{
        //    _jsRuntime              = jsRuntime;
        //    _stateHasChangedHandler = stateHasChangedHandler;
        //    _userState              = userState;
        //    _httpClient             = httpClient;
        //    
        //    if (!string.IsNullOrWhiteSpace(defaultLanguage))
        //        _i18n.Language = defaultLanguage;
        //
        //    if (jsRuntime != null)
        //        _manager            = new LoginManager(jsRuntime, 
        //                                delegate() { stateHasChangedHandler(); }, 
        //                                this, httpClient, userState, RedirectUser);
	    //}
        //
        //internal void StateHasChanged()
        //{
        //    _stateHasChangedHandler();
        //}
		#endregion



		#region ------------- UI Language ---------------------------------------------------------
        //public void SetLanguage(string language)
		//{
        //    _i18n.Language = language;
		//}
		#endregion



		#region ------------- Nav bar -------------------------------------------------------------
        internal void InitNavBar()
        {       
            if (_userState?.CurrentUser?.IsAdmin == true)
                _navLinkCount = 3;
            else
                _navLinkCount = 4;

            #pragma warning disable 4014
            SetFocusNew($"NavLink{_selectedNavLink}");
        }

        internal void OnNavBarKeyDown(KeyboardEventArgs args)
        {
            if (args.Key == "ArrowLeft")
            {
                if (_selectedNavLink > 1) _selectedNavLink--;
                #pragma warning disable 4014
                SetFocusNew($"NavLink{_selectedNavLink}");
            }
            else if (args.Key == "ArrowRight")
            {
                if (_selectedNavLink < _navLinkCount) _selectedNavLink++;
                #pragma warning disable 4014
                SetFocusNew($"NavLink{_selectedNavLink}");
            }
        }
		#endregion



		#region ------------- Relogin -------------------------------------------------------------
        //public async Task ReloginAfterF5(bool force = false)
		//{
        //    if (_userState.CurrentUser == null || force)
        //    {
        //        await _manager.Init();
        //        await _manager.TryLogin(calculateHash:false);
        //    }
		//}
        #endregion



		#region ------------- Delayed action ------------------------------------------------------
        public void WaitAndThenExecute(int wait_ms, StateHasChangedHandler action)
        {
            new Timer(delegate(object state)
            {
                action();
            }
            , 0, wait_ms, Timeout.Infinite);
        }
        #endregion



		#region ------------- Focus ---------------------------------------------------------------
        public void WaitAndThenSetFocusTo(string entryfield, int wait_ms = 200)
        {
            new Timer(async delegate(object state)
            {
                await SetFocusNew(entryfield);
            }
            , 0, wait_ms, Timeout.Infinite);
        }

        public async Task SetFocusNew(string controlId)
        {
            await _jsRuntime.InvokeAsync<object>("MySetFocus", new object[] { controlId });
        }

        public async Task Alert(string message)
        {
            await _jsRuntime.InvokeAsync<string>("Alert", new object[] { message });
        }

        public async Task ScrollIntoView(string element_id)
        {
            try
            {
                await _jsRuntime.InvokeAsync<object>("ScrollIntoView", new object[] { element_id });
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex);
            }
        }

        public async Task ScrollToTop()
        {
            try
            {
                await _jsRuntime.InvokeAsync<object>("ScrollToTop", new object[] {});
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex);
            }
        }
		#endregion



        #region ------------- Checkbox ------------------------------------------------------------
        public async Task<bool> GetCheckboxCheckedState(string controlId)
        {
            var result = await _jsRuntime.InvokeAsync<int>("GetCheckboxCheckedState", new object[] { controlId });
            return result == 1;
        }

        public async Task CheckboxSetChecked(string controlId, bool check)
		{
            await _jsRuntime.InvokeAsync<int>("SetCheckboxCheckedState", new object[] { controlId, check });
		}
		#endregion



        #region ------------- Combobox / Dropdown lists -------------------------------------------
        //public static string DropdownGenerateCountryValues(List<Country> countries, string selectedOption, string pleaseSelectPhrase = "Please Select")
        //{
        //    var result = $"<option value=\"--\" disabled {(("--" == selectedOption) ? "selected" : "")}>- {pleaseSelectPhrase} -</option>";
        //
        //    foreach (var country in countries)
        //    {
        //        result += $"<option value=\"{country.Code}\" {((country.Code == selectedOption) ? "selected" : "")}>{country.Name}</option>";
        //    }
        //
        //    return result;
        //}

        public static string DropdownGenerateOptionValues(List<(string,string)> options, string selectedOption, string pleaseSelectPhrase = "Please Select")
        {
            var result = $"<option value=\"--\" disabled {(("--" == selectedOption) ? "selected" : "")}>- {pleaseSelectPhrase} -</option>";

            foreach (var (id, displayValue) in options)
            {
                result += $"<option value=\"{id}\" {((id == selectedOption) ? "selected" : "")}>{displayValue}</option>";
            }

            return result;
        }

        public static string DropdownInitSelectedValue(string selectedDropDownValue)
        {
            if (string.IsNullOrWhiteSpace(selectedDropDownValue))
                selectedDropDownValue = "--";
            return selectedDropDownValue;
        }

        public static bool DropdownListHasDefaultValue(string value)
        {
            return value == "--";
        }     
		#endregion



        #region ------------- Radio buttons -------------------------------------------------------
        public static string RadiobuttonGenerateOptionValues(List<(string,string)> options, string selectedOption, string onChangeMethodName)
        {
            var result = "";
            int number = 1;

            foreach (var (id, displayValue) in options)
            {
                var selected = (id == selectedOption) ? "true" : "false";
                result += $"<input type=\"radio\" class=\"btn-check\" name=\"btnradio\" id=\"btnradio{number++}\" autocomplete=\"off\" @onchange=\"{onChangeMethodName}\" checked=\"{selected}\">";
                result += $"<label class=\"btn btn-outline-primary\" for=\"btnradio1\">{displayValue}</label>";
            }

            return result;
        }

        public static string RadiobuttonGetCodeByDescription(List<(string,string)> values, ChangeEventArgs args)
        {
            var description = args.Value as string;
            var item = values.FirstOrDefault(x => x.Item2 == description);
            return item.Item1;
        }

        public static string RadiobuttonGetDescription(List<(string,string)> values, ChangeEventArgs args)
        {
            var description = args.Value as string;
            return description;
        }

        public static string RadiobuttonInitSelectedValue(string selectedDropDownValue)
        {
            if (string.IsNullOrWhiteSpace(selectedDropDownValue))
                selectedDropDownValue = "--";
            return selectedDropDownValue;
        }

        public static bool RadiobuttonListHasDefaultValue(string value)
        {
            return value == "--";
        }     
		#endregion



        #region ------------- MessageBox ----------------------------------------------------------
        //public enum MessageboxResult { OK, Yes, No, Cancel };
        //public delegate void OnMessageBoxCloseDelegate(MessageboxResult result);
        //public bool IsMessageBoxVisible { get; private set; }
        //public string MessageBoxTitle { get; set; }
        //public string MessageBoxText { get; set; }
        //public MessageBox.MessageboxButtons MessageBoxButtons { get; set; }
        //public string MessageBoxEntryfieldText { get; set; }
        //private OnMessageBoxCloseDelegate OnMessageBoxClose;
        //
		//public void ShowMessageBox(string title, 
        //                           string message,
        //                           OnMessageBoxCloseDelegate onclose = null, 
        //                           MessageBox.MessageboxButtons buttons = MessageBox.MessageboxButtons.OK)
        //{
        //    MessageBoxTitle     = title;
        //    MessageBoxText      = message.Replace("\n", " ");
        //    MessageBoxButtons   = buttons;
        //    OnMessageBoxClose   = onclose;
        //    IsMessageBoxVisible = true;
        //    _stateHasChangedHandler();
        //}
        //
        public void ShowMessageBox(Exception ex)//, OnMessageBoxCloseDelegate onclose = null)
        {
        //    if (onclose == null)
        //        onclose = delegate(MessageboxResult result) {};
        //
        //    MessageBoxTitle = "Ooops";
        //
        //    var text = ex.ToString();
        //    if (text.Length > 200)
        //        text = text.Substring(0, 200);
        //    MessageBoxText      =ex.StackTrace.ToString() +  "This should not happen!<br /><br />" + text;
        //    OnMessageBoxClose   = onclose;
        //    IsMessageBoxVisible = true;
        //    _stateHasChangedHandler();
        }
        //
        //public void MessageBox_OnOK(MessageBox.MessageboxResult result)
        //{
        //    IsMessageBoxVisible = false;
        //    _stateHasChangedHandler();
        //    if (OnMessageBoxClose != null)
        //        OnMessageBoxClose( (MessageboxResult)result );
        //}
        //public void ShowDataSavedMessage(OnMessageBoxCloseDelegate onclose = null)
        //{
        //    WaitAndThenExecute(200,
        //        delegate()
        //        {
        //            ShowMessageBox(Texts[Txt.Notice], Texts[Txt.DataHasBeenSaved], onclose);
        //        });
		//}
		#endregion



        #region ------------- Analytics -----------------------------------------------------------
        //public void RecordPageVisit(string page)
        //{
        //    try
        //    {
        //        //PostAsync("/api/analytics/pagevisit", page);
        //        LoadSingleRowAsync<object>($"/api/analytics/pagevisit/{page}");
        //    }
        //    catch (Exception ex)
        //    {
        //        // the customer should not have trouble when analytics has a problem
        //    }
        //}
        #endregion



        #region ------------- Backend calls -------------------------------------------------------
        //public delegate void RefreshHandler();
        //
        //public async Task<byte[]> GetBinary(string backendUri)
        //{
        //    try
		//	{
        //        _httpClient.DefaultRequestHeaders.Authorization = _userState.GetAuthorizationHeader();
        //        return await _httpClient.GetByteArrayAsync(backendUri);
		//	}
        //    catch (Exception ex)
		//	{
        //        ShowMessageBox(ex);
        //        return null;
		//	}
        //}
        //
        //public async Task<T> LoadSingleRow<T>(string backendUri)
        //{
        //    try
		//	{
        //        _httpClient.DefaultRequestHeaders.Authorization = _userState.GetAuthorizationHeader();
        //        return await _httpClient.GetJsonAsync<T>(backendUri);
		//	}
        //    catch (Exception ex)
		//	{
        //        ShowMessageBox(ex);
        //        return default(T);
		//	}
        //}
        //
        //public async Task<T> LoadSingleRowAsync<T>(string backendUri)
        //{
        //    _httpClient.DefaultRequestHeaders.Authorization = _userState.GetAuthorizationHeader();
        //    return await _httpClient.GetJsonAsync<T>(backendUri);
        //}
        //
        //public async Task<T> Post<T>(string backendUri, object parameters)
        //{
        //    try
		//	{
        //        _httpClient.DefaultRequestHeaders.Authorization = _userState.GetAuthorizationHeader();
        //        return await _httpClient.PostJsonAsync<T>(backendUri, parameters);
		//	}
        //    catch (Exception ex)
		//	{
        //        ShowMessageBox(ex);
        //        return default(T);
        //    }
        //}
        //
        //public async Task PostAsync(string backendUri, object parameters)
        //{
        //    _httpClient.DefaultRequestHeaders.Authorization = _userState.GetAuthorizationHeader();
        //    await _httpClient.PostJsonAsync(backendUri, parameters);
        //}
        //
        //public async Task PutAsync(string backendUri, object parameters)
        //{
        //    _httpClient.DefaultRequestHeaders.Authorization = _userState.GetAuthorizationHeader();
        //    await _httpClient.PutJsonAsync(backendUri, parameters);
        //}
        //
        //public async Task<T> PutAsync<T>(string backendUri, object parameters)
        //{
        //    _httpClient.DefaultRequestHeaders.Authorization = _userState.GetAuthorizationHeader();
        //    return await _httpClient.PutJsonAsync<T>(backendUri, parameters);
        //}
        //
        //public async Task<string> PutAsyncWithResponse(string requestUri, object parameters)
        //{
        //    string content2 = JsonSerializer.Serialize(parameters);
        //    HttpResponseMessage httpResponseMessage = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Put, requestUri)
        //    {
        //        Content = new StringContent(content2, Encoding.UTF8, "application/json")
        //    });
        //    return await httpResponseMessage.Content.ReadAsStringAsync();
        //}
        //
        //
        //public async Task Post(string backendUri, object parameters)
        //{
        //    _httpClient.DefaultRequestHeaders.Authorization = _userState.GetAuthorizationHeader();
        //    await _httpClient.PostJsonAsync(backendUri, parameters);
        //}
        //
        //public async Task<List<T>> LoadData<T>(string backendUri)
        //{
        //    try
		//	{
        //        await _jsRuntime.InvokeAsync<string>("NProgressStart");
        //        _httpClient.DefaultRequestHeaders.Authorization = _userState.GetAuthorizationHeader();
        //        var rows = await _httpClient.GetJsonAsync<List<T>>(backendUri);
        //        await _jsRuntime.InvokeAsync<string>("NProgressDone");
        //        return rows.ToList();
		//	}
        //    catch (Exception ex)
		//	{
        //        ShowMessageBox(ex);
        //        throw;
		//	}
        //}
        //
        //public async void UpdateRow(string backendUri, object selectedRow, RefreshHandler refreshHandler)
        //{
        //    try
        //    {
        //        await _jsRuntime.InvokeAsync<string>("NProgressStart");
        //        _httpClient.DefaultRequestHeaders.Authorization = _userState.GetAuthorizationHeader();
        //        await _httpClient.PutJsonAsync(backendUri, selectedRow);
        //        await _jsRuntime.InvokeAsync<string>("NProgressDone");
        //        refreshHandler();
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowMessageBox(ex);
        //    }
        //}
        //
        //public async Task Put(string backendUri, object parameters)
        //{
        //    _httpClient.DefaultRequestHeaders.Authorization = _userState.GetAuthorizationHeader();
        //    await _httpClient.PutJsonAsync(backendUri, parameters);
        //}
        //
        //public async Task<T> Put<T>(string backendUri, object parameters)
        //{
        //    _httpClient.DefaultRequestHeaders.Authorization = _userState.GetAuthorizationHeader();
        //    var result = await _httpClient.PutJsonAsync<T>(backendUri, parameters);
        //    return result;
        //}
        //
        //public async Task DeleteAsync(string backendUri)
        //{
        //    _httpClient.DefaultRequestHeaders.Authorization = _userState.GetAuthorizationHeader();
        //    await _httpClient.DeleteAsync(backendUri);
        //}

        //public async Task<T> GetSelectedRow<T>(List<T> rows) where T:IRow
        //{
        //    await ReadAllRowStates(rows);
        //    return (from r in rows where r.Selected select r).FirstOrDefault();
        //}
        //
        //public async Task ReadAllRowStates<T>(List<T> rows) where T:IRow
        //{
        //    for (int i = 0; i < rows.Count; i++)
        //    {
        //        var checkboxValue = await _jsRuntime.InvokeAsync<object>("GetDataGridRowCheckboxWithDefault", "MainDataGrid", i + 1, 0, "CheckboxSelected");
        //        rows[i].Selected = (Convert.ToString(checkboxValue) != "False");
        //    }
        //}
        #endregion
	}
}
