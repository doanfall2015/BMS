﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainModel.Abstract;
using System.Collections.Specialized;
using DomainModel;
using System.Data;
using DomainModel.Controls;
using System.Data.SqlClient;
using System.Web.Mvc;
namespace VIETTEL.Models
{
    public class MucLucNganSachModels
    {
        //Danh sách trường của mục lục ngân sách
        public static String strDSTruongTienTieuDe = "Tên công trình,Ngày,Người,Chi tại kho bạc,Tồn kho,Tự chi,Chi tập trung,Hàng nhập,Hàng mua,Hiện vật,Phân cấp,Dự phòng";
        public static String strDSTruongTienTieuDe_So = "Ngày,Người,Chi tại kho bạc,Tồn kho,Tự chi,Chi tập trung,Hàng nhập,Hàng mua,Hiện vật,Phân cấp,Dự phòng";
        public static String strDSTruongTien_So = "rNgay,rSoNguoi,rChiTaiKhoBac,rTonKho,rTuChi,rChiTapTrung,rHangNhap,rHangMua,rHienVat,rPhanCap,rDuPhong";
        public static String strDSTruongTien_Xau = "sTenCongTrinh";
        public static String strDSTruongTien = strDSTruongTien_Xau + "," + strDSTruongTien_So;
        
        public static String strDSTruongTienDoRong_So = "100,100,100,100,100,100,100,100,100,100,100,100";
        public static String strDSTruongTienDoRong_Xau = "150";
        public static String strDSTruongTienDoRong = strDSTruongTienDoRong_Xau + "," + strDSTruongTienDoRong_So;

        public static String strDSTruongTien_Full = strDSTruongTien + ",rTongSo";
        public static String strDSTruongTienDoRong_Full = strDSTruongTienDoRong + ",100";
        public static String strDSTruongTienTieuDe_Full = strDSTruongTienTieuDe + ",Tổng số";

        public static String strDSDuocNhapTruongTien = "b" + strDSTruongTien.Replace(",", ",b");

        public static String strDSTruongSapXep = "sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG";

        public static String strDSTruongTieuDe = "LNS,L,K,M,TM,TTM,NG,TNG,Nội dung";
        public static String strDSTruong = "sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa";
        public static String strDSTruongDoRong = "60,30,30,40,40,30,30,30,250";

        public static String strDSTruong_LNS_New = "sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa";
        public static String strDSTruongDoRong_LNS_New = "60,40,40,40,40,40,40,350";

        public static String strDSTruong_LNS = "sM,sTM,sTTM,sNG,sMoTa";
        public static String strDSTruongDoRong_LNS = "40,40,40,40,350";
        public static String strDSTruong_80 = "sLNS,sNG,sMoTa";
        public static String strDSTruongDoRong_80 = "60,40,350";
        public static String[] arrDSTruongTieuDe = strDSTruongTieuDe.Split(',');
        public static String[] arrDSTruong = strDSTruong.Split(',');
        public static String[] arrDSTruongDoRong = strDSTruongDoRong.Split(',');

        
      
        //Danh sách trường tiền của phần Thu Nộp Ngân Sách
        public static String strDSTruongTien_ThuNopTieuDe = "Kế hoạch,Nộp NSQP,Tổng thu trong năm,Hao mòn TSCĐ,Tiền lương,QTNS Khác,Chi phí khác,Tổng số chi phí,Chênh lệch T/C,Nộp thuế TNDN,Thuế nộp NS N.nước,Thuế TNDN qua BQP,Nộp NS N.nước-khác ,Nộp cấp trên,Bổ sung k.phí,Trích quỹ đơn vị,Số chưa phân phối, Ghi chú";
        public static String strDSTruongTien_ThuNop_So = "rKeHoach,rNopNSQP,rTongThu,rKhauHaoTSCD,rTienLuong,rQTNSKhac,rChiPhiKhac,rTongSo,rChenhLech,rNopThueTNDN,rNopNSNN,rNopNSNNQuaBQP,rNopNSNNKhac,rNopCapTren,rBoSungKinhPhi,rTrichQuyDonVi,rSoChuaPhanPhoi";
        public static String strDSTruongTien_ThuNop_So_Loai1 = "rKeHoach,rThu,rThoaiThu,rTongThu";
       
