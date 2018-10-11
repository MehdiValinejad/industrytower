using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndustryTower.ViewModels
{
    public class POSPaymentViewModel
    {
    }

    public class BPPayRequest
    {
        public long terminalID { get; set; }
        public string userName { get; set; }
        public string userPassword { get; set; }
        public long orderID { get; set; }
        public long amount { get; set; }
        public string localDate  { get; set; }
        public string localTime { get; set; }

        public string additionalData { get; set; }
        public string callBackUrl { get; set; }
        public string payerId { get; set; }
    }


    public class BPPayValidate
    {
        public string RefID { get; set; }
        public string ResCode { get; set; }
        public long saleOrderId { get; set; }
        public long saleReferenceId { get; set; }

    }

    public class BPVerifyRequest
    {
        public long terminalID { get; set; }
        public string userName { get; set; }
        public string userPassword { get; set; }
        public long orderID { get; set; }
        public long saleOrderId { get; set; }
        public long saleReferenceId { get; set; }

    }


    public class BPSettleRequest
    {
        public long terminalID { get; set; }
        public string userName { get; set; }
        public string userPassword { get; set; }
        public long orderID { get; set; }
        public long saleOrderId { get; set; }
        public long saleReferenceId { get; set; }

    }


    public class BPInquiryRequest
    {
        public long terminalID { get; set; }
        public string userName { get; set; }
        public string userPassword { get; set; }
        public long orderID { get; set; }
        public long saleOrderId { get; set; }
        public long saleReferenceId { get; set; }

    }
}