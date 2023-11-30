using DTO.Client;
using Helper.CommonModel;

namespace ServiceLayer.Interface
{
    public interface IClient
    {
        public CommonResponse GetClient();
        public CommonResponse AddClient(AddClientReqDTO addClientReqDTO);

        public CommonResponse UpdateClient(UpdateClientReqDTO updateClientReqDTO);

        public CommonResponse DeleteClient(DeleteClientReqDTO deleteClientReqDTO);

        public CommonResponse UploadDocument(UploadDocumentReqDTO uploadDocumentReqDTO);

        public CommonResponse Login(LoginReqDTO loginReqDTO);

        public CommonResponse SalaryClient(SalaryClientReqDTO salaryClientReqDTO);

        public CommonResponse Refresh(RefreshReqDTO refreshReqDTO);
    }
}
