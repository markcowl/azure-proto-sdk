﻿using Azure;
using Azure.ResourceManager.Network;
using Azure.ResourceManager.Network.Models;
using azure_proto_core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace azure_proto_network
{

    public class NetworkInterfaceOperations : ResourceClientBase<PhNetworkInterface>
    {
        public NetworkInterfaceOperations(ArmClientBase parent, ResourceIdentifier context) : base(parent, context)
        {
        }

        public NetworkInterfaceOperations(ArmClientBase parent, azure_proto_core.Resource context) : base(parent, context)
        {
        }

        protected override ResourceType ResourceType => "Microsoft.Network/networkInterfaces";


        public override ArmOperation<Response> Delete()
        {
            return new ArmVoidOperation(Operations.StartDelete(Context.ResourceGroup, Context.Name));
        }

        public async override Task<ArmOperation<Response>> DeleteAsync(CancellationToken cancellationToken = default)
        {
            return new ArmVoidOperation(await Operations.StartDeleteAsync(Context.ResourceGroup, Context.Name, cancellationToken));
        }

        public override Response<ResourceClientBase<PhNetworkInterface>> Get()
        {
            return new PhArmResponse<ResourceClientBase<PhNetworkInterface>, NetworkInterface>(
                Operations.Get(Context.ResourceGroup, Context.Name),
                n => { Resource = new PhNetworkInterface(n); return this; });
        }

        public async override Task<Response<ResourceClientBase<PhNetworkInterface>>> GetAsync(CancellationToken cancellationToken = default)
        {
            return new PhArmResponse<ResourceClientBase<PhNetworkInterface>, NetworkInterface>(
                await Operations.GetAsync(Context.ResourceGroup, Context.Name, null, cancellationToken),
                n => { Resource = new PhNetworkInterface(n); return this; });
        }

        public override ArmOperation<ResourceClientBase<PhNetworkInterface>> AddTag(string key, string value)
        {
            var patchable = new TagsObject();
            patchable.Tags[key] = value;
            return new PhArmOperation<ResourceClientBase<PhNetworkInterface>, NetworkInterface>(Operations.UpdateTags(Context.ResourceGroup, Context.Name, patchable),
                n => { Resource = new PhNetworkInterface(n); return this; });
        }

        public async override Task<ArmOperation<ResourceClientBase<PhNetworkInterface>>> AddTagAsync(string key, string value, CancellationToken cancellationToken = default)
        {
            var patchable = new TagsObject();
            patchable.Tags[key] = value;
            return new PhArmOperation<ResourceClientBase<PhNetworkInterface>, NetworkInterface>(
                await Operations.UpdateTagsAsync(Context.ResourceGroup, Context.Name, patchable, cancellationToken), 
                n => { Resource = new PhNetworkInterface(n); return this; });
        }

        internal NetworkInterfacesOperations Operations => GetClient<NetworkManagementClient>((uri, cred) => new NetworkManagementClient(Context.Subscription, uri, cred)).NetworkInterfaces;

    }
}