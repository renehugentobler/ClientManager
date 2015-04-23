using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using HSoft.SQL;

public partial class Pages_UnbounceReceiver : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        /*
        String ssql = String.Empty;
        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);

        try
        {

            ssql = String.Format("INSERT INTO _auditt([Table], [Field], [Key], OldValue, NewValue, createdby) " +
                                     "VALUES ('{0}','{1}','{4}','{2}','{3}','{4}') ", "Unbounce", "QS", Request.QueryString, "", Guid.Parse("D0F06FCC-B87E-4F33-A3D3-D64354863F39"));
            _sql.Execute(ssql);

            //         Page.Response.ContentType = "application/x-www-form-urlencoded";
            String xmlData = Request.Form["data.xml"];
            //         StreamReader reader = new StreamReader(Page.Request.InputStream);
            //         String xmlData = reader.ReadToEnd();

            ssql = String.Format("INSERT INTO _auditt([Table], [Field], [Key], OldValue, NewValue, createdby) " +
                                             "VALUES ('{0}','{1}','{4}','{2}','{3}','{4}') ", "Unbounce", "Xml", xmlData, "", Guid.Parse("D0F06FCC-B87E-4F33-A3D3-D64354863F39"));
            _sql.Execute(ssql);
        }
        catch (Exception ex)
        {
            ssql = String.Format("INSERT INTO _auditt([Table], [Field], [Key], OldValue, NewValue, createdby) " +
                                             "VALUES ('{0}','{1}','{4}','{2}','{3}','{4}') ", "Unbounce", "Error", ex.Message.Replace("'","''"), "", Guid.Parse("D0F06FCC-B87E-4F33-A3D3-D64354863F39"));
            _sql.Execute(ssql);
        }
*/
  
    }
}