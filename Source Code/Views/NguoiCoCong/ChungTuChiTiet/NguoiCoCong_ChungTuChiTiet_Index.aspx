﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script src="<%= Url.Content("~/Scripts/jsBang_NguoiCoCong.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_Editable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_KeyTable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<script src="<%= Url.Content("~/Scripts/jsBang_Data.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<script src="<%= Url.Content("~/Scripts/jsBang.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<%
String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
if (String.IsNullOrEmpty(iID_MaChungTu)) iID_MaChungTu = Convert.ToString(ViewData["iID_MaChungTu"]);
NameValueCollection data = NguoiCoCong_ChungTuModels.LayThongTin(iID_MaChungTu);
String strTenDonVi = DonViModels.Get_TenDonVi(data["iID_MaDonVi"]);
int iLoai = Convert.ToInt32(data["iLoai"]);
%>
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
                <%
                switch (Convert.ToInt32(iLoai)) { 
                    case 1:
                        %>
                        <%=MyHtmlHelper.ActionLink(Url.Action("Index", "NguoiCoCong_ChungTu", new { iLoai = iLoai}), "Chứng từ người có công")%> |      
                        <%
                        break;
                    case 2:
                        %>
                        <%=MyHtmlHelper.ActionLink(Url.Action("Index", "NguoiCoCong_ChungTu", new { iLoai = iLoai}), "Chứng từ trợ cấp khó khăn")%> |      
                        <%
                        break;
                }    
                %>  
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "NguoiCoCong_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu }), "Chi tiết chứng từ")%>
            </div>
        </td>
    </tr>
</table>
<div style="width: 100%; float: left;">
    <div style="width: 100%; float:left;">
        <div class="box_tong">
            <div class="title_tong">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td>
                            <span>Thông tin chứng từ</span>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="nhapform">
                <div id="form2">
                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td style="width: 50%;" valign="top">
                                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form3">
                                    <tr>
                                        <td class="td_form2_td1"><div><b>Số chứng từ</b></div></td>
                                        <td class="td_form2_td5"><div><b><%=data["sTienToChungTu"]%><%=data["iSoChungTu"]%></b></div></td>
                                    </tr>
                                    <tr>
                                        <td class="td_form2_td1"><div><b>Thời gian</b></div></td>
                                        <td class="td_form2_td5"><div><b>
                                        <%
                                            String strThoiGianQuyetToan = "";
                                            switch (Convert.ToInt32(data["bLoaiThang_Quy"]))
                                            {
                                                case 0:
                                                    strThoiGianQuyetToan = "Tháng: " + Convert.ToString(data["iThang_Quy"]) + "/" + Convert.ToString(data["iNamLamViec"]);
                                                    break;
                                                case 1:
                                                    strThoiGianQuyetToan = "Quý: " + Convert.ToString(Convert.ToInt32(data["iThang_Quy"])/3) + "/" + Convert.ToString(data["iNamLamViec"]);
                                                    break;
                                                case 2:
                                                    strThoiGianQuyetToan = "Năm: " + Convert.ToString(data["iThang_Quy"]) + "/" + Convert.ToString(data["iNamLamViec"]);
                                                    break;
                                            }    
                                        %>
                                        <%=strThoiGianQuyetToan %>
                                        </b></div></td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 50%;" valign="top">
                                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form3">
                                    <tr>
                                        <td class="td_form2_td1" style="width: 15%">
                                            <div><b>Ngày chứng từ</b></div>
                                        </td>
                                        <td class="td_form2_td5"><div><b>
                                            <%=String.Format("{0:dd/MM/yyyy}",Convert.ToDateTime(data["dNgayChungTu"]))%></b></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_form2_td1"><div><b>Đơn vị</b></div></td>
                                        <td class="td_form2_td5"><div><b><%=strTenDonVi%></b></div></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form3">
                                    <tr>
                                        <td class="td_form2_td1_left" align="left"><div><b>Nội dung chứng từ: <%=data["sNoiDung"]%></b></div></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>                    
                    <%Html.RenderPartial("~/Views/NguoiCoCong/ChungTuChiTiet/NguoiCoCong_ChungTuChiTiet_Index_DanhSach.ascx", new { ControlID = "ChungTuChiTiet", MaND = User.Identity.Name }); %>    
                </div>
            </div>
        </div>
    </div>
</div>    
</asp:Content>



