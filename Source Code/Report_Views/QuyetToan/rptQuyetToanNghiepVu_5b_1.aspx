﻿<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QuyetToan" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 193px;
        }
        .style2
        {
            width: 200px;
        }
        .style3
        {
            width: 237px;
        }
        .style4
        {
            width: 96px;
        }
    </style>
    <script type="text/javascript" src="../../../Scripts/jquery-latest.js"></script>
</head>
<body>
      <%
        String ParentID = "QuyetToanNganSach";
        String MaND = User.Identity.Name;
          String Thang_Quy = Convert.ToString(ViewData["Thang_Quy"]);
        if (String.IsNullOrEmpty(Thang_Quy))
        {
            Thang_Quy = "1";
        }
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        DataTable dtThang = DanhMucModels.DT_Quy();
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaQuy", "TenQuy");
        dtThang.Dispose();
          //dt Trạng thái duyệt
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        DataTable dtTrangThai = rptQuyetToan_ThongTri_5_NganhController.tbTrangThai();
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "TenTrangThai");
        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
        {
            if (dtTrangThai.Rows.Count > 0)
            {
                iID_MaTrangThaiDuyet = Convert.ToString(dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"]);
            }
            else
            {
                iID_MaTrangThaiDuyet = Guid.Empty.ToString();
            }
        }
        dtTrangThai.Dispose();
          //dt Loại ngân sách
        String sLNS = Convert.ToString(ViewData["sLNS"]);
        String iID_MaPhongBan = NguoiDung_PhongBanModels.getMaPhongBan_NguoiDung(MaND);
        DataTable dtLNS = rptQuyetToanNghiepVu_5b_1Controller.DanhSachLNS(iID_MaPhongBan);
        SelectOptionList slLNS = new SelectOptionList(dtLNS, "sLNS", "TenHT");
        if (String.IsNullOrEmpty(sLNS))
        {
            if (dtLNS.Rows.Count > 0)
            {
                sLNS = Convert.ToString(dtLNS.Rows[0]["sLNS"]);
            }
            else
            {
                sLNS = Guid.Empty.ToString();
            }
        }
        String[] arrLNS = sLNS.Split(',');
        dtLNS.Dispose();
        String LoaiIn = Convert.ToString(ViewData["LoaiIn"]);
        if (String.IsNullOrEmpty(LoaiIn))
        {
            LoaiIn = "ChiTiet";
        }
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
         if(String.IsNullOrEmpty(iID_MaDonVi))
         {
             PageLoad = "0";
         }
        DataTable dtDonVi = rptQuyetToanNghiepVu_5b_1Controller.DanhSachDonVi(Thang_Quy, sLNS, iID_MaTrangThaiDuyet, MaND);
        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
        //if (String.IsNullOrEmpty(iID_MaDonVi))
        //{
        //    if (dtDonVi.Rows.Count > 0)
        //    {
        //        iID_MaDonVi = dtDonVi.Rows[0]["sTen"].ToString();
        //    }
        //    else
        //    {
        //        iID_MaDonVi = Guid.Empty.ToString();
        //    }
        //}
        dtDonVi.Dispose();
        String[] arrMaDonVi;
        String MaDonVi = "-111";//= arrMaDonVi[ChiSo];
        int ChiSo = 0;
            if (String.IsNullOrEmpty(iID_MaDonVi) == false)
        {
            arrMaDonVi = iID_MaDonVi.Split(',');

            ChiSo = Convert.ToInt16(Request.QueryString["ChiSo"]);
            if(ChiSo==arrMaDonVi.Length)
            {
                ChiSo = 0;
            }
            if (ChiSo <= arrMaDonVi.Length - 1)
            {
                MaDonVi = arrMaDonVi[ChiSo];
                ChiSo = ChiSo + 1;
            }
            else
            {
                ChiSo = 0;
                MaDonVi = arrMaDonVi[0];
            }
        }
        else
        {
            iID_MaDonVi = "-111";
        }
        String BackURL = Url.Action("Index", "QuyetToan_Report", new { Loai = 1 });
      
        String urlReport = "";

        if (PageLoad.Equals("1"))
        {
            urlReport = Url.Action("ViewPDF", "rptQuyetToanNghiepVu_5b_1", new { Thang_Quy = Thang_Quy, sLNS = sLNS, iID_MaDonVi = MaDonVi, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, LoaiIn = LoaiIn, ChiSo = ChiSo,MaND=MaND });
        }
        String urlExport = Url.Action("ExportToExcel", "rptQuyetToanNghiepVu_5b_1", new {Thang_Quy = Thang_Quy, sLNS = sLNS, iID_MaDonVi = MaDonVi, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, LoaiIn = LoaiIn,MaND=MaND });
        using (Html.BeginForm("EditSubmit", "rptQuyetToanNghiepVu_5b_1", new { ParentID = ParentID, ChiSo = ChiSo }))
        {
    %>
   
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo Quyết toán thông tri 5b_1</span>
                    </td>
                     <td width="52%" style="text-align: left;">
                        <div class="login1" style="width: 50px; height: 20px; text-align: left;">
                            <a style="cursor: pointer;"></a>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="rptMain" style="background-color:#F0F9FE;">
            <div id="Div2">
<table width="100%" border="0" cellpadding="0" cellspacing="0" style="height: 93px">
  <tr>
  <td width="1%"></td>
    <td rowspan="26" width="5%" align="right"><b>LNS : </b> </td>
     <td style="width: 25%;" rowspan="26">
                        <div style="width: 99%; height: 500px; overflow: scroll; border:1px solid black;">
                            <table class="mGrid">
                                <tr>
                               <td><input type="checkbox" id="checkAll" onclick="Chonall(this.checked)"></td>
                                <td> Chọn tất cả LNS </td>
                                </tr>                           
                               
                                    <%
                                    String TenLNS = ""; String sLNS1 = "";
                                    String _Checked = "checked=\"checked\"";  
                                    for (int i = 0; i < dtLNS.Rows.Count; i++)
                                    {
                                        _Checked = "";
                                        TenLNS = Convert.ToString(dtLNS.Rows[i]["TenHT"]);
                                        sLNS1 = Convert.ToString(dtLNS.Rows[i]["sLNS"]);
                                        for (int j = 0; j < arrLNS.Length; j++)
                                        {
                                            if (sLNS1 == arrLNS[j])
                                            {
                                                _Checked = "checked=\"checked\"";
                                                break;
                                            }
                                        }    
                                    %>
                                    <tr>
                                        <td style="width: 10%;">
                                            <input type="checkbox" value="<%=sLNS1 %>" <%=_Checked %> check-group="sLNS" id="Checkbox1" onchange="Chon()"
                                                name="sLNS" />
                                        </td>
                                        <td>
                                            <%=TenLNS%>
                                        </td>
                                    </tr>
                                  <%}%>
                                </table>
                            </div>
                        </td> 
    <td align="right" class="style4" width="10%"><b>Quý :</b> </td>
    <td class="style2" width="7%">
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, Thang_Quy, "iThangQuy", "", "class=\"input1_2\" style=\"width:50%;\"onchange=Chon() ")%>
    
     </td>    
    <td rowspan="26" width="5%" align="right" valign="top"><b>Đơn vị :</b> </td>
    <td rowspan="26" width="43%" valign="top">
        <div id="<%= ParentID %>_tdDonVi" style="height:500px; overflow:scroll;" >
            
        </div>
    
    </td>
    <td></td>
  </tr>
  <tr>
  <td></td>
        <td class="style4" align="right"><b>Trạng Thái : </b></td>
        <td class="style2" ><div><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%;heigh:22px;\"onchange=Chon()")%>  <br /></div></td>
  </tr>
  <tr>
  <td></td>
    <td align="right" class="style4"><b>Tổng hợp dữ liệu theo: </b></td>
    <td class="style2" > &nbsp;
        <%=MyHtmlHelper.Option(ParentID, "ChiTiet", LoaiIn, "LoaiIn", "")%>&nbsp;&nbsp;Chi tiết&nbsp;&nbsp;
        <%=MyHtmlHelper.Option(ParentID, "TongHop", LoaiIn, "LoaiIn", "")%>&nbsp;&nbsp;Tổng hợp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    </td>

  </tr>
  <tr>
  <td></td>
  <td></td>
  <td></td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
  </tr>
  <tr>
  <td></td>
    <td colspan="6" align="center">  <table cellpadding="0" cellspacing="0" border="0" align="center">
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
                            </table></td>
  </tr>
