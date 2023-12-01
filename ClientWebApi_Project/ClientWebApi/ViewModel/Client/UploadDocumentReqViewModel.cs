namespace ClientWebApi.ViewModel.Client
{
    public class UploadDocumentReqViewModel
    {
        public string? Token { get; set; }
        public string? DocumentName { get; set; }
        public IFormFile Document { get; set; }
    }
}
