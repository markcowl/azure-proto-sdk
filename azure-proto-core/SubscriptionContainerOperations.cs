using Azure;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Resources.Models;
using azure_proto_core.Adapters;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace azure_proto_core
{
    /// <summary>
    /// Subscription Container Operations
    /// </summary>
    public class SubscriptionContainerOperations : OperationsBase
    {
        public static readonly string AzureResourceType = "Microsoft.Resources/subscriptions";

        public SubscriptionContainerOperations(ArmClientContext context) : base(context, ResourceIdentifier.KnownKeys.Subscription) { }

        public override ResourceType ResourceType => AzureResourceType;

        public Pageable<SubscriptionOperations> List(CancellationToken cancellationToken = default)
        {
            return new PhWrappingPageable<Subscription, SubscriptionOperations>(
                Operations.List(cancellationToken),
                this.convertor());
        }

        public AsyncPageable<SubscriptionOperations> ListAsync(CancellationToken cancellationToken = default)
        {
            return new PhWrappingAsyncPageable<Subscription, SubscriptionOperations>(
                Operations.ListAsync(cancellationToken),
                this.convertor());
        }

        private Func<Subscription, SubscriptionOperations> convertor()
        {
            return s => new SubscriptionOperations(ClientContext, new PhSubscriptionModel(s));
        }

        internal async Task<string> GetDefaultSubscription(CancellationToken token = default(CancellationToken))
        {
            var subs = ListAsync(token).GetAsyncEnumerator();
            string sub = null;
            if (await subs.MoveNextAsync())
            {
                if (subs.Current != null)
                {
                    sub = subs.Current.Id.Subscription;
                }
            }
            return sub;
        }

        internal SubscriptionsOperations Operations => GetClient<ResourcesManagementClient>((uri, cred) => new ResourcesManagementClient(uri, Guid.NewGuid().ToString(), cred)).Subscriptions;
    }
}