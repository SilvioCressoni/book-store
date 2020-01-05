namespace Users.Web.Proto
{
    public partial class RemoveAddressReplay : IReply<VoidType>
    {
        VoidType IReply<VoidType>.Value => null;
    }
}
