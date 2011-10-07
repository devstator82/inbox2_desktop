using System;

namespace LumiSoft.Net
{
	/// <summary>
	/// Represent the method what will handle Error event.
	/// </summary>
	/// <param name="sender">Delegate caller.</param>
	/// <param name="e">Event data.</param>
	public delegate void ErrorEventHandler(object sender,Error_EventArgs e);

	/// <summary>
	/// To be supplied.
	/// </summary>
	public delegate void LogEventHandler(object sender,Log_EventArgs e);


	/// <summary>
	/// Represents the method that will handle the <see href="LumiSoftMailServerSMTPSMTP_ServerValidateIPAddressFieldOrEvent.html">SMTP_Server.ValidateIPAddress</see> and <see href="LumiSoftMailServerPOP3POP3_ServerValidateIPAddressFieldOrEvent.html">POP3_Server.ValidateIPAddress</see>event.
	/// </summary>
	/// <param name="sender">The source of the event. </param>
	/// <param name="e">A <see href="LumiSoftMailServerValidateIP_EventArgs.html">ValidateIP_EventArgs</see> that contains the event data.</param>
	public delegate void ValidateIPHandler(object sender,ValidateIP_EventArgs e);	
}
