﻿<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 92px;
        }
        .style2
        {
            height: 10px;
            width: 92px;
        }
    </style>
</head>
<body>
    <% 
        String ParentID = "QuyetToan";
        String MaND = User.Identity.Name;
        String iNamLamViec = ReportModels.LayNamLamViec(MaND);
        
        //lấy danh sách quý
        String iThang_Quy = Convert.ToString(ViewData["iThang_Quy"]);
        if (String.IsNullOrEmpty(iThang_Quy))
        {
            ReportModels.LayQuyHienTai();
        }
        
        DataTable dtQuy = DanhMucModels.DT_Quy_QuyetToan();
        DataRow R1 = dtQuy.NewRow();
        R1["MaQuy"] = "5";
        R1["TenQuy"] = "Bổ sung";
        dtQuy.Rows.Add(R1);
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
        dtQuy.Dispose();
        
        //lấy năm
        String iID_MaNamNganSach = Convert.ToString(ViewData["iID_MaNamNganSach"]);
        DataTable dtNamNganSach = QuyetToanModels.getDSNamNganSach();
        SelectOptionList slNamNganSach = new SelectOptionList(dtNamNganSach, "MaLoai", "sTen");
        dtNamNganSach.Dispose();
        
        //lấy danh sách phòng ban
        String iID_MaPhongBan = Convert.ToString(ViewData["iID_MaPhongBan"]);
        DataTable dtPhongBan = QuyetToanModels.getDSPhongBan(iNamLamViec, MaND);
        SelectOptionList slPhongBan = new SelectOptionList(dtPhongBan, "iID_MaPhongBan", "sTenPhongBan");
        dtPhongBan.Dispose();

        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        if (String.IsNullOrEmpty(PageLoad))
        {
            PageLoad = "0";
        }
        
        String Chuoi = "";
        String View = "";
        
        if (PageLoad == "1")
        {
            View = String.Format(@"/rptQuyetToanTongHopNhapSoLieu/viewpdf?iThang_Quy={0}&iID_MaNamNganSach={1}&iID_MaPhongBan={2}&MaND={3}",
                                            iThang_Quy, iID_MaNamNganSach, iID_MaPhongBan, MaND);
            Chuoi += View;
        }
        
        String BackURL = Url.Action("Index", "QuyetToan_Report", new { Loai = 0 });

        using (Html.BeginForm("FormSubmit", "rptQuyetToanTongHopNhapSoLieu", new { ParentID = ParentID }))
        {
        %>
        <%=MyHtmlHelper.Hidden(ParentID, MaND, "MaND", "")%>
        <div class="box_tong">
            <div class="title_tong">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td>
                            <span>Báo cáo tổng hợp nhập số liệu
                                <%=iNamLamViec%></span>
                        </td>
                        <td width="52%" style="text-align: left;">
                            <div class="login1" style="width: 50px; height: 20px; text-align: left;">
                                <a style="cursor: pointer;"></a>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="rptMain" style="background-color: #F0F9FE;">
                <div id="Div2" style="margin-left: 10px;" class="table_form2">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="td_form2_td1" style="width: 10%; height: 10px">
                                <div>
                                    <b>Chọn quý </b></div>
                            </td>
                            <td style="width: 10%">
                                <div>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slQuy, iThang_Quy, "iThang_Quy", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                </div>
                            </td>
                            <td class="td_form2_td1" style="width: 10%; height: 20px">
                                <div>
                                    <b>Chọn năm  </b></div>
                            </td>
                            <td style="width: 10%">
                                <div>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slNamNganSach, iID_MaNamNganSach, "iID_MaNamNganSach", "", "class=\"input1_2\" style=\"width:100%;\")")%>
                                </div>
                            </td>
                            <td class="td_form2_td1" style="width: 10%; height: 20px">
                                <div>
                                    <b>Chọn phòng ban  </b></div>
                            </td>
                            <td style="width: 10%">
                                <div>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slPhongBan, iID_MaPhongBan, "iID_MaPhongBan", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                </div>
                            </td>
                            <td style="width: 10%">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                       
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        
                        </tr>
                       
                        <tr style="text-align: center;">
                            <td colspan="6" align="center">
                                <table cellpadding="0" cellspacing="0" border="0" align="center">
                                    <tr>
                                        <td>
                                            <input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                        </td>
                                        <td width="5px">
                                        </td>
                                        <td>
                                            <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <script type="text/javascript">
                $(document).ready(function () {
                   $('.title_tong a').click(function () {
                    $('div#rptMain').slideToggle('normal');
                    $(this).toggleClass('active');
                    return false;
                });
                var chuoi = '<%=Chuoi%>';
                var pageLoad = <%=PageLoad %>;
                   if(pageLoad=="1"){
                         window.open(chuoi, '_blank');
                   }
               });
            </script>
            <script type="text/javascript">
                function Huy() {
                    window.location.href = '<%=BackURL%>';
                }
            </script>
        </div>
        <%} %>
</body>
</html>