</table>
            </div>
        </div>
        <script type="text/javascript">
                function CheckAll(value) {               
                    $("input:checkbox[check-group='DonVi']").each(function (i) {
                        this.checked = value;
                    });
                }
                function Chonall(sLNS) {
                    $("input:checkbox[check-group='sLNS']").each(function (i) {
                        if (sLNS) {
                            this.checked = true;
                        }
                        else {
                            this.checked = false;
                        }

                    });
                    Chon();
                }  
        </script>
      <script type="text/javascript">
          
          $(document).ready(function () {
              $("div#rptMain").hide();
              $('.title_tong a').click(function () {
                  $('div#rptMain').slideToggle('normal');
                  $(this).toggleClass('active');
                  return false;
              });
          });
          Chon();
          function Chon() {
                  Thang = document.getElementById("<%= ParentID %>_iThangQuy").value;
                  var sLNS = "";
                  $("input:checkbox[check-group='sLNS']").each(function (i) {
                      if (this.checked) {
                          if (sLNS != "") sLNS += ",";
                          sLNS += this.value;
                      }
                  });
              var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID%>_iID_MaTrangThaiDuyet").value;
              jQuery.ajaxSetup({ cache: false });
              var url = unescape('<%= Url.Action("Ds_DonVi?ParentID=#0&Thang_Quy=#1&sLNS=#2&iID_MaDonVi=#3&iID_MaTrangThaiDuyet=#4", "rptQuyetToanNghiepVu_5b_1") %>');
              url = unescape(url.replace("#0", "<%= ParentID %>"));
              url = unescape(url.replace("#1", Thang));              
              url = unescape(url.replace("#2", sLNS));
              url = unescape(url.replace("#3", "<%= iID_MaDonVi %>"));
              url = unescape(url.replace("#4", iID_MaTrangThaiDuyet));
              $.getJSON(url, function (data) {
                  document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data;
              });
          }
                                                     
      </script>
         <script type="text/javascript">
             function Huy() {
                 window.location.href = '<%=BackURL%>';
             }
    </script>
         <div>
    <%=MyHtmlHelper.ActionLink(urlExport, "Export To Excel") %>
    </div>
    </div>
    <%} %>
        <iframe src="<%=urlReport%>" height="600px" width="100%">
        </iframe>
</body>
</html>
