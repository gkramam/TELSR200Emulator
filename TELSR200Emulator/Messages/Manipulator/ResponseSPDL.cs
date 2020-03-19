using System.Text;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class ResponseSPDL : BaseResponse
    {
        public ResponseSPDL(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            CommandSPDL req = (CommandSPDL)_request;

            _responseBuilder.Append(req.MemorySpec);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.TransferStation);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Posture);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Hand);

            return base.Generate(device);
        }
    }
}
