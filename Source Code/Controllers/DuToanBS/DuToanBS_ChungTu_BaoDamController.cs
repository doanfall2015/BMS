﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel;
using System.Collections.Specialized;
using VIETTEL.Models;
using System.IO;
using System.Data.OleDb;
using VIETTEL.Models.DuToanBS;
namespace VIETTEL.Controllers.DuToanBS
{
    public class DuToanBS_ChungTu_BaoDamController : Controller
    {
        #region Hằng Số
        private const string EDIT = "Edit";
        private const string CREATE = "Create";
        private const string DETAIL = "Detail";
        private const string CONTROLLER_NAME = "DuToanBS_ChungTu_BaoDam";
        private const string PERMITION_MESSAGE_CONTROLLER = "PermitionMessage";
        private const string VIEWS_ROOT_PATH = "~/Views/DuToanBS/ChungTuBaoDam/";
        private const string VIEW_CHUNGTU_INDEX = "ChungTu_Index.aspx";
        private const string VIEW_CHUNGTU_EDIT = "ChungTu_Edit.aspx";
        private const string VIEW_NHAN_KYTHUAT_INDEX = "ChungTu_NhanKyThuat_Index.aspx";
        private const string VIEW_NHAN_KYTHUAT_DETAIL = "ChungTu_NhanKyThuat_Detail.aspx";
        private const string VIEW_GOM_LAN2_INDEX = "ChungTu_GomLan2_Index.aspx";
        private const string VIEW_NHAN_BKHAC_INDEX = "ChungTu_Gom_NhanBKhac_Index.aspx";
        #endregion

