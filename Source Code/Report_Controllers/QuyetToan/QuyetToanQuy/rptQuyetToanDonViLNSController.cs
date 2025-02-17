﻿using System;
using System.Web.Mvc;
using FlexCel.Core;
using FlexCel.Report;
using FlexCel.Render;
using FlexCel.XlsAdapter;
using System.Data;
using VIETTEL.Models;
using System.IO;
using VIETTEL.Models.QuyetToan;

namespace VIETTEL.Report_Controllers.QuyetToan.QuyetToanQuy
{
    public class rptQuyetToanDonViLNSController : Controller
    {
        
        public string sViewPath = "~/Report_Views/";
        private const String EXCEL_FILE_PATH = "/Report_ExcelFrom/QuyetToan/QuyetToanQuy/rptQuyetToan_DonVi_LNS.xls";
        private const string VIEW_PATH_QUYETTOAN_DONVI_LNS = "~/Report_Views/QuyetToan/QuyetToanQuy/rptQuyetToan_DonVi_LNS.aspx";

        public ActionResult Index()
        {
            ViewData["path"] = VIEW_PATH_QUYETTOAN_DONVI_LNS;
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// Lấy các giá trị từ Form gán vào ViewData
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult FormSubmit(String ParentID)
        {
            String MaPhongBan = Request.Form[ParentID + "_iID_MaPhongBan"];
            String sLNS = Request.Form["sLNS"];
            String MaND = Request.Form["QuyetToanNganSach" + "_MaND"];
            String iID_MaDonVi = Request.Form["QuyetToanNganSach" + "_iID_MaDonVi"];
            String iThang_Quy = Request.Form["QuyetToanNganSach" + "_iThang_Quy"];
            String iID_MaNamNganSach = Request.Form["QuyetToanNganSach" + "_iID_MaNamNganSach"];

            ViewData["MaPhongBan"] = MaPhongBan;
            ViewData["PageLoad"] = "1";
            ViewData["sLNS"] = sLNS;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iThang_Quy"] = iThang_Quy;
            ViewData["iID_MaNamNganSach"] = iID_MaNamNganSach;
            ViewData["path"] = VIEW_PATH_QUYETTOAN_DONVI_LNS;

            if (String.IsNullOrEmpty(sLNS)) sLNS = "-1";

            return RedirectToAction("ViewPDF", new { MaND = MaND, sLNS = sLNS, iID_MaDonVi = iID_MaDonVi, iThang_Quy = iThang_Quy, iID_MaNamNganSach = iID_MaNamNganSach, MaPhongBan = MaPhongBan });

        }

        /// <summary>
        /// Xuất file PDF quyết toán của từng đơn vị
        /// </summary>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="iThang_Quy">Quý</param>
        /// <param name="sLNS">Loại ngân sách</param>
        /// <param name="iID_MaDonVi">Mã đơn vị</param>
        /// <param name="iID_MaNamNganSach">Năm ngân sách</param>
        /// <param name="MaPhongBan">Mã phòng ban</param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND, String iThang_Quy, String sLNS, String iID_MaDonVi, String iID_MaNamNganSach, String MaPhongBan)
        {
            HamChung.Language();
            String sDuongDan = "";
            sDuongDan = EXCEL_FILE_PATH;

            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), MaND, sLNS, iThang_Quy, iID_MaDonVi, iID_MaNamNganSach, MaPhongBan);
            using (FlexCelPdfExport pdf = new FlexCelPdfExport())
            {
                pdf.Workbook = xls;
                using (MemoryStream ms = new MemoryStream())
                {
                    pdf.BeginExport(ms);
                    pdf.ExportAllVisibleSheets(false, "BaoCao");
                    pdf.EndExport();
                    ms.Position = 0;
                    return File(ms.ToArray(), "application/pdf");
                }
            }
        }

        /// <summary>
        /// Tạo file PDF xuất dữ liệu của quyết toán từng đơn vị
        /// </summary>
        /// <param name="path"></param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="sLNS">Loại ngân sách</param>
        /// <param name="iThang_Quy">Quý</param>
        /// <param name="iID_MaDonVi">Mã đơn vị</param>
        /// <param name="iID_MaNamNganSach">Năm ngân sách</param>
        /// <param name="MaPhongBan">Mã phòng ban</param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String MaND, String sLNS, String iThang_Quy, String iID_MaDonVi, String iID_MaNamNganSach, String MaPhongBan)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToan_DonVi_LNS");

            LoadData(fr, MaND, sLNS, iThang_Quy, iID_MaDonVi, iID_MaNamNganSach, MaPhongBan);
            String Nam = ReportModels.LayNamLamViec(MaND);

            String NamNganSach = "";
            if (iID_MaNamNganSach == "1")
                NamNganSach = "QUYẾT TOÁN NĂM TRƯỚC";
            else if (iID_MaNamNganSach == "2")
                NamNganSach = "QUYẾT TOÁN NĂM NAY";
            else
            {
                NamNganSach = "TỔNG HỢP";
            }

