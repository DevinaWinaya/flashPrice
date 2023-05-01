using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using HCPLUSFx;

namespace KopkariFX.PinjamanTxn
{
    public class BOPinjamanTxn : BOBase
    {
        public string pinjamanID { set; get; }
        public string pinjamanNo { set; get; }
        public decimal amount { set; get; }
        public string txnStatus { set; get; }
        public string statusName { set; get; }
        public DateTime txnDate { set; get; }

        public string userID { set; get; }
        public string anggotaNIP { set; get; }
        public string anggotaName { set; get; }
        public string empTypeID { set; get; }
        public string classID { set; get; }

        public int tenor { set; get; }
        public decimal cicilanPokok { set; get; }
        public decimal bunga { set; get; }





        public string typeID { set; get; }
        public string packetID { set; get; }
        public string tujuan { set; get; }
        public string isDocComplete { set; get; }
        public string notes { set; get; }

        public string compID { set; get; }
        public string compName { set; get; }

        public string locID { set; get; }
        public string locName { set; get; }

        public string jobTitleID { set; get; }
        public string jobTitleName { set; get; }

        public int jmlCicil { get; set; }

        public string jobLevelID { set; get; }
        public string orgID { set; get; }
        public string superiorID { set; get; }
        public string superiorName { set; get; }

        public long hrdID { set; get; }
        public string bpuID { set; get; }

        public string alasanID { set; get; }
        public string description { set; get; }

        /// <summary>
        /// untuk bedain dari anggota ato admin
        /// </summary>
        public string txnOrigin { set; get; }

        /// <summary>
        /// bedain dari web & apps
        /// </summary>
        public string txnRequestFrom { set; get; }
        public DateTime? pencairanDate { set; get; }
        public DateTime? pelunasanDate { set; get; }

        public string hrdNIP { set; get; }

        public string packetName { set; get; }
        public string statusByAnggota { get; set; }

        public string superSuperiorID { get; set; }
        public string superSuperiorName { get; set; }

        //public DateTime entryDate { set; get; }
        //public string entryOperator { set; get; }
        //public DateTime lastUpdate { set; get; }
        //public string lastOperator { set; get; }

        public string txnStatusPNJStyle
        {
            get
            {
                switch (txnStatus)
                {
                    case "PSA001":
                        return "badge badge-pill badge-danger col-md-12";

                    case "PSA002":
                        return "badge badge-pill badge-secondary col-md-12";

                    case "PSA003":
                        return "badge badge-pill badge-success col-md-12";

                    case "PSA004":
                        return "badge badge-pill badge-danger col-md-12";

                    case "PSA005":
                        return "badge badge-pill badge-success col-md-12";

                    case "PSA006":
                        return "badge badge-pill badge-danger col-md-12";

                    case "PSA007":
                        return "badge badge-pill badge-success col-md-12";

                    case "PSA008":
                        return "badge badge-pill badge-primary col-md-12";

                    case "PSA009":
                        return "badge badge-pill badge-primary col-md-12";

                    case "PSA010":
                        return "badge badge-pill badge-success col-md-12";

                    case "PSA011":
                        return "badge badge-pill badge-warning col-md-12";

                    case "PSA012":
                        return "badge badge-pill badge-warning col-md-12";

                    case "PSA013":
                        return "badge badge-pill badge-success col-md-12";

                    case "PSA014":
                        return "badge badge-pill badge-danger col-md-12";

                    case "PSA015":
                        return "badge badge-pill badge-warning col-md-12";

                    case "PSA016":
                        return "badge badge-pill badge-success col-md-12";

                    case "PSA017":
                        return "badge badge-pill badge-danger col-md-12";

                    case "PSA018":
                        return "badge badge-pill badge-primary col-md-12";

                    case "PSA019":
                        return "badge badge-pill badge-primary col-md-12";

                    case "PSA020":
                        return "badge badge-pill badge-success col-md-12";

                    case "PSA021":
                        return "badge badge-pill badge-success col-md-12";

                    case "PSA022":
                        return "badge badge-pill badge-success col-md-12";

                    default:
                        return txnStatus;
                }
            }
        }


