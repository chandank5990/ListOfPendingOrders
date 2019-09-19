<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" UICulture="en-US"
    Culture="en-US" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Page2.aspx.cs"
    Inherits="List_Of_Pending_Orders.Page2" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajax" %>
    <style type="text/css">
        .style1
        {
            text-align: center;
            color: #000000;
            font-size: x-large;
        }
        .style3
        {
            font-weight: bold;
            color: #000000;
            text-align: center;
        }
        
        .grid-sltrow
        {
            background: #ddd;
            font-weight: bold;
        }
        .SubTotalRowStyle
        {
            background-color: #ffffff;
            font-weight: bold;
        }
        .GrandTotalRowStyle
        {
            background-color: #ffffff;
            color: #000000;
            font-weight: bold;
        }
        .GroupHeaderStyle
        {
            background-color: #ffffff;
            color: #000000;
            font-weight: bold;
        }
        .serh-grid
        {
            width: 85%;
            border: 1px solid #6AB5FF;
            background: #fff;
            line-height: 14px;
            font-size: 11px;
            font-family: Verdana;
        }
        .hidden-field
        {
            display: none;
        }
        .divWaiting
        {
            position: absolute;
            background-color: #FAFAFA;
            z-index: 2147483647 !important;
            opacity: 0.8;
            overflow: hidden;
            text-align: center;
            top: 0;
            left: 0;
            height: 100%;
            width: 100%;
            padding-top: 20%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="width: 100%; height: auto; border-color: #006699;">
        <div class="style1">
            <strong><em>List Of Pending Orders
            </em></strong>
        </div>
         <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </ajax:ToolkitScriptManager>
        <asp:Panel ID="Panel1" runat="server" Style="text-align: justify; margin-left: 55px;
            margin-top: 50px; margin-bottom: 500px;">
            <asp:Label ID="Label1" runat="server" Text="Label" CssClass="style3">From Date&nbsp;&nbsp;&nbsp;</asp:Label>
            <asp:TextBox ID="TextBox1" runat="server" CssClass="style3" Placeholder="Enter From Date"
                Text="01/01/2017"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;
            <asp:Label ID="Label2" runat="server" Text="Label" CssClass="style3">To Date</asp:Label>&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="TextBox2" runat="server" CssClass="style3" Placeholder="Enter To Date"></asp:TextBox>&nbsp;&nbsp;&nbsp;
           <div style="margin-left:510px;margin-top:-23px;">
                <ContentTemplate>
                    <asp:Button ID="Button1" runat="server" Text="Submit" CssClass="style3" OnClick="Button1_Click" />&nbsp;
                    <asp:Button ID="Button2" runat="server" CssClass="style3" Text="AWS-1" 
                        OnClick="Button2_Click" />
                    &nbsp;
                    <asp:Button ID="btnAWS2" runat="server" Text="AWS-2" CssClass="style3" 
                        onclick="btnAWS2_Click" />
                    &nbsp;
                    <asp:CheckBox ID="CheckBox1" runat="server" Style="font-weight: 700; color: #000000"
                        Text="@Call Off" />
                        <asp:Button ID="btnExcel" runat="server" onclick="btnExcel_Click" 
                    style="z-index: 1; left: 965px; top: 76px; position: absolute" Text="Download" 
                        Font-Bold="True" />
                </ContentTemplate>
            </div>
        </asp:Panel>
        &nbsp;&nbsp;&nbsp;
        <div style="margin-left: 0;">
                <ContentTemplate>
                    <asp:GridView ID="GridView1" runat="server" CellSpacing="2" Style="position: static;
                        z-index: 1; margin-left: -9px; margin-top: -480px; margin-bottom: 20px; height: 218px;
                        width: 960px; color: Black; text-align: center;" AutoGenerateColumns="False"
                        ShowFooter="True" ShowHeaderWhenEmpty="True" OnRowDataBound="GridView1_RowDataBound"
                        OnRowCommand="GridView1_RowCommand" OnRowCreated="GridView1_RowCreated">
                        <Columns>
                            <asp:BoundField DataField="PinOrd" HeaderText="Order No." ReadOnly="True" />
                            <asp:BoundField DataField="FecPed" HeaderText="Order.Date" DataFormatString="{0:dd/MM/yyyy}"
                                ReadOnly="True" />
                            <asp:BoundField DataField="EntOrd" HeaderText="Delivery.Date" DataFormatString="{0:dd/MM/yyyy}"
                                ReadOnly="True" />
                            <asp:TemplateField HeaderText="...UID...">
                                <EditItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("NumOrd") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lnkView" OnClick="lnkView_Click" Text='<%# Bind("NumOrd") %>'
                                        autopostback="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ArtOrd" HeaderText="......Article......" ReadOnly="True" />
                            <asp:BoundField DataField="R" HeaderText="" ReadOnly="True" />
                            <asp:BoundField DataField="NomArt" HeaderText="Description" ReadOnly="True" />
                            <asp:BoundField DataField="Pending" HeaderText="Qty" FooterText="" ReadOnly="True" />
                            <asp:BoundField DataField="PreOrd" HeaderText="Price" ReadOnly="True" />
                            <asp:BoundField DataField="PedPed" HeaderText="Cust.Ord.No" HtmlEncode="False" DataFormatString="{0:D19}"
                                SortExpression="Cust.Ord.No" ReadOnly="True" />
                            <asp:BoundField DataField="PieOrd" HeaderText="" ReadOnly="True">
                                <ItemStyle CssClass="hidden-field" />
                            </asp:BoundField>
                            <asp:BoundField DataField="EntCli" HeaderText="Vname" ReadOnly="True">
                                <ItemStyle CssClass="hidden-field" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Datos" HeaderText="G_Remarks" ReadOnly="True" />
                            <asp:BoundField DataField="NomPro" HeaderText="" ReadOnly="True" />
                            <asp:BoundField DataField="GeartRemarks" HeaderText="" ReadOnly="True" />
                        </Columns>
                        <HeaderStyle BackColor="#ffffff" BorderStyle="Groove" />
                        <EmptyDataTemplate>
                            No Record Available</EmptyDataTemplate>
                        <RowStyle BackColor="#EFF3FB" HorizontalAlign="Center" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
                </ContentTemplate>
        </div>
    </div>
    <asp:Panel ID="Panel2" runat="server" BackColor="White" 
            
            style="z-index: 1; left: 317px; top: 2310px; position: absolute; position:static; margin-left: 10px;" 
            ScrollBars="Auto" Height="270px" Width="1000px" Visible="false">
                    <asp:GridView ID="GridView2" runat="server" CellSpacing="2" Style="
                        z-index: 1; margin-left: 30px; margin-bottom: 20px; height: 218px;
                        width: 900px; color: Black; text-align: center;" AutoGenerateColumns="False"
                        ShowFooter="True" ShowHeaderWhenEmpty="True" OnRowDataBound="GridView1_RowDataBound"
                        OnRowCommand="GridView1_RowCommand" >
                        <Columns>
                            <asp:BoundField DataField="PinOrd" HeaderText="Order No." ReadOnly="True" />
                            <asp:BoundField DataField="FecPed" HeaderText="Order.Date" DataFormatString="{0:dd/MM/yyyy}"
                                ReadOnly="True" />
                            <asp:BoundField DataField="EntOrd" HeaderText="Delivery.Date" DataFormatString="{0:dd/MM/yyyy}"
                                ReadOnly="True" />
                            <asp:BoundField DataField="NumOrd" HeaderText="UID" ReadOnly="True" />
                            <asp:BoundField DataField="ArtOrd" HeaderText="......Article......" ReadOnly="True" />
                            <asp:BoundField DataField="R" HeaderText="" ReadOnly="True" />
                            <asp:BoundField DataField="NomArt" HeaderText="Description" ReadOnly="True" />
                            <asp:BoundField DataField="Pending" HeaderText="Qty" FooterText="" ReadOnly="True" />
                            <asp:BoundField DataField="PreOrd" HeaderText="Price" ReadOnly="True" />
                            <asp:BoundField DataField="PedPed" HeaderText="Cust.Ord.No" HtmlEncode="False" DataFormatString="{0:D19}"
                                SortExpression="Cust.Ord.No" ReadOnly="True" />
                            <asp:BoundField DataField="PieOrd" HeaderText="" ReadOnly="True">
                                <ItemStyle CssClass="hidden-field" />
                            </asp:BoundField>
                            <asp:BoundField DataField="EntCli" HeaderText="" ReadOnly="True">
                                <ItemStyle CssClass="hidden-field" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Datos" HeaderText="Datos" ReadOnly="True" />
                            <asp:BoundField DataField="NomPro" HeaderText="Vname" ReadOnly="True" />
                            <asp:BoundField DataField="Observaciones" HeaderText="Observaciones" ReadOnly="True" />
                            <asp:BoundField DataField="GeartRemarks" HeaderText="G_Remarks" ReadOnly="True" />
                        </Columns>
                        <HeaderStyle BackColor="#ffffff" BorderStyle="Groove" />
                        <EmptyDataTemplate>
                            No Record Available</EmptyDataTemplate>
                        <RowStyle BackColor="#EFF3FB" HorizontalAlign="Center" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
            </asp:Panel>
    <ajax:CalendarExtender ID="TextBox1_CalendarExtender" runat="server" Enabled="True"
        TargetControlID="TextBox1" Format="dd/MM/yyyy">
    </ajax:CalendarExtender>
    <ajax:CalendarExtender ID="TextBox2_CalendarExtender" runat="server" Enabled="True"
        TargetControlID="TextBox2" Format="dd/MM/yyyy">
    </ajax:CalendarExtender>
</asp:Content>
