﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    String ParentID = "Edit";
    String UserID = User.Identity.Name;
    String iLoai = Convert.ToString(ViewData["iLoai"]);
    String MaChungTu = Convert.ToString(ViewData["MaChungTu"]);
    String MaPhongBanNguoiDung = NganSach_HamChungModels.MaPhongBanCuaMaND(UserID);

    DataTable dtChungTu = QuyetToan_QuanSo_ChungTuModels.GetChungTu(MaChungTu);
    DataRow R;
    int iSoChungTu = 0;
    String sLNS = "", dNgayChungTu = "", sNoiDung = "", sLyDo = "", iID_MaTrangThaiDuyet = "", sThang = "", iID_MaDonVi = "";
    sThang = DateTime.Now.Month.ToString();
    if (dtChungTu.Rows.Count > 0)
    {
        R = dtChungTu.Rows[0];
        dNgayChungTu = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayChungTu"]));
        sNoiDung = Convert.ToString(R["sNoiDung"]);
        iSoChungTu = Convert.ToInt32(R["iSoChungTu"]);
        sLyDo = Convert.ToString(R["sLyDo"]);
        iID_MaTrangThaiDuyet = Convert.ToString(R["iID_MaTrangThaiDuyet"]);
        sThang = Convert.ToString(R["iThang_Quy"]);
        iID_MaDonVi = Convert.ToString(R["iID_MaDonVi"]);
    }
    else
    {
        dNgayChungTu = CommonFunction.LayXauNgay(DateTime.Now);
    }
    String strTienToChungChu = PhanHeModels.LayTienToChungTu(QuyetToanModels.iID_MaPhanHeQuyetToan);
    if (Convert.ToString(ViewData["DuLieuMoi"]) == "1")
    {
        iSoChungTu = QuyetToan_QuanSo_ChungTuModels.GetMaxChungTu() + 1;
    }
    dtChungTu.Dispose();

    if (String.IsNullOrEmpty(iID_MaDonVi))
    {
        iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
    }
    
    DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(UserID);
    SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
    dtDonVi.Dispose();

    DataTable dtThang = new DataTable();
    dtThang.Columns.Add("MaThang", typeof(String));
    dtThang.Columns.Add("TenThang", typeof(String));
    DataRow Row;
    for (int i = 0; i < 13; i++)
    {
        Row = dtThang.NewRow();
        dtThang.Rows.Add(Row);
        Row[0] = Convert.ToString(i);
        Row[1] = Convert.ToString(i);
    }
    SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
    dtThang.Dispose();

    using (Html.BeginForm("EditSubmit", "QuyetToan_QuanSo_ChungTu", new { ParentID = ParentID, MaChungTu = MaChungTu}))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
<%= Html.Hidden(ParentID + "_iLoai", iLoai)%>

<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td align="left" style="width: 9%;">
            <div style="padding-left: 22px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                <b><%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
            </div>         
        </td>
        <td align="left">
            <div style="padding-bottom: 5px; color:#ec3237;">
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%> |
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "QuyetToan_QuanSo_ChungTu"), "Chứng từ quyết toán")%>
            </div>
        </td>
    </tr>
</table>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td><span>
                    <%
                    if (ViewData["DuLieuMoi"] == "1")
                    {
                        %>
                        <%=NgonNgu.LayXau("Thêm mới chứng từ")%>
                        <%
                    }
                    else
                    {
                        %>
                        <%=NgonNgu.LayXau("Sửa thông tin chứng từ")%>
                        <%
                    }
                    %>&nbsp; &nbsp;
                </span></td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <div style="width: 60%; float: left;">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td class="td_form2_td1" style="width: 30%;">
                            <div><b>Số chứng từ</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <b><%=strTienToChungChu%><%=iSoChungTu%></b>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div><b>Đơn vị</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\"")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaDonVi")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div><b>Tháng</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, sThang, "iThang_Quy", "", "class=\"input1_2\" style=\"width:98%;\"")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_iThang_Quy")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div><b>Ngày chứng từ</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div><%=MyHtmlHelper.DatePicker(ParentID, dNgayChungTu, "dNgayChungTu", "", "class=\"input1_2\"")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayChungTu")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div><b>Nội dung</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div><%=MyHtmlHelper.TextArea(ParentID, sNoiDung, "sNoiDung", "", "class=\"input1_2\" style=\"height: 100px;\"")%></div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div><b>&nbsp;</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div><%= Html.ValidationMessage(ParentID + "_" + "err_ChuaCoThang0")%></div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1"></td>
                        <td class="td_form2_td5">
                            <div>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td width="65%" class="td_form2_td5">&nbsp;</td>   
                                        <td width="30%" align="right" class="td_form2_td5">
                                            <input type="submit" class="button" id="Submit1" value="Lưu" />
                                        </td>          
                                            <td width="5px">&nbsp;</td>          
                                        <td class="td_form2_td5">
                                            <input class="button" type="button" value="Hủy" onclick="history.go(-1)" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</div>
<%
    }       
%>
</asp:Content>



