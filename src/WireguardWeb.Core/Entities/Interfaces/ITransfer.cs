namespace WireguardWeb.Core.Entities.Interfaces;

public interface ITransfer<T>
{
    public T ToTransfer();
    public void ChangeOf(T transfer);
}