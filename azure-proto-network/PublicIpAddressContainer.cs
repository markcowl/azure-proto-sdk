﻿using Azure.ResourceManager.Network;
using Azure.ResourceManager.Network.Models;
using azure_proto_core;
using System.Threading;
using System.Threading.Tasks;

namespace azure_proto_network
{
    public class PublicIpAddressContainer : ResourceContainerOperations<PhPublicIPAddress>
    {
        public PublicIpAddressContainer(ArmClientBase parent, ResourceIdentifier context) : base(parent, context)
        {
        }

        public PublicIpAddressContainer(ArmClientBase parent, azure_proto_core.Resource context) : base(parent, context)
        {
        }

        protected override ResourceType ResourceType => "Microsoft.Network/publicIpAddresses";

        public override ArmOperation<ResourceClientBase<PhPublicIPAddress>> Create(string name, PhPublicIPAddress resourceDetails)
        {
            return new PhArmOperation<ResourceClientBase<PhPublicIPAddress>, PublicIPAddress>(Operations.StartCreateOrUpdate(Context.ResourceGroup, name, resourceDetails), 
                n => new PublicIpAddressOperations(this, new PhPublicIPAddress(n)));
        }

        public async override Task<ArmOperation<ResourceClientBase<PhPublicIPAddress>>> CreateAsync(string name, PhPublicIPAddress resourceDetails, CancellationToken cancellationToken = default)
        {
            return new PhArmOperation<ResourceClientBase<PhPublicIPAddress>, PublicIPAddress>(
                await Operations.StartCreateOrUpdateAsync(Context.ResourceGroup, name, resourceDetails, cancellationToken),
                n => new PublicIpAddressOperations(this, new PhPublicIPAddress(n)));
        }

        public PhPublicIPAddress ConstructIPAddress(Location location = null)
        {
            var ipAddress = new PublicIPAddress()
            {
                PublicIPAddressVersion = Azure.ResourceManager.Network.Models.IPVersion.IPv4.ToString(),
                PublicIPAllocationMethod = IPAllocationMethod.Dynamic,
                Location = location ?? DefaultLocation,
            };

            return new PhPublicIPAddress(ipAddress);
        }

        internal PublicIPAddressesOperations Operations => GetClient<NetworkManagementClient>((uri, cred) => new NetworkManagementClient(Context.Subscription, uri, cred)).PublicIPAddresses;

    }
}