using PX.Data;
using PX.Data.BQL;
using PX.Objects.SO;
using PX.SM;
using System;

namespace OrderAttachments
{
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
    // Acuminator disable once PX1011 InheritanceFromPXCacheExtension [Justification]
    [Serializable]
    [PXVirtual]
    [PXCacheName("UploadFileExtension")]
    public class OrderAttachment : IBqlTable
    {
        #region OrderType
        [PXDefault(typeof(SOOrder.orderType))]
        public string OrderType { get; set; }

        public abstract class orderType : BqlString.Field<orderType>
        {
        }
        #endregion

        #region OrderNbr
        [PXDefault(typeof(SOOrder.orderNbr))]
        public string OrderNbr { get; set; }

        public abstract class orderNbr : BqlString.Field<orderNbr>
        {
        }
        #endregion

        #region FileName
        [PXDefault(typeof(UploadFile.name))]
        public string FileName { get; set; }
        public abstract class fileName : BqlString.Field<fileName>
        {
        }
        #endregion

        #region FileID
        [PXDefault(typeof(UploadFile.fileID))]
        public Guid? FileID { get; set; }
        public abstract class fileID : BqlType<IBqlGuid, Guid>.Field<fileID>
        {
        }
        #endregion

        #region CreatedByID
        [PXDBCreatedByID()]
        public virtual Guid? CreatedByID { get; set; }
        public abstract class createdByID : IBqlField { }
        #endregion

        #region CreatedByScreenID
        [PXDBCreatedByScreenID()]
        public virtual string CreatedByScreenID { get; set; }
        public abstract class createdByScreenID : IBqlField { }
        #endregion

        #region CreatedDateTime
        [PXDBCreatedDateTime()]
        [PXUIField(DisplayName = "Created Date Time")]
        public virtual DateTime? CreatedDateTime { get; set; }
        public abstract class createdDateTime : IBqlField { }
        #endregion

        #region LastModifiedByID
        [PXDBLastModifiedByID()]
        public virtual Guid? LastModifiedByID { get; set; }
        public abstract class lastModifiedByID : IBqlField { }
        #endregion

        #region LastModifiedByScreenID
        [PXDBLastModifiedByScreenID()]
        public virtual string LastModifiedByScreenID { get; set; }
        public abstract class lastModifiedByScreenID : IBqlField { }
        #endregion

        #region LastModifiedDateTime
        [PXDBLastModifiedDateTime()]
        [PXUIField(DisplayName = "Last Modified Date Time")]
        public virtual DateTime? LastModifiedDateTime { get; set; }
        public abstract class lastModifiedDateTime : IBqlField { }
        #endregion

        #region Tstamp
        [PXDBTimestamp()]
        [PXUIField(DisplayName = "Tstamp")]
        public virtual byte[] Tstamp { get; set; }
        public abstract class tstamp : IBqlField { }
        #endregion 
    }
}
