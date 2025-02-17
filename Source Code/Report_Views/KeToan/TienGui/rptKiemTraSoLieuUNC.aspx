﻿<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.TienGui" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="Stylesheet" type="text/css" href="../../../Content/Report_Style/ReportCSS.css" />
    <style type="text/css">
         ul.inlineBlock li fieldset legend{text-align:left; padding:3px 6px;font-size:13px;}  
        ul.inlineBlock li fieldset p{padding:2px 4px; text-align:left;}
    </style>
    <script type="text/javascript" language="javascript" src="../../../Scripts/jquery-latest.js"></script>
</head>
<body>
    <% 
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "KeToan";
        String Nam = DateTime.Now.Year.ToString();
        String TuNgay = Convert.ToString(ViewData["TuNgay"]);
        String DenNgay =Convert.ToString(ViewData["DenNgay"]);
        String TuThang = Convert.ToString(ViewData["TuThang"]);
        String DenThang = Convert.ToString(ViewData["DenThang"]);
        String sSoChungTuGhiSo =Convert.ToString(ViewData["sSoChungTuGhiSo"]);
        String iNamLamViec = Convert.ToString(DateTime.Now.Year);
        DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
        iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
        String iTuThang = DateTime.Now.Month.ToString();
        String iDenThang = DateTime.Now.Month.ToString();
        String OnSuccess = "";
        OnSuccess = Request.QueryString["OnSuccess"];
        if (String.IsNullOrEmpty(TuNgay)) TuNgay = "1";
        if (String.IsNullOrEmpty(DenNgay)) DenNgay = "1";
        if (String.IsNullOrEmpty(TuThang)) TuThang = "1";
        if (String.IsNullOrEmpty(DenThang)) DenThang = "2";
        //lay nam lam viec
        //var objNguoidung = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
        //if (objNguoidung.Rows.Count > 0)
        //{
        //    DataRow dr = objNguoidung.Rows[0];
        //    iTuThang = iDenThang = HamChung.ConvertToString(dr["iThangLamViec"]);
        //    iNamLamViec = HamChung.ConvertToString(dr["iNamLamViec"]);
        //}
        ////ngay       
        //if (String.IsNullOrEmpty(TuNgay) == true)
        //{
        //    TuNgay = "1";
        //}
        //if (String.IsNullOrEmpty(TuThang) == true || TuThang == "")
        //{
        //    TuThang = iTuThang;
        //}
        //if (String.IsNullOrEmpty(DenThang) == true)
        //{
        //    //DenThang = TuThang;
        //    DenThang = iDenThang;
        //}
        //if (String.IsNullOrEmpty(DenNgay) == true)
        //{
        //    DenNgay = HamChung.GetDaysInMonth(Convert.ToInt32(DenThang), Convert.ToInt32(Nam)).ToString();
        //}
        ////tháng     
        //var dtThang = HamChung.getMonth(DateTime.Now, false, "", "");
        //SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        //if (dtThang != null) dtThang.Dispose();

        /////ngày
        //var dtNgay = HamChung.getDaysInMonths(DateTime.Now.Month, DateTime.Now.Year, false, "", "");
        //SelectOptionList slNgay = new SelectOptionList(dtNgay, "MaNgay", "TenNgay");
        //if (dtNgay != null) dtNgay.Dispose();
        String BackURL = Url.Action("Index", "KeToanChiTietTienGui");
        //Kieu giay
        String Loai = Request.QueryString["Loai"];
        //DataTable dtKieuGiay = VIETTEL.Report_Controllers.KeToan.TienGui.rptKiemTraSoLieuUNCController.dtLoaiBangKe();
        //SelectOptionList slKieuGiay = new SelectOptionList(dtKieuGiay, "MaBangKe", "TenBangKe");
        //if (String.IsNullOrEmpty(Loai))
        //{
        //    Loai = "0";
        //}
        //dtKieuGiay.Dispose();
        ////SoCT
        //DataTable dtSoCT = rptKiemTraSoLieuUNCController.dtSoChungTu(iNamLamViec, TuThang, DenThang);
        //SelectOptionList slSoCT = new SelectOptionList(dtSoCT, "sSoChungTuGhiSo", "sSoChungTuGhiSo");
        //if (String.IsNullOrEmpty(sSoChungTuGhiSo))
        //{
        //    if (dtSoCT.Rows.Count > 0)
        //    {
        //        sSoChungTuGhiSo = Convert.ToString(dtSoCT.Rows[0]["sSoChungTuGhiSo"]);
        //    }
        //    else
        //    {
        //        sSoChungTuGhiSo = "";
        //    }
        //}
        //DataTable dtTrangThai = HamChung.GetTrangThai(PhanHeModels.iID_MaPhanHeKeToanTongHop, LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop), true, "--Tất cả--");
        //SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
        String iTrangThai = Request.QueryString["iID_MaTrangThaiDuyet"];
        //if (String.IsNullOrEmpty(iTrangThai))
        //    iTrangThai = dtTrangThai.Rows.Count > 0 ? Convert.ToString(dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"]) : Guid.Empty.ToString();
        using (Html.BeginForm("EditSubmit", "rptKiemTraSoLieuUNC", new { ParentID = ParentID, iNamLamViec = iNamLamViec }))
        {
    %>
    <div class="box_tong">
        <%--<div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td width="47.9%">
                        <span>Báo cáo Kiểm tra số liệu</span>
                    </td>
                    <td width="52%" style="text-align: left;">
                        <div class="login1" style="width: 50px; height: 20px; text-align: left;">
                            <a style="cursor: pointer;"></a>
                        </div>
                    </td>
                </tr>
            </table>
        </div>--%>
        <div id="table_form2" class="table_form2">
            <div id="rptMain" style="width: 100%; margin: 0px auto; padding: 0px 0px; overflow: visible; text-align:center;">                
               <%-- <ul class="inlineBlock">
                    <li>
                        <fieldset>
                            <legend><%=NgonNgu.LayXau("Khoảng thời gian cần xem") %></legend>
                            <p><%=NgonNgu.LayXau("Từ ngày&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Tháng&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Đến ngày&nbsp;&nbsp;&nbsp;&nbsp;Tháng")%></p>
                            <p style="margin-bottom:10px;">
                             <label id="td_TuNgay">
                                <%=MyHtmlHelper.DropDownList(ParentID, slNgay, TuNgay, "TuNgay", "", "class=\"input1_2\"style=\"width: 60px;\"")%> </label>
                             <label>   <%=MyHtmlHelper.DropDownList(ParentID, slThang, TuThang, "TuThang", "", "class=\"input1_2\" style=\"width: 60px;\"onchange=\"ChonThang(this.value,'TuNgay')\"")%>
                             </label>
                               <label id="td_DenNgay"> <%=MyHtmlHelper.DropDownList(ParentID, slNgay, DenNgay, "DenNgay", "", "class=\"input1_2\" style=\"width: 60px;\"")%>
                              </label> 
                               <label> <%=MyHtmlHelper.DropDownList(ParentID, slThang, DenThang, "DenThang", "", "class=\"input1_2\" style=\"width: 60px;\"onchange=\"ChonThang(this.value,'DenNgay')\"")%>
                            </label>
                            </p>
                        </fieldset>
                    </li>
                     <li>
                        <fieldset style=" height:80px">
                             <legend>
                            <%=NgonNgu.LayXau("Kiểu in:") %></legend>
                        <div style=" text-align:left; padding-bottom: 7px;">
                            <%=MyHtmlHelper.Option(ParentID, "0", Loai, "Loai", "")%><span style="font-size: 13px;
                                font-weight: bold;">1. Tất cả</span></div>
                        <div style=" text-align:left; padding-bottom: 5px;">
                            <%=MyHtmlHelper.Option(ParentID, "1", Loai, "Loai", "")%><span style="font-size: 13px;
                                font-weight: bold;">2. Riêng số ghi sổ</span>
                             <span id="divDonVi">
                             <%=MyHtmlHelper.DropDownList(ParentID, slSoCT, sSoChungTuGhiSo, "sSoChungTuGhiSo", "", "class=\"input1_2\" style=\"width: 60px;\" size='1' tab-index='-1' onchange=\"chonSo()\"")%>
                             </span>   
                        </div>
                        </fieldset>
                    </li>
                     <li>
                        <fieldset>
                            <legend><%=NgonNgu.LayXau("Chọn trạng thái:") %></legend>
                            <p>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iTrangThai, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 150px;\" size='3' tab-index='-1'")%>
                            </p>
                        </fieldset>
                    </li>
                </ul>--%>
                <p style="text-align:center; padding:4px;"><input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" style="display:inline-block; margin-right:5px;" /><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>"  onclick="Huy()" style="display:inline-block; margin-left:5px;" /></p>                    
            </div>
        </div>
    </div>
    <%} %>
  
    <script type="text/javascript">
