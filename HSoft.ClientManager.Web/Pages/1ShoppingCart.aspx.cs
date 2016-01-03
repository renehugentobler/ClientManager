using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Net;
using System.Net.Http;
using System.IO;

public partial class Pages_1ShoppingCart : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ShoppingCart();

    }

    protected void ShoppingCart()
    {
//        string DATA = @"{""Request"":{""Key"":""9679623159ec4a51a9""}}";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.mcssl.com/API/59757");

        request.Method = "POST";
        request.ContentType = "text/xml";
        request.ContentLength = 0;

        //request.ContentLength = DATA.Length;
        //StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
        //requestWriter.Write(DATA);
        //requestWriter.Close();

        try
        {
            WebResponse webResponse = request.GetResponse();
            Stream webStream = webResponse.GetResponseStream();
            StreamReader responseReader = new StreamReader(webStream);
            string response = responseReader.ReadToEnd();
            Console.Out.WriteLine(response);
            responseReader.Close();
        }
        catch (Exception e)
        {
            Console.Out.WriteLine("-----------------");
            Console.Out.WriteLine(e.Message);
        }
             

    }

}