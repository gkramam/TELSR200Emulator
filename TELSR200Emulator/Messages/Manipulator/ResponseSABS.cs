using System.Text;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class ResponseSABS : BaseResponse
    {
        public ResponseSABS(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            CommandSABS req = (CommandSABS)_request;

            _responseBuilder.Append(req.MemorySpec);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.RegistrationMode);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.TransferStation);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Posture);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Hand);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.RotationAxis);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.ExtensionAxis);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.WristAxis1);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.WristAxis2);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.ElevationAxis);

            return base.Generate(device);
        }
    }
}
