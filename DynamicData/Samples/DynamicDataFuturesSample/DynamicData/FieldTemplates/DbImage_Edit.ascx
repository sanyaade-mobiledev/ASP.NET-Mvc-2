<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DbImage_Edit.ascx.cs" Inherits="DynamicDataFuturesSample.DbImage_Edit" %>
<asp:PlaceHolder ID="PlaceHolderImage" runat="server" Visible="false">
    <asp:Image ID="ImageEdit" runat="server" /><br />
</asp:PlaceHolder>
<asp:FileUpload ID="FileUploadEdit" runat="server" />