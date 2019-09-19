using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Drawing;
using System.Collections;
namespace List_Of_Pending_Orders
{
    public partial class Page2 : System.Web.UI.Page
    {
        public OleDbConnection database;
        DataTable data = new DataTable();
        DataTable data1 = new DataTable();
        DataTable dataTable3 = new DataTable();
        bool chkstatus = false;
        OleDbConnection con = new OleDbConnection();
        OleDbCommand oleDbCmd = new OleDbCommand();
        //String connParam = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\CSK\Tablas.mdb";
        //String connParam = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\C#\Office\Tablas.mdb";
        String connParam = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=W:\test\Access\Tablas.mdb";
        //String connParam = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=W:\\test\\Access\\tablas.mdb";
        OleDbDataAdapter da;

        // To keep track of the previous row Group Identifier    
        string strPreviousRowID = string.Empty;
        // To keep track the Index of Group Total    
        int intSubTotalIndex = 1;
        // To temporarily store Sub Total    
        double dblSubTotalUnitPrice = 0;
        double dblSubTotalQuantity = 0;
        double dblSubTotalDiscount = 0;
        double dblSubTotalAmount = 0;
        // To temporarily store Grand Total    
        double dblGrandTotalUnitPrice = 0;
        double dblGrandTotalQuantity = 0;
        double dblGrandTotalDiscount = 0;
        double dblGrandTotalAmount = 0;



