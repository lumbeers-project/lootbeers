using System.Runtime.Serialization;
using Solnet.Rpc.Models;

namespace LootbeersBackend.Services;

public class BlockchainInvokeException : Exception
{
    public BlockchainInvokeException()
    {
    }

    protected BlockchainInvokeException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public BlockchainInvokeException(string? message) : base(message)
    {
    }

    public BlockchainInvokeException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

public interface IBlockchainInvoke
{
    Task<string> SendTransaction(Transaction tx);
}