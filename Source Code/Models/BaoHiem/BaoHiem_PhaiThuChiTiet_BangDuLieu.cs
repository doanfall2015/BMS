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

namespace VIETTEL.Models
{
    /// <summary>
    /// Lớp BaoHiemChi_BangDuLieu cho phần bảng của Bảo hiểm chi
    /// </summary>
    public class BaoHiem_PhaiThuChiTiet_BangDuLieu:BangDuLieu
    {
        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="iID_MaBaoHiemPhaiThu"></param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="IPSua">IP của máy yêu cầu</param>
        public BaoHiem_PhaiThuChiTiet_BangDuLieu(String iID_MaBaoHiemPhaiThu, Dictionary<String, String> arrGiaTriTimKiem, String MaND, String IPSua)
        {
            this._iID_Ma = iID_MaBaoHiemPhaiThu;
            this._MaND = MaND;
            this._IPSua = IPSua;

            String SQL;
            SqlCommand cmd;
            SQL = "SELECT * FROM BH_PhaiThuChungTu WHERE iID_MaBaoHiemPhaiThu=@iID_MaBaoHiemPhaiThu AND iTrangThai=1";
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaBaoHiemPhaiThu", _iID_Ma);
            cmd.CommandText = SQL;
            _dtBang = Connection.GetDataTable(cmd);
            cmd.Dispose();

            int iID_MaTrangThaiDuyet = Convert.ToInt32(_dtBang.Rows[0]["iID_MaTrangThaiDuyet"]);

            Boolean ND_DuocSuaChungTu = LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeBaoHiem, MaND, iID_MaTrangThaiDuyet);
            if (LuongCongViecModel.KiemTra_TrangThaiDaDuyet(PhanHeModels.iID_MaPhanHeBaoHiem, iID_MaTrangThaiDuyet))
            {
                _ChiDoc = true;
            }

            if (ND_DuocSuaChungTu == false)
            {
                _ChiDoc = true;
            }

            if (LuongCongViecModel.KiemTra_TrangThaiTrinhDuyet(PhanHeModels.iID_MaPhanHeBaoHiem, iID_MaTrangThaiDuyet) &&
                ND_DuocSuaChungTu)
            {
                _CoCotDuyet = true;
                _DuocSuaDuyet = true;
            }

            if (LuongCongViecModel.KiemTra_TrangThaiTuChoi(PhanHeModels.iID_MaPhanHeBaoHiem, iID_MaTrangThaiDuyet))
            {
                _CoCotDuyet = true;
            }

            _DuocSuaChiTiet = LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHeBaoHiem, MaND);