        protected void Page_Load(object sender, EventArgs e)
        {
            con = new OleDbConnection(connParam);
            if (!IsPostBack)
            {
                CheckBox1.Checked = true;
                TextBox2.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (TextBox1.Text == "")
            {
                string script = "alert('Please Enter From Date!!!')";
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Button1, this.GetType(), "Test", script, true);
                GridView1.DataSource = null;
                GridView1.DataBind();
            }
            else if (TextBox2.Text == "")
            {
                string script = "alert('Please Enter To Date!!!')";
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Button1, this.GetType(), "Test", script, true);
                GridView1.DataSource = null;
                GridView1.DataBind();

            }
            else
            {
                //<asp:BoundField DataField="NomArt" HeaderText="Description" ReadOnly="True" />
                //DataSet ds = new DataSet();
                OleDbCommand oleDbCmd = con.CreateCommand();
                con.Open();
                oleDbCmd = new OleDbCommand("SELECT DISTINCTROW  [Ordenes de fabricación].NumOrd, [Ordenes de fabricación].DtoOrd, [Ordenes de fabricación].ArtOrd, [Ordenes de fabricación].PinOrd, [Ordenes de fabricación].EntOrd, [Ordenes de fabricación].PieOrd, [Ordenes de fabricación].DtoOrd, [Ordenes de fabricación].PreOrd, [Ordenes de fabricación].EntCli, ([Ordenes de fabricación].PieOrd - [Ordenes de fabricación].EntCli) as Pending, ([Ordenes de fabricación].PieOrd - [Ordenes de fabricación].EntCli) as R, (Pending*[Ordenes de fabricación].PreOrd*(1-[Ordenes de fabricación].DtoOrd/100)) as Price, [Ordenes de fabricación].Observaciones, [Pedidos de clientes].FecPed, [Artículos de clientes].NomArt,[Pedidos de clientes].PedPed, [Pedidos de clientes].CliPed, [Ordenes de fabricación].Datos, [Ordenes de fabricación].GeartRemarks " +   /* Clientes.NomCli, Clientes.Divisa, Divisas.Cambio"  +*/
                                         " FROM   " +
                                         " ([Pedidos de clientes] INNER JOIN ([Artículos de clientes] INNER JOIN [Ordenes de fabricación] ON [Ordenes de fabricación].ArtOrd = [Artículos de clientes].CodArt) ON [Ordenes de fabricación].PinOrd = [Pedidos de clientes].NumPed) " +
                                         " " +
                                         " WHERE (((([Ordenes de fabricación].FinOrd) Is Null))  AND (((([Ordenes de fabricación].EntOrd) Between format(#" + TextBox1.Text + "#, \"dd/mm/yyyy\") And format(#" + TextBox2.Text + "#, \"dd/mm/yyyy\"))))) ORDER BY [Pedidos de clientes].CliPed, [Ordenes de fabricación].NumOrd ASC", con); //GROUP BY [Pedidos de clientes].CliPed, [Ordenes de fabricación].NumOrd ";

                OleDbDataAdapter Da = new OleDbDataAdapter(oleDbCmd);
                Da.SelectCommand = oleDbCmd;
                Da.Fill(data);
                
                //GridView1.DataSource = data;
                //GridView1.DataBind();


                DataTable data1 = new DataTable();
                OleDbCommand oleDbCmd3 = con.CreateCommand();
                oleDbCmd = new OleDbCommand("SELECT DISTINCTROW [Proveedores].NomPro FROM (( [Pedidos a proveedor (líneas)] INNER JOIN [Pedidos a proveedor (cabeceras)]   ON  [Pedidos a proveedor (líneas)].NumPed = [Pedidos a proveedor (cabeceras)].NumPed) INNER JOIN [Proveedores] ON [Pedidos a proveedor (cabeceras)].ProPed = [Proveedores].CodPro ) ", con);
                oleDbCmd3 = new OleDbCommand("SELECT DISTINCTROW  [Ordenes de fabricación].NumOrd, [Proveedores].NomPro" +   /* Clientes.NomCli, Clientes.Divisa, Divisas.Cambio"  +*/
                                         " FROM   " +
                                         " ([Pedidos a proveedor (cabeceras)] INNER JOIN ([Pedidos a proveedor (líneas)] INNER JOIN [Ordenes de fabricación] ON [Ordenes de fabricación].NumOrd = [Pedidos a proveedor (líneas)].NumOrd) ON [Pedidos a proveedor (líneas)].NumPed = [Pedidos a proveedor (cabeceras)].NumPed) INNER JOIN [Proveedores] ON [Pedidos a proveedor (cabeceras)].ProPed = [Proveedores].CodPro" +
                                        " " +
                                         " WHERE (((([Ordenes de fabricación].FinOrd) Is Null))  AND (((([Ordenes de fabricación].EntOrd) Between format(#" + TextBox1.Text + "#, \"dd/mm/yyyy\") And format(#" + TextBox2.Text + "#, \"dd/mm/yyyy\"))))) ORDER BY [Ordenes de fabricación].NumOrd ASC ", con);

                OleDbDataAdapter Da2 = new OleDbDataAdapter(oleDbCmd);
                Da2.SelectCommand = oleDbCmd3;

                DataSet ds = new DataSet();
                Da2.Fill(ds, "Status");
                data1 = ds.Tables["Status"];
                data1 = RemoveDuplicateRows(data1, "NumOrd");

                // Merging DataTables...............

                data.PrimaryKey = new DataColumn[] { data.Columns["NumOrd"] };
                data1.PrimaryKey = new DataColumn[] { data1.Columns["NumOrd"] };



                dataTable3 = data.Copy();
                dataTable3.Merge(data1, false, MissingSchemaAction.Add);
                dataTable3.AcceptChanges();
                dataTable3.Columns["Observaciones"].SetOrdinal(18);
                // RemoveFutureUIDs(dataTable3);


                OleDbDataAdapter adcli = new OleDbDataAdapter("SELECT CodCli,Divisa FROM Clientes  ORDER BY CodCli ASC", con);
                DataTable dtcli = new DataTable();
                adcli.Fill(dtcli);

                string s1 = "$";
                string s2 = "Rs";
                string s3 = "EU";

                for (int j = 0; j < dtcli.Rows.Count; j++)
                {
                    for (int i = 0; i < dataTable3.Rows.Count; i++)
                    {
                        string customer_code = dataTable3.Rows[i][15].ToString().Substring(0, 6);
                        string customer_dtcli = dtcli.Rows[j][0].ToString();
                        string currency = dtcli.Rows[j][1].ToString();

                        if (customer_code.Equals(customer_dtcli) && currency.Equals(s1))
                        {
                            dataTable3.Rows[i][11] = Convert.ToDecimal(dataTable3.Rows[i][11].ToString()) * Convert.ToDecimal(0.74);//**for $ **//
                            //dataTable3.Rows[i][11] = Convert.ToDecimal(dataTable3.Rows[i][11].ToString()) / Convert.ToDecimal(1.1);//**for $ **//
                        }
                        if (customer_code.Equals(customer_dtcli) && currency.Equals(s2))
                        {
                            string rupees = dataTable3.Rows[i][11].ToString();
                            if (dataTable3.Rows[i][11] != null)
                            {
                                dataTable3.Rows[i][11] = Convert.ToDecimal(dataTable3.Rows[i][11].ToString()) * Convert.ToDecimal(0.02);//**for Rs.**//
                                //dataTable3.Rows[i][11] = Convert.ToDecimal(dataTable3.Rows[i][11].ToString()) / Convert.ToDecimal(60);//**for Rs.**//
                            }
                            string rupees1 = dataTable3.Rows[i][11].ToString();
                        }
                        if (customer_code.Equals(customer_dtcli) && currency.Equals(s3))
                        {
                            if (dataTable3.Rows[i][11] != null)
                                dataTable3.Rows[i][11] = Convert.ToDecimal(dataTable3.Rows[i][11].ToString()) * Convert.ToDecimal(1);//**for Euro**//
                        }
                    }
                }
                if (CheckBox1.Checked == false)
                {
                    foreach (DataRow dr in dataTable3.Rows)
                    {
                        if (Convert.ToString(dr[16]).Contains("@"))
                        {
                            dr.Delete();
                        }
                    }
                }
                dataTable3.AcceptChanges();
                FillBlankCell();
                GridView1.DataSource = dataTable3;
                GridView1.DataBind();

                GridView2.DataSource = dataTable3;
                GridView2.DataBind();
                //BindDropDown();


            }
        }

        void FillBlankCell()
        {
            for (int l = 0; l < dataTable3.Rows.Count; l++)
            {
                for (int m = 0; m < dataTable3.Columns.Count; m++)
                {
                    if ((dataTable3.Rows[l][m].ToString()) == "-1")
                    {
                        // Write your Custom Code
                        dataTable3.Rows[l][m] = "0";
                    }
                }
            }
        }

