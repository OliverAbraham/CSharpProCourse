using libfintx.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace libfintx
{
	public class LibFinTx
	{
		#region ------------- Types and constants -------------------------------------------------
		#endregion



		#region ------------- Properties ----------------------------------------------------------
		#endregion



		#region ------------- Fields --------------------------------------------------------------
		private LibFinTxConfiguration _configuration;
		private static string _messages;
		private static TANDialog _tanDialog;
		#endregion



		#region ------------- Init ----------------------------------------------------------------
		public LibFinTx(LibFinTxConfiguration configuration)
		{
			this._configuration = configuration;
		}
		#endregion



		#region ------------- Methods -------------------------------------------------------------
		public void InitAssemblies()
		{
			LoadAssembly("SixLabors.Fonts.dll");
			LoadAssembly("SixLabors.ImageSharp.dll");
			LoadAssembly("SixLabors.ImageSharp.Drawing.dll");
			LoadAssembly("StatePrinter.dll");
			LoadAssembly("Zlib.Portable.dll");
			LoadAssembly("BouncyCastle.Crypto.dll");
			LoadAssembly("Microsoft.Extensions.Logging.dll");
			LoadAssembly("System.Security.Cryptography.Xml.dll");
		}

		public async Task<string> ReadBalance()
		{
			string value = "";
			try
			{
				_tanDialog = new TANDialog(WaitForTanAsync, null);//pBox_tan);

				var connectionDetails = new ConnectionDetails()
				{
					Url     = _configuration.BankURL,
					Account = _configuration.Kontonummer,
					Blz     = Convert.ToInt32(_configuration.BLZ),
					Pin     = _configuration.LoginPIN,
					UserId  = _configuration.LoginName,
					Iban    = _configuration.IBAN,
					Bic     = _configuration.BIC,
				};

				var client = new FinTsClient(connectionDetails);

				HBCIDialogResult sync;
				try
				{
					sync = await client.Synchronization();
				}
				catch (Exception ex)
				{
					_messages = ex.ToString();
					return "";
				}

				HBCIOutput(sync.Messages);

				if (sync.IsSuccess)
				{
					client.HIRMS = "";//txt_tanverfahren.Text;
					if (!await InitTANMedium(client))
						return "";
					var balance = await client.Balance(_tanDialog);
					HBCIOutput(balance.Messages);
					if (!balance.IsSuccess)
						return "";
					
					if (balance.Data != null)
						_messages = Convert.ToString(balance.Data.Balance);

					value = _messages;
					if (!string.IsNullOrWhiteSpace(_configuration.Format) &&
						_configuration.Format.Contains("{0}"))
					{
						value = _configuration.Format.Replace("{0}", value);
					}
				}
			}
			catch (Exception ex)
			{
				_messages = ex.ToString();
			}
			return value;
		}

		#endregion



		#region ------------- Implementation ------------------------------------------------------
        private void LoadAssembly(string filename)
		{
            Assembly.LoadFrom(filename);
		}

		private static async Task<bool> InitTANMedium(FinTsClient client)
        {
            // TAN-Medium-Name
            var accounts = await client.Accounts(_tanDialog);
            if (!accounts.IsSuccess)
            {
                HBCIOutput(accounts.Messages);
                return false;
            }
            var conn = client.ConnectionDetails;
            AccountInformation accountInfo = UPD.HIUPD?.GetAccountInformations(conn.Account, conn.Blz.ToString());
            if (accountInfo != null && accountInfo.IsSegmentPermitted("HKTAB"))
            {
                client.HITAB = "";// txt_tan_medium.Text;
            }

            return true;
        }

        private static void HBCIOutput(IEnumerable<HBCIBankMessage> hbcimsg)
        {
			_messages = string.Join("\n", hbcimsg);
        }

        static async Task<string> WaitForTanAsync(TANDialog tanDialog)
        {
            foreach (var msg in tanDialog.DialogResult.Messages)
                Console.WriteLine(msg);

            return await Task.FromResult(Console.ReadLine());
        }
		#endregion
	}
}