            _dtChiTiet = BaoHiem_PhaiThuChiTietModels.Get_dtBaoHiemChiChiTiet(_iID_Ma, arrGiaTriTimKiem);
            _dtChiTiet_Cu = _dtChiTiet.Copy();
            DienDuLieu();
        }

        /// <summary>
        /// Hàm điền dữ liệu
        /// Mục đích: Điền tất cả thông tin vào các tham số của đối tượng Bảng dữ liệu
        /// </summary>
        protected void DienDuLieu()
        {
            if (_arrDuLieu == null)
            {
                CapNhapDanhSachMaHang();
                CapNhapDanhSachMaCot_Fixed();
                CapNhapDanhSachMaCot_Slide();
                CapNhapDanhSachMaCot_Them();
                CapNhap_arrLaHangCha();
                CapNhap_arrEdit();
                CapNhap_arrDuLieu();
                CapNhap_arrThayDoi();
            }
        }

        /// <summary>
        /// Hàm cập nhập vào tham số _arrDSMaHang
        /// </summary>
        protected void CapNhapDanhSachMaHang()
        {
            _arrDSMaHang = new List<string>();
            for (int i = 0; i < _dtChiTiet.Rows.Count; i++)
            {
                DataRow R = _dtChiTiet.Rows[i];
                String MaHang = Convert.ToString(R["iID_MaBaoHiemPhaiThuChiTiet"]);
                _arrDSMaHang.Add(MaHang);
            }
        }

        /// <summary>
        /// Hàm thêm danh sách cột Fixed vào bảng
        ///     - Cột Fixed của bảng gồm:
        ///         + Các trường của mục lục ngân sách
        ///     - Cập nhập số lượng cột Fixed
        /// </summary>
        protected void CapNhapDanhSachMaCot_Fixed()
        {
            //Khởi tạo các mảng
            _arrHienThiCot = new List<Boolean>();
            _arrTieuDe = new List<string>();
            _arrDSMaCot = new List<string>();
            _arrWidth = new List<int>();

            String[] arrDSTruong;
            String[] arrDSTruongTieuDe;
            String[] arrDSTruongDoRong;


            arrDSTruong = "sMoTa".Split(',');
            arrDSTruongTieuDe = "Nội dung".Split(',');
            arrDSTruongDoRong = "200".Split(',');
            
            //Tiêu đề fix
            for (int j = 0; j < arrDSTruongTieuDe.Length; j++)
            {
                _arrDSMaCot.Add(arrDSTruong[j]);
                _arrTieuDe.Add(arrDSTruongTieuDe[j]);
                _arrWidth.Add(Convert.ToInt32(arrDSTruongDoRong[j]));             
                _arrHienThiCot.Add(true);
                _arrSoCotCungNhom.Add(1);
                _arrTieuDeNhomCot.Add("");
            }

            _nCotFixed = _arrDSMaCot.Count;
        }

        /// <summary>
        /// Hàm thêm danh sách cột Slide vào bảng
        ///     - Cột Slide của bảng gồm:
        ///         + Trường của cột tiền
        ///         + Trường bDongY, sLyDo
        ///     - Cập nhập số lượng cột Slide
        /// </summary>
        protected void CapNhapDanhSachMaCot_Slide()
        {
            String[] arrDSTruongTien;
            String[] arrDSTruongTienTieuDe;
            String[] arrDSTruongTienDoRong;
            arrDSTruongTien = "rSoNguoi,rLuongCoBan,rThamNien,rPCChucVu,rOmDauNgan,rKhac,rTongSo".Split(',');
            arrDSTruongTienTieuDe = "Số người,Lương cơ bản,Thâm niên,Chức vụ,Ốm ngắn,Khác, Tổng cộng".Split(',');
            arrDSTruongTienDoRong = "100,100,100,100,100,100,100".Split(',');

            //Tiêu đề tiền
            for (int j = 0; j < arrDSTruongTien.Length; j++)
            {
                _arrDSMaCot.Add(arrDSTruongTien[j]);
                _arrTieuDe.Add(arrDSTruongTienTieuDe[j]);
                _arrWidth.Add(Convert.ToInt32(arrDSTruongTienDoRong[j]));
                _arrHienThiCot.Add(true);
                _arrSoCotCungNhom.Add(1);
                _arrTieuDeNhomCot.Add("");
            }

            //Them cot duyet
            if (CoCotDuyet)
            {
                //Cột đồng ý
                _arrDSMaCot.Add("bDongY");
                if (_ChiDoc)
                {
                    _arrTieuDe.Add("<div class='check'></div>");
                }
                else
                {
                    _arrTieuDe.Add("<div class='check' onclick='BangDuLieu_CheckAll();'></div>");
                }
                _arrWidth.Add(20);
                _arrHienThiCot.Add(true);
                _arrSoCotCungNhom.Add(1);
                _arrTieuDeNhomCot.Add("");
                //Cột Lý do
                _arrDSMaCot.Add("sLyDo");
                _arrTieuDe.Add("Nhận xét");
                _arrWidth.Add(200);
                _arrHienThiCot.Add(true);
                _arrSoCotCungNhom.Add(1);
                _arrTieuDeNhomCot.Add("");
            }

            _nCotSlide = _arrDSMaCot.Count - _nCotFixed;
        }

        /// <summary>
        /// Hàm thêm các cột thêm của bảng
        /// </summary>
        protected void CapNhapDanhSachMaCot_Them()
        {
            
            //String[] arrDSTruong = "rTongCong".Split(',');
            //String[] arrDSTruongTieuDe = "Tổng cộng".Split(',');
            //String[] arrDSTruongDoRong = "100".Split(',');
            //for (int j = 0; j < arrDSTruong.Length; j++)
            //{
            //    if (String.IsNullOrEmpty(arrDSTruong[j]) == false)
            //    {
            //        _arrDSMaCot.Add(arrDSTruong[j]);
            //        _arrTieuDe.Add(arrDSTruongTieuDe[j]);
            //        _arrWidth.Add(Convert.ToInt32(arrDSTruongDoRong[j]));
            //        _arrHienThiCot.Add(true);
            //        _arrSoCotCungNhom.Add(1);
            //        _arrTieuDeNhomCot.Add("");
            //    }
            //}
        }


        /// <summary>
        /// Hàm xác định hàng cha, con
        /// </summary>
        protected void CapNhap_arrLaHangCha()
        {
            //Xác định hàng là hàng cha, con
            _arrCSCha = new List<int>();
            _arrLaHangCha = new List<bool>();           
            for (int i = 0; i < _dtChiTiet.Rows.Count; i++)
            {
                DataRow R = _dtChiTiet.Rows[i];
                //Xac dinh hang nay co phai la hang cha khong?
                _arrLaHangCha.Add(Convert.ToBoolean(R["bLaHangCha"]));
                int CSCha = -1;
                for (int j = i - 1; j >= 0; j--)
                {
                    if (Convert.ToString(R["iID_MaMucLucDoiTuong_Cha"]) == Convert.ToString(_dtChiTiet.Rows[j]["iID_MaMucLucDoiTuong"]))
                    {
                        CSCha = j;
                        break;
                    }
                }
                _arrCSCha.Add(CSCha);
            }
        }

        /// <summary>
        /// Hàm xác định các ô có được Edit hay không
        /// </summary>
        protected void CapNhap_arrEdit()
        {
            _arrEdit = new List<List<string>>();
            for (int i = 0; i < _dtChiTiet.Rows.Count; i++)
            {
                Boolean okHangChiDoc = false;
                _arrEdit.Add(new List<string>());
                DataRow R = _dtChiTiet.Rows[i];

                if (Convert.ToBoolean(R["bLaHangCha"]))
                {
                    okHangChiDoc = true;
                }

                for (int j = 0; j < _arrDSMaCot.Count; j++)
                {
                    Boolean okOChiDoc = true;
                    //Xac dinh o chi doc
                    if (_arrDSMaCot[j] == "bDongY" || _arrDSMaCot[j] == "sLyDo")
                    {
                        //Cot duyet
                        if (_DuocSuaDuyet && _ChiDoc == false && okHangChiDoc == false)
                        {
                            okOChiDoc = false;
                        }
                    }
                    else
                    {
                        //Cot tien
                        if (_DuocSuaChiTiet && _arrDSMaCot[j] != "rTongSo" &&
                                _ChiDoc == false &&
                                okHangChiDoc == false)
                        {
                            okOChiDoc = false;
                        }
                    }
                    if (okOChiDoc)
                    {
                        _arrEdit[i].Add("");
                    }
                    else
                    {
                        _arrEdit[i].Add("1");
                    }
                }
            }
        }

        /// <summary>
        /// Hàm cập nhập mảng dữ liệu
        /// </summary>
        protected void CapNhap_arrDuLieu()
        {
            _arrDuLieu = new List<List<string>>();
            for (int i = 0; i < _dtChiTiet.Rows.Count; i++)
            {
                _arrDuLieu.Add(new List<string>());
                DataRow R = _dtChiTiet.Rows[i];
                for (int j = 0; j < _arrDSMaCot.Count; j++)
                {
                    //Xac dinh gia tri
                    _arrDuLieu[i].Add(Convert.ToString(R[_arrDSMaCot[j]]));
                }
            }
        }
    }
}