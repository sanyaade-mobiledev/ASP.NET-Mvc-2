namespace Microsoft.Web.Preview.UI.Controls.WebParts {
    using System.Web.UI.WebControls.WebParts;

    internal interface IWebPartZone {
        WebPartCollection WebParts { get; }
        PartChromeType GetEffectiveChromeType(Part part);
    }
}