        /// <summary>
        /// Index
        /// </summary>
        /// <param name="MaDotNganSach">Mã đợt ngân sách</param>
        /// <param name="iLoai">Loại thực hiện</param>
        /// <param name="iLan">lần phân cấp</param>
        /// <param name="iKyThuat">Ngành kỹ thuật</param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index(string MaDotNganSach, string iLoai, string iLan,string iKyThuat)
        {
            ViewData["MaDotNganSach"] = MaDotNganSach;
            ViewData["iLoai"] = iLoai;
            switch (iLoai)
            { 
                //case "1":
                //    return View(VIEWS_ROOT_PATH + "ChungTu_Gom_Index.aspx");
                //case "2":
                //    return View(VIEWS_ROOT_PATH + "ChungTu_Gom_THCuc_Index.aspx");
                case "3":
                    ViewData["iKyThuat"] = iKyThuat;
                    return View(VIEWS_ROOT_PATH + VIEW_NHAN_BKHAC_INDEX);
                case "4":
                    ViewData["iKyThuat"] = iKyThuat;
                    return View(VIEWS_ROOT_PATH + VIEW_NHAN_KYTHUAT_DETAIL);
                case "5":
                    ViewData["iKyThuat"] = iKyThuat;
                    return View(VIEWS_ROOT_PATH + VIEW_NHAN_KYTHUAT_INDEX);
                default:
                    if (iLan == "lan2")
                    {
                        return View(VIEWS_ROOT_PATH + VIEW_GOM_LAN2_INDEX);
                    }
                    return View(VIEWS_ROOT_PATH + VIEW_CHUNGTU_INDEX);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iLoai"></param>
        /// <param name="iID_MaChungTu_TLTH"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult TimKiemChungTu(string ParentID, string iLoai, string iID_MaChungTu_TLTH)
        {
            string TuNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dTuNgay"];
            string DenNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dDenNgay"];
            string SoChungTu = Request.Form[ParentID + "_iSoChungTu"];
            string sLNS_TK = Request.Form[ParentID + "_sLNS_TK"];
            string iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            return RedirectToAction("Index", CONTROLLER_NAME, new { iLoai = iLoai, SoChungTu = SoChungTu, TuNgay = TuNgay, DenNgay = DenNgay, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, sLNS_TK = sLNS_TK, iID_MaChungTu = iID_MaChungTu_TLTH });
        }

        [Authorize]
        public ActionResult SuaChungTu(string MaDotNganSach, string iID_MaChungTu, string ChiNganSach, string sLNS,string iKyThuat)
        {
            String MaND = User.Identity.Name;
            if (String.IsNullOrEmpty(iID_MaChungTu) && LuongCongViecModel.NguoiDung_DuocThemChungTu(DuToanModels.iID_MaPhanHe, MaND) == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", PERMITION_MESSAGE_CONTROLLER);
            }
            if (BaoMat.ChoPhepLamViec(MaND, "DTBS_ChungTu", EDIT) == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", PERMITION_MESSAGE_CONTROLLER);
            }
            if (String.IsNullOrEmpty(iID_MaChungTu))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["MaDotNganSach"] = MaDotNganSach;
            ViewData["MaChungTu"] = iID_MaChungTu;
            ViewData["ChiNganSach"] = ChiNganSach;
            ViewData["sLNS"] = sLNS;
            ViewData["iKyThuat"] = iKyThuat;
            return View(VIEWS_ROOT_PATH + VIEW_CHUNGTU_EDIT);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ThemSuaChungTu(string ParentID, String MaChungTu, string sLNS1, string iKyThuat)
        {
            String MaND = User.Identity.Name;
            string sChucNang = EDIT;

            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = CREATE;
            }
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(MaND, "DTBS_ChungTu", sChucNang) == false)
            {
                return RedirectToAction("Index", PERMITION_MESSAGE_CONTROLLER);
            }

            //validate dữ liệu
            NameValueCollection Errors = new NameValueCollection();
            String sLNS = Convert.ToString(Request.Form[ParentID + "_sLNS"]);
            String NgayChungTu = Convert.ToString(Request.Form[ParentID + "_vidNgayChungTu"]);
            String iID_MaPhongBanDich = Convert.ToString(Request.Form[ParentID + "_iID_MaPhongBanDich"]);
            if(String.IsNullOrEmpty(iKyThuat))
                 iKyThuat = Convert.ToString(Request.Form[ParentID + "_iKyThuat"]);
            if (NgayChungTu == string.Empty || NgayChungTu == "" || NgayChungTu == null)
            {
                Errors.Add("err_dNgayChungTu", "Bạn chưa nhập ngày chứng từ!");
            }
           
            if (sChucNang == CREATE)
            {
               
                if (sLNS == string.Empty || sLNS == "" || sLNS == null)
                {
                    Errors.Add("err_sLNS", "Bạn chưa chọn LNS!");
                }
            }

            if (Errors.Count > 0)
            {
                for (int i = 0; i <= Errors.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + Errors.GetKey(i), Errors[i]);
                }
                if (sChucNang==CREATE)
                {
                    return View(VIEWS_ROOT_PATH + "ChungTu_index.aspx");

                }
                else
                {
                    ViewData["MaChungTu"] = MaChungTu;
                    ViewData["sLNS"] = sLNS1;
                    return View(VIEWS_ROOT_PATH + "ChungTu_Edit.aspx");
                }
            }
            else
            {

                Bang bang = new Bang("DTBS_ChungTu");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
                {
                    //lay soChungtuTheoLamLamViec
                    int iSoChungTu = 0;
                    String iNamLamViec = NguoiDungCauHinhModels.iNamLamViec.ToString();
                    DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);

                    // iSoChungTu = DuToanBS_ChungTu_BaoDamModels.iSoChungTu(iNamLamViec)+1;
                    //bang.CmdParams.Parameters.AddWithValue("@sTienToChungTu", PhanHeModels.LayTienToChungTu(DuToanModels.iID_MaPhanHe));
                    // bang.CmdParams.Parameters.AddWithValue("@iSoChungTu", iSoChungTu);
                    String iID_MaNguonNganSach = "", iID_MaNamNganSach = "", iID_MaPhongBan = "", sTenPhongBan = "";
                    if (dtCauHinh.Rows.Count > 0)
                    {
                        iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                        iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                        dtCauHinh.Dispose();
                    }
                    DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(MaND);
                    if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
                    {
                        DataRow drPhongBan = dtPhongBan.Rows[0];
                        iID_MaPhongBan = Convert.ToString(drPhongBan["sKyHieu"]);
                        sTenPhongBan = Convert.ToString(drPhongBan["sTen"]);
                        dtPhongBan.Dispose();
                    }
                    bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
                    bang.CmdParams.Parameters.AddWithValue("@sDSLNS", sLNS);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBanDich", iID_MaPhongBan);
                    bang.CmdParams.Parameters.AddWithValue("@sTenPhongBan", sTenPhongBan);
                    
                    //Neu la nganh ky thuat 
                    if(iKyThuat=="1")
                    {
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(2));
                    }
                    else
                    {
                         bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanHeModels.iID_MaPhanHeDuToan));
                         bang.CmdParams.Parameters.AddWithValue("@iKyThuat",0);
                    }
                    String MaChungTuAddNew = Convert.ToString(bang.Save());
                    //DuToanBS_ChungTu_BaoDamModels.InsertDuyetChungTu(MaChungTuAddNew, "Mới ", User.Identity.Name, Request.UserHostAddress);

