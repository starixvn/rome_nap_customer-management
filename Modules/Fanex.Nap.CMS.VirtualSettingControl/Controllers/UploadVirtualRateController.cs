namespace Fanex.Nap.CMS.VirtualSettingControl.Controllers
{
    using System.IO;
    using System.Net.Mime;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Infrastructure;
    using Infrastructure.Controllers;
    using Infrastructure.Resources;
    using Models.UploadVirtualRate;
    using Resources;
    using Services.UploadVirtualRate;

    public class UploadVirtualRateController : BaseController
    {
        private readonly IESportsUploadService uploadService;

        public UploadVirtualRateController()
            : this(null)
        {
        }

        public UploadVirtualRateController(IESportsUploadService uploadService)
        {
            this.uploadService = uploadService ?? new ESportsUploadService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ESports_UploadVirtualRate, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View("~/Areas/VirtualSettingControl/Views/UploadVirtualRate/Index.cshtml");
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ESports_UploadVirtualRate, Policy = Constant.Policy.Read)]
        public ActionResult DownloadTemplate()
        {
            var path = Server.MapPath("~/App_Data/ExcelTemplates");
            const string fileName = "E-Sports-UploadVirtualRateTemplate.xlsx";
            var fileBytes = System.IO.File.ReadAllBytes($"{path}/{fileName}");

            return File(fileBytes, MediaTypeNames.Application.Octet, fileName);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ESports_UploadVirtualRate, Policy = Constant.Policy.Host)]
        public ActionResult Verify(HttpPostedFileBase file)
        {
            var errors = file == null || file.ContentLength == 0
                ? new ExcelErrorModel(new[] { new ExcelErrorDTO(Labels.Message_NoReportData) }, 0, 0, $"{ESportsResource.FormatError}: 1")
                : uploadService.Verify(
                    file.InputStream,
                    Path.GetExtension(file.FileName),
                    CurrentSession.UserId);

            return JsonData(errors, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ESports_UploadVirtualRate, Policy = Constant.Policy.Host)]
        public ActionResult Update()
            => JsonData(
                uploadService.Update(CurrentSession.UserId, CurrentContext.IP),
                JsonRequestBehavior.DenyGet);
    }
}