        public BOProcessResult validateAsTrainingResult(int xRowData)
        {
            BOProcessResult xResult = new BOProcessResult("BOPinjamanTxn.validateAsTrainingResult");

            xResult.xMessage = null;

            if (string.IsNullOrWhiteSpace(anggotaNIP))
            {
                xResult.xMessageList.Add("Row:" + xRowData.ToString() + ":nip anggota harus diisi");
            }
            if (string.IsNullOrWhiteSpace(anggotaName))
            {
                xResult.xMessageList.Add("Row:" + xRowData.ToString() + ":nama anggota harus diisi");
            }
            if (string.IsNullOrWhiteSpace(packetID))
            {
                xResult.xMessageList.Add("Row:" + xRowData.ToString() + ":paket ID harus diisi");
            }
            if (amount == 0)
            {
                xResult.xMessageList.Add("Row:" + xRowData.ToString() + ":amount harus diisi");
            }
            if (txnDate == DateTime.MinValue)
            {
                xResult.xMessageList.Add("Row:" + xRowData.ToString() + ":tanggal pinjaman harus diisi dengan format TEXT dan berbentuk yyyy-MM-dd");
            }
            if (tenor == 0)
            {
                xResult.xMessageList.Add("Row:" + xRowData.ToString() + ":tenor harus diisi");
            }
            if (string.IsNullOrWhiteSpace(alasanID))
            {
                xResult.xMessageList.Add("Row:" + xRowData.ToString() + ":alasan ID harus diisi");
            }
            if (string.IsNullOrWhiteSpace(tujuan))
            {
                xResult.xMessageList.Add("Row:" + xRowData.ToString() + ":tujuan harus diisi");
            }
            if (string.IsNullOrWhiteSpace(lastOperator))
            {
                xResult.xMessageList.Add("Row:" + xRowData.ToString() + ":lastOperator tidak ditemukan, silahkan login ulang");
            }
            if (xResult.xMessageList.Count == 0)
            {
                xResult.isSuccess = true;
            }

            return xResult;
        }





        #region keperluan analis kredit


        /* logic             
        * -. ambil list yang harus di approve analis kredit
        * -. tentuin perlu ambil pinjaman lain atau gk
        * -. ambil pinjaman lain, 
        * -. ambil pinjaman DKI
        * -. ambil loan
        * -. ambil gaji
        * -. kalkulasi 30% ==> ada toleransi gk?misal 10rb dll
        * -. tampilkan analisa ke grid. 
        * -. wait confirm dari manusia
        * -. looping approve analis kredit sambil write log --> pinjamanID, amount, cicilan per bulan, pinjaman lain, pinjaman DKI, loan, total cicilan pinjaman per bulan, limit 30%gaji, catatan analisa perhitungan
        * */

        public decimal cicilanTotal
        {
            get
            {
                return cicilanPokok + bunga;
            }
        }

        public bool isAllowAutoBypassSuperior { set; get; }
        public bool isAllowAutoBypassAnalystCredit { set; get; }
        public string analystCreditMethod { set; get; }


        public decimal amountPaid { get; set; }
        public int tenorPaid { get; set; }


        public decimal sisaPokok
        {
            get
            {
                return this.amount - this.amountPaid;
            }
        }


        //ini termasuk bunga 1x 
        public decimal amountMustPay
        {
            get
            {
                return this.amount - this.amountPaid
                       + (this.bunga / 12 / 100
                       * this.amount);
            }
        }


        public override string ToString()
        {
            return
               "ID:" + this.pinjamanID
                + System.Environment.NewLine + "No:" + this.pinjamanNo
                + System.Environment.NewLine + "Status:" + this.statusName
                + System.Environment.NewLine + "Jml:" + this.amount.ToString("N0")
                + System.Environment.NewLine + "Tenor:" + this.tenor.ToString("N0")
                + System.Environment.NewLine + "Cicilan:" + this.cicilanTotal.ToString("N0")
                + System.Environment.NewLine + "Sisa Tenor:" + (this.tenor - this.tenorPaid).ToString("N0")
                + System.Environment.NewLine + "Sisa Harus Bayar:" + amountMustPay.ToString("N0");
        }


        public string ToStringHTML()
        {
            return ToString().Replace(System.Environment.NewLine, "<br />");
        }




        #endregion



    }
}
