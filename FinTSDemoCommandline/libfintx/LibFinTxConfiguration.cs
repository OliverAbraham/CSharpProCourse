namespace libfintx
{
	public class LibFinTxConfiguration
	{
		public string IBAN          { get; set; }
		public string BIC           { get; set; }
		public string LoginName     { get; set; }
		public string LoginPIN      { get; set; }
		public string Format        { get; set; }
		public string Kontonummer   { get; set; }

		public string BLZ           ;
		public string BankURL       ;
		public string Zentrale      ;
		public string HBCIVersion   ;
    }
}
