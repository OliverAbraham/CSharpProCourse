using System;

namespace BlazorApp1
{
	public class UserState
	{
		#region ------------- Properties ----------------------------------------------------------
		public bool   IsLoggedIn        { get; set; }
        public string DisplayName       { get; set; }
        public string UserName          { get; set; }
        public string UserHash          { get; set; }
        public int    ID                { get; set; }
        public bool   IsAdmin           { get; set; }
        public bool   IsSuperAdmin      { get; set; }
        public string Language          { get; set; }
        public System.Net.Http.Headers.AuthenticationHeaderValue AuthenticationHeader { get; set; }
		#endregion



		#region ------------- Init ----------------------------------------------------------------
		public UserState()
		{
            Language = "DE";
		}
		#endregion



		#region ------------- Methods -------------------------------------------------------------
        public string Serialize()
        {
            string Serialized = UserName + "|||" + UserHash;
            return Serialized;
        }

        public static UserState Deserialize(string savedState)
        {
            if (savedState.StartsWith("LoginState="))
                savedState = savedState.Substring("LoginState=".Length);

			var state = new UserState();
            var parts = savedState.Split(new char[]{'|' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.GetLength(0) == 2)
            {
                state.UserName = parts[0];
                state.UserHash = parts[1];
            }
            return state;
        }
		#endregion
    }
}