//        $(function () {
//            $('div.login1 a').click(function () {
//                $('div#rptMain').slideToggle('normal');
//                $(this).toggleClass('active');
//                return false;
//            });
//        });
        function Huy() {
            window.location.href = '<%=BackURL %>';
        }

//         function ChonThang(Thang,TenTruong) {            
//            var Ngay = document.getElementById("<%=ParentID %>_" + TenTruong).value;
//            jQuery.ajaxSetup({ cache: false });
//            var url = unescape('<%= Url.Action("Get_objNgayThang?ParentID=#0&TenTruong=#1&Ngay=#2&Thang=#3&iNam=#4", "rptKiemTraSoLieuUNC") %>');
//            url = unescape(url.replace("#0", "<%= ParentID %>"));
//            url = unescape(url.replace("#1", TenTruong));
//            url = unescape(url.replace("#2", Ngay));
//            url = unescape(url.replace("#3", Thang));
//            url = unescape(url.replace("#4", '<%=iNamLamViec %>'));
//                $.getJSON(url, function (data) {
//                document.getElementById("td_"+TenTruong).innerHTML = data;                 
//                chonSo();
//            });            
//        }       
//        function chonSo() {
//            var TuThang = document.getElementById("<%=ParentID %>_TuThang").value;
//            var DenThang = document.getElementById("<%=ParentID %>_DenThang").value;          
//            var sSoChungTuGhiSo = document.getElementById("<%=ParentID %>_sSoChungTuGhiSo").value;
//            jQuery.ajaxSetup({ cache: false });
//            var url = unescape('<%= Url.Action("ObjDanhSachDonVi?ParentID=#0&TuThang=#1&DenThang=#2&iNamLamViec=#3&sSoChungTuGhiSo=#4", "rptKiemTraSoLieuUNC") %>');
//            url = unescape(url.replace("#0", "<%=ParentID %>"));
//            url = unescape(url.replace("#1", TuThang));
//            url = unescape(url.replace("#2", DenThang));
//            url = unescape(url.replace("#3", '<%=iNamLamViec %>'));
//            url = unescape(url.replace("#4", "<%=sSoChungTuGhiSo %>"));
//            $.getJSON(url, function (data) {
//                document.getElementById('divDonVi').innerHTML = data;
//               
//            });
//        }
         
    </script>

    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptKiemTraSoLieuUNC", new { sSoChungTuGhiSo = sSoChungTuGhiSo, iNamLamViec = iNamLamViec, TuNgay = TuNgay, DenNgay = DenNgay, TuThang = TuThang, DenThang = DenThang, Loai = Loai, iTrangThai = iTrangThai }), "Export To Excel")%>
    <iframe src="<%=Url.Action("ViewPDF","rptKiemTraSoLieuUNC",new{sSoChungTuGhiSo = sSoChungTuGhiSo , iNamLamViec = iNamLamViec, TuNgay = TuNgay, DenNgay = DenNgay, TuThang = TuThang, DenThang = DenThang, Loai = Loai, iTrangThai = iTrangThai })%>"
        height="600px" width="100%"></iframe>
</body>
</html>