            String sTenDonVi = DonViModels.Get_TenDonVi(iID_MaDonVi);
            fr.SetValue("BoQuocPhong", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("QuanKhu", ReportModels.CauHinhTenDonViSuDung(2));
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Nam", Nam);
            fr.SetValue("Quy", iThang_Quy);
            fr.SetValue("NamNganSach", NamNganSach);
            fr.SetValue("sTenDonVi", sTenDonVi);
            fr.Run(Result);

            return Result;
        }

       /// <summary>
       /// Lấy dữ liệu chi tiết của quyết toán từng đơn vị
       /// </summary>
       /// <param name="fr"></param>
       /// <param name="MaND">Mã người dùng</param>
       /// <param name="sLNS">Loại ngân sách</param>
       /// <param name="iThang_Quy">Quý</param>
       /// <param name="iID_MaDonVi">Mã đơn vị</param>
       /// <param name="iID_MaNamNganSach">Năm ngân sách</param>
       /// <param name="MaPhongBan">Mã phòng ban</param>
        private void LoadData(FlexCelReport fr, String MaND, String sLNS, String iThang_Quy, String iID_MaDonVi, String iID_MaNamNganSach, String MaPhongBan)
        {
            DataRow r;
            DataTable data = new DataTable();

            data = QuyetToan_ReportModels.rptQuyetToanDonViLNS(MaND, sLNS, iThang_Quy, iID_MaDonVi, iID_MaNamNganSach, MaPhongBan);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);

            DataTable dtsTM = HamChung.SelectDistinct("dtsTM", data, "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sTM", "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            DataTable dtsM = HamChung.SelectDistinct("dtsM", dtsTM, "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM", "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            DataTable dtsL = HamChung.SelectDistinct("dtsL", dtsM, "sLNS1,sLNS3,sLNS5,sLNS,sL,sK", "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sMoTa", "sLNS,sL,sK,sM");
            DataTable dtsLNS = HamChung.SelectDistinct("dtsLNS", dtsL, "sLNS1,sLNS3,sLNS5,sLNS", "sLNS1,sLNS3,sLNS5,sLNS,sMoTa", "sLNS,sL");

            DataTable dtsLNS5 = HamChung.SelectDistinct("dtsLNS5", dtsLNS, "sLNS1,sLNS3,sLNS5", "sLNS1,sLNS3,sLNS5,sMoTa");
            for (int i = 0; i < dtsLNS5.Rows.Count; i++)
            {
                r = dtsLNS5.Rows[i];
                r["sMoTa"] = ReportModels.LayMoTa(Convert.ToString(r["sLNS5"]));
            }

            DataTable dtsLNS3 = HamChung.SelectDistinct("dtsLNS3", dtsLNS5, "sLNS1,sLNS3", "sLNS1,sLNS3,sMoTa");
            for (int i = 0; i < dtsLNS3.Rows.Count; i++)
            {
                r = dtsLNS3.Rows[i];
                r["sMoTa"] = ReportModels.LayMoTa(Convert.ToString(r["sLNS3"]));
            }

            DataTable dtsLNS1 = HamChung.SelectDistinct("dtsLNS1", dtsLNS3, "sLNS1", "sLNS1,sMoTa");
            for (int i = 0; i < dtsLNS1.Rows.Count; i++)
            {
                r = dtsLNS1.Rows[i];
                r["sMoTa"] = ReportModels.LayMoTa(Convert.ToString(r["sLNS1"]));
            }

            fr.AddTable("dtsTM", dtsTM);
            fr.AddTable("dtsM", dtsM);
            fr.AddTable("dtsL", dtsL);
            fr.AddTable("dtsLNS", dtsLNS);
            fr.AddTable("dtsLNS1", dtsLNS1);
            fr.AddTable("dtsLNS3", dtsLNS3);
            fr.AddTable("dtsLNS5", dtsLNS5);

            data.Dispose();
            dtsTM.Dispose();
            dtsM.Dispose();
            dtsL.Dispose();
            dtsLNS.Dispose();
            dtsLNS1.Dispose();
            dtsLNS3.Dispose();
            dtsLNS5.Dispose();

        }
     
        /// <summary>
        /// Lấy danh sách loại ngân sách dựa vào quý, năm ngân sách, mã người dùng, mã phòng ban và mã đơn vị
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="Thang_Quy">Quý</param>
        /// <param name="iID_MaDonVi">Mã đơn vị</param>
        /// <param name="sLNS">Loại ngân sách</param>
        /// <param name="iID_MaNamNganSach">Năm ngân sách</param>
        /// <param name="MaPhongBan">Mã phòng ban</param>
        /// <returns></returns>
        public JsonResult LayDanhSachDonVi(String ParentID, String Thang_Quy, String iID_MaDonVi, String sLNS, String iID_MaNamNganSach, String MaPhongBan)
        {
            String MaND = User.Identity.Name;
            String sViewPath = "~/Views/DungChung/DonVi/LNS_DanhSach.ascx";

            DataTable dt = QuyetToan_ReportModels.dtDonVi_LNS(Thang_Quy, iID_MaNamNganSach, MaND, iID_MaDonVi, MaPhongBan);
            {
                sLNS = Guid.Empty.ToString();
            }

            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, sLNS, dt, ParentID);
            String strDonVi = HamChung.RenderPartialViewToStringLoad(sViewPath, Model, this);

            return Json(strDonVi, JsonRequestBehavior.AllowGet);
        }

    }
}