                    return RedirectToAction("Index", "DuToanBS_ChungTuChiTiet", new { iID_MaChungTu = MaChungTuAddNew });
                }
                else
                {
                    bang.GiaTriKhoa = MaChungTu;
                    bang.DuLieuMoi = false;
                    bang.Save();
                    return RedirectToAction("Index", "DuToanBS_ChungTu_BaoDam", new { sLNS = sLNS1, iKyThuat = iKyThuat });
                }
            }

        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit_GomNhanBKhac(String ParentID, String MaChungTu, String sLNS1, String iLan2)
        {
            String MaND = User.Identity.Name;
            string sChucNang = "Edit";
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = "Create";
            }
            Bang bang = new Bang("DTBS_ChungTu_TLTH");
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(MaND, bang.TenBang, sChucNang) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int i;
            NameValueCollection arrLoi = new NameValueCollection();
            String iID_MaChungTu = Convert.ToString(Request.Form["iID_MaChungTu"]);
            String NgayChungTu = Convert.ToString(Request.Form[ParentID + "_vidNgayChungTu"]);
            if (NgayChungTu == string.Empty || NgayChungTu == "" || NgayChungTu == null)
            {
                arrLoi.Add("err_dNgayChungTu", "Bạn chưa nhập ngày chứng từ!");
            }
            if (iID_MaChungTu == string.Empty || iID_MaChungTu == "" || iID_MaChungTu == null)
            {
                arrLoi.Add("err_iID_MaChungTu", "Không có đợt được chọn!");
            }
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                if (NgayChungTu == string.Empty || NgayChungTu == "" || NgayChungTu == null)
                {
                    arrLoi.Add("err_sLNS", "Không có đợt ngân sách!");
                }
            }

            if (arrLoi.Count > 0)
            {
                for (i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["MaChungTu"] = MaChungTu;
                if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
                {
                    ViewData["DuLieuMoi"] = "0";
                    ViewData["sLNS"] = sLNS1;
                    return RedirectToAction("index", "DuToanBS_ChungTu_BaoDam", new { iLoai = 3 });

                }
                else
                {
                    ViewData["MaChungTu"] = MaChungTu;
                    ViewData["DuLieuMoi"] = "0";
                    ViewData["sLNS"] = sLNS1;
                    return View(VIEWS_ROOT_PATH + "ChungTu_Edit.aspx");
                }
            }
            else
            {

                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
                {
                    //lay soChungtuTheoLamLamViec
                    int iSoChungTu = 0;
                    String iNamLamViec = NguoiDungCauHinhModels.iNamLamViec.ToString();
                    DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);

                    // iSoChungTu = DuToanBS_ChungTu_BaoDamModels.iSoChungTu(iNamLamViec)+1;
                    //bang.CmdParams.Parameters.AddWithValue("@sTienToChungTu", PhanHeModels.LayTienToChungTu(DuToanModels.iID_MaPhanHe));
                    // bang.CmdParams.Parameters.AddWithValue("@iSoChungTu", iSoChungTu);
                    String iID_MaNguonNganSach = "", iID_MaNamNganSach = "", iID_MaPhongBan = "", sTenPhongBan = "";
                    if (dtCauHinh.Rows.Count > 0)
                    {
                        iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                        iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                        dtCauHinh.Dispose();
                    }
                    DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(MaND);
                    if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
                    {
                        DataRow drPhongBan = dtPhongBan.Rows[0];
                        iID_MaPhongBan = Convert.ToString(drPhongBan["sKyHieu"]);
                        sTenPhongBan = Convert.ToString(drPhongBan["sTen"]);
                        dtPhongBan.Dispose();
                    }
                    String DK = "";
                    if (String.IsNullOrEmpty(iID_MaChungTu)) iID_MaChungTu = Guid.Empty.ToString();
                    String[] arrChungtu = iID_MaChungTu.Split(',');
                    SqlCommand cmd = new SqlCommand();
                    for (int j = 0; j < arrChungtu.Length; j++)
                    {
                        DK += " iID_MaChungTu =@iID_MaChungTu" + j;
                        if (j < arrChungtu.Length - 1)
                            DK += " OR ";
                        cmd.Parameters.AddWithValue("@iID_MaChungTu" + j, arrChungtu[j]);

                    }
                    int iID_MaTrangThaiDuyet = 3;
                    ///Update trạng thái check cho bảng chứng từ
                    String SQL = "";
                    //neu gom lan 2
                    if (iLan2 == "1")
                        SQL = @"UPDATE DTBS_ChungTu SET iCheckLan2=1 WHERE iTrangThai=1 AND (" + DK + ")";
                    else
                        SQL = @"UPDATE DTBS_ChungTu SET iCheck=1 WHERE iTrangThai=1 AND (" + DK + ")";
                    cmd.CommandText = SQL;
                    Connection.UpdateDatabase(cmd);
                    cmd.Dispose();

                    bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
                    bang.CmdParams.Parameters.AddWithValue("@sTenPhongBan", sTenPhongBan);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
                    bang.CmdParams.Parameters.AddWithValue("@iLoai", "3");
                    String MaChungTuAddNew = Convert.ToString(bang.Save());
                    //DuToanBS_ChungTu_BaoDamModels.InsertDuyetChungTu(MaChungTuAddNew, "Mới ", User.Identity.Name, Request.UserHostAddress);

                    return RedirectToAction("Index", "DuToanBS_ChungTu_BaoDam", new { iLoai = 3, sLNS = sLNS1 });
                }
                else
                {
                    bang.GiaTriKhoa = MaChungTu;
                    bang.DuLieuMoi = false;
                    bang.Save();
                    return RedirectToAction("Index", "DuToanBS_ChungTu_BaoDam", new { iLoai = 3, sLNS = sLNS1 });
                }
            }

        }
        
        [Authorize]
        public ActionResult XoaChungTu(string iID_MaChungTu, string MaDotNganSach, string ChiNganSach, string sLNS, string iKyThuat)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DTBS_ChungTu", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int iXoa = 0;
            iXoa = DuToanBSChungTuModels.XoaChungTu(iID_MaChungTu);
            return RedirectToAction("Index", "DuToanBS_ChungTu_BaoDam", new { MaDotNganSach = MaDotNganSach, ChiNganSach = ChiNganSach, sLNS = sLNS, iKyThuat = iKyThuat });
        }
        
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpLoadExcel(string iID_MaChungTu, string iLoai)
        {
            string path = string.Empty;
            string sTenKhoa = "TMTL";
            path = TuLieuLichSuModels.ThuMucLuu(sTenKhoa);
            String sFileName = "";
            string newPath = AppDomain.CurrentDomain.BaseDirectory + path;
            //string newPath = path + dateString;
            if (Directory.Exists(newPath) == false)
            {
                Directory.CreateDirectory(newPath);
            }
            sFileName = Path.GetFileName(Request.Files["uploadFile"].FileName);
            sFileName = Path.Combine(newPath, sFileName);
            String ConnectionString = String.Format(ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=Yes'", sFileName);
            OleDbConnection connExcel = new OleDbConnection(ConnectionString);
            OleDbCommand cmd = new OleDbCommand();
            OleDbConnection conn = null;
            conn = new OleDbConnection(ConnectionString);
            //try
            //{
                Request.Files["uploadFile"].SaveAs(Path.Combine(newPath, sFileName));
                OleDbCommand cmdExcel = new OleDbCommand();
                cmdExcel.Connection = connExcel;
                connExcel.Open();


                conn.Open();

                DataTable dtSheet = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                String Sheetname = Convert.ToString(dtSheet.Rows[0]["TABLE_NAME"]);

                cmd.CommandText = String.Format(@"SELECT * FROM [{0}]", Sheetname);
                cmd.Connection = conn;
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                DataTable dt = new DataTable();

                adapter.Fill(dt);
                dt.Columns[0].ColumnName = "STT";
                dt.Columns[1].ColumnName = "B";
                dt.Columns[2].ColumnName = "DV";
                dt.Columns[3].ColumnName = "TenDV";
                dt.Columns[4].ColumnName = "sLNS";
                dt.Columns[5].ColumnName = "sL";
                dt.Columns[6].ColumnName = "sK";
                dt.Columns[7].ColumnName = "sM";
                dt.Columns[8].ColumnName = "sTM";
                dt.Columns[9].ColumnName = "sTTM";
                dt.Columns[10].ColumnName = "sNG";
                dt.Columns[11].ColumnName = "TC";
                dt.Columns[12].ColumnName = "HV";
                SqlCommand cmd1;
                if (iLoai == "PC")
                {

                    String SQL = String.Format(@"SELECT *
FROM DTBS_ChungTuChiTiet
WHERE iTrangThai=1 AND iID_MaChungTuChiTiet=@iID_MaChungTu
AND( rTuChi<>0 OR rPhanCap<>0 OR rHienVat<>0 OR rHangNhap<>0 OR rHangMua<>0 OR rDuPhong<>0)");
                    cmd1 = new SqlCommand(SQL);
                    cmd1.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                    DataTable dtChungTu = Connection.GetDataTable(cmd1);

                    String MaND = User.Identity.Name;
                    String iNamLamViec = NguoiDungCauHinhModels.iNamLamViec.ToString();
                    DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
                    String iID_MaNguonNganSach = "", iID_MaNamNganSach = "", iID_MaPhongBan = "", sTenPhongBan = "";
                    if (dtCauHinh.Rows.Count > 0)
                    {
                        iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                        iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                        dtCauHinh.Dispose();
                    }
                    DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(MaND);
                    if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
                    {
                        DataRow drPhongBan = dtPhongBan.Rows[0];
                        iID_MaPhongBan = Convert.ToString(drPhongBan["sKyHieu"]);
                        sTenPhongBan = Convert.ToString(drPhongBan["sTen"]);
                        dtPhongBan.Dispose();
                    }
                    for (int i = 0; i < dtChungTu.Rows.Count; i++)
                    {
                        DataRow R = dtChungTu.Rows[i];
                        String sXau = String.Format(@"sL='{0}' AND sK='{1}' AND sM='{2}' AND sTM='{3}' AND sTTM='{4}' AND sNG='{5}'", R["sL"], R["sK"], R["sM"], R["sTM"], R["sTTM"], R["sNG"]);
                        DataRow[] dr = dt.Select(sXau);
                        //check neu da co du lieu se xoa cai cu sau do them cai moi
                        if (dr.Length > 0)
                        {
                            //Check bang DTBS_ChungTuchitiet_codulieu
                            SQL = String.Format(@"SELECT COUNT(*) FROM DTBS_ChungTuChiTiet_PhanCap WHERE iID_MaChungTu=@iID_MaChungTu");
                            cmd1 = new SqlCommand(SQL);
                            cmd1.Parameters.AddWithValue("@iID_MaChungTu", R["iID_MaChungTuChiTiet"]);


                            int count = Convert.ToInt32(Connection.GetValue(cmd1, 0));
                            //neu co du lieu se xoa
                            if (count > 0)
                            {
                                SQL = String.Format(@"DELETE DTBS_ChungTuChiTiet_PhanCap WHERE iID_MaChungTu=@iID_MaChungTu");
                                cmd1 = new SqlCommand(SQL);
                                cmd1.Parameters.AddWithValue("@iID_MaChungTu", R["iID_MaChungTuChiTiet"]);
                                Connection.UpdateDatabase(cmd1);
                            }
                        }
                          String sXauNoiMa="1020100-" + R["sL"] + "-" + R["sK"] + "-" + R["sM"] + "-" + R["sTM"] + "-" + R["sTTM"] + "-" + R["sNG"];

                          SQL = String.Format("SELECT * FROM NS_MucLucNganSach WHERE  sXauNoiMa=@sXauNoiMa AND iTrangThai=1");
                          cmd1 = new SqlCommand(SQL);
                          cmd1.Parameters.AddWithValue("@sXauNoiMa", sXauNoiMa);
                          DataTable dtMucLuc = Connection.GetDataTable(cmd1);
                        for (int j = 0; j < dr.Length; j++)
                        {
                            SQL = String.Format(@"INSERT INTO DTBS_ChungTuChiTiet_PhanCap(iID_MaChungTu,iID_MaPhongBan,sTenPhongBan,iID_MaPhongBanDich,iID_MaTrangThaiDuyet,
                        iNamLamViec,iID_MaNguonNganSach,iID_MaNamNganSach,iID_MaDonVi,sTenDonVi,iID_MaMucLucNganSach,iID_MaMucLucNganSach_Cha,sXauNoiMa,sLNS,sL,sK,sM,sTM,sTTM,sNG,
                        sMoTa,rTuChi,rHienVat,iID_MaNhomNguoiDung_DuocGiao,sID_MaNguoiDung_DuocGiao,sID_MaNguoiDungTao,sIPSua,sID_MaNguoiDungSua) VALUES (@iID_MaChungTu,@iID_MaPhongBan,@sTenPhongBan,@iID_MaPhongBanDich,@iID_MaTrangThaiDuyet,@iNamLamViec,@iID_MaNguonNganSach,@iID_MaNamNganSach,
@iID_MaDonVi,@sTenDonVi,@iID_MaMucLucNganSach,@iID_MaMucLucNganSach_Cha,@sXauNoiMa,@sLNS,@sL,@sK,@sM,@sTM,@sTTM,@sNG, @sMoTa,@rTuChi,@rHienVat,@iID_MaNhomNguoiDung_DuocGiao,@sID_MaNguoiDung_DuocGiao,@sID_MaNguoiDungTao,@sIPSua,@sID_MaNguoiDungSua)");
                            cmd1 = new SqlCommand(SQL);
                            cmd1.Parameters.AddWithValue("@iID_MaChungTu", R["iID_MaChungTuChiTiet"]);
                            cmd1.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
                            cmd1.Parameters.AddWithValue("@sTenPhongBan", sTenPhongBan);
                            cmd1.Parameters.AddWithValue("@iID_MaPhongBanDich", Convert.ToString(dr[j]["b"]));
                            cmd1.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanHeModels.iID_MaPhanHeDuToan));
                            cmd1.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                            cmd1.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
                            cmd1.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
                            cmd1.Parameters.AddWithValue("@iID_MaDonVi", Convert.ToString(dr[j]["DV"]));
                            String sTenDonVi = DonViModels.Get_TenDonVi(Convert.ToString(dr[j]["DV"]));
                            cmd1.Parameters.AddWithValue("@sTenDonVi", Convert.ToString(dr[j]["DV"]) + "-" + sTenDonVi);



                            cmd1.Parameters.AddWithValue("@iID_MaMucLucNganSach", dtMucLuc.Rows[0]["iID_MaMucLucNganSach"]);
                            cmd1.Parameters.AddWithValue("@iID_MaMucLucNganSach_Cha", dtMucLuc.Rows[0]["iID_MaMucLucNganSach_Cha"]);
                            cmd1.Parameters.AddWithValue("@sXauNoiMa", "1020100-" + R["sL"] + "-" + R["sK"] + "-" + R["sM"] + "-" + R["sTM"] + "-" + R["sTTM"] + "-" + R["sNG"]);
                            cmd1.Parameters.AddWithValue("@sLNS", "1020100");
                            cmd1.Parameters.AddWithValue("@sL", R["sL"]);
                            cmd1.Parameters.AddWithValue("@sK", R["sK"]);
                            cmd1.Parameters.AddWithValue("@sM", R["sM"]);
                            cmd1.Parameters.AddWithValue("@sTM", R["sTM"]);
                            cmd1.Parameters.AddWithValue("@sTTM", R["sTTM"]);
                            cmd1.Parameters.AddWithValue("@sNG", R["sNG"]);
                            cmd1.Parameters.AddWithValue("@sMoTa", R["sMoTa"]);
                            if (!String.IsNullOrEmpty(Convert.ToString(dr[j]["TC"])))
                            {
                                cmd1.Parameters.AddWithValue("@rTuChi", Convert.ToDecimal(dr[j]["TC"]) * 1000);
                            }
                            else
                                cmd1.Parameters.AddWithValue("@rTuChi", 0);
                            if (!String.IsNullOrEmpty(Convert.ToString(dr[j]["HV"])))
                            {
                                cmd1.Parameters.AddWithValue("@rHienVat", Convert.ToDecimal(dr[j]["HV"]) * 1000);
                            }
                            else
                                cmd1.Parameters.AddWithValue("@rHienVat", 0);

                            cmd1.Parameters.AddWithValue("@iID_MaNhomNguoiDung_DuocGiao", R["iID_MaNhomNguoiDung_DuocGiao"]);
                            cmd1.Parameters.AddWithValue("@sID_MaNguoiDung_DuocGiao", MaND);
                            cmd1.Parameters.AddWithValue("@sID_MaNguoiDungTao", MaND);
                            cmd1.Parameters.AddWithValue("@sIPSua", Request.UserHostAddress);
                            cmd1.Parameters.AddWithValue("@sID_MaNguoiDungSua", MaND);
                            Connection.UpdateDatabase(cmd1);
                        }
                        dtMucLuc.Dispose();
                    }
                }
                else
                {
                    String SQL = String.Format(@"SELECT *
FROM DTBS_ChungTuChiTiet
WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu
AND( rTuChi<>0 OR rPhanCap<>0 OR rHienVat<>0 OR rHangNhap<>0 OR rHangMua<>0 OR rDuPhong<>0)");
                    cmd1 = new SqlCommand(SQL);
                    cmd1.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                    DataTable dtChungTu = Connection.GetDataTable(cmd1);

                    String MaND = User.Identity.Name;
                    String iNamLamViec = NguoiDungCauHinhModels.iNamLamViec.ToString();
                    DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
                    String iID_MaNguonNganSach = "", iID_MaNamNganSach = "", iID_MaPhongBan = "", sTenPhongBan = "";
                    if (dtCauHinh.Rows.Count > 0)
                    {
                        iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                        iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                        dtCauHinh.Dispose();
                    }
                    DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(MaND);
                    if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
                    {
                        DataRow drPhongBan = dtPhongBan.Rows[0];
                        iID_MaPhongBan = Convert.ToString(drPhongBan["sKyHieu"]);
                        sTenPhongBan = Convert.ToString(drPhongBan["sTen"]);
                        dtPhongBan.Dispose();
                    }
                    for (int i = 0; i < dtChungTu.Rows.Count; i++)
                    {
                        DataRow R = dtChungTu.Rows[i];
                        String sXau = String.Format(@"sL='{0}' AND sK='{1}' AND sM='{2}' AND sTM='{3}' AND sTTM='{4}' AND sNG='{5}'", R["sL"], R["sK"], R["sM"], R["sTM"], R["sTTM"], R["sNG"]);
                        DataRow[] dr = dt.Select(sXau);
                        //check neu da co du lieu se xoa cai cu sau do them cai moi
                        if (dr.Length > 0)
                        {
                            //Check bang DTBS_ChungTuchitiet_codulieu
                            SQL = String.Format(@"SELECT COUNT(*) FROM DTBS_ChungTuChiTiet_PhanCap WHERE iID_MaChungTu=@iID_MaChungTu");
                            cmd1 = new SqlCommand(SQL);
                            cmd1.Parameters.AddWithValue("@iID_MaChungTu", R["iID_MaChungTuChiTiet"]);


                            int count = Convert.ToInt32(Connection.GetValue(cmd1, 0));
                            //neu co du lieu se xoa
                            if (count > 0)
                            {
                                SQL = String.Format(@"DELETE DTBS_ChungTuChiTiet_PhanCap WHERE iID_MaChungTu=@iID_MaChungTu");
                                cmd1 = new SqlCommand(SQL);
                                cmd1.Parameters.AddWithValue("@iID_MaChungTu", R["iID_MaChungTuChiTiet"]);
                                Connection.UpdateDatabase(cmd1);
                            }
                        }
                        String sXauNoiMa = "1020100-" + R["sL"] + "-" + R["sK"] + "-" + R["sM"] + "-" + R["sTM"] + "-" + R["sTTM"] + "-" + R["sNG"];

                        SQL = String.Format("SELECT * FROM NS_MucLucNganSach WHERE  sXauNoiMa=@sXauNoiMa AND iTrangThai=1");
                        cmd1 = new SqlCommand(SQL);
                        cmd1.Parameters.AddWithValue("@sXauNoiMa", sXauNoiMa);
                        DataTable dtMucLuc = Connection.GetDataTable(cmd1);
                        for (int j = 0; j < dr.Length; j++)
                        {
                            SQL = String.Format(@"INSERT INTO DTBS_ChungTuChiTiet_PhanCap(iID_MaChungTu,iID_MaPhongBan,sTenPhongBan,iID_MaPhongBanDich,iID_MaTrangThaiDuyet,
                        iNamLamViec,iID_MaNguonNganSach,iID_MaNamNganSach,iID_MaDonVi,sTenDonVi,iID_MaMucLucNganSach,iID_MaMucLucNganSach_Cha,sXauNoiMa,sLNS,sL,sK,sM,sTM,sTTM,sNG,
                        sMoTa,rTuChi,rHienVat,iID_MaNhomNguoiDung_DuocGiao,sID_MaNguoiDung_DuocGiao,sID_MaNguoiDungTao,sIPSua,sID_MaNguoiDungSua) VALUES (@iID_MaChungTu,@iID_MaPhongBan,@sTenPhongBan,@iID_MaPhongBanDich,@iID_MaTrangThaiDuyet,@iNamLamViec,@iID_MaNguonNganSach,@iID_MaNamNganSach,
@iID_MaDonVi,@sTenDonVi,@iID_MaMucLucNganSach,@iID_MaMucLucNganSach_Cha,@sXauNoiMa,@sLNS,@sL,@sK,@sM,@sTM,@sTTM,@sNG, @sMoTa,@rTuChi,@rHienVat,@iID_MaNhomNguoiDung_DuocGiao,@sID_MaNguoiDung_DuocGiao,@sID_MaNguoiDungTao,@sIPSua,@sID_MaNguoiDungSua)");
                            cmd1 = new SqlCommand(SQL);
                            cmd1.Parameters.AddWithValue("@iID_MaChungTu", R["iID_MaChungTuChiTiet"]);
                            cmd1.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
                            cmd1.Parameters.AddWithValue("@sTenPhongBan", sTenPhongBan);
                            cmd1.Parameters.AddWithValue("@iID_MaPhongBanDich", Convert.ToString(dr[j]["b"]));
                            cmd1.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanHeModels.iID_MaPhanHeDuToan));
                            cmd1.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                            cmd1.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
                            cmd1.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
                            cmd1.Parameters.AddWithValue("@iID_MaDonVi", Convert.ToString(dr[j]["DV"]));
                            String sTenDonVi = DonViModels.Get_TenDonVi(Convert.ToString(dr[j]["DV"]));
                            cmd1.Parameters.AddWithValue("@sTenDonVi", Convert.ToString(dr[j]["DV"]) + "-" + sTenDonVi);
                            cmd1.Parameters.AddWithValue("@iID_MaMucLucNganSach", dtMucLuc.Rows[0]["iID_MaMucLucNganSach"]);
                            cmd1.Parameters.AddWithValue("@iID_MaMucLucNganSach_Cha", dtMucLuc.Rows[0]["iID_MaMucLucNganSach_Cha"]);
                            cmd1.Parameters.AddWithValue("@sXauNoiMa", "1020100-" + R["sL"] + "-" + R["sK"] + "-" + R["sM"] + "-" + R["sTM"] + "-" + R["sTTM"] + "-" + R["sNG"]);
                            cmd1.Parameters.AddWithValue("@sLNS", "1020100");
                            cmd1.Parameters.AddWithValue("@sL", R["sL"]);
                            cmd1.Parameters.AddWithValue("@sK", R["sK"]);
                            cmd1.Parameters.AddWithValue("@sM", R["sM"]);
                            cmd1.Parameters.AddWithValue("@sTM", R["sTM"]);
                            cmd1.Parameters.AddWithValue("@sTTM", R["sTTM"]);
                            cmd1.Parameters.AddWithValue("@sNG", R["sNG"]);
                            cmd1.Parameters.AddWithValue("@sMoTa", R["sMoTa"]);
                            if (!String.IsNullOrEmpty(Convert.ToString(dr[j]["TC"])))
                            {
                                cmd1.Parameters.AddWithValue("@rTuChi", Convert.ToDecimal(dr[j]["TC"]) * 1000);
                            }
                            else
                                cmd1.Parameters.AddWithValue("@rTuChi", 0);
                            if (!String.IsNullOrEmpty(Convert.ToString(dr[j]["HV"])))
                            {
                                cmd1.Parameters.AddWithValue("@rHienVat", Convert.ToDecimal(dr[j]["HV"]) * 1000);
                            }
                            else
                                cmd1.Parameters.AddWithValue("@rHienVat", 0);
                            cmd1.Parameters.AddWithValue("@iID_MaNhomNguoiDung_DuocGiao", R["iID_MaNhomNguoiDung_DuocGiao"]);
                            cmd1.Parameters.AddWithValue("@sID_MaNguoiDung_DuocGiao", MaND);
                            cmd1.Parameters.AddWithValue("@sID_MaNguoiDungTao", MaND);
                            cmd1.Parameters.AddWithValue("@sIPSua", Request.UserHostAddress);
                            cmd1.Parameters.AddWithValue("@sID_MaNguoiDungSua", MaND);
                            Connection.UpdateDatabase(cmd1);
                        }
                        dtMucLuc.Dispose();
                    }

                }
                cmd.Dispose();
                cmd1.Dispose();

            //}
            //catch (Exception Exception) { throw Exception; }
            //finally
            //{
                conn.Close();
                connExcel.Close();
                conn.Dispose();
                connExcel.Dispose();

                string url = newPath + "/" + Path.GetFileName(Request.Files["uploadFile"].FileName);
                System.IO.File.Delete(url);

            //}
            if (iLoai == "PC")
                return RedirectToAction("ChungTuChiTiet_Frame", "DuToan_phanCapChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu });
            return RedirectToAction("ChungTuChiTiet_Frame", "DuToan_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu });
        }
    }
}
