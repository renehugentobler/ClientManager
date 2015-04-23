using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

using HSoft.SQL;

using OboutInc.Calendar2;

public partial class Pages_Contact_overview : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);
        String ssql = String.Format("SELECT TOP {0} " +
                                    "       c.Id,c.Number, c.FirstName, c.LastName, c.OrigEntryDate, AssignedToId, " +
                                    "       e.Email, p.Number PhoneNumber, " +
                                    "       l.CallLaterDate, emp.FirstName empFirstName, emp.LastName empLastName, " +   
                                    "       nc.Note cNote,ne.Note eNote " +
                                    "  FROM Customer c " +
                                    "   INNER JOIN Email e ON c.Id = e.CustomerId AND e.[Primary] = 1 AND e.isdeleted = 0 " +
                                    "   INNER JOIN Lead l ON c.Id = l.CustomerId AND e.isdeleted = 0 " +
                                    "   INNER JOIN Employee emp ON emp.Id = l.AssignedToId  AND emp.isdeleted = 0 " +
                                    "   INNER JOIN PhoneNumber p ON c.Id = p.CustomerId AND p.isdeleted = 0 AND IsLead = 1 "  +
                                    "   INNER JOIN Note nc ON c.Id = nc.CustomerId AND nc.NoteTypeId = (SELECT Id FROM _NoteType WHERE isCustomer=1 AND isEmployee = 0) " +
                                    "   INNER JOIN Note ne ON c.Id = ne.CustomerId AND ne.NoteTypeId = (SELECT Id FROM _NoteType WHERE isCustomer=1 AND isEmployee = 1 AND isPrivate=0) " +
                                    " WHERE e.isdeleted = 0 " +
//                                    "   AND Priority = 'f00563e9-b454-42c6-8257-476a702efabf' " +
//                                    "  AND AssignedToId = '7D5AA961-5478-4FA1-B5DB-D6A2071ED834' " +
                                    " ORDER BY OrigEntryDate DESC, Id " +
                                    "", 2);
        DataTable table = _sql.GetTable(ssql);

        ssql = "SELECT * FROM _LeadEmail WHERE isdeleted = 0 ORDER BY shortCode";       
        DataTable tablelead = _sql.GetTable(ssql);

        ssql = "SELECT l.Id,MasterId,SlaveId,FirstName,LastName FROM _LeadSalesmanPath l,Employee e WHERE l.isdeleted = 0 AND e.isdeleted = 0 AND l.SlaveId = e.Id ORDER BY MasterId";
        DataTable tableleadsale = _sql.GetTable(ssql);

        ssql = String.Format("SELECT * FROM LeadEmail WHERE isdeleted = 0 ","");
        DataTable tableleademail = _sql.GetTable(ssql);

        ssql = String.Format("SELECT * FROM Employee", "");
        DataTable tableemployee = _sql.GetTable(ssql);

        foreach (DataRow dr in table.Rows)
        {
            TableRow r = new TableRow();

            TableCell c = new TableCell();
            c.Controls.Add(new LiteralControl(dr["Number"].ToString()));
            r.Cells.Add(c);

            c = new TableCell();
            c.Controls.Add(new LiteralControl(dr["Email"].ToString()));
            r.Cells.Add(c);

            c = new TableCell();
            c.Controls.Add(new LiteralControl(dr["PhoneNumber"].ToString()));
            r.Cells.Add(c);

            c = new TableCell();
            c.Controls.Add(new LiteralControl(String.Format("{0} {1}", dr["FirstName"].ToString().Trim(), dr["LastName"].ToString().Trim()).Replace(" ", "&nbsp;")));
            r.Cells.Add(c);

            c = new TableCell();
            c.Controls.Add(new LiteralControl(String.Format("{0:yyyy-MM-dd HH:mm:ss}", dr["OrigEntryDate"]).Replace(" ", "&nbsp;").Replace("-", "&#8209;")));
            r.Cells.Add(c);

            c = new TableCell();
            TextBox t = new TextBox();
            t.Columns = 10;
            t.ID = String.Format("{0}_{1}", dr["Id"], "dp");
            OboutInc.Calendar2.Calendar ca = new OboutInc.Calendar2.Calendar();
            ca.DatePickerMode = true;
            ca.TextBoxId = t.ID;
            ca.Enabled = true;
            ca.DateMin = DateTime.Now;
            ca.MultiSelectedDates = false;
            ca.DatePickerSynchronize = true;
            ca.DatePickerImagePath = @"..\Images\Calendar\styles\date_picker1.gif";
            ca.AllowDeselect = true;
            ca.ValidateRequestMode = System.Web.UI.ValidateRequestMode.Disabled;
            ca.ShowTimeSelector = false;
            ca.DateFormat = "MM/dd/yyyy";
            if (dr["CallLaterDate"].ToString().Length != 0) { t.Text = String.Format("{0:MM/dd/yyyy}",dr["CallLaterDate"]); }
            if (dr["CallLaterDate"].ToString().Length != 0)
            {
                ca.SelectedDate = DateTime.Parse(dr["CallLaterDate"].ToString());
            }
            c.Controls.Add(t);
            c.Controls.Add(ca);
            r.Cells.Add(c);

            //c = new TableCell();
            //DropDownList d = new DropDownList();
            //SortedList sl = new SortedList(10);
            //DataRow dl2 = tableemployee.Select(String.Format("Id = '{0}'", dr["AssignedToId"]))[0];
            //d.Items.Add(new ListItem(String.Format("{0} {1}", dl2["FirstName"], dl2["LastName"]), dr["AssignedToId"].ToString()));
            //foreach (DataRow dl in tableleadsale.Select(String.Format("MasterId = '{0}'", Session["guser"])))
            //{
            //    if (dl["SlaveId"].ToString() != dr["AssignedToId"].ToString())
            //    {
            //        d.Items.Add(new ListItem(String.Format("{0} {1}", dl["FirstName"], dl["LastName"]), dl["SlaveId"].ToString()));
            //    }
            //}
            //c.Controls.Add(d);
            //r.Cells.Add(c);


            c = new TableCell();
            foreach (DataRow dr2 in tablelead.Rows)
            {
                Button b = new Button();
                b.Text = dr2["shortCode"].ToString();
                b.Visible = true;
                b.Width = 3;
                b.Enabled = (tableleademail.Select(String.Format("CustomerId='{0}' AND LeadEmailId='{1}'",dr["Id"],dr2["Id"])).GetLength(0)==0);
                c.Controls.Add(b);
            }
            r.Cells.Add(c);

            c = new TableCell();
            c.Controls.Add(new LiteralControl(dr["cNote"].ToString()));
            r.Cells.Add(c);

            c = new TableCell();
            c.Controls.Add(new LiteralControl(dr["eNote"].ToString()));
            r.Cells.Add(c);

            tblCustomer.Rows.Add(r);
        }

    }
}