using System.Text;

namespace TELSR200Emulator.Messages.PreAligner
{
    public class ResponseINIT : BaseResponse
    {
        public ResponseINIT(CommandINIT request) : base(request)
        {
            _request = request;
        }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();
            CommandINIT req = (CommandINIT)_request;
            _responseBuilder.Append(req.ErrorClear ? "1" : "0");
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.ServoOn ? "1" : "0");
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.HomeAxis);

            return base.Generate(device);
        }
    }
}
