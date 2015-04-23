using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Verifalia.Api;

public partial class Pages_EmailVerify : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        VerifaliaRestClient restClient = new VerifaliaRestClient("af533e54318140e39d3b0a02ccb0fc30", "6B2beQUWgSZneAf8wn0N");

        var result = restClient.EmailValidations.Submit(new[]
        {
            "hoedebeck@gmail.com"
        },
        new WaitForCompletionOptions(TimeSpan.FromMinutes(1)));

        if (result != null) // Result is null if timeout expires
        {
            foreach (var entry in result.Entries)
            {
                Console.WriteLine("Address: {0} => Result: {1}",
                    entry.InputData,
                    entry.Status);
            }
        }
    }
}