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
    int i;
    String MaND = User.Identity.Name;
    String ParentID = "PhanBo";
    String ChiNganSach = Request.QueryString["ChiNganSach"];
    String iID_MaChiTieu = Request.QueryString["iID_MaChiTieu"];
    DataTable dtTrangThai_All = LuongCongViecModel.Get_dtDSTrangThaiDuyet(PhanBoModels.iID_MaPhanHePhanBo);
    DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(MaND);

    String page = Request.QueryString["page"];
    int CurrentPage = 1;
    if (String.IsNullOrEmpty(page) == false)
    {
        CurrentPage = Convert.ToInt32(page);
    }

    DataTable dt = Connection.GetDataTable(String.Format("SELECT * FROM PB_PhanBo WHERE bPhanBoTong=1 AND iNamLamViec={0}", NguoiDungCauHinhModels.iNamLamViec));
    //String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new { iID_MaChiTieu = iID_MaChiTieu, MaND = MaND, page = x }));
    String strThemMoi = Url.Action("Edit", "PhanBo_PhanBo", new { iID_MaChiTieu = iID_MaChiTieu});
    
%>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Danh sách phân bổ</span>
                </td>
                <td align="right" style="padding-right: 10px;">
                    <%--<input id="Button1" type="submit" class="button_title" value="Trình duyệt" onclick="javascript:location.href=''" />--%>
                </td>
            </tr>
        </table>
    </div>
    <table class="mGrid" id="<%= ParentID %>_thList">
        <tr>
            <th style="width: 5%;" align="center">STT</th>
            <th style="width: 15%;" align="center">Ngày chứng từ</th>
            <th style="width: 15%;" align="center">Số chứng từ</th>
            <th style="width: 20%;" >Nội dung</th>
            <th style="width: 15%;" align="center">Trạng thái</th>
            <th style="width: 5%;" align="center">Chi tiết</th>
            <th style="width: 5%;" align="center">Sửa</th>
            <%--<th style="width: 5%;" align="center">Xóa</th>--%>
        </tr>
        <%
    for (i = 0; i < dt.Rows.Count; i++)
    {
            DataRow R = dt.Rows[i];
            
            String strEdit = "";
            String strDelete = "";
            String classtr = "";
            String NgayChungTu = "";
            String sTrangThai = "";
            String strColor = "";
            String strURL = "";
            String strNoiDung = "";
            string strDetail = MyHtmlHelper.ActionLink(Url.Action("TaoMoi", "PhanBo_PhanBoChiTiet", new { iID_MaChiTieu = iID_MaChiTieu }).ToString(), "Tạo mới", "Detail", ""); ;
            int STT = i + 1;
            if (R != null)
            {
                strNoiDung = Convert.ToString( R["sNoiDung"]);
                NgayChungTu = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayChungTu"]));
                sTrangThai = "";
                strColor = "";
                for (int j = 0; j < dtTrangThai_All.Rows.Count; j++)
                {
                    if (Convert.ToString(R["iID_MaTrangThaiDuyet"]) == Convert.ToString(dtTrangThai_All.Rows[j]["iID_MaTrangThaiDuyet"]))
                    {
                        sTrangThai = Convert.ToString(dtTrangThai_All.Rows[j]["sTen"]);
                        strColor = String.Format("style='background-color: {0}; background-repeat: repeat;'", dtTrangThai_All.Rows[j]["sMauSac"]);
                        break;
                    }
                }
                if (LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanBoModels.iID_MaPhanHePhanBo, MaND) &&
                                LuongCongViecModel.KiemTra_TrangThaiKhoiTao(PhanBoModels.iID_MaPhanHePhanBo, Convert.ToInt32(R["iID_MaTrangThaiDuyet"])))
                {
                    strEdit = MyHtmlHelper.ActionLink(Url.Action("Edit", "PhanBo_Tong", new { iID_MaPhanBo = R["iID_MaPhanBo"]}), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "");                    
                }
                strEdit = MyHtmlHelper.ActionLink(Url.Action("Edit", "PhanBo_Tong", new { iID_MaPhanBo = R["iID_MaPhanBo"] }), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "");
                strURL = MyHtmlHelper.ActionLink(Url.Action("Detail", "PhanBo_Tong", new { iID_MaPhanBo = R["iID_MaPhanBo"] }).ToString(), "<img src='../Content/Themes/images/btnSetting.png' alt='' />", "Detail", null, "title=\"Xem chi tiết chứng từ\"");
            }
            
                       
            %>
            <tr <%=strColor %>>
                <td align="center"><%=STT%></td>            
                <td align="center"><%=NgayChungTu %></td>
                <td><%=Convert.ToString(R["sTienToChungTu"]) + Convert.ToString(R["iSoChungTu"])%></td>
                <td align="left"><%=strNoiDung%></td>
                <td align="center"><%=sTrangThai %></td>
                <td align="center">
                    <%=strURL %>
                </td>
                <td align="center">
                    <%=strEdit%>                   
                </td>
                <%--<td align="center">
                    <%=strDelete%>                                       
                </td>--%>
            </tr>
        <%} %>
     <%--   <tr class="pgr">
            <td colspan="8" align="right">
                <%=strPhanTrang%>
            </td>
        </tr>--%>
    </table>
</div>
<%
dt.Dispose();
dtTrangThai_All.Dispose();
dtDonVi.Dispose();
%>
</asp:Content>



