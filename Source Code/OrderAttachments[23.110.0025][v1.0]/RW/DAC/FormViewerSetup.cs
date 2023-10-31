using PX.Data;
using PX.SM;
using System;

namespace ReportViewer.DAC
{
    [Serializable]
    [PXCacheName("ReportViewer Configuration")]
    public class FormViewerSetup : IBqlTable
    {
        #region UserName

        [PXDBString(30, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "User Name", Required = true)]
        [PXSelector(typeof(Search<Users.username>), typeof(Users.email))]
        public string UserName { get; set; }
        public abstract class userName : IBqlField { }

        #endregion

        #region SSAPIPassword
        [PXDBDefault]
        [PXRSACryptString(512, IsUnicode = true, IsViewDecrypted = true)]
        [PXUIField(DisplayName = "Password")]
        public string Password { get; set; }
        public abstract class password : IBqlField { }

        #endregion

        #region SSConfirmAPIPassword
        [PXDBDefault]
        [PXRSACryptString(512, IsUnicode = true, InputMask = "", IsViewDecrypted = true)]
        [PXUIField(DisplayName = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        public abstract class confirmPassword : IBqlField { }

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