        public static String strDSTruongTien_ThuNop_Xau = "sGhiChu";
        public static String strDSTruongTien_ThuNop = strDSTruongTien_ThuNop_So + "," + strDSTruongTien_ThuNop_Xau;
        public static String strDSTruongTien_ThuNop_Loai1 = strDSTruongTien_ThuNop_So_Loai1 + "," + strDSTruongTien_ThuNop_Xau;

        public static String strDSTruongTien_ThuNopDoRong = "120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,250";
        

        public static DataTable NS_LoaiNganSach()
        {
            String SQL = String.Format("SELECT distinct(sLNS),sMoTa,{0} FROM NS_MucLucNganSach WHERE LEN(sLNS)=7 AND sL='' ORDER BY sLNS", strDSDuocNhapTruongTien + ",sNhapTheoTruong");
            return Connection.GetDataTable(SQL);
        }

        public static DataTable dt_ChiTietMucLucNganSach(String iID_MaMucLucNganSach)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM NS_MucLucNganSach WHERE iID_MaMucLucNganSach=@iID_MaMucLucNganSach");
            cmd.Parameters.AddWithValue("@iID_MaMucLucNganSach", iID_MaMucLucNganSach);
            DataTable vR =  Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static DataTable dt_ChiTietMucLucNganSach_XauNoiMa(String sXauNoiMa)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM NS_MucLucNganSach WHERE sXauNoiMa=@sXauNoiMa");
            cmd.Parameters.AddWithValue("@sXauNoiMa", sXauNoiMa);
            DataTable vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static DataTable dt_ChiTietMucLucNganSach_XauNoiMa_KeCan(String sXauNoiMa)
        {
            SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM NS_MucLucNganSach WHERE sXauNoiMa LIKE @sXauNoiMa");
            cmd.Parameters.AddWithValue("@sXauNoiMa", sXauNoiMa + "%");
            DataTable vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static DataTable dt_ChiTiet_TheoDSTruong(String[] arrDSGiaTri)
        {
            SqlCommand cmd = new SqlCommand();
            String DK = "";
            int i = 0;
            for (i = 0; i < arrDSTruong.Length-1; i++)
            {
                DK += String.Format(" AND {0}=@{0}", arrDSTruong[i]);
                cmd.Parameters.AddWithValue("@" + arrDSTruong[i], arrDSGiaTri[i]);
            }

            String SQL = String.Format("SELECT TOP 2 * FROM NS_MucLucNganSach WHERE bLaHangCha=0 {0}", DK);
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable dt_ChiTiet_TheoDSTruong_DuToan(String[] arrDSGiaTri)
        {
            SqlCommand cmd = new SqlCommand();
            String DK = "";
            int i = 0;
            for (i = 0; i < arrDSTruong.Length - 2; i++)
            {
                DK += String.Format(" AND {0}=@{0}", arrDSTruong[i]);
                cmd.Parameters.AddWithValue("@" + arrDSTruong[i], arrDSGiaTri[i]);
            }

            String SQL = String.Format("SELECT TOP 2 * FROM NS_MucLucNganSach WHERE bLaHangCha=0 {0}", DK);
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static void CapNhapLai()
        {
            SqlCommand cmd;
            String[] arrDSTruong = MucLucNganSachModels.strDSTruong.Split(',');
            //Cap nhap lai xau ma
            String SQL = "SELECT * FROM NS_MucLucNganSach ORDER BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG";
            DataTable dt = Connection.GetDataTable(SQL);
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                DataRow RMucLucNganSach = dt.Rows[i];
                string sXauNoiMa = "";
                Boolean bLaHangCha = false;
                for (int j = 0; j < arrDSTruong.Length-1; j++)
                {
                    if (String.IsNullOrEmpty(Convert.ToString(RMucLucNganSach[arrDSTruong[j]])) == false)
                    {
                        if (sXauNoiMa != "") sXauNoiMa += "-";
                        sXauNoiMa += Convert.ToString(RMucLucNganSach[arrDSTruong[j]]);
                    }
                    else
                    {
                        if (j < arrDSTruong.Length - 2)
                        {
                            bLaHangCha = true;
                        }
                    }
                }
                cmd = new SqlCommand("UPDATE NS_MucLucNganSach SET sXauNoiMa=@sXauNoiMa, bLaHangCha=@bLaHangCha WHERE iID_MaMucLucNganSach=@iID_MaMucLucNganSach");
                cmd.Parameters.AddWithValue("@sXauNoiMa",sXauNoiMa);
                cmd.Parameters.AddWithValue("@bLaHangCha",bLaHangCha);
                cmd.Parameters.AddWithValue("@iID_MaMucLucNganSach",RMucLucNganSach["iID_MaMucLucNganSach"]);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();
            }
            dt.Dispose();
            //Cap nhap lai ma cha
            SQL = "SELECT * FROM NS_MucLucNganSach ORDER BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG";
            dt = Connection.GetDataTable(SQL);
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                DataRow RMucLucNganSach = dt.Rows[i];
                Object iID_MaMucLucNganSachCha = DBNull.Value;
                
                string sXauNoiMa = Convert.ToString(RMucLucNganSach["sXauNoiMa"]);
                
                for (int j = i - 1; j >= 0; j--)
                {
                    String tg_sXauNoiMa=Convert.ToString(dt.Rows[j]["sXauNoiMa"]);
                    if (sXauNoiMa.StartsWith(tg_sXauNoiMa))
                    {
                        cmd = new SqlCommand("UPDATE NS_MucLucNganSach SET iID_MaMucLucNganSach_Cha=@iID_MaMucLucNganSach_Cha WHERE iID_MaMucLucNganSach=@iID_MaMucLucNganSach");
                        cmd.Parameters.AddWithValue("@iID_MaMucLucNganSach",RMucLucNganSach["iID_MaMucLucNganSach"]);
                        cmd.Parameters.AddWithValue("@iID_MaMucLucNganSach_Cha",dt.Rows[j]["iID_MaMucLucNganSach"]);
                        Connection.UpdateDatabase(cmd);
                        cmd.Dispose();
                        break;
                    }
                }                
            }
            dt.Dispose();
        }
        public static void CapNhapLai(String sLNS)
        {
            SqlCommand cmd;
            String[] arrDSTruong = MucLucNganSachModels.strDSTruong.Split(',');
            //Cap nhap lai xau ma
            String DK="";
            if (String.IsNullOrEmpty(sLNS) == false) DK = " WHERE sLNS LIKE '"+ sLNS + "%'";
            String SQL = String.Format("SELECT * FROM NS_MucLucNganSach {0} ORDER BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG",DK);
            DataTable dt = Connection.GetDataTable(SQL);
            String sXauNoiMa1 = "";
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                DataRow RMucLucNganSach = dt.Rows[i];
                string sXauNoiMa = "";
                

                Boolean bLaHangCha = false;
                for (int j = 0; j < arrDSTruong.Length - 1; j++)
                {
                    if (String.IsNullOrEmpty(Convert.ToString(RMucLucNganSach[arrDSTruong[j]])) == false)
                    {
                        if (sXauNoiMa != "") sXauNoiMa += "-";
                        sXauNoiMa += Convert.ToString(RMucLucNganSach[arrDSTruong[j]]);
                    }
                    else
                    {
                        if (j < arrDSTruong.Length - 2)
                        {
                            bLaHangCha = true;
                        }
                    }
                }
                if (i == dt.Rows.Count - 1)
                {
                    sXauNoiMa1 = sXauNoiMa;
                }
                if (sXauNoiMa1.StartsWith(sXauNoiMa) && sXauNoiMa1.Equals(sXauNoiMa)==false)
                {
                    bLaHangCha = true;                   
                }
                sXauNoiMa1 = sXauNoiMa;
                cmd = new SqlCommand("UPDATE NS_MucLucNganSach SET sXauNoiMa=@sXauNoiMa, bLaHangCha=@bLaHangCha WHERE iID_MaMucLucNganSach=@iID_MaMucLucNganSach");
                cmd.Parameters.AddWithValue("@sXauNoiMa", sXauNoiMa);
                cmd.Parameters.AddWithValue("@bLaHangCha", bLaHangCha);
                cmd.Parameters.AddWithValue("@iID_MaMucLucNganSach", RMucLucNganSach["iID_MaMucLucNganSach"]);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();
            }
            dt.Dispose();
            //Cap nhap lai ma cha
           // String.Format("SELECT * FROM NS_MucLucNganSach {0} ORDER BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG", DK);
            dt = Connection.GetDataTable(SQL);
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                DataRow RMucLucNganSach = dt.Rows[i];
                Object iID_MaMucLucNganSachCha = DBNull.Value;

                string sXauNoiMa = Convert.ToString(RMucLucNganSach["sXauNoiMa"]);

                for (int j = i - 1; j >= 0; j--)
                {
                    String tg_sXauNoiMa = Convert.ToString(dt.Rows[j]["sXauNoiMa"]);
                    if (sXauNoiMa.StartsWith(tg_sXauNoiMa))
                    {
                        cmd = new SqlCommand("UPDATE NS_MucLucNganSach SET iID_MaMucLucNganSach_Cha=@iID_MaMucLucNganSach_Cha WHERE iID_MaMucLucNganSach=@iID_MaMucLucNganSach");
                        cmd.Parameters.AddWithValue("@iID_MaMucLucNganSach", RMucLucNganSach["iID_MaMucLucNganSach"]);
                        cmd.Parameters.AddWithValue("@iID_MaMucLucNganSach_Cha", dt.Rows[j]["iID_MaMucLucNganSach"]);
                        Connection.UpdateDatabase(cmd);
                        cmd.Dispose();
                        break;
                    }
                }
            }
            dt.Dispose();
        }

