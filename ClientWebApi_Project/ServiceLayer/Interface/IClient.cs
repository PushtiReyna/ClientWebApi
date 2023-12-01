using DTO.Client;
using DTO.Login;
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
    }
}
