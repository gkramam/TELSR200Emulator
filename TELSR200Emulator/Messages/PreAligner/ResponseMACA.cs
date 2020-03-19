using System.Text;

namespace TELSR200Emulator.Messages.PreAligner
{
    public class ResponseMACA:BaseResponse
    {
        public ResponseMACA(CommandMACA req) : base(req) { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            CommandMACA req = (CommandMACA)_request;

            _responseBuilder.Append(req.Mode);

            return base.Generate(device);
        }
    }
}