        public static DataTable Get_dtDanhSachMucLucNganSach(Dictionary<String, String> arrGiaTriTimKiem)
        {
            String SQL = "SELECT * FROM NS_MucLucNganSach WHERE   {0} ORDER BY sXauNoiMa";
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            #region Điều kiện
            if (arrGiaTriTimKiem != null)
            {
                //DK = DK + " (";
                for (int i = 0; i < arrDSTruong.Length; i++)
                {
                    if (String.IsNullOrEmpty(arrGiaTriTimKiem[arrDSTruong[i]]) == false)
                    {
                        if (!String.IsNullOrEmpty(DK)) DK += " AND ";
                        DK += String.Format("{0}=@{0}", arrDSTruong[i]);
                        cmd.Parameters.AddWithValue("@" + arrDSTruong[i], arrGiaTriTimKiem[arrDSTruong[i]]);
                    }
                }

                String sLNS = arrGiaTriTimKiem["sLNS"];
                if (String.IsNullOrEmpty(sLNS) == false)
                {
                    //DK+=" OR sLNS='" + sLNS.Substring(0,1)+"'";
                    DK += String.Format(" OR sLNS LIKE '{0}%'", sLNS);
                    if (sLNS.Length >= 3)
                        DK += " OR sLNS='" + sLNS.Substring(0, 3) + "'";


                }
                DK = DK + ")";
            }
            #endregion Điều kiện
            if (String.IsNullOrEmpty(DK) == false)
            {
                DK = "iTrangThai=1 AND (" + DK;
            }
            SQL = String.Format(SQL, DK);
            cmd.CommandText = SQL;
            DataTable vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static DataTable Get_dtDanhSachMucLucNganSach_Nhom()
        {
            String SQL = "SELECT DISTINCT sLNS,sL + '-'+ sK + '-'+ sM as sM FROM NS_MucLucNganSach WHERE iTrangThai=1 AND sTM='' ORDER BY sLNS,sL,sK";
            SqlCommand cmd = new SqlCommand();
            
            cmd.CommandText = SQL;
            DataTable vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }



        public static DataTable Get_dtMucLucNganSach(int Trang = 1, int SoBanGhi = 0, String sLNS = "", String sL = "", String sK = "", String sM = "", String sTM = "", String sTTM = "", String sNG = "", String sTNG = "")
        {
            String SQL = "SELECT * FROM NS_MucLucNganSach {0} ";
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            #region Điều kiện
            if (String.IsNullOrEmpty(sLNS) == false)
            {
                if (String.IsNullOrEmpty(DK) == false) DK += " AND ";
                DK = "sLNS=@sLNS";
                cmd.Parameters.AddWithValue("@sLNS", sLNS);
            }
            if (String.IsNullOrEmpty(sL) == false)
            {
                if (String.IsNullOrEmpty(DK) == false) DK += " AND ";
                DK = "sL=@sL";
                cmd.Parameters.AddWithValue("@sL", sL);
            }
            if (String.IsNullOrEmpty(sK) == false)
            {
                if (String.IsNullOrEmpty(DK) == false) DK += " AND ";
                DK = "sK=@sK";
                cmd.Parameters.AddWithValue("@sK", sK);
            }
            if (String.IsNullOrEmpty(sM) == false)
            {
                if (String.IsNullOrEmpty(DK) == false) DK += " AND ";
                DK = "sM=@sM";
                cmd.Parameters.AddWithValue("@sM", sM);
            }
            if (String.IsNullOrEmpty(sTM) == false)
            {
                if (String.IsNullOrEmpty(DK) == false) DK += " AND ";
                DK = "sTM=@sTM";
                cmd.Parameters.AddWithValue("@sTM", sTM);
            }
            if (String.IsNullOrEmpty(sTTM) == false)
            {
                if (String.IsNullOrEmpty(DK) == false) DK += " AND ";
                DK = "sTTM=@sTTM";
                cmd.Parameters.AddWithValue("@sTTM", sTTM);
            }
            if (String.IsNullOrEmpty(sNG) == false)
            {
                if (String.IsNullOrEmpty(DK) == false) DK += " AND ";
                DK = "sNG=@sNG";
                cmd.Parameters.AddWithValue("@sNG", sNG);
            }
            if (String.IsNullOrEmpty(sTNG) == false)
            {
                if (String.IsNullOrEmpty(DK) == false) DK += " AND ";
                DK = "sTNG=@sTNG";
                cmd.Parameters.AddWithValue("@sTNG", sTNG);
            }

            #endregion Điều kiện

            if (String.IsNullOrEmpty(DK) == false) DK = " WHERE " + DK;
            SQL = String.Format(SQL, DK);
            cmd.CommandText = SQL;
            DataTable vR = CommonFunction.dtData(cmd, "iSTT", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_MucLucNganSach_Count(String sLNS = "", String sL = "", String sK = "", String sM = "", String sTM = "", String sTTM = "", String sNG = "", String sTNG = "")
        {
            String SQL = "SELECT COUNT(*) FROM NS_MucLucNganSach {0} ";
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            #region Điều kiện
            if (String.IsNullOrEmpty(sLNS) == false)
            {
                if (String.IsNullOrEmpty(DK) == false) DK += " AND ";
                DK = "sLNS=@sLNS";
                cmd.Parameters.AddWithValue("@sLNS", sLNS);
            }
            if (String.IsNullOrEmpty(sL) == false)
            {
                if (String.IsNullOrEmpty(DK) == false) DK += " AND ";
                DK = "sL=@sL";
                cmd.Parameters.AddWithValue("@sL", sL);
            }
            if (String.IsNullOrEmpty(sK) == false)
            {
                if (String.IsNullOrEmpty(DK) == false) DK += " AND ";
                DK = "sK=@sK";
                cmd.Parameters.AddWithValue("@sK", sK);
            }
            if (String.IsNullOrEmpty(sM) == false)
            {
                if (String.IsNullOrEmpty(DK) == false) DK += " AND ";
                DK = "sM=@sM";
                cmd.Parameters.AddWithValue("@sM", sM);
            }
            if (String.IsNullOrEmpty(sTM) == false)
            {
                if (String.IsNullOrEmpty(DK) == false) DK += " AND ";
                DK = "sTM=@sTM";
                cmd.Parameters.AddWithValue("@sTM", sTM);
            }
            if (String.IsNullOrEmpty(sTTM) == false)
            {
                if (String.IsNullOrEmpty(DK) == false) DK += " AND ";
                DK = "sTTM=@sTTM";
                cmd.Parameters.AddWithValue("@sTTM", sTTM);
            }
            if (String.IsNullOrEmpty(sNG) == false)
            {
                if (String.IsNullOrEmpty(DK) == false) DK += " AND ";
                DK = "sNG=@sNG";
                cmd.Parameters.AddWithValue("@sNG", sNG);
            }
            if (String.IsNullOrEmpty(sTNG) == false)
            {
                if (String.IsNullOrEmpty(DK) == false) DK += " AND ";
                DK = "sTNG=@sTNG";
                cmd.Parameters.AddWithValue("@sTNG", sTNG);
            }

            #endregion Điều kiện

            if (String.IsNullOrEmpty(DK) == false) DK= " WHERE " + DK;
            SQL = String.Format(SQL, DK);
            cmd.CommandText = SQL;
            int vR=Convert.ToInt16(Connection.GetValue(cmd,0));
            cmd.Dispose();

            return vR;
        }
    }
}