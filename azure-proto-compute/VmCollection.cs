﻿using Azure.ResourceManager.Compute;
using Azure.ResourceManager.Compute.Models;
using azure_proto_core;
using System.Collections.Generic;

namespace azure_proto_compute
{
    public class VmCollection : AzureCollection<AzureVm>
    {
        public VmCollection(TrackedResource resourceGroup) : base(resourceGroup) { }

        private ComputeManagementClient Client => ClientFactory.Instance.GetComputeClient(Parent.Id.Subscription);

        public AzureVm CreateOrUpdateVm(string name, AzureVm vm)
        {
            var vmResult = Client.VirtualMachines.StartCreateOrUpdate(Parent.Name, name, vm.Model).WaitForCompletionAsync().Result;
            AzureVm avm = new AzureVm(Parent, new PhVirtualMachine(vmResult.Value));
            return avm;
        }

        public static AzureVm GetVm(string subscriptionId, string rgName, string vmName)
        {
            var client = ClientFactory.Instance.GetComputeClient(subscriptionId);
            var vmResult = client.VirtualMachines.Get(rgName, vmName);
            return new AzureVm(null, new PhVirtualMachine(vmResult.Value));
        }

        protected override AzureVm Get(string vmName)
        {
            var vmResult = Client.VirtualMachines.Get(Parent.Name, vmName);
            return new AzureVm(Parent, new PhVirtualMachine(vmResult.Value));
        }

        protected override IEnumerable<AzureVm> GetItems()
        {
            foreach (var vm in Client.VirtualMachines.List(Parent.Name))
            {
                yield return new AzureVm(Parent, new PhVirtualMachine(vm));
            }
        }

        public IEnumerable<AzureVm> GetItemsByTag(string key, string value)
        {
            foreach(var vm in Client.VirtualMachines.List(Parent.Name))
            {
                string rValue;
                if (vm.Tags != null && vm.Tags.TryGetValue(key, out rValue) && rValue == value)
                    yield return new AzureVm(Parent, new PhVirtualMachine(vm));
            }
        }
    }
}
