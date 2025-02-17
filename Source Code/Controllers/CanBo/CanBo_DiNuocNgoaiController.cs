﻿using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel.Controls;
using DomainModel;
using System.Collections.Specialized;
using VIETTEL.Models;
using System.Text;

namespace VIETTEL.Controllers.NhanSu
{
    public class CanBo_DiNuocNgoaiController : Controller
    {
        //
        // GET: /DiNuocNgoai/

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Thêm dữ liệu từ bảng người phụ thuộc
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="OnSuccess"></param>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public JavaScriptResult Edit_Fast_DiNuocNgoai_Submit(String ParentID, String OnSuccess, String iID_MaCanBo, String iID_MaDiNuocNgoai)
        {
            //Insert into data vào bảng: KT_ChungTuChiTiet
            Bang bang = new Bang("CB_DiNuocNgoai");
            bang.TruyenGiaTri(ParentID, Request.Form);
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaCanBo", iID_MaCanBo);
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                bang.Save();
            }
            else
            {
                bang.GiaTriKhoa = iID_MaDiNuocNgoai;
                bang.Save();
            }

            String strJ = "";
            if (String.IsNullOrEmpty(OnSuccess) == false)
            {
                strJ = String.Format("Dialog_close('{0}');{1}();", ParentID, OnSuccess);
            }
            else
            {
                strJ = String.Format("Dialog_close('{0}');", ParentID);
            }
            return JavaScript(strJ);
        }

        /// <summary>
        /// Xóa thông tin
        /// </summary>
        /// <param name="iID_MaQuaTrinhCongTac"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Delete(String iID_MaDiNuocNgoai, String iID_MaCanBo)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "CB_DiNuocNgoai", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            //Xóa dữ liệu trong bảng người phụ thuộc
            Bang bang = new Bang("CB_DiNuocNgoai");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            bang.TruongKhoa = "iID_MaDiNuocNgoai";
            bang.GiaTriKhoa = iID_MaDiNuocNgoai;
            bang.Delete();
            return RedirectToAction("Edit", "CanBo_HoSoNhanSu", new
            {
                iID_MaCanBo = iID_MaCanBo
            });
        }
    }
}
