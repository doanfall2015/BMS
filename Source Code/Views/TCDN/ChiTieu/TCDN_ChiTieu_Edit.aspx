﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        String iID_MaChiTieu = Convert.ToString(ViewData["iID_MaChiTieu"]);
        String iID_MaChiTieu_Cha = Convert.ToString(ViewData["iID_MaChiTieu_Cha"]);
        String ParentID = "Edit";
        String sKyHieu = "", sTen = "", iLoai = "", sThuyetMinh = "",sTT="",sCongThuc="";
        bool bLaHangCha = false;
        bool bLaTong = false;
        bool bLaText = false;
        //chi tiết chỉ tiêu nếu trong trường hợp sửa
        DataTable dt = TCDN_ChiTieuModels.Get_ChiTietChiTieu_Row(iID_MaChiTieu);
        if (dt.Rows.Count > 0 && iID_MaChiTieu != null && iID_MaChiTieu != "")
        {
            DataRow R = dt.Rows[0];
            sKyHieu = HamChung.ConvertToString(R["sKyHieu"]);
            sTen = HamChung.ConvertToString(R["sTen"]);
            iLoai = Convert.ToString(R["iLoai"]);
            bLaHangCha = Convert.ToBoolean(R["bLaHangCha"]);
            bLaTong = Convert.ToBoolean(R["bLaTong"]);
            bLaText = Convert.ToBoolean(R["bLaText"]);
            sThuyetMinh = Convert.ToString(R["sThuyetMinh"]);
            sTT = Convert.ToString(R["sTT"]);
            sCongThuc = Convert.ToString(R["sCongThuc"]);
        }
        String tgLaHangCha = "";
        String tgLaTong = "";
        String tgLaText = "";
        if (bLaHangCha == true)
        {
            tgLaHangCha = "on";
        }
        if (bLaTong == true)
        {
            tgLaTong = "on";
        }
        if (bLaText == true)
        {
            tgLaText = "on";
        }
        //lấy dữ liệu đưa vào Combobox
        //nhom tK
        DataTable dtNhomChiTieu = TCDN_ChiTieuModels.DT_LoaiChiTieu();
        SelectOptionList optLoaiHinh = new SelectOptionList(dtNhomChiTieu, "ID", "sTen");
        if (dtNhomChiTieu != null) dtNhomChiTieu.Dispose();
        
        using (Html.BeginForm("EditSubmit", "TCDN_ChiTieu", new { ParentID = ParentID, iID_MaChiTieu = iID_MaChiTieu }))
        {
    %>
    <%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
    <%= Html.Hidden(ParentID + "_iID_MaChiTieu_Cha", iID_MaChiTieu_Cha)%>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td style="width: 10%">
                <div style="padding-left: 20px; padding-top: 5px; padding-bottom: 5px; text-transform: uppercase;
                    color: #ec3237;">
                    <b>
                        <%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
                </div>
            </td>
            <td align="left">
                <div style="padding-top: 5px; padding-bottom: 5px; color: #ec3237;">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>
                    |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "TCDN_ChiTieu"), "Danh sách chỉ tiêu")%>
                </div>
            </td>
        </tr>
    </table>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                     <% if (ViewData["DuLieuMoi"] == "1")
                   {
                       %>
                	 <span>Nhập thông tin danh sách chỉ tiêu</span>
                    <% 
                   }
                   else
                   { %>
                    <span>Sửa thông tin danh sách chỉ tiêu</span>
                    <% } %>
                       
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <table cellpadding="5" cellspacing="5" width="50%">
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Mã chỉ tiêu</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%
                                String strReadonly = "";
                                if (ViewData["DuLieuMoi"] == "0") {
                                    strReadonly = "readonly=\"readonly\""; 
                                }    
                                %>
                                <%=MyHtmlHelper.TextBox(ParentID, sKyHieu, "sKyHieu", "", " " + strReadonly + " class=\"input1_2\" tab-index='-1'", 2)%><br />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Tên chỉ tiêu</b>&nbsp;<span  style="color:Red;">*</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sTen, "sTen", "", "class=\"input1_2\"", 2)%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_sTen")%>
                            </div>
                        </td>
                    </tr>
                     <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Công thức</b>&nbsp;</div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sCongThuc, "sCongThuc", "", "class=\"input1_2\"", 2)%><br />
                            </div>
                        </td>
                    </tr
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>STT</b>&nbsp;</div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sTT, "sTT", "", "class=\"input1_2\"", 2)%><br />
                            </div>
                        </td>
                    </tr>
                 <%--   <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Ký hiệu thuyết minh</b>&nbsp;</div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sThuyetMinh, "sThuyetMinh", "", "class=\"input1_2\"", 2)%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_sThuyetMinh")%>
                            </div>
                        </td>
                    </tr>--%>
                  <%--  <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Tính chất chỉ tiêu</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, optLoaiHinh, iLoai, "iLoai", null, "class=\"input1_2\"")%>
                            </div>
                        </td>
                    </tr>--%>
                    <tr>
                        <td class="td_form2_td1"><div><b>Là tổng</b></div></td>
                        <td class="td_form2_td5">
                            <div><%=MyHtmlHelper.CheckBox(ParentID, tgLaTong, "bLaTong", String.Format("value='{0}'", bLaHangCha))%></div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1"><div><b>In đậm</b></div></td>
                        <td class="td_form2_td5">
                            <div><%=MyHtmlHelper.CheckBox(ParentID, tgLaHangCha, "bLaHangCha", String.Format("value='{0}'", bLaHangCha))%></div>
                        </td>
                    </tr>
                     <tr>
                        <td class="td_form2_td1"><div><b>Là Text</b></div></td>
                        <td class="td_form2_td5">
                            <div><%=MyHtmlHelper.CheckBox(ParentID, tgLaText, "bLaText","")%></div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <br />
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td width="70%">
                &nbsp;
            </td>
            <td width="30%" align="right">
                <table cellpadding="0" cellspacing="0" border="0" align="right">
                    <tr>
                        <td>
                            <input type="submit" class="button" value="Lưu" />
                        </td>
                        <td width="5px">
                        </td>
                        <td>
                            <input type="button" class="button" value="Hủy" onclick="javascript:history.go(-1)" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <%
        } if (dt != null) { dt.Dispose(); };    
    %>
</asp:Content>
