namespace Users.Web.Proto
{
    public partial class RemovePhoneReplay : IReply<VoidType>
    {
        VoidType IReply<VoidType>.Value => null;
    }
}
