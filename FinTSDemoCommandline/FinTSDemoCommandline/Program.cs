using libfintx;

namespace FinTSDemoCommandline
{
	partial class Program
	{
		public static void Main()
		{
			var configuration = new LibFinTxConfiguration();
			configuration.IBAN        = System.IO.File.ReadAllText(@"C:\Cloud\OpenItemsBalancingDebug1.txt");
			configuration.BIC         = System.IO.File.ReadAllText(@"C:\Cloud\OpenItemsBalancingDebug2.txt");
			configuration.LoginName   = System.IO.File.ReadAllText(@"C:\Cloud\OpenItemsBalancingDebug3.txt");
			configuration.LoginPIN    = System.IO.File.ReadAllText(@"C:\Cloud\OpenItemsBalancingDebug4.txt");
			configuration.Kontonummer = System.IO.File.ReadAllText(@"C:\Cloud\OpenItemsBalancingDebug5.txt");
			configuration.BankURL     = Bank.TryGetBankByBIC(configuration.BIC).Url;
			configuration.BLZ         = Bank.TryGetBankByBIC(configuration.BIC).Blz;

			var lib = new LibFinTx(configuration);
			lib.InitAssemblies();
			var balance = lib.ReadBalance().GetAwaiter().GetResult();

			Console.WriteLine($"The balance of your bank account is {balance}");
		}
    }
}
