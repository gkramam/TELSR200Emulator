using System.Text;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class ResponseSMSK : BaseResponse
    {
        public ResponseSMSK(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            CommandSMSK req = (CommandSMSK)_request;

            _responseBuilder.Append(req.Valid);

            return base.Generate(device);
        }
    }
}
