using PX.Data.BQL;

namespace OrderAttachments
{
    public class Constants
    {
        public class SalesOrdersScreenId : BqlType<IBqlString, string>.Constant<SalesOrdersScreenId>
        {
            public static readonly string screenId = "SO301000";
            public SalesOrdersScreenId() : base(screenId) { }
        }
        public class ShipmentsScreenId : BqlType<IBqlString, string>.Constant<ShipmentsScreenId>
        {
            public static readonly string screenId = "SO302000";
            public ShipmentsScreenId() : base(screenId) { }
        }
        public class InvoiceScreenId : BqlType<IBqlString, string>.Constant<InvoiceScreenId>
        {
            public static readonly string screenId = "SO303000";
            public InvoiceScreenId() : base(screenId) { }
        }
    }
}