        /*void BindDropDown()
        {
            DataTable dropdowndata = new DataTable();
            OleDbCommand oleDbCmd4 = con.CreateCommand();
            oleDbCmd4 = new OleDbCommand("SELECT DISTINCTROW [Proveedores].NomPro FROM  [Proveedores] ORDER BY [Proveedores].NomPro ASC", con);
            OleDbDataAdapter Da4 = new OleDbDataAdapter(oleDbCmd);
            Da4.SelectCommand = oleDbCmd4;
            Da4.Fill(dropdowndata);
            DropDownList1.DataSource = dropdowndata;
            DropDownList1.DataTextField = "NomPro";
            DropDownList1.DataValueField = "NomPro";
            DropDownList1.DataBind();
            DropDownList1.Items.Insert(0, new ListItem("-- Select All --", ""));

        }*/

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            OleDbCommand oleDbCmd = con.CreateCommand();
            con.Open();
            oleDbCmd = new OleDbCommand("SELECT DISTINCTROW  [Ordenes de fabricación].NumOrd, [Ordenes de fabricación].DtoOrd, [Ordenes de fabricación].ArtOrd, [Ordenes de fabricación].PinOrd, [Ordenes de fabricación].EntOrd, [Ordenes de fabricación].PieOrd, [Ordenes de fabricación].DtoOrd, [Ordenes de fabricación].PreOrd, [Ordenes de fabricación].EntCli, ([Ordenes de fabricación].PieOrd - [Ordenes de fabricación].EntCli) as Pending, ([Ordenes de fabricación].PieOrd - [Ordenes de fabricación].EntCli) as R, (Pending*[Ordenes de fabricación].PreOrd*(1-[Ordenes de fabricación].DtoOrd/100)) as Price, [Pedidos de clientes].FecPed, [Artículos de clientes].NomArt,[Pedidos de clientes].PedPed, [Pedidos de clientes].CliPed, [Ordenes de fabricación].Datos " +   /* Clientes.NomCli, Clientes.Divisa, Divisas.Cambio"  +*/
                                     " FROM   " +
                                     " ([Pedidos de clientes] INNER JOIN ([Artículos de clientes] INNER JOIN [Ordenes de fabricación] ON [Ordenes de fabricación].ArtOrd = [Artículos de clientes].CodArt) ON [Ordenes de fabricación].PinOrd = [Pedidos de clientes].NumPed) " +
                                     " " +
                                     " WHERE (((([Ordenes de fabricación].FinOrd) Is Null))  AND (((([Ordenes de fabricación].EntOrd) Between format(#" + TextBox1.Text + "#, \"dd/mm/yyyy\") And format(#" + TextBox2.Text + "#, \"dd/mm/yyyy\"))))) ORDER BY [Pedidos de clientes].CliPed, [Ordenes de fabricación].NumOrd ASC", con); //GROUP BY [Pedidos de clientes].CliPed, [Ordenes de fabricación].NumOrd ";

            OleDbDataAdapter Da = new OleDbDataAdapter(oleDbCmd);
            Da.SelectCommand = oleDbCmd;
            Da.Fill(data);
            //GridView1.DataSource = data;
            //GridView1.DataBind();


            DataTable data1 = new DataTable();
            OleDbCommand oleDbCmd3 = con.CreateCommand();
            oleDbCmd = new OleDbCommand("SELECT DISTINCTROW [Proveedores].NomPro FROM (( [Pedidos a proveedor (líneas)] INNER JOIN [Pedidos a proveedor (cabeceras)]   ON  [Pedidos a proveedor (líneas)].NumPed = [Pedidos a proveedor (cabeceras)].NumPed) INNER JOIN [Proveedores] ON [Pedidos a proveedor (cabeceras)].ProPed = [Proveedores].CodPro ) ", con);
            oleDbCmd3 = new OleDbCommand("SELECT DISTINCTROW  [Ordenes de fabricación].NumOrd, [Proveedores].NomPro" +   /* Clientes.NomCli, Clientes.Divisa, Divisas.Cambio"  +*/
                                     " FROM   " +
                                     " ([Pedidos a proveedor (cabeceras)] INNER JOIN ([Pedidos a proveedor (líneas)] INNER JOIN [Ordenes de fabricación] ON [Ordenes de fabricación].NumOrd = [Pedidos a proveedor (líneas)].NumOrd) ON [Pedidos a proveedor (líneas)].NumPed = [Pedidos a proveedor (cabeceras)].NumPed) INNER JOIN [Proveedores] ON [Pedidos a proveedor (cabeceras)].ProPed = [Proveedores].CodPro" +
                                    " " +
                                     " WHERE (((([Ordenes de fabricación].FinOrd) Is Null))  AND (((([Ordenes de fabricación].EntOrd) Between format(#" + TextBox1.Text + "#, \"dd/mm/yyyy\") And format(#" + TextBox2.Text + "#, \"dd/mm/yyyy\"))))) ORDER BY [Ordenes de fabricación].NumOrd ASC ", con);

            OleDbDataAdapter Da2 = new OleDbDataAdapter(oleDbCmd);
            Da2.SelectCommand = oleDbCmd3;

            DataSet ds = new DataSet();
            Da2.Fill(ds, "Status");
            data1 = ds.Tables["Status"];
            data1 = RemoveDuplicateRows(data1, "NumOrd");

            // DataTable data3 = ds.Tables["Status"].DefaultView.ToTable();
            //GridView1.DataSource = ds.Tables["Status"].DefaultView;
            //GridView1.DataBind();




            // Merging DataTables...............

            data.PrimaryKey = new DataColumn[] { data.Columns["NumOrd"] };
            data1.PrimaryKey = new DataColumn[] { data1.Columns["NumOrd"] };



            dataTable3 = data.Copy();
            dataTable3.Merge(data1, false, MissingSchemaAction.Add);
            dataTable3.AcceptChanges();


