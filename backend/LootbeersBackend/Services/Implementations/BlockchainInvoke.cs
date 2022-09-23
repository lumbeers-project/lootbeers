using Microsoft.AspNetCore.Components;
using Solnet.Rpc;
using Solnet.Rpc.Core.Http;
using Solnet.Rpc.Models;
using Solnet.Rpc.Types;

namespace LootbeersBackend.Services.Implementations;

public class BlockchainInvoke : IBlockchainInvoke
{
    public BlockchainInvoke(IRpcClient rpcClient)
    {
        RpcClient = rpcClient;
    }
    
    public IRpcClient RpcClient { get; set; }


    public async Task<string> SendTransaction(Transaction tx)
    {
        string txId;

        try
        {
            RequestResult<string> sendResponse = await RpcClient.SendTransactionAsync(tx.Serialize(), commitment: Commitment.Confirmed);
            txId = sendResponse.Result ?? throw new NullReferenceException("Transaction id wasnt found in response");
        }
        catch (Exception e)
        {
            throw new BlockchainInvokeException("An error occured, see inner exception for details", e);
        }

        try
        {
            await WaitForConfirmation(txId).WaitAsync(TimeSpan.FromSeconds(30));
        }
        catch (TaskCanceledException)
        {
            throw new BlockchainInvokeException("Transaction was not confirmed after a while");
        }

        return txId;
    }

    private async Task WaitForConfirmation(string txId)
    {
        while (true)
        {
            var response = await RpcClient.GetTransactionAsync(txId, Commitment.Confirmed);

            if (response.Result is null)
            {
                await Task.Delay(1000);
                continue;
            }

            if (response.Result.Meta.Error is not null)
            {
                throw new BlockchainInvokeException("An error occured while sending a transaction");
            }

            break;
        }
    }
}