            ////DataTable dropdowndata = new DataTable();
            //            OleDbCommand oleDbCmd5 = con.CreateCommand();
            ////            oleDbCmd5 = new OleDbCommand("SELECT * FROM  dataTable3 WHERE NomPro = DropDownList1", con);
            ////            oleDbCmd5.Parameters.AddWithValue("@location", DropDownList1.SelectedValue);
            //           OleDbDataAdapter Da5 = new OleDbDataAdapter(oleDbCmd5);
            //            Da5.SelectCommand = oleDbCmd5;
            //            Da5.Fill(dataTable3);

            //if (DropDownList1.SelectedValue != "")
            //{
            //    //if (DropDownList1.SelectedValue == "-- Select All --")
            //    //{
            //    //    GridView1.DataSource = dataTable3;
            //    //    GridView1.DataBind();
            //    //}
            //    DataView dvData = new DataView(dataTable3);
            //    dvData.RowFilter = "NomPro = '" + DropDownList1.Text + "'";


            //    GridView1.DataSource = dvData;
            //    GridView1.DataBind();
            //}
            //else
            //{
            //DataTable dropdowndata2 = new DataTable();
            //            OleDbCommand oleDbCmd6 = con.CreateCommand();
            //            oleDbCmd6 = new OleDbCommand("SELECT * FROM  dataTable3 ", con);
            //            OleDbDataAdapter Da6 = new OleDbDataAdapter(oleDbCmd6);
            //            Da6.SelectCommand = oleDbCmd6;
            //            Da6.Fill(dropdowndata2);
            GridView1.DataSource = dataTable3;
            GridView1.DataBind();
            //}
        }
        public void RemoveFutureUIDs(DataTable dt)
        {
            DateTime deldate;
            foreach (DataRow dr in dt.Rows)
            {
                // deldate = DateTime.dr[2].ToString("dd/MM/yyyy");

            }
        }

        public DataTable RemoveDuplicateRows(DataTable dx, string NumOrd)
        {
            Hashtable hTable = new Hashtable();
            ArrayList duplicateList = new ArrayList();

            foreach (DataRow drow in dx.Rows)
            {
                if (hTable.Contains(drow[NumOrd]))
                {
                    drow[1] = "AWS2";
                    duplicateList.Add(drow);

                }
                else
                    hTable.Add(drow[NumOrd], drow["NomPro"]);
            }

            foreach (DataRow dRow in duplicateList)
            {
                dx.Rows.Remove(dRow);
            }
            return dx;
        }

        double TotalPrice = 0;
        Double total = 0;
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt = new DataTable();

            //string val3 = e.Row.Cells[7].Text;
            //string val4 = e.Row.Cells[8].Text;
            //string val5 = e.Row.Cells[9].Text;
            ////Label lblTotal = (Label)e.Row.Cells[9].FindControl("Label1");

            //double multiply = Math.Round(Convert.ToDouble(val3) * Convert.ToDouble(val4) * Convert.ToDouble(1 - Convert.ToDouble(val5) / 100), 2);
            ////lblTotal.Text += multiply.ToString();
            ////total += multiply;

            /* if (e.Row.RowType == DataControlRowType.DataRow)
             {
                 TotalPrice += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "PrePie"));

             }
             else if (e.Row.RowType == DataControlRowType.Footer)
             {
                 e.Row.Cells[7].Text = TotalPrice.ToString();
                 e.Row.Cells[7].Font.Bold = true;

             }*/



            // This is for cumulating the values       
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                strPreviousRowID = DataBinder.Eval(e.Row.DataItem, "CliPed").ToString();
                double dblUnitPrice = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "Pending").ToString());
                //double dblQuantity = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "Quantity").ToString());
                //foreach (DataRow dr in dt.Rows)
                //{
                //    if ((DataBinder.Eval(e.Row.DataItem, "Pending") != DBNull.Value))
                //    {
                //        double dblAmount = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "Pending").ToString());
                //        dblSubTotalAmount += dblAmount;
                //        dblGrandTotalAmount += dblAmount;
                //    }
                //}
                double dblDiscount = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "Price").ToString());
                double dblAmount = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "Pending").ToString());
                // Cumulating Sub Total            
                dblSubTotalUnitPrice += dblUnitPrice;
                //dblSubTotalQuantity += dblQuantity;
                dblSubTotalDiscount += dblDiscount;
                dblSubTotalAmount += dblAmount;
                // Cumulating Grand Total           
                dblGrandTotalUnitPrice += dblUnitPrice;
                //dblGrandTotalQuantity += dblQuantity;
                dblGrandTotalDiscount += dblDiscount;
                dblGrandTotalAmount += dblAmount;

                // This is for cumulating the values
                // Change Row Color on Mouse Hover
                //if (e.Row.RowType == DataControlRowType.DataRow)
                //{
                //    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#ddd'");
                //    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=''");
                //    e.Row.Attributes.Add("style", "cursor:pointer;");
                //    //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(GridView1, "Select$" + e.Row.RowIndex);
                //}
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Convert.ToInt32(e.Row.Cells[11].Text) < Convert.ToInt32(e.Row.Cells[10].Text) && Convert.ToInt32(e.Row.Cells[11].Text) > 0 || Convert.ToInt32(e.Row.Cells[10].Text) - Convert.ToInt32(e.Row.Cells[11].Text) < 0) //Here is the condition!
                {
                    //   
                    //Change the cell color.
                    e.Row.Cells[5].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[5].Text = "R";

                    //
                    //Change the back color.
                    //e.Row.Cells[3].BackColor = Color.Red;
                    //Label1.Visible = true;
                    // Label1.Text = "Pending On AWS2";

                }
                else
                    e.Row.Cells[5].Text = "";


            }
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    if (Convert.ToInt32(e.Row.Cells[7].Text) < 0) //Here is the condition!
            //    {
            //        //   
            //        //Change the cell color.
            //        e.Row.Cells[7].ForeColor = System.Drawing.Color.Red;
            //        e.Row.Cells[7].Text = "0";

            //        //
            //        //Change the back color.
            //        //e.Row.Cells[3].BackColor = Color.Red;
            //        //Label1.Visible = true;
            //        // Label1.Text = "Pending On AWS2";

            //    }

            //}

        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                string productID = GridView1.Rows[Convert.ToInt32(e.CommandArgument.ToString())].Cells[0].Text;
                string productName = GridView1.Rows[Convert.ToInt32(e.CommandArgument.ToString())].Cells[1].Text;
                //lblProductID.Text = productID;
                //lblProduct.Text = productName;
            }

            if (e.CommandName == "update-something")
            {
                GridView1.SelectedIndex = Convert.ToInt32(e.CommandArgument);
            }
        }
        //Drawing in pop-up window
        protected void lnkView_Click(object sender, EventArgs e)
        {

            //Pop up...........................
            GridViewRow grdrow = (GridViewRow)((LinkButton)sender).NamingContainer;
            int intId = 100;

            string strPopup = "<script language='javascript' ID='script1'>"

            // Passing intId to popup window.
            + "window.open('Drawing2.aspx?UID=" + grdrow.Cells[3].Text + "&Article=" + grdrow.Cells[4].Text + "&testdrawing= kkk" + "data=" + HttpUtility.UrlEncode("UID=" + grdrow.Cells[3].Text + "&Article=" + grdrow.Cells[4].Text + "&testdrawing= kkk")

            + "','new window', 'top=70, left=250, width=470, height=590, dependant=no, location=0, alwaysRaised=no, menubar=no, resizeable=no, scrollbars=n, toolbar=no, status=no, center=yes')"

            + "</script>";

            ScriptManager.RegisterStartupScript((Page)HttpContext.Current.Handler, typeof(Page), "Script1", strPopup, false);

        }

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            bool IsSubTotalRowNeedToAdd = false;
            bool IsGrandTotalRowNeedtoAdd = false;
            if ((strPreviousRowID != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "CliPed") != null))
                if (strPreviousRowID != DataBinder.Eval(e.Row.DataItem, "CliPed").ToString())
                    IsSubTotalRowNeedToAdd = true;
            if ((strPreviousRowID != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "CliPed") == null))
            {
                IsSubTotalRowNeedToAdd = true;
                IsGrandTotalRowNeedtoAdd = true;
                intSubTotalIndex = 0;
            }
            #region Inserting first Row and populating fist Group Header details
            if ((strPreviousRowID == string.Empty) && (DataBinder.Eval(e.Row.DataItem, "CliPed") != null))
            {
                GridView grdViewOrders = (GridView)sender;
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                TableCell cell = new TableCell();
                cell.Text = "" + DataBinder.Eval(e.Row.DataItem, "CliPed").ToString();
                //cell.Text = "Customer Code : " + DataBinder.Eval(e.Row.DataItem, "CliPed").ToString();
                cell.ColumnSpan = 11;
                cell.HorizontalAlign = HorizontalAlign.Left;
                cell.CssClass = "GroupHeaderStyle";
                row.Cells.Add(cell);
                grdViewOrders.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row);
                intSubTotalIndex++;
            }
            #endregion
            if (IsSubTotalRowNeedToAdd)
            {
                #region Adding Sub Total Row
                GridView grdViewOrders = (GridView)sender;
                // Creating a Row          
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                //Adding Total Cell          
                TableCell cell = new TableCell();
                //cell.Text = "Total Per Customer (Points) :";
                //cell.Text = "Total Per Customer (Points) :";
                cell.HorizontalAlign = HorizontalAlign.Left;
                cell.ColumnSpan = 7;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);
                //Adding Unit Price Column          
                cell = new TableCell();
                //cell.Text = string.Format("{0:0.00}", dblSubTotalUnitPrice);

                //if (DataBinder.Eval(e.Row.DataItem, "Divisa").ToString() == "EU")
                //{
                //    cell.Text = string.Format("{0:n0}", dblSubTotalUnitPrice * 2);
                //}
                //else
                //{
                cell.Text = string.Format("{0:n0}", dblSubTotalUnitPrice);
                //}
                cell.Text = string.Format("{0:n0}", dblSubTotalUnitPrice);
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);
                //Adding Quantity Column            
                cell = new TableCell();
                //cell.Text = string.Format("{0:0.00}", dblSubTotalDiscount);
                //cell.Text = string.Format("{0:n}", dblSubTotalDiscount);

                cell.Text = string.Format("{0:n}", dblSubTotalDiscount);
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);
                //Adding Discount Column         
                cell = new TableCell();
                //cell.Text = string.Format("{0:0.00}", dblSubTotalAmount);
                cell.Text = string.Format("{0:n0}", dblSubTotalAmount);
                cell.Text = string.Format("{0:n0}", dblSubTotalAmount);
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "SubTotalRowStyle";
                //row.Cells.Add(cell);
                //Adding Amount Column         
                cell = new TableCell();
                //cell.Text = string.Format("{0:0.00}", dblSubTotalAmount);
                //cell.HorizontalAlign = HorizontalAlign.Right;
                cell.CssClass = "SubTotalRowStyle"; row.Cells.Add(cell);
                //Adding the Row at the RowIndex position in the Grid      
                grdViewOrders.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row);
                intSubTotalIndex++;
                #endregion
                #region Adding Next Group Header Details
                if (DataBinder.Eval(e.Row.DataItem, "CliPed") != null)
                {
                    row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                    cell = new TableCell();
                    cell.Text = "" + DataBinder.Eval(e.Row.DataItem, "CliPed").ToString();
                    //cell.Text = "Customer Code : " + DataBinder.Eval(e.Row.DataItem, "CliPed").ToString();
                    cell.ColumnSpan = 11;
                    cell.HorizontalAlign = HorizontalAlign.Left;
                    cell.CssClass = "GroupHeaderStyle";
                    row.Cells.Add(cell);
                    grdViewOrders.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row);
                    intSubTotalIndex++;
                }
                #endregion
                #region Reseting the Sub Total Variables
                dblSubTotalUnitPrice = 0;
                dblSubTotalQuantity = 0;
                dblSubTotalDiscount = 0;
                dblSubTotalAmount = 0;
                #endregion
            }
            if (IsGrandTotalRowNeedtoAdd)
            {
                #region Grand Total Row
                GridView grdViewOrders = (GridView)sender;
                // Creating a Row      
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                //Adding Total Cell           
                TableCell cell = new TableCell();
                cell.Text = "Grand Total";
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.ColumnSpan = 7;
                cell.CssClass = "GrandTotalRowStyle";
                row.Cells.Add(cell);
                //Adding Unit Price Column          
                cell = new TableCell();
                //cell.Text = string.Format("{0:0.00}", dblGrandTotalUnitPrice);
                //cell.Text = string.Format("{0:0}", dblGrandTotalUnitPrice);
                cell.Text = string.Format("{0:n0}", dblGrandTotalUnitPrice);
                cell.HorizontalAlign = HorizontalAlign.Right;
                cell.CssClass = "GrandTotalRowStyle";
                row.Cells.Add(cell);
                //Adding Quantity Column           
                cell = new TableCell();
                //cell.Text = string.Format("{0:0.00}", dblGrandTotalDiscount);
                cell.Text = string.Format("{0:n}", dblGrandTotalDiscount);
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "GrandTotalRowStyle";
                row.Cells.Add(cell);
                //Adding Discount Column          
                //cell = new TableCell();
                //cell.Text = string.Format("{0:0.00}", dblGrandTotalAmount);
                //cell.HorizontalAlign = HorizontalAlign.Center;
                cell.CssClass = "GrandTotalRowStyle";
                row.Cells.Add(cell);
                //Adding Amount Column          
                cell = new TableCell();
                //cell.Text = string.Format("{0:0.00}", dblGrandTotalAmount);
                //cell.HorizontalAlign = HorizontalAlign.Right;
                cell.CssClass = "GrandTotalRowStyle";
                row.Cells.Add(cell);
                //Adding the Row at the RowIndex position in the Grid     
                grdViewOrders.Controls[0].Controls.AddAt(e.Row.RowIndex, row);
                #endregion
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            //ExportGridToExcel();
            //Button1_Click(null,null);


            PendingOnAWS1();
            foreach (DataRow dr in dataTable3.Rows)
            {
                if (Convert.ToString(dr[18]) == "AWS2")
                {
                    dr.Delete();

                }
            }

            dataTable3.AcceptChanges();

            foreach (DataRow dr in dataTable3.Rows)
            {
                if (Convert.ToString(dr[9]) == "2")
                {
                    dr.Delete();
                }
            }
            dataTable3.AcceptChanges();

            if (CheckBox1.Checked == false)
            {
                foreach (DataRow dr in dataTable3.Rows)
                {
                    if (Convert.ToString(dr[17]).Contains("@"))
                    {
                        dr.Delete();

                    }
                }
            }
            dataTable3.AcceptChanges();
            GridView1.DataSource = dataTable3;
            GridView1.DataBind();

            GridView2.DataSource = dataTable3;
            GridView2.DataBind();
        }

        void PendingOnAWS1()
        {
            if (TextBox1.Text == "")
            {
                string script = "alert('Please Enter From Date!!!')";
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Button1, this.GetType(), "Test", script, true);
                GridView1.DataSource = null;
                GridView1.DataBind();
            }
            else if (TextBox2.Text == "")
            {
                string script = "alert('Please Enter To Date!!!')";
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Button1, this.GetType(), "Test", script, true);
                GridView1.DataSource = null;
                GridView1.DataBind();

            }
            else
            {
                OleDbCommand oleDbCmd = con.CreateCommand();
                con.Open();

                oleDbCmd = new OleDbCommand("SELECT DISTINCTROW  [Ordenes de fabricación].NumOrd, [Ordenes de fabricación].DtoOrd, [Ordenes de fabricación].ArtOrd, [Ordenes de fabricación].PinOrd, [Ordenes de fabricación].EntOrd, [Ordenes de fabricación].PieOrd, [Ordenes de fabricación].DtoOrd, [Ordenes de fabricación].PreOrd, [Ordenes de fabricación].EntCli, [Ordenes de fabricación].Location, ([Ordenes de fabricación].PieOrd - [Ordenes de fabricación].EntCli) as Pending, ([Ordenes de fabricación].PieOrd - [Ordenes de fabricación].EntCli) as R, (Pending*[Ordenes de fabricación].PreOrd*(1-[Ordenes de fabricación].DtoOrd/100)) as Price, [Ordenes de fabricación].Observaciones, [Pedidos de clientes].FecPed, [Artículos de clientes].NomArt,[Pedidos de clientes].PedPed, [Pedidos de clientes].CliPed, [Ordenes de fabricación].Datos,[Ordenes de fabricación].GeartRemarks " +   /* Clientes.NomCli, Clientes.Divisa, Divisas.Cambio"  +*/
                                         " FROM   " +
                                         " ([Pedidos de clientes] INNER JOIN ([Artículos de clientes] INNER JOIN [Ordenes de fabricación] ON [Ordenes de fabricación].ArtOrd = [Artículos de clientes].CodArt) ON [Ordenes de fabricación].PinOrd = [Pedidos de clientes].NumPed)" +
                                         " " +
                                         " WHERE (((([Ordenes de fabricación].FinOrd) Is Null))  AND (((([Ordenes de fabricación].EntOrd) Between format(#" + TextBox1.Text + "#, \"dd/mm/yyyy\") And format(#" + TextBox2.Text + "#, \"dd/mm/yyyy\"))))) ORDER BY [Pedidos de clientes].CliPed, [Ordenes de fabricación].NumOrd ASC", con); //GROUP BY [Pedidos de clientes].CliPed, [Ordenes de fabricación].NumOrd ";

                OleDbDataAdapter Da = new OleDbDataAdapter(oleDbCmd);
                Da.SelectCommand = oleDbCmd;
                Da.Fill(data);
                //GridView1.DataSource = data;
                //GridView1.DataBind();


                DataTable data1 = new DataTable();
                OleDbCommand oleDbCmd3 = con.CreateCommand();
                oleDbCmd = new OleDbCommand("SELECT DISTINCTROW [Proveedores].NomPro FROM (( [Pedidos a proveedor (líneas)] INNER JOIN [Pedidos a proveedor (cabeceras)]   ON  [Pedidos a proveedor (líneas)].NumPed = [Pedidos a proveedor (cabeceras)].NumPed) INNER JOIN [Proveedores] ON [Pedidos a proveedor (cabeceras)].ProPed = [Proveedores].CodPro ) ", con);
                oleDbCmd3 = new OleDbCommand("SELECT DISTINCTROW  [Ordenes de fabricación].NumOrd, [Proveedores].NomPro" +   /* Clientes.NomCli, Clientes.Divisa, Divisas.Cambio"  +*/
                                         " FROM   " +
                                         " ([Pedidos a proveedor (cabeceras)] INNER JOIN ([Pedidos a proveedor (líneas)] INNER JOIN [Ordenes de fabricación] ON [Ordenes de fabricación].NumOrd = [Pedidos a proveedor (líneas)].NumOrd) ON [Pedidos a proveedor (líneas)].NumPed = [Pedidos a proveedor (cabeceras)].NumPed) INNER JOIN [Proveedores] ON [Pedidos a proveedor (cabeceras)].ProPed = [Proveedores].CodPro" +
                                        " " +
                                         " WHERE (((([Ordenes de fabricación].FinOrd) Is Null))  AND (((([Ordenes de fabricación].EntOrd) Between format(#" + TextBox1.Text + "#, \"dd/mm/yyyy\") And format(#" + TextBox2.Text + "#, \"dd/mm/yyyy\"))))) ORDER BY [Ordenes de fabricación].NumOrd ASC ", con);

                OleDbDataAdapter Da2 = new OleDbDataAdapter(oleDbCmd);
                Da2.SelectCommand = oleDbCmd3;

                DataSet ds = new DataSet();
                Da2.Fill(ds, "Status");
                data1 = ds.Tables["Status"];
                data1 = RemoveDuplicateRows(data1, "NumOrd");

                // DataTable data3 = ds.Tables["Status"].DefaultView.ToTable();
                //GridView1.DataSource = ds.Tables["Status"].DefaultView;
                //GridView1.DataBind();




                // Merging DataTables...............

                data.PrimaryKey = new DataColumn[] { data.Columns["NumOrd"] };
                data1.PrimaryKey = new DataColumn[] { data1.Columns["NumOrd"] };



                dataTable3 = data.Copy();
                dataTable3.Merge(data1, false, MissingSchemaAction.Add);
                dataTable3.AcceptChanges();
                dataTable3.Columns["Observaciones"].SetOrdinal(18);
                // RemoveFutureUIDs(dataTable3);

                OleDbDataAdapter adcli = new OleDbDataAdapter("SELECT CodCli,Divisa FROM Clientes  ORDER BY CodCli ASC", con);
                DataTable dtcli = new DataTable();
                adcli.Fill(dtcli);

                string s1 = "$";
                string s2 = "Rs";
                string s3 = "EU";

                for (int j = 0; j < dtcli.Rows.Count; j++)
                {
                    for (int i = 0; i < dataTable3.Rows.Count; i++)
                    {
                        string customer_code = dataTable3.Rows[i][16].ToString().Substring(0, 6);
                        string customer_dtcli = dtcli.Rows[j][0].ToString();
                        string currency = dtcli.Rows[j][1].ToString();

                        if (customer_code.Equals(customer_dtcli) && currency.Equals(s1))
                        {
                            dataTable3.Rows[i][12] = Convert.ToDecimal(dataTable3.Rows[i][12].ToString()) * Convert.ToDecimal(0.74);//**for $ **//
                            //dataTable3.Rows[i][11] = Convert.ToDecimal(dataTable3.Rows[i][11].ToString()) / Convert.ToDecimal(1.1);//**for $ **//
                        }
                        if (customer_code.Equals(customer_dtcli) && currency.Equals(s2))
                        {
                            string rupees = dataTable3.Rows[i][12].ToString();
                            if (dataTable3.Rows[i][12] != null)
                            {
                                dataTable3.Rows[i][12] = Convert.ToDecimal(dataTable3.Rows[i][12].ToString()) * Convert.ToDecimal(0.02);//**for Rs.**//
                                //dataTable3.Rows[i][11] = Convert.ToDecimal(dataTable3.Rows[i][11].ToString()) / Convert.ToDecimal(60);//**for Rs.**//
                            }
                            string rupees1 = dataTable3.Rows[i][12].ToString();
                        }
                        if (customer_code.Equals(customer_dtcli) && currency.Equals(s3))
                        {
                            if (dataTable3.Rows[i][12] != null)
                                dataTable3.Rows[i][12] = Convert.ToDecimal(dataTable3.Rows[i][12].ToString()) * Convert.ToDecimal(1);//**for Euro**//
                        }
                    }
                }



                FillBlankCell();
                //GridView1.DataSource = dataTable3;
                //GridView1.DataBind();
                //BindDropDown();


            }
        }
        void ExportToExcel()
        {
            string FileName = "List Of Pending Orders ( "+DateTime.Today.ToString("dd/MM/yy")+" )";
            string FileNameDateFormat = FileName;
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename="+FileName+".xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                //GridView1.AllowPaging = false;
                //this.btnAWS2_Click(null,null);

                GridView2.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in GridView2.HeaderRow.Cells)
                {
                    cell.BackColor = GridView2.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in GridView2.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = GridView2.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = GridView2.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                GridView2.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
        protected void Button3_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }
        protected void btnAWS2_Click(object sender, EventArgs e)
        {
            //ExportGridToExcel();
            //Button1_Click(null,null);
            PendingOnAWS1();
            //foreach (DataRow dr in dataTable3.Rows)
            //{
            //    if (Convert.ToString(dr[18]) == "AWS1")
            //    {
            //        dr.Delete();

            //    }
            //}

            //dataTable3.AcceptChanges();

            foreach (DataRow dr in dataTable3.Rows)
            {
                if (Convert.ToString(dr[9]) == "1")
                {
                    dr.Delete();
                }
            }
            dataTable3.AcceptChanges();

            if (CheckBox1.Checked == false)
            {
                foreach (DataRow dr in dataTable3.Rows)
                {
                    if (Convert.ToString(dr[17]).Contains("@"))
                    {
                        dr.Delete();

                    }
                }
            }


            //Currency Check

            //OleDbDataAdapter adcli = new OleDbDataAdapter("SELECT CodCli,Divisa FROM Clientes  ORDER BY CodCli ASC", con);
            //DataTable dtcli = new DataTable();
            //adcli.Fill(dtcli);

            //string s1 = "$";
            //string s2 = "Rs";
            //string s3 = "EU";

            //for (int j = 0; j < dtcli.Rows.Count; j++)
            //{
            //    for (int i = 0; i < dataTable3.Rows.Count; i++)
            //    {
            //        string customer_code = dataTable3.Rows[i][15].ToString().Substring(0, 6);
            //        string customer_dtcli = dtcli.Rows[j][0].ToString();
            //        string currency = dtcli.Rows[j][1].ToString();

            //        if (customer_code.Equals(customer_dtcli) && currency.Equals(s1))
            //        {
            //            dataTable3.Rows[i][11] = Convert.ToDecimal(dataTable3.Rows[i][11].ToString()) * Convert.ToDecimal(0.74);//**for $ **//
            //            //dataTable3.Rows[i][11] = Convert.ToDecimal(dataTable3.Rows[i][11].ToString()) / Convert.ToDecimal(1.1);//**for $ **//
            //        }
            //        if (customer_code.Equals(customer_dtcli) && currency.Equals(s2))
            //        {
            //            string rupees = dataTable3.Rows[i][11].ToString();
            //            if (dataTable3.Rows[i][11] != null)
            //            {
            //                dataTable3.Rows[i][11] = Convert.ToDecimal(dataTable3.Rows[i][11].ToString()) * Convert.ToDecimal(0.02);//**for Rs.**//
            //                //dataTable3.Rows[i][11] = Convert.ToDecimal(dataTable3.Rows[i][11].ToString()) / Convert.ToDecimal(60);//**for Rs.**//
            //            }
            //            string rupees1 = dataTable3.Rows[i][11].ToString();
            //        }
            //        if (customer_code.Equals(customer_dtcli) && currency.Equals(s3))
            //        {
            //            if (dataTable3.Rows[i][11] != null)
            //                dataTable3.Rows[i][11] = Convert.ToDecimal(dataTable3.Rows[i][11].ToString()) * Convert.ToDecimal(1);//**for Euro**//
            //        }
            //    }
            //}



            dataTable3.AcceptChanges();
            GridView1.DataSource = dataTable3;
            GridView1.DataBind();

            GridView2.DataSource = dataTable3;
            GridView2.DataBind();
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            if (GridView2.Rows.Count > 0)
            {
                ExportToExcel();
            }
            else
            {
                string script = "alert('Nothing To Download!!!')";
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(btnExcel, this.GetType(), "Test", script, true);
            }
        }
}